// DTO for filtering the admin doctor list.
using CureFusion.Contracts.Common;
using CureFusion.Enums;

namespace CureFusion.Contracts.Admin;

public class AdminDoctorFilter : RequestFilter
{
    // Add specific filter properties for doctors if needed, e.g.:
    public AccountStatus? StatusFilter { get; set; }
    public string? SpecializationFilter { get; set; }
}

