namespace CureFusion.Entities;

public class SubscriptionPlan
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // e.g., "basic", "premium", "family"
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public int ConsultationsPerPeriod { get; set; }
    public bool IncludesPriorityBooking { get; set; }
    public bool IncludesSpecialistAccess { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; }
}