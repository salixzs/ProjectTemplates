using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Crosscut.Exceptions;
using WebApiTemplate.Database.Orm;
using WebApiTemplate.Domain.SystemFeedbacks;
using WebApiTemplate.Translations;

namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;

/// <inheritdoc cref="ISystemFeedbackQueries"/>
public sealed class SystemFeedbackQueries : ISystemFeedbackQueries
{
    private readonly WebApiTemplateDbContext _db;

    private readonly ITranslate<ErrorMessageTranslations> _l10n;

    /// <inheritdoc cref="ISystemFeedbackQueries"/>
    public SystemFeedbackQueries(WebApiTemplateDbContext databaseContext, ITranslate<ErrorMessageTranslations> l10n)
    {
        _db = databaseContext;
        _l10n = l10n;
    }

    /// <inheritdoc/>
    public async Task<SystemFeedback> GetFeedbackById(int feedbackId, CancellationToken cancellationToken)
    {
        var feedback = await _db.SystemFeedbacks
            .Include(f => f.Comments)
            .Where(f => f.Id == feedbackId)
            .Select(db =>
                new SystemFeedback
                {
                    Id = db.Id,
                    Title = db.Title,
                    Content = db.Content,
                    SystemInfo = db.SystemInfo,
                    Category = db.Category,
                    Priority = db.Priority,
                    Status = db.Status,
                    CreatedAt = db.CreatedAt,
                    ModifiedAt = db.ModifiedAt,
                    Comments = db.Comments
                        .OrderByDescending(f => f.CreatedAt)
                        .Select(dbc =>
                            new SystemFeedbackComment
                            {
                                Id = dbc.Id,
                                Content = dbc.Content,
                                CreatedAt = dbc.CreatedAt,
                                SystemFeedbackId = db.Id
                            })
                        .ToList()
                })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new BusinessException(
                _l10n[nameof(ErrorMessageTranslations.Record_NotFoundById), "FeedbackById", feedbackId],
                BusinessExceptionType.RequestError);

        return feedback;
    }

    /// <inheritdoc/>
    public async Task<List<SystemFeedback>> GetFeedbacks(
        SystemFeedbackFilter filter,
        CancellationToken cancellationToken) =>
        await _db.SystemFeedbacks
            .FilterByUsers(filter)
            .FilterByTitle(filter)
            .FilterByContents(filter)
            .FilterByCategories(filter)
            .FilterByPriority(filter)
            .FilterByStatus(filter)
            .Select(db =>
                new SystemFeedback
                {
                    Id = db.Id,
                    Title = db.Title,
                    Content = db.Content,
                    SystemInfo = db.SystemInfo,
                    Category = db.Category,
                    Priority = db.Priority,
                    Status = db.Status,
                    CreatedAt = db.CreatedAt,
                    ModifiedAt = db.ModifiedAt
                })
            .ToListAsync(cancellationToken);
}
