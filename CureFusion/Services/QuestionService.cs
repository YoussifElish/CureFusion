using System.Linq.Dynamic.Core;
using CureFusion.Application.Services;

namespace CureFusion.API.Services;

public class QuestionService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<QuestionResponse>> CreateQuestion(QuestionRequest questionRequest, CancellationToken cancellationToken)
    {
        var userid = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var question = questionRequest.Adapt<Question>();
        question.UserId = userid;
        // Initialize votes
        question.Upvotes = 0;
        question.Downvotes = 0;
        question.RepliedByDoctorId = null; // Ensure it's null initially

        await _context.Questions.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload question with user details for response
        var createdQuestion = await _context.Questions
            .Include(q => q.User)
            .Where(q => q.Id == question.Id)
            .Select(q => new QuestionResponse(
                q.Id,
                q.Content,
                q.CreatedIn,
                0, // Answers count is initially 0
                q.UserId,
                q.User.FirstName + " " + q.User.LastName,
                q.Upvotes,
                q.Downvotes,
                q.RepliedByDoctorId,
                null // No answers initially
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (createdQuestion == null)
        {
            // Should not happen
            return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);
        }

        return Result.Success(createdQuestion);
    }

    public async Task<Result> DeleteQuestionAsync(int id, CancellationToken cancellationToken)
    {

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var question = await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        if (question is null)
        {
            return Result.Failure(QuestionError.QuestionNotFound);
        }

        // Check ownership (or admin role if needed)
        if (question.UserId != userId)
        {
            // TODO: Add Admin check if admins can delete any question
            return Result.Failure(CommonErrors.Unauthorized);
        }

        // Remove associated answers first if cascading delete is not configured
        _context.Answers.RemoveRange(question.Answers);
        _context.Questions.Remove(question);

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<PageinatedList<QuestionResponse>>> GetAllQuestion(RequestFilter filter, CancellationToken cancellationToken)
    {
        var questionsQuery = _context.Questions
            .Include(q => q.User)
            .Include(q => q.Answers)
                .ThenInclude(a => a.User)
            .Include(q => q.RepliedByDoctor) // Include the doctor who replied
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(filter.SearchValue))
        {
            questionsQuery = questionsQuery.Where(q => q.Content.Contains(filter.SearchValue) || (q.User.FirstName + " " + q.User.LastName).Contains(filter.SearchValue));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(filter.SortColumn))
        {
            // Basic sorting implementation, can be expanded
            var sortColumn = filter.SortColumn switch
            {
                "CreatedIn" => "CreatedIn",
                "Upvotes" => "Upvotes",
                // Add more sortable columns if needed
                _ => "CreatedIn" // Default sort
            };
            questionsQuery = questionsQuery.OrderBy($"{sortColumn} {filter.SortDirection ?? "desc"}");
        }
        else
        {
            questionsQuery = questionsQuery.OrderByDescending(q => q.CreatedIn); // Default sort
        }

        var source = questionsQuery.Select(q => new QuestionResponse(
            q.Id,
            q.Content,
            q.CreatedIn,
            q.Answers.Count(),
            q.UserId,
            q.User.FirstName + " " + q.User.LastName,
            q.Upvotes, // Use new property
            q.Downvotes, // Use new property
            q.RepliedByDoctorId,
            q.Answers.Select(a => new AnswerResponse(
                a.Id,
                a.QuestionId,
                a.Content,
                a.CreatedIn,
                a.UserId,
                a.User.FirstName + " " + a.User.LastName, // Assuming FirstName/LastName exist
                a.Upvotes, // Use new property
                a.Downvotes // Use new property
            )).ToList() // Materialize answers here
        )).AsNoTracking();

        var response = await PageinatedList<QuestionResponse>.CreateAsync(
            source,
            filter.PageNumber,
            filter.PageSize,
            cancellationToken
        );
        return Result.Success(response);
    }

    public async Task<Result<QuestionResponse>> GetQuestion(int id, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .Include(q => q.User)
            .Include(q => q.Answers)
                .ThenInclude(a => a.User)
            .Include(q => q.RepliedByDoctor)
            .Where(q => q.Id == id)
            .Select(q => new QuestionResponse(
                q.Id,
                q.Content,
                q.CreatedIn,
                q.Answers.Count(),
                q.UserId,
                q.User.FirstName + " " + q.User.LastName,
                q.Upvotes,
                q.Downvotes,
                q.RepliedByDoctorId,
                q.Answers.Select(a => new AnswerResponse(
                    a.Id,
                    a.QuestionId,
                    a.Content,
                    a.CreatedIn,
                    a.UserId,
                    a.User.FirstName + " " + a.User.LastName,
                    a.Upvotes,
                    a.Downvotes
                )).ToList()
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return question is not null ? Result.Success(question)
            : Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);
    }

    public async Task<Result> UpdateQuestion(int id, QuestionRequest questionRequest, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var question = await _context.Questions.FindAsync(id, cancellationToken);
        if (question is null)
        {
            return Result.Failure(QuestionError.QuestionNotFound);
        }

        // Check ownership
        if (question.UserId != userId)
        {
            return Result.Failure(CommonErrors.Unauthorized);
        }

        // Only allow updating content, not votes or answered status
        question.Content = questionRequest.Content;
        _context.Questions.Update(question);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    // --- Voting Methods ---

    public async Task<Result> UpvoteQuestionAsync(int questionId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var question = await _context.Questions.FindAsync(questionId, cancellationToken);
        if (question is null)
        {
            return Result.Failure(QuestionError.QuestionNotFound);
        }

        // TODO: Implement logic to prevent multiple upvotes from the same user
        // For now, just increment the count.
        question.Upvotes++;
        _context.Questions.Update(question);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DownvoteQuestionAsync(int questionId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var question = await _context.Questions.FindAsync(questionId, cancellationToken);
        if (question is null)
        {
            return Result.Failure(QuestionError.QuestionNotFound);
        }

        // TODO: Implement logic to prevent multiple downvotes from the same user
        // For now, just increment the count.
        question.Downvotes++;
        _context.Questions.Update(question);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
