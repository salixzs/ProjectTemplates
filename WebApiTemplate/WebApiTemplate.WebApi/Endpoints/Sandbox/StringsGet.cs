using System.Net;
using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class StringsGet : EndpointWithoutRequest<StringsResponse>
{
    public override void Configure()
    {
        Get(Urls.Sandbox.Strings);
        Tags(Urls.Sandbox.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.Sandbox.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Display String and Char object serialization.";
            swagger.Description = "Returns object with static and dynamic text (string, char) with national characters serialized as JSON.";
            swagger.Response<StringsResponse>((int)HttpStatusCode.OK, "Returns strings.");
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new StringsResponse(), cancellationToken);
}
