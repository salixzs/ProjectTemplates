using System.Diagnostics.CodeAnalysis;
using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Domain.Sandbox;

/// <summary>
/// Several text types in .Net
/// </summary>
[ExcludeFromCodeCoverage]
public class LocalizationResponse
{
    /// <summary>
    /// Current Culture, set in application for given request.
    /// </summary>
    public string CurrentCulture { get; set; } = System.Globalization.CultureInfo.CurrentCulture.Name;

    /// <summary>
    /// Current UI Culture, set in application for given request.
    /// </summary>
    public string CurrentUiCulture { get; set; } = System.Globalization.CultureInfo.CurrentUICulture.Name;

    /// <summary>
    /// Humanized DateTime value.
    /// </summary>
    public string HumanizedDateTime { get; set; } = DateTime.Now.AddMinutes(-12).ToStringHuman(3);
}
