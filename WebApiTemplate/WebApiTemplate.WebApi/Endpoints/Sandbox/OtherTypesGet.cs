using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class OtherTypesGet : EndpointWithoutRequest<OtherTypesResponse>
{
    public override void Configure()
    {
        Get(Urls.Sandbox.OtherTypes);
        Options(opts => opts.WithTags("Sandbox"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new OtherTypesResponse(), cancellationToken);
}
