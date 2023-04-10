namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedback;

/// <summary>
/// Data modification routines for end-user feedback (bugs, requests) to development/support team.
/// </summary>
public interface ISystemFeedbackCommands
{
    /// <summary>
    /// Creates a new end-user feedback item to triage by support/development team.
    /// </summary>
    Task<int> CreateFeedback(Domain.SystemFeedback.SystemFeedback feedback, CancellationToken cancellationToken);
}
