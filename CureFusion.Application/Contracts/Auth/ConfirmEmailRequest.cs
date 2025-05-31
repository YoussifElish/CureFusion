namespace CureFusion.Application.Contracts.Authentication
{
    public record ConfirmEmailRequest(
        string Email,
        String Code
        );
}
