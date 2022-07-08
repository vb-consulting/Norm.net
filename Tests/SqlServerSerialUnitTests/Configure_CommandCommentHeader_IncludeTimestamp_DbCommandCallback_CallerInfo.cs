namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_IncludeTimestamp_DbCommandCallback_CallerInfo_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "Sql Text Command. Timeout: 30 seconds.",
        "at Configure_CommandCommentHeader_IncludeTimestamp_DbCommandCallback_CallerInfo_Test",
        $"Timestamp: {DateTime.Now.ToString("o")[..11]}",
        "*/",
        "select 1"
        };
        string? actual = null;
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);
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

        var actualLines = actual?.Split("\n");
        Assert.Equal(expected?.Length, actualLines?.Length);
        Assert.Equal(actualLines?[1], expected?[1]);
        Assert.StartsWith(expected?[2], actualLines?[2]);
        Assert.StartsWith(expected?[3], actualLines?[3]);
        Assert.Equal(expected?[4], actualLines?[4]);
    }
}