namespace WebApiTemplate.Domain.Sandbox;

public class OtherTypesResponse
{
    public bool BoolTrueValue { get; set; } = true;
    public bool BoolFalseValue { get; set; }
    public bool? BoolNullableNull { get; set; }
    public SandboType EnumValue { get; set; } = SandboType.OtherTypes;
    public string[] StringArray { get; set; } = new string[3] { "one", "two", "three" };
    public int[] IntegerArray { get; set; } = new int[3] { 1, 2, 3 };
}

public enum SandboType
{
    Undefined = 0,
    Numbers = 1,
    Strings = 2,
    DateTimes = 3,
    OtherTypes = 4,
}
