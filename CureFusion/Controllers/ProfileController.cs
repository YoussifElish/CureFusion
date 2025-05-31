using CureFusion.Application.Services;

namespace CureFusion.API.Controllers
{
    [Route("me")]
    [ApiController]
    public class ProfileController(IProfileService profileService) : ControllerBase
    {
        private readonly IProfileService _profileService = profileService;


        [HttpGet]
        [Route("GetProfile")]
        [Authorize]
        public async Task<IActionResult> GetProfileAsync(CancellationToken cancellationToken)
        {
            var result = await _profileService.GetProfileAsync(cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost]
        [Route("ChangeEmail")]
        [Authorize]

        public async Task<IActionResult> ChangeEmailAsync([FromQuery] string newEmail, CancellationToken cancellationToken)
        {
            var result = await _profileService.ChangeEmailAsync(newEmail, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]

        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest changePasswordRequest, CancellationToken cancellationToken)
        {
            var result = await _profileService.ChangePasswordAsync(changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPut]
        [Route("UpdateSessionExpiry")]
        [Authorize]

        public async Task<IActionResult> UpdateSessionExpiry([FromQuery] int expiryMinutes, CancellationToken cancellationToken)
        {
            var result = await _profileService.UpdateSessionExpiryAsync(expiryMinutes, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }


        [HttpPost]
        [Route("UpdateProfileImage")]
        [Authorize]

        public async Task<IActionResult> UpdateProfileImageAsync([FromForm] UploadImageRequest uploadImageRequest, CancellationToken cancellationToken)
        {
            var result = await _profileService.UpdateProfileImageAsync(uploadImageRequest.Image, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();

        }
    }
}

