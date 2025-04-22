using CureFusion.Enums;

namespace CureFusion.Contracts.Doctor;

public record DoctorAvailabilityRequest (
    DateTime Date,
    TimeSpan From ,
    TimeSpan To , 
    int SlotDurationInMinutes,
    decimal PricePerSlot ,
    AppointmentType SessionMode 
    );