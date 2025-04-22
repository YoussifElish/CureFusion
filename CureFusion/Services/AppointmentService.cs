using CureFusion.Abstactions;
using CureFusion.Contracts.Appointment;
using CureFusion.Errors;
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

    public async Task<Result<IEnumerable<AppointmentResponse>>> GetActiveAppointments(CancellationToken cancellationToken = default)
    {
        var appointments = await _dbContext.Appointments.Where(Appointment => Appointment.Status != Enums.AppointmentStatus.Canceled)
       .AsNoTracking()
       .ToListAsync(cancellationToken);
        var appointmentResponses = appointments.Adapt<IEnumerable<AppointmentResponse>>();

        return Result.Success(appointmentResponses);

    }
    public async Task<Result> CancelAppointment(int AppointmentId,int userId, CancellationToken cancellationToken = default)
    {
        var appointment = await _dbContext.Appointments
            .Where(x => x.Id == AppointmentId)
            .FirstOrDefaultAsync() ;    
        if (appointment.DoctorId != userId)
        {
            return Result.Failure(AppointmentErrors.NotAuthorized);
        }

        var isExceededTime = appointment.AppointmentDate < DateTime.UtcNow.AddHours(-1);
        if (isExceededTime)
        {
            return Result.Failure(AppointmentErrors.ExceedTime);
        }
        if (appointment.Status == Enums.AppointmentStatus.Canceled)
        {
            return Result.Failure(AppointmentErrors.CancelledSession);
        }
            appointment.Status = Enums.AppointmentStatus.Canceled;
        _dbContext.Appointments.Update(appointment);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

  

    //public async Task<Result<AppointmentResponse>> RescheduleAppointment(AppointmentReschudleRequest request, string userId, CancellationToken cancellationToken = default)
    //{
    //    var appointment = await _dbContext.Appointments
    //        .Where(x => x.Id == request.Id)
    //        .FirstOrDefaultAsync();
    //    if (appointment.DoctorId != userId)
    //    {
    //        return Result.Failure<AppointmentResponse>(AppointmentErrors.NotAuthorized);
    //    }

    //    var isExceededTime = appointment.AppointmentDate < DateTime.UtcNow.AddHours(-1);
    //    if (isExceededTime)
    //    {
    //        return Result.Failure<AppointmentResponse>(AppointmentErrors.ExceedTime);
    //    }
    //    appointment.AppointmentDate = request.NewTime;
    //    _dbContext.Appointments.Update(appointment);
    //    await _dbContext.SaveChangesAsync(cancellationToken);
    //    var appointmentResponse = appointment.Adapt<AppointmentResponse>();
    //    return Result.Success(appointmentResponse);
    //}

    public async Task<Result<AppointmentResponse>> ReverseAppointment(AppointmentRequest request, CancellationToken cancellationToken = default)
    {

        var requestedStartTime = request.AppointmentDate;
        var requestedEndTime = requestedStartTime.AddMinutes(request.DurationInMinutes);
        var conflictingAppointments = await _dbContext.Appointments
       .Where(x => x.DoctorId == request.DoctorId && x.Status != Enums.AppointmentStatus.Canceled)
       .Where(x =>
           (x.AppointmentDate < requestedEndTime && x.AppointmentDate.AddMinutes(x.DurationInMinutes) > requestedStartTime) 
       )
       .AnyAsync(cancellationToken);

        if (conflictingAppointments)
        {
            return Result.Failure<AppointmentResponse>(Errors.AppointmentErrors.NotAvaliable);
        }

        var newAppointment = new Appointment
        {
           
            DoctorId = request.DoctorId,
            AppointmentDate = request.AppointmentDate,
            AppointmentType = request.AppointmentType,
            Status = Enums.AppointmentStatus.Pending,  
            DurationInMinutes = request.DurationInMinutes
        };
        _dbContext.Appointments.Add(newAppointment);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var appointmentResponse = newAppointment.Adapt<AppointmentResponse>();
        return Result.Success(appointmentResponse);
    }

    public Task<Result> CancelAppointment(int AppointmentId, string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AppointmentResponse>> RescheduleAppointment(AppointmentReschudleRequest request, string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
