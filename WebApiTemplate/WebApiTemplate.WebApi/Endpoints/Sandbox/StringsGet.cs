using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class StringsGet : EndpointWithoutRequest<StringsResponse>
{
    public override void Configure()
    {
        Get(Urls.Sandbox.Strings);
        Options(opts => opts.WithTags("Sandbox"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new StringsResponse(), cancellationToken);
}
