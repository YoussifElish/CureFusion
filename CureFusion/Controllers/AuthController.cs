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
        var AuthResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken); 
        return AuthResult is null? BadRequest("invalid email/password"):Ok(AuthResult);
    }
}
