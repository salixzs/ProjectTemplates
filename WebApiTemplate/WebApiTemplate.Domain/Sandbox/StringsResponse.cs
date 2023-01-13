namespace WebApiTemplate.Domain.Sandbox;

public class StringsResponse
{
    public string CurrentDateTimeToString { get; set; } = DateTime.Now.ToString("D");
    public string SimpleString { get; set; } = "Brown Fox Jumped Over Lazy Dog";
    public string NationalChars { get; set; } = "LV:Āčēžūž; RU:Выдалъ; NO:Æøå; FI:Åäö";
    public char CharValue { get; set; } = 'q';
}
