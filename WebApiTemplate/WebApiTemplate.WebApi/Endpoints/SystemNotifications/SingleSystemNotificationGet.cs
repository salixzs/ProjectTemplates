using System.Net;
using Salix.AspNetCore.JsonExceptionHandler;
using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class SingleSystemNotificationGet : EndpointWithoutRequest<SystemNotification>
{
    private readonly ISystemNotificationQueries _queryHandler;

    public SingleSystemNotificationGet(ISystemNotificationQueries queryHandler) => _queryHandler = queryHandler;

    public override void Configure()
    {
        Get(Urls.SystemNotifications.GetById);
        Tags(Urls.SystemNotifications.SwaggerTag);
        AllowAnonymous();
        DontCatchExceptions();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemNotifications.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Returns system notification by its Id.";
            swagger.Description = "Returns full data of system notification by given ID value.";
            swagger.Response<List<ActiveSystemNotification>>((int)HttpStatusCode.OK, "Returns active notification(s) or empty list if no active notifications found.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occured in server during data retrieval.");
            swagger.ResponseExamples[200] = new List<ActiveSystemNotification> {
                new ActiveSystemNotification {
                    Id = 1001,
                    ShowCountdown = false,
                    IsEmphasized = true,
                    MessageType = Enumerations.SystemNotificationType.Warning,
                    EndTime = DateTime.UtcNow.AddMinutes(21),
                    Messages = new List<SystemNotificationMessage> { new SystemNotificationMessage { Language = "en", Message = "System maintenance will start." } }
                }
            };
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        int notificationId = Route<int>("id");
        var activeNotifications = await _queryHandler.GetActive(cancellationToken);
        await SendOkAsync(new SystemNotification(), cancellationToken);
    }
}

