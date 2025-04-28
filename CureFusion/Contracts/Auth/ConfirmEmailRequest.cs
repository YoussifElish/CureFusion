namespace CureFusion.Contracts.Authentication
{
    public record ConfirmEmailRequest(
        string Email,
        String Code
        );
}
