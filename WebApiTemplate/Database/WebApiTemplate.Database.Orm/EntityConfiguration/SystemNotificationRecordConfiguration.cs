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
            .IsRequired()
            .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

        builder.Property(e => e.EndTime)
            .IsRequired()
            .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

        builder.Property(e => e.Type)
            .IsRequired()
            .HasDefaultValue(0)
            .HasConversion<int>();

        builder.Property(e => e.EmphasizeSince)
            .IsRequired()
            .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

        builder.Property(e => e.EmphasizeType)
            .IsRequired()
            .HasDefaultValue(0)
            .HasConversion<int>();

        builder.Property(e => e.CountdownSince)
            .IsRequired()
            .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));
    }
}
