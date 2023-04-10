using FluentValidation;
using WebApiTemplate.Domain.SystemFeedback;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class SystemFeedbackValidator : Validator<SystemFeedback>
{
    public SystemFeedbackValidator()
    {
        RuleFor(o => o.Title)
            .NotEmpty()
            .WithMessage("Title must be specified!");

        RuleFor(o => o.Priority)
            .NotEqual(Enumerations.SystemFeedbackPriority.NotSpecified)
            .WithMessage("Priority must be set!");
    }
}
