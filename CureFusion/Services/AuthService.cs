using System.Security.Cryptography;
using CureFusion.Entities;
using Microsoft.AspNetCore.Identity;

namespace CureFusion.Services;

public class AuthService(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    private readonly int _refreshtokenexpirydate = 14;

    public async Task<AuthResponse?> GetTokenAsync(string Email, string Password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(Email);

        if (user is null)

            return null;

        var isvalidpassword=await _userManager.CheckPasswordAsync(user, Password);

        if (!isvalidpassword) return null;

        var (token, ExpiresIn) = _jwtProvider.GenerateToken(user);

        var RefreshToken =GenerateRefreshToken();

        var RefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshtokenexpirydate);

        user.RefreshTokens.Add(new RefreshTokens
        {
            Token= RefreshToken,
            ExpiresOn= RefreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);
        return new AuthResponse(user.Id, Email, user.FirstName, user.LastName,token,ExpiresIn, RefreshToken,
            RefreshTokenExpiration);


    }
  

    public async Task<AuthResponse?> GetRefreshTokenAsync(string Token, string RefreshToken, CancellationToken cancellationToken = default)
    {
        var UserId = _jwtProvider.ValidateToken(Token);
        if (UserId is null)
            return null;


        var user = await _userManager.FindByIdAsync(UserId);
        if (user is null)
            return null;



        var UserRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == RefreshToken && x.IsActive);
        if (UserRefreshToken is null)
            return null;


        UserRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newtoken, ExpiresIn) = _jwtProvider.GenerateToken(user);
        var newRefreshtoken = GenerateRefreshToken();
        var RefreshtokenExpiration = DateTime.UtcNow.AddDays(_refreshtokenexpirydate);
        user.RefreshTokens.Add(new RefreshTokens
        {
            Token = newRefreshtoken,
            ExpiresOn = RefreshtokenExpiration
        });
        await _userManager.UpdateAsync(user);

       return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newtoken, ExpiresIn, newRefreshtoken, RefreshtokenExpiration);

    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
