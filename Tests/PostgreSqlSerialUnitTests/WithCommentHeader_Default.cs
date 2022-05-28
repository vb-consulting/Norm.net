namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_Default_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
        "-- Npgsql Text Command. Timeout: 30 seconds.",
        "-- at WithCommentHeader_Default_Test in ",
        "select 1"
        };
        string actual = "";
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommentHeader()
            .WithCommandCallback(c => actual = c.CommandText)
            .Execute("select 1");

        var actualLines = actual.Split(Environment.NewLine);

        Assert.Equal(3, actualLines.Length);
        Assert.Equal(expected.Length, actualLines?.Length);
        Assert.Equal(expected[0], actualLines?[0]);
        Assert.StartsWith(expected[1], actualLines?[1]);
        //Assert.EndsWith(" 36", actualLines?[1]);
        Assert.Equal(expected[2], actualLines?[2]);

        connection
            .WithCommandCallback(c => actual = c.CommandText)
            .Execute("select 1");

        Assert.Equal("select 1", actual);
    }
}