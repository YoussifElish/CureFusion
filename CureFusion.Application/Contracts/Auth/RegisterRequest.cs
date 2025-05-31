namespace CureFusion.Application.Contracts.Auth
{
    public record RegisterRequest(
        string Email,
        string Password,
        string ConfirmPassword,
        string FirstName,
        string LastName,
        string PhoneNumber,
        DateOnly DOB
        );
}
