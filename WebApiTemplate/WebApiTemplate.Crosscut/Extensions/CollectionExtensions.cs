namespace WebApiTemplate.Crosscut.Extensions;

public static class CollectionExtensions
{
    /// <summary>
    /// Checks whether collection is null or empty.
    /// </summary>
    /// <param name="collection">Strongly typed collection of objects.</param>
    /// <typeparam name="T">IEnumerable collection containing objects type.</typeparam>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection?.Any() != true;

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

    /// <summary>
    /// Removes all instances of [itemToRemove] from array [original]
    /// Returns the new array, without modifying [original] directly.
    /// </summary>
    /// <typeparam name="T">Type of elements in array.</typeparam>
    /// <param name="original">Array to modify.</param>
    /// <param name="itemToRemove">item (its value) to remove from array.</param>
    public static T[] RemoveFromArray<T>(this T[] original, T itemToRemove)
    {
        var numIdx = Array.IndexOf(original, itemToRemove);
        if (numIdx == -1)
        {
            return original;
        }

        var tmp = new List<T>(original);
        tmp.RemoveAt(numIdx);
        return tmp.ToArray();
    }
}