using System.Net;
using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class DateTimesGet : EndpointWithoutRequest<DateTimeResponse>
{
    public override void Configure()
    {
        // Endpoint setup (behavior)
        Get(Urls.Sandbox.DateTimes);
        Tags(Urls.Sandbox.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.Sandbox.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Display Data and Time object serialization.";
            swagger.Description = "Returns object with static and dynamic (current) dates, times, timespans and related .Net types serialized into JSON.";
            swagger.Response<DateTimeResponse>((int)HttpStatusCode.OK, "Returns dates and times values.");
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new DateTimeResponse(), cancellationToken);
}
