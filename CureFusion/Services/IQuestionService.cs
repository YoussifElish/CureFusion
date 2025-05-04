using CureFusion.Abstactions;
using CureFusion.Contracts.Common;
using CureFusion.Contracts.Medicine;
using CureFusion.Contracts.Question;

namespace CureFusion.Services;

public interface IQuestionService
{
   Task<Result<PageinatedList<QuestionResponse>>> GetAllQuestion(RequestFilter filter,CancellationToken cancellationToken);
    Task<Result<QuestionResponse>> GetQuestion(int id, CancellationToken cancellationToken);
    Task<Result<QuestionResponse>> CreateQuestion(QuestionRequest questionRequest, CancellationToken cancellationToken);
    Task<Result> UpdateQuestion(int id, QuestionRequest questionRequest, CancellationToken cancellationToken);
     Task<Result> DeleteQuestionAsync(int id, CancellationToken cancellationToken);
    Task<Result> DownvoteQuestionAsync(int questionId, CancellationToken cancellationToken);
    Task<Result> UpvoteQuestionAsync(int questionId, CancellationToken cancellationToken);
}
