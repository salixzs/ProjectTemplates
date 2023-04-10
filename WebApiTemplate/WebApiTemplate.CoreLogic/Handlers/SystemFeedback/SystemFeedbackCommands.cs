using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Database.Orm;

namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedback;

/// <inheritdoc cref="ISystemFeedbackCommands"/>
public sealed class SystemFeedbackCommands : ISystemFeedbackCommands
{
    private readonly WebApiTemplateDbContext _db;

    /// <inheritdoc cref="ISystemFeedbackCommands"/>
    public SystemFeedbackCommands(WebApiTemplateDbContext databaseContext) => _db = databaseContext;

    /// <inheritdoc/>
    public async Task<int> CreateFeedback(Domain.SystemFeedback.SystemFeedback feedback, CancellationToken cancellationToken)
    {
        var newFeedbackRecord = new SystemFeedbackRecord
        {
            Title = feedback.Title,
            Content = feedback.Content,
            SystemInfo = feedback.SystemInfo,
            Category = feedback.Category,
            Priority = feedback.Priority,
            Status = feedback.Status,
            CreatedAt = DateTimeOffset.Now,
            ModifiedAt = DateTimeOffset.Now
        };

        _db.SystemFeedbacks.Add(newFeedbackRecord);
        await _db.SaveChangesAsync(cancellationToken);
        return newFeedbackRecord.Id;
    }
}
