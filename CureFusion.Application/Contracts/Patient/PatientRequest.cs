namespace CureFusion.Application.Contracts.Patient;

public record PatientRequest
    (

 string Name,
 string ChronicDisease,
 string CurrentMedication,
 int Phone,
 string Gender
    );

