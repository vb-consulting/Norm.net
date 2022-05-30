namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void ChangeLogExample1()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
        "-- My foo bar query",
        "-- Sql Text Command. Timeout: 30 seconds.",
        "-- at ChangeLogExample1 in ",
        $"-- Timestamp: {DateTime.Now.ToString("o")[..11]}",
        "-- @foo nvarchar = \"foo value\"",
        "-- @bar nvarchar = \"bar value\"",
        "select @foo, @bar"
        };
        string actual = "";
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);

        var (foo, bar) = connection
            .WithCommentHeader(comment: "My foo bar query", includeCommandAttributes: true, includeParameters: true, includeCallerInfo: true, includeTimestamp: true)
            .WithCommandCallback(c => actual = c.CommandText)
            .WithParameters(new { foo = "foo value", bar = "bar value" })
            .Read<string, string>("select @foo, @bar")
            .Single();

        var actualLines = actual.Split(Environment.NewLine);

        Assert.Equal("foo value", foo);
        Assert.Equal("bar value", bar);

        Assert.Equal(7, actualLines.Length);
        Assert.Equal(expected.Length, actualLines?.Length);
        Assert.Equal(expected[0], actualLines?[0]);
        Assert.Equal(expected[1], actualLines?[1]);

        Assert.StartsWith(expected[2], actualLines?[2]);
        Assert.StartsWith(expected[3], actualLines?[3]);

        Assert.Equal(expected[4], actualLines?[4]);
        Assert.Equal(expected[5], actualLines?[5]);
        Assert.Equal(expected[6], actualLines?[6]);
    }
}