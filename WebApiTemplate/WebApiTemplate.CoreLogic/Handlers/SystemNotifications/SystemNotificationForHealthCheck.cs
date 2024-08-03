using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Database.Orm.Entities;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <inheritdoc cref="ISystemNotificationForHealthCheck"/>
public sealed class SystemNotificationForHealthCheck(WebApiTemplateDbContext databaseContext) : ISystemNotificationForHealthCheck
{
    /// <inheritdoc/>
    public async Task HandleHealthCheckSystemNotification(HealthReport healthReport, string? moreInfoUrl, CancellationToken cancellationToken)
    {
        var healthCheckSystemNotification = await databaseContext.SystemNotifications
            .Include(db => db.Messages)
            .Where(db => db.IsHealthCheck)
            .FirstOrDefaultAsync(cancellationToken);

        if (healthReport.Status == HealthStatus.Healthy)
        {
            if (healthCheckSystemNotification != null)
            {
                databaseContext.SystemNotifications.Remove(healthCheckSystemNotification);
                await databaseContext.SaveChangesAsync(cancellationToken);
            }

            return;
        }

        var currentTime = DateTimeOffset.UtcNow;
        if (healthCheckSystemNotification == null)
        {
            databaseContext.SystemNotifications.Add(
                new SystemNotificationRecord
                {
                    StartTime = currentTime,
                    EndTime = currentTime.AddHours(3),
                    CountdownSince = currentTime.AddHours(3),
                    EmphasizeSince = currentTime.AddHours(3),
                    Type = Enumerations.SystemNotificationType.Warning,
                    EmphasizeType = Enumerations.SystemNotificationType.Warning,
                    MoreInfoUrl = moreInfoUrl,
                    UserDismissType = Enumerations.SystemNotificationUserDismissType.Permanent,
                    IsHealthCheck = true,
                    Messages = new List<SystemNotificationMessageRecord>
                    {
                        new SystemNotificationMessageRecord
                        {
                            LanguageCode = "en",
                            Message = CompileMessage(healthReport)
                        }
                    }
                });
        }
        else
        {
            healthCheckSystemNotification.StartTime = currentTime;
            healthCheckSystemNotification.EndTime = currentTime.AddHours(3);
            healthCheckSystemNotification.CountdownSince = currentTime.AddHours(3);
            healthCheckSystemNotification.EmphasizeSince = currentTime.AddHours(3);
            healthCheckSystemNotification.Messages.First().Message = CompileMessage(healthReport);
            healthCheckSystemNotification.MoreInfoUrl = moreInfoUrl;
        }

        await databaseContext.SaveChangesAsync(cancellationToken);
    }

#pragma warning disable IDE0071 // Simplify interpolation
    private static string CompileMessage(HealthReport healthReport) =>
        @$"Health check reports {healthReport.Status.ToString()} status.
Application may be broken or limited in its functionality. Last check: {DateTime.UtcNow:dd. MMM HH:mm}";
#pragma warning restore IDE0071 // Simplify interpolation
}
