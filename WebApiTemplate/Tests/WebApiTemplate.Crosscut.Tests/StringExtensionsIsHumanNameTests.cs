using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsIsHumanNameTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("Anrijs", true)]
    [InlineData("1A", false)]
    [InlineData("A1", false)]
    [InlineData("1 a", false)]
    [InlineData("a 1", false)]
    [InlineData("12.3", false)]
    [InlineData("12,3", false)]
    [InlineData("1 000 000", false)]
    [InlineData("1_000_000", false)]
    [InlineData("1939308443", false)]
    [InlineData("-", false)]
    [InlineData("?", false)]
    [InlineData("a", false)]
    [InlineData("a-", false)]
    [InlineData("Yo", true)]
    [InlineData("Anrijs Vītoliņš", true)]
    [InlineData("Väinöstø Åstæveinåland", true)]
    [InlineData("Pądękįrkų Žargōnė", true)]
    [InlineData("Юрий Преснъяков", true)]
    [InlineData("Vaira Vīķe-Freiberga", true)]
    [InlineData("John McLaud", true)]
    [InlineData("John O'Connor", true)]
    [InlineData("John O`Connor", true)]
    [InlineData("John Brown IV", true)]
    public void StringExtTests_IsHumanName_AsExpected(string? testable, bool expected) => testable.IsHumanName().Should().Be(expected);
}
