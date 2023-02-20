using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class DateTimeOffsetTExtensionsMonthFirstLastDayTests
{
    [Fact]
    public void DateTimeOffset_FirstMonthDay_Correct()
    {
        var date = new DateTimeOffset(2023, 2, 21, 14, 27, 45, new TimeSpan(2, 0, 0));
        var result = date.FirstDayOfMonth();
        result.Should().Be(new DateTimeOffset(2023, 2, 1, 0, 0, 0, new TimeSpan(2, 0, 0)));
    }

    [Fact]
    public void DateTimeOffset_LastMonthDay_Correct()
    {
        var date = new DateTimeOffset(2023, 2, 21, 14, 27, 45, new TimeSpan(2, 0, 0));
        var result = date.LastDayOfMonth();
        result.Should().Be(new DateTimeOffset(2023, 2, 28, 23, 59, 59, 999, 999, new TimeSpan(2, 0, 0)));
    }
}
