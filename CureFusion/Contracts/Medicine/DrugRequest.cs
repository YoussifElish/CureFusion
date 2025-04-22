namespace CureFusion.Contracts.Medicine;

public record DrugRequest
(
 int Dosage,
 string Name ,
 string SideEffect,
 string Interaction 
);