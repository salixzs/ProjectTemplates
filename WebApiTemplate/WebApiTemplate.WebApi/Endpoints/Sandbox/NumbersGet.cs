using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class NumbersGet : EndpointWithoutRequest<NumbersResponse>
{
    public override void Configure()
    {
        Get(Urls.Sandbox.Numbers);
        Options(opts => opts.WithTags("Sandbox"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new NumbersResponse(), cancellationToken);
}
