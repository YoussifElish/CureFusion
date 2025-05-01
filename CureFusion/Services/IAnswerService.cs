
namespace CureFusion.Services;

public interface IAnswerService
{
    Task<Result<IEnumerable<AnswerResponse>>> GetAllAnswers(int QuestionId, CancellationToken cancellationToken);
    //Task<Result<AnswerResponse>> GetAnswers(int QuestionId, int id,CancellationToken cancellationToken); what is the point of the method????
    Task<Result<AnswerResponse>> AddAnswer(int QuestionId, AnswerRequest content,string UserId, CancellationToken cancellationToken);
    Task<Result> UpdateAnswerAsync(int id, int questionId,AnswerRequest content,string userId, CancellationToken cancellationToken);
    Task<Result> DeleteAnswerAsync(int id, int questionId,string userId, CancellationToken cancellationToken);
}
