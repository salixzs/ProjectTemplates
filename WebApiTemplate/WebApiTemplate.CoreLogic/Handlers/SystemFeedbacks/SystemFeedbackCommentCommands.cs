using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Crosscut.Exceptions;
using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Translations;

namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;

/// <inheritdoc cref="ISystemFeedbackCommentCommands"/>
public sealed class SystemFeedbackCommentCommands : ISystemFeedbackCommentCommands
{
    private readonly WebApiTemplateDbContext _db;

    private readonly ITranslate<ErrorMessageTranslations> _l10n;

    /// <inheritdoc cref="ISystemFeedbackCommentCommands"/>
    public SystemFeedbackCommentCommands(WebApiTemplateDbContext databaseContext, ITranslate<ErrorMessageTranslations> l10n)
    {
        _db = databaseContext;
        _l10n = l10n;
    }

    /// <inheritdoc/>
    public async Task<int> Create(Domain.SystemFeedbacks.SystemFeedbackComment feedbackComment, CancellationToken cancellationToken)
    {
        // TODO: Add user/auth
        var feedbackRecord = _db.SystemFeedbacks.FirstOrDefault(f => f.Id == feedbackComment.SystemFeedbackId)
            ?? throw new BusinessException(
                _l10n[
                    nameof(ErrorMessageTranslations.Record_NotFoundById),
                    "System feedback",
                    feedbackComment.SystemFeedbackId ?? 0],
                BusinessExceptionType.RequestError);
        var newFeedbackCommentRecord =
            new SystemFeedbackCommentRecord
            {
                Content = feedbackComment.Content,
                CreatedAt = DateTimeOffset.Now,
            };
        feedbackRecord.Comments.Add(newFeedbackCommentRecord);
        await _db.SaveChangesAsync(cancellationToken);
        return newFeedbackCommentRecord.Id;
    }

    /// <inheritdoc/>
    public async Task Update(Domain.SystemFeedbacks.SystemFeedbackComment feedbackComment, CancellationToken cancellationToken)
    {
        // TODO: Add user/auth
        var feedbackCommentRecord = _db.SystemFeedbackComments.FirstOrDefault(fc => fc.Id == feedbackComment.Id)
            ?? throw new BusinessException(
                _l10n[
                    nameof(ErrorMessageTranslations.Record_NotFoundById),
                    "System feedback comment",
                    feedbackComment.Id],
                BusinessExceptionType.RequestError);

        feedbackCommentRecord.Content = feedbackComment.Content;
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Delete(int feedbackCommentId, CancellationToken cancellationToken)
    {
        // TODO: Add user/auth
        var feedbackCommentRecord = await _db.SystemFeedbackComments
            .Where(fc => fc.Id == feedbackCommentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (feedbackCommentRecord == null)
        {
            return;
        }

        _db.SystemFeedbackComments.Remove(feedbackCommentRecord);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
