namespace CureFusion.Services;

public interface ISessionService
{
    Task<UserSession?> IsSessionValidAsync(string sessionToken, CancellationToken cancellationToken);
}

