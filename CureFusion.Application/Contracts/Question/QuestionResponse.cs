using CureFusion.Application.Contracts.Answer;

namespace CureFusion.Application.Contracts.Question;

public record QuestionResponse
(
     int Id,
     string Content,
     DateTime CreatedIn,
     int AnswerCount,
     string UserId,
     string UserName,
     int Upvotes,
     int Downvotes,
     string? RepliedByDoctorId,
    IEnumerable<AnswerResponse> Answers
);
