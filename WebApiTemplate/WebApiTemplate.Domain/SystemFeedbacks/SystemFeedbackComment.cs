using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace WebApiTemplate.Domain.SystemFeedbacks;

/// <summary>
/// A one of comments to end user feedback item.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public class SystemFeedbackComment
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Feedback comment content - responses, clarifications to feedback.
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// Date/Time when Feedback comment was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Primary key for parent System Feedback record (relevant for Create command).
    /// </summary>
    public int? SystemFeedbackId { get; set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay => $"{CreatedAt:g} (ID: {Id.ToString("D", System.Globalization.CultureInfo.InvariantCulture)})";
}
