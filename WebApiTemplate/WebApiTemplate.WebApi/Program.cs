using WebApiTemplate.BootSetup;

namespace WebApiTemplate;

#pragma warning disable RCS1102 // Make class static.
public class Program
#pragma warning restore RCS1102 // Make class static.
{
    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args)
            .AddSerilogLogging()
            .AddApplicationInsights()
            .AddWebApiFeatures()
            .AddAuth()
            .AddHttpsSsl()
            .AddCorsUrls()
            .AddDependencies()
            .AddRequestLocalization()
            .AddSwaggerServices()
            .AddHealthChecks();

        var app = builder.Build()
            .UseJsonErrorHandling()
            .UseWebApiFeatures()
            .UseCORS()
            .UseAuth()
            .UseRequestLocalization()
            .UseHealthChecking()
            .UseSwaggerPage();

        app.UseHttpsRedirection();
        await app.RunAsync();
        return 0;
    }
}
