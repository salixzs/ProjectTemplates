using FluentValidation;
using WebApiTemplate.Domain.SystemFeedbacks;
using WebApiTemplate.Translations;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbackCommentPatchValidator : Validator<SystemFeedbackComment>
{
    public SystemFeedbackCommentPatchValidator(ITranslate<ValidationTranslations> l10n)
    {
        RuleFor(o => o.Content)
            .NotEmpty()
            .WithMessage(l10n[nameof(ValidationTranslations.StringCannotBeNullOrEmpty)]);

        RuleFor(o => o.Id)
            .NotEmpty()
            .WithMessage(l10n[nameof(ValidationTranslations.Id_MustBeSupplied)]);
    }
}
