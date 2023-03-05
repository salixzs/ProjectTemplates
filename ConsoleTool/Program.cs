using ConsoleTool.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Salix.Extensions;
using Serilog.Events;
using Serilog;

namespace ConsoleTool;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        const string consoleToolTitle = "This is a template for Console Tool.";

        // Logging setup start =====>
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("ConsoleToolLog.txt", LogEventLevel.Verbose, buffered: true, rollingInterval: RollingInterval.Minute)
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .CreateLogger();
        Log.Information("Tool starting.");
        // <===== Logging setup end

        Consolix.SetColorScheme(ConsoleColorScheme.Campbell);

        // Configuration handling start =====>
        if (!File.Exists("settings.json"))
        {
            Consolix.WriteLine("There is no settings.json file in application folder", ConsoleColor.Red);
            Log.Error("There is no settings.json file in application folder");
            Log.CloseAndFlush();
            return 1;
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json", false, true)
            .Build();

        // Remove this line. Just a demo.
        Consolix.WriteLine(configuration.GetValue<string>("ToolSetting") ?? "Configuration value not found", ConsoleColor.Cyan);
        // <===== Configuration handling end

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => logging.ClearProviders())
            .UseConsoleLifetime()
            .ConfigureServices((context, collection) => SetupContainer(context, collection, configuration))
            .UseSerilog()
            .Build();
        Log.Information("Console Host created (with DI).");

        try
        {
            var consoleOperationHandler = host.Services.GetRequiredService<ConsoleOperationHandler>();
            consoleOperationHandler.PrepareOperation(args, configuration);
            if (args.Contains("--h") || args.Contains("--help"))
            {
                consoleOperationHandler.OutputHelp(
                    typeof(Program).Assembly.GetName().Name!,
                    consoleToolTitle);
                return 0;
            }

            if (consoleOperationHandler.SelectedOperation is { IsReady: true })
            {
                Log.Information($"Executing command \"{consoleOperationHandler.SelectedOperation.OperationName}\".");
                return await consoleOperationHandler.SelectedOperation.DoWork();
            }

            consoleOperationHandler.OutputHelp(
                typeof(Program).Assembly.GetName().Name!,
                consoleToolTitle);
            return -1;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Fatal error occurred.");
            Consolix.WriteLine(ex.Message, ConsoleColor.Red);
            return -1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void SetupContainer(HostBuilderContext context, IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IConsoleOperation, SomeCommand>();
        services.Configure<SomeCommandConfiguration>(configuration.GetSection("first"));
        services.AddTransient<ConsoleOperationHandler>();
    }
}
