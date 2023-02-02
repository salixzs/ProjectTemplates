using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;
public class DateTimeExtensionsIsBetweenTests
{
    [Theory]
    [MemberData(nameof(BetweenData))]
    public void DateTime_IsBetween_ExpectedResult(DateTime startTime, DateTime endTime, DateTime testDate, bool expected)
    {
        var result = testDate.IsBetween(startTime, endTime);
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(BetweenData))]
    public void DateTimeNullable_IsBetween_ExpectedResult(DateTime startTime, DateTime endTime, DateTime testDate, bool expected)
    {
        DateTime? nullableStartTime = startTime;
        DateTime? nullableEndTime = endTime;
        var result = testDate.IsBetween(nullableStartTime, nullableEndTime);
        result.Should().Be(expected);
    }

    public static TheoryData<DateTime, DateTime, DateTime, bool> BetweenData =>
        new()
        {
            { new DateTime(2017, 2, 1), new DateTime(2017, 2, 20), new DateTime(2017, 2, 10), true },
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 6, 0, 0), true },
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 5, 59, 59), true },
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 6, 0, 2), true },
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 5, 59, 58), false },
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 6, 0, 3), false }
        };
}
