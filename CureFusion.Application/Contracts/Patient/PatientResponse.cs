namespace CureFusion.Application.Contracts.Patient;

public record PatientResponse

 (
     int Id,
     string Name,
     string ChronicDisease,
     string CurrentMedication,
     int Phone,
     string Gender
    );

