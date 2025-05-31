using FluentValidation;

namespace CureFusion.Application.Contracts.Question;

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(1000)
            .WithMessage("Content must not exceed 1000 characters.");
    }
}
