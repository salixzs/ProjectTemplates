using WebApiTemplate.Database.Orm;
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
        return 15;
    }
}
