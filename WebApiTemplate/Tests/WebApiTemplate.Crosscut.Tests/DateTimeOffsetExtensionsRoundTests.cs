using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;
public class DateTimeOffsetExtensionsRoundTests
{
    [Theory]
    [MemberData(nameof(RoundingData))]
    public void DateTimeOffset_Round_ExpectedResult(DateTimeOffset originalDateTime, DateTimeOffset roundedDateTime, long scope)
    {
        var result = originalDateTime.Round(scope);
        result.Should().Be(roundedDateTime);
    }

    [Fact]
    public void DateTimeOffsetNullable_RoundNull_ReturnsNull()
    {
        var nullableDate = (DateTimeOffset?)null;
        var result = nullableDate.Round();
        result.Should().BeNull();
    }

    [Fact]
    public void DateTimeOffsetNullable_Round_Works()
    {
        var nullableDate = (DateTimeOffset?)new DateTimeOffset(2023, 4, 5, 6, 7, 8, 323, new TimeSpan(2, 0, 0));
        var result = nullableDate.Round();
        result.Should().Be(new DateTimeOffset(2023, 4, 5, 6, 7, 8, 0, 0, new TimeSpan(2, 0, 0)));
    }

    public static TheoryData<DateTimeOffset, DateTimeOffset, long> RoundingData =>
        new()
        {
            { new DateTimeOffset(2023, 4, 5, 6, 7, 8, 323, new TimeSpan(2, 0, 0)), new DateTimeOffset(2023, 4, 5, 6, 7, 8, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerSecond },
            { new DateTimeOffset(2023, 4, 5, 6, 7, 8, 869, new TimeSpan(2, 0, 0)), new DateTimeOffset(2023, 4, 5, 6, 7, 9, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerSecond },
            { new DateTimeOffset(2023, 4, 5, 6, 7, 8, new TimeSpan(2, 0, 0)), new DateTimeOffset(2023, 4, 5, 6, 7, 0, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerMinute },
            { new DateTimeOffset(2023, 4, 5, 6, 8, 48, new TimeSpan(2, 0, 0)) , new DateTimeOffset(2023, 4, 5, 6, 9, 0, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerMinute },
            { new DateTimeOffset(2023, 4, 5, 6, 7, 8, new TimeSpan(2, 0, 0)), new DateTimeOffset(2023, 4, 5, 6, 0, 0, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerHour },
            { new DateTimeOffset(2023, 4, 5, 6, 37, 48, new TimeSpan(2, 0, 0)), new DateTimeOffset(2023, 4, 5, 7, 0, 0, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerHour },
            { new DateTimeOffset(2023, 4, 5, 6, 7, 8, new TimeSpan(2, 0, 0)), new DateTimeOffset(2023, 4, 5, 0, 0, 0, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerDay },
            { new DateTimeOffset(2023, 4, 5, 21, 37, 48, new TimeSpan(2, 0, 0)), new DateTimeOffset(2023, 4, 6, 0, 0, 0, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerDay },
            { new DateTimeOffset(2023, 4, 5, 0, 0, 0, new TimeSpan(2, 0, 0)), new DateTimeOffset(2023, 4, 5, 0, 0, 0, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerDay },
            { new DateTimeOffset(2023, 4, 5, 6, 7, 8, 0, new TimeSpan(2, 0, 0)), new DateTimeOffset(2023, 4, 5, 6, 7, 8, new TimeSpan(2, 0, 0)), TimeSpan.TicksPerSecond },
        };
}
