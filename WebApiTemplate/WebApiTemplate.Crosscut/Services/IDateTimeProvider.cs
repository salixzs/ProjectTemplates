namespace WebApiTemplate.Crosscut.Services;

/// <summary>
/// Abstracts away system clock to avoid usage of static DateTimeOffset.Now, which is hard to unit-test.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Default implementation (production) returns <c>DateTimeOffset.Now</c>.<br/>
    /// Use mocking in tests to return desired value.
    /// </summary>
    DateTimeOffset DateTimeOffsetNow { get; }

    /// <summary>
    /// Default implementation (production) returns <c>DateTimeOffset.UtcNow</c>.<br/>
    /// Use mocking in tests to return desired value.
    /// </summary>
    DateTimeOffset DateTimeOffsetUtcNow { get; }

    /// <summary>
    /// Default implementation (production) returns <c>DateTime.Now</c>.<br/>
    /// Use mocking in tests to return desired value.
    /// </summary>
    DateTime DateTimeNow { get; }

    /// <summary>
    /// Default implementation (production) returns <c>DateTime.UtcNow</c>.<br/>
    /// Use mocking in tests to return desired value.
    /// </summary>
    DateTime DateTimeUtcNow { get; }
}
