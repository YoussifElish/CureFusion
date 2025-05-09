namespace CureFusion.Contracts.Auth;

public record UpdateUserRequest
(
    string FirstName,
    string LastName,
    string Email,
    IList<string> Roles
    );

