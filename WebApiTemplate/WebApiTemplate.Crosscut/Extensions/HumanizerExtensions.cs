using System.Text;

namespace WebApiTemplate.Crosscut.Extensions;

/// <summary>
/// Extensions to return human readable and vocalized strings of some date/time and numeric values.<br/>
/// Some extensions to transform text of some types to normalized or reduced forms.
/// </summary>
public static class HumanizerExtensions
{
    /// <summary>
    /// For Human first and last names - Converts them from anything to ProperCase.<br/>
    /// Ex: VANHALEN - VanHalen, macdonalds = MacDonalds, g'kar = G'Kar.
    /// </summary>
    /// <param name="humanName">Name of the human.</param>
    public static string? ToHumanNameProperCase(this string? humanName)
    {
        if (string.IsNullOrEmpty(humanName))
        {
            return humanName;
        }

        var totalTransform = humanName.IsUppercase(1) || humanName.IsLowercase();
        var properName = new StringBuilder();
        var nextIsUppercase = true;
        for (var letterInWord = 0; letterInWord < humanName.Length; letterInWord++)
        {
            if (humanName[letterInWord] is ' ' or '-' or '`' or '\'')
            {
                properName.Append(humanName[letterInWord]);
                nextIsUppercase = true;
                continue;
            }

            // First two letters are to be special-handled
            if (letterInWord < 2)
            {
                properName.Append(nextIsUppercase ? char.ToUpper(humanName[letterInWord]) : char.ToLower(humanName[letterInWord]));
                nextIsUppercase = false;
                continue;
            }

            // starting from 3rd letter - do transform only when all UPPERCASE, otherwise - leave as is.
            if (totalTransform && letterInWord > 1)
            {
                properName.Append(nextIsUppercase ? char.ToUpper(humanName[letterInWord]) : char.ToLower(humanName[letterInWord]));
            }
            else
            {
                properName.Append(nextIsUppercase ? char.ToUpper(humanName[letterInWord]) : humanName[letterInWord]);
            }

            nextIsUppercase = false;
        }

        properName.Replace(" Van ", " van ")
            .Replace(" Von ", " von ")
            .Replace(" Der ", " der ")
            .Replace(" De ", " de ")
            .Replace(" La ", " la ")
            .Replace("Van ", "van ", 0, 4)
            .Replace("Von ", "von ", 0, 4)
            .Replace("Der ", "der ", 0, 4)
            .Replace("De ", "de ", 0, 3)
            .Replace("La ", "la ", 0, 3);

        if (!totalTransform)
        {
            return properName.ToString();
        }

        // Special cases with Mc Mac
        var separateWords = properName.ToString().Split(' ');
        for (var wordIndex = 0; wordIndex < separateWords.Length; wordIndex++)
        {
            if (separateWords[wordIndex].StartsWith("Mc", StringComparison.OrdinalIgnoreCase))
            {
                separateWords[wordIndex] = string.Concat("Mc", separateWords[wordIndex].Substring(2, 1).ToUpper(), separateWords[wordIndex].AsSpan(3));
            }

            if (separateWords[wordIndex].StartsWith("Mac"))
            {
                separateWords[wordIndex] = string.Concat("Mac", separateWords[wordIndex].Substring(3, 1).ToUpper(), separateWords[wordIndex].AsSpan(4));
            }
        }

        return string.Join(" ", separateWords);
    }

