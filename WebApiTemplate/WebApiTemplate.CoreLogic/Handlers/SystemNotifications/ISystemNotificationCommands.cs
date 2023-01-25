using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <summary>
/// System Notification data modification operations.
/// </summary>
public interface ISystemNotificationCommands
{
    /// <summary>
    /// Creates a new system notification.
    /// </summary>
    /// <returns>ID of the new created notification.</returns>
    Task<int> Create(SystemNotification notification, CancellationToken cancellationToken);
}
