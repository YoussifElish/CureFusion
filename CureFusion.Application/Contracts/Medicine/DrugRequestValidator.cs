using FluentValidation;

namespace CureFusion.Application.Contracts.Medicine;

public class DrugRequestValidator : AbstractValidator<DrugRequest>
{
    public DrugRequestValidator()
    {
        RuleFor(drug => drug.Name)
        .NotEmpty()
        .MaximumLength(100);

        RuleFor(drug => drug.Interaction)
            .MaximumLength(100);
        RuleFor(drug => drug.SideEffect)
            .MaximumLength(100);
        RuleFor(drug => drug.Dosage)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(drug => drug.Description)
            .NotEmpty()
            .MaximumLength(1500);

    }
}
