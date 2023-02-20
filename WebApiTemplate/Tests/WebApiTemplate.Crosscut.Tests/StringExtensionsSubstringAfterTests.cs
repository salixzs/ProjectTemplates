using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsSubstringAfterTests
{
    [Theory]
    [InlineData("abcde|fgh", "|", "fgh")]
    [InlineData("abc|de|fgh", "|", "de|fgh")]
    [InlineData("abcdefgh", "d", "efgh")]
    [InlineData("bazaar", "|", "")]
    [InlineData("qwerty", "", "")]
    [InlineData("dvorak", null, "")]
    [InlineData(null, "&", null)]
    public void SubstringAfter_Theorem_ExpectedResult(string? input, string? separator, string? expected) =>
        input.SubstringAfter(separator).Should().Be(expected);
}
