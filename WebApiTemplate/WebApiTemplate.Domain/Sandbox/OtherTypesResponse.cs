namespace WebApiTemplate.Domain.Sandbox;

/// <summary>
/// Other types of .Net, not covered with text, numbers and Date/Times.
/// </summary>
public class OtherTypesResponse
{
    /// <summary>
    /// Boolean TRUE.
    /// </summary>
    public bool BoolTrueValue { get; set; } = true;

    /// <summary>
    /// Boolean FALSE.
    /// </summary>
    public bool BoolFalseValue { get; set; }

    /// <summary>
    /// Nullable boolean as NULL.
    /// </summary>
    public bool? BoolNullableNull { get; set; }

    /// <summary>
    /// Enum value (SandboxType.OtherTypes = 4).
    /// </summary>
    public SandboxType EnumValue { get; set; } = SandboxType.OtherTypes;

    /// <summary>
    /// Array of strings.
    /// </summary>
    public string[] StringArray { get; set; } = new string[3] { "one", "two", "three" };

    /// <summary>
    /// Array of Integer values.
    /// </summary>
    public int[] IntegerArray { get; set; } = new int[3] { 1, 2, 3 };
}

public enum SandboxType
{
    Undefined = 0,
    Numbers = 1,
    Strings = 2,
    DateTimes = 3,
    OtherTypes = 4,
}
