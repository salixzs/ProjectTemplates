namespace WebApiTemplate.Database.Orm.Entities;

/// <summary>
/// Database record for System-wide notification message(s).
/// <code>
/// TABLE: SystemNotificationMessages
/// </code>
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public class SystemNotificationMessageRecord
{
    /// <summary>
    /// Primary key.
    /// <code>
    /// [Id] INT NOT NULL IDENTITY (1000, 1)
    /// </code>
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Two-letter language code for this record message language.
    /// <code>
    /// [LanguageCode] CHAR(2) NOT NULL
    /// </code>
    /// </summary>
    public string LanguageCode { get; set; } = null!;

    /// <summary>
    /// Message text in language, specified in <see cref="LanguageCode">LanguageCode</see>.
    /// <code>
    /// [Message] NVARCHAR(2000) NOT NULL
    /// </code>
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Related System Notification this language message belongs to.
    /// </summary>
    public virtual SystemNotificationRecord SystemNotification { get; set; } = null!;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
    private string DebuggerDisplay => $"{LanguageCode.ToUpperInvariant()}: {Message}";
}
