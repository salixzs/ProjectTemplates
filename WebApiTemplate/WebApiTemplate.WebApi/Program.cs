using Serilog;
using WebApiTemplate.BootSetup;

namespace WebApiTemplate;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
#if DEBUG
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .CreateBootstrapLogger();
#else
        Log.Logger = new LoggerConfiguration()
            .WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces, Serilog.Events.LogEventLevel.Verbose)
            .CreateBootstrapLogger();
#endif
        try
        {
            Log.Information("Starting up API. Setup and Configuration...");

            var builder = WebApplication.CreateBuilder(args)
                .UseSerilogLogging()
                .UseApplicationInsights()
                .ConfigureApi()
                .UseAuth()
                .RegisterDependencies()
                .UseSwaggerServices();

            var app = builder.Build();
            app.UseWebApi()
                .UseAuth()
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
