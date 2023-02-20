using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsTruncateTests
{
    [Theory]
    [InlineData("Greater", 5, "Great")]
    [InlineData("Greater", 15, "Greater")]
    [InlineData("", 10, "")]
    [InlineData(null, 10, null)]
    public void Truncate_VariousNullable_AreExpected(string? input, int maxLength, string expected) =>
        input.Truncate(maxLength).Should().Be(expected);

    [Theory]
    [InlineData("Greater", 5, "Great")]
    [InlineData("Greater", 15, "Greater")]
    [InlineData("", 10, "")]
    public void Truncate_Various_AreExpected(string input, int maxLength, string expected) =>
        input.Truncate(maxLength).Should().Be(expected);
}
