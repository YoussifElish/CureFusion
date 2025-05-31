using CureFusion.Application.Contracts.Auth;
using CureFusion.Application.Contracts.Files;
using CureFusion.Domain.Abstactions;

namespace CureFusion.Application.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string Email, string Password, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string Token, string RefreshToken, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(Application.Contracts.Auth.RegisterRequest request, CancellationToken cancellationToken);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
    Task<Result> RegisterDoctorAsync(RegisterAsDoctorRequest request, RegisterDoctorImageRequest imageRequest, CancellationToken cancellationToken);

    Task<Result> SentResetPasswordCodeAsync(string email);
    Task<Result> ResetPasswordAsync(Application.Contracts.Authentication.ResetPasswordRequest request);



}
