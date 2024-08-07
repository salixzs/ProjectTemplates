using WebApiTemplate.CoreLogic.Handlers.Samples;
using WebApiTemplate.Domain.Samples;

namespace WebApiTemplate.WebApi.Endpoints.Samples;

public class WeatherForecastGet(IWeatherForecastHandler handler) : EndpointWithoutRequest<IEnumerable<WeatherForecast>>
{
    public override void Configure()
    {
        // Endpoint setup (behaviour)
        Get(Urls.Samples.WeatherForecast);
        Tags(Urls.Samples.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.Samples.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Classic VS template sample of weather data.";
            swagger.Description = "Returns list of objects with random weather data.";
            swagger.Response<WeatherForecast>((int)HttpStatusCode.OK, "Returns short list of weather data.");
            swagger.ResponseExamples[200] = new WeatherForecast { Date = new DateOnly(2023, 1, 12), TemperatureC = 12, Summary = "Chilly" };
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(await handler.Handle(), cancellationToken);
}
