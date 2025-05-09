using CureFusion.Helpers;

namespace CureFusion.Services;

public interface IUserService
{
    Task<Result<PageinatedList<UserResponse>>> GetAllUsersAsync(UserQueryParameters userQuery,CancellationToken cancellationToken);
    Task<Result<UserResponse>>GetUsersAsync(string Id,CancellationToken cancellationToken);
    Task<Result<UserResponse>> AddAsync(UserRequest request, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken);
    Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken);
    Task<Result> UnlockUserAsync(string id, CancellationToken cancellationToken);

}
