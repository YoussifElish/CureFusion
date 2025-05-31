// DTO for displaying appointment information in the admin list.
namespace CureFusion.Application.Contracts.Admin;

public record AppointmentAdminViewDto(
    int AppointmentId,
    string DoctorName,
    string PatientName,
    string Specialization,
    DateTime AppointmentDateTime,
    string Status, // Assuming appointments have a status (e.g., Scheduled, Completed, Cancelled)
    decimal Amount // Assuming appointment has an associated cost/amount
);

