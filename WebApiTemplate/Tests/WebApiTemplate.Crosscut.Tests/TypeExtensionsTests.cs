using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class TypeExtensionsTests
{
    [Fact]
    public void IsSimple_Framework_True()
    {
        typeof(int).IsSimple().Should().BeTrue();
        typeof(short).IsSimple().Should().BeTrue();
        typeof(double).IsSimple().Should().BeTrue();
        typeof(decimal).IsSimple().Should().BeTrue();
        typeof(long).IsSimple().Should().BeTrue();
        typeof(float).IsSimple().Should().BeTrue();
        typeof(string).IsSimple().Should().BeTrue();
        typeof(DateTime).IsSimple().Should().BeTrue();
        typeof(DateTimeOffset).IsSimple().Should().BeTrue();
        typeof(TimeSpan).IsSimple().Should().BeTrue();
    }

    [Fact]
    public void IsSimple_FrameworkNullable_True()
    {
        typeof(int?).IsSimple().Should().BeTrue();
        typeof(short?).IsSimple().Should().BeTrue();
        typeof(double?).IsSimple().Should().BeTrue();
        typeof(decimal?).IsSimple().Should().BeTrue();
        typeof(long?).IsSimple().Should().BeTrue();
        typeof(float?).IsSimple().Should().BeTrue();
        typeof(DateTime?).IsSimple().Should().BeTrue();
        typeof(DateTimeOffset?).IsSimple().Should().BeTrue();
        typeof(TimeSpan?).IsSimple().Should().BeTrue();
    }

    [Fact]
    public void IsSimple_Complex_False() =>
        typeof(ArgumentException).IsSimple().Should().BeFalse();
}
