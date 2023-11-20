using WebApiTemplate.CoreLogic.Handlers.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class ExceptionThrow(ISandboxHandler handler) : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        // Endpoint setup (behaviour)
        Get(Urls.Sandbox.Exception);
        Tags(Urls.Sandbox.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.Sandbox.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Throws exception on purpose.";
            swagger.Description = "Shows error handling behaviour in API.";
            swagger.Response<string>((int)HttpStatusCode.OK, "Returns string if exception is not thrown (impossible).");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Returns 500 with Json error object with error details.");
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(handler.ThrowExceptionOnPurpose(), cancellationToken);
}
