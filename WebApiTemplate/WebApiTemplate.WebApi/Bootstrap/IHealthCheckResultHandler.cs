using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApiTemplate.WebApi.Bootstrap;

public interface IHealthCheckResultHandler
{
    Task Handle(HttpContext context, HealthReport report, bool isDevelopment = false);
}
