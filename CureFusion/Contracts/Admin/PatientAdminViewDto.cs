// DTO for displaying patient information in the admin list.
using System;

namespace CureFusion.Contracts.Admin;

public record PatientAdminViewDto(
    string UserId,
    string PatientName, // Combine FirstName and LastName from ApplicationUser
    string PhoneNumber,
    string Email,
    DateTime? LastVisit // This might require joining with Appointments
);

