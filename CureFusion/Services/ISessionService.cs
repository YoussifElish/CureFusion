namespace CureFusion.Services;

public interface ISessionService
{
    Task<bool> IsSessionValidAsync(string sessionToken, CancellationToken cancellationToken);
}

