using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Crosscut.Exceptions;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Domain.SystemNotifications;
using WebApiTemplate.Translations;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <inheritdoc cref="ISystemNotificationCommands"/>
public sealed class SystemNotificationCommands : ISystemNotificationCommands
{
    private readonly WebApiTemplateDbContext _db;

    private readonly ITranslate<ErrorMessageTranslations> _l10n;

    /// <inheritdoc cref="ISystemNotificationCommands"/>
    public SystemNotificationCommands(WebApiTemplateDbContext databaseContext, ITranslate<ErrorMessageTranslations> l10n)
    {
        _db = databaseContext;
        _l10n = l10n;
    }

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
            UserDismissType = notification.UserDismissType,
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
                _l10n[
                    nameof(ErrorMessageTranslations.Record_NotFoundById),
                    "System notification",
                    notification.Id],
                BusinessExceptionType.RequestError);

        updateableRecord.StartTime = notification.StartTime;
        updateableRecord.EndTime = notification.EndTime;
        updateableRecord.Type = notification.Type;
        updateableRecord.EmphasizeSince = notification.EmphasizeSince ?? notification.EndTime;
        updateableRecord.EmphasizeType = notification.EmphasizeType ?? notification.Type;
        updateableRecord.CountdownSince = notification.CountdownSince ?? notification.EndTime;
        updateableRecord.MoreInfoUrl = notification.MoreInfoUrl;
        updateableRecord.UserDismissType = notification.UserDismissType;

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
