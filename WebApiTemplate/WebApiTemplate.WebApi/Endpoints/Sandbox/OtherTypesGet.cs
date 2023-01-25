using System.Net;
using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class OtherTypesGet : EndpointWithoutRequest<OtherTypesResponse>
{
    public override void Configure()
    {
        // Endpoint setup (behavior)
        Get(Urls.Sandbox.OtherTypes);
        Tags(Urls.Sandbox.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.Sandbox.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Display bool, enum and arrays serialization.";
            swagger.Description = "Returns object with static values of boolean, enum and array .Net types serialized into JSON.";
            swagger.Response<OtherTypesResponse>((int)HttpStatusCode.OK, "Returns other types.");
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new OtherTypesResponse(), cancellationToken);
}
