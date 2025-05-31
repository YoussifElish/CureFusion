namespace CureFusion.Domain.Entities;

public class UserSession
{
    public int Id { get; set; }
    public string SessionToken { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime LastActivity { get; set; }
    public DateTime ExpiryAt { get; set; }
    public bool IsActive { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
