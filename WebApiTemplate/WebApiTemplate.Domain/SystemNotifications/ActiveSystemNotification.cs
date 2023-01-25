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
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Type of the notification (decision for coloring, emphasizing).
    /// </summary>
    public SystemNotificationType MessageType { get; set; }

    /// <summary>
    /// Actual notification message(s) in one or more languages.
    /// </summary>
    public List<SystemNotificationMessage> Messages { get; set; } = new List<SystemNotificationMessage>();

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

            debugString.Append($"; {Messages.Count} languages");
            return debugString.ToString();
        }
    }
}
