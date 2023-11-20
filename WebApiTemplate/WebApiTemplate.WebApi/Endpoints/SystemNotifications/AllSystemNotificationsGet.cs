using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;
using WebApiTemplate.Domain.Fakes;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemNotifications;

public class AllSystemNotificationsGet(ISystemNotificationQueries queryHandler) : EndpointWithoutRequest<List<SystemNotification>>
{
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
            var domainFakes = new DomainFakesFactory();
            var example = domainFakes.GetTestObject<SystemNotification>();
            example.Messages.Add(domainFakes.GetTestObject<SystemNotificationMessage>());
            swagger.ResponseExamples[(int)HttpStatusCode.OK] = example;
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var allNotifications = await queryHandler.GetAll(cancellationToken);
        await SendOkAsync(allNotifications, cancellationToken);
    }
}
