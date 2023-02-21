using System.Globalization;
using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class HumanizerDateStringTests
{
    private readonly ITestOutputHelper _output;

    public HumanizerDateStringTests(ITestOutputHelper output) => _output = output;

    [Theory, MemberData(nameof(DateTimeToStringHumanEnglishTestData))]
    public void ToStringHuman_Value_ExpectedEnglish(DateTime dateTime, int textDays, DateTime? relativeDate, string expected)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        var compareDate = relativeDate ?? DateTime.Now;
#pragma warning disable CA1305 // Specify IFormatProvider
        _output.WriteLine("DateTime: " + dateTime.ToString("dd.MM.yyyy HH:mm"));
        _output.WriteLine("Compare2: " + compareDate.ToString("dd.MM.yyyy HH:mm"));
        _output.WriteLine("  Differ: " + (dateTime - compareDate).ToString());
        _output.WriteLine("    Days: " + textDays.ToString());
        _output.WriteLine("Expected: " + expected);
        dateTime.ToStringHuman(textDays, relativeDate).Should().Be(expected);
#pragma warning restore CA1305 // Specify IFormatProvider
    }

    public static IEnumerable<object[]> DateTimeToStringHumanEnglishTestData()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        var relativeDate = new DateTime(2023, 2, 10, 12, 0, 0);
        yield return new object[] { new DateTime(2016, 2, 1), 5, relativeDate, "February 1, 2016" };
        yield return new object[] { new DateTime(2023, 1, 23), 5, relativeDate, "January 23" };
        yield return new object[] { new DateTime(2023, 2, 5), 4, relativeDate, "February 5" };
        yield return new object[] { new DateTime(2023, 2, 5), 5, relativeDate, "5 days ago" };
        yield return new object[] { new DateTime(2023, 2, 6), 5, relativeDate, "4 days ago" };
        yield return new object[] { new DateTime(2023, 2, 7), 5, relativeDate, "3 days ago" };
        yield return new object[] { new DateTime(2023, 2, 8), 5, relativeDate, "2 days ago" };
        yield return new object[] { new DateTime(2023, 2, 9), 5, relativeDate, "Yesterday" };
        yield return new object[] { new DateTime(2023, 2, 10, 9, 12, 45), 3, relativeDate, "3 hours ago" };
        yield return new object[] { new DateTime(2023, 2, 10, 10, 52, 12), 3, relativeDate, "An hour ago" };
        yield return new object[] { new DateTime(2023, 2, 10, 11, 12, 45), 3, relativeDate, "47 minutes ago" };
        yield return new object[] { new DateTime(2023, 2, 10, 11, 58, 59), 3, relativeDate, "A minute ago" };
        yield return new object[] { new DateTime(2023, 2, 10, 11, 59, 45), 3, relativeDate, "Just now" };
        yield return new object[] { new DateTime(2023, 2, 10, 12, 1, 5), 3, relativeDate, "In a minute" };
        yield return new object[] { new DateTime(2023, 2, 10, 12, 2, 59), 3, relativeDate, "In 3 minutes" };
        yield return new object[] { new DateTime(2023, 2, 10, 13, 15, 32), 3, relativeDate, "Within the hour" };
        yield return new object[] { new DateTime(2023, 2, 10, 19, 39, 12), 3, relativeDate, "In 8 hours" };
        yield return new object[] { new DateTime(2023, 2, 11), 5, relativeDate, "Tomorrow" };
        yield return new object[] { new DateTime(2023, 2, 12), 5, relativeDate, "In 2 days" };
        yield return new object[] { new DateTime(2023, 2, 13), 5, relativeDate, "In 3 days" };
        yield return new object[] { new DateTime(2023, 2, 14), 5, relativeDate, "In 4 days" };
        yield return new object[] { new DateTime(2023, 2, 15), 5, relativeDate, "In 5 days" };
        yield return new object[] { new DateTime(2023, 2, 15), 4, relativeDate, "February 15" };
        yield return new object[] { new DateTime(2024, 4, 3), 4, relativeDate, "April 3, 2024" };
    }

    [Theory, MemberData(nameof(DateTimeToStringHumanNorwegianTestData))]
    public void ToStringHuman_Value_ExpectedNorwegian(DateTime dateTime, int textDays, DateTime? relativeDate, string expected)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("nb-NO");
        var compareDate = relativeDate ?? DateTime.Now;
