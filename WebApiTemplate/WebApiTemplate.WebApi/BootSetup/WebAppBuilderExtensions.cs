using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

namespace WebApiTemplate.BootSetup;

/// <summary>
/// Extensions for WebApp builder - Host, Services, Configuration.
/// </summary>
public static class WebAppBuilderExtensions
{

    public static WebApplicationBuilder ConfigureApi(this WebApplicationBuilder builder)
    {
        builder.WebHost
            .ConfigureKestrel(options =>
            {
                options.AddServerHeader = false;
                options.AllowSynchronousIO = false;
            })
            .CaptureStartupErrors(true);
        builder.Host.UseConsoleLifetime(options => options.SuppressStatusMessages = true);

        builder.Services.AddFastEndpoints(options => options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All);
        return builder;
    }

    public static WebApplicationBuilder UseAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        return builder;
    }

    public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiDependencies(builder.Configuration);
        return builder;
    }

    public static WebApplicationBuilder UseSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((hostBuilderContext, services, loggerConfiguration) =>
            loggerConfiguration
                .ReadFrom.Configuration(hostBuilderContext.Configuration)
                .Enrich.FromLogContext());
        return builder;
    }

    public static WebApplicationBuilder UseApplicationInsights(this WebApplicationBuilder builder)
    {
        // TODO: remove this options setting after Serilog.Sinks.ApplicationInsights fixes usage of deprecated Telemetryconfiguration.Active singleton instance.
        // More info: https://github.com/serilog/serilog-sinks-applicationinsights/issues/156
        builder.Services.AddApplicationInsightsTelemetry(options => options.EnableActiveTelemetryConfigurationSetup = true);
#if DEBUG
        // Disable line below to get Debug events in View->Other Windows->AppInsights Search (Note: It also will spam Debug Output window)
        //Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryDebugWriter.IsTracingDisabled = true;
#endif
        return builder;
    }

    public static WebApplicationBuilder UseSwaggerServices(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddSwaggerDoc(settings =>
            {
                settings.Title = "WebApiTemplate";
                settings.Version = "v1.0";
                settings.DocumentName = "Version-1.0";
                settings.Description = "Here goes a longer and detailed description on API purpose and usage.";
            });
        }

        return builder;
    }
}
