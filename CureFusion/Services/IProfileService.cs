using CureFusion.Contracts.Profile;

namespace CureFusion.Services;

public interface IProfileService
{
    Task<Result<IProfileDto>> GetProfileAsync(CancellationToken cancellationToken);
    Task<Result> ChangeEmailAsync(string newEmail, CancellationToken cancellationToken);
    Task<Result> ChangePasswordAsync(string currentPassword, string newPassword, CancellationToken cancellationToken);
    Task<Result> UpdateSessionExpiryAsync(int expiryMinutes, CancellationToken cancellationToken);
    Task<Result> UpdateProfileImageAsync(IFormFile imageFile, CancellationToken cancellationToken);


}
