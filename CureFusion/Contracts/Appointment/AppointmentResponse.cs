using CureFusion.Enums;

namespace CureFusion.Contracts.Appointment;

public record AppointmentResponse (
    int Id,
    int DoctorId,
    DateTime AppointmentDate,
   int DurationInMinutes,
   decimal PricePerSlot    
);

