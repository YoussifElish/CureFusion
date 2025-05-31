// DTO for displaying doctor information in the admin list.
using CureFusion.Domain.Enums;

namespace CureFusion.Application.Contracts.Admin;

public record DoctorAdminViewDto(
    string UserId,
    string DoctorName, // Combine FirstName and LastName from ApplicationUser
    string Specialization,
    AccountStatus Status,
    decimal Earned, // This might require calculation based on appointments/transactions
    double Rating // From Doctor entity
);

