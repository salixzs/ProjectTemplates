using System.Text;
using WebApiTemplate.Crosscut.Extensions;
using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Database.Orm.Entities;

/// <summary>
/// Database record for System-wide notification.
/// <code>
/// TABLE: SystemNotifications
/// </code>
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public class SystemNotificationRecord
{
    /// <summary>
    /// Primary key.
    /// <code>
    /// [Id] INT NOT NULL IDENTITY (1000, 1)
    /// </code>
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Date and time when notification should start to show up.<br/>
    /// See also <see cref="EndTime">EndTime</see>.<br/>
    /// It is UTC time!
    /// <code>
    /// [StartTime] DATETIME NOT NULL
    /// </code>
    /// </summary>
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// Date and time when notification should cease to show up.<br/>
    /// See also <see cref="StartTime">StartTime</see>.<br/>
    /// It is UTC time!
    /// <code>
    /// [EndTime] DATETIME NOT NULL
    /// </code>
    /// </summary>
    public DateTimeOffset EndTime { get; set; }

    /// <summary>
    /// Type (usually - coloring) of notification.
    /// <code>
    /// [Type] TINYINT NOT NULL DEFAULT 0
    /// </code>
    /// </summary>
    public SystemNotificationType Type { get; set; }

    /// <summary>
    /// Date and time when notification is getting emphasized (bolded).<br/>
    /// Should be either between <see cref="StartTime">StartTime</see> and
    /// before (or equal to) <see cref="EndTime">EndTime</see>.<br/>
    /// When equal to <see cref="EndTime">EndTime</see> will not be emphasized.<br/>
    /// It is UTC time!
    /// <code>
    /// [EmphasizeSince] DATETIME NOT NULL
    /// </code>
    /// </summary>
    public DateTimeOffset EmphasizeSince { get; set; }

    /// <summary>
    /// Type (usually - coloring) of notification when it is in emphasized mode.<br/>
    /// May be different to <see cref="Type">Tyoe</see> to make it more critical.
    /// <code>
    /// [EmphasizeType] TINYINT NOT NULL DEFAULT 0
    /// </code>
    /// </summary>
    public SystemNotificationType EmphasizeType { get; set; }

    /// <summary>
    /// Date and time when notification should include countdown timer to <see cref="EndTime">EndTime</see>.<br/>
    /// Should be either between <see cref="StartTime">StartTime</see> and
    /// before (or equal to) <see cref="EndTime">EndTime</see>.<br/>
    /// When equal to <see cref="EndTime">EndTime</see> will not show countdown timer.<br/>
    /// It is UTC time!
    /// <code>
    /// [CountdownSince] DATETIME NOT NULL
    /// </code>
    /// </summary>
    public DateTimeOffset CountdownSince { get; set; }

    /// <summary>
    /// Messages themselves with language codes they are written into.
    /// <code>
    /// FK from [SystemNotificationMessages] table records.
    /// </code>
    /// </summary>
    public virtual ICollection<SystemNotificationMessageRecord> Messages { get; set; } = new HashSet<SystemNotificationMessageRecord>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay
    {
        get
        {
            var debugString = new StringBuilder();
            if (DateTimeOffset.Now.IsBetween(StartTime, EndTime))
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
                    debugString.Append($"WILL START: ");
                    debugString.Append(StartTime.ToString("dd.MMM HH:mm", System.Globalization.CultureInfo.CurrentCulture));
                }

                if (DateTimeOffset.UtcNow > EndTime.UtcDateTime)
                {
                    debugString.Append($"ENDED: ");
                    debugString.Append(EndTime.ToString("dd.MMM HH:mm", System.Globalization.CultureInfo.CurrentCulture));
                }
            }

            return debugString.ToString();
        }
    }
}
