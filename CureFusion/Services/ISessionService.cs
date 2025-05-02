namespace CureFusion.Services;

public interface ISessionService
{
    Task<UserSession?> IsSessionValidAsync(string sessionToken, CancellationToken cancellationToken);
    Task<Result<List<UserSession>>> GetAllSessionsAsync(CancellationToken cancellationToken);
    Task<Result> TerminateSessionAsync(string sessionToken, CancellationToken cancellationToken);
    Task<Result<int>> TerminateAllSessionsAsync(CancellationToken cancellationToken);
}

