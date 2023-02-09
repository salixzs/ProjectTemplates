using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class CollectionExtensionsTests
{
    [Fact]
    public void IsNullOrEmpty_Null_IsTrue() => ((List<int>?)null).IsNullOrEmpty().Should().BeTrue();

    [Fact]
    public void IsNullOrEmpty_Empty_IsTrue()
    {
        var testable = new List<int>();
        testable.IsNullOrEmpty().Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_Elements_IsFalse()
    {
        var testable = new List<int> { 1, 2 };
        testable.IsNullOrEmpty().Should().BeFalse();
    }

    [Fact]
    public void AddIfNotNull_Objects_AddOnlyNonNull()
    {
        var collection = new List<object?>();
        var values = new object?[5]
        {
                new object(),
                new object(),
                null,
                null,
                new object(),
        };

        foreach (var value in values)
        {
            collection.AddIfNotNull(value);
        }

        collection.Should().HaveCount(3);
    }

    [Fact]
    public void AddIfNotNull_NullableStructs_AddOnlyNonNull()
    {
        var collection = new List<int?>();
        var values = new int?[7]
        {
                1,
                2,
                null,
                null,
                70,
                null,
                4,
        };

        foreach (var value in values)
        {
            collection.AddIfNotNull(value);
        }

        collection.Should().HaveCount(4);
    }
}
