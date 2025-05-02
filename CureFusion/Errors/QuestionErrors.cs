using CureFusion.Abstactions;
using Error = CureFusion.Abstactions.Error;


namespace CureFusion.Errors;

public static class QuestionError
{
    public static readonly Error QuestionNotFound = new("QuestionError.QuestionNotFound", "Question not found", StatusCodes.Status404NotFound);
    //public static readonly Error QuestionAlreadyExists = new("QuestionError.QuestionAlreadyExists", "Question already exists", StatusCodes.Status406NotAcceptable);
    public static readonly Error InvalidCredentials = new("QuestionError.InvalidCredentials", "Invalid credentials", StatusCodes.Status401Unauthorized);
}
