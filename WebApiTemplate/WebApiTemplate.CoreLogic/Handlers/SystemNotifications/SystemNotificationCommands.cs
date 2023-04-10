using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Crosscut.Exceptions;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <inheritdoc cref="ISystemNotificationCommands"/>
public sealed class SystemNotificationCommands : ISystemNotificationCommands
{
    private readonly WebApiTemplateDbContext _db;

    /// <inheritdoc cref="ISystemNotificationCommands"/>
    public SystemNotificationCommands(WebApiTemplateDbContext databaseContext) => _db = databaseContext;

    /// <inheritdoc/>
    public async Task<int> Create(SystemNotification notification, CancellationToken cancellationToken)
    {
        var newNotificationRecord = new SystemNotificationRecord
        {
            StartTime = notification.StartTime,
            EndTime = notification.EndTime,
            Type = notification.Type,
            EmphasizeSince = notification.EmphasizeSince ?? notification.EndTime,
            EmphasizeType = notification.EmphasizeType ?? notification.Type,
            CountdownSince = notification.CountdownSince ?? notification.EndTime,
            MoreInfoUrl = notification.MoreInfoUrl,
        };

        foreach (var message in notification.Messages)
        {
            newNotificationRecord.Messages.Add(
                new SystemNotificationMessageRecord
                {
                    LanguageCode = message.Language,
                    Message = message.Message,
                });
        }

        _db.SystemNotifications.Add(newNotificationRecord);
        await _db.SaveChangesAsync(cancellationToken);
        return newNotificationRecord.Id;
    }

    /// <inheritdoc/>
    public async Task Update(SystemNotification notification, CancellationToken cancellationToken)
    {
        var updateableRecord = await _db.SystemNotifications
                .Include(db => db.Messages)
                .Where(db => db.Id == notification.Id)
                .FirstOrDefaultAsync(cancellationToken)
            ?? throw new BusinessException(
                $"System Notification update did not find existing record with Id: {notification.Id:D}",
                BusinessExceptionType.RequestError);

        updateableRecord.StartTime = notification.StartTime;
        updateableRecord.EndTime = notification.EndTime;
        updateableRecord.Type = notification.Type;
        updateableRecord.EmphasizeSince = notification.EmphasizeSince ?? notification.EndTime;
        updateableRecord.EmphasizeType = notification.EmphasizeType ?? notification.Type;
        updateableRecord.CountdownSince = notification.CountdownSince ?? notification.EndTime;
        updateableRecord.MoreInfoUrl = notification.MoreInfoUrl;

        updateableRecord.Messages.Clear();
        foreach (var message in notification.Messages)
        {
            updateableRecord.Messages.Add(
                new SystemNotificationMessageRecord
                {
                    LanguageCode = message.Language,
                    Message = message.Message,
                });
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(int notificationId, CancellationToken cancellationToken)
    {
        var notificationRecord = await _db.SystemNotifications
            .Where(notification => notification.Id == notificationId)
            .FirstOrDefaultAsync(cancellationToken);

        if (notificationRecord == null)
        {
            return;
        }

        _db.SystemNotifications.Remove(notificationRecord);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
