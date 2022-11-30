namespace WebApiTemplate.BootSetup;

/// <summary>
/// Extensions for built WebApp (app) - Use* extensions.
/// </summary>
public static class WebAppExtensions
{
    public static WebApplication UseWebApi(this WebApplication app)
    {
        app.UseFastEndpoints();
        return app;
    }

    public static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

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
}
