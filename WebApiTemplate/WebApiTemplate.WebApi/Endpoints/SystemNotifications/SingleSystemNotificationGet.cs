using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Domain.Fakes;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class SingleSystemNotificationGet : EndpointWithoutRequest<SystemNotification>
{
    private readonly ISystemNotificationQueries _queryHandler;

    public SingleSystemNotificationGet(ISystemNotificationQueries queryHandler) => _queryHandler = queryHandler;

    public override void Configure()
    {
        Get(Urls.SystemNotifications.WithId);
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
            swagger.Response<SystemNotification>((int)HttpStatusCode.OK, "Returns requested system notification with full data contract.");
            swagger.Response((int)HttpStatusCode.NotFound, "System notification record by given ID was not found.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during data retrieval.");
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
            var example = DomainFakesFactory.Instance.GetTestObject<SystemNotification>();
            example.Messages.Add(DomainFakesFactory.Instance.GetTestObject<SystemNotificationMessage>());
            swagger.ResponseExamples[(int)HttpStatusCode.OK] = example;
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var notificationId = Route<int>("id");
        var notification = await _queryHandler.GetById(notificationId, cancellationToken);
        if (notification != null)
        {
            await SendOkAsync(notification, cancellationToken);
        }

        await SendNotFoundAsync(cancellationToken);
    }
}
