using CureFusion.Abstactions;

namespace CureFusion.Errors;

public static class AuthErrors
{
    public static readonly Error NotAuthorized = new("AuthError.invalidCredentials", "invalud username or password", StatusCodes.Status401Unauthorized);
    public static readonly Error DuplicatedEmail = new("AuthError.DuplicatedEmail", "Another User With The Same Email Is Exist", StatusCodes.Status409Conflict);
    public static readonly Error InvalidCode = new("AuthError.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
    public static readonly Error DuplicatedConfirmation = new("AuthError.DuplicatedConfirmation", "Email already Confirmed", StatusCodes.Status400BadRequest);
    public static readonly Error InvalidToken = new("AuthError.InvalidToken", "Invalid Email Or Password", StatusCodes.Status400BadRequest);
    public static readonly Error InvalidSession = new("AuthError.InvalidSession", "Session not found or already terminated.", StatusCodes.Status400BadRequest);
    public static readonly Error Invalidroles = new("AuthError.InvalidRoles", "roles not found or already terminated.", StatusCodes.Status400BadRequest);

    public static readonly Error DisabledUser = new("AuthError.DisabledUser", "Disabled User Please Contacy Your Administrator", StatusCodes.Status409Conflict);
    public static readonly Error InavlidUser = new("AuthError.InvalidUser", "Invalid Email Or Password", StatusCodes.Status400BadRequest);
    public static readonly Error NotFound = new("AuthError.NotFound", "User Not Found", StatusCodes.Status404NotFound);
    public static readonly Error InavlidPassword = new("AuthError.InavlidPassword", "Invalid Password", StatusCodes.Status400BadRequest);
    public static readonly Error OperationFailed = new("AuthError.OperatinFailed", "Operation Failed", StatusCodes.Status400BadRequest);
    public static readonly Error EmailNotConfirmed = new("AuthError.EmailNotConfirmed", "Email Is Not Confirmed", StatusCodes.Status401Unauthorized);
    public static readonly Error LockedUser = new("AuthError.LockedUser", "Please Contact Your Administrator", StatusCodes.Status401Unauthorized);

}
