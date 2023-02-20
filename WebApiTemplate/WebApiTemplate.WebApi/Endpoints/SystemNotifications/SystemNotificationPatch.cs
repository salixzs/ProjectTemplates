using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class SystemNotificationPatch : Endpoint<SystemNotification>
{
    private readonly ISystemNotificationCommands _commandHandler;

    public SystemNotificationPatch(ISystemNotificationCommands commandHandler) => _commandHandler = commandHandler;

    public override void Configure()
    {
        Patch(Urls.SystemNotifications.BaseUri);
        Tags(Urls.SystemNotifications.SwaggerTag);
        Validator<SystemNotificationValidator>();
        AllowAnonymous();

        // This disables FastEndpoints built in error handlers (We have our own).
        DontCatchExceptions();
        DontThrowIfValidationFails();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemNotifications.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Updates a system notification.";
            swagger.Description = @"Updates a data for of an existing system notification with new data (properties),
including its messages. Missing messages gets deleted. Should have at least one message in data patched.";
            swagger.Response<int>((int)HttpStatusCode.Created, "System notification is successfully updated.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during notification update.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Request (submitted notification data) has validation errors.");
            swagger.ResponseExamples[(int)HttpStatusCode.OK] = 1025;
            swagger.ResponseExamples[(int)HttpStatusCode.UnprocessableEntity] = EndpointHelpers.ExampleApiValidationError();
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
            swagger.ExampleRequest = new SystemNotification
            {
                Id = 0,
                StartTime = DateTime.UtcNow.AddSeconds(15),
                EndTime = DateTime.UtcNow.AddMinutes(11),
                EmphasizeSince = DateTime.UtcNow.AddMinutes(6),
                CountdownSince = DateTime.UtcNow.AddMinutes(9),
                EmphasizeType = Enumerations.SystemNotificationType.Critical,
                Type = Enumerations.SystemNotificationType.Warning,
                Messages = new List<SystemNotificationMessage>
                {
                    new SystemNotificationMessage
                    {
                        Id = 0,
                        Language = "en",
                        Message = "Maintenance will happen soon."
                    },
                    new SystemNotificationMessage
                    {
                        Id = 0,
                        Language = "lv",
                        Message = "Drīz sāksies apkopes darbi."
                    }
                }
            };
        });
    }

    public override async Task HandleAsync(SystemNotification notification, CancellationToken cancellationToken)
    {
        if (notification.Id == 0)
        {
            AddError("Request should have an ID of existing system notification.");
        }

        EndpointHelpers.ThrowIfRequestValidationFailed(ValidationFailed, ValidationFailures, GetType().Name);
        await _commandHandler.Update(notification, cancellationToken);
        await SendOkAsync(cancellation: cancellationToken);
    }
}
