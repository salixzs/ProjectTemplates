using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using WebApiTemplate.Enumerations;
using System.Text;
using System.Globalization;

namespace WebApiTemplate.Domain.SystemNotifications;

/// <summary>
/// Data object to transfer active system notifications.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class ActiveSystemNotification
{
    /// <summary>
    /// Internal unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Flag, indicating whether notification should be emphasized.
    /// </summary>
    public bool IsEmphasized { get; set; }

    /// <summary>
    /// Flag, indicating whether countdown timer should be shown together with message.
    /// </summary>
    public bool ShowCountdown { get; set; }

    /// <summary>
    /// End time of system notification. It is time to which <see cref="ShowCountdown"/> should countdown time.
    /// </summary>
    public DateTimeOffset EndTime { get; set; }

    /// <summary>
    /// Type of the notification (decision for coloring, emphasizing).
    /// </summary>
    public SystemNotificationType MessageType { get; set; }

    /// <summary>
    /// Can hold a link to some page where end-user can find more information on this system notification.
    /// </summary>
    public string? MoreInfoUrl { get; set; }

    /// <summary>
    /// Specifies whether user can dismiss (close) notification and how he can dismiss this notification (Forever, UntilEmphasize, ForOneDay, UntilCountdown).
    /// UI should implement dismissal check based on this enumeration value.
    /// </summary>
    public SystemNotificationUserDismissType UserDismissType { get; set; }

    /// <summary>
    /// Actual notification message(s) in one or more languages.
    /// </summary>
    public List<SystemNotificationMessage> Messages { get; set; } = [];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay
    {
        get
        {
            var debugString = new StringBuilder();
            debugString.Append(MessageType.ToString());
            if (IsEmphasized)
            {
                debugString.Append("; EMPHASIZED");
            }

            if (ShowCountdown)
            {
                debugString.Append("; with TIMER");
                debugString.Append(" until ");
                debugString.Append(EndTime.ToString("d. MMM, HH:mm", CultureInfo.CurrentCulture));
            }

            debugString.Append("; ").Append(Messages.Count).Append(" languages");
            return debugString.ToString();
        }
    }
}
