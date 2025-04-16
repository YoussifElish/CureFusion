using CureFusion.Abstactions;
using CureFusion.Contracts.Appointment;
using Mapster;

namespace CureFusion.Services;

public class AppointmentService(ApplicationDbContext dbContext) : IAppointmentService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IEnumerable<AppointmentResponse>>> GetAllAppointments(CancellationToken cancellationToken = default)
    {
        var appointments = await _dbContext.Appointments
               .AsNoTracking()
               .ToListAsync(cancellationToken);
        var appointmentResponses = appointments.Adapt<IEnumerable<AppointmentResponse>>();

        return Result.Success(appointmentResponses);

    
    }
    public Task<Result<AppointmentResponse>> CancelAppointment(int AppointmentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<AppointmentResponse>>> GetActiveAppointments(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

  

    public Task<Result<AppointmentResponse>> RescheduleAppointment(int AppointmentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AppointmentResponse>> ReverseAppointment(AppointmentRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
