using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Text;
using WebApiTemplate.Enumerations;
using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Domain.SystemNotifications;

/// <summary>
/// Database record for System-wide notification.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public class SystemNotification
{
    /// <summary>
    /// Primary unique key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Date and time when notification should start to show up.<br/>
    /// See also <see cref="EndTime">EndTime</see>.<br/>
    /// It is UTC time!
    /// </summary>
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// Date and time when notification should cease to show up.<br/>
    /// See also <see cref="StartTime">StartTime</see>.<br/>
    /// It is UTC time!
    /// </summary>
    public DateTimeOffset EndTime { get; set; }

    /// <summary>
    /// Type (usually - coloring) of notification.
    /// </summary>
    public SystemNotificationType Type { get; set; }

    /// <summary>
    /// Date and time when notification is getting emphasized (bolded).<br/>
    /// Should be either between <see cref="StartTime">StartTime</see> and
    /// before (or equal to) <see cref="EndTime">EndTime</see>.<br/>
    /// When equal to <see cref="EndTime">EndTime</see> will not be emphasized.<br/>
    /// It is UTC time!<br/>
    /// Leave empty (NULL) to disallow emphasizing of the system message.
    /// </summary>
    public DateTimeOffset? EmphasizeSince { get; set; }

    /// <summary>
    /// Type (usually - coloring) of notification when it is in emphasized mode.<br/>
    /// May be different to <see cref="Type">Type</see> to make it more critical.
    /// </summary>
    public SystemNotificationType? EmphasizeType { get; set; }

    /// <summary>
    /// Date and time when notification should include countdown timer to <see cref="EndTime">EndTime</see>.<br/>
    /// Should be either between <see cref="StartTime">StartTime</see> and
    /// before (or equal to) <see cref="EndTime">EndTime</see>.<br/>
    /// When equal to <see cref="EndTime">EndTime</see> will not show countdown timer.<br/>
    /// It is UTC time!<br/>
    /// Leave empty (NULL) to not use countdown timer at all.
    /// </summary>
    public DateTimeOffset? CountdownSince { get; set; }

    /// <summary>
    /// Can hold a link to some page where end-user can find more information on this system notification.<br/>
    /// Can use to add link or button-link to system notification.
    /// </summary>
    public string? MoreInfoUrl { get; set; }

    /// <summary>
    /// A flag, TRUE indicating system notification is created by health check with degraded or unhealthy status.<br/>
    /// Health check with success status will remove this flagged system notification automatically.<br/>
    /// Saving notification will ignore this flag, if set to True. Only Health check can create these.
    /// </summary>
    public bool IsHealthCheck { get; set; }

    /// <summary>
    /// Messages themselves with language codes they are written into.
    /// </summary>
    public virtual List<SystemNotificationMessage> Messages { get; set; } = [];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay
    {
        get
        {
            var debugString = new StringBuilder();
            if (DateTimeOffset.UtcNow.IsBetween(StartTime, EndTime))
            {
                debugString.Append("ACTIVE");
                if (DateTimeOffset.UtcNow.IsBetween(EmphasizeSince, EndTime))
                {
                    debugString.Append("; EMPHASIZED");
                }

                if (DateTimeOffset.UtcNow.IsBetween(CountdownSince, EndTime))
                {
                    debugString.Append("; with TIMER");
                }
                else
                {
                    debugString.Append("; NO timer");
                }
            }
            else
            {
                if (DateTimeOffset.UtcNow < StartTime.UtcDateTime)
                {
                    debugString.Append("WILL START: ");
                    debugString.Append(StartTime.ToString("dd.MMM HH:mm", System.Globalization.CultureInfo.CurrentCulture));
                }

                if (DateTimeOffset.UtcNow > EndTime.UtcDateTime)
                {
                    debugString.Append("ENDED: ");
                    debugString.Append(EndTime.ToString("dd.MMM HH:mm", System.Globalization.CultureInfo.CurrentCulture));
                }
            }

            debugString.Append("; ").Append(Messages.Count).Append(" languages");
            return debugString.ToString();
        }
    }
}
