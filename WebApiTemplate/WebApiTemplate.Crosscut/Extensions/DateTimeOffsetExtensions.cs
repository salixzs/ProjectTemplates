namespace WebApiTemplate.Crosscut.Extensions;

public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Rounds DateTimeOffset value to nearest given hour/minute/second/millisecond.
    /// </summary>
    /// <param name="originalDateTimeOffset">Original DateTimeOffset value.</param>
    /// <param name="ticksType">Use <c>TimeSpan.TicksPerSecond</c>, <c>TimeSpan.TicksPerMinute</c> and similar.</param>
    /// <returns>Rounded DateTimeOffset value to nearest given ticks value.</returns>
    public static DateTimeOffset Round(this DateTimeOffset originalDateTimeOffset, long ticksType = TimeSpan.TicksPerSecond)
    {
        if (ticksType > 1)
        {
            var fraction = originalDateTimeOffset.Ticks % ticksType;
            if (fraction != 0)
            {
                if (fraction * 2 >= ticksType)
                {
                    // round up
                    return new DateTimeOffset(originalDateTimeOffset.Ticks + ticksType - fraction, originalDateTimeOffset.Offset);
                }

                // round down
                return new DateTimeOffset(originalDateTimeOffset.Ticks - fraction, originalDateTimeOffset.Offset);
            }
        }

        return originalDateTimeOffset;
    }

    /// <summary>
    /// Rounds DateTimeOffset value to nearest given hour/minute/second/millisecond.
    /// </summary>
    /// <param name="originalDateTimeOffset">Original DateTimeOffset value.</param>
    /// <param name="ticksType">Use <c>TimeSpan.TicksPerSecond</c>, <c>TimeSpan.TicksPerMinute</c> and similar.</param>
    /// <returns>Rounded DateTimeOffset value to nearest given ticks value.</returns>
    public static DateTimeOffset? Round(this DateTimeOffset? originalDateTimeOffset, long ticksType = TimeSpan.TicksPerSecond)
    {
        if (!originalDateTimeOffset.HasValue)
        {
            return null;
        }

        return originalDateTimeOffset.Value.Round(ticksType);
    }

    /// <summary>
    /// Returns the first day of month for given date (time: 0:0:0.0).<br/>
    /// <code>
    /// new DateTime(2023, 7, 8).FirstDayOfMonth() // = 2023.7.1 0:0:0
    /// </code>
    /// </summary>
    public static DateTimeOffset FirstDayOfMonth(this DateTimeOffset value) =>
        new(new DateTime(value.Year, value.Month, 1), value.Offset);

    /// <summary>
    /// Returns the last day of month for given date (time: 0:0:0.0).<br/>
    /// <code>
    /// new DateTime(2023, 6, 8).LastDayOfMonth() // = 2023.6.30 23:59:59
    /// </code>
    /// </summary>
    public static DateTimeOffset LastDayOfMonth(this DateTimeOffset value) =>
        value.FirstDayOfMonth().AddMonths(1).AddMicroseconds(-1);
}
