using CureFusion.Enums;

namespace CureFusion.Contracts.Answer;

public record AnswerResponse
(
    int Id,
    int QuestionId,
     string Content,
     DateTime CreatedIn,
     string UserId,
     string UserName,
    int Upvotes,
    int Downvotes
 );
