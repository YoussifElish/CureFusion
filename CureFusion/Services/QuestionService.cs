using System.Linq.Dynamic.Core;
using CureFusion.Abstactions;
using CureFusion.Contracts.Answer;
using CureFusion.Contracts.Common;
using CureFusion.Contracts.Medicine;
using CureFusion.Contracts.Question;
using CureFusion.Errors;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CureFusion.Services;

public class QuestionService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<QuestionResponse>> CreateQuestion(QuestionRequest questionRequest, string userid, CancellationToken cancellationToken)
    {   
        var question = questionRequest.Adapt<Question>();
        question.UserId = userid;


        await  _context.AddAsync(question, cancellationToken);
        await  _context.SaveChangesAsync(cancellationToken);
        var response = question.Adapt<QuestionResponse>();

        return Result.Success(response);
    }

    public async Task<Result> DeleteQuestionAsync(int id, CancellationToken cancellationToken)
    {
      var question=await _context.Questions.FindAsync(id, cancellationToken);
        if (question is null)
        {
            return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);
        }
        _context.Remove(question);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<PageinatedList<QuestionResponse>>> GetAllQuestion(RequestFilter filter, CancellationToken cancellationToken)
    {
        var questions = _context.Questions
            .Where(x => string.IsNullOrEmpty(filter.SearchValue) || x.Content.Contains(filter.SearchValue));
        if (!string.IsNullOrEmpty(filter.SortColumn))
        {
            questions = questions.OrderBy($"{filter.SortColumn} {filter.SortDirection}");
        } 
        var source = questions.Include(q => q.User)
    .Include(q => q.Answers)
        .ThenInclude(a => a.User)
      .Select(q => new QuestionResponse(
          q.Id,
          q.Content,
          q.CreatedIn,
          q.Answers.Count(),
          q.UserId,
          q.User.FirstName + " " + q.User.LastName, 
          q.React,
          q.Answers.Select(a => new AnswerResponse(
              a.Id,
              a.QuestionId,
              a.Content,
              a.CreatedIn,
              a.UserId,
              a.User.UserName!,
              a.React

          ))
      ))
      .AsNoTracking();
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
        var QuestionIsExsist = await _context.Questions
        .Include(q => q.User) 
        .Include(q => q.Answers)
            .ThenInclude(a => a.User)
            

        .FirstOrDefaultAsync(q=>q.Id==id, cancellationToken);

        return QuestionIsExsist is not null ? Result.Success(QuestionIsExsist.Adapt<QuestionResponse>()) 
            : Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);
    }

    public async Task<Result> UpdateQuestion(int id, QuestionRequest questionRequest, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.FindAsync(id, cancellationToken);
        if (question is null)
        {
            return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);
        }
        question.Content = questionRequest.Content;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }
}


