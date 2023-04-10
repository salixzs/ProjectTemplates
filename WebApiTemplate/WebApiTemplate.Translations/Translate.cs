using System.Globalization;
using Microsoft.Extensions.Localization;

namespace WebApiTemplate.Translations;

public class Translate<T> : ITranslate<T> where T : class
{
    private readonly IStringLocalizer<T> _localizer;

    public Translate(IStringLocalizer<T> localizer) =>
        _localizer = localizer;

    /// <inheritdoc/>
    public virtual string this[string name]
    {
        get
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _localizer[name].Value;
        }
    }

    /// <inheritdoc/>
    public virtual string this[string name, string locale]
    {
        get
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(locale))
            {
                throw new ArgumentNullException(nameof(locale));
            }

            var cultureFromParameter = CultureInfo.CurrentUICulture;
            try
            {
                cultureFromParameter = CultureInfo.GetCultureInfo(locale);
            }
            catch
            {
                return _localizer[name].Value;
            }

            var initialUiCulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentUICulture = cultureFromParameter;

            var translatedString = _localizer[name].Value;

            CultureInfo.CurrentUICulture = initialUiCulture;
            return translatedString;
        }
    }

    /// <inheritdoc/>
    public virtual string this[string name, params object[] arguments]
    {
        get
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _localizer[name, arguments].Value;
        }
    }

    /// <inheritdoc/>
    public virtual string this[string name, string locale, params object[] arguments]
    {
        get
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var cultureFromParameter = CultureInfo.CurrentUICulture;
            try
            {
                cultureFromParameter = CultureInfo.GetCultureInfo(locale);
            }
            catch
            {
                return _localizer[name].Value;
            }

            var initialUiCulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentUICulture = cultureFromParameter;

            var translatedString = _localizer[name, arguments].Value;

            CultureInfo.CurrentUICulture = initialUiCulture;

            return translatedString;
        }
    }
}
