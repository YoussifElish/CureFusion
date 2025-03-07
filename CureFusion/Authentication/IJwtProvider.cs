namespace CureFusion.Authentication;

public interface IJwtProvider
{
    (string token, int expiresin) GenerateToken(ApplicationUser user);
    public string? ValidateToken(string token);
}

