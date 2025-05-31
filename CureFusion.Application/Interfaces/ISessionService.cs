using CureFusion.Domain.Abstactions;
using CureFusion.Domain.Entities;

namespace CureFusion.Application.Services;

public interface ISessionService
{
    Task<UserSession?> IsSessionValidAsync(string sessionToken, CancellationToken cancellationToken);
    Task<Result<List<UserSession>>> GetAllSessionsAsync(CancellationToken cancellationToken);
    Task<Result> TerminateSessionAsync(int sessionId, CancellationToken cancellationToken);
    Task<Result<int>> TerminateAllSessionsAsync(CancellationToken cancellationToken);
}

