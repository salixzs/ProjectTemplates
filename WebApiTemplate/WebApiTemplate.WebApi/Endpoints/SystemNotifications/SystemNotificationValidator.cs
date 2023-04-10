using FluentValidation;
using WebApiTemplate.Domain.SystemNotifications;
using WebApiTemplate.Translations;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class SystemNotificationValidator : Validator<SystemNotification>
{
    public SystemNotificationValidator(ITranslate<ValidationTranslations> l10n)
    {
        RuleFor(o => o.Messages)
            .NotEmpty()
            .WithMessage(l10n[nameof(ValidationTranslations.SystemNotification_NoMessages)]);
        RuleForEach(o => o.Messages)
            .ChildRules(msg =>
            {
                msg.RuleFor(x => x.Language).NotEmpty().WithMessage(l10n[nameof(ValidationTranslations.StringCannotBeNullOrEmpty)]);
                msg.RuleFor(x => x.Language).Length(2).WithMessage(l10n[nameof(ValidationTranslations.SystemNotification_Language)]);
                msg.RuleFor(x => x.Message).NotEmpty().WithMessage(l10n[nameof(ValidationTranslations.StringCannotBeNullOrEmpty)]);
                msg.RuleFor(x => x.Message).MinimumLength(2).WithMessage(l10n[nameof(ValidationTranslations.StringLengthGreaterThan), 2]);
            });

        RuleFor(o => o.StartTime)
            .GreaterThan(DateTimeOffset.UtcNow.AddYears(-3))
            .WithMessage(l10n[nameof(ValidationTranslations.DateTimeIsNotCorrect)]);
        RuleFor(o => o.EndTime)
            .GreaterThan(DateTimeOffset.UtcNow.AddYears(-3))
            .WithMessage(l10n[nameof(ValidationTranslations.DateTimeIsNotCorrect)]);
        RuleFor(o => o.StartTime)
            .Must((notification, startTime) => notification.EndTime > startTime)
            .WithMessage(l10n[nameof(ValidationTranslations.SystemNotification_StartDateLessThanEndDate)]);
        RuleFor(o => o.EmphasizeSince)
            .Must((notification, emphasizeTime) => emphasizeTime >= notification.StartTime && emphasizeTime <= notification.EndTime)
            .When(o => o.EmphasizeSince.HasValue)
            .WithMessage(l10n[nameof(ValidationTranslations.SystemNotification_EmphasizeTime)]);
        RuleFor(o => o.CountdownSince)
            .Must((notification, countdownTime) => countdownTime >= notification.StartTime && countdownTime <= notification.EndTime)
            .When(o => o.CountdownSince.HasValue)
            .WithMessage(l10n[nameof(ValidationTranslations.SystemNotification_CountdownTime)]);
        RuleFor(o => o.EmphasizeType)
            .NotNull()
            .When(o => o.EmphasizeSince.HasValue)
            .WithMessage(l10n[nameof(ValidationTranslations.SystemNotification_EmphasizeType)]);
    }
}
