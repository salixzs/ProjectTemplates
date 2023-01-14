using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace WebApiTemplate.Database.Orm;

[ExcludeFromCodeCoverage]
public static class ContextRegistration
{
    private static readonly string ConfigurationKey = "WebApiTemplateDatabase";

    public static void AddDatabaseContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WebApiTemplateDbContext>(options =>
        {
            options
                .UseSqlServer(configuration.GetConnectionString(ConfigurationKey))
                .ConfigureLoggingCacheTime(TimeSpan.FromMinutes(1));
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
            options.ConfigureWarnings(evt => evt.Log(
                (RelationalEventId.ConnectionOpening, LogLevel.Trace),
                (RelationalEventId.ConnectionOpened, LogLevel.Debug),
                (RelationalEventId.ConnectionClosing, LogLevel.Trace),
                (RelationalEventId.ConnectionClosed, LogLevel.Debug),
                (RelationalEventId.ConnectionError, LogLevel.Warning),
                (RelationalEventId.TransactionStarting, LogLevel.Trace),
                (RelationalEventId.TransactionStarted, LogLevel.Debug),
                (RelationalEventId.AmbientTransactionEnlisted, LogLevel.Debug),
                (RelationalEventId.AmbientTransactionWarning, LogLevel.Debug),
                (RelationalEventId.TransactionUsed, LogLevel.Debug),
                (RelationalEventId.TransactionCommitting, LogLevel.Trace),
                (RelationalEventId.TransactionCommitted, LogLevel.Debug),
                (RelationalEventId.TransactionRollingBack, LogLevel.Trace),
                (RelationalEventId.TransactionRolledBack, LogLevel.Debug),
                (RelationalEventId.TransactionError, LogLevel.Error),
                (RelationalEventId.TransactionDisposed, LogLevel.Trace),
                (RelationalEventId.CommandCreating, LogLevel.Trace),
                (RelationalEventId.CommandCreated, LogLevel.Debug),
                (RelationalEventId.CommandError, LogLevel.Error),
                (RelationalEventId.CommandExecuting, LogLevel.Trace),
                (RelationalEventId.CommandExecuted, LogLevel.Debug),
                (RelationalEventId.DataReaderDisposing, LogLevel.Trace),
                (CoreEventId.ContextInitialized, LogLevel.Debug),
                (CoreEventId.ContextDisposed, LogLevel.Debug),
                (CoreEventId.SaveChangesStarting, LogLevel.Trace),
                (CoreEventId.SaveChangesCompleted, LogLevel.Debug),
                (CoreEventId.SaveChangesFailed, LogLevel.Error),
                (CoreEventId.StartedTracking, LogLevel.Debug),
                (CoreEventId.QueryCompilationStarting, LogLevel.Trace),
                (CoreEventId.QueryExecutionPlanned, LogLevel.Trace),
                (CoreEventId.DetectChangesStarting, LogLevel.Debug),
                (CoreEventId.DetectChangesCompleted, LogLevel.Debug),
                (CoreEventId.CollectionChangeDetected, LogLevel.Debug),
                (CoreEventId.ForeignKeyChangeDetected, LogLevel.Debug),
                (CoreEventId.PropertyChangeDetected, LogLevel.Debug),
                (CoreEventId.ReferenceChangeDetected, LogLevel.Debug),
                (CoreEventId.StateChanged, LogLevel.Debug)
            ));
        });
    }
}
