namespace WebApiTemplate.Enumerations;

/// <summary>
/// Category (type) of the user feedback.<br/>
/// Feedback, Bug report, Support ticket and similar.
/// </summary>
public enum SystemFeedbackCategory
{
    /// <summary>
    /// Simple feedback with no requirement to act on it. (Good Work!)
    /// </summary>
    SimpleFeedback = 0,

    /// <summary>
    /// A proposal for something to improve or change.
    /// </summary>
    Proposal = 1,

    /// <summary>
    /// A change request (development request) for application.
    /// </summary>
    ChangeRequest = 2,

    /// <summary>
    /// Request to fix something with data.
    /// </summary>
    SupportRequest = 3,

    /// <summary>
    /// Bug report. Should be resolved by developer(s).
    /// </summary>
    BugReport = 4,
}
