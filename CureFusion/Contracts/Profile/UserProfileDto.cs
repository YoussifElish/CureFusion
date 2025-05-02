namespace CureFusion.Contracts.Profile;

public class UserProfileDto : IProfileDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateOnly DOB { get; set; }
    public string? ProfileImageUrl { get; set; }
    public int QuestionsCount { get; set; }
    public int UpcomingAppointments { get; set; }
}
