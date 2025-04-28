using CureFusion.Abstactions;
using CureFusion.Contracts.Authentication;
using CureFusion.Contracts.Files;

namespace CureFusion.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string Email,string Password,CancellationToken cancellationToken=default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string Token,string RefreshToken,CancellationToken cancellationToken=default);
    Task<Result> RegisterAsync(Contracts.Auth.RegisterRequest request, CancellationToken cancellationToken);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
    Task<Result> RegisterDoctorAsync(RegisterAsDoctorRequest request, RegisterDoctorImageRequest imageRequest, CancellationToken cancellationToken);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ResendConfirmEmailAsync(Contracts.Authentication.ResendConfirmationEmailRequest request);
    Task<Result> SentResetPasswordCodeAsync(string email);
    Task<Result> ResetPasswordAsync(Contracts.Authentication.ResetPasswordRequest request);



}
