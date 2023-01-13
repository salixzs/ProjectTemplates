using Salix.AspNetCore.JsonExceptionHandler;
using WebApiTemplate.CoreLogic.Handlers.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class ExceptionThrow : EndpointWithoutRequest<string>
{
    private readonly ISandboxHandler _handler;

    public ExceptionThrow(ISandboxHandler handler) => _handler = handler;

    public override void Configure()
    {
        // Endpoint setup (behavior)
        Get(Urls.Sandbox.Exception);
        Tags("Sandbox");
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags("Sandbox")
            .Produces<ApiError>(500, "application/json+problem"));
        Summary(swagger =>
        {
            swagger.Summary = "Throws exception on purpose.";
            swagger.Description = "Shows error handling behavior in API.";
            swagger.Responses[200] = "Returns string if exception is not thrown (impossible).";
            swagger.Responses[500] = "Returns exception/error with details.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(_handler.ThrowExceptionOnPurpose(), cancellationToken);
}
