using FluentValidation;

namespace CureFusion.Application.Contracts.Auth;

public class LoginValidator : AbstractValidator<CureFusion.Application.Contracts.Auth.Loginrequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Password).NotEmpty();
    }
}
