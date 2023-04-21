using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Domain.SystemFeedbacks;

namespace WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;

public static class SystemFeedbackQueryExtensions
{
    public static IQueryable<SystemFeedbackRecord> FilterByTitle(
        this IQueryable<SystemFeedbackRecord> query,
        SystemFeedbackFilter filter)
    {
        if (string.IsNullOrWhiteSpace(filter.TitleContains))
        {
            return query;
        }

        var titleContains = filter.TitleContains.Trim();
        return query.Where(f => f.Title.Contains(titleContains));
    }

    public static IQueryable<SystemFeedbackRecord> FilterByContents(
        this IQueryable<SystemFeedbackRecord> query,
        SystemFeedbackFilter filter)
    {
        if (string.IsNullOrWhiteSpace(filter.ContentContains))
        {
            return query;
        }

        var contentContains = filter.ContentContains.Trim();
        return query.Where(f => f.Content!.Contains(contentContains));
    }

    public static IQueryable<SystemFeedbackRecord> FilterByCategories(
        this IQueryable<SystemFeedbackRecord> query,
        SystemFeedbackFilter filter)
    {
        if (filter.Categories == null || filter.Categories.Count == 0)
        {
            return query;
        }

        if (filter.Categories.Count == 1)
        {
            return query.Where(f => f.Category == filter.Categories[0]);
        }

        return query.Where(f => filter.Categories.Contains(f.Category));
    }

    public static IQueryable<SystemFeedbackRecord> FilterByStatus(
        this IQueryable<SystemFeedbackRecord> query,
        SystemFeedbackFilter filter)
    {
        if (filter.Statuses == null || filter.Statuses.Count == 0)
        {
            return query;
        }

        if (filter.Statuses.Count == 1)
        {
            return query.Where(f => f.Status == filter.Statuses[0]);
        }

        return query.Where(f => filter.Statuses.Contains(f.Status));
    }

    public static IQueryable<SystemFeedbackRecord> FilterByPriority(
        this IQueryable<SystemFeedbackRecord> query,
        SystemFeedbackFilter filter)
    {
        if (filter.Priorities == null || filter.Priorities.Count == 0)
        {
            return query;
        }

        if (filter.Priorities.Count == 1)
        {
            return query.Where(f => f.Priority == filter.Priorities[0]);
        }

        return query.Where(f => filter.Priorities.Contains(f.Priority));
    }
}
