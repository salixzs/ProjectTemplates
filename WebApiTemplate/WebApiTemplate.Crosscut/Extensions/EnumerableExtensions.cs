using System.Globalization;

namespace WebApiTemplate.Crosscut.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// Checks whether collection is null or empty.
    /// </summary>
    /// <param name="collection">Strongly typed collection of objects.</param>
    /// <typeparam name="T">IEnumerable collection containing objects type.</typeparam>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection) => collection?.Any() != true;

    /// <summary>
    /// Returns two collections (partitions).<br/>
    /// One collection contains all elements that match a predicate and the other collection contains all elements that do not match the predicate.
    /// <code>
    /// var (even, odd) = numbers.Partition(n => n % 2 == 0);
    /// Console.WriteLine($"Even numbers: {string.Join(", ", even)}");
    /// Console.WriteLine($"Odd numbers: {string.Join(", ", odd)}");
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of a collection</typeparam>
    /// <param name="source">Source collection.</param>
    /// <param name="predicate">Boolean expression on which to split/partition source collection.</param>
    public static (IEnumerable<T> True, IEnumerable<T> False) Partition<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        var partitions = source.ToLookup(predicate);
        return (partitions[true], partitions[false]);
    }

    /// <summary>
    /// The median is the middle number in a sorted, ascending or descending, list of numbers and can be more descriptive of that data set than the average.
    /// <code>
    /// var median = new[] { 1, 1, 1, 1, 5, 6, 7, 8, 9 }.Median(); // 5
    /// var average = new[] { 1, 1, 1, 1, 5, 6, 7, 8, 9 }.Average(); // 4.333333333333333
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of collection. Should be numeric type (int, long, decimal etc.)</typeparam>
    /// <param name="source">Source collection.</param>
    /// <exception cref="InvalidOperationException">Source collection is empty.</exception>
    public static double Median<T>(this IEnumerable<T> source) where T : IConvertible
    {
        ArgumentNullException.ThrowIfNull(source);

        var sortedList = source.Select(x => x.ToDouble(CultureInfo.InvariantCulture)).OrderBy(x => x).ToList();
        var count = sortedList.Count;

        if (count == 0)
        {
            throw new InvalidOperationException("The source sequence is empty.");
        }

        if (count % 2 == 0)
        {
            return (sortedList[(count / 2) - 1] + sortedList[count / 2]) / 2;
        }

        return sortedList[count / 2];
    }

    /// <summary>
    /// The major is the number that is repeated most often in a set of numbers.
    /// <code>
    /// var major = new[] { 1, 1, 1, 1, 5, 6, 7, 8, 9 }.Major(); // 1
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of collection. Should be number type (int, long, decimal etc.)</typeparam>
    /// <param name="source">Source collection.</param>
    public static IEnumerable<T> Major<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var groups = source.GroupBy(x => x);
        var maxCount = groups.Max(g => g.Count());
        return groups.Where(g => g.Count() == maxCount).Select(g => g.Key);
    }

    /// <summary>
    /// The standard deviation is a measure of how spread out numbers are.
    /// <code>
    /// var standardDeviation = new[] { 1, 1, 1, 1, 5, 6, 7, 8, 9 }.StandardDeviation(); // 3.1622776601683795
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of collection. Should be number type (int, long, decimal etc.)</typeparam>
    /// <param name="source">Source collection.</param>
    /// <exception cref="InvalidOperationException">Source is empty.</exception>
    public static double StandardDeviation<T>(this IEnumerable<T> source) where T : IConvertible
    {
        ArgumentNullException.ThrowIfNull(source);

        var values = source.Select(x => x.ToDouble(CultureInfo.InvariantCulture)).ToList();
        var count = values.Count;

        if (count == 0)
        {
            throw new InvalidOperationException("The source sequence is empty.");
        }

        var avg = values.Average();
        var sum = values.Sum(d => Math.Pow(d - avg, 2));
        return Math.Sqrt(sum / count);
    }
}
