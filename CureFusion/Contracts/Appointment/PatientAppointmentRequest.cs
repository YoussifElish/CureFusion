namespace CureFusion.Contracts.Appointment;

public record PatientAppointmentRequest(
    int AppointmentId,
    string? Notes
);