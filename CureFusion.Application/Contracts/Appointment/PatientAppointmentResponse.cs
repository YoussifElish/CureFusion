namespace CureFusion.Application.Contracts.Appointment;

public record PatientAppointmentResponse(
    int Id,
    int AppointmentId,
    string UserId,
    DateTime BookedAt,
    string Status,
    string? Notes,
    string? PaymentUrl
);