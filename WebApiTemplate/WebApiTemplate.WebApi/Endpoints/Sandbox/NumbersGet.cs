using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class NumbersGet : EndpointWithoutRequest<NumbersResponse>
{
    public override void Configure()
    {
        // Endpoint setup (behavior)
        Get(Urls.Sandbox.Numbers);
        Tags(Urls.Sandbox.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.Sandbox.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Display numerics object serialization.";
            swagger.Description = "Returns object with static values of .Net numeric types serialized into JSON.";
            swagger.Response<NumbersResponse>((int)HttpStatusCode.OK, "Returns numbers.");
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new NumbersResponse(), cancellationToken);
}
