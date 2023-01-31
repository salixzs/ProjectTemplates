using FluentValidation;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class SystemNotificationValidator : Validator<SystemNotification>
{
    public SystemNotificationValidator()
    {
        RuleFor(o => o.Messages)
            .NotEmpty()
            .WithMessage("Notification should have at least one message in any language!");
        RuleForEach(o => o.Messages)
            .ChildRules(msg =>
            {
                msg.RuleFor(x => x.Language).NotEmpty().Length(2).WithMessage("Language for message should be specified (2-letter code)");
                msg.RuleFor(x => x.Message).NotEmpty().MinimumLength(2).WithMessage("System notification message should actually contain a message.");
            });

        RuleFor(o => o.StartTime)
            .GreaterThan(DateTime.MinValue)
            .WithMessage("Start time should be specified!");
        RuleFor(o => o.EndTime)
            .GreaterThan(DateTime.MinValue)
            .WithMessage("End time should be specified!");
        RuleFor(o => o.StartTime)
            .Must((notification, startTime) => notification.EndTime > startTime)
            .WithMessage("End time should be greater than Start time!");
        RuleFor(o => o.EmphasizeSince)
            .Must((notification, emphasizeTime) => emphasizeTime >= notification.StartTime && emphasizeTime <= notification.EndTime)
            .When(o => o.EmphasizeSince.HasValue)
            .WithMessage("Making emphasize should be within start and end times!");
        RuleFor(o => o.CountdownSince)
            .Must((notification, countdownTime) => countdownTime >= notification.StartTime && countdownTime <= notification.EndTime)
            .When(o => o.CountdownSince.HasValue)
            .WithMessage("Making emphasize should be within start and end times!");
        RuleFor(o => o.EmphasizeType)
            .NotNull()
            .When(o => o.EmphasizeSince.HasValue)
            .WithMessage("Emphasize type must be specified when emphasize time is given!");
    }
}
