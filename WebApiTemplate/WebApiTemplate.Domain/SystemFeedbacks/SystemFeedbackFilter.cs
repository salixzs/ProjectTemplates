using WebApiTemplate.Enumerations;

namespace WebApiTemplate.Domain.SystemFeedbacks;

public class SystemFeedbackFilter
{
    // TODO: Filter by user has to be when auth is added!
    /// <summary>
    /// Case insensitive string of search term, which should be contained in titles of feedbacks.<br/>
    /// Leaving empty will not do search by it.
    /// </summary>
    public string? TitleContains { get; set; }

    /// <summary>
    /// Case insensitive string of search term, which should be contained in contents of feedbacks.<br/>
    /// Leaving empty will not do search by it.
    /// </summary>
    public string? ContentContains { get; set; }

    /// <summary>
    /// A [white]list of categories to filter by.<br/>
    /// Leaving empty will not do search by it.
    /// </summary>
    public List<SystemFeedbackCategory>? Categories { get; set; }

    /// <summary>
    /// A [white]list of priorities to filter by.<br/>
    /// Leaving empty will not do search by it.
    /// </summary>
    public List<SystemFeedbackPriority>? Priorities { get; set; }

    /// <summary>
    /// A [white]list of statuses to filter by.<br/>
    /// Leaving empty will not do search by it.
    /// </summary>
    public List<SystemFeedbackStatus>? Statuses { get; set; }
}
