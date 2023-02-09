namespace WebApiTemplate.Crosscut.Extensions;

/// <summary>
/// Extension methods to <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines whether the specified type is primitive/simple specific data holding type (not DTO or business class).<br/>
    /// It returns true for framework types, like int, long, double and similar.<br/>
    /// NOTE: Also counts DateTime/DateOnly/TimeOnly/DateTimeOffset, Decimal and String as "primitives/simples".
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>
    /// <c>true</c> if the specified type is simple; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsSimple(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            // Unwrapping nullable type
            type = type.GetGenericArguments()[0];
        }

        return type.IsPrimitive
               || type.Equals(typeof(string))
               || type.Equals(typeof(decimal))
               || type.Equals(typeof(DateTime))
               || type.Equals(typeof(DateOnly))
               || type.Equals(typeof(TimeOnly))
               || type.Equals(typeof(TimeSpan))
               || type.Equals(typeof(DateTimeOffset));
    }
}
