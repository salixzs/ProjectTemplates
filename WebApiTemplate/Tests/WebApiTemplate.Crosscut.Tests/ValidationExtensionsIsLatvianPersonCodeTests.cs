using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class ValidationExtensionsIsLatvianPersonCodeTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("  ", true)]
    [InlineData("030468-12236", true)]
    [InlineData("180983-11400", true)]
    [InlineData("210903-22452", true)]
    [InlineData("070162-11374", true)]
    [InlineData("320000-00001", true)]
    [InlineData("330000-00001", true)]
    [InlineData("340000-00001", true)]
    [InlineData("350000-00001", true)]
    [InlineData("360000-00001", true)]
    [InlineData("370000-00001", true)]
    [InlineData("380000-00001", true)]
    [InlineData("390000-00001", true)]

    [InlineData("030468-12237", false)]
    [InlineData("123123-12345", false)]
    [InlineData("210903-22451", false)]
    [InlineData("400000-00001", false)]
    [InlineData("420000-00001", false)]
    [InlineData("590000-00001", false)]
    public void IsValidPersonCode_AsExpected(string? testable, bool expected) =>
        testable.IsValidPersonCode().Should().Be(expected);
}
