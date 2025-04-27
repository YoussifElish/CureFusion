namespace CureFusion.Contracts.Medicine;

public class DrugRequestValidator : AbstractValidator<DrugRequest>
{
    public DrugRequestValidator()
    {
        RuleFor(drug => drug.Name)
        .NotEmpty()
        .MaximumLength(100);

        RuleFor(drug=>drug.Interaction)
            .MaximumLength(100);
    }
}
