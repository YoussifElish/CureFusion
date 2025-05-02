namespace CureFusion.Contracts.Medicine;

public record DrugRequest
(
 string Dosage,
 string Name ,
 string SideEffect,
 string Interaction,
 string Description
);