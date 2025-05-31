namespace CureFusion.Application.Contracts.Appointment;

public record PatientAppointmentRequest(
    int AppointmentId,
    string? Notes
);