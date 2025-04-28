using CureFusion.Contracts.Authentication;
using CureFusion.Contracts.Files;
using CureFusion.Services;
using Microsoft.AspNetCore.Identity.Data;
using SurveyBasket.Abstactions;

namespace CureFusion.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IOptions<JwtOptions> JwtOptions) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly JwtOptions _jwtOptions = JwtOptions.Value;

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(Loginrequest request, CancellationToken     cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken CT)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, CT);


        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }

    [HttpPost("register")]

    public async Task<IActionResult> Register([FromBody] Contracts.Auth.RegisterRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.RegisterAsync(request, cancellationToken);
        return authResult.IsSuccess ? Ok() : authResult.ToProblem();

    }
    [HttpPost("register-as-doctor")]

    public async Task<IActionResult> RegisterAsDoctor([FromForm] RegisterAsDoctorRequest request, [FromForm]  RegisterDoctorImageRequest imageRequest, CancellationToken cancellationToken)
    {
        var authResult = await _authService.RegisterDoctorAsync(request, imageRequest, cancellationToken);
        return authResult.IsSuccess ? Ok() : authResult.ToProblem();

    }

    [HttpPut("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        return isRevoked.IsSuccess ? Ok() : isRevoked.ToProblem();

    }
  

 
    [HttpPost("forget-password")]

    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var authResult = await _authService.SentResetPasswordCodeAsync(request.Email);
        return authResult.IsSuccess ? Ok() : authResult.ToProblem();

    }

    [HttpPost("reset-password")]

    public async Task<IActionResult> ResetPassword([FromBody] Contracts.Authentication.ResetPasswordRequest request)
    {
        var authResult = await _authService.ResetPasswordAsync(request);
        return authResult.IsSuccess ? Ok() : authResult.ToProblem();

    }


}
