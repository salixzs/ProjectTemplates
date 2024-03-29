using FluentValidation;
using WebApiTemplate.Domain.SystemFeedbacks;
using WebApiTemplate.Translations;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbackValidator : Validator<SystemFeedback>
{
    public SystemFeedbackValidator(ITranslate<ValidationTranslations> l10n)
    {
        RuleFor(o => o.Title)
            .NotEmpty()
            .WithMessage(l10n[nameof(ValidationTranslations.StringCannotBeNullOrEmpty)]);

        RuleFor(o => o.Priority)
            .NotEqual(Enumerations.SystemFeedbackPriority.NotSpecified)
            .WithMessage(l10n[nameof(ValidationTranslations.SystemFeedback_Priority)]);
    }
}
