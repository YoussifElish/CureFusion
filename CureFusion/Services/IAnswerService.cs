using CureFusion.Abstactions;
using CureFusion.Contracts.Answer;

namespace CureFusion.Services;

public interface IAnswerService
{
    Task<Result<IEnumerable<AnswerResponse>>> GetAllAnswers(int QuestionId, CancellationToken cancellationToken);
    //Task<Result<AnswerResponse>> GetAnswers(int QuestionId, int id,CancellationToken cancellationToken); what is the point of the method????
    Task<Result<AnswerResponse>> AddAnswer(int QuestionId, AnswerRequest content,string UserId, CancellationToken cancellationToken);
}
