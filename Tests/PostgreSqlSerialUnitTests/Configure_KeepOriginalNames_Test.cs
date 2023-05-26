using Xunit.Sdk;

namespace PostgreSqlSerialUnitTests;

#nullable enable

public partial class PostgreSqlSerialUnitTest
{
    class FooBarClass
    {
        public string? Foo_Bar { get; set; }
        public string? FooBar { get; set; }
    }

    [Fact]
    public void Configure_KeepOriginalNames_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
        
        NormOptions.Configure(options =>
        {
            options.KeepOriginalNames = false;
        });

        var result = connection
            .Read<FooBarClass>("select 'foobar' as foo_bar")
            .Single();

        Assert.NotNull(result);
        Assert.Equal("foobar", result.FooBar);
        Assert.Null(result.Foo_Bar);

        NormOptions.Configure(options =>
        {
            options.KeepOriginalNames = true;
        });

        result = connection
            .Read<FooBarClass>("select 'foobar' as foo_bar")
            .Single();

        Assert.NotNull(result);
        Assert.Equal("foobar", result.Foo_Bar);
        Assert.Null(result.FooBar);
    }
}