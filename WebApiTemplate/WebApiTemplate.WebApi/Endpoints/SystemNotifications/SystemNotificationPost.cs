using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class SystemNotificationPost(ISystemNotificationCommands commandHandler) : Endpoint<SystemNotification, int>
{
    public override void Configure()
    {
        Post(Urls.SystemNotifications.BaseUri);
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
            swagger.Summary = "Create a new system notification.";
            swagger.Description = "Creates a data for new system notification.";
            swagger.Response<int>((int)HttpStatusCode.Created, "System notification is successfully created.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during notification create.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Request (submitted notification data) has validation errors.");
            swagger.ResponseExamples[(int)HttpStatusCode.Created] = 1025;
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
                Messages =
                [
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
                ]
            };
        });
    }

    public override async Task HandleAsync(SystemNotification notification, CancellationToken cancellationToken)
    {
        EndpointHelpers.ThrowIfRequestValidationFailed(ValidationFailed, ValidationFailures, GetType().Name);
        var newId = await commandHandler.Create(notification, cancellationToken);
        await SendCreatedAtAsync<SingleSystemNotificationGet>(routeValues: new { id = newId }, responseBody: newId, cancellation: cancellationToken);
    }
}
