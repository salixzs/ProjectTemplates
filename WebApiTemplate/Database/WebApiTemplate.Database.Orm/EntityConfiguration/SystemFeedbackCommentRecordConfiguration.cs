namespace WebApiTemplate.Database.Orm.EntityConfiguration;

internal sealed class SystemFeedbackCommentRecordConfiguration : IEntityTypeConfiguration<SystemFeedbackCommentRecord>
{
    public void Configure(EntityTypeBuilder<SystemFeedbackCommentRecord> builder)
    {
        builder.ToTable("SystemFeedbackComments");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .IsRequired()
            .UseIdentityColumn(1000, 1);

        builder.Property(e => e.Content)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property<int>("SystemFeedbackId");
        builder.HasOne(d => d.SystemFeedback)
            .WithMany(p => p.Comments)
            .HasForeignKey("SystemFeedbackId");
    }
}
