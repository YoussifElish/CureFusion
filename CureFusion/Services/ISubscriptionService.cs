namespace CureFusion.Services;

public interface ISubscriptionService
{
    Task<Subscription> GetActiveSubscription(int patientId);
    Task<Subscription> Subscribe(int patientId, string planType, string paymentMethod, string transactionId);
    Task<bool> CancelSubscription(int subscriptionId);
    Task<bool> IsConsultationCovered(int patientId);
    Task<int> GetRemainingConsultations(int patientId);
    Task<bool> DeductConsultation(int patientId);
    Task<IEnumerable<SubscriptionPlan>> GetAvailablePlans();
}
