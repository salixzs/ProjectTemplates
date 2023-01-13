using Serilog;
using WebApiTemplate.BootSetup;

namespace WebApiTemplate;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
#pragma warning disable CA1305 // Specify IFormatProvider
#if DEBUG
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .CreateBootstrapLogger();
#else
        Log.Logger = new LoggerConfiguration()
            .WriteTo.ApplicationInsights(Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces, Serilog.Events.LogEventLevel.Verbose)
            .CreateBootstrapLogger();
#endif
#pragma warning restore CA1305 // Specify IFormatProvider
        try
        {
            Log.Information("Starting up API. Setup and Configuration...");

            var builder = WebApplication.CreateBuilder(args)
                .AddSerilogLogging()
                .AddApplicationInsights()
                .AddWebApiFeatures()
                .AddAuth()
                .AddHttpsSsl()
                .AddCorsUrls()
                .AddDependencies()
                .AddSwaggerServices()
                .AddHealthChecks();

            var app = builder.Build()
                .UseJsonErrorHandling()
                .UseWebApiFeatures()
                .UseCORS()
                .UseAuth()
                .UseHealthChecking()
                .UseSwaggerPage();

            app.UseHttpsRedirection();

            Log.Information("Setup finalized. Launching API...");
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "API host terminated unexpectedly.");
            return 1;
        }
        finally
        {
            Log.Information("Shut down complete");
            Log.CloseAndFlush();
        }
    }
}
