using System.Diagnostics.CodeAnalysis;

namespace WebApiTemplate.Domain.Sandbox;

/// <summary>
/// Object with several .Net types regarding numbers.
/// </summary>
[ExcludeFromCodeCoverage]
public class NumbersResponse
{
    /// <summary>
    /// Integer value (43689).
    /// </summary>
    public int IntegerValue { get; set; } = 43689;

    /// <summary>
    /// Decimal value (3231.932)
    /// </summary>
    public decimal DecimalValue { get; set; } = 3231.932M;

    /// <summary>
    /// Negative decimal value (-823.26)
    /// </summary>
    public decimal NegativeDecimalValue { get; set; } = -823.26M;

    /// <summary>
    /// Double value (8921347.3252).
    /// </summary>
    public double DoubleValue { get; set; } = 8921347.3252;

    /// <summary>
    /// Float value (4.5753935).
    /// </summary>
    public float FloatValue { get; set; } = 4.5753935f;
}
