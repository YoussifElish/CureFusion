
namespace CureFusion.Services;

public class AnswerService(ApplicationDbContext context) : IAnswerService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<AnswerResponse>> AddAnswer(int QuestionId, AnswerRequest content,string UserId, CancellationToken cancellationToken)
    {
       
        var question = await _context.Questions.FindAsync(QuestionId, cancellationToken);
        if (question is null)
        {
            return Result.Failure<AnswerResponse>(QuestionError.QuestionNotFound);
        }


        var answer = content.Adapt<Answer>();
        answer.QuestionId = QuestionId;
        answer.UserId = UserId;
        


        await _context.Answers.AddAsync(answer,cancellationToken);
       await  _context.SaveChangesAsync(cancellationToken);
        var fullAnswer = await _context.Answers
    .Include(a => a.User)
    .FirstOrDefaultAsync(a => a.Id == answer.Id, cancellationToken);
        var response = fullAnswer.Adapt<AnswerResponse>();
        return Result.Success(response);

    }

    public async Task<Result> DeleteAnswerAsync(int id, int questionId, string userId, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.FindAsync(questionId, cancellationToken);
        if (question is null)
        {
            return Result.Failure<AnswerResponse>(QuestionError.QuestionNotFound);
        }
        var answer = await _context.Answers.FindAsync(id, cancellationToken);
        if (answer is null)
        {
            return Result.Failure<AnswerResponse>(AnswerErrors.NotFound);
        }
        if (answer.UserId != userId)
        {
            return Result.Failure<AnswerResponse>(AnswerErrors.Unauthorized);
        }
        _context.Answers.Remove(answer);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public  async Task<Result<IEnumerable<AnswerResponse>>> GetAllAnswers(int QuestionId, CancellationToken cancellationToken)
    {

        var question = await _context.Questions.FindAsync(QuestionId, cancellationToken);
        if (question is null)
        {
            return Result.Failure<IEnumerable<AnswerResponse>>(QuestionError.QuestionNotFound);
        }
        var answers = _context.Answers

             .Include(q => q.User)
             .Where(x => x.QuestionId == QuestionId)
             .Select(q => new AnswerResponse(
                 q.Id,
                 q.QuestionId,
                 q.Content,
                 q.CreatedIn,
                 q.UserId,
                 q.User.FirstName + " " + q.User.LastName,
                 q.React
             ));
        var response = await answers.AsNoTracking().ToListAsync(cancellationToken);
        return response is not null ? Result.Success<IEnumerable<AnswerResponse>>(response) : Result.Failure<IEnumerable<AnswerResponse>>(QuestionError.QuestionNotFound);
    }

    public async Task<Result> UpdateAnswerAsync(int id, int questionId, AnswerRequest content, string userId, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.FindAsync(questionId, cancellationToken);
        if (question is null)
        {
            return Result.Failure<AnswerResponse>(QuestionError.QuestionNotFound);
        }
        var answer = await _context.Answers.FindAsync(id, cancellationToken);
        if (answer is null)
        {
            return Result.Failure<AnswerResponse>(AnswerErrors.NotFound);
        }

        if (answer.UserId != userId)
        {
            return Result.Failure<AnswerResponse>(AnswerErrors.Unauthorized);
        }
        answer.Content = content.Content;
        
        _context.Answers.Update(answer);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
