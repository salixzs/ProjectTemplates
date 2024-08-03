namespace WebApiTemplate.Translations;

/// <summary>
/// Resource-based translation functionality for string translations in supported languages.<br/>
/// Use as property getter with key of resource to get its translation into CurrentLocale or given locale.
/// <code>
/// constructor(ITranslate&lt;ResxName&gt; l10n)
///
/// var translation = l10n["Key"];
/// </code>
/// </summary>
/// <typeparam name="T">Type of Resource (RESX)</typeparam>
public interface ITranslate<T> where T : class
{
    /// <summary>
    /// Gets the string resource (translation) with the given name (key).<br />
    /// Name (key) is case sensitive!<br/>
    /// <code>
    /// var translation = Translate["GivenKey"]
    /// </code>
    /// </summary>
    /// <param name="name">The name of the string resource.</param>
    /// <exception cref="ArgumentNullException">Name is null or empty.</exception>
    string this[string name] { get; }

    /// <summary>
    /// Gets the string resource (translation) with the given name (key) and in specified locale (if it is supported).<br />
    /// Name (key) is case sensitive!<br/>
    /// <code>
    /// var translation = Translate["GivenKey", "lv"]
    /// </code>
    /// </summary>
    /// <param name="name">The name of the string resource.</param>
    /// <param name="locale">Optional parameter to specify different locale (language to translate).</param>
    /// <exception cref="ArgumentNullException">Name is null or empty.</exception>
    string this[string name, string locale] { get; }

    /// <summary>
    /// Gets the string resource (translation) with the given name (key) and replacing placeholder with given argument value(s).<br />
    /// Name (key) is case sensitive!<br/>
    /// <code>
    /// // GivenKey has value of "Something {0} with placeholder"
    /// var translation = Translate["GivenKey", 12]
    /// </code>
    /// </summary>
    /// <param name="name">The name of the string resource.</param>
    /// <param name="arguments">One or more arguments, matching count of placeholders in translated string.</param>
    /// <exception cref="ArgumentNullException">Name is null or empty.</exception>
    string this[string name, params object[] arguments] { get; }

    /// <summary>
    /// Gets the string resource (translation) in specified locale with the given name (key) and replacing placeholder with given argument value(s).<br />
    /// Name (key) is case sensitive!<br/>
    /// <code>
    /// // GivenKey has value of "Something {0} with placeholder"
    /// var translation = Translate["GivenKey", "en", 12]
    /// </code>
    /// </summary>
    /// <param name="name">The name of the string resource.</param>
    /// <param name="locale">Optional parameter to specify different locale (language to translate).</param>
    /// <param name="arguments">One or more arguments, matching count of placeholders in translated string.</param>
    /// <exception cref="ArgumentNullException">Name is null or empty.</exception>
    string this[string name, string locale, params object[] arguments] { get; }
}
