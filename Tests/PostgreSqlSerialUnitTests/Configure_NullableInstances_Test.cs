using Xunit.Sdk;

namespace PostgreSqlSerialUnitTests;

#nullable enable

public partial class PostgreSqlSerialUnitTest
{
    class TestClass1
    {
        public int? Foo1 { get; set; }
        public string? Bar1 { get; set; }
    }

    class TestClass2
    {
        public int? Foo2 { get; set; }
        public string? Bar2 { get; set; }
    }

    [Fact]
    public void Configure_NullableInstances_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
        
        NormOptions.Configure(options =>
        {
            options.NullableInstances = true;
        });

        var (result1, result2) = connection
            .Read<TestClass1?, TestClass2?>("select 1 as foo1, 'bar' as bar1, null as foo2, null as bar2")
            .Single();

        Assert.NotNull(result1);
        Assert.Equal(1, result1?.Foo1);
        Assert.Equal("bar", result1?.Bar1);

        Assert.Null(result2);

        NormOptions.Configure(options =>
        {
            options.NullableInstances = false;
        });

        (result1, result2) = connection
            .Read<TestClass1?, TestClass2?>("select 1 as foo1, 'bar' as bar1, null as foo2, null as bar2")
            .Single();

        Assert.NotNull(result1);
        Assert.Equal(1, result1?.Foo1);
        Assert.Equal("bar", result1?.Bar1);

        Assert.NotNull(result2);
        Assert.Null(result2?.Foo2);
        Assert.Null(result2?.Bar2);
    }
}