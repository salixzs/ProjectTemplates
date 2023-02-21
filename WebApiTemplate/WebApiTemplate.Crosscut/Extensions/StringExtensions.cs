using System.Globalization;

namespace WebApiTemplate.Crosscut.Extensions;

/// <summary>
/// Some handy extensions to string values.<br/>
/// Some are usable in elaborate validation rules.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Determines whether the specified string to check is integer. Also handles empty/null strings accordingly.
    /// </summary>
    /// <param name="stringToCheck">The string to check.</param>
    /// <returns>
    /// <c>true</c> if the specified string to check is integer; otherwise (incl. empty/null), <c>false</c>.
    /// </returns>
    public static bool IsInteger(this string stringToCheck) =>
        !string.IsNullOrWhiteSpace(stringToCheck) && stringToCheck.Trim().All(char.IsNumber);

    /// <summary>
    /// Checks whether string contains only letters (words), including unicode letters. Empty/null strings are not alpha.
    /// </summary>
    /// <param name="stringToCheck">The string to check</param>
    /// <returns>True, is string does not contains anything else beside normal text</returns>
    public static bool IsAlphaOnly(this string stringToCheck) =>
        !string.IsNullOrEmpty(stringToCheck) && stringToCheck.All(c => char.IsLetter(c) || c == ' ');

    /// <summary>
    /// Checks whether string contains only letters, space, dash (-) and apostrophes ('`).<br/>
    /// String containing less than 2 letters are considered not a human name.<br/>
    /// Empty/null strings are not human name.
    /// </summary>
    /// <param name="stringToCheck">The string to check</param>
    /// <returns>True, is string does not contains anything else beside normal text</returns>
    public static bool IsHumanName(this string stringToCheck) =>
        !string.IsNullOrWhiteSpace(stringToCheck)
            && stringToCheck.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '`' || c == '\'')
            && stringToCheck.Count(char.IsLetter) > 1;

    /// <summary>
    /// Checks whether string contains only uppercase letters, including unicode letters.
    /// Non-letter characters are ignored. null/empty will return false.
    /// </summary>
    /// <param name="stringToCheck">The string to check.</param>
    /// <param name="allowedLowercaseCount">Count of lowercase letters, which are allowed to still consider string as UPPERCASE.</param>
    /// <returns>True, is string contains all letters in their uppercase form, otherwise false.</returns>
    public static bool IsUppercase(this string? stringToCheck, int allowedLowercaseCount = 0)
    {
        if (string.IsNullOrWhiteSpace(stringToCheck))
        {
            return false;
        }

        var lowercaseCounter = allowedLowercaseCount;
        for (var i = 0; i < stringToCheck.Length; i++)
        {
            if (char.IsLetter(stringToCheck[i]) && !char.IsUpper(stringToCheck[i]))
            {
                if (lowercaseCounter > 0)
                {
                    lowercaseCounter--;
                    continue;
                }

                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks whether string contains only lowercase letters, including unicode letters.
    /// Non-letter characters are ignored. null/empty will return false.
    /// </summary>
    /// <param name="stringToCheck">The string to check.</param>
    /// <param name="allowedUppercaseCount">Count of UPPERCASE letters, which are allowed to still consider string as lowercase.</param>
    /// <returns>True, is string contains all letters in their lowercase form, otherwise false.</returns>
    public static bool IsLowercase(this string? stringToCheck, int allowedUppercaseCount = 0)
    {
        if (string.IsNullOrWhiteSpace(stringToCheck))
        {
            return false;
        }

        var uppercaseCounter = allowedUppercaseCount;
        for (var i = 0; i < stringToCheck.Length; i++)
        {
            if (char.IsLetter(stringToCheck[i]) && !char.IsLower(stringToCheck[i]))
            {
                if (uppercaseCounter > 0)
                {
                    uppercaseCounter--;
                    continue;
                }

                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks string whether it contains any of Unicode characters that cannot be found in ASCII range (only Latin).
    /// Also handles empty/null strings accordingly.
    /// </summary>
    /// <param name="stringToCheck">The string to check</param>
    /// <returns>True, if string contains any non-Ascii character</returns>
    public static bool ContainsUnicodeCharacter(this string stringToCheck) =>
        !string.IsNullOrWhiteSpace(stringToCheck) && stringToCheck.ToCharArray().Any(c => c > 126);

    /// <summary>
    /// Determines whether the specified string to check contains characters only in western languages (not Greek, Russian, Arab, Chinese).
    /// Also handles empty/null strings accordingly.
    /// </summary>
    /// <param name="stringToCheck">The string to check.</param>
    public static bool IsWesternLanguage(this string stringToCheck) =>
        string.IsNullOrWhiteSpace(stringToCheck) || stringToCheck.ToCharArray().All(c => c <= 0x017F);

    /// <summary>
    /// Gives corresponding Excel Column name to given integer (zero-based, max value: 16383).<br/>
    /// Specifics: after reaching "Z" it will use two-letter names "AA", after "ZZ" - "AAA".<br/>
    /// 0 = "A", 25 = "Z", 701 = "ZZ", 10000 = "NTQ"
    /// </summary>
    /// <param name="number">The number of column in Excel sheet (ZERO-based) Max value = 16383.</param>
    /// <exception cref="ArgumentOutOfRangeException">Column name should be in range 0-16383.</exception>
    public static string ToExcelColumnName(this in int number)
    {
        if (number is < 0 or > 16383)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Excel column name index number should be in range of 0-16383");
        }

        const byte letters = 'Z' - 'A' + 1;
        var name = string.Empty;

        var index = number;
        do
        {
            name = Convert.ToChar('A' + (index % letters)) + name;
            index = (index / letters) - 1;
        }
        while (index >= 0);

        return name;
    }

    /// <summary>
    /// Similar to <seealso cref="string.Substring"/>, but returning original, if length is bigger than actual length of a string without throwing exception.
    /// </summary>
    /// <param name="original">Original string.</param>
    /// <param name="maxLength">Maximum allowed length of a string.</param>
    public static string? Truncate(this string? original, int maxLength)
    {
        if (original == null)
        {
            return null;
        }

        return original[..Math.Min(original.Length, maxLength)];
    }

    /// <summary>
    /// Retrieves string part before given separator, if separator exists in source string.
    /// Returns null/empty string if parameters are null/empty or separator string not found in original string.
    /// </summary>
    /// <param name="originalString">The string to retrieve substring from.</param>
    /// <param name="separator">The separator, before which substring should be returned.</param>
    public static string? SubstringBefore(this string? originalString, string? separator)
    {
        if (string.IsNullOrEmpty(originalString))
        {
            return originalString;
        }

        if (string.IsNullOrEmpty(separator))
        {
            return string.Empty;
        }

        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        var index = compareInfo.IndexOf(originalString, separator, CompareOptions.Ordinal);
        return index < 0 ? string.Empty : originalString[..index];
    }

    /// <summary>
    /// Retrieves string part after given separator, if separator exists in given source string.<br/>
    /// Returns null/empty string if parameters are null/empty or separator string not found in original string.
    /// </summary>
    /// <param name="originalString">The string to retrieve substring from.</param>
    /// <param name="separator">The separator, after which substring should be returned.</param>
    public static string? SubstringAfter(this string? originalString, string? separator)
    {
        if (string.IsNullOrEmpty(originalString))
        {
            return originalString;
        }

        if (string.IsNullOrEmpty(separator))
        {
            return string.Empty;
        }

        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        var index = compareInfo.IndexOf(originalString, separator, CompareOptions.Ordinal);
        if (index < 0)
        {
            // No such substring
            return string.Empty;
        }

        return originalString[(index + separator.Length)..];
    }

    /// <summary>
    /// Returns alternative Text if string is null or empty.
    /// <code>
    /// var actualOrDefaultValue = variableFromOutside.WhenEmpty("Def");
    /// </code>
    /// </summary>
    /// <param name="stringInQuestion">Input string.</param>
    /// <param name="alternativeText">Alternative/Default string.</param>
    public static string WhenEmpty(this string? stringInQuestion, string alternativeText) =>
        string.IsNullOrEmpty(stringInQuestion) ? alternativeText : stringInQuestion;
}
