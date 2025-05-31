
using CureFusion.Application.Contracts.Answer;
using CureFusion.Domain.Abstactions;

namespace CureFusion.Application.Services;

public interface IAnswerService
{
    Task<Result<IEnumerable<AnswerResponse>>> GetAllAnswers(int QuestionId, CancellationToken cancellationToken);
    //Task<Result<AnswerResponse>> GetAnswers(int QuestionId, int id,CancellationToken cancellationToken); what is the point of the method????
    Task<Result<AnswerResponse>> AddAnswer(int QuestionId, AnswerRequest content, CancellationToken cancellationToken);
    Task<Result> UpdateAnswerAsync(int id, int questionId, AnswerRequest content, CancellationToken cancellationToken);
    Task<Result> DeleteAnswerAsync(int id, int questionId, CancellationToken cancellationToken);
    Task<Result> DownvoteDAnswerAsync(int answerId, CancellationToken cancellationToken);
    Task<Result> UpvoteAnswerAsync(int answerId, CancellationToken cancellationToken);
}
