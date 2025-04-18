using CureFusion.Abstactions;

namespace CureFusion.Errors;

public static class AuthErrors
{
    public static readonly Error NotAuthorized = new("AuthError.invalidCredentials","invalud username or password", StatusCodes.Status401Unauthorized);

}
