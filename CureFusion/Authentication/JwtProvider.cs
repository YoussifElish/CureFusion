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


        var expirationdate = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireyMinutes);

        var token = new JwtSecurityToken
            (
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expirationdate,
            signingCredentials: signingcredetials


            );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresin: _jwtOptions.ExpireyMinutes * 60);
    }
    

    public string? ValidateToken(string token)
    {
        throw new NotImplementedException();
    }
}
