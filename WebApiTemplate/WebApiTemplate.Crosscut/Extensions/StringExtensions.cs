namespace WebApiTemplate.Crosscut.Extensions;

/// <summary>
/// Some handy extensions to string values.
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
    public static bool IsInteger(this string stringToCheck) => !string.IsNullOrWhiteSpace(stringToCheck) && stringToCheck.Trim().All(char.IsNumber);

    /// <summary>
    /// Checks whether string contains only letters (words), including unicode letters. Empty/null strings are not alpha.
    /// </summary>
    /// <param name="stringToCheck">The string to check</param>
    /// <returns>True, is string does not contains anything else beside normal text</returns>
    public static bool IsAlphaOnly(this string stringToCheck) => !string.IsNullOrEmpty(stringToCheck) && stringToCheck.All(c => char.IsLetter(c) || c == ' ');

    /// <summary>
    /// Checks whether string contains only letters, space, dash (-) and apostrophes ('`). 
    /// Strig containing less than 2 letters are considered not a human name.
    /// Empty/null strings are not human name.
    /// </summary>
    /// <param name="stringToCheck">The string to check</param>
    /// <returns>True, is string does not contains anything else beside normal text</returns>
    public static bool IsHumanName(this string stringToCheck)
    {
        return !string.IsNullOrWhiteSpace(stringToCheck)
               && stringToCheck.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '`' || c == '\'')
               && stringToCheck.Count(char.IsLetter) > 1;
    }

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
    /// <returns>True, is string contains all letters in their lowercase form, otherwise false.</returns>
    public static bool IsLowercase(this string? stringToCheck)
    {
        if (string.IsNullOrWhiteSpace(stringToCheck))
        {
            return false;
        }

        for (var i = 0; i < stringToCheck.Length; i++)
        {
            if (char.IsLetter(stringToCheck[i]) && !char.IsLower(stringToCheck[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks whether string contains only letters, numbers, spaces and normal punctuation. Empty/null strings are not normal text.
    /// </summary>
    /// <param name="stringToCheck">The string to check</param>
    /// <returns>True, if string does not contains anything else beside normal text</returns>
    public static bool IsNormalText(this string stringToCheck) => !string.IsNullOrWhiteSpace(stringToCheck) && stringToCheck.All(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c) || char.IsControl(c) || c == '(' || c == ')' || c == '_');

    /// <summary>
    /// Checks string whether it contains any of Unicode characters that cannot be found in ASCII range (only Latin).
    /// Also handles empty/null strings accordingly.
    /// </summary>
    /// <param name="stringToCheck">The string to check</param>
    /// <returns>True, if string contains any non-Ascii character</returns>
    public static bool ContainsUnicodeCharacter(this string stringToCheck) => !string.IsNullOrWhiteSpace(stringToCheck) && stringToCheck.ToCharArray().Any(c => c > 126);

    /// <summary>
    /// Determines whether the specified string to check contains characters only in western languages (not Greek, Russian, Arab, Chinese).
    /// Also handles empty/null strings accordingly.
    /// </summary>
    /// <param name="stringToCheck">The string to check.</param>
    public static bool IsWesternLanguage(this string stringToCheck) => string.IsNullOrWhiteSpace(stringToCheck) || stringToCheck.ToCharArray().All(c => c <= 0x017F);

    /// <summary>
    /// Gives corresponding Excel Column name to given integer.
    /// Specific is after reaching "Z" it will use two-letter names "AA".
    /// </summary>
    /// <param name="number">The number of column in Excel sheet.</param>
    public static string ToExcelColumnName(this in int number)
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        var value = string.Empty;
        var index = number - 1;
        if (index >= letters.Length)
        {
            value += letters[(index / letters.Length) - 1];
        }

        value += letters[index % letters.Length];
        return value;
    }

    /// <summary>
    /// Returns letter by English alphabet based on given position (1 = A, 2 = B).
    /// Number should be in range 1-26, otherwise empty string is returned.
    /// </summary>
    /// <param name="number">Sequence number in alphabet (1..26).</param>
    /// <param name="asUppercase">True - UPPERCASE letter, false - lowercase. Default = UPPERCASE.</param>
    public static string ToLetter(this in int number, in bool asUppercase = true)
    {
        if (number is < 1 or > 26)
        {
            return string.Empty;
        }

        var c = (char)((asUppercase ? 65 : 97) + (number - 1));
        return c.ToString();
    }
}
