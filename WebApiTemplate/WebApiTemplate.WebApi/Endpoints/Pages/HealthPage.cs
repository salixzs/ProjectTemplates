using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Salix.AspNetCore.HealthCheck;
using WebApiTemplate.CoreLogic.Handlers.SystemNotifications;

namespace WebApiTemplate.Endpoints.Pages;

[ExcludeFromCodeCoverage]
public class HealthPageGet(HealthCheckService healthChecks, ISystemNotificationForHealthCheck saveLogic)
    : EndpointWithoutRequest<ContentResult>
{
    public override void Configure()
    {
        Get(Urls.Pages.HealthPage);
        Tags("Pages");
        Options(opts => opts.WithTags("Pages"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var url = string.Concat(BaseURL[..^1], Urls.Pages.HealthPage);
        var healthResult = await healthChecks.CheckHealthAsync(ct);
        await saveLogic.HandleHealthCheckSystemNotification(healthResult, url, ct);
#pragma warning disable RCS0056 // A line is too long.
        var healthPageContents = HealthTestPage.GetContents(
            healthReport: healthResult,
            originalHealthTestEndpoint: "/health",
            testingLinks:
            [
                new HealthTestPageLink { TestEndpoint = Urls.Sandbox.Strings, Name = "Strings", Description = "Different string type values serialization." },
                new HealthTestPageLink { TestEndpoint = Urls.Sandbox.Numbers, Name = "Numbers", Description = "Different number type values serialization." },
                new HealthTestPageLink { TestEndpoint = Urls.Sandbox.DateTimes, Name = "Dates & Times", Description = "Different Date and Time type values serialization." },
                new HealthTestPageLink { TestEndpoint = Urls.Sandbox.OtherTypes, Name = "Other types", Description = "Bool, Array, Enum serialization." },
                new HealthTestPageLink { TestEndpoint = Urls.Sandbox.Exception, Name = "Error", Description = "Throws error on purpose to see how API responds with it." },
            ]);
#pragma warning restore RCS0056 // A line is too long.
        await SendBytesAsync(Encoding.UTF8.GetBytes(healthPageContents), contentType: "text/html", cancellation: ct);
    }
}
