namespace WebApiTemplate.Crosscut.Extensions;

public static class ValidationExtensions
{
    /// <summary>
    /// Checks whether string contains valid e-mail address (only by syntax).<br/>
    /// NULL or empty strings are considered valid.
    /// </summary>
    public static bool IsValidEmailSyntax(this string? emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            return true;
        }

        var emailTrimmed = emailAddress.Trim();
        var hasWhitespace = emailTrimmed.Contains(' ');
        var indexOfAtSign = emailTrimmed.LastIndexOf('@');
        if (indexOfAtSign <= 0 || hasWhitespace)
        {
            return false;
        }

        var afterAtSign = emailTrimmed[(indexOfAtSign + 1)..];
        var indexOfDotAfterAtSign = afterAtSign.LastIndexOf('.');
        return indexOfDotAfterAtSign > 0 && afterAtSign[indexOfDotAfterAtSign..].Length > 1;
    }

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
    /// Determines whether the evaluated DateTime value is between (inclusive) two specified dates.<br/>
    /// NOTE: Null values are considered as valid (produces true outcome)!.<br/>
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
    /// Checks Latvian personal code validity.<br/>
    /// NOTE: When null, empty or whitespace - returns true.
    /// </summary>
    /// <param name="personCode">Personal code with dash, like "210594-14328".</param>
    public static bool IsValidPersonCode(this string? personCode)
    {
        if (string.IsNullOrWhiteSpace(personCode))
        {
            return true;
        }

        var pkNoDash = personCode.Replace("-", string.Empty);
        if (pkNoDash.Length > 11 || pkNoDash.Length < 11 || !pkNoDash.IsInteger())
        {
            return false;
        }

        var newFormatCheck = int.Parse(
            pkNoDash[..2],
            System.Globalization.NumberStyles.Integer,
            System.Globalization.CultureInfo.InvariantCulture);
        if (newFormatCheck is > 31 and < 40)
        {
            return true;
        }

        var identificationCode = pkNoDash[..10];
        var checkDigit = int.Parse(
            pkNoDash.AsSpan(10, 1),
            System.Globalization.NumberStyles.Integer,
            System.Globalization.CultureInfo.InvariantCulture);

        var checksum = 0;
        var checkSumDigits = new int[] { 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        for (var index = 0; index < 10; index++)
        {
            checksum += int.Parse(
                identificationCode.AsSpan(index, 1),
                System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture)
                * checkSumDigits[index];
        }

        checksum = (1 - checksum) % 11;
        checksum += (checksum < -1) ? 11 : 0;

        return checksum == checkDigit;
    }
}
