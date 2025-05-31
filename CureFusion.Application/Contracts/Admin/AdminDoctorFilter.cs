// DTO for filtering the admin doctor list.
using CureFusion.Application.Contracts.Common;
using CureFusion.Domain.Enums;

namespace CureFusion.Application.Contracts.Admin;

public class AdminDoctorFilter : RequestFilter
{
    // Add specific filter properties for doctors if needed, e.g.:
    public AccountStatus? StatusFilter { get; set; }
    public string? SpecializationFilter { get; set; }
}

