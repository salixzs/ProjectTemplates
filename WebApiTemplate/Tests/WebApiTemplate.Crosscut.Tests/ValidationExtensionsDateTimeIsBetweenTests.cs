using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class ValidationExtensionsDateTimeIsBetweenTests
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

    [Theory]
    [MemberData(nameof(BetweenNullData))]
    public void DateTimeNullable_IsBetweenNull_ExpectedResult(DateTime? startTime, DateTime? endTime, DateTime testDate, bool expected)
    {
        var nullableStartTime = startTime;
        var nullableEndTime = endTime;
        var result = testDate.IsBetween(nullableStartTime, nullableEndTime);
        result.Should().Be(expected);
    }

    public static TheoryData<DateTime, DateTime, DateTime, bool> BetweenData =>
        new()
        {
            // within days
            { new DateTime(2017, 2, 1), new DateTime(2017, 2, 20), new DateTime(2017, 2, 10), true },
            // within seconds
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 6, 0, 0), true },
            // same as start time
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 5, 59, 59), true },
            // same as end time
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 6, 0, 2), true },
            // second before start time
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 5, 59, 58), false },
            // second after end time
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 2, 1, 6, 0, 3), false },
            // long before start time
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 1, 29, 14, 25, 3), false },
            // long after end time
            { new DateTime(2017, 2, 1, 5, 59, 59), new DateTime(2017, 2, 1, 6, 0, 2), new DateTime(2017, 3, 29, 14, 25, 3), false }
        };

    public static TheoryData<DateTime?, DateTime?, DateTime, bool> BetweenNullData =>
        new()
        {
            // null end time, after start time
            { new DateTime(2017, 2, 1), null, new DateTime(2017, 2, 10), true },
            // null end time, before start time
            { new DateTime(2017, 2, 1), null, new DateTime(2017, 1, 21), false },
            // null start time, before end time
            { null, new DateTime(2017, 2, 18), new DateTime(2017, 2, 10), true },
            // null start time after end time
            { null, new DateTime(2017, 2, 18), new DateTime(2017, 2, 19), false },
        };

    [Fact]
    public void DateTimeNullable_IsBetweenSwap_Throws()
    {
        var startDate = new DateTime(2017, 2, 15);
        var endDate = new DateTime(2017, 2, 2);
        var testDate = new DateTime(2017, 2, 10);
        Action act = () => testDate.IsBetween(startDate, endDate);
        act.Should().Throw<ArgumentException>();
    }
}
