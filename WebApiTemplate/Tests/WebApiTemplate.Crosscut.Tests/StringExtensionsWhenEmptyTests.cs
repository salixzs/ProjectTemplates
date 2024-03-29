using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class StringExtensionsWhenEmptyTests
{
    [Fact]
    public void WhenEmpty_NonEmpty_ReturnsSameText()
    {
        const string testable = "value is in";
        testable.WhenEmpty("not replaced!").Should().Be("value is in");
    }

    [Fact]
    public void WhenEmpty_Empty_ReturnsNewText()
    {
        var testable = string.Empty;
        testable.WhenEmpty("is replaced!").Should().Be("is replaced!");
    }

    [Fact]
    public void WhenEmpty_Null_ReturnsNewText()
    {
        const string? testable = null;
        testable.WhenEmpty("is replaced!").Should().Be("is replaced!");
    }
}
