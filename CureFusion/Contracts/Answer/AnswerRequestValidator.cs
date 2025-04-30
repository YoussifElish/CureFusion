namespace CureFusion.Contracts.Answer;

public class AnswerRequestValidator :AbstractValidator<AnswerRequest>
{
    public AnswerRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(1000)
            .WithMessage("Content must not exceed 1000 characters.");

   
    }
}

