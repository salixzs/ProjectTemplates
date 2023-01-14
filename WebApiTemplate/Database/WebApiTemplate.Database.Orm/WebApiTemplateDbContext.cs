using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Database.Orm.Entities;

namespace WebApiTemplate.Database.Orm;

public class WebApiTemplateDbContext : DbContext
{
    public WebApiTemplateDbContext() => ChangeTracker.LazyLoadingEnabled = false;

    public WebApiTemplateDbContext(DbContextOptions<WebApiTemplateDbContext> options)
        : base(options) => ChangeTracker.LazyLoadingEnabled = false;

    public virtual DbSet<SystemNotificationRecord> SystemNotifications { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
