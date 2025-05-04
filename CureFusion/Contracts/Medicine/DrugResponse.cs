namespace CureFusion.Contracts.Medicine;

public record DrugResponse
    (
 int Id ,
  string Name ,
 string Dosage ,
    string Interaction ,
 string SideEffect ,
  string Description,
 string? DrugImage
);
