using CureFusion.Domain.Enums;

namespace CureFusion.Application.Contracts.Doctor;

public record DoctorAvailabilityRequest(
    DateTime Date,
    TimeSpan From,
    TimeSpan To,
    int SlotDurationInMinutes,
    decimal PricePerSlot,
    AppointmentType SessionMode
    );