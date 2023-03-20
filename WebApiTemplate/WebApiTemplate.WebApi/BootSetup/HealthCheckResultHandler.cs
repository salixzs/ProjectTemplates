using Microsoft.Extensions.Diagnostics.HealthChecks;
using Salix.AspNetCore.HealthCheck;
using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

namespace WebApiTemplate.WebApi.BootSetup;

public class HealthCheckResultHandler : IHealthCheckResultHandler
{
    private readonly ISystemNotificationForHealthCheck _saveLogic;

    public HealthCheckResultHandler(ISystemNotificationForHealthCheck saveLogic) => _saveLogic = saveLogic;

    public async Task Handle(HttpContext context, HealthReport report, bool isDevelopment = false)
    {
        var url = $"{context.Request.Scheme}://{context.Request.Host.ToString()}{context.Request.PathBase.ToString()}{Urls.Pages.HealthPage}";
        await _saveLogic.HandleHealthCheckSystemNotification(report, url, CancellationToken.None);
        await HealthCheckFormatter.JsonResponseWriter(
            context,
            report,
            isDevelopment);
    }
}
