// DTO for filtering the admin appointment list.
using CureFusion.Application.Contracts.Common;

namespace CureFusion.Application.Contracts.Admin;

public class AdminAppointmentFilter : RequestFilter
{
    // Add specific filter properties for appointments if needed, e.g.:
    public string? DoctorNameFilter { get; set; }
    public string? PatientNameFilter { get; set; }
    public DateTime? AppointmentDateFrom { get; set; }
    public DateTime? AppointmentDateTo { get; set; }
    // Add status filter if appointments have statuses
}

