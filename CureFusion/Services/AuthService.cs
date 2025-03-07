namespace CureFusion.Services;

public class AuthService(UserManager<ApplicationUser> userManager) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<AuthResponse?> GetTokenAsync(string Email, string Password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(Email);

        if (user is null)
            return null;
        var isvalidpassword=await _userManager.CheckPasswordAsync(user, Password);
        if (!isvalidpassword) return null;


        return new AuthResponse(user.Id, Email, user.FirstName, user.LastName, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
            3600);

         


    }
}
