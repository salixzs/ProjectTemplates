namespace WebApiTemplate.Crosscut.Extensions;

public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Determines whether the evaluated DateTimeOffset value is between (inclusive) two specified dates (UTC times are compared).<br/>
    /// Time part is significant. Take care if specify <paramref name="endTime" /> without Time part - it may give logically false results near endDate (time should be 23:59:59).
    /// </summary>
    /// <param name="dateToCheck">The date to check.</param>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <returns>
    ///   <c>true</c> if the specified start time is between; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">DateTime.IsBetween() extension method got startTime bigger than endTime</exception>
    public static bool IsBetween(this DateTimeOffset dateToCheck, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        if (startTime.UtcDateTime > endTime.UtcDateTime)
        {
            throw new ArgumentException("DateTime.IsBetween() extension method got startTime bigger than endTime");
        }

        return dateToCheck.Ticks >= startTime.Ticks && dateToCheck.Ticks <= endTime.Ticks;
    }

    /// <summary>
    /// Determines whether the evaluated DateTimeOffset value is between (inclusive) two specified dates (UTC times are compared).<br/>
    /// Null values are considered as valid (produces true outcome).<br/>
    /// Time part is significant. Take care if specify <paramref name="endTime" /> without Time part - it may give logically false results near endDate (time should be 23:59:59).
    /// </summary>
    /// <param name="dateToCheck">The date to check.</param>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <returns>
    ///   <c>true</c> if the specified start time is between; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">DateTime.IsBetween() extension method got startTime bigger than endTime when both specified.</exception>
    public static bool IsBetween(this DateTimeOffset dateToCheck, DateTimeOffset? startTime, DateTimeOffset? endTime)
    {
        if (startTime.HasValue && endTime.HasValue)
        {
            return dateToCheck.IsBetween(startTime.Value.UtcDateTime, endTime.Value.UtcDateTime);
        }

        if (endTime.HasValue && dateToCheck.UtcTicks >= endTime.Value.UtcTicks)
        {
            return false;
        }

        if (startTime.HasValue && dateToCheck.UtcTicks <= startTime.Value.UtcTicks)
        {
            return false;
        }

        return true;
    }

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
