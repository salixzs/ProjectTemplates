using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsExcelColumnTests
{
    [Theory]
    [InlineData(0, "A")]
    [InlineData(1, "B")]
    [InlineData(3, "D")]
    [InlineData(25, "Z")]
    [InlineData(26, "AA")]
    [InlineData(32, "AG")]
    [InlineData(128, "DY")]
    [InlineData(511, "SR")]
    [InlineData(701, "ZZ")]
    [InlineData(702, "AAA")]
    [InlineData(2048, "BZU")]
    [InlineData(10000, "NTQ")]
    [InlineData(16383, "XFD")]
    public void StringExtTests_ExcelColumnName_AsExpected(int testable, string expected) => testable.ToExcelColumnName().Should().Be(expected);

    [Fact]
    public void StringExtTests_ExcelColumnNameNegative_Throws()
    {
        const int columnIndex = -3;
        Action act = () => columnIndex.ToExcelColumnName();
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void StringExtTests_ExcelColumnNameTooBig_Throws()
    {
        const int columnIndex = 32000;
        Action act = () => columnIndex.ToExcelColumnName();
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
