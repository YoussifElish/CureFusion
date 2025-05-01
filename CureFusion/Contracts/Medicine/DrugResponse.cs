namespace CureFusion.Contracts.Medicine;

public record DrugResponse
(
 int Id,
 string Dosage,
 string Name,
 string SideEffect,
 string Interaction,
 string Description,
 string? DrugImage
);