    /// <summary>
    /// Transforms integer to Roman number (I II III IV V VI).
    /// </summary>
    /// <param name="number">The normal integer number.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Given value is outside range 1 and 3999.
    /// </exception>
    public static string ToRoman(this in int number)
    {
        const string errorMsg = "Failed to convert to Roman: number is too large";

        var romanNumerals = new string[][]
        {
            new string[] { string.Empty, "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" }, // ones
            new string[] { string.Empty, "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" }, // tens
            new string[] { string.Empty, "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" }, // hundreds
            new string[] { string.Empty, "M", "MM", "MMM" }, // thousands
        };

        // split integer string into array and reverse array
        var inputNumberDigits = number.ToString().Reverse().ToArray();
        var inputNumberLength = inputNumberDigits.Length;
        var romanNumeral = new StringBuilder();
        var i = inputNumberLength;

        // starting with the highest place (for 3046, it would be the thousands
        // place, or 3), get the roman numeral representation for that place
        // and add it to the final roman numeral string
        while (i-- > 0)
        {
            if (romanNumerals[i].Length < int.Parse(inputNumberDigits[i].ToString()))
            {
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            romanNumeral.Append(romanNumerals[i][int.Parse(inputNumberDigits[i].ToString())]);
        }

        return romanNumeral.ToString();
    }

    /// <summary>
    /// Returns TimeSpan string representation in its shortest possible form.<br/>
    /// Not recommended to use for times more than few minutes!<br/>
    /// Examples: 304ms, 2.34s, 1 min 28.4323 sec.
    /// </summary>
    /// <param name="elapsedTime">The TimeSpan, usually elapsed time from Stopwatch.Elapsed.</param>
    public static string ToStringHuman(this TimeSpan elapsedTime)
    {
        if (elapsedTime.TotalMilliseconds < 10)
        {
            return $"{elapsedTime.TotalMilliseconds:0.##0}ms";
        }

        if (elapsedTime.TotalMilliseconds < 1000)
        {
            return $"{elapsedTime.TotalMilliseconds:0}ms";
        }

        if (elapsedTime.Minutes > 0)
        {
            return $"{elapsedTime.Minutes:D} min {elapsedTime.Seconds:D} sec";
        }

        if (elapsedTime.Seconds < 10)
        {
            return $"{elapsedTime.Seconds:D}s {elapsedTime.Milliseconds:0}ms";
        }

        return $"{elapsedTime.Seconds:D}s";
    }

    /// <summary>
    /// Converts date/time to humanly readable string.<br/>
    /// Ex. "Just now", "1 hour ago", "Yesterday", "2 days ago".<br/>
    /// Time is compared with <c>DateTime.Now</c> (Local) if not given in <paramref name="relativeDate"/>.
    /// </summary>
    /// <param name="dateTime">Date/Time to be humanly readable.</param>
    /// <param name="textDays">How many days to convert to "X days ago" (maximum/default: 5)</param>
    /// <param name="relativeDate">To calculate human string, relative to given date/time, not default current date/time (LOCAL - not UTC).</param>
    public static string ToStringHuman(this DateTime dateTime, int textDays = 5, DateTime? relativeDate = null)
    {
        var compareDate = relativeDate ?? DateTime.Now;
        if (!dateTime.Date.IsBetween(compareDate.Date.AddDays(0 - textDays), compareDate.Date.AddDays(textDays).AddHours(23).AddMinutes(59).AddSeconds(59)))
        {
            if (dateTime.Year != compareDate.Year)
            {
                return $"{dateTime.ToString("M")}, {dateTime.Year}";
            }

            return dateTime.ToString("M");
        }

        if (dateTime.Date == compareDate.Date)
        {
            var differenceTimespan = compareDate > dateTime ? (compareDate - dateTime) : (dateTime - compareDate);
            if (differenceTimespan > TimeSpan.FromHours(1))
            {
                var hourCount = Math.Round(differenceTimespan.TotalHours);
                if (hourCount == 1)
                {
                    return compareDate > dateTime ? Translations.HourAgo : Translations.In1Hour;
                }
                else
                {
                    return string.Format((compareDate > dateTime ? Translations.HoursAgo : Translations.InHours), hourCount);
                }
            }

            if (differenceTimespan > TimeSpan.FromMinutes(1))
            {
                var minuteCount = Math.Round(differenceTimespan.TotalMinutes);
                if (minuteCount == 1)
                {
                    return compareDate > dateTime ? Translations.MinuteAgo : Translations.In1Minute;
                }
                else
                {
                    return string.Format((compareDate > dateTime ? Translations.MinutesAgo : Translations.InMinutes), minuteCount);
                }
            }

            return Translations.JustNow;
        }

        if (textDays > 0)
        {
            if (dateTime.Date == compareDate.Date.AddDays(1))
            {
                return Translations.Tomorrow;
            }

            if (dateTime.Date == compareDate.Date.AddDays(-1))
            {
                return Translations.Yesterday;
            }
        }

        if (textDays > 1)
        {
            if (dateTime.Date == compareDate.Date.AddDays(2))
            {
                return Translations.DayAfterTomorrow;
            }

            if (dateTime.Date == compareDate.Date.AddDays(-2))
            {
                return Translations.DayBeforeYesterday;
            }
        }

        if (textDays > 2)
        {
            if (dateTime.Date == compareDate.Date.AddDays(3))
            {
                return Translations.ThreeDaysFromNow;
            }

            if (dateTime.Date == compareDate.Date.AddDays(-3))
            {
                return Translations.ThreeDaysAgo;
            }
        }

        for (var daysFromNow = 4; daysFromNow <= textDays; daysFromNow++)
        {
            if (dateTime.Date == compareDate.Date.AddDays(daysFromNow))
            {
                return string.Format(Translations.DaysFromNow, daysFromNow);
            }

            if (dateTime.Date == compareDate.Date.AddDays(0 - daysFromNow))
            {
                return string.Format(Translations.DaysAgo, daysFromNow);
            }
        }

        return dateTime.ToString("dd MMMM");
    }
}
