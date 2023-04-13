using System.Globalization;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace WebApiTemplate.Translations.Tests;

public class TranslateStringTests
{
    private readonly Translate<ValidationTranslations> _sut;

    public TranslateStringTests()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("lv");
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("lv");
        var options = Options.Create(new LocalizationOptions());
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        _sut = new Translate<ValidationTranslations>(new StringLocalizer<ValidationTranslations>(factory));
    }

    [Fact]
    public void String_Default_Latvian() => _sut["Test"].Should().Be("Šis ir tests.");

    [Fact]
    public void String_Latvian_Latvian() => _sut["Test", "lv"].Should().Be("Šis ir tests.");

    [Fact]
    public void String_English_English() => _sut["Test", "en"].Should().Be("This is a test.");

    [Fact]
    public void String_WrongKey_GivenKey() => _sut["Wrongest"].Should().Be("Wrongest");

    [Fact]
    public void String_NonExistingLocale_DefaultEnglish() => _sut["Test", "jp"].Should().Be("This is a test.");

    [Fact]
    public void String_Case_Sensitive() => _sut["tESt"].Should().Be("tESt");

    [Fact]
    public void StringPlaceholder_Default_Latvian() => _sut["TestPlaceholder", 12].Should().Be("Aizvietots: 12 tests.");

    [Fact]
    public void StringPlaceholder_Norwegian_Norwegian() => _sut["TestPlaceholder", "no", 21].Should().Be("Plassholder: 21 test.");

    [Fact]
    public void StringPlaceholder_English_English() => _sut["TestPlaceholder", "en", 777].Should().Be("Placeholder: 777 test.");

    [Fact]
    public void StringPlaceholder_NonExistingLocale_DefaultEnglish() => _sut["TestPlaceholder", "jp", 18].Should().Be("Placeholder: 18 test.");

    [Fact]
    public void String_Empty_Throws()
    {
        Func<string> act = () => _sut[string.Empty];
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void StringPlaceholder_Empty_Throws()
    {
        Func<string> act = () => _sut[string.Empty, 12];
        act.Should().Throw<ArgumentNullException>();
    }
}
