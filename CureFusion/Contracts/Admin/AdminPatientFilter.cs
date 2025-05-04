// DTO for filtering the admin patient list.
using CureFusion.Contracts.Common;

namespace CureFusion.Contracts.Admin;

public class AdminPatientFilter : RequestFilter
{
    // Add specific filter properties for patients if needed, e.g.:
    // public DateTime? RegistrationDateFrom { get; set; }
    // public DateTime? RegistrationDateTo { get; set; }
}

