namespace WebApiTemplate.Enumerations;

/// <summary>
/// Priority of the user feedback item.<br/>
/// Normal, Low, High, Critical to prioritize order when multiple feedbacks
/// (usually bug report, change request or support request) are reported.
/// </summary>
public enum SystemFeedbackPriority
{
    /// <summary>
    /// Default priority which should not be used.
    /// </summary>
    NotSpecified = 0,

    /// <summary>
    /// Critical feedback, which prevents application from working.
    /// </summary>
    Critical = 1,

    /// <summary>
    /// High importance - app is still working, but some of its parts are inoperable or working incorrectly.
    /// </summary>
    High = 2,

    /// <summary>
    /// For normal planned work or bugs which does not impact application work (have workarounds).
    /// </summary>
    Normal = 3,

    /// <summary>
    /// Normal feedback, postponed and unimportant items to solve right now.
    /// </summary>
    Low = 4
}
