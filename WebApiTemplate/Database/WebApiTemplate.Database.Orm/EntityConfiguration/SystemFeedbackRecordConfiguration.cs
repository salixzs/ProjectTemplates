namespace WebApiTemplate.Database.Orm.EntityConfiguration;

internal sealed class SystemFeedbackRecordConfiguration : IEntityTypeConfiguration<SystemFeedbackRecord>
{
    public void Configure(EntityTypeBuilder<SystemFeedbackRecord> builder)
    {
        builder.ToTable("SystemFeedbacks");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn(1000, 1);

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Content)
            .IsRequired(false);

        builder.Property(e => e.SystemInfo)
            .IsRequired(false);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.ModifiedAt)
            .IsRequired();

        builder.Property(e => e.CompletedAt)
            .IsRequired(false);

        builder.Property(e => e.Category)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(e => e.Priority)
            .IsRequired()
            .HasConversion<int>();
    }
}
