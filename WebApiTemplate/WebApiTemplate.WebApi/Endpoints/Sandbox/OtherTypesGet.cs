using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class OtherTypesGet : EndpointWithoutRequest<OtherTypesResponse>
{
    public override void Configure()
    {
        // Endpoint setup (behavior)
        Get(Urls.Sandbox.OtherTypes);
        Tags("Sandbox");
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags("Sandbox"));
        Summary(swagger =>
        {
            swagger.Summary = "Display bool, enum adn arrays serialization.";
            swagger.Description = "Returns object with static values of boolean, enum and array .Net types serialized into JSON.";
            swagger.Responses[200] = "Returns other types.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new OtherTypesResponse(), cancellationToken);
}
