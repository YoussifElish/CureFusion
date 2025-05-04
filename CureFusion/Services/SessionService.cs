using Microsoft.EntityFrameworkCore;

namespace CureFusion.Services;

public class SessionService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : ISessionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
    public async Task<UserSession?> IsSessionValidAsync(string sessionToken, CancellationToken cancellationToken)
    {
        return await _context.UserSessions
       .FirstOrDefaultAsync(s =>
           s.SessionToken == sessionToken &&
           s.IsActive &&
           s.ExpiryAt > DateTime.UtcNow, cancellationToken);
    }

    public async Task<Result<List<UserSession>>> GetAllSessionsAsync(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Result.Failure<List<UserSession>>(AuthErrors.InavlidUser);

        var sessions = await _context.UserSessions
            .Where(s => s.UserId == userId && s.IsActive)
            .ToListAsync(cancellationToken);

        return Result.Success(sessions);
    }

    public async Task<Result> TerminateSessionAsync(int sessionId, CancellationToken cancellationToken)
    {
        var session = await _context.UserSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);

        if (session == null)
            return Result.Failure(AuthErrors.InvalidSession);

        var userId = GetCurrentUserId();
        if (userId == null || session.UserId != userId)
            return Result.Failure(AuthErrors.InavlidUser);

        session.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<int>> TerminateAllSessionsAsync(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Result.Failure<int>(AuthErrors.InavlidUser);

        var sessions = await _context.UserSessions
            .Where(s => s.UserId == userId && s.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var session in sessions)
        {
            session.IsActive = false;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(sessions.Count);
    }
}
