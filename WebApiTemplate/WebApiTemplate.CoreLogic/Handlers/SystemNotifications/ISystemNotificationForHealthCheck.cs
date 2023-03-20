using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

/// <summary>
/// Automated creating/removing system notification from Health Check results.
/// </summary>
public interface ISystemNotificationForHealthCheck
{
    /// <summary>
    /// Automatically creates system notification for degraded/unhealthy health check.<br/>
    /// Automatically removes existing health check system notification if health check is successful.
    /// </summary>
    Task HandleHealthCheckSystemNotification(HealthReport healthReport, string? moreInfoUrl, CancellationToken cancellationToken);
}
