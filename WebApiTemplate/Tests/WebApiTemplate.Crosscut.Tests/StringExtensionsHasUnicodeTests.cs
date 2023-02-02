using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsHasUnicodeTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("Anrijs", false)]
    [InlineData("1A", false)]
    [InlineData("1 a", false)]
    [InlineData("12.3", false)]
    [InlineData("1_000_000", false)]
    [InlineData("1939308443", false)]
    [InlineData("Anrijs Vītoliņš", true)]
    [InlineData("Väinöstø Åstæveinåland", true)]
    [InlineData("Pądękįrkų Žargōnė", true)]
    [InlineData("Юрий Преснъяков", true)]
    [InlineData("Vaira Vīķe-Freiberga", true)]
    [InlineData("John O'Connor", false)]
    [InlineData("John O`Connor", false)]
    public void StringExtTests_HasUnicode_AsExpected(string testable, bool expected) => testable.ContainsUnicodeCharacter().Should().Be(expected);
}
