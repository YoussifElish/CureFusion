using CureFusion.Application.Contracts.Doctor;
using CureFusion.Application.Services;

namespace CureFusion.API.Services;

public class DoctorService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ISessionService sessionService, IFileService fileService) : IDoctorService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ISessionService _sessionService = sessionService;
    private readonly IFileService _fileService = fileService;

    public async Task<Result> RegisterAsDoctor(DoctorRegisterRequest request, RegisterDoctorImageRequest imageRequest, CancellationToken cancellationToken = default)
    {
        // var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", string.Empty);
        //var session = await _sessionService.IsSessionValidAsync(token, cancellationToken);
        // if (!session)
        // {
        //     return Result.Failure(AuthErrors.InvalidSession);
        // }

        var user = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isRegisteredBefore = await _context.Doctors.Where(d => d.UserId == user).SingleOrDefaultAsync();

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
                var doctorCertificateImage = await _fileService.UploadImagesAsync(imageRequest.CertificateImage, cancellationToken);
                var doctorProfileImage = await _fileService.UploadImagesAsync(imageRequest.ProfileImage, cancellationToken);

                isRegisteredBefore.Specialization = request.Specialization;
                isRegisteredBefore.Bio = request.Bio;
                isRegisteredBefore.accountStatus = AccountStatus.Pending;
                isRegisteredBefore.CertificationDocumentId = doctorCertificateImage.Id;
                isRegisteredBefore.ProfileImageId = doctorProfileImage.Id;

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
        }



        var newDoctor = request.Adapt<Doctor>();
        newDoctor.UserId = user; // TODO : I will remove it later when i add iaccessor to get current user id 
        _context.Doctors.Add(newDoctor);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }


    public async Task<Result> DoctorAvaliability(DoctorAvailabilityRequest request, CancellationToken cancellationToken = default)
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
                        (x.From < request.To && x.To > request.From && x.Date == request.Date && x.IsActive)
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
                DoctorAvailabilityId = doctorAvailability.Id,
                doctorAvailability = doctorAvailability,
                Doctor = isUserDoctor,
                PricePerSlot = request.PricePerSlot
            };
            appointments.Add(appointment);
            startTime += sessionDuration;
        }
        _context.Appointments.AddRange(appointments);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<List<DoctorAvailability>>> GetDoctorAvailabilities(CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var doctor = await _context.Doctors
            .Where(d => d.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (doctor == null)
            return Result.Failure<List<DoctorAvailability>>(DoctorErrors.NotDoctor);

        var availabilities = await _context.DoctorAvailabilities
        .Where(a => a.DoctorId == doctor.Id
         && a.Date >= DateTime.Today
         && a.To > DateTime.Now.TimeOfDay && a.IsActive)
            .OrderByDescending(a => a.Date)
            .ToListAsync(cancellationToken);

        var result = availabilities.Adapt<List<DoctorAvailability>>();

        return Result.Success(result);
    }

    public async Task<Result<List<Appointment>>> GetDoctorAppointments(CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var doctor = await _context.Doctors
            .Where(d => d.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (doctor == null)
            return Result.Failure<List<Appointment>>(DoctorErrors.NotDoctor);
        var appointments = await _context.Appointments.Include(x => x.doctorAvailability)
            .Where(a => a.DoctorId == doctor.Id && a.AppointmentDate >= DateTime.UtcNow.Date && a.doctorAvailability.IsActive)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync(cancellationToken);
        var result = appointments.Adapt<List<Appointment>>();
        return Result.Success(result);
    }
    public async Task<Result> DeleteDoctorAvaliability(int id, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure(AuthErrors.NotFound);
        }

        var doctorAvailability = await _context.DoctorAvailabilities
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (doctorAvailability is null)
        {
            return Result.Failure(DoctorErrors.NotFound);
        }

        if (!doctorAvailability.IsActive)
        {
            return Result.Failure(DoctorErrors.AlreadyDeleted);
        }

        var doctorId = await _context.Doctors
            .Where(d => d.UserId == userId)
            .Select(d => (int?)d.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (doctorId is null)
        {
            return Result.Failure(DoctorErrors.NotDoctor);
        }

        if (doctorAvailability.DoctorId != doctorId)
        {
            return Result.Failure(DoctorErrors.NotYourSession);
        }

        var appointmentsToDelete = await _context.Appointments
            .Where(a => a.DoctorAvailabilityId == id)
            .ToListAsync(cancellationToken);

        if (appointmentsToDelete.Any())
        {
            var appointmentIds = appointmentsToDelete.Select(a => a.Id).ToList();

            var patientAppointments = await _context.patientAppointments
                .Where(p => appointmentIds.Contains(p.AppointmentId))
                .ToListAsync(cancellationToken);

            if (patientAppointments.Any())
            {
                _context.patientAppointments.RemoveRange(patientAppointments);
            }

            _context.Appointments.RemoveRange(appointmentsToDelete);

        }

        doctorAvailability.IsActive = false;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception)
        {
            // يمكن إضافة تسجيل للخطأ هنا إذا لزم الأمر
            return Result.Failure(DoctorErrors.Duplicated);
        }
    }


    public async Task<Result> DeleteDoctorAppointment(int Id, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var isSessionExist = await _context.Appointments.Where(x => x.Id == Id).FirstOrDefaultAsync(cancellationToken);
        if (isSessionExist is null)
        {
            return Result.Failure(DoctorErrors.NotFound);
        }
        if (isSessionExist.Status == AppointmentStatus.Completed)
        {
            return Result.Failure(DoctorErrors.SessionEnded);
        }
        var patientAppointments = await _context.patientAppointments
            .Where(p => p.Id == Id).ToListAsync();

        _context.RemoveRange(patientAppointments);
        _context.RemoveRange(isSessionExist);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

}
