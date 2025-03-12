namespace CureFusion.Services;

public interface IAuthService
{
    Task<AuthResponse?> GetTokenAsync(string Email,string Password,CancellationToken cancellationToken=default);
    Task<AuthResponse?> GetRefreshTokenAsync(string Token,string RefreshToken,CancellationToken cancellationToken=default);
}
