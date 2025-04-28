    using CureFusion.Abstactions.Consts;

    namespace CureFusion.Contracts.Authentication
    {
        public class RegisterRequestValidator : AbstractValidator<Auth.RegisterRequest>
        {
            public RegisterRequestValidator()
            {
            RuleFor(x => x.ConfirmPassword)
               .NotEmpty().WithMessage("Confirm Password is required")
               .Must((model, confirmPassword) => confirmPassword == model.Password)
               .WithMessage("Password and Confirm Password must match");
        }
        }
    }
