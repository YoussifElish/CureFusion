namespace CureFusion.Entities;

public class ApplicationUser : IdentityUser 
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsDisabled { get; set; } = false;
    public DateOnly DOB { get; set; }

    public List<RefreshTokens> RefreshTokens { get; set; } = [];
    public Guid? ProfileImageId { get; set; }
    public UploadedFile? ProfileImage { get; set; }
    public string? ResetPasswordCode { get; set; }
    public DateTime? ResetPasswordCodeExpiration { get; set; }

    public string? EmailConfirmationCode { get; set; }
    public DateTime? EmailConfirmationCodeExpiration { get; set; }
    public ICollection<Question> Questions { get; set; }
    public ICollection<Answer> Answers { get; set; }

}
