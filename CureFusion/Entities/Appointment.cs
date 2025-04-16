using CureFusion.Enums;

namespace CureFusion.Entities;

public class Appointment : AuditableEntity
{
    public int Id { get; set; }
    public ApplicationUser User { get; set; }
    public string UserId { get; set; } 
    public AppointmentType AppointmentType { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime? StatusChangedAt { get; set; } // when the session status changed for example when it changed from pending to confirmed
    public int DurationInMinutes { get; set; }   // We will add a feature allow to the patient to select session duration for example if session duration is 30 minute it will be for 50$ and if 60 minute will be for 100 $ , etc
    public string Notes { get; set; }

}
