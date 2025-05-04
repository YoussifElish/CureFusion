using CureFusion.Abstactions;
using CureFusion.Contracts.Answer;
using CureFusion.Entities;
using CureFusion.Errors;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CureFusion.Services;

// Inject UserManager to check user roles
public class AnswerService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : IAnswerService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<AnswerResponse>> AddAnswer(int QuestionId, AnswerRequest content,  CancellationToken cancellationToken)
    {
        var UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
        {
            return Result.Failure<AnswerResponse>(AuthErrors.NotFound);
        }

        // Check if the user is a Doctor
        var isDoctor = await _userManager.IsInRoleAsync(user, "Doctor");
        if (!isDoctor)
        {
            return Result.Failure<AnswerResponse>(AnswerErrors.Unauthorized);
        }

        var question = await _context.Questions.FindAsync(QuestionId, cancellationToken);
        if (question is null)
        {
            return Result.Failure<AnswerResponse>(QuestionError.QuestionNotFound);
        }

        // Check if the question has already been answered by a doctor
        if (!string.IsNullOrEmpty(question.RepliedByDoctorId))
        {
            return Result.Failure<AnswerResponse>(QuestionError.AlreadyAnswered);
        }

        var answer = content.Adapt<Answer>();
        answer.QuestionId = QuestionId;
        answer.UserId = UserId;
        // Initialize votes
        answer.Upvotes = 0;
        answer.Downvotes = 0;

        // Update the question to mark it as answered by this doctor
        question.RepliedByDoctorId = UserId;

        await _context.Answers.AddAsync(answer, cancellationToken);
        _context.Questions.Update(question); // Update the question entity

        await _context.SaveChangesAsync(cancellationToken);

        // Reload the answer with user details for the response
        var fullAnswer = await _context.Answers
            .Include(a => a.User)
            .Where(a => a.Id == answer.Id)
            .Select(a => new AnswerResponse(
                a.Id,
                a.QuestionId,
                a.Content,
                a.CreatedIn,
                a.UserId,
                a.User.FirstName + " " + a.User.LastName, // Assuming FirstName/LastName exist
                a.Upvotes,
                a.Downvotes
            ))
            .FirstOrDefaultAsync(cancellationToken);

        // It should exist, but check just in case
        if (fullAnswer == null)
        {
            // This case should ideally not happen after successful save
            return Result.Failure<AnswerResponse>(AnswerErrors.NotFound);
        }

        return Result.Success(fullAnswer);
    }

    public async Task<Result> DeleteAnswerAsync(int id, int questionId,  CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Find the answer first to check ownership
        var answer = await _context.Answers.FindAsync(new object[] { id }, cancellationToken);
        if (answer is null)
        {
            return Result.Failure(AnswerErrors.NotFound);
        }

        // Only the doctor who answered can delete it (or maybe an admin? Add admin check if needed)
        if (answer.UserId != userId)
        {
            return Result.Failure(AnswerErrors.Unauthorized);
        }

        var question = await _context.Questions.FindAsync(new object[] { questionId }, cancellationToken);
        if (question is null)
        {
            // Should not happen if answer exists, but check anyway
            return Result.Failure(QuestionError.QuestionNotFound);
        }

        // Reset the RepliedByDoctorId on the question
        if (question.RepliedByDoctorId == answer.UserId)
        {
            question.RepliedByDoctorId = null;
            _context.Questions.Update(question);
        }

        _context.Answers.Remove(answer);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<AnswerResponse>>> GetAllAnswers(int QuestionId, CancellationToken cancellationToken)
    {
        var questionExists = await _context.Questions.AnyAsync(q => q.Id == QuestionId, cancellationToken);
        if (!questionExists)
        {
            return Result.Failure<IEnumerable<AnswerResponse>>(QuestionError.QuestionNotFound);
        }

        var answers = _context.Answers
             .Include(a => a.User)
             .Where(a => a.QuestionId == QuestionId)
             .Select(a => new AnswerResponse(
                 a.Id,
                 a.QuestionId,
                 a.Content,
                 a.CreatedIn,
                 a.UserId,
                 a.User.FirstName + " " + a.User.LastName,
                 a.Upvotes,
                 a.Downvotes
             ));

        var response = await answers.AsNoTracking().ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<AnswerResponse>>(response);
    }

    public async Task<Result> UpdateAnswerAsync(int id, int questionId, AnswerRequest content, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var answer = await _context.Answers.FindAsync(new object[] { id }, cancellationToken);
        if (answer is null)
        {
            return Result.Failure(AnswerErrors.NotFound);
        }

        // Check if the question exists (optional, but good practice)
        var questionExists = await _context.Questions.AnyAsync(q => q.Id == questionId, cancellationToken);
        if (!questionExists)
        {
            return Result.Failure(QuestionError.QuestionNotFound);
        }

        // Only the doctor who wrote the answer can update it
        if (answer.UserId != userId)
        {
            return Result.Failure(AnswerErrors.Unauthorized);
        }

        // Update content
        answer.Content = content.Content;
        // Note: Should votes be updatable here? Probably not.

        _context.Answers.Update(answer);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    public async Task<Result> UpvoteAnswerAsync(int answerId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var answer = await _context.Answers.FindAsync(answerId, cancellationToken);
        if (answer is null)
        {
            return Result.Failure(AnswerErrors.NotFound);


        }

        answer.Upvotes++;
        _context.Answers.Update(answer);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }


    public async Task<Result> DownvoteDAnswerAsync(int answerId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var answer = await _context.Answers.FindAsync(answerId, cancellationToken);
        if (answer is null)
        {
            return Result.Failure(AnswerErrors.NotFound);


        }

        answer.Downvotes++;
        _context.Answers.Update(answer);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
