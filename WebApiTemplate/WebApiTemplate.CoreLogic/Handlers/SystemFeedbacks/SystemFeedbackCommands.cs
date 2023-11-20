using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Crosscut.Exceptions;
using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Translations;

namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;

/// <inheritdoc cref="ISystemFeedbackCommands"/>
public sealed class SystemFeedbackCommands(WebApiTemplateDbContext databaseContext, ITranslate<ErrorMessageTranslations> l10n)
    : ISystemFeedbackCommands
{
    /// <inheritdoc/>
    public async Task<int> Create(Domain.SystemFeedbacks.SystemFeedback feedback, CancellationToken cancellationToken)
    {
        // TODO: Add user/auth
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

        databaseContext.SystemFeedbacks.Add(newFeedbackRecord);
        await databaseContext.SaveChangesAsync(cancellationToken);
        return newFeedbackRecord.Id;
    }

    /// <inheritdoc/>
    public async Task Update(Domain.SystemFeedbacks.SystemFeedback feedback, CancellationToken cancellationToken)
    {
        // TODO: Add user/auth
        var feedbackRecord = databaseContext.SystemFeedbacks.FirstOrDefault(f => f.Id == feedback.Id)
            ?? throw new BusinessException(
                l10n[
                    nameof(ErrorMessageTranslations.Record_NotFoundById),
                    "System feedback",
                    feedback.Id],
                BusinessExceptionType.RequestError);

        feedbackRecord.Title = feedback.Title;
        feedbackRecord.Content = feedback.Content;
        feedbackRecord.SystemInfo = feedback.SystemInfo;
        feedbackRecord.Category = feedback.Category;
        feedback.Priority = feedback.Priority;
        feedback.Status = feedback.Status;
        feedbackRecord.ModifiedAt = DateTimeOffset.Now;

        await databaseContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Delete(int feedbackId, CancellationToken cancellationToken)
    {
        var feedbackRecord = await databaseContext.SystemFeedbacks
            .Where(feedback => feedback.Id == feedbackId)
            .FirstOrDefaultAsync(cancellationToken);

        if (feedbackRecord == null)
        {
            return;
        }

        databaseContext.SystemFeedbacks.Remove(feedbackRecord);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}
