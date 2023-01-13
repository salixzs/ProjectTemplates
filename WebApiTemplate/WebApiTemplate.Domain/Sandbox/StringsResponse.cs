namespace WebApiTemplate.Domain.Sandbox;

/// <summary>
/// Several text types in .Net
/// </summary>
public class StringsResponse
{
    /// <summary>
    /// Current Local DateTime as Full string representation (non-static value).
    /// </summary>
    public string CurrentDateTimeToString { get; set; } = DateTime.Now.ToString("D");

    /// <summary>
    /// Siple string with ASCII characters only.
    /// </summary>
    public string SimpleString { get; set; } = "Brown Fox Jumped Over Lazy Dog";

    /// <summary>
    /// String with several language national characters.
    /// </summary>
    public string NationalChars { get; set; } = "LV:Āčēžūž; RU:Выдалъ; NO:Æøå; FI:Åäö";

    /// <summary>
    /// Single CHAR value.
    /// </summary>
    public char CharValue { get; set; } = 'q';
}
