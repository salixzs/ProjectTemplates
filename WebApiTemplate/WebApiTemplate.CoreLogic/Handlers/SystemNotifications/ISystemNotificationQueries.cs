using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <summary>
/// System Notification data retrieval operations.
/// </summary>
public interface ISystemNotificationQueries
{
    /// <summary>
    /// Returns only currently active system notifications with simplified data transformations for consumption in UI.<br/>
    /// If no active notification exists - returns empty list.
    /// </summary>
    Task<List<ActiveSystemNotification>> GetActive(CancellationToken cancellationToken);

    /// <summary>
    /// Returns full-data object of requested notification.<br/>
    /// NULL if not found.
    /// </summary>
    Task<SystemNotification?> GetById(int notificationId, CancellationToken cancellationToken);

    /// <summary>
    /// Returns fully all system notifications (including expired) with messages.
    /// </summary>
    Task<List<SystemNotification>> GetAll(CancellationToken cancellationToken);
}
