using CureFusion.Domain.Abstactions;
using Microsoft.AspNetCore.Http;

namespace CureFusion.Domain.Errors;


public static class SessionErrors
{
    public static readonly Error Invalid = new(
        "SessionErrors.Invalid",
        "Session expiry must be between 10 minutes and 30 days",
        StatusCodes.Status404NotFound);

}