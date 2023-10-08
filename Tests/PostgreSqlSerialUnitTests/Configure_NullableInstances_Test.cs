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

    class TestClass3
    {
        public int Foo2 { get; set; }
        public string Bar2 { get; set; }
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

        var i = connection
            .Read<TestClass1?>("select null as foo1, null as bar1")
            .Single();

        Assert.Null(i);

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

        NormOptions.Configure(options =>
        {
            options.NullableInstances = true;
        });

        var result = connection
            .Read<TestClass1?, TestClass3?>(@"select * from (
                values
                    (1, 'a1', 11, 'b1'),
                    (2, 'a2', null, null),
                    (3, 'a3', 33, 'b3'),
                    (4, 'a4', null, null)
            ) t(foo1, bar1, foo2, bar2)")
            .ToList();

        Assert.Equal(1, result[0].Item1?.Foo1);
        Assert.Equal("a1", result[0].Item1?.Bar1);
        Assert.Equal(11, result[0].Item2?.Foo2);
        Assert.Equal("b1", result[0].Item2?.Bar2);

        Assert.Equal(2, result[1].Item1?.Foo1);
        Assert.Equal("a2", result[1].Item1?.Bar1);
        Assert.Null(result[1].Item2);

        Assert.Equal(3, result[2].Item1?.Foo1);
        Assert.Equal("a3", result[2].Item1?.Bar1);
        Assert.Equal(33, result[2].Item2?.Foo2);
        Assert.Equal("b3", result[2].Item2?.Bar2);

        Assert.Equal(4, result[3].Item1?.Foo1);
        Assert.Equal("a4", result[3].Item1?.Bar1);
        Assert.Null(result[3].Item2);
    }
}