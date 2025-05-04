// DTO for updating a doctor's account status.
using CureFusion.Enums;
using System.ComponentModel.DataAnnotations;

namespace CureFusion.Contracts.Admin;

public record UpdateDoctorStatusRequest(
    [Required]
    AccountStatus NewStatus,
    string? RejectionReason // Optional reason if status is Rejected
);

