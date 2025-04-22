using CureFusion.Abstactions;
using CureFusion.Contracts.Doctor;
using CureFusion.Entities;
using CureFusion.Enums;
using CureFusion.Errors;
using Mapster;

namespace CureFusion.Services;

public class DoctorService(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor,ISessionService sessionService ) : IDoctorService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ISessionService _sessionService = sessionService;

    public async Task<Result> RegisterAsDoctor(DoctorRegisterRequest request, CancellationToken cancellationToken = default)
    {
       // var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", string.Empty);
       //var session = await _sessionService.IsSessionValidAsync(token, cancellationToken);
       // if (!session)
       // {
       //     return Result.Failure(AuthErrors.InvalidSession);
       // }

        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isRegisteredBefore = await _context.Doctors.Where(d=>d.UserId == userId).SingleOrDefaultAsync();
        
        if (isRegisteredBefore is not null)
        {
            if (isRegisteredBefore.accountStatus == AccountStatus.Pending)
            {
                return Result.Failure(DoctorErrors.Pending);
            }
            else if (isRegisteredBefore.accountStatus == AccountStatus.Accepted)
            {
                return Result.Failure(DoctorErrors.RegisteredBefore);
            }
            else if (isRegisteredBefore.accountStatus == AccountStatus.Removed)
            {
                return Result.Failure(DoctorErrors.Removed);
            }

           else if (isRegisteredBefore.accountStatus == AccountStatus.Rejected)
            {
                isRegisteredBefore.Specialization = request.Specialization;
                isRegisteredBefore.Bio = request.Bio;
                isRegisteredBefore.accountStatus = AccountStatus.Pending;

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
        }

       

            var newDoctor = request.Adapt<Doctor>();
            newDoctor.UserId = userId; // TODO : I will remove it later when i add iaccessor to get current user id 
            _context.Doctors.Add(newDoctor);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        
    }


    public async Task<Result> DoctorAvaliability(DoctorAvailabilityRequest request,  CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var isUserDoctor = await _context.Doctors.Where(d => d.UserId == userId).SingleOrDefaultAsync(cancellationToken);
        if (isUserDoctor is null)
        {
            return Result.Failure(DoctorErrors.NotDoctor);
        }

        var isSessionDuplicated = await _context.DoctorAvailabilities
     .AnyAsync(x => x.DoctorId == isUserDoctor.Id &&
                    (
                        (x.From < request.To && x.To > request.From)  
                    ), cancellationToken);


        if (isSessionDuplicated)
            return Result.Failure(DoctorErrors.Duplicated);

        var doctorAvailability = request.Adapt<DoctorAvailability>();
        doctorAvailability.DoctorId = isUserDoctor.Id;
         _context.DoctorAvailabilities.Add(doctorAvailability);
        await _context.SaveChangesAsync(cancellationToken);

        var appointments = new List<Appointment>();

        var startTime = request.From;
        var endTime = request.To;
        var sessionDuration = TimeSpan.FromMinutes(request.SlotDurationInMinutes);

        while (startTime + sessionDuration <= endTime)
        {
            var appointment = new Appointment
            {
                DoctorId = isUserDoctor.Id,
                AppointmentDate = request.Date.Add(startTime),
                Status = AppointmentStatus.NotReversed, 
                DurationInMinutes = request.SlotDurationInMinutes, 
                AppointmentType = request.SessionMode,
                DoctorAvailabilityId = doctorAvailability.Id
            };
            appointments.Add(appointment);
            startTime += sessionDuration;
        }
        _context.Appointments.AddRange(appointments);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteDoctorAvaliability(int Id,  CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var isSessionExist = await _context.DoctorAvailabilities.Where(x => x.Id == Id).SingleOrDefaultAsync(cancellationToken);
        if (isSessionExist is null)
        {
            return Result.Failure(DoctorErrors.NotFound);
        }
        if (!isSessionExist.IsActive)
        {
            return Result.Failure(DoctorErrors.AlreadyDeleted);
        }

        if (isSessionExist.DoctorId != _context.Doctors.Where(d => d.UserId == userId).Select(d => d.Id).SingleOrDefault())
        {
            return Result.Failure(DoctorErrors.NotYourSession);
        }

        var appointmentsToDelete = await _context.Appointments
       .Where(a => a.DoctorAvailabilityId == Id)
       .ToListAsync(cancellationToken);

        if (appointmentsToDelete.Any())
        {
            _context.Appointments.RemoveRange(appointmentsToDelete);
        }

        isSessionExist.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
