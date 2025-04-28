namespace CureFusion.Contracts.Doctor;

public record DoctorRegisterRequest
(
   string Specialization,
   string Bio,
   int YearsOfExperience
 );
