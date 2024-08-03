using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class ValidationExtensionsIsValidEmailSyntaxTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("  ", true)]
    [InlineData("salixzs@gmail.com", true)]
    [InlineData("1@2.3", true)]
    [InlineData("jons.plunts@domain.com", true)]
    [InlineData("jons_plunts@domain.com", true)]
    [InlineData("jons-plunts@domain.com", true)]
    [InlineData("noreply@domain.subdomain.com", true)]
    [InlineData("@.", false)]
    [InlineData("fake@.com", false)]
    [InlineData("@.com", false)]
    [InlineData("@domain.com", false)]
    [InlineData("name@", false)]
    [InlineData("name.com", false)]
    [InlineData("some.in@com", false)]
    public void StringExtTests_IsEmail_AsExpected(string? testable, bool expected) => testable.IsValidEmailSyntax().Should().Be(expected);
}
