using Bogus;
using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Database.Orm.Fakes.Fakes;

namespace WebApiTemplate.Database.Orm.Fakes;

/// <summary>
/// Use Factory in Tests/Docs to get fake Database Entities and their lists.
/// <code>
/// EntityFakesFactory.Instance.GetTestObject&lt;User&gt;();
/// EntityFakesFactory.Instance.GetTestObjects&lt;Account&gt;(10, 30);
/// </code>
/// </summary>
public sealed class EntityFakesFactory
{
    private readonly Dictionary<string, Func<object>> _instantiators = new();

    /// <summary>
    /// Prevents a default instance of <see cref="EntityFakesFactory"/> class from being created from outside.<br/>
    /// Normal Singleton implementation where class is instantiated in static read-only field.<br/>
    /// Here into Dictionary of Domain API object fakes should be added.
    /// </summary>
    private EntityFakesFactory()
    {
        _instantiators.Add(typeof(SystemNotificationRecord).FullName!, SystemNotificationRecordFake.GetBogus);
        _instantiators.Add(typeof(SystemNotificationMessageRecord).FullName!, SystemNotificationMessageRecordFake.GetBogus);
    }

    static EntityFakesFactory()
    {
    }

    /// <summary>
    /// Use Factory in tests to get fake Database Entities objects and their lists.
    /// <code>
    /// EntityFakesFactory.Instance.GetTestObject&lt;User&gt;();
    /// EntityFakesFactory.Instance.GetTestObjects&lt;Account&gt;(10, 30);
    /// </code>
    /// </summary>
    public static EntityFakesFactory Instance { get; } = new();

    public bool HasFakeFor(string fullName) => _instantiators.ContainsKey(fullName);

    /// <summary>
    /// Method to get single fake Domain API object.
    /// <code>
    /// EntityFakesFactory.Instance.GetTestObject&lt;User&gt;();
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of object to create fake for.</typeparam>
    /// <exception cref="ArgumentException">Faker not implemented.</exception>
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
    /// EntityFakesFactory.Instance.GetTestObjects&lt;User&gt;(minCount, maxCount);
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of object to create fake for.</typeparam>
    /// <exception cref="ArgumentException">Faker not implemented.</exception>
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
    /// EntityFakesFactory.Instance.GetTestObjects&lt;User&gt;(count);
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
