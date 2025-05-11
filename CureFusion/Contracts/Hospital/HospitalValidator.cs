namespace CureFusion.Contracts.Hospital;

public class HospitalValidator:AbstractValidator<HospitalRequest>
{
    public HospitalValidator()
    {
            RuleFor(x => x.Zone)
                .NotEmpty()
                .WithMessage("Zone is required.")
                .Matches(@"^[A-Za-z0-9\s]+$")
                .WithMessage("Zone can only contain letters, numbers, and spaces.");
    }
}
