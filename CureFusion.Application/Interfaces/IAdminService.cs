// This interface defines the contract for the Admin Service.
// It includes methods for managing doctors, patients, appointments,
// and other administrative tasks based on the dashboard requirements.

using CureFusion.Application.Contracts.Admin;
using CureFusion.Domain.Abstactions; // Assuming Result is here or in a shared location

namespace CureFusion.Application.Services;

public interface IAdminService
{
    // Retrieves statistics for the admin dashboard.
    Task<Result<DashboardStatsDto>> GetDashboardStatsAsync(CancellationToken cancellationToken);

    // Retrieves a paginated list of doctors based on filter criteria.
    Task<Result<PageinatedList<DoctorAdminViewDto>>> GetDoctorsAsync(AdminDoctorFilter filter, CancellationToken cancellationToken);

    // Retrieves detailed information for a specific doctor, including documents for review.
    Task<Result<DoctorDetailsDto>> GetDoctorDetailsAsync(string doctorUserId, CancellationToken cancellationToken);

    // Updates the account status of a specific doctor (e.g., Pending -> Accepted/Rejected).
    Task<Result> UpdateDoctorStatusAsync(string doctorUserId, UpdateDoctorStatusRequest request, CancellationToken cancellationToken);

    // Retrieves a paginated list of patients based on filter criteria.
    Task<Result<PageinatedList<PatientAdminViewDto>>> GetPatientsAsync(AdminPatientFilter filter, CancellationToken cancellationToken);

    // Retrieves a paginated list of appointments based on filter criteria.
    Task<Result<PageinatedList<AppointmentAdminViewDto>>> GetAppointmentsAsync(AdminAppointmentFilter filter, CancellationToken cancellationToken);

    // Add other methods as needed for Reviews, Transactions, Settings, Reports etc.
    // Example:
    // Task<Result<PageinatedList<ReviewAdminViewDto>>> GetReviewsAsync(AdminReviewFilter filter, CancellationToken cancellationToken);
    // Task<Result<PageinatedList<TransactionAdminViewDto>>> GetTransactionsAsync(AdminTransactionFilter filter, CancellationToken cancellationToken);
}

