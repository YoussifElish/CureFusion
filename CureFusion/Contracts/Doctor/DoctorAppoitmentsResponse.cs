namespace CureFusion.Contracts.Doctor;

public class DoctorAppoitmentsResponse
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string doctorName { get; set; }
    
    public string Specialization { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public double Rating { get; set; } = 0.0;
    public int TotalReviews { get; set; } = 0;
    public string ProfileImagePath { get; set; } 

}
