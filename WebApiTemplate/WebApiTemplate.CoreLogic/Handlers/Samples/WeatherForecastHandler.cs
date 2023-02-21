using System.Diagnostics.CodeAnalysis;
using WebApiTemplate.Domain.Samples;

namespace WebApiTemplate.CoreLogic.Handlers.Samples;

[ExcludeFromCodeCoverage]
public class WeatherForecastHandler : IWeatherForecastHandler
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IEnumerable<WeatherForecast>> Handle() =>
        Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
        .ToList();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
