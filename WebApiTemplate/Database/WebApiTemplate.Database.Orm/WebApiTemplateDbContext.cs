using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebApiTemplate.Database.Orm;

/// <summary>
/// MS SQL Database / Entity Framework context to use for database data query and modification.
/// </summary>
public class WebApiTemplateDbContext : DbContext
{
    /// <summary>
    /// MS SQL Database / Entity Framework context to use for database data query and modification.
    /// </summary>
    public WebApiTemplateDbContext() => ChangeTracker.LazyLoadingEnabled = false;

    /// <summary>
    /// MS SQL Database / Entity Framework context to use for database data query and modification.
    /// </summary>
    public WebApiTemplateDbContext(DbContextOptions<WebApiTemplateDbContext> options)
        : base(options) => ChangeTracker.LazyLoadingEnabled = false;

    /// <summary>
    /// System-wide notifications/alerts for end-users.
    /// <code>TABLE: dbo.SystemNotifications</code>
    /// </summary>
    public virtual DbSet<SystemNotificationRecord> SystemNotifications { get; set; } = null!;

    /// <summary>
    /// Child objects of <see cref="SystemNotifications">System notifications</see> holding notification messages in one or more languages.<br/>
    /// Cascade deleted if parent system notification is deleted.
    /// <code>TABLE: dbo.SystemNotificationMessages</code>
    /// </summary>
    public virtual DbSet<SystemNotificationMessageRecord> SystemNotificationMessages { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // FOR TESTING!
            // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToBinaryConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
            // This only supports millisecond precision, but should be sufficient for most use cases.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }

            // Make SQLite Case Insensitive, like MS SQL
            modelBuilder.UseCollation("NOCASE");
        }
    }
}
