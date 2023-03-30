namespace WebApiTemplate.Database.Orm.Entities;

/// <summary>
/// Database record for end user feedback item comment (feedback can have many).
/// <code>
/// TABLE: SystemFeedbackComments
/// </code>
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public class SystemFeedbackCommentRecord
{
    /// <summary>
    /// Primary key.
    /// <code>
    /// [Id] INT NOT NULL IDENTITY (1000, 1)
    /// </code>
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Feedback comment content - responses, clarifications to feedback.
    /// <code>
    /// [Content] NVARCHAR(MAX) NOT NULL
    /// </code>
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// Date/Time when Feedback comment was created.
    /// <code>
    /// [CreatedAt] DATETIMEOFFSET(0) NOT NULL
    /// </code>
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Messages themselves with language codes they are written into.
    /// <code>
    /// FK from [SystemNotificationMessages] table records.
    /// </code>
    /// </summary>
    public virtual SystemFeedbackRecord? SystemFeedback { get; set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay => $"{CreatedAt:g} (ID: {Id.ToString("D", System.Globalization.CultureInfo.InvariantCulture)})";
}
