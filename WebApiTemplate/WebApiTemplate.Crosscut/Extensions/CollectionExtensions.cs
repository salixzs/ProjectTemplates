namespace WebApiTemplate.Crosscut.Extensions;

public static class CollectionExtensions
{
    /// <summary>
    /// Adds object to the list if object is not null.
    /// </summary>
    /// <param name="collection">Strongly typed collection of objects.</param>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="value">Object to insert into IList.</param>
    public static void AddIfNotNull<T>(this IList<T> collection, T value)
    {
        if (value.IsGenericValueNull())
        {
            return;
        }

        collection.Add(value);
    }
}
