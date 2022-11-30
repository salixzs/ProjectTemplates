using WebApiTemplate.Domain.Samples;

namespace WebApiTemplate.CoreLogic.Handlers.Samples;

public interface IWeatherForecastHandler
{
    Task<IEnumerable<WeatherForecast>> Handle();
}
