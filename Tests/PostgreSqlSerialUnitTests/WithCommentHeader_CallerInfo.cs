namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_CallerInfo_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "at WithCommentHeader_CallerInfo_Test in ",
        "*/",
        "select 1"
        };
        string actual = "";
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommentHeader(comment: null, includeCommandAttributes: false, includeParameters: false, includeCallerInfo: true, includeTimestamp: false)
            .WithCommandCallback(c => actual = c.CommandText)
            .Execute("select 1");

        var actualLines = actual.Split("\n");

        Assert.Equal(4, actualLines.Length);
        Assert.Equal(expected.Length, actualLines?.Length);
        Assert.StartsWith(expected[1], actualLines?[1]);
        //Assert.EndsWith(" 184", actualLines?[0]);
        Assert.Equal(expected[2], actualLines?[2]);

        connection
            .WithCommentCallerInfo()
            .WithCommandCallback(c => actual = c.CommandText)
            .Execute("select 1");

        actualLines = actual.Split("\n");

        Assert.Equal(4, actualLines.Length);
        Assert.Equal(expected.Length, actualLines?.Length);
        Assert.StartsWith(expected[1], actualLines?[1]);
        //Assert.EndsWith(" 197", actualLines?[0]);
        Assert.Equal(expected[2], actualLines?[2]);
    }
}