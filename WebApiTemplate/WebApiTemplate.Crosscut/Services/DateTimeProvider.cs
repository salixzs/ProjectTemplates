using System.Diagnostics.CodeAnalysis;

namespace WebApiTemplate.Crosscut.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage(Justification = "One-liners")]
public class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc/>
    public DateTimeOffset DateTimeOffsetNow => DateTimeOffset.Now;

    /// <inheritdoc/>
    public DateTimeOffset DateTimeOffsetUtcNow => DateTimeOffset.UtcNow;

    /// <inheritdoc/>
    public DateTime DateTimeNow => DateTime.Now;

    /// <inheritdoc/>
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
}
