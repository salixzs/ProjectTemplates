using System.Net;
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
        Tags(Urls.Sandbox.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.Sandbox.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Throws exception on purpose.";
            swagger.Description = "Shows error handling behavior in API.";
            swagger.Response<string>((int)HttpStatusCode.OK, "Returns string if exception is not thrown (impossible).");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Returns 500 with Json error object with error details.");
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(_handler.ThrowExceptionOnPurpose(), cancellationToken);
}
