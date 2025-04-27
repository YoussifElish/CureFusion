namespace CureFusion.Contracts.Medicine;

public record DrugResponse
(
 int Id,
 int Dosage,
 string Name,
 string SideEffect,
 string Interaction
);
