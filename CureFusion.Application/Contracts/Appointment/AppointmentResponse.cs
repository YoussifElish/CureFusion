namespace CureFusion.Application.Contracts.Appointment;

public record AppointmentResponse(
    int Id,
    int DoctorId,
    DateTime AppointmentDate,
   int DurationInMinutes,
   decimal PricePerSlot
);

