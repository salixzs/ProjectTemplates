using System.Globalization;
using Microsoft.Extensions.Localization;

namespace WebApiTemplate.Translations;

/// <inheritdoc cref="ITranslate{T}"/>
public class Translate<T>(IStringLocalizer<T> localizer) : ITranslate<T> where T : class
{
    /// <inheritdoc/>
    public virtual string this[string name]
    {
        get
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return localizer[name].Value;
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
                cultureFromParameter = CultureInfo.GetCultureInfo(locale is "no" or "nb" ? "nb-NO" : locale);
            }
            catch
            {
                return localizer[name].Value;
            }

            var initialUiCulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentUICulture = cultureFromParameter;

            var translatedString = localizer[name].Value;

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

            return localizer[name, arguments].Value;
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
                cultureFromParameter = CultureInfo.GetCultureInfo(locale is "no" or "nb" ? "nb-NO" : locale);
            }
            catch
            {
                return localizer[name].Value;
            }

            var initialUiCulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentUICulture = cultureFromParameter;

            var translatedString = localizer[name, arguments].Value;

            CultureInfo.CurrentUICulture = initialUiCulture;

            return translatedString;
        }
    }
}
