using WebApiTemplate.Translations;

namespace WebApiTemplate.CoreLogic.Tests;

internal static class DefaultMocks
{
    internal static Mock<ITranslate<ValidationTranslations>> GetValidationTranslationMock()
    {
        var l10n = new Mock<ITranslate<ValidationTranslations>>();
        l10n.SetupGet(t => t[It.IsAny<string>()])
            .Returns("Validation translation");
        l10n.SetupGet(t => t[It.IsAny<string>(), It.IsAny<string>()])
            .Returns("Validation translation Lang");
        l10n.SetupGet(t => t[It.IsAny<string>(), It.IsAny<object[]>()])
            .Returns("Validation translation Parms");
        l10n.SetupGet(t => t[It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object[]>()])
            .Returns("Validation translation Lang Parms");
        return l10n;
    }

    internal static Mock<ITranslate<ErrorMessageTranslations>> GetErrorTranslationMock()
    {
        var l10n = new Mock<ITranslate<ErrorMessageTranslations>>();
        l10n.SetupGet(t => t[It.IsAny<string>()])
            .Returns("Error translation");
        l10n.SetupGet(t => t[It.IsAny<string>(), It.IsAny<string>()])
            .Returns("Error translation Lang");
        l10n.SetupGet(t => t[It.IsAny<string>(), It.IsAny<object[]>()])
            .Returns("Error translation Parms");
        l10n.SetupGet(t => t[It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object[]>()])
            .Returns("Error translation Lang Parms");
        return l10n;
    }
}
