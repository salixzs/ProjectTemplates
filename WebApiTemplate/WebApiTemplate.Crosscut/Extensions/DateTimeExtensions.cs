namespace WebApiTemplate.Crosscut.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Determines whether the evaluated DateTime value is between (inclusive) two specified dates.
    /// Time part is significant. Take care if specify <paramref name="endTime" /> without Time part - it may give logically false results near endDate (time should be 23:59:59).
    /// </summary>
    /// <param name="dateToCheck">The date to check.</param>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <returns>
    ///   <c>true</c> if the specified start time is between; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">DateTime.IsBetween() extension method got startTime bigger than endTime</exception>
    public static bool IsBetween(this DateTime dateToCheck, DateTime startTime, DateTime endTime)
    {
        if (startTime > endTime)
        {
            throw new ArgumentException("DateTime.IsBetween() extension method got startTime bigger than endTime");
        }

        return dateToCheck.Ticks >= startTime.Ticks && dateToCheck.Ticks <= endTime.Ticks;
    }

    /// <summary>
    /// Determines whether the evaluated DateTime value is between (inclusive) two specified dates.
    /// Null values are considered as valid (produces true outcome).
    /// Time part is significant. Take care if specify <paramref name="endTime" /> without Time part - it may give logically false results near endDate (time should be 23:59:59).
    /// </summary>
    /// <param name="dateToCheck">The date to check.</param>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <returns>
    ///   <c>true</c> if the specified start time is between; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">DateTime.IsBetween() extension method got startTime bigger than endTime when both specified.</exception>
    public static bool IsBetween(this DateTime dateToCheck, DateTime? startTime, DateTime? endTime)
    {
        if (startTime.HasValue && endTime.HasValue)
        {
            return dateToCheck.IsBetween(startTime.Value, endTime.Value);
        }

        if (endTime.HasValue && dateToCheck.Ticks >= endTime.Value.Ticks)
        {
            return false;
        }

        if (startTime.HasValue && dateToCheck.Ticks <= startTime.Value.Ticks)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether the evaluated DateTimeOffset value is between (inclusive) two specified dates.
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
    /// Determines whether the evaluated DateTimeOffset value is between (inclusive) two specified dates.
    /// Null values are considered as valid (produces true outcome).
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
            return dateToCheck.IsBetween(startTime.Value, endTime.Value);
        }

        if (endTime.HasValue && dateToCheck.Ticks >= endTime.Value.Ticks)
        {
            return false;
        }

        if (startTime.HasValue && dateToCheck.Ticks <= startTime.Value.Ticks)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Rounds DateTime value to nearest given hour/minute/second/millisecond.
    /// </summary>
    /// <param name="originalDateTime">Original DateTime value.</param>
    /// <param name="ticksType">Use <c>TimeSpan.TicksPerSecond</c>, <c>TimeSpan.TicksPerMinute</c> and similar.</param>
    /// <returns>Rounded DateTime value to nearest given ticks value.</returns>
    public static DateTime Round(this DateTime originalDateTime, long ticksType = TimeSpan.TicksPerSecond)
    {
        if (ticksType > 1)
        {
            var fraction = originalDateTime.Ticks % ticksType;
            if (fraction != 0)
            {
                if (fraction * 2 >= ticksType)
                {
                    // round up
                    return new DateTime(originalDateTime.Ticks + ticksType - fraction);
                }
                else
                {
                    // round down
                    return new DateTime(originalDateTime.Ticks - fraction);
                }
            }
        }

        return originalDateTime;
    }

    /// <summary>
    /// Rounds DateTime value to nearest given hour/minute/second/millisecond.
    /// </summary>
    /// <param name="originalDateTime">Original DateTime value.</param>
    /// <param name="ticksType">Use <c>TimeSpan.TicksPerSecond</c>, <c>TimeSpan.TicksPerMinute</c> and similar.</param>
    /// <returns>Rounded DateTime value to nearest given ticks value.</returns>
    public static DateTime? Round(this DateTime? originalDateTime, long ticksType = TimeSpan.TicksPerSecond)
    {
        if (originalDateTime.HasValue)
        {
            return originalDateTime.Value.Round(ticksType);
        }

        return null;
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
                else
                {
                    // round down
                    return new DateTimeOffset(originalDateTimeOffset.Ticks - fraction, originalDateTimeOffset.Offset);
                }
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
        if (originalDateTimeOffset.HasValue)
        {
            return originalDateTimeOffset.Value.Round(ticksType);
        }

        return null;
    }

    /// <summary>
    /// Returns person/device age in years given his birthday/purchase/installation.
    /// </summary>
    /// <param name="birthDate">Date of birth of a person (or date of purchase).</param>
    /// <param name="relativeYear">Calculate relative to given year. If null - takes current year.</param>
    /// <returns>Age in years.</returns>
    public static int Age(this DateTime birthDate, int? relativeYear = null)
    {
        var compareDate = DateTime.Now;
        if (relativeYear.HasValue)
        {
            compareDate = new DateTime(relativeYear.Value, compareDate.Month, compareDate.Day, compareDate.Hour, compareDate.Minute, compareDate.Second);
        }

        if (birthDate > compareDate)
        {
            return 0;
        }

        var age = compareDate.Year - birthDate.Year;
        if (compareDate < birthDate.AddYears(age))
        {
            age--;
        }

        return age;
    }

    /// <summary>
    /// Returns person/device age in years given his birthday/purchase/installation.
    /// </summary>
    /// <param name="birthDate">Date of birth of a person (or date of purchase).</param>
    /// <param name="relativeYear">Calculate relative to given year. If null - takes current year.</param>
    /// <returns>Age in years.</returns>
    public static int? Age(this DateTime? birthDate, int? relativeYear = null) => birthDate?.Age(relativeYear);

    /// <summary>
    /// Returns a list of all days in given year/month as DateTime objects.<br/>
    /// Can be used in LINQ expressions to get specific days, like this for getting all Fridays:
    /// <code>
    /// var fridays = AllMonthDates(2022, 7).Where(x => x.DayOfWeek == DayOfWeek.Friday);
    /// </code>
    /// </summary>
    public static IEnumerable<DateTime> AllMonthDates(this DateTime yearMonth)
    {
        var year = yearMonth.Year;
        var month = yearMonth.Month;
        var days = DateTime.DaysInMonth(year, month);
        for (var day = 1; day <= days; day++)
        {
            yield return new DateTime(year, month, day);
        }
    }
}
