using CureFusion.Application.Contracts.Doctor;
using FluentValidation;

public class DoctorAvailabilityRequestValidator : AbstractValidator<DoctorAvailabilityRequest>
{
    public DoctorAvailabilityRequestValidator()
    {
        RuleFor(x => x.From)
            .Must(x => x >= TimeSpan.Zero && x < TimeSpan.FromDays(1))
            .WithMessage("From time must be between 00:00:00 and 23:59:59");

        RuleFor(x => x.To)
            .Must(x => x >= TimeSpan.Zero && x < TimeSpan.FromDays(1))
            .WithMessage("To time must be between 00:00:00 and 23:59:59");

        RuleFor(x => x.To)
            .GreaterThan(x => x.From)
            .WithMessage("'To' time must be greater than 'From' time");
    }
}
