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

    /// <summary>
    /// Updates an existing system notification with new data.
    /// </summary>
    Task Update(SystemNotification notification, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the existing system notification.
    /// </summary>
    Task Delete(int notificationId, CancellationToken cancellationToken);
}
