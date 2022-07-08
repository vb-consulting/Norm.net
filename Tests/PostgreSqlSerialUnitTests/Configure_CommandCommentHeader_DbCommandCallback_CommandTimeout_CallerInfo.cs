namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_DbCommandCallback_CommandTimeout_CallerInfo_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "Npgsql Text Command. Timeout: 60 seconds.",
        "at Configure_CommandCommentHeader_DbCommandCallback_CommandTimeout_CallerInfo_Test",
        "*/",
        "select 1"
        };
        string? actual = null;
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
        NormOptions.Configure(options =>
        {
            options.CommandTimeout = 60;
            options.CommandCommentHeader.Enabled = true;
            options.DbCommandCallback = cmd =>
            {
                actual = cmd.CommandText;
            };
        });

        connection.Execute("select 1");

        var actualLines = actual?.Split("\n");
        Assert.Equal(expected?.Length, actualLines?.Length);
        Assert.Equal(actualLines?[1], expected?[1]);
        Assert.StartsWith(expected?[2], actualLines?[2]);
        Assert.Equal(expected?[3], actualLines?[3]);
    }
}