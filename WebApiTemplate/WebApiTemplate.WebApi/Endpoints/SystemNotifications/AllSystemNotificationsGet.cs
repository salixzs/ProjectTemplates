using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Domain.Fakes;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class AllSystemNotificationsGet : EndpointWithoutRequest<List<SystemNotification>>
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
            swagger.Description = "Returns all system notifications in full contract, including expired together with their messages.";
            swagger.Response<List<SystemNotification>>(
                (int)HttpStatusCode.OK,
                "Returns existing notification(s) or empty list if no notifications exist.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during data retrieval.");
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
            var example = DomainFakesFactory.Instance.GetTestObject<SystemNotification>();
            example.Messages.Add(DomainFakesFactory.Instance.GetTestObject<SystemNotificationMessage>());
            swagger.ResponseExamples[(int)HttpStatusCode.OK] = example;
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var allNotifications = await _queryHandler.GetAll(cancellationToken);
        await SendOkAsync(allNotifications, cancellationToken);
    }
}
