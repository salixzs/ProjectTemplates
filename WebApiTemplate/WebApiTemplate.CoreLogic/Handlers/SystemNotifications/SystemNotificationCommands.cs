using WebApiTemplate.Database.Orm;
using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <inheritdoc/>
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
        };

        foreach (var message in notification.Messages)
        {
            newNotificationRecord.Messages.Add(new SystemNotificationMessageRecord
            {
                LanguageCode = message.Language,
                Message = message.Message,
            });
        }

        _db.SystemNotifications.Add(newNotificationRecord);
        await _db.SaveChangesAsync(cancellationToken);
        return newNotificationRecord.Id;
    }
}
