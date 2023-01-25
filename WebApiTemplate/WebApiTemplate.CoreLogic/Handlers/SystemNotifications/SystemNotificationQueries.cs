using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <inheritdoc/>
public sealed class SystemNotificationQueries : ISystemNotificationQueries
{
    private readonly WebApiTemplateDbContext _db;

    /// <inheritdoc cref="ISystemNotificationQueries"/>
    public SystemNotificationQueries(WebApiTemplateDbContext databaseContext) => _db = databaseContext;

    /// <inheritdoc/>
    public async Task<List<ActiveSystemNotification>> GetActive(CancellationToken cancellationToken)
    {
        var notificationRecords = await _db.SystemNotifications
            .Include(notification => notification.Messages)
            .Where(notification => notification.StartTime >= DateTime.UtcNow
                && notification.EndTime <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        return notificationRecords.ConvertAll(dbRecord => new ActiveSystemNotification
        {
            Id = dbRecord.Id,
            MessageType = dbRecord.EmphasizeSince <= DateTime.UtcNow ? dbRecord.EmphasizeType : dbRecord.Type,
            EndTime = dbRecord.EndTime,
            IsEmphasized = dbRecord.EmphasizeSince <= DateTime.UtcNow,
            ShowCountdown = dbRecord.CountdownSince <= DateTime.UtcNow,
            Messages = dbRecord.Messages.Select(message => new SystemNotificationMessage
            {
                Language = message.LanguageCode,
                Message = message.Message,
            }).ToList()
        });
    }
}
