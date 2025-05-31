using FluentValidation;

namespace CureFusion.Application.Contracts.Appointment;

public class PatientAppointmentRequestValidator : AbstractValidator<PatientAppointmentRequest>
{
    public PatientAppointmentRequestValidator()
    {
        RuleFor(x => x.AppointmentId).NotNull()
            .GreaterThan(0)
            .WithMessage("Appointment ID must be a positive number.");



        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters.");
    }
}