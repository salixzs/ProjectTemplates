using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;
public class DateTimeExtensionsRoundTests
{
    [Theory]
    [MemberData(nameof(RoundingData))]
    public void DateTime_Round_ExpectedResult(DateTime originalDateTime, DateTime roundedDateTime, long scope)
    {
        var result = originalDateTime.Round(scope);
        result.Should().Be(roundedDateTime);
    }

    [Fact]
    public void DateTimeNullable_RoundNull_ReturnsNull()
    {
        var nullableDate = (DateTime?)null;
        var result = nullableDate.Round();
        result.Should().BeNull();
    }

    [Fact]
    public void DateTimeNullable_Round_Works()
    {
        var nullableDate = (DateTime?)new DateTime(2023, 4, 5, 6, 7, 8, 323);
        var result = nullableDate.Round();
        result.Should().Be(new DateTime(2023, 4, 5, 6, 7, 8));
    }

    public static TheoryData<DateTime, DateTime, long> RoundingData =>
        new()
        {
            { new DateTime(2023, 4, 5, 6, 7, 8, 323), new DateTime(2023, 4, 5, 6, 7, 8), TimeSpan.TicksPerSecond },
            { new DateTime(2023, 4, 5, 6, 7, 8, 869), new DateTime(2023, 4, 5, 6, 7, 9), TimeSpan.TicksPerSecond },
            { new DateTime(2023, 4, 5, 6, 7, 8), new DateTime(2023, 4, 5, 6, 7, 0), TimeSpan.TicksPerMinute },
            { new DateTime(2023, 4, 5, 6, 7, 48), new DateTime(2023, 4, 5, 6, 8, 0), TimeSpan.TicksPerMinute },
            { new DateTime(2023, 4, 5, 6, 7, 8), new DateTime(2023, 4, 5, 6, 0, 0), TimeSpan.TicksPerHour },
            { new DateTime(2023, 4, 5, 6, 37, 48), new DateTime(2023, 4, 5, 7, 0, 0), TimeSpan.TicksPerHour },
            { new DateTime(2023, 4, 5, 6, 7, 8), new DateTime(2023, 4, 5, 0, 0, 0), TimeSpan.TicksPerDay },
            { new DateTime(2023, 4, 5, 21, 37, 48), new DateTime(2023, 4, 6, 0, 0, 0), TimeSpan.TicksPerDay },
            { new DateTime(2023, 4, 5, 0, 0, 0), new DateTime(2023, 4, 5, 0, 0, 0), TimeSpan.TicksPerDay },
            { new DateTime(2023, 4, 5, 6, 7, 8, 0), new DateTime(2023, 4, 5, 6, 7, 8), TimeSpan.TicksPerSecond },
        };
}
