namespace CureFusion.Entities;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ChronicDisease { get; set; } = string.Empty;
    public string CurrentMedication { get; set; } = string.Empty;
}
