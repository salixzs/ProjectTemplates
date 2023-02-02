namespace WebApiTemplate.Crosscut.Services;

/// <inheritdoc/>
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
