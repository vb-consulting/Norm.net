using Microsoft.VisualBasic;

namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    private class NameParserTest
    {
        public string? Foo { get; set; }
        public string? Bar { get; set; }
        public string? FooBar { get; set; }
    }

    [Fact]
    public void WithNameParserCallback_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        var r1 = connection.Read<NameParserTest>("select 'foo' as foo, 'bar' as bar").Single();
        Assert.Equal("foo", r1.Foo);
        Assert.Equal("bar", r1.Bar);
        Assert.Equal(default, r1.FooBar);

        NormOptions.Configure(o =>
        {
            o.NameParserCallback = arg => arg.Name switch
            {
                "foo" => "FooBar",
                _ => arg.Name
            };
        });

        var r2 = connection.Read<NameParserTest>("select 'foo' as foo, 'bar' as bar").Single();
        Assert.Equal(default, r2.Foo);
        Assert.Equal("bar", r2.Bar);
        Assert.Equal("foo", r2.FooBar);

        NormOptions.Configure(o =>
        {
            o.NameParserCallback = arg => arg.Ordinal switch
            {
                0 => "FooBar",
                _ => arg.Name
            };
        });

        var r3 = connection.Read<NameParserTest>("select 'foo' as foo, 'bar' as bar").Single();
        Assert.Equal(default, r3.Foo);
        Assert.Equal("bar", r3.Bar);
        Assert.Equal("foo", r3.FooBar);

        NormOptions.Configure(o =>
        {
            o.NameParserCallback = arg =>
                arg.Name.StartsWith("prefix_") ? arg.Name[7..] : arg.Name;
        });

        var r4 = connection.Read<NameParserTest>(
            "select 'foo' as prefix_foo, 'bar' as prefix_bar, 'foobar' as foobar").Single();
        Assert.Equal("foo", r4.Foo);
        Assert.Equal("bar", r4.Bar);
        Assert.Equal("foobar", r4.FooBar);
    }
}
