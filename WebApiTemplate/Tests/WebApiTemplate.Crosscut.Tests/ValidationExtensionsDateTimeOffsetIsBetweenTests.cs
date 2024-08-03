using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class ValidationExtensionsDateTimeOffsetIsBetweenTests
{
    [Theory]
    [MemberData(nameof(BetweenData))]
    public void DateTimeOffset_IsBetween_ExpectedResult(DateTimeOffset startTime, DateTimeOffset endTime, DateTimeOffset testDate, bool expected)
    {
        var result = testDate.IsBetween(startTime, endTime);
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(BetweenData))]
    public void DateTimeOffsetNullable_IsBetween_ExpectedResult(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        DateTimeOffset testDate,
        bool expected)
    {
        DateTimeOffset? nullableStartTime = startTime;
        DateTimeOffset? nullableEndTime = endTime;
        var result = testDate.IsBetween(nullableStartTime, nullableEndTime);
        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(BetweenNullData))]
    public void DateTimeOffsetNullable_IsBetweenNull_ExpectedResult(
        DateTimeOffset? startTime,
        DateTimeOffset? endTime,
        DateTimeOffset testDate,
        bool expected)
    {
        var nullableStartTime = startTime;
        var nullableEndTime = endTime;
        var result = testDate.IsBetween(nullableStartTime, nullableEndTime);
        result.Should().Be(expected);
    }

#pragma warning disable RCS0056 // A line is too long.
    public static TheoryData<DateTimeOffset, DateTimeOffset, DateTimeOffset, bool> BetweenData =>
        new()
        {
            // within days
            { new DateTimeOffset(2017, 2, 1, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2017, 2, 20, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2017, 2, 10, 12, 0, 0, TimeSpan.Zero), true },
            // within seconds
            { new DateTimeOffset(2017, 2, 1, 5, 59, 59, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 2, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 0, TimeSpan.Zero), true },
            // == start time
            { new DateTimeOffset(2017, 2, 1, 5, 59, 59, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 2, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 5, 59, 59, TimeSpan.Zero), true },
            // == end time time
            { new DateTimeOffset(2017, 2, 1, 5, 59, 59, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 2, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 2, TimeSpan.Zero), true },
            // second before start time
            { new DateTimeOffset(2017, 2, 1, 5, 59, 59, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 2, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 5, 59, 58, TimeSpan.Zero), false },
            // second after end time
            { new DateTimeOffset(2017, 2, 1, 5, 59, 59, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 2, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 3, TimeSpan.Zero), false },
            // quite before start time
            { new DateTimeOffset(2017, 2, 1, 5, 59, 59, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 2, TimeSpan.Zero), new DateTimeOffset(2017, 1, 29, 14, 25, 3, TimeSpan.Zero), false },
            // quite after end time
            { new DateTimeOffset(2017, 2, 1, 5, 59, 59, TimeSpan.Zero), new DateTimeOffset(2017, 2, 1, 6, 0, 2, TimeSpan.Zero), new DateTimeOffset(2017, 3, 29, 14, 25, 3, TimeSpan.Zero), false },

            // TIMEZONEs
            { new DateTimeOffset(2017, 2, 1, 5, 59, 59, new TimeSpan(-2, 0, 0)), new DateTimeOffset(2017, 2, 1, 6, 0, 2, new TimeSpan(-2, 0, 0)), new DateTimeOffset(2017, 1, 4, 14, 25, 3, new TimeSpan(2, 0, 0)), false },
        };
#pragma warning restore RCS0056 // A line is too long.

    public static TheoryData<DateTimeOffset?, DateTimeOffset?, DateTimeOffset, bool> BetweenNullData =>
        new()
        {
            { new DateTimeOffset(2017, 2, 1, 12, 0, 0, TimeSpan.Zero), null, new DateTimeOffset(2017, 2, 10, 12, 0, 0, TimeSpan.Zero), true },
            { new DateTimeOffset(2017, 2, 1, 12, 0, 0, TimeSpan.Zero), null, new DateTimeOffset(2017, 1, 21, 12, 0, 0, TimeSpan.Zero), false },
            { null, new DateTimeOffset(2017, 2, 18, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2017, 2, 10, 12, 0, 0, TimeSpan.Zero), true },
            { null, new DateTimeOffset(2017, 2, 18, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2017, 2, 19, 12, 0, 0, TimeSpan.Zero), false },
        };

    [Fact]
    public void DateTimeNullable_IsBetweenSwap_Throws()
    {
        var startDate = new DateTimeOffset(2017, 2, 15, 12, 0, 0, TimeSpan.Zero);
        var endDate = new DateTimeOffset(2017, 2, 2, 12, 0, 0, TimeSpan.Zero);
        var testDate = new DateTimeOffset(2017, 2, 10, 12, 0, 0, TimeSpan.Zero);
        Action act = () => testDate.IsBetween(startDate, endDate);
        act.Should().Throw<ArgumentException>();
    }
}
