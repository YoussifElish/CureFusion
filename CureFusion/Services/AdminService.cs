// Implementation of the IAdminService interface.
// Updated GetDoctorDetailsAsync to use StoredFileName directly from UploadedFile entity.
// Updated GetPatientsAsync and GetAppointmentsAsync to fix type conversion errors and correctly handle Patient as User.

using System.Linq.Dynamic.Core;
using CureFusion.Application.Contracts.Admin;
using CureFusion.Application.Services;

namespace CureFusion.API.Services;

public class AdminService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IAdminService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    // --- Dashboard --- 
    public async Task<Result<DashboardStatsDto>> GetDashboardStatsAsync(CancellationToken cancellationToken)
    {
        var totalDoctors = await _context.Doctors.CountAsync(cancellationToken);
        var pendingDoctors = await _context.Doctors.CountAsync(d => d.accountStatus == AccountStatus.Pending, cancellationToken);
        var patientUsers = await _userManager.GetUsersInRoleAsync("Patient");
        var totalPatients = patientUsers.Count;
        var totalAppointments = await _context.Appointments.CountAsync(cancellationToken);
        decimal totalRevenue = 0; // TODO: Implement revenue calculation
        var stats = new DashboardStatsDto(totalDoctors, pendingDoctors, totalPatients, totalAppointments, totalRevenue);
        return Result.Success(stats);
    }

    // --- Doctor Management ---
    public async Task<Result<PageinatedList<DoctorAdminViewDto>>> GetDoctorsAsync(AdminDoctorFilter filter, CancellationToken cancellationToken)
    {
        var doctorsQuery = _context.Doctors
            .Include(d => d.User)
            .AsQueryable();

        // Apply filters
        if (filter.StatusFilter.HasValue)
        {
            doctorsQuery = doctorsQuery.Where(d => d.accountStatus == filter.StatusFilter.Value);
        }
        if (!string.IsNullOrWhiteSpace(filter.SpecializationFilter))
        {
            doctorsQuery = doctorsQuery.Where(d => d.Specialization.Contains(filter.SpecializationFilter));
        }
        if (!string.IsNullOrWhiteSpace(filter.SearchValue))
        {
            doctorsQuery = doctorsQuery.Where(d => (d.User.FirstName + " " + d.User.LastName).Contains(filter.SearchValue) || d.Specialization.Contains(filter.SearchValue));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(filter.SortColumn))
        {
            var sortColumn = filter.SortColumn switch
            {
                "DoctorName" => "User.FirstName",
                "Specialization" => "Specialization",
                "MemberSince" => "User.CreatedAt",
                "Status" => "accountStatus",
                "Rating" => "Rating",
                _ => "User.FirstName"
            };
            if (filter.SortColumn == "DoctorName")
            {
                doctorsQuery = filter.SortDirection?.ToLower() == "desc"
                   ? doctorsQuery.OrderByDescending(d => d.User.FirstName).ThenByDescending(d => d.User.LastName)
                   : doctorsQuery.OrderBy(d => d.User.FirstName).ThenBy(d => d.User.LastName);
            }
            else
            {
                doctorsQuery = doctorsQuery.OrderBy($"{sortColumn} {filter.SortDirection ?? "asc"}");
            }
        }
        else
        {
            doctorsQuery = doctorsQuery.OrderBy(d => d.User.FirstName).ThenBy(d => d.User.LastName);
        }

        // Project to DTO before passing to CreateAsync
        var source = doctorsQuery.Select(d => new DoctorAdminViewDto(
            d.UserId,
            d.User.FirstName + " " + d.User.LastName,
            d.Specialization,
            d.accountStatus,
            0, // TODO: Implement Earned calculation
            d.Rating
        ));

        var response = await PageinatedList<DoctorAdminViewDto>.CreateAsync(
            source.AsNoTracking(), // Apply AsNoTracking here
            filter.PageNumber,
            filter.PageSize,
            cancellationToken
        );

        return Result.Success(response);
    }

    public async Task<Result<DoctorDetailsDto>> GetDoctorDetailsAsync(string doctorUserId, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.ProfileImage)
            .Include(d => d.CertificationDocument)
            .FirstOrDefaultAsync(d => d.UserId == doctorUserId, cancellationToken);

        if (doctor == null || doctor.User == null)
        {
            return Result.Failure<DoctorDetailsDto>(AuthErrors.NotFound);
        }

        string? profileImageStoredName = doctor.ProfileImage?.StoredFileName;
        string? certificationDocStoredName = doctor.CertificationDocument?.StoredFileName;

        var detailsDto = new DoctorDetailsDto(
            doctor.UserId,
            doctor.User.FirstName,
            doctor.User.LastName,
            doctor.User.Email ?? "",
            doctor.User.PhoneNumber ?? "",
            doctor.Specialization,
            doctor.Bio,
            doctor.YearsOfExperience,
            doctor.accountStatus,
            profileImageStoredName,
            certificationDocStoredName
        );

        return Result.Success(detailsDto);
    }

    public async Task<Result> UpdateDoctorStatusAsync(string doctorUserId, UpdateDoctorStatusRequest request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.UserId == doctorUserId, cancellationToken);

        if (doctor == null)
        {
            return Result.Failure(AuthErrors.NotFound);
        }

        if (!Enum.IsDefined(typeof(AccountStatus), request.NewStatus))
        {
            return Result.Failure(CommonErrors.InvalidInput);
        }

        doctor.accountStatus = request.NewStatus;
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    // --- Patient Management ---
    public async Task<Result<PageinatedList<PatientAdminViewDto>>> GetPatientsAsync(AdminPatientFilter filter, CancellationToken cancellationToken)
    {
        var patientRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Patient", cancellationToken);
        if (patientRole == null) return Result.Failure<PageinatedList<PatientAdminViewDto>>(CommonErrors.NotFound);

        var patientsQuery = _context.UserRoles
            .Where(ur => ur.RoleId == patientRole.Id)
            .Join(_context.Users, ur => ur.UserId, u => u.Id, (ur, u) => u);

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.SearchValue))
        {
            patientsQuery = patientsQuery.Where(u => (u.FirstName + " " + u.LastName).Contains(filter.SearchValue) || (u.Email != null && u.Email.Contains(filter.SearchValue)) || (u.PhoneNumber != null && u.PhoneNumber.Contains(filter.SearchValue)));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(filter.SortColumn))
        {
            var sortColumn = filter.SortColumn switch
            {
                "PatientName" => "FirstName",
                "PhoneNumber" => "PhoneNumber",
                "Email" => "Email",
                "MemberSince" => "CreatedAt",
                _ => "FirstName"
            };
            if (filter.SortColumn == "PatientName")
            {
                patientsQuery = filter.SortDirection?.ToLower() == "desc"
                   ? patientsQuery.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName)
                   : patientsQuery.OrderBy(u => u.FirstName).ThenBy(u => u.LastName);
            }
            else if (filter.SortColumn != "LastVisit")
            {
                patientsQuery = patientsQuery.OrderBy($"{sortColumn} {filter.SortDirection ?? "asc"}");
            }
            else
            {
                patientsQuery = patientsQuery.OrderBy(u => u.FirstName).ThenBy(u => u.LastName);
            }
        }
        else
        {
            patientsQuery = patientsQuery.OrderBy(u => u.FirstName).ThenBy(u => u.LastName);
        }

        // Project to DTO before passing to CreateAsync
        var source = patientsQuery.Select(u => new PatientAdminViewDto(
            u.Id,
            u.FirstName + " " + u.LastName,
            u.PhoneNumber ?? "",
            u.Email ?? "",
            null // TODO: Implement LastVisit logic
        ));

        var response = await PageinatedList<PatientAdminViewDto>.CreateAsync(
            source.AsNoTracking(), // Apply AsNoTracking here
            filter.PageNumber,
            filter.PageSize,
            cancellationToken
        );

        return Result.Success(response);
    }

    // --- Appointment Management ---
    public async Task<Result<PageinatedList<AppointmentAdminViewDto>>> GetAppointmentsAsync(AdminAppointmentFilter filter, CancellationToken cancellationToken)
    {
        // Query PatientAppointments (actual bookings)
        var patientAppointmentsQuery = _context.patientAppointments
            .Include(pa => pa.Appointment) // Include the original Appointment slot
                .ThenInclude(a => a.Doctor) // Include the Doctor from the Appointment slot
                    .ThenInclude(d => d.User) // Include the User for the Doctor
            .Join(_context.Users, // Join with Users table to get Patient details
                  pa => pa.UserId, // Key from PatientAppointment table
                  user => user.Id, // Key from Users table
                  (pa, user) => new { PatientAppointment = pa, PatientUser = user }); // Result selector

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.DoctorNameFilter))
        {
            // Filter by Doctor's name from the included Appointment -> Doctor -> User
            patientAppointmentsQuery = patientAppointmentsQuery.Where(pa => (pa.PatientAppointment.Appointment.Doctor.User.FirstName + " " + pa.PatientAppointment.Appointment.Doctor.User.LastName).Contains(filter.DoctorNameFilter));
        }
        if (!string.IsNullOrWhiteSpace(filter.PatientNameFilter))
        {
            // Filter by Patient's name from the joined PatientUser
            patientAppointmentsQuery = patientAppointmentsQuery.Where(pa => (pa.PatientUser.FirstName + " " + pa.PatientUser.LastName).Contains(filter.PatientNameFilter));
        }
        if (filter.AppointmentDateFrom.HasValue)
        {
            // Filter by the date/time from the included Appointment
            patientAppointmentsQuery = patientAppointmentsQuery.Where(pa => pa.PatientAppointment.Appointment.AppointmentDate >= filter.AppointmentDateFrom.Value);
        }
        if (filter.AppointmentDateTo.HasValue)
        {
            // Filter by the date/time from the included Appointment
            patientAppointmentsQuery = patientAppointmentsQuery.Where(pa => pa.PatientAppointment.Appointment.AppointmentDate <= filter.AppointmentDateTo.Value);
        }
        if (!string.IsNullOrWhiteSpace(filter.SearchValue))
        {
            // General search across Doctor name, Patient name, Specialization
            patientAppointmentsQuery = patientAppointmentsQuery.Where(pa =>
               (pa.PatientAppointment.Appointment.Doctor.User.FirstName + " " + pa.PatientAppointment.Appointment.Doctor.User.LastName).Contains(filter.SearchValue) ||
               (pa.PatientUser.FirstName + " " + pa.PatientUser.LastName).Contains(filter.SearchValue) ||
               pa.PatientAppointment.Appointment.Doctor.Specialization.Contains(filter.SearchValue)
            );
        }
        // TODO: Add filter for PatientAppointment.Status if needed in AdminAppointmentFilter

        // Apply sorting
        if (!string.IsNullOrEmpty(filter.SortColumn))
        {
            // Sorting on joined anonymous types with Dynamic LINQ can be tricky.
            // Defaulting to sorting by AppointmentDate for now.
            // Consider standard LINQ OrderBy/ThenBy for more complex sorting before projection if needed.
            patientAppointmentsQuery = patientAppointmentsQuery.OrderByDescending(pa => pa.PatientAppointment.Appointment.AppointmentDate);
        }
        else
        {
            // Default sort
            patientAppointmentsQuery = patientAppointmentsQuery.OrderByDescending(pa => pa.PatientAppointment.Appointment.AppointmentDate);
        }

        // Project to DTO before passing to CreateAsync
        var source = patientAppointmentsQuery.Select(pa => new AppointmentAdminViewDto(
            pa.PatientAppointment.Id, // Use PatientAppointment Id as the unique booking ID
            pa.PatientAppointment.Appointment.Doctor.User.FirstName + " " + pa.PatientAppointment.Appointment.Doctor.User.LastName,
            pa.PatientUser.FirstName + " " + pa.PatientUser.LastName,
            pa.PatientAppointment.Appointment.Doctor.Specialization,
            pa.PatientAppointment.Appointment.AppointmentDate,
            pa.PatientAppointment.Status.ToString(), // Status from PatientAppointment
            pa.PatientAppointment.Appointment.PricePerSlot // Amount from the original Appointment slot
        ));

        var response = await PageinatedList<AppointmentAdminViewDto>.CreateAsync(
            source.AsNoTracking(), // Apply AsNoTracking here
            filter.PageNumber,
            filter.PageSize,
            cancellationToken
        );

        return Result.Success(response);
    }
}
