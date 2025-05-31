// DTO for updating a doctor's account status.
using System.ComponentModel.DataAnnotations;
using CureFusion.Domain.Enums;

namespace CureFusion.Application.Contracts.Admin;

public record UpdateDoctorStatusRequest(
    [Required]
    AccountStatus NewStatus,
    string? RejectionReason // Optional reason if status is Rejected
);

