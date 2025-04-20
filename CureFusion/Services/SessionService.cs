using Microsoft.EntityFrameworkCore;

namespace CureFusion.Services;

public class SessionService(ApplicationDbContext context) : ISessionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<UserSession?> IsSessionValidAsync(string sessionToken, CancellationToken cancellationToken)
    {
        return await _context.UserSessions
       .FirstOrDefaultAsync(s =>
           s.SessionToken == sessionToken &&
           s.IsActive &&
           s.ExpiryAt > DateTime.UtcNow, cancellationToken);
    }
}
