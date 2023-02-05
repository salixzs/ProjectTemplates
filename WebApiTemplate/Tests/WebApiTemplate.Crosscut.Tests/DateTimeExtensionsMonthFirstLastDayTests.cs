using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;
public class DateTimeExtensionsMonthFirstLastDayTests
{
    [Fact]
    public void DateTime_FirstMonthDay_Correct()
    {
        var date = new DateTime(2023, 2, 21, 14, 27, 45);
        var result = date.FirstDayOfMonth();
        result.Should().Be(new DateTime(2023, 2, 1, 0, 0, 0));
    }

    [Fact]
    public void DateTime_LastMonthDay_Correct()
    {
        var date = new DateTime(2023, 2, 21, 14, 27, 45);
        var result = date.LastDayOfMonth();
        result.Should().Be(new DateTime(2023, 2, 28, 23, 59, 59, 999, 999));
    }
}
