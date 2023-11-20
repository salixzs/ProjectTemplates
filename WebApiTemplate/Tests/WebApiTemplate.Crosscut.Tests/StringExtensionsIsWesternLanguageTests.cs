using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsIsWesternLanguageTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("Anrijs", true)]
    [InlineData("1A", true)]
    [InlineData("1 a", true)]
    [InlineData("12.3", true)]
    [InlineData("1_000_000", true)]
    [InlineData("1939308443", true)]
    [InlineData("Anrijs Vītoliņš", true)]
    [InlineData("Väinöstø Åstæveinåland", true)]
    [InlineData("Pądękįrkų Žargōnė", true)]
    [InlineData("Юрий Преснъяков", false)]
    [InlineData("Κυρηναῖοι Ῥαιτικῆς", false)]
    [InlineData("ყველა ადამიანი", false)]
    [InlineData("王孙董", false)]
    public void StringExtTests_IsWesternLanguage_AsExpected(string? testable, bool expected) => testable.IsWesternLanguage().Should().Be(expected);
}
