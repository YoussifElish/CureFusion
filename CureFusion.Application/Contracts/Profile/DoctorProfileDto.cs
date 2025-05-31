using CureFusion.Domain.Enums;

namespace CureFusion.Application.Contracts.Profile;

public class DoctorProfileDto : IProfileDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Specialization { get; set; }
    public int YearsOfExperience { get; set; }
    public double Rating { get; set; }
    public int TotalReviews { get; set; }
    public string Bio { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? CertificationDocumentUrl { get; set; }
    public int TotalAnswers { get; set; }
    public int TotalAppointments { get; set; }
    public int CompletedAppointments { get; set; }
    public int UpcomingAppointments { get; set; }
}