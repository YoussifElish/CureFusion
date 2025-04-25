using CureFusion.Abstactions;
using CureFusion.Contracts.Appointment;

namespace CureFusion.Services;

public interface IAppointmentService
{
    Task<Result<IEnumerable<AppointmentResponse>>> GetAllAppointments(CancellationToken cancellationToken = default!); 
    Task<Result<IEnumerable<AppointmentResponse>>> GetActiveAppointments(CancellationToken cancellationToken = default!); 
    Task<Result> CancelAppointment(int AppointmentId, CancellationToken cancellationToken = default!); 
    Task<Result<PatientAppointmentResponse>> BookAppointment(PatientAppointmentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ConfirmAppointmentPayment(int AppointmentId, bool isSuccess, CancellationToken cancellationToken = default);
}
