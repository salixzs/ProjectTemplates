using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Crosscut.Exceptions;
using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Translations;

namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedback;

/// <inheritdoc cref="ISystemFeedbackCommands"/>
public sealed class SystemFeedbackCommands : ISystemFeedbackCommands
{
    private readonly WebApiTemplateDbContext _db;

    private readonly ITranslate<ErrorMessageTranslations> _l10n;

    /// <inheritdoc cref="ISystemFeedbackCommands"/>
    public SystemFeedbackCommands(WebApiTemplateDbContext databaseContext, ITranslate<ErrorMessageTranslations> l10n)
    {
        _db = databaseContext;
        _l10n = l10n;
    }

    /// <inheritdoc/>
    public async Task<int> Create(Domain.SystemFeedback.SystemFeedback feedback, CancellationToken cancellationToken)
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

        _db.SystemFeedbacks.Add(newFeedbackRecord);
        await _db.SaveChangesAsync(cancellationToken);
        return newFeedbackRecord.Id;
    }

    /// <inheritdoc/>
    public async Task Update(Domain.SystemFeedback.SystemFeedback feedback, CancellationToken cancellationToken)
    {
        // TODO: Add user/auth
        var feedbackRecord = _db.SystemFeedbacks.FirstOrDefault(f => f.Id == feedback.Id)
            ?? throw new BusinessException(
                _l10n[
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

        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Delete(int feedbackId, CancellationToken cancellationToken)
    {
        var feedbackRecord = await _db.SystemFeedbacks
            .Where(feedback => feedback.Id == feedbackId)
            .FirstOrDefaultAsync(cancellationToken);

        if (feedbackRecord == null)
        {
            return;
        }

        _db.SystemFeedbacks.Remove(feedbackRecord);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
