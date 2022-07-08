namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void ChangeLogExample1()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
        "/*",
        "My foo bar query",
        "Npgsql Text Command. Timeout: 30 seconds.",
        "at ChangeLogExample1 in ",
        $"Timestamp: {DateTime.Now.ToString("o")[..11]}",
        "@foo text = \"foo value\"",
        "@bar text = \"bar value\"",
        "*/",
        "select @foo, @bar",
        };
        string actual = "";
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        var (foo, bar) = connection
            .WithCommentHeader(comment: "My foo bar query", includeCommandAttributes: true, includeParameters: true, includeCallerInfo: true, includeTimestamp: true)
            .WithCommandCallback(c => actual = c.CommandText)
            .WithParameters(new { foo = "foo value", bar = "bar value" })
            .Read<string, string>("select @foo, @bar")
            .Single();

        var actualLines = actual.Split("\n");

        Assert.Equal("foo value", foo);
        Assert.Equal("bar value", bar);

        Assert.Equal(9, actualLines.Length);
        Assert.Equal(expected.Length, actualLines?.Length);
        Assert.Equal(expected[1], actualLines?[1]);
        Assert.Equal(expected[2], actualLines?[2]);

        Assert.StartsWith(expected[3], actualLines?[3]);
        Assert.StartsWith(expected[4], actualLines?[4]);

        Assert.Equal(expected[5], actualLines?[5]);
        Assert.Equal(expected[6], actualLines?[6]);
        Assert.Equal(expected[7], actualLines?[7]);
    }
}