using CureFusion.Enums;

namespace CureFusion.Entities;

public class PatientAppointment
{
    public int Id { get; set; }

    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; }

    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public DateTime? StatusChangedAt { get; set; }

    public string Notes { get; set; }
}
