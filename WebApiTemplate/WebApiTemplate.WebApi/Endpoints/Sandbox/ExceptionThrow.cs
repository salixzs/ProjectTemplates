using WebApiTemplate.CoreLogic.Handlers.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class ExceptionThrow : EndpointWithoutRequest<string>
{
    private readonly ISandboxHandler _handler;

    public ExceptionThrow(ISandboxHandler handler) => _handler = handler;

    public override void Configure()
    {
        Get(Urls.Sandbox.Exception);
        Options(opts => opts.WithTags("Sandbox"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(_handler.ThrowExceptionOnPurpose(), cancellationToken);
}
