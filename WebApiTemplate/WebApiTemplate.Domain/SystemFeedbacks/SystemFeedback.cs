using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Domain.SystemFeedbacks;

/// <summary>
/// End user feedback to support/development team (also bug, change request, support request).
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public class SystemFeedback
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Summary title/name of feedback item.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Feedback content - description of joy or problem.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Technical part of the feedback. Normally should be populated with technical context,
    /// like Stack Trace, request URL etc.
    /// </summary>
    public string? SystemInfo { get; set; }

    /// <summary>
    /// Category (type) of the user feedback.<br/>
    /// Feedback, Bug report, Support ticket and similar.
    /// </summary>
    public SystemFeedbackCategory Category { get; set; }

    /// <summary>
    /// Normal, Low, High, Critical priority usually used with
    /// bug report, change request or support request.
    /// </summary>
    public SystemFeedbackPriority Priority { get; set; }

    /// <summary>
    /// Status of the user feedback item.<br/>
    /// New, In Process, Completed and similar.
    /// </summary>
    public SystemFeedbackStatus Status { get; set; }

    /// <summary>
    /// Date/Time when Feedback was created.<br/>
    /// Feedback create/modify will ignore this field.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Date/Time when Feedback was last changed.<br/>
    /// Feedback create/modify will ignore this field.
    /// </summary>
    public DateTimeOffset? ModifiedAt { get; set; }

    /// <summary>
    /// Date/Time when Feedback item was resolved/closed.<br/>
    /// Feedback create/modify will ignore this field - set by <see cref="Status"/> change.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; set; }

    /// <summary>
    /// Comments to user feedback. Ignored in feedback update operations.
    /// </summary>
    public List<SystemFeedbackComment> Comments { get; set; } = [];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay => $"{Title}, created {CreatedAt:g} (ID: {Id.ToString("D", System.Globalization.CultureInfo.InvariantCulture)})";
}
