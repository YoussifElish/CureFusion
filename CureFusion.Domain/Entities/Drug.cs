namespace CureFusion.Domain.Entities;

public class Drug
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SideEffect { get; set; } = string.Empty;
    public string Dosage { get; set; }
    public string Interaction { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public UploadedFile DrugImage { get; set; }
    public Guid? DrugImageId { get; set; }

}
