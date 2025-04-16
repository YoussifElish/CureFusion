using CureFusion.Abstactions;
using CureFusion.Contracts.Appointment;

namespace CureFusion.Services;

public interface IAppointmentService
{
    Task<Result<IEnumerable<AppointmentResponse>>> GetAllAppointments(CancellationToken cancellationToken = default!); 
    Task<Result<IEnumerable<AppointmentResponse>>> GetActiveAppointments(CancellationToken cancellationToken = default!); 
    Task<Result<AppointmentResponse>> ReverseAppointment(AppointmentRequest request, CancellationToken cancellationToken = default!); 
    Task<Result> CancelAppointment(int AppointmentId, string userId, CancellationToken cancellationToken = default!); 
    Task<Result<AppointmentResponse>> RescheduleAppointment(AppointmentReschudleRequest request, string userId, CancellationToken cancellationToken = default!); 
}
