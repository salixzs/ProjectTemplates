using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Domain.Fakes;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class ActiveSystemNotificationsGet : EndpointWithoutRequest<List<ActiveSystemNotification>>
{
    private readonly ISystemNotificationQueries _queryHandler;

    public ActiveSystemNotificationsGet(ISystemNotificationQueries queryHandler) => _queryHandler = queryHandler;

    public override void Configure()
    {
        Get(Urls.SystemNotifications.BaseUri);
        Tags(Urls.SystemNotifications.SwaggerTag);
        AllowAnonymous();
        DontCatchExceptions();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemNotifications.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Returns ACTIVE system notifications.";
            swagger.Description =
            "Returns ONLY ACTIVE system notifications transformed for fast display on UI. Empty list if no active notification exist.";
            swagger.Response<List<ActiveSystemNotification>>(
                (int)HttpStatusCode.OK,
                "Returns active notification(s) or empty list if no active notifications found.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during data retrieval.");
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
            var domainFakes = new DomainFakesFactory();
            var example = domainFakes.GetTestObject<ActiveSystemNotification>();
            example.Messages.Add(domainFakes.GetTestObject<SystemNotificationMessage>());
            swagger.ResponseExamples[(int)HttpStatusCode.OK] = example;
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var activeNotifications = await _queryHandler.GetActive(cancellationToken);
        await SendOkAsync(activeNotifications, cancellationToken);
    }
}
