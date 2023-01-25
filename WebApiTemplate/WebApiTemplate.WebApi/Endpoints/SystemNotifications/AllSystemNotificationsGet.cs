using System.Net;
using Salix.AspNetCore.JsonExceptionHandler;
using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class AllSystemNotificationsGet : EndpointWithoutRequest<List<ActiveSystemNotification>>
{
    private readonly ISystemNotificationQueries _queryHandler;

    public AllSystemNotificationsGet(ISystemNotificationQueries queryHandler) => _queryHandler = queryHandler;

    public override void Configure()
    {
        Get(Urls.SystemNotifications.All);
        Tags(Urls.SystemNotifications.SwaggerTag);
        AllowAnonymous();
        DontCatchExceptions();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemNotifications.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Returns all system notifications.";
            swagger.Description = "Returns all system notifications in full contract, including expired.";
            swagger.Response<List<SystemNotification>>((int)HttpStatusCode.OK, "Returns existing notification(s) or empty list if no notifications exist.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during data retrieval.");
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var allNotifications = await _queryHandler.GetAll(cancellationToken);
        await SendOkAsync(allNotifications, cancellationToken);
    }
}

