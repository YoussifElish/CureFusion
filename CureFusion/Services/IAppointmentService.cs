using CureFusion.Abstactions;
using CureFusion.Contracts.Appointment;

namespace CureFusion.Services;

public interface IAppointmentService
{
    Task<Result<IEnumerable<AppointmentResponse>>> GetAllAppointments(CancellationToken cancellationToken = default!); 
    Task<Result<IEnumerable<AppointmentResponse>>> GetActiveAppointments(CancellationToken cancellationToken = default!); 
    Task<Result<AppointmentResponse>> ReverseAppointment(AppointmentRequest request, CancellationToken cancellationToken = default!); 
    Task<Result<AppointmentResponse>> CancelAppointment(int AppointmentId, CancellationToken cancellationToken = default!); 
    Task<Result<AppointmentResponse>> RescheduleAppointment(int AppointmentId, CancellationToken cancellationToken = default!); 
}
