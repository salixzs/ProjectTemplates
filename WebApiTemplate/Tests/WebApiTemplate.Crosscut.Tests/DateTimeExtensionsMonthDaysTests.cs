using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class DateTimeExtensionsMonthDaysTests
{
    [Fact]
    public void DateTime_AllMonthDays_February28()
    {
        var date = new DateTime(2023, 2, 21);
        var result = date.AllMonthDates();
        result.Should().HaveCount(28);
    }

    [Fact]
    public void DateTime_AllMonthDays_February29()
    {
        var date = new DateTime(2020, 2, 12);
        var result = date.AllMonthDates();
        result.Should().HaveCount(29);
    }

    [Fact]
    public void DateTime_AllMonthDays_April30()
    {
        var date = new DateTime(2020, 4, 3);
        var result = date.AllMonthDates();
        result.Should().HaveCount(30);
    }

    [Fact]
    public void DateTime_AllMonthDays_December31()
    {
        var date = new DateTime(2020, 12, 23);
        var result = date.AllMonthDates();
        result.Should().HaveCount(31);
    }
}
