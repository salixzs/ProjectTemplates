using Bogus;
using WebApiTemplate.Database.Orm.Entities;
using WebApiTemplate.Database.Orm.Fakes.Fakes;

namespace WebApiTemplate.Database.Orm.Fakes;

/// <summary>
/// Use Factory in Tests/Docs to get fake Database Entities and their lists.
/// <code>
/// GetTestObject&lt;User&gt;();
/// GetTestObjects&lt;Account&gt;(10, 30);
/// </code>
/// </summary>
public sealed class EntityFakesFactory
{
    private readonly Dictionary<string, Func<object>> _instantiators = new();

    private readonly Dictionary<string, object> _instantiatedFakes = new();

    /// <summary>
    /// Use Factory in Tests/Docs to get fake Database Entities and their lists.
    /// <code>
    /// GetTestObject&lt;User&gt;();
    /// GetTestObjects&lt;Account&gt;(10, 30);
    /// </code>
    /// </summary>
    public EntityFakesFactory()
    {
        _instantiators.Add(typeof(SystemNotificationRecord).FullName!, SystemNotificationRecordFake.GetBogus);
        _instantiators.Add(typeof(SystemNotificationMessageRecord).FullName!, SystemNotificationMessageRecordFake.GetBogus);
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
    /// Returns true if factory has defined fake for given object type name.
    /// </summary>
    public bool HasFakeFor(string fullName) => _instantiators.ContainsKey(fullName);

    /// <summary>
    /// Method to get single fake Domain API object.
    /// <code>
    /// GetTestObject&lt;User&gt;();
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of object to create fake for.</typeparam>
    /// <exception cref="ArgumentException">Faker not implemented.</exception>
    public T GetTestObject<T>() where T : class => GetFakerInstance<T>().Generate();

    /// <summary>
    /// Method to get multiple fake Domain API objects.
    /// <code>
    /// GetTestObjects&lt;User&gt;(minCount, maxCount);
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of object to create fake for.</typeparam>
    /// <exception cref="ArgumentException">Faker not implemented.</exception>
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
