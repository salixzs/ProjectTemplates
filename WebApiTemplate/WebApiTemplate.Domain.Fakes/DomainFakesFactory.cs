global using Bogus;
using WebApiTemplate.Domain.Fakes.Fakes;

namespace WebApiTemplate.Domain.Fakes;

/// <summary>
/// Use Factory in Tests/Docs to get fake Domain objects and their lists.
/// <code>
/// GetTestObject&lt;User&gt;();
/// GetTestObjects&lt;Account&gt;(10, 30);
/// </code>
/// </summary>
public sealed class DomainFakesFactory
{
    private readonly Dictionary<string, Func<object>> _instantiators = new();

    private readonly Dictionary<string, object> _instantiatedFakes = new();

    /// <summary>
    /// Use Factory in Tests/Docs to get fake Domain objects and their lists.
    /// <code>
    /// GetTestObject&lt;User&gt;();
    /// GetTestObjects&lt;Account&gt;(10, 30);
    /// </code>
    /// </summary>
    public DomainFakesFactory()
    {
        _instantiators.Add(typeof(SystemNotifications.SystemNotification).FullName!, SystemNotificationFake.GetBogus);
        _instantiators.Add(typeof(SystemNotifications.ActiveSystemNotification).FullName!, ActiveSystemNotificationFake.GetBogus);
        _instantiators.Add(typeof(SystemNotifications.SystemNotificationMessage).FullName!, SystemNotificationMessageFake.GetBogus);
        _instantiators.Add(typeof(SystemFeedbacks.SystemFeedback).FullName!, SystemFeedbackFake.GetBogus);
    }

    private Faker<T> GetFakerInstance<T>() where T : class
    {
        var fullClassName = typeof(T).FullName;
        if (string.IsNullOrEmpty(fullClassName) || !HasFakeFor(fullClassName))
        {
            throw new ArgumentException(
                $"There is no defined instance for {fullClassName} faker object or object does not exist.");
        }

        if (!_instantiatedFakes.ContainsKey(fullClassName))
        {
            _instantiatedFakes[fullClassName] = _instantiators[fullClassName]();
        }

        return (Faker<T>)_instantiatedFakes[fullClassName];
    }

    /// <summary>
    /// Returns true if factory has faker defined for given Domain object type name.
    /// </summary>
    public bool HasFakeFor(string fullName) => _instantiators.ContainsKey(fullName);

    /// <summary>
    /// Method to get single fake Domain API object.
    /// <code>
    /// GetTestObject&lt;User&gt;();
    /// </code>
    /// </summary>
    /// <exception cref="ArgumentException">Faker definition does not exist.</exception>
    public T GetTestObject<T>() where T : class => GetFakerInstance<T>().Generate();

    /// <summary>
    /// Method to get multiple fake Domain API objects.
    /// <code>
    /// GetTestObjects&lt;User&gt;(minCount, maxCount);
    /// </code>
    /// </summary>
    /// <exception cref="ArgumentException">Faker definition does not exist.</exception>
    public List<T> GetTestObjects<T>(int min, int max) where T : class => GetFakerInstance<T>().GenerateBetween(min, max);

    /// <summary>
    /// Method to get multiple fake Domain API objects.
    /// <code>
    /// GetTestObjects&lt;User&gt;(count);
    /// </code>
    /// </summary>
    /// <param name="count">Number of items to generate</param>
    /// <typeparam name="T">Type of entity to create</typeparam>
    /// <returns>List of fake objects created</returns>
    /// <exception cref="ArgumentException">When faker object does not exist for provided type &lt;T&gt;</exception>
    public List<T> GetTestObjects<T>(int count) where T : class => GetTestObjects<T>(count, count);
}
