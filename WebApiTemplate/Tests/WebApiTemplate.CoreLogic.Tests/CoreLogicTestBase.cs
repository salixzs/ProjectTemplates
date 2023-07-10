using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Database.Orm.Fakes;
using WebApiTemplate.Domain.Fakes;

namespace WebApiTemplate.CoreLogic.Tests;

/// <summary>
/// Helper base class providing means to initialize in-memory or SqLite DB contexts.
/// </summary>
public abstract class CoreLogicTestBase : IDisposable
{
    private EntityFakesFactory? _databaseObjectsFakesFactory;

    /// <summary>
    /// Use Factory in tests to get fakes of database objects and their lists.
    /// <code>
    /// GetTestObject&lt;User&gt;();
    /// GetTestObjects&lt;Account&gt;(10, 30);
    /// </code>
    /// </summary>
    protected EntityFakesFactory DatabaseDataFaker =>
        _databaseObjectsFakesFactory ??= new EntityFakesFactory();

    private DomainFakesFactory? _domainObjectsFakesFactory;

    /// <summary>
    /// Use Factory in tests to get fakes of Domain objects and their lists.
    /// <code>
    /// GetTestObject&lt;User&gt;();
    /// GetTestObjects&lt;Account&gt;(10, 30);
    /// </code>
    /// </summary>
    protected DomainFakesFactory DomainDataFaker =>
        _domainObjectsFakesFactory ??= new DomainFakesFactory();

    /// <summary>
    /// Set to TRUE to output transaction events to logger.
    /// </summary>
    public bool LogTransactions { get; set; }

    /// <summary>
    /// Database Context Logger instance.
    /// </summary>
    protected XUnitLogger<WebApiTemplateDbContext> DbLogger { get; init; }

    protected CoreLogicTestBase(ITestOutputHelper output) => DbLogger = new XUnitLogger<WebApiTemplateDbContext>(output);

    /// <summary>
    /// Creates a fake SQLite database context.
    /// </summary>
    protected async Task<WebApiTemplateDbContext> GetDatabaseContextAsync()
    {
        var dbContext = CreateDatabaseContext();

        // SQLite needs to open connection to the DB.
        // Not required for in-memory-database and MS SQL.
        await dbContext.Database.OpenConnectionAsync();
        await dbContext.Database.EnsureCreatedAsync();
        DbLogger.LogStatements.Clear();
        DbLogger.LoggingDisabled = false;
        return dbContext;
    }

    /// <summary>
    /// Creates a fake SQLite database context.
    /// </summary>
    protected WebApiTemplateDbContext GetDatabaseContext()
    {
        var dbContext = CreateDatabaseContext();

        // SQLite needs to open connection to the DB.
        // Not required for in-memory-database and MS SQL.
        dbContext.Database.OpenConnection();
        dbContext.Database.EnsureCreated();
        DbLogger.LogStatements.Clear();
        DbLogger.LoggingDisabled = false;
        return dbContext;
    }

    private WebApiTemplateDbContext CreateDatabaseContext()
    {
        var builder = new DbContextOptionsBuilder<WebApiTemplateDbContext>();
        builder.UseSqlite("DataSource=:memory:");
        SetUpLogging(builder);
        return new WebApiTemplateDbContext(builder.Options);
    }

    private void SetUpLogging<TContext>(DbContextOptionsBuilder<TContext> builder)
        where TContext : DbContext
    {
        builder.ConfigureWarnings(warnings => warnings
            .Default(WarningBehavior.Ignore)
            .Log(GetLoggableEvents())
            .Ignore(GetIgnoreEvents()));
        builder.EnableSensitiveDataLogging();

        var factoryMock = new Mock<ILoggerFactory>();
        factoryMock.Setup(f => f.CreateLogger(It.IsAny<string>())).Returns(DbLogger);
        builder.UseLoggerFactory(factoryMock.Object);

        // Prevent Context creation (in-memory DB creation events) from appearing in log.
        DbLogger.LoggingDisabled = true;
    }

