namespace WebApiTemplate.Domain.Sandbox;

/// <summary>
/// Several types of Dates and Times in .Net
/// </summary>
public class DateTimeResponse
{
    /// <summary>
    /// Current DateTime in Server set Local timezone.
    /// </summary>
    public DateTime LocalCurrentDateTimeValue { get; set; } = DateTime.Now;

    /// <summary>
    /// Current DateTime in UTC.
    /// </summary>
    public DateTime UtcCurrentDateTimeValue { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Current DateTimeOffset value.
    /// </summary>
    public DateTimeOffset CurrentDateTimeOffsetValue { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Current DateTimeOffset UTC value (reused value from CurrentDateTimeOffsetValue).
    /// </summary>
    public DateTimeOffset CurrentDateTimeOffsetUtcValue => CurrentDateTimeOffsetValue.UtcDateTime;

    /// <summary>
    /// Static value of DateTime (2023-Jan-17 14:15:16)).
    /// </summary>
    public DateTime DateTimeValue { get; set; } = new DateTime(2023, 1, 17, 14, 15, 16, kind: DateTimeKind.Local);

    /// <summary>
    /// DateOnly type representation (1968-Apr-3).
    /// </summary>
    public DateOnly DateOnlyValue { get; set; } = new DateOnly(1968, 4, 3);

    /// <summary>
    /// TimeOnly represenatation (09:10:11).
    /// </summary>
    public TimeOnly TimeOnlyValue { get; set; } = new TimeOnly(9, 10, 11);

    /// <summary>
    /// TimeSpan value representation (1 min, 2 secs).
    /// </summary>
    public TimeSpan TimeSpanValue { get; set; } = new TimeSpan(0, 1, 2);
}
