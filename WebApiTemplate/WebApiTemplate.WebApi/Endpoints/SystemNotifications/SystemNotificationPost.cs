using System.Net;
using FluentValidation.Results;
using Salix.AspNetCore.JsonExceptionHandler;
using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class SystemNotificationPost : Endpoint<SystemNotification>
{
    private readonly ISystemNotificationCommands _commandHandler;

    public SystemNotificationPost(ISystemNotificationCommands commandHandler) => _commandHandler = commandHandler;

    public override void Configure()
    {
        Post(Urls.SystemNotifications.BaseUri);
        Tags(Urls.SystemNotifications.SwaggerTag);
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
            swagger.Response<List<ActiveSystemNotification>>((int)HttpStatusCode.Created, "System notification is sucessfully created.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occured in server during notification create.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Request has validation errors.");
        });
    }

    public override async Task HandleAsync(SystemNotification notification, CancellationToken cancellationToken)
    {
        EndpointHelpers.ThrowIfRequestValidationFailed(ValidationFailed, ValidationFailures, GetType().Name);
        await _commandHandler.Create(notification, cancellationToken);
        await SendCreatedAtAsync<ActiveSystemNotificationsGet>(12, 13, Http.GET, cancellation: cancellationToken);
    }
}

