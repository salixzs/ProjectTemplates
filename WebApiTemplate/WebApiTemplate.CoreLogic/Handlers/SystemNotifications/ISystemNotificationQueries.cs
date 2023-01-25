using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <summary>
/// System Notification data retrieval operations.
/// </summary>
public interface ISystemNotificationQueries
{
    /// <summary>
    /// Returns only currently active system notifications with simplified data trasformations for consumption in UI.<br/>
    /// If no active notification exists - returns empty list.
    /// </summary>
    Task<List<ActiveSystemNotification>> GetActive(CancellationToken cancellationToken);
}
