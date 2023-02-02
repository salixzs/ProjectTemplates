using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class GenericExtensionsIsGenericNullValueTests
{
    [Fact]
    public void GenericsExt_IsGenericNullValueType_ReturnsFalse()
    {
        var testable = 454L;
        testable.IsGenericValueNull().Should().BeFalse();
    }

    [Fact]
    public void GenericsExt_IsGenericNullValueNullableType_ReturnsFalseForValue()
    {
        long? testable = 454L;
        testable.IsGenericValueNull().Should().BeFalse();
    }

    [Fact]
    public void GenericsExt_IsGenericNullValueNullableType_ReturnsTrueForNull()
    {
        long? testable = null;
        testable.IsGenericValueNull().Should().BeTrue();
    }

    [Fact]
    public void GenericsExt_IsGenericNullString_ReturnsFalse()
    {
        var testable = "This is ok";
        testable.IsGenericValueNull().Should().BeFalse();
    }

    [Fact]
    public void GenericsExt_IsGenericNullStringNull_ReturnsTrue()
    {
        string? testable = null;
        testable.IsGenericValueNull().Should().BeTrue();
    }

    [Fact]
    public void GenericsExt_IsGenericNullClassObjectNull_ReturnsTrue()
    {
        ApplicationId? testable = null;
        testable.IsGenericValueNull().Should().BeTrue();
    }

    [Fact]
    public void GenericsExt_IsGenericNullClassObject_ReturnsFalse()
    {
        var testable = new Exception("This is just for reference object test");
        testable.IsGenericValueNull().Should().BeFalse();
    }
}
