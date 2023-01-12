using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Salix.AspNetCore.HealthCheck;
using Salix.AspNetCore.TitlePage;

namespace WebApiTemplate.Endpoints.Samples;

public class HealthPageGet : EndpointWithoutRequest<ContentResult>
{
    private readonly HealthCheckService _healthChecks;

    public HealthPageGet(HealthCheckService healthChecks) =>
        _healthChecks = healthChecks;

    public override void Configure()
    {
        Get(Urls.Pages.HealthPage);
        Options(opts => opts.WithTags("Pages"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var healthResult = await _healthChecks.CheckHealthAsync(ct);
        var healthPageContents = HealthTestPage.GetContents(
                healthReport: healthResult,
                originalHealthTestEndpoint: "/health",
                testingLinks: new List<HealthTestPageLink>
                {
                        new HealthTestPageLink { TestEndpoint = "/api/sample/exception", Name = "Exception", Description = "Throws dummy exception/error to check Json Error functionality." },
                        new HealthTestPageLink { TestEndpoint = "/api/sample/validation", Name = "Validation Error", Description = "Throws dummy data validation exception to check Json Error functionality for data validation." },
                });
        await SendBytesAsync(Encoding.UTF8.GetBytes(healthPageContents), contentType: "text/html", cancellation: ct);
    }
}
