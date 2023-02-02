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
    /// Returns string of readable, nicely formatted, shortened and humanized date string (in English).
    /// Using also "Today", "3 days ago" and 1.-2. February.
    /// Year added if it is not current year.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date (optional).</param>
    /// <param name="howManyDaysAsText">Days (from today) to use as textual (0 = today, 1 = tomorrow, yesterday, 2 = 2 days ago, 3 = 3 days ago etc.)</param>
    public static string ToHumanEnglishDateTimeString(this in DateTime startDate, DateTime? endDate = null, int howManyDaysAsText = 1)
    {
        var str = new StringBuilder();

        // Handle multiple dates separately
        if (endDate > startDate)
        {
            if (startDate.Month == endDate.Value.Month)
            {
                str.AppendFormat("{0}. – {1}. {2:MMMM}", startDate.Day, endDate.Value.Day, startDate);
            }
            else
            {
                str.AppendFormat("{0:M} – {1:M}", startDate, endDate.Value);
            }

            if (startDate.Year != DateTime.Now.Year)
            {
                str.AppendFormat(", {0}", startDate.Year);
            }

            return str.ToString();
        }

        // Here goes single date handling, starting with close to today
        if (startDate.Date.IsBetween(DateTime.Now.Date.AddDays(0 - howManyDaysAsText), DateTime.Now.Date.AddDays(howManyDaysAsText).AddHours(23).AddMinutes(59).AddSeconds(59)))
        {
            if (startDate.Date == DateTime.Now.Date)
            {
                return $"Today ({startDate:t})";
            }

            if (howManyDaysAsText > 0)
            {
                if (startDate.Date == DateTime.Now.Date.AddDays(1))
                {
                    return "Tomorrow";
                }

                if (startDate.Date == DateTime.Now.Date.AddDays(-1))
                {
                    return $"Yesterday ({startDate:t})";
                }
            }

            if (howManyDaysAsText > 1)
            {
                if (startDate.Date == DateTime.Now.Date.AddDays(2))
                {
                    return "The day after tomorrow";
                }

                if (startDate.Date == DateTime.Now.Date.AddDays(-2))
                {
                    return "The day before yesterday";
                }
            }

            if (howManyDaysAsText > 2)
            {
                if (startDate.Date == DateTime.Now.Date.AddDays(3))
                {
                    return "Three days from now";
                }

                if (startDate.Date == DateTime.Now.Date.AddDays(-3))
                {
                    return "Three days ago";
                }
            }

            for (int daysFromNow = 4; daysFromNow <= howManyDaysAsText; daysFromNow++)
            {
                if (startDate.Date == DateTime.Now.Date.AddDays(daysFromNow))
                {
                    return string.Format("{0} days from now", daysFromNow);
                }

                if (startDate.Date == DateTime.Now.Date.AddDays(0 - daysFromNow))
                {
                    return string.Format("{0} days ago", daysFromNow);
                }
            }
        }

        str.Append(startDate.ToString("M"));
        if (startDate.Year != DateTime.Now.Year)
        {
            str.AppendFormat(", {0}", startDate.Year);
        }

        return str.ToString();
    }
}
