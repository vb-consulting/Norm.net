namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_IncludeTimestamp_DbCommandCallback_CallerInfo_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
        "-- Npgsql Text Command. Timeout: 30 seconds.",
        "-- at Configure_CommandCommentHeader_IncludeTimestamp_DbCommandCallback_CallerInfo_Test",
        $"-- Timestamp: {DateTime.Now.ToString("o")[..11]}",
        "select 1"
        };
        string? actual = null;
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
        NormOptions.Configure(options =>
        {
            options.CommandCommentHeader.Enabled = true;
            options.CommandCommentHeader.IncludeCallerInfo = true;
            options.CommandCommentHeader.IncludeTimestamp = true;
            options.DbCommandCallback = cmd =>
            {
                actual = cmd.CommandText;
            };
        });

        connection.Execute("select 1");

        var actualLines = actual?.Split(Environment.NewLine);
        Assert.Equal(expected?.Length, actualLines?.Length);
        Assert.Equal(actualLines?[0], expected?[0]);
        Assert.StartsWith(expected?[1], actualLines?[1]);
        Assert.StartsWith(expected?[2], actualLines?[2]);
        Assert.Equal(expected?[3], actualLines?[3]);
    }
}