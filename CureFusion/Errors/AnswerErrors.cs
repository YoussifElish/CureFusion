using CureFusion.Abstactions;

namespace CureFusion.Errors;

public static class AnswerErrors
{
    public static readonly Error NotFound = new("AnswerErrors.NotFound", "Answer not found", StatusCodes.Status404NotFound);
    public static readonly Error AlreadyExists = new("AnswerErrors.AlreadyExists", "Answer already exists", StatusCodes.Status409Conflict);
    public static readonly Error InvalidContent = new("AnswerErrors.InvalidContent", "Invalid answer content", StatusCodes.Status400BadRequest);
    public static readonly Error Unauthorized = new("AnswerErrors.Unauthorized", "Unauthorized access to answer", StatusCodes.Status401Unauthorized);
}

