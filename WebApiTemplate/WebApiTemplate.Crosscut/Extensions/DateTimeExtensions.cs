namespace WebApiTemplate.Crosscut.Extensions;

public static class DateTimeExtensions
{
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

                // round down
                return new DateTime(originalDateTime.Ticks - fraction);
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
        if (!originalDateTime.HasValue)
        {
            return null;
        }

        return originalDateTime.Value.Round(ticksType);
    }

    /// <summary>
    /// Returns person/device age in years given his birthday/purchase/installation.
    /// </summary>
    /// <param name="birthDate">Date of birth of a person (or date of purchase).</param>
    /// <param name="relativeDate">Calculate relative to given date (calculate age in the past - at given time). If null - takes current date.</param>
    /// <returns>Age in years.</returns>
    public static int Age(this DateTime birthDate, DateTime? relativeDate = null)
    {
        var compareDate = DateTime.Now;
        if (relativeDate.HasValue)
        {
            compareDate = relativeDate.Value;
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
    /// <param name="relativeDate">Calculate relative to given date (calculate age in the past - at given time). If null - takes current date.</param>
    /// <returns>Age in years.</returns>
    public static int? Age(this DateTime? birthDate, DateTime? relativeDate = null) => birthDate?.Age(relativeDate);

    /// <summary>
    /// Returns a list of all days in given year/month as DateTime objects.<br/>
    /// Can be used in LINQ expressions to get specific days, like this for getting all Fridays:
    /// <code>
    /// var fridays = monthInQuestion.AllMonthDates().Where(x => x.DayOfWeek == DayOfWeek.Friday);
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

    /// <summary>
    /// Returns the first day of month for given date (time: 0:0:0.0).<br/>
    /// <code>
    /// new DateTime(2023, 7, 8).FirstDayOfMonth() // = 2023.7.1 0:0:0
    /// </code>
    /// </summary>
    public static DateTime FirstDayOfMonth(this DateTime value) =>
        new(value.Year, value.Month, 1);

    /// <summary>
    /// Returns the last day of month for given date (time: 0:0:0.0).<br/>
    /// <code>
    /// new DateTime(2023, 6, 8).LastDayOfMonth() // = 2023.6.30 23:59:59
    /// </code>
    /// </summary>
    public static DateTime LastDayOfMonth(this DateTime value) =>
        value.FirstDayOfMonth().AddMonths(1).AddMicroseconds(-1);
}
