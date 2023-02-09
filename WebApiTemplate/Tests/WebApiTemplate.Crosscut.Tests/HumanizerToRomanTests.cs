using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class HumanizerToRomanTests
{
    [Theory]
    [InlineData(6, "VI")]
    [InlineData(38, "XXXVIII")]
    [InlineData(64, "LXIV")]
    [InlineData(128, "CXXVIII")]
    [InlineData(256, "CCLVI")]
    [InlineData(512, "DXII")]
    [InlineData(1024, "MXXIV")]
    [InlineData(2048, "MMXLVIII")]
    [InlineData(3999, "MMMCMXCIX")]
    public void ToRoman_Various_ExpectedResult(int inputObject, string expected) =>
        inputObject.ToRoman().Should().Be(expected);
}
