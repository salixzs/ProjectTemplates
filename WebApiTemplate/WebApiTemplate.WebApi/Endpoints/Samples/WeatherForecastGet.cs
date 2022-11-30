using WebApiTemplate.CoreLogic.Handlers.Samples;
using WebApiTemplate.Domain.Samples;

namespace WebApiTemplate.Endpoints.Samples;

public class WeatherForecastGet : EndpointWithoutRequest<IEnumerable<WeatherForecast>>
{
    private readonly IWeatherForecastHandler _handler;

    public WeatherForecastGet(IWeatherForecastHandler handler) => _handler = handler;

    public override void Configure()
    {
        Get(Urls.Samples.WeatherForecast);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        SendOkAsync(await _handler.Handle(), cancellationToken);
}
