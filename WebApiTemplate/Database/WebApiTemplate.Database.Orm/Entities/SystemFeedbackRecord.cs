using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Database.Orm.Entities;

/// <summary>
/// Database record for end user feedback (or bug, change request, support request).
/// <code>
/// TABLE: SystemFeedbacks
/// </code>
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public class SystemFeedbackRecord
{
    /// <summary>
    /// Primary key.
    /// <code>
    /// [Id] INT NOT NULL IDENTITY (1000, 1)
    /// </code>
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Summary title/name of feedback item.
    /// <code>
    /// [Title] NVARCHAR(200) NOT NULL
    /// </code>
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Feedback content - description of joy or problem.
    /// <code>
    /// [Content] NVARCHAR(MAX) NULL
    /// </code>
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Technical part of the feedback. Normally should be populated with technical context,
    /// like Stack Trace, request URL etc.
    /// <code>
    /// [SystemInfo] NVARCHAR(MAX) NULL
    /// </code>
    /// </summary>
    public string? SystemInfo { get; set; }

    /// <summary>
    /// Category (type) of the user feedback.<br/>
    /// Feedback, Bug report, Support ticket and similar.
    /// <code>
    /// [Category] INT NOT NULL
    /// </code>
    /// </summary>
    public SystemFeedbackCategory Category { get; set; }

    /// <summary>
    /// Normal, Low, High, Critical priority usually used with
    /// bug report, change request or support request.
    /// <code>
    /// [Priority] INT NOT NULL
    /// </code>
    /// </summary>
    public SystemFeedbackPriority Priority { get; set; }

    /// <summary>
    /// Status of the user feedback item.<br/>
    /// New, In Process, Completed and similar.
    /// <code>
    /// [Status] INT NOT NULL
    /// </code>
    /// </summary>
    public SystemFeedbackStatus Status { get; set; }

    /// <summary>
    /// Date/Time when Feedback was created.
    /// <code>
    /// [CreatedAt] DATETIMEOFFSET(0) NOT NULL
    /// </code>
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Date/Time when Feedback was last changed.
    /// <code>
    /// [ModifiedAt] DATETIMEOFFSET(0) NOT NULL
    /// </code>
    /// </summary>
    public DateTimeOffset ModifiedAt { get; set; }

    /// <summary>
    /// Date/Time when Feedback item was resolved/closed.
    /// <code>
    /// [CompletedAt] DATETIMEOFFSET(0) NULL
    /// </code>
    /// </summary>
    public DateTimeOffset CompletedAt { get; set; }

    /// <summary>
    /// Messages themselves with language codes they are written into.
    /// <code>
    /// FK from [SystemNotificationMessages] table records.
    /// </code>
    /// </summary>
    public virtual ICollection<SystemFeedbackCommentRecord> Comments { get; set; } = new HashSet<SystemFeedbackCommentRecord>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay => $"{Title}, created {CreatedAt:g} (ID: {Id.ToString("D", System.Globalization.CultureInfo.InvariantCulture)})";
}
