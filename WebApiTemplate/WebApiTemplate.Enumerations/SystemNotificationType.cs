namespace WebApiTemplate.Enumerations;

/// <summary>
/// Type of System-wide notification (dynamic banner on main or all pages in UI).
/// </summary>
public enum SystemNotificationType
{
    /// <summary>
    /// Normal notification (black/white/grey).
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Cheerful, green notification.
    /// </summary>
    Success = 1,

    /// <summary>
    /// Yellow warning notification.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Red critical notification.
    /// </summary>
    Critical = 3
}
