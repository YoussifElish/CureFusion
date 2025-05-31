namespace CureFusion.Application.Contracts.Auth
{
    public record RegisterAsDoctorRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        DateOnly DOB,
         string Specialization,
   string Bio,
   int YearsOfExperience

        );
}
