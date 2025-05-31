// DTO for displaying detailed doctor information for admin review.
// Updated to include StoredFileNames instead of FileDto.
using CureFusion.Domain.Enums;

namespace CureFusion.Application.Contracts.Admin;

public record DoctorDetailsDto(
    string UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Specialization,
    string Bio,
    int YearsOfExperience,
    AccountStatus Status,
    string? ProfileImageStoredName, // Stored file name for the profile image
    string? CertificationDocumentStoredName // Stored file name for the certification document
);







