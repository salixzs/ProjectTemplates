using FluentValidation;
using WebApiTemplate.Domain.SystemFeedbacks;
using WebApiTemplate.Translations;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbackCommentPostValidator : Validator<SystemFeedbackComment>
{
    public SystemFeedbackCommentPostValidator(ITranslate<ValidationTranslations> l10n)
    {
        RuleFor(o => o.Content)
            .NotEmpty()
            .WithMessage(l10n[nameof(ValidationTranslations.StringCannotBeNullOrEmpty)]);

        RuleFor(o => o.SystemFeedbackId)
            .NotEmpty()
            .WithMessage(l10n[nameof(ValidationTranslations.Id_MustBeSupplied)]);
    }
}
