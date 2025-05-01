using CureFusion.Abstactions;
using CureFusion.Contracts.Appointment;
using CureFusion.Contracts.Doctor;
using CureFusion.Entities;
using CureFusion.Enums;
using CureFusion.Errors;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CureFusion.Services;

public class AppointmentService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, PaymobService paymobService) : IAppointmentService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly PaymobService _paymobService = paymobService;
    private readonly string _filesPath = $"https://curefusion2.runasp.net/Uploads";

    public async Task<Result<IEnumerable<AppointmentResponse>>> GetAllAppointments(CancellationToken cancellationToken = default)
    {
        var appointments = await _dbContext.Appointments
               .AsNoTracking()
               .ToListAsync(cancellationToken);
        var appointmentResponses = appointments.Adapt<IEnumerable<AppointmentResponse>>();

        return Result.Success(appointmentResponses);


    }

    public async Task<Result<IEnumerable<DoctorAppoitmentsResponse>>> GetAllDoctorsWithAppoitments(CancellationToken cancellationToken = default)
    {
        var doctorsWithAppointments = await _dbContext.Appointments
            .Include(x => x.Doctor).
            ThenInclude(x=>x.User)
            .Where(x => x.Doctor != null)
            .Select(x => x.Doctor)
            .Distinct()
            .ToListAsync();
        

        foreach (var item in doctorsWithAppointments)
        {
            item.ProfileImage = await _dbContext.UploadedFiles
                .FirstOrDefaultAsync(x => x.Id == item.ProfileImageId, cancellationToken);
        }
        var response = doctorsWithAppointments.Select(appoitment => new DoctorAppoitmentsResponse
        {
            Id = appoitment.Id,
            UserId = appoitment.UserId,
            Specialization = appoitment.Specialization,
            Bio = appoitment.Bio,
            YearsOfExperience = appoitment.YearsOfExperience,
            Rating = appoitment.Rating,
            TotalReviews = appoitment.TotalReviews,
            ProfileImagePath = $"{_filesPath}/{appoitment.ProfileImage.StoredFileName}",
            doctorName = appoitment.User.FirstName + " " + appoitment.User.LastName

        });

        return Result.Success(response);


    }

    public async Task<Result<IEnumerable<AppointmentResponse>>> GetActiveAppointments(CancellationToken cancellationToken = default)
    {
        var appointments = await _dbContext.Appointments.Where(Appointment => Appointment.Status != Enums.AppointmentStatus.Canceled)
       .AsNoTracking()
       .ToListAsync(cancellationToken);
        var appointmentResponses = appointments.Adapt<IEnumerable<AppointmentResponse>>();

        return Result.Success(appointmentResponses);

    }

    public async Task<Result<IEnumerable<AppointmentResponse>>> GetActiveAppointmentsByDoctorId(int id , CancellationToken cancellationToken = default)
    {
        var appointments = await _dbContext.Appointments.Where(Appointment => Appointment.DoctorId == id &&Appointment.Status != Enums.AppointmentStatus.Canceled)
       .AsNoTracking()
       .ToListAsync(cancellationToken);
        var appointmentResponses = appointments.Adapt<IEnumerable<AppointmentResponse>>();

        return Result.Success(appointmentResponses);

    }
    public async Task<Result> CancelAppointment(int AppointmentId, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var appointment = await _dbContext.Appointments
            .Where(x => x.Id == AppointmentId)
            .FirstOrDefaultAsync();
        if (appointment.Doctor.UserId != userId)
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


    public async Task<Result<PatientAppointmentResponse>> BookAppointment(PatientAppointmentRequest request, CancellationToken cancellationToken = default)
    {

        //var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = "419c725a-7792-4b1f-b9cd-5a9896b98bd2";


        var appointment = await _dbContext.Appointments
            .FirstOrDefaultAsync(x => x.Id == request.AppointmentId, cancellationToken);

        if (appointment is null)
            return Result.Failure<PatientAppointmentResponse>(AppointmentErrors.NotFound);

        var isAlreadyBooked = await _dbContext.patientAppointments
            .AnyAsync(x => x.AppointmentId == request.AppointmentId && (x.Status == AppointmentStatus.Completed
                    || x.Status == AppointmentStatus.Pending
                    || x.Status == AppointmentStatus.Confirmed),
              cancellationToken);

        if (isAlreadyBooked)
            return Result.Failure<PatientAppointmentResponse>(AppointmentErrors.AlreadyBooked);


        var patientAppointment = request.Adapt<PatientAppointment>();
        patientAppointment.BookedAt = DateTime.UtcNow;
        patientAppointment.Status = AppointmentStatus.Pending;
        patientAppointment.UserId = userId;


        var paymentLink = await _paymobService.GeneratePaymentLink(appointment.PricePerSlot, appointment.Id);
        patientAppointment.PaymentUrl = paymentLink;
        _dbContext.patientAppointments.Add(patientAppointment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = patientAppointment.Adapt<PatientAppointmentResponse>();
        


        return Result.Success(response);
    }


    public Task<Result> CancelAppointment(int AppointmentId, string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> ConfirmAppointmentPayment(int appointmentId, bool isSuccess, CancellationToken cancellationToken = default)
    {
        var patientAppointment = await _dbContext.patientAppointments
            .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId && x.Status == AppointmentStatus.Pending || x.Status == AppointmentStatus.NotReversed, cancellationToken);

        var appointment = await _dbContext.Appointments
            .FirstOrDefaultAsync(x => x.Id == appointmentId && (x.Status == AppointmentStatus.Pending || x.Status == AppointmentStatus.NotReversed), cancellationToken);

        if (appointment is null || patientAppointment is null)
            return Result.Failure(AppointmentErrors.NotFound);

        if (isSuccess)
        {
            patientAppointment.Status = AppointmentStatus.Confirmed;
            appointment.Status = AppointmentStatus.Confirmed;
        }
        else
        {
            patientAppointment.Status = AppointmentStatus.Canceled;
            appointment.Status = AppointmentStatus.NotReversed;
        }

        patientAppointment.StatusChangedAt = DateTime.UtcNow;

        _dbContext.patientAppointments.Update(patientAppointment);
        _dbContext.Appointments.Update(appointment);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return isSuccess ? Result.Success(patientAppointment) : Result.Failure(AppointmentErrors.PaymentFailed);
    }

}
