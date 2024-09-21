using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsIsAlphaOnlyTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("1A", false)]
    [InlineData("A1", false)]
    [InlineData("1 a", false)]
    [InlineData("a 1", false)]
    [InlineData("12.3", false)]
    [InlineData("12,3", false)]
    [InlineData("1 000 000", false)]
    [InlineData("1_000_000", false)]
    [InlineData("1939308443", false)]
    [InlineData("Anrijs", true)]
    [InlineData("Anrijs Vītoliņš", true)]
    [InlineData("Väinöstø Åstæveinåland", true)]
    [InlineData("Pądękįrkų Žargōnė", true)]
    [InlineData("Юрий Преснъяков", true)]
    public void StringExtTests_IsAlphaOnly_AsExpected(string? testable, bool expected) => testable.IsAlphaOnly().Should().Be(expected);
}
