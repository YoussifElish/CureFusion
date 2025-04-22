using CureFusion.Enums;

namespace CureFusion.Entities;

public class DoctorAvailability
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public DateTime Date { get; set; }              
    public TimeSpan From { get; set; }               
    public TimeSpan To { get; set; }                
    public int SlotDurationInMinutes { get; set; }   
    public decimal PricePerSlot { get; set; }       
    public AppointmentType SessionMode { get; set; }
    public bool IsActive { get; set; } = true;
}
