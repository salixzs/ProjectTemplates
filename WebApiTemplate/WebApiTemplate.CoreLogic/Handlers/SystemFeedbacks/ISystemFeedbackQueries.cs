using WebApiTemplate.Domain.SystemFeedbacks;

namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;

/// <summary>
/// Provides data reading to get end-user feedback items stored in system.
/// </summary>
public interface ISystemFeedbackQueries
{
    /// <summary>
    /// Reads a list of end-user system feedbacks (without their comments).<br/>
    /// If <paramref name="filter"/> is empty (not null!) - all existing feedbacks are returned.
    /// </summary>
    Task<List<SystemFeedback>> GetFeedbacks(SystemFeedbackFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Gets one single end-user system feedback by its ID. Including its related comments.
    /// </summary>
    Task<SystemFeedback> GetFeedbackById(int feedbackId, CancellationToken cancellationToken);
}
