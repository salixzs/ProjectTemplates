using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsIsLowercaseTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("A", false)]
    [InlineData("a", true)]
    [InlineData("Anrijs", false)]
    [InlineData("anrijs", true)]
    [InlineData("ANRIJS", false)]
    [InlineData("1A", false)]
    [InlineData("A1", false)]
    [InlineData("1a", true)]
    [InlineData("a1", true)]
    [InlineData("12.3", true)]
    [InlineData("12,3", true)]
    [InlineData("abc1ABC", false)]
    [InlineData("PA$$W0RD", false)]
    [InlineData("pa$$w0rd", true)]
    [InlineData("This is Sentence.", false)]
    [InlineData("SCREAMING SENTENCE.", false)]
    [InlineData("shift key is not working.", true)]
    public void StringExtTests_IsUppercase_AsExpected(string testable, bool expected) => testable.IsLowercase().Should().Be(expected);

    [Theory]
    [InlineData("Anrijs", 1, true)]
    [InlineData("anrijs", 1, true)]
    [InlineData("ANRIJS", 1, false)]
    [InlineData("1A", 1, true)]
    [InlineData("A1", 1, true)]
    [InlineData("1a", 1, true)]
    [InlineData("a1", 1, true)]
    [InlineData("12.3", 1, true)]
    [InlineData("12,3", 1, true)]
    [InlineData("abc1ABC", 1, false)]
    [InlineData("PA$$W0RD", 1, false)]
    [InlineData("pa$$w0rd", 1, true)]
    [InlineData("This is Sentence.", 1, false)]
    [InlineData("This is Sentence.", 2, true)]
    [InlineData("SCREAMING SENTENCE.", 5, false)]
    [InlineData("shift key is not working.", 3, true)]
    public void StringExtTests_IsUppercaseAllowed_AsExpected(string testable, int allowed, bool expected) => testable.IsLowercase(allowed).Should().Be(expected);
}
