namespace CureFusion.Authentication;

public class JwtProvider(IOptions<JwtOptions> JwtOptions) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = JwtOptions.Value;

    public (string token, int expiresin) GenerateToken(ApplicationUser user)
    {
        Claim[] claims =
            [
            new (JwtRegisteredClaimNames.Sub,user.Id),
            new (JwtRegisteredClaimNames.Email,user.Email!),
            new (JwtRegisteredClaimNames.GivenName,user.FirstName),
            new (JwtRegisteredClaimNames.FamilyName,user.LastName),
            new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),


            ];

        var symmetricsecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var signingcredetials = new SigningCredentials(symmetricsecuritykey, SecurityAlgorithms.HmacSha256);


        var expirationdate = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes);

        var token = new JwtSecurityToken
            (
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expirationdate,
            signingCredentials: signingcredetials


            );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresin: _jwtOptions.ExpiryMinutes * 60);
    }
    

    public string? ValidateToken(string token)
    {
        var TokenHandler = new JwtSecurityTokenHandler();

        var symmetricsecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        try
        {
            TokenHandler.ValidateToken(token, new TokenValidationParameters
            {

                IssuerSigningKey= symmetricsecuritykey,
                ValidateIssuerSigningKey=true,
                ValidateIssuer=false,
                ValidateAudience=false,
                ClockSkew=TimeSpan.Zero,


            }, out SecurityToken validatedToken);

         var JwtToken=(JwtSecurityToken)validatedToken;
            return JwtToken.Claims.First(x=>x.Type==JwtRegisteredClaimNames.Sub).Value;

        }
        catch 
        {

            return null;
        }
    }
}
