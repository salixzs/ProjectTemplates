using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Crosscut.Services;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <inheritdoc/>
public sealed class SystemNotificationQueries : ISystemNotificationQueries
{
    private readonly WebApiTemplateDbContext _db;

    private readonly IDateTimeProvider _dateTimeProvider;

    private static readonly Func<WebApiTemplateDbContext, DateTimeOffset, IAsyncEnumerable<SystemNotificationRecord>>
        GetActiveNotificationsCompiledQueryAsync = EF.CompileAsyncQuery((WebApiTemplateDbContext dbc, DateTimeOffset currentDateTime) =>
            dbc.SystemNotifications
                .Include(db => db.Messages)
                .Where(db => db.StartTime <= currentDateTime
                    && db.EndTime >= currentDateTime)
                .AsNoTracking());

    /// <inheritdoc cref="ISystemNotificationQueries"/>
    public SystemNotificationQueries(WebApiTemplateDbContext databaseContext, IDateTimeProvider dateTimeProvider)
    {
        _db = databaseContext;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc/>
    public async Task<List<ActiveSystemNotification>> GetActive(CancellationToken cancellationToken)
    {
        var currentDateTime = _dateTimeProvider.DateTimeOffsetNow;
        var activeNotifications = new List<ActiveSystemNotification>();
        await foreach (var dbRecord in GetActiveNotificationsCompiledQueryAsync(_db, currentDateTime))
        {
            activeNotifications.Add(
                new ActiveSystemNotification
                {
                    Id = dbRecord.Id,
                    MessageType = dbRecord.EmphasizeSince <= currentDateTime ? dbRecord.EmphasizeType : dbRecord.Type,
                    EndTime = dbRecord.EndTime,
                    IsEmphasized = dbRecord.EmphasizeSince <= currentDateTime,
                    ShowCountdown = dbRecord.CountdownSince <= currentDateTime,
                    MoreInfoUrl = dbRecord.MoreInfoUrl,
                    UserDismissType = dbRecord.UserDismissType,
                    Messages = dbRecord.Messages.Select(
                        message => new SystemNotificationMessage
                        {
                            Id = message.Id,
                            Language = message.LanguageCode,
                            Message = message.Message,
                        })
                    .ToList()
                });
        }

        return activeNotifications;
    }

    /// <inheritdoc/>
    public async Task<SystemNotification?> GetById(int notificationId, CancellationToken cancellationToken)
    {
        var notificationRecord = await _db.SystemNotifications
            .Include(db => db.Messages)
            .Where(db => db.Id == notificationId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (notificationRecord == null)
        {
            return null;
        }

        return new SystemNotification
        {
            Id = notificationRecord.Id,
            StartTime = notificationRecord.StartTime,
            EndTime = notificationRecord.EndTime,
            Type = notificationRecord.Type,
            EmphasizeSince = notificationRecord.EmphasizeSince,
            EmphasizeType = notificationRecord.EmphasizeType,
            CountdownSince = notificationRecord.CountdownSince,
            MoreInfoUrl = notificationRecord.MoreInfoUrl,
            IsHealthCheck = notificationRecord.IsHealthCheck,
            UserDismissType = notificationRecord.UserDismissType,
            Messages = notificationRecord.Messages.Select(
                message => new SystemNotificationMessage
                {
                    Id = message.Id,
                    Language = message.LanguageCode,
                    Message = message.Message,
                }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<List<SystemNotification>> GetAll(CancellationToken cancellationToken)
    {
        var notificationRecords = await _db.SystemNotifications
            .Include(db => db.Messages)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return notificationRecords.ConvertAll(
            dbRecord => new SystemNotification
            {
                Id = dbRecord.Id,
                StartTime = dbRecord.StartTime,
                EndTime = dbRecord.EndTime,
                Type = dbRecord.Type,
                EmphasizeSince = dbRecord.EmphasizeSince,
                EmphasizeType = dbRecord.EmphasizeType,
                CountdownSince = dbRecord.CountdownSince,
                MoreInfoUrl = dbRecord.MoreInfoUrl,
                UserDismissType = dbRecord.UserDismissType,
                IsHealthCheck = dbRecord.IsHealthCheck,
                Messages = dbRecord.Messages.Select(
                    message => new SystemNotificationMessage
                    {
                        Id = message.Id,
                        Language = message.LanguageCode,
                        Message = message.Message,
                    }).ToList()
            });
    }
}
