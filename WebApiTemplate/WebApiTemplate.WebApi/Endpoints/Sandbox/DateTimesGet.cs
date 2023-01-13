using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class DateTimesGet : EndpointWithoutRequest<DateTimeResponse>
{
    public override void Configure()
    {
        Get(Urls.Sandbox.DateTimes);
        Options(opts => opts.WithTags("Sandbox"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new DateTimeResponse(), cancellationToken);
}
