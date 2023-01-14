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
}
