using CureFusion.Domain.Abstactions;
using Microsoft.AspNetCore.Http;

namespace CureFusion.Domain.Errors;

public static class AppointmentErrors
{
    public static readonly Error NotAuthorized = new("Appointment.NotAuthorized", "You are not authorized to cancel this appointment", StatusCodes.Status401Unauthorized);
    public static readonly Error ExceedTime = new("Appointment.ExceedTime", "Sorry You Cant Cancel or edit you appointment before 1 hour from session time", StatusCodes.Status400BadRequest);
    public static readonly Error CancelledSession = new("Appointment.CancelledSession", "You Can't Access This Session because session status is cancelled", StatusCodes.Status400BadRequest);
    public static readonly Error NotAvaliable = new("Appointment.NotAvaliable", "no sessions with selected doctor in the selected time", StatusCodes.Status400BadRequest);
    public static readonly Error NotFound = new("AppointmentErros.NotFound", "Appointment not found.", StatusCodes.Status400BadRequest);
    public static readonly Error AlreadyBooked = new("AppointmentErros.NotFound", "Appointment already booked", StatusCodes.Status400BadRequest);
    public static readonly Error PaymentFailed = new("Appointment.PaymentFailed", "Payment failed. Appointment was not confirmed", StatusCodes.Status400BadRequest);

}
