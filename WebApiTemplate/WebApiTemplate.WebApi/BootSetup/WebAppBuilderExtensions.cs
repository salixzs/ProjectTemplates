using FastEndpoints.Swagger;
using Serilog;

namespace WebApiTemplate.BootSetup;

/// <summary>
/// Extensions for WebApp builder - Host, Services, Configuration.
/// </summary>
public static class WebAppBuilderExtensions
{

    public static WebApplicationBuilder AddWebApiFeatures(this WebApplicationBuilder builder)
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

    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        return builder;
    }

    public static WebApplicationBuilder AddHttpsSsl(this WebApplicationBuilder builder)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?WT.mc_id=DT-MVP-5003978#http-strict-transport-security-protocol-hsts
        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security
        builder.Services.AddHsts(options =>
        {
            options.MaxAge = TimeSpan.FromDays(90);
            options.IncludeSubDomains = true;
            options.Preload = true;
        });

        builder.Services.AddHttpsRedirection(options =>
        {
            options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
            options.HttpsPort = 443;
        });

        return builder;
    }

    public static WebApplicationBuilder AddCorsUrls(this WebApplicationBuilder builder)
    {
        //var securityOptions = builder.Configuration
        //    .GetSection(ConfigurationSecuritySettings.ConfigField)
        //    .Get<ConfigurationSecuritySettings>();

        //if (securityOptions == null)
        //{
        //    throw new Domain.Exceptions.DomainException($"Missing \"{ConfigurationSecuritySettings.ConfigField}\" configuration section or its contents are wrong.");
        //}

        // var origins = securityOptions.CorsOrigins.Split(";");

        //builder.Services.AddCors(options => options.AddDefaultPolicy(
        //    builder => builder
        //        .WithOrigins()
        //        .WithExposedHeaders("Content-Disposition")
        //        .AllowAnyHeader()
        //        .AllowAnyMethod()));

        return builder;
    }

    public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiDependencies(builder.Configuration);
        return builder;
    }

    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((hostBuilderContext, services, loggerConfiguration) =>
            loggerConfiguration
                .ReadFrom.Configuration(hostBuilderContext.Configuration)
                .Enrich.FromLogContext());
        return builder;
    }

    public static WebApplicationBuilder AddApplicationInsights(this WebApplicationBuilder builder)
    {
        // TODO: remove this options setting after Serilog.Sinks.ApplicationInsights fixes usage of deprecated Telemetryconfiguration.Active singleton instance.
        // More info: https://github.com/serilog/serilog-sinks-applicationinsights/issues/156
        builder.Services.AddApplicationInsightsTelemetry(options => options.EnableActiveTelemetryConfigurationSetup = true);
#if DEBUG
        // Disable line below to get Debug events in View->Other Windows->AppInsights Search (Note: It also will spam Debug Output window)
        Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryDebugWriter.IsTracingDisabled = true;
#endif
        return builder;
    }

    public static WebApplicationBuilder AddSwaggerServices(this WebApplicationBuilder builder)
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