#pragma warning disable CA1305 // Specify IFormatProvider
        _output.WriteLine("DateTime: " + dateTime.ToString("dd.MM.yyyy HH:mm"));
        _output.WriteLine("Compare2: " + compareDate.ToString("dd.MM.yyyy HH:mm"));
        _output.WriteLine("  Differ: " + (dateTime - compareDate).ToString());
        _output.WriteLine("    Days: " + textDays.ToString());
        _output.WriteLine("Expected: " + expected);
        dateTime.ToStringHuman(textDays, relativeDate).Should().Be(expected);
#pragma warning restore CA1305 // Specify IFormatProvider
    }

    public static IEnumerable<object[]> DateTimeToStringHumanNorwegianTestData()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        var relativeDate = new DateTime(2023, 2, 10, 12, 0, 0);
        yield return new object[] { new DateTime(2016, 2, 1), 5, relativeDate, "1. februar, 2016" };
        yield return new object[] { new DateTime(2023, 1, 23), 5, relativeDate, "23. januar" };
        yield return new object[] { new DateTime(2023, 2, 5), 4, relativeDate, "5. februar" };
        yield return new object[] { new DateTime(2023, 2, 5), 5, relativeDate, "5 dager siden" };
        yield return new object[] { new DateTime(2023, 2, 6), 5, relativeDate, "4 dager siden" };
        yield return new object[] { new DateTime(2023, 2, 7), 5, relativeDate, "3 dager siden" };
        yield return new object[] { new DateTime(2023, 2, 8), 5, relativeDate, "2 dager siden" };
        yield return new object[] { new DateTime(2023, 2, 9), 5, relativeDate, "I går" };
        yield return new object[] { new DateTime(2023, 2, 10, 9, 12, 45), 3, relativeDate, "3 timer siden" };
        yield return new object[] { new DateTime(2023, 2, 10, 10, 52, 12), 3, relativeDate, "En time siden" };
        yield return new object[] { new DateTime(2023, 2, 10, 11, 12, 45), 3, relativeDate, "47 minutter siden" };
        yield return new object[] { new DateTime(2023, 2, 10, 11, 58, 59), 3, relativeDate, "For et minutt siden" };
        yield return new object[] { new DateTime(2023, 2, 10, 11, 59, 45), 3, relativeDate, "Akkurat nå" };
        yield return new object[] { new DateTime(2023, 2, 10, 12, 1, 5), 3, relativeDate, "På 1 minutt" };
        yield return new object[] { new DateTime(2023, 2, 10, 12, 2, 59), 3, relativeDate, "Om 3 minutter" };
        yield return new object[] { new DateTime(2023, 2, 10, 13, 15, 32), 3, relativeDate, "Innen time" };
        yield return new object[] { new DateTime(2023, 2, 10, 19, 39, 12), 3, relativeDate, "Om 8 timer" };
        yield return new object[] { new DateTime(2023, 2, 11), 5, relativeDate, "I morgen" };
        yield return new object[] { new DateTime(2023, 2, 12), 5, relativeDate, "Overimorgen" };
        yield return new object[] { new DateTime(2023, 2, 13), 5, relativeDate, "Om 3 dager" };
        yield return new object[] { new DateTime(2023, 2, 14), 5, relativeDate, "Om 4 dager" };
        yield return new object[] { new DateTime(2023, 2, 15), 5, relativeDate, "Om 5 dager" };
        yield return new object[] { new DateTime(2023, 2, 15), 4, relativeDate, "15. februar" };
        yield return new object[] { new DateTime(2024, 4, 3), 4, relativeDate, "3. april, 2024" };
    }
}
