namespace CureFusion.Application.Contracts.Appointment;

public record AppointmentReschudleRequest
(
    int Id
    , DateTime NewTime);
