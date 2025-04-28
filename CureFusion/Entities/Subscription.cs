namespace CureFusion.Entities;
public enum SubscriptionStatus
{
    Active,
    Expired,
    Cancelled
}
public class Subscription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int PlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public SubscriptionStatus Status { get; set; }
    public int RemainingConsultations { get; set; }
    public string PaymentMethod { get; set; }
    public string TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public SubscriptionPlan Plan { get; set; }
    public Patient Patient { get; set; }
}
