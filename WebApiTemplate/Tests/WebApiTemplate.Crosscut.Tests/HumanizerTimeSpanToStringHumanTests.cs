using System.Globalization;
using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class HumanizerTimeSpanToStringHumanTests
{
    public HumanizerTimeSpanToStringHumanTests() => Thread.CurrentThread.CurrentCulture = new CultureInfo("lv-LV");

    [Fact]
    public void ToMinimumString_FewTicks_DecimalMilliseconds()
    {
        var testable = new TimeSpan(30L);
        testable.ToStringHuman().Should().BeOneOf("0,003ms", "0.003ms");
    }

    [Fact]
    public void ToMinimumString_Milliseconds_DecimalMilliseconds()
    {
        var testable = new TimeSpan(35681L);
        testable.ToStringHuman().Should().BeOneOf("3,568ms", "3.568ms");
    }

    [Fact]
    public void ToMinimumString_WholeMilliseconds_GivesMilliseconds()
    {
        var testable = new TimeSpan(0, 0, 0, 0, 49);
        testable.ToStringHuman().Should().Be("49ms");
    }

    [Fact]
    public void ToMinimumString_Seconds_DecimalSeconds()
    {
        var testable = new TimeSpan(0, 0, 0, 8, 340);
        testable.ToStringHuman().Should().Be("8s 340ms");
    }

    [Fact]
    public void ToMinimumString_WholeSeconds_DecimalSeconds()
    {
        var testable = new TimeSpan(0, 0, 0, 48);
        testable.ToStringHuman().Should().Be("48s");
    }

    [Fact]
    public void ToMinimumString_SecMs_SecMs()
    {
        var testable = new TimeSpan(0, 0, 0, 5, 895);
        testable.ToStringHuman().Should().Be("5s 895ms");
    }

    [Fact]
    public void ToMinimumString_Minutes_NoMilliseconds()
    {
        var testable = new TimeSpan(0, 0, 2, 12, 45);
        testable.ToStringHuman().Should().Be("2 min 12 sec");
    }
}
