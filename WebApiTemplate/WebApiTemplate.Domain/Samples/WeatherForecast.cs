using System.Diagnostics.CodeAnalysis;

namespace WebApiTemplate.Domain.Samples;

/// <summary>
/// Weather data for specified date.
/// </summary>
[ExcludeFromCodeCoverage]
public class WeatherForecast
{
    /// <summary>
    /// Date of weather.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Temperature in celsius (C).
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Temperature in Farenheiot (F).
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Description of weather.
    /// </summary>
    public string? Summary { get; set; }
}
