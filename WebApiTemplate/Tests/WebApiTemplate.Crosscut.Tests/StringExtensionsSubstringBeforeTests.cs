using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsSubstringBeforeTests
{

    [Theory]
    [InlineData("abcde|fgh", "|", "abcde")]
    [InlineData("abc|de|fgh", "|", "abc")]
    [InlineData("abcdefgh", "d", "abc")]
    [InlineData("bazaar", "|", "")]
    [InlineData("querty", "", "")]
    [InlineData("dvorak", null, "")]
    [InlineData(null, "&", null)]
    public void SubstringBefore_Theorem_ExpectedResult(string? input, string? separator, string? expected)
        => input.SubstringBefore(separator).Should().Be(expected);
}
