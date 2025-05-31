using CureFusion.Domain.Abstactions;
using Microsoft.AspNetCore.Http;



namespace CureFusion.Domain.Errors;

public static class QuestionError
{
    public static readonly Error QuestionNotFound = new("QuestionError.QuestionNotFound", "Question not found", StatusCodes.Status404NotFound);
    //public static readonly Error QuestionAlreadyExists = new("QuestionError.QuestionAlreadyExists", "Question already exists", StatusCodes.Status406NotAcceptable);
    public static readonly Error InvalidCredentials = new("QuestionError.InvalidCredentials", "Invalid credentials", StatusCodes.Status401Unauthorized);
    public static readonly Error AlreadyAnswered = new("QuestionError.AlreadyAnswered", "This question has already been answered by a doctor.", StatusCodes.Status401Unauthorized);
}
