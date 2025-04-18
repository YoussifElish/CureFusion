using SurveyBasket.Abstactions;

namespace CureFusion.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IOptions<JwtOptions> JwtOptions) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly JwtOptions _jwtOptions = JwtOptions.Value;

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(Loginrequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken); 

        return authResult.IsSuccess? Ok(authResult.Value): authResult.ToProblem();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken CT)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, CT);


        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }
}
