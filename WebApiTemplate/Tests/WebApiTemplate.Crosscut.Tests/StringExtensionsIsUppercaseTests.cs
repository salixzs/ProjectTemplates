using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsIsUppercaseTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("A", true)]
    [InlineData("a", false)]
    [InlineData("Anrijs", false)]
    [InlineData("anrijs", false)]
    [InlineData("ANRIJS", true)]
    [InlineData("1A", true)]
    [InlineData("A1", true)]
    [InlineData("1a", false)]
    [InlineData("a1", false)]
    [InlineData("12.3", true)]
    [InlineData("12,3", true)]
    [InlineData("abc1ABC", false)]
    [InlineData("PA$$W0RD", true)]
    [InlineData("This is Sentence.", false)]
    [InlineData("SCREAMING SENTENCE.", true)]
    public void StringExtTests_IsUppercase_AsExpected(string testable, bool expected) => testable.IsUppercase().Should().Be(expected);

    [Theory]
    [InlineData(null, 1, false)]
    [InlineData("", 1, false)]
    [InlineData("A", 1, true)]
    [InlineData("a", 1, true)]
    [InlineData("Anrijs", 1, false)]
    [InlineData("anrijs", 1, false)]
    [InlineData("ANRIJS", 1, true)]
    [InlineData("1A", 1, true)]
    [InlineData("A1", 1, true)]
    [InlineData("1a", 1, true)]
    [InlineData("a1", 1, true)]
    [InlineData("12.3", 1, true)]
    [InlineData("12,3", 1, true)]
    [InlineData("ab1AB", 1, false)]
    [InlineData("ab1AB", 2, true)]
    [InlineData("PA$$W0RD", 1, true)]
    [InlineData("PA$$W0rd", 2, true)]
    public void StringExtTests_IsUppercaseAllowed_AsExpected(string testable, int allowed, bool expected) => testable.IsUppercase(allowed).Should().Be(expected);
}
