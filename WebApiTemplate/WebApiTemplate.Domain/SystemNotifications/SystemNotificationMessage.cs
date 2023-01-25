using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace WebApiTemplate.Domain.SystemNotifications;

/// <summary>
/// System notification message in specified language.<br/>
/// Used as list type for system notification messages in some of parent objects.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class SystemNotificationMessage
{
    /// <summary>
    /// Two-letter notification message language code (en, no, lv)<br/>
    /// Reference: <a href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO Language codes</a>
    /// </summary>
    public string Language { get; set; } = "en";

    /// <summary>
    /// Actual notification message in specified <see cref="Language"/>.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay => $"{Language.ToLowerInvariant()}: {Message}";
}
