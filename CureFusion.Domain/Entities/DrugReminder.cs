namespace CureFusion.Domain.Entities;

public class DrugReminder
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public Drug Drug { get; set; }
    public int DrugId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public DateTime LastReminderTime { get; set; }
    public int RepeatIntervalMinutes { get; set; }
}
