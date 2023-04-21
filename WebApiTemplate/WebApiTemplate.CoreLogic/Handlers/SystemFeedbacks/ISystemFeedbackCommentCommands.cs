using WebApiTemplate.Domain.SystemFeedbacks;

namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;

/// <summary>
/// End-user system feedback comment data management commands.
/// </summary>
public interface ISystemFeedbackCommentCommands
{
    /// <summary>
    /// Creates a new end-user system feedback comment. Parent system feedback ID must be provided, so it attaches to that.
    /// </summary>
    Task<int> Create(SystemFeedbackComment feedbackComment, CancellationToken cancellationToken);

    /// <summary>
    /// Updates content of end-user system feedback comment.
    /// </summary>
    Task Update(SystemFeedbackComment feedbackComment, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a commend from end-user system feedback.
    /// </summary>
    Task Delete(int feedbackCommentId, CancellationToken cancellationToken);
}
