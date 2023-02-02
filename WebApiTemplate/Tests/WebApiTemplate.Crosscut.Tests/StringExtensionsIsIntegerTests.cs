using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsIsIntegerTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("Anrijs", false)]
    [InlineData("1A", false)]
    [InlineData("A1", false)]
    [InlineData("1 a", false)]
    [InlineData("a 1", false)]
    [InlineData("12.3", false)]
    [InlineData("12,3", false)]
    [InlineData("1 000 000", false)]
    [InlineData("1_000_000", false)]
    [InlineData("1939308443", true)]
    public void StringExtTests_IsInteger_AsExpected(string testable, bool expected) => testable.IsInteger().Should().Be(expected);
}
