namespace CureFusion.Entities;

public class ApplicationUser : IdentityUser 
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsDisabled { get; set; } = false;
    public DateOnly DOB { get; set; }

    public List<RefreshTokens> RefreshTokens { get; set; } = [];

}
