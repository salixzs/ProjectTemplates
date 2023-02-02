using System.Globalization;

namespace WebApiTemplate.Crosscut.Tests;

public class HumanizerDateStringTests
{
    private readonly ITestOutputHelper _output;

    public HumanizerDateStringTests(ITestOutputHelper output) => _output = output;

    //[Theory, MemberData(nameof(HumanDateStringTestData))]
    //public void ToHumanDateString_VariousValues_ExpectedResult(DateTime startTime, DateTime? endTime, int textDays, string expected)
    //{
    //    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
    //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
    //    _output.WriteLine("   Start: " + startTime.ToString("dd.MM.yyyy HH:mm"));
    //    _output.WriteLine("     End: " + (endTime == null ? "NULL" : endTime.Value.ToString("dd.MM.yyyy HH:mm")));
    //    _output.WriteLine("    Days: " + textDays.ToString());
    //    _output.WriteLine("Expected: " + expected);
    //    startTime.ToHumanDateString(endTime, textDays).Should().Be(expected);
    //}

    public static IEnumerable<object[]> HumanDateStringTestData()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        yield return new object[] { new DateTime(2016, 2, 1), null, 5, "February 1, 2016" };
        yield return new object[] { new DateTime(2016, 2, 1), new DateTime(2016, 2, 1), 5, "February 1, 2016" };
        yield return new object[] { new DateTime(2016, 2, 1), new DateTime(2016, 2, 2), 5, "1. – 2. February, 2016" };
        yield return new object[] { new DateTime(2016, 3, 31), new DateTime(2016, 4, 1), 5, "March 31 – April 1, 2016" };
        yield return new object[] { new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 23, 15), null, 5, "Today (2:23 PM)" };
        yield return new object[] { DateTime.Now.AddDays(1), null, 5, "Tomorrow" };
        yield return new object[] { DateTime.Now.AddDays(2), null, 5, "The day after tomorrow" };
        yield return new object[] { DateTime.Now.AddDays(3), null, 5, "Three days from now" };
        yield return new object[] { new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 14, 23, 15), null, 5, "Yesterday (2:23 PM)" };
        yield return new object[] { DateTime.Now.AddDays(-2), null, 5, "The day before yesterday" };
        yield return new object[] { DateTime.Now.AddDays(-3), null, 5, "Three days ago" };
        yield return new object[] { DateTime.Now.AddDays(4), null, 5, "4 days from now" };
        yield return new object[] { DateTime.Now.AddDays(-4), null, 5, "4 days ago" };
        yield return new object[] { DateTime.Now.AddDays(6), null, 5, DateTime.Now.AddDays(6).ToString("M") };
        yield return new object[] { DateTime.Now.AddDays(-6), null, 5, DateTime.Now.AddDays(-6).ToString("M") };
        yield return new object[] { DateTime.Now.AddDays(-4), null, 4, "4 days ago" };
        yield return new object[] { DateTime.Now.AddDays(-4), null, 3, DateTime.Now.AddDays(-4).ToString("M") };
        yield return new object[] { DateTime.Now.AddDays(-3), null, 3, "Three days ago" };
        yield return new object[] { DateTime.Now.AddDays(-3), null, 2, DateTime.Now.AddDays(-3).ToString("M") };
        yield return new object[] { DateTime.Now.AddDays(-2), null, 2, "The day before yesterday" };
        yield return new object[] { DateTime.Now.AddDays(-2), null, 1, DateTime.Now.AddDays(-2).ToString("M") };
        yield return new object[] { new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 14, 23, 15), null, 1, "Yesterday (2:23 PM)" };
        yield return new object[] { DateTime.Now.AddDays(-1), null, 0, DateTime.Now.AddDays(-1).ToString("M") };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
