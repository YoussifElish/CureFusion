namespace CureFusion.Application.Contracts.Doctor;

public record DoctorRegisterRequest
(
   string Specialization,
   string Bio,
   int YearsOfExperience
 );
