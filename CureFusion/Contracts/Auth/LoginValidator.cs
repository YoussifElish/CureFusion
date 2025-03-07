namespace CureFusion.Contracts.Auth;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x=>x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x=>x.Password).NotEmpty();
    }
}
