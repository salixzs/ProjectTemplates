namespace WebApiTemplate.Crosscut.Extensions;

public static class GenericExtensions
{
    /// <summary>
    /// Determines whether Generic type is null. Provides safety for value types (non-nullable).
    /// </summary>
    /// <typeparam name="T">Type of Generic type.</typeparam>
    /// <param name="value">The value of generic type.</param>
    /// <returns>
    /// <c>true</c> if generic value is null; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsGenericValueNull<T>(this T value)
    {
        var type = typeof(T);
        return (type.IsClass || Nullable.GetUnderlyingType(type) != null)
               && EqualityComparer<T>.Default.Equals(value, default);
    }
}
