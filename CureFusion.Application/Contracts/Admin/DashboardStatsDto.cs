// DTO for displaying dashboard statistics.

namespace CureFusion.Application.Contracts.Admin;

public record DashboardStatsDto(
    int TotalDoctors,
    int PendingDoctors,
    int TotalPatients,
    int TotalAppointments,
    decimal TotalRevenue // Assuming revenue calculation is needed
);

