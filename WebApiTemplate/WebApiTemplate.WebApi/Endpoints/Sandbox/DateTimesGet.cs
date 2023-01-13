using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class DateTimesGet : EndpointWithoutRequest<DateTimeResponse>
{
    public override void Configure()
    {
        // Endpoint setup (behavior)
        Get(Urls.Sandbox.DateTimes);
        Tags("Sandbox");
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags("Sandbox"));
        Summary(swagger =>
        {
            swagger.Summary = "Display Data and Time object serialization.";
            swagger.Description = "Returns object with static and dynamic (current) dates, times, timespans and related .Net types serialized into JSON.";
            swagger.Responses[200] = "Returns Dates and Times.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new DateTimeResponse(), cancellationToken);
}
