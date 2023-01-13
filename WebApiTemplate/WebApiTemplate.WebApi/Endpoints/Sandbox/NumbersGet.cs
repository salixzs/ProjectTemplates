using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class NumbersGet : EndpointWithoutRequest<NumbersResponse>
{
    public override void Configure()
    {
        // Endpoint setup (behavior)
        Get(Urls.Sandbox.Numbers);
        Tags("Sandbox");
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags("Sandbox"));
        Summary(swagger =>
        {
            swagger.Summary = "Display numerics object serialization.";
            swagger.Description = "Returns object with static values of .Net numeric types serialized into JSON.";
            swagger.Responses[200] = "Returns numbers.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new NumbersResponse(), cancellationToken);
}
