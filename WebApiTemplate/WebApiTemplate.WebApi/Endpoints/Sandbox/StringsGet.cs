using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class StringsGet : EndpointWithoutRequest<StringsResponse>
{
    public override void Configure()
    {
        Get(Urls.Sandbox.Strings);
        Tags("Sandbox");
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags("Sandbox"));
        Summary(swagger =>
        {
            swagger.Summary = "Display String and Char object serialization.";
            swagger.Description = "Returns object with static and dynamic text (string, char) with national characters serialized as JSON.";
            swagger.Responses[200] = "Returns Strings.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new StringsResponse(), cancellationToken);
}
