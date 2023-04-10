using WebApiTemplate.Domain.Sandbox;

namespace WebApiTemplate.WebApi.Endpoints.Sandbox;

public class LocalizationGet : EndpointWithoutRequest<LocalizationResponse>
{
    public override void Configure()
    {
        Get(Urls.Sandbox.Localization);
        Tags(Urls.Sandbox.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.Sandbox.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Display Localization abilities.";
            swagger.Description = "Returns object with localized content based on given Accept-Languages header in request.";
            swagger.Response<LocalizationResponse>((int)HttpStatusCode.OK, "Returns localized content.");
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(new LocalizationResponse(), cancellationToken);
}
