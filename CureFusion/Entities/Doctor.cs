using CureFusion.Enums;

namespace CureFusion.Entities;

public class Doctor 
{
    public int Id { get; set; }
    public ApplicationUser User { get; set; }
    public string UserId { get; set; }
    public string Specialization { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public bool IsActive { get; set; } = true;
    public AccountStatus accountStatus  { get; set; } = AccountStatus.Pending;
    public string CertificationDocumentUrl { get; set; } = string.Empty;
    public double Rating { get; set; } = 0.0;
    public int TotalReviews { get; set; } = 0;

}
