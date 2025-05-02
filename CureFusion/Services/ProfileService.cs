using CureFusion.Contracts.Profile;
using RealState.Services;

namespace CureFusion.Services;

public class ProfileService : IProfileService
{
    private readonly ApplicationDbContext _context;
    private readonly IFileService _fileService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileService(ApplicationDbContext context, IFileService fileService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _fileService = fileService;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    private string? GetUserId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    // Main Profile Method - Depending on Role
    public async Task<Result<IProfileDto>> GetProfileAsync(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId == null)
            return Result.Failure<IProfileDto>(AuthErrors.InavlidUser);

        var user = await _context.Users
            .Include(u => u.ProfileImage).Include(u => u.Questions)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Result.Failure<IProfileDto>(AuthErrors.NotFound);

        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (role == "Doctor")
        {
            return await GetDoctorProfileAsync(user, cancellationToken);
        }
        else if (role == "Patient")
        {
            return await GetUserProfileAsync(user, cancellationToken);
        }

        return Result.Failure<IProfileDto>(AuthErrors.OperationFailed);
    }

    // Get Profile for Doctor
    // Get Profile for Doctor
    private async Task<Result<IProfileDto>> GetDoctorProfileAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(d => d.ProfileImage)
            .Include(d => d.CertificationDocument)
            .FirstOrDefaultAsync(d => d.UserId == user.Id, cancellationToken);

        if (doctor == null)
            return Result.Failure<IProfileDto>(DoctorErrors.NotFound);

        var answersCount = await _context.Answers.CountAsync(a => a.UserId == user.Id, cancellationToken);

        var now = DateTime.UtcNow;

        var totalAppointments = await _context.patientAppointments
            .Include(p => p.Appointment)
            .CountAsync(p =>
                p.Appointment.DoctorId == doctor.Id &&
                (p.Status == AppointmentStatus.Confirmed || p.Status == AppointmentStatus.Completed),
                cancellationToken);

        var completedAppointments = await _context.patientAppointments
            .Include(p => p.Appointment)
            .CountAsync(p =>
                p.Appointment.DoctorId == doctor.Id &&
                p.Status == AppointmentStatus.Completed,
                cancellationToken);

        var upcomingAppointments = await _context.patientAppointments
            .Include(p => p.Appointment)
            .CountAsync(p =>
                p.Appointment.DoctorId == doctor.Id &&
                p.Status == AppointmentStatus.Confirmed &&
                p.Appointment.AppointmentDate > now,
                cancellationToken);

        var doctorProfile = new DoctorProfileDto
        {
            Id = doctor.UserId,
            FullName = $"{doctor.User.FirstName} {doctor.User.LastName}",
            Email = doctor.User.Email ?? string.Empty,
            Specialization = doctor.Specialization,
            YearsOfExperience = doctor.YearsOfExperience,
            Rating = doctor.Rating,
            TotalReviews = doctor.TotalReviews,
            Bio = doctor.Bio,
            AccountStatus = doctor.accountStatus,
            // Get ProfileImage URL from UploadedFiles table
            ProfileImageUrl = doctor.ProfileImageId != null
                ? $"http://curefusion2.runasp.net/Uploads/{await _context.UploadedFiles.Where(x => x.Id == doctor.ProfileImageId).Select(x => x.StoredFileName).FirstOrDefaultAsync()}"
                : null,
            // Check if CertificationDocument exists and generate URL
            CertificationDocumentUrl = doctor.CertificationDocument != null
                ? $"http://curefusion2.runasp.net/Uploads/{await _context.UploadedFiles.Where(x => x.Id == doctor.CertificationDocument.Id).Select(x => x.StoredFileName).FirstOrDefaultAsync()}"
                : null,
            TotalAnswers = answersCount,
            TotalAppointments = totalAppointments,
            CompletedAppointments = completedAppointments,
            UpcomingAppointments = upcomingAppointments
        };

        return Result.Success<IProfileDto>(doctorProfile);
    }

    // Get Profile for Member/User
    private async Task<Result<IProfileDto>> GetUserProfileAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var upcomingAppointments = await _context.patientAppointments
            .Include(a => a.Appointment)
            .CountAsync(a => a.UserId == user.Id &&
                             a.Status == AppointmentStatus.Confirmed &&
                             a.Appointment.AppointmentDate > DateTime.UtcNow,
                             cancellationToken);

        var userProfile = new UserProfileDto
        {
            Id = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email ?? string.Empty,
            DOB = user.DOB,
            // Get ProfileImage URL from UploadedFiles table
            ProfileImageUrl = user.ProfileImageId != null
                ? $"http://curefusion2.runasp.net/Uploads/{await _context.UploadedFiles.Where(x => x.Id == user.ProfileImageId).Select(x => x.StoredFileName).FirstOrDefaultAsync()}"
                : null,
            QuestionsCount = user.Questions.Count,
            UpcomingAppointments = upcomingAppointments
        };

        return Result.Success<IProfileDto>(userProfile);
    }

    // Update Email Address
    public async Task<Result> ChangeEmailAsync(string newEmail, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId == null)
            return Result.Failure(AuthErrors.InavlidUser);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Failure(AuthErrors.NotFound);

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        var result = await _userManager.ChangeEmailAsync(user, newEmail, token);

        return result.Succeeded ? Result.Success() : Result.Failure(AuthErrors.OperationFailed);
    }

    // Update Password
    public async Task<Result> ChangePasswordAsync(string currentPassword, string newPassword, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId == null)
            return Result.Failure(AuthErrors.InavlidUser);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Failure(AuthErrors.NotFound);
        

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.Succeeded ? Result.Success() : Result.Failure(AuthErrors.OperationFailed);
    }
    public async Task<Result> UpdateSessionExpiryAsync(int expiryMinutes, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId == null)
            return Result.Failure(AuthErrors.InavlidUser);

        if (expiryMinutes < 10 || expiryMinutes > 43200) 
            return Result.Failure(SessionErrors.Invalid);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Result.Failure(AuthErrors.NotFound);

        user.SessionExpiryMinutes = expiryMinutes;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateProfileImageAsync(IFormFile imageFile, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId == null)
            return Result.Failure(AuthErrors.InavlidUser);

        if (imageFile == null || imageFile.Length == 0)
            return Result.Failure(ArticleErrors.ImageNotProvided);

        // رفع الملف
        var uploadResult = await _fileService.UploadImagesAsync(imageFile, cancellationToken);
       

        var uploadedFile = uploadResult;

        // البحث عن المستخدم
        var user = await _context.Users.Include(u => u.ProfileImage).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
            return Result.Failure(AuthErrors.NotFound);

        var roles = await _userManager.GetRolesAsync(user);
        var isDoctor = roles.Contains("Doctor");

        // إذا كان طبيب
        if (isDoctor)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id, cancellationToken);
            if (doctor == null)
                return Result.Failure(DoctorErrors.NotFound);

            doctor.ProfileImageId = uploadedFile.Id;
        }
        else
        {
            user.ProfileImageId = uploadedFile.Id;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}