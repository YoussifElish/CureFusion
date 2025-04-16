using CureFusion.Enums;

namespace CureFusion.Contracts.Appointment;

public record AppointmentResponse (
    int Id,
    string UserId,
    int DoctorId,
    DateTime AppointmentDate,
    AppointmentStatus Status,
    int DurationInMinutes,
    string Notes);

