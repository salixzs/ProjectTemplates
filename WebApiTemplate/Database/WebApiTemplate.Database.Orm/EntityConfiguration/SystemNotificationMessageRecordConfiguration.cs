namespace WebApiTemplate.Database.Orm.EntityConfiguration;

internal sealed class SystemNotificationMessageRecordConfiguration : IEntityTypeConfiguration<SystemNotificationMessageRecord>
{
    public void Configure(EntityTypeBuilder<SystemNotificationMessageRecord> builder)
    {
        builder.ToTable("SystemNotificationMessages");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn(1000, 1);

        builder.Property(e => e.LanguageCode)
            .IsRequired()
            .HasColumnType("char(2)");

        builder.Property(e => e.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property<int>("SystemNotificationId");
        builder.HasOne(d => d.SystemNotification)
            .WithMany(p => p.Messages)
            .HasForeignKey("SystemNotificationId");
    }
}
