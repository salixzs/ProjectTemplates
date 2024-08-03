namespace WebApiTemplate.Enumerations;

/// <summary>
/// Specifies how user can dismiss notification in UI (should it reappear under defined circumstances). Default = Permanent (cannot close).
/// </summary>
public enum SystemNotificationUserDismissType
{
    /// <summary>
    /// User cannot dismiss (close) notification.
    /// </summary>
    Permanent = 0,

    /// <summary>
    /// After user dismisses (closes) notification - it should not appear anymore.
    /// </summary>
    CloseForever = 1,

    /// <summary>
    /// After user dismisses (closes) notification - it should reappear within 1 hour.
    /// </summary>
    ForOneHour = 2,

    /// <summary>
    /// After user dismisses (closes) notification - it should reappear the next day.
    /// </summary>
    ForOneDay = 3,

    /// <summary>
    /// After user dismisses (closes) notification - it should reappear when emphasized time is reached.
    /// After that message reappears constantly (cannot be closable anymore).
    /// </summary>
    UntilEmphasizeThenPermanent = 7,

    /// <summary>
    /// After user dismisses (closes) notification - it should reappear when emphasized time is reached.
    /// After that message does not reappear if closed again.
    /// </summary>
    UntilEmphasize = 8,

    /// <summary>
    /// After user dismisses (closes) notification - it should reappear when countdown timer should start.
    /// After that message cannot be closable anymore.
    /// </summary>
    UntilCountdownThenPermanent = 10,
}
