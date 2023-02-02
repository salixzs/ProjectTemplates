using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Database.Orm.EntityConfiguration;

internal sealed class SystemNotificationRecordConfiguration : IEntityTypeConfiguration<SystemNotificationRecord>
{
    public void Configure(EntityTypeBuilder<SystemNotificationRecord> builder)
    {
        builder.ToTable("SystemNotifications");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn(1000, 1);

        builder.Property(e => e.StartTime)
            .IsRequired();

        builder.Property(e => e.EndTime)
            .IsRequired();

        builder.Property(e => e.Type)
            .IsRequired()
            .HasDefaultValue(SystemNotificationType.Normal)
            .HasConversion(new EnumToNumberConverter<SystemNotificationType, byte>());

        builder.Property(e => e.EmphasizeSince)
            .IsRequired();

        builder.Property(e => e.EmphasizeType)
            .IsRequired()
            .HasDefaultValue(SystemNotificationType.Normal)
            .HasConversion(new EnumToNumberConverter<SystemNotificationType, byte>());

        builder.Property(e => e.CountdownSince)
            .IsRequired();
    }
}
