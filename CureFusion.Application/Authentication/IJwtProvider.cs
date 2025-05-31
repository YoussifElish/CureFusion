using CureFusion.Domain.Entities;

namespace CureFusion.Application.Authentication;

public interface IJwtProvider
{
    (string token, int expiresin) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
    public string? ValidateToken(string token);
}

