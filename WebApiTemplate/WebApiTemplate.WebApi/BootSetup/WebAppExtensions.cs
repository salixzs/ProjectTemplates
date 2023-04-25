using ConfigurationValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using WebApiTemplate.Crosscut.Exceptions;
using WebApiTemplate.Enumerations;
using WebApiTemplate.WebApi.BootSetup;
using WebApiTemplate.WebApi.Middleware;

namespace WebApiTemplate.BootSetup;

/// <summary>
/// Extensions for built WebApp (app) - Use* extensions.
/// </summary>
public static class WebAppExtensions
{
    public static WebApplication UseWebApiFeatures(this WebApplication app)
    {
        app.UseFastEndpoints(config =>
        {
            config.Endpoints.ShortNames = true;

            // There are required to deserialize List<Enum> passed in SystemFeedback functionality GET
            config.Serializer.Options.Converters.Add(new JsonEnumListConverter<SystemFeedbackCategory>());
            config.Serializer.Options.Converters.Add(new JsonEnumListConverter<SystemFeedbackStatus>());
            config.Serializer.Options.Converters.Add(new JsonEnumListConverter<SystemFeedbackPriority>());
        });

        return app;
    }

    public static WebApplication UseJsonErrorHandling(this WebApplication app)
    {
        app.AddJsonExceptionHandler<ApiJsonErrorMiddleware>(
            new ApiJsonExceptionOptions
            {
                OmitSources = new HashSet<string> { "Middleware", "ThrowIfRequestValidationFailed" },
                ShowStackTrace = true
            });
        return app;
    }

    public static WebApplication UseCORS(this WebApplication app)
    {
        //app.UseCors();
        return app;
    }

    public static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static WebApplication UseRequestLocalization(this WebApplication app)
    {
        var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>()
            ?? throw new BusinessException(
                "UseRequestLocalization was not able to get RequestLocalizationOptions. Did you forget to use AddRequestLocalization() to services?",
                BusinessExceptionType.ConfigurationError);

        app.UseRequestLocalization(options.Value);
        return app;
    }

    public static WebApplication UseSwaggerPage(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.AdditionalSettings["filter"] = true;
                settings.AdditionalSettings["persistAuthorization"] = true;
                settings.AdditionalSettings["displayRequestDuration"] = true;
                settings.AdditionalSettings["tryItOutEnabled"] = true;
                settings.TagsSorter = "alpha";
                settings.OperationsSorter = "alpha";
                settings.CustomInlineStyles = """
                    .servers-title, .servers{display:none}
                    .swagger-ui .info{margin:10px 0}
                    .swagger-ui .scheme-container{margin:10px 0;padding:10px 0}
                    .swagger-ui .info .title{font-size:25px}
                    .swagger-ui textarea{min-height:150px}
                    """;
            });
        }

        return app;
    }

    public static WebApplication UseHealthChecking(this WebApplication app)
    {
        app.UseConfigurationValidationErrorPage();
        var healthCheckOptions = new HealthCheckOptions
        {
            ResponseWriter = (HttpContext context, HealthReport report) => HealthCheckResultHandler(context, report, app),
        };
        app.UseHealthChecks("/health", healthCheckOptions);

        return app;
    }

    private static async Task HealthCheckResultHandler(HttpContext context, HealthReport report, WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var handler = scope.Resolve<IHealthCheckResultHandler>();
        await handler.Handle(context, report);
    }
}
