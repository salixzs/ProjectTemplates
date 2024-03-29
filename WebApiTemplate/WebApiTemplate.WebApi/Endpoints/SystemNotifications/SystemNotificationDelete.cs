using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class SystemNotificationDelete(ISystemNotificationCommands commandHandler) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete(Urls.SystemNotifications.WithId);
        Tags(Urls.SystemNotifications.SwaggerTag);
        AllowAnonymous();
        DontCatchExceptions();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemNotifications.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Deletes the system notification.";
            swagger.Description = "Deletes the system notification together with its messages.";
            swagger.Response<int>((int)HttpStatusCode.NoContent, "System notification is successfully deleted.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during notification delete.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Id is not provided.");
            swagger.ResponseExamples[(int)HttpStatusCode.UnprocessableEntity] = EndpointHelpers.ExampleApiValidationError();
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var notificationId = Route<int>("id");
        await commandHandler.Delete(notificationId, cancellationToken);
        await SendNoContentAsync(cancellationToken);
    }
}
