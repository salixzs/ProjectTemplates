namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;

/// <summary>
/// Data modification routines for end-user feedback (bugs, requests) to development/support team.
/// </summary>
public interface ISystemFeedbackCommands
{
    /// <summary>
    /// Creates a new end-user feedback item to triage by support/development team.
    /// </summary>
    Task<int> Create(Domain.SystemFeedbacks.SystemFeedback feedback, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing end-user feedback with new information.
    /// </summary>
    Task Update(Domain.SystemFeedbacks.SystemFeedback feedback, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a system feedback together with all comments and data referencing it.
    /// </summary>
    Task Delete(int feedbackId, CancellationToken cancellationToken);
}
