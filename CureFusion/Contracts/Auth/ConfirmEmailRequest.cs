namespace CureFusion.Contracts.Authentication
{
    public record ConfirmEmailRequest(
        string UserId,
        String Code
        );
}
