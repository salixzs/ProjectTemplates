namespace WebApiTemplate.Enumerations;

/// <summary>
/// Status of the user feedback item.<br/>
/// New, In Process, Completed and similar.
/// </summary>
public enum SystemFeedbackStatus
{
    /// <summary>
    /// Default status for all new feedback types.
    /// </summary>
    New = 0,

    /// <summary>
    /// Feedback item is reviewed and scheduled for work (planned).
    /// </summary>
    Planned = 1,

    /// <summary>
    /// This is currently being handled/developed.
    /// </summary>
    InProcess = 2,

    /// <summary>
    /// Item is completed and available to end-user.
    /// </summary>
    Completed = 9,
}
