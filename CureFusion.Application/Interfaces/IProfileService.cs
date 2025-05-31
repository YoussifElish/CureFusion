using CureFusion.Application.Contracts.Profile;
using CureFusion.Domain.Abstactions;
using Microsoft.AspNetCore.Http;

namespace CureFusion.Application.Services;

public interface IProfileService
{
    Task<Result<IProfileDto>> GetProfileAsync(CancellationToken cancellationToken);
    Task<Result> ChangeEmailAsync(string newEmail, CancellationToken cancellationToken);
    Task<Result> ChangePasswordAsync(string currentPassword, string newPassword, CancellationToken cancellationToken);
    Task<Result> UpdateSessionExpiryAsync(int expiryMinutes, CancellationToken cancellationToken);
    Task<Result> UpdateProfileImageAsync(IFormFile imageFile, CancellationToken cancellationToken);


}
