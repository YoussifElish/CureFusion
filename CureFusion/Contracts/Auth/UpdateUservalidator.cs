namespace CureFusion.Contracts.Auth;

public class UpdateUservalidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUservalidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Valid email is required.");
        RuleFor(x => x.Roles)
            .NotEmpty()
            .WithMessage("Roles are required.")
            .Must(roles => roles.All(role => role == DefaultRoles.Admin || role == DefaultRoles.Member || role == DefaultRoles.Doctor))
            .WithMessage("Invalid role(s) provided.")
            .Must(roles => roles.Distinct().Count() == roles.Count);
    }
}

