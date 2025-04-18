using CureFusion.Enums;

namespace CureFusion.Entities;

public class DoctorAppoitment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public DateTime AvailableAt { get; set; }
    public int DurationInMinutes { get; set; }
    public decimal Price { get; set; }
    public AppointmentType SessionMode { get; set; } 
    public bool IsBooked { get; set; } = false;

    public string? Notes { get; set; }


}
