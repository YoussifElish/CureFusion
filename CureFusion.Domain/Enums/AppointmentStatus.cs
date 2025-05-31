namespace CureFusion.Domain.Enums;

public enum AppointmentStatus
{
    Pending,   // booked but waiting payment
    Confirmed,
    Canceled,
    Completed,
    NotReversed,
    PaymentFailed
}
