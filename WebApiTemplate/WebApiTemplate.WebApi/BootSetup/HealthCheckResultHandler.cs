using Microsoft.Extensions.Diagnostics.HealthChecks;
using Salix.AspNetCore.HealthCheck;
using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

namespace WebApiTemplate.WebApi.BootSetup;

/// <summary>
/// Handles the HealthCheck results with putting system notification if failing.
/// </summary>
public class HealthCheckResultHandler(ISystemNotificationForHealthCheck saveLogic) : IHealthCheckResultHandler
{
    public async Task Handle(HttpContext context, HealthReport report, bool isDevelopment = false)
    {
#pragma warning disable IDE0071 // Simplify interpolation
        var url = $"{context.Request.Scheme}://{context.Request.Host.ToString()}{context.Request.PathBase.ToString()}{Urls.Pages.HealthPage}";
#pragma warning restore IDE0071 // Simplify interpolation
        await saveLogic.HandleHealthCheckSystemNotification(report, url, CancellationToken.None);
        await HealthCheckFormatter.JsonResponseWriter(
            context,
            report,
            isDevelopment);
    }
}