    private EventId[] GetIgnoreEvents()
    {
        var ignoreEvents = new List<EventId>
        {
            CoreEventId.AmbiguousEndRequiredWarning,
            CoreEventId.CascadeDelete,
            CoreEventId.CascadeDeleteOrphan,
            CoreEventId.CollectionChangeDetected,
            CoreEventId.CollectionWithoutComparer,
            CoreEventId.ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning,
            CoreEventId.ConflictingKeylessAndKeyAttributesWarning,
            CoreEventId.ConflictingShadowForeignKeysWarning,
            CoreEventId.ContextDisposed,
            CoreEventId.ContextInitialized,
            CoreEventId.CoreBaseId,
            CoreEventId.DetachedLazyLoadingWarning,
            CoreEventId.DetachedLazyLoadingWarning,
            CoreEventId.DetectChangesCompleted,
            CoreEventId.DetectChangesCompleted,
            CoreEventId.DetectChangesStarting,
            CoreEventId.DetectChangesStarting,
            CoreEventId.DistinctAfterOrderByWithoutRowLimitingOperatorWarning,
            CoreEventId.DistinctAfterOrderByWithoutRowLimitingOperatorWarning,
            CoreEventId.DuplicateDependentEntityTypeInstanceWarning,
            CoreEventId.DuplicateDependentEntityTypeInstanceWarning,
            CoreEventId.ExecutionStrategyRetrying,
            CoreEventId.ExecutionStrategyRetrying,
            CoreEventId.FirstWithoutOrderByAndFilterWarning,
            CoreEventId.FirstWithoutOrderByAndFilterWarning,
            CoreEventId.ForeignKeyAttributesOnBothNavigationsWarning,
            CoreEventId.ForeignKeyAttributesOnBothNavigationsWarning,
            CoreEventId.ForeignKeyAttributesOnBothPropertiesWarning,
            CoreEventId.ForeignKeyAttributesOnBothPropertiesWarning,
            CoreEventId.ForeignKeyChangeDetected,
            CoreEventId.ForeignKeyChangeDetected,
            CoreEventId.IncompatibleMatchingForeignKeyProperties,
            CoreEventId.IncompatibleMatchingForeignKeyProperties,
            CoreEventId.InvalidIncludePathError,
            CoreEventId.InvalidIncludePathError,
            CoreEventId.LazyLoadOnDisposedContextWarning,
            CoreEventId.LazyLoadOnDisposedContextWarning,
            CoreEventId.ManyServiceProvidersCreatedWarning,
            CoreEventId.ManyServiceProvidersCreatedWarning,
            CoreEventId.MultipleInversePropertiesSameTargetWarning,
            CoreEventId.MultipleInversePropertiesSameTargetWarning,
            CoreEventId.MultipleNavigationProperties,
            CoreEventId.MultipleNavigationProperties,
            CoreEventId.MultiplePrimaryKeyCandidates,
            CoreEventId.MultiplePrimaryKeyCandidates,
            CoreEventId.NavigationBaseIncludeIgnored,
            CoreEventId.NavigationBaseIncluded,
            CoreEventId.NavigationBaseIncluded,
            CoreEventId.NavigationLazyLoading,
            CoreEventId.NonOwnershipInverseNavigationWarning,
            CoreEventId.OldModelVersionWarning,
            CoreEventId.OptimisticConcurrencyException,
            CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning,
            CoreEventId.PossibleUnintendedCollectionNavigationNullComparisonWarning,
            CoreEventId.PossibleUnintendedReferenceComparisonWarning,
            CoreEventId.PropertyChangeDetected,
            CoreEventId.QueryCompilationStarting,
            CoreEventId.QueryExecutionPlanned,
            CoreEventId.QueryIterationFailed,
            CoreEventId.RedundantAddServicesCallWarning,
            CoreEventId.RedundantForeignKeyWarning,
            CoreEventId.RedundantIndexRemoved,
            CoreEventId.ReferenceChangeDetected,
            CoreEventId.RequiredAttributeOnCollection,
            CoreEventId.RequiredAttributeOnSkipNavigation,
            CoreEventId.RowLimitingOperationWithoutOrderByWarning,
            CoreEventId.SaveChangesStarting,
            CoreEventId.SensitiveDataLoggingEnabledWarning,
            CoreEventId.ServiceProviderCreated,
            CoreEventId.ServiceProviderDebugInfo,
            CoreEventId.ShadowForeignKeyPropertyCreated,
            CoreEventId.ShadowPropertyCreated,
            CoreEventId.SkipCollectionChangeDetected,
            CoreEventId.StartedTracking,
            CoreEventId.StateChanged,
            CoreEventId.ValueGenerated,
            RelationalEventId.AllIndexPropertiesNotToMappedToAnyTable,
            RelationalEventId.AmbientTransactionEnlisted,
            RelationalEventId.AmbientTransactionWarning,
            RelationalEventId.BatchExecutorFailedToReleaseSavepoint,
            RelationalEventId.BatchExecutorFailedToRollbackToSavepoint,
            RelationalEventId.BatchReadyForExecution,
            RelationalEventId.BatchSmallerThanMinBatchSize,
            RelationalEventId.BoolWithDefaultWarning,
            RelationalEventId.ColumnOrderIgnoredWarning,
            RelationalEventId.CommandCreated,
            RelationalEventId.CommandCreating,
            RelationalEventId.CommandExecuting,
            RelationalEventId.ConnectionClosed,
            RelationalEventId.ConnectionClosing,
            RelationalEventId.ConnectionError,
            RelationalEventId.ConnectionOpened,
            RelationalEventId.ConnectionOpening,
            RelationalEventId.CreatedTransactionSavepoint,
            RelationalEventId.CreatingTransactionSavepoint,
            RelationalEventId.DataReaderDisposing,
            RelationalEventId.DuplicateColumnOrders,
            RelationalEventId.ExplicitTransactionEnlisted,
            RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables,
            RelationalEventId.IndexPropertiesBothMappedAndNotMappedToTable,
            RelationalEventId.IndexPropertiesMappedToNonOverlappingTables,
            RelationalEventId.MigrateUsingConnection,
            RelationalEventId.MigrationApplying,
            RelationalEventId.MigrationAttributeMissingWarning,
            RelationalEventId.MigrationGeneratingDownScript,
            RelationalEventId.MigrationGeneratingUpScript,
            RelationalEventId.MigrationReverting,
            RelationalEventId.MigrationsNotApplied,
            RelationalEventId.MigrationsNotFound,
            RelationalEventId.ModelValidationKeyDefaultValueWarning,
            RelationalEventId.MultipleCollectionIncludeWarning,
            RelationalEventId.OptionalDependentWithAllNullPropertiesWarning,
            RelationalEventId.OptionalDependentWithoutIdentifyingPropertyWarning,
            RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning,
            RelationalEventId.ReleasedTransactionSavepoint,
            RelationalEventId.ReleasingTransactionSavepoint,
            RelationalEventId.RolledBackToTransactionSavepoint,
            RelationalEventId.RollingBackToTransactionSavepoint,
            RelationalEventId.TransactionCommitting,
            RelationalEventId.TransactionRollingBack,
            RelationalEventId.TransactionStarting
        };

        if (!LogTransactions)
        {
            ignoreEvents.AddRange(
                new[]
                {
                    RelationalEventId.TransactionStarted,
                    RelationalEventId.TransactionUsed,
                    RelationalEventId.TransactionCommitted,
                    RelationalEventId.TransactionRolledBack,
                    RelationalEventId.TransactionError,
                    RelationalEventId.TransactionDisposed
                });
        }

        return ignoreEvents.ToArray();
    }

    private EventId[] GetLoggableEvents()
    {
        var logEvents = new List<EventId>
        {
            CoreEventId.SaveChangesCompleted,
            CoreEventId.SaveChangesFailed,
            RelationalEventId.CommandError,
            RelationalEventId.CommandExecuted
        };

        if (LogTransactions)
        {
            logEvents.AddRange(
                new[]
                {
                    RelationalEventId.TransactionStarted,
                    RelationalEventId.TransactionUsed,
                    RelationalEventId.TransactionCommitted,
                    RelationalEventId.TransactionRolledBack,
                    RelationalEventId.TransactionError,
                    RelationalEventId.TransactionDisposed
                });
        }

        return logEvents.ToArray();
    }

    public void Dispose()
    {
        DbLogger.Dispose();
        GC.SuppressFinalize(this);
    }
}
