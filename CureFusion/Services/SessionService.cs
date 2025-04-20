using Microsoft.EntityFrameworkCore;

namespace CureFusion.Services;

public class SessionService(ApplicationDbContext context) : ISessionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> IsSessionValidAsync(string sessionToken, CancellationToken cancellationToken)
    {
        var session = await _context.UserSessions
            .FirstOrDefaultAsync(s => s.SessionToken == sessionToken && s.IsActive == true, cancellationToken);

        if (session == null || session.ExpiryAt < DateTime.UtcNow)
            return false;

        return true;
    }
}
