using WebApiTemplate.Crosscut.Extensions;
using WebApiTemplate.Crosscut.Services;

namespace WebApiTemplate.Crosscut.Tests;

public class DateTimeExtensionsAgeTests
{
    [Theory]
    [MemberData(nameof(AgeData))]
    public void DateTime_Age_ExpectedResult(DateTime birthDay, int expected)
    {
        var result = birthDay.Age(new DateTime(2023, 5, 14, 18, 12, 41));
        result.Should().Be(expected);
    }

    [Fact]
    public void DateTimeNullable_Age_Returns()
    {
        var birthDay = (DateTime?)new DateTime(2013, 3, 29);
        var result = birthDay.Age(new DateTime(2023, 5, 14, 18, 12, 41));
        result.Should().Be(10);
    }

    [Fact]
    public void DateTimeNullable_NullAge_ReturnsNull()
    {
        var birthDay = (DateTime?)null;
        var result = birthDay.Age(new DateTime(2023, 5, 14, 18, 12, 41));
        result.Should().BeNull();
    }

    public static TheoryData<DateTime, int> AgeData =>
        new()
        {
            { new DateTime(2013, 3, 29), 10 },
            { new DateTime(2013, 11, 2), 9 },
            // Second before birthday
            { new DateTime(2013, 5, 14, 18, 12, 40), 10 },
            // Second after birthday
            { new DateTime(2013, 5, 14, 18, 12, 43), 9 },
            // Same year
            { new DateTime(2023, 12, 23), 0 },
            // Before birthday
            { new DateTime(2022, 8, 10), 0 },
        };
}

internal sealed class TestDateTimeProvider(DateTime currentDateTime) : IDateTimeProvider
{
    public DateTimeOffset DateTimeOffsetNow { get; }

    public DateTimeOffset DateTimeOffsetUtcNow { get; }

    public DateTime DateTimeNow { get; } = currentDateTime;

    public DateTime DateTimeUtcNow { get; }
}
