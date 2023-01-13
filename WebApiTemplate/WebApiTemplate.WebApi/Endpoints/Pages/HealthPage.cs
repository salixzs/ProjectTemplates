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
                    new HealthTestPageLink { TestEndpoint = Urls.Sandbox.Strings, Name = "Strings", Description = "Different string type values serialization." },
                    new HealthTestPageLink { TestEndpoint = Urls.Sandbox.Numbers, Name = "Numbers", Description = "Different number type values serialization." },
                    new HealthTestPageLink { TestEndpoint = Urls.Sandbox.DateTimes, Name = "Dates & Times", Description = "Different Date and Time type values serialization." },
                    new HealthTestPageLink { TestEndpoint = Urls.Sandbox.OtherTypes, Name = "Other types", Description = "Bool, Array, Enum serialization." },
                    new HealthTestPageLink { TestEndpoint = Urls.Sandbox.Exception, Name = "Error", Description = "Throws error on purpose to see how API responds with it." },
                });
        await SendBytesAsync(Encoding.UTF8.GetBytes(healthPageContents), contentType: "text/html", cancellation: ct);
    }
}
