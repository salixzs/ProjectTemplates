global using Bogus;
using WebApiTemplate.Domain.Fakes.Fakes;
using WebApiTemplate.Domain.SystemNotifications;

namespace WebApiTemplate.Domain.Fakes;

/// <summary>
/// Use Factory in Tests/Docs to get fake Domain objects and their lists.
/// <code>
/// DomainFakesFactory.Instance.GetTestObject&lt;User&gt;();
/// DomainFakesFactory.Instance.GetTestObjects&lt;Account&gt;(10, 30);
/// </code>
/// </summary>
public sealed class DomainFakesFactory
{
    private readonly Dictionary<string, Func<object>> _instantiators = new();

    /// <summary>
    /// Prevents a default instance of <see cref="DomainFakesFactory"/> class from being created from outside.<br/>
    /// Normal Singleton implementation where class is instantiated in static read-only field.<br/>
    /// Here into Dictionary of Domain API object fakes should be added.
    /// </summary>
    private DomainFakesFactory()
    {
        _instantiators.Add(typeof(SystemNotification).FullName!, SystemNotificationFake.GetBogus);
        _instantiators.Add(typeof(ActiveSystemNotification).FullName!, ActiveSystemNotificationFake.GetBogus);
        _instantiators.Add(typeof(SystemNotificationMessage).FullName!, SystemNotificationMessageFake.GetBogus);
    }

    static DomainFakesFactory()
    {
    }

    /// <summary>
    /// Use Factory in tests to get fake Domain API objects and their lists.
    /// <code>
    /// DomainFakesFactory.Instance.GetTestObject&lt;User&gt;();
    /// DomainFakesFactory.Instance.GetTestObjects&lt;Account&gt;(10, 30);
    /// </code>
    /// </summary>
    public static DomainFakesFactory Instance { get; } = new();

    public bool HasFakeFor(string fullName) => _instantiators.ContainsKey(fullName);

    /// <summary>
    /// Method to get single fake Domain API object.
    /// <code>
    /// DomainFakesFactory.Instance.GetTestObject&lt;User&gt;();
    /// </code>
    /// </summary>
    /// <exception cref="ArgumentException">Faker definition does not exist.</exception>
    public T GetTestObject<T>() where T : class
    {
        var fullClassName = typeof(T).FullName;
        if (string.IsNullOrEmpty(fullClassName) || !HasFakeFor(fullClassName))
        {
            throw new ArgumentException(
                $"There is no defined instance for {fullClassName} faker object or object does not exist.");
        }

        return ((Faker<T>)_instantiators[fullClassName]()).Generate();
    }

    /// <summary>
    /// Method to get multiple fake Domain API objects.
    /// <code>
    /// DomainFakesFactory.Instance.GetTestObjects&lt;User&gt;(minCount, maxCount);
    /// </code>
    /// </summary>
    /// <exception cref="ArgumentException">Faker definition does not exist.</exception>
    public List<T> GetTestObjects<T>(int min, int max) where T : class
    {
        var fullClassName = typeof(T).FullName;
        if (string.IsNullOrEmpty(fullClassName) || !HasFakeFor(fullClassName))
        {
            throw new ArgumentException(
                $"There is no defined instance for {fullClassName} faker object or object does not exist.");
        }

        return ((Faker<T>)_instantiators[fullClassName]()).GenerateBetween(min, max);
    }

    /// <summary>
    /// Method to get multiple fake Domain API objects.
    /// <code>
    /// DomainFakesFactory.Instance.GetTestObjects&lt;User&gt;(count);
    /// </code>
    /// </summary>
    /// <param name="count">Number of items to generate</param>
    /// <typeparam name="T">Type of entity to create</typeparam>
    /// <returns>List of fake objects created</returns>
    /// <exception cref="ArgumentException">When faker object does not exist for provided type &lt;T&gt;</exception>
    public List<T> GetTestObjects<T>(int count) where T : class
    {
        var fullClassName = typeof(T).FullName;
        if (string.IsNullOrEmpty(fullClassName) || !HasFakeFor(fullClassName))
        {
            throw new ArgumentException(
                $"There is no defined instance for {fullClassName} faker object or object does not exist.");
        }

        return ((Faker<T>)_instantiators[fullClassName]()).Generate(count);
    }
}
