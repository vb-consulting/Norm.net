namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_DbCommandCallback_CallerInfo_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "-- Sql Text Command. Timeout: 30 seconds.",
            "-- at Configure_CommandCommentHeader_DbCommandCallback_CallerInfo_Test",
            "select 1"
        };
        string? actual = null;
        int? timeout = null;
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);
        NormOptions.Configure(options =>
        {
            options.CommandCommentHeader.Enabled = true;
            options.DbCommandCallback = cmd =>
            {
                actual = cmd.CommandText;
                timeout = cmd.CommandTimeout;
            };
        });

        connection.Execute("select 1");

        var actualLines = actual?.Split(Environment.NewLine);
        Assert.Equal(expected?.Length, actualLines?.Length);
        Assert.Equal(actualLines?[0], expected?[0]);
        Assert.StartsWith(expected?[1], actualLines?[1]);
        Assert.Equal(expected?[2], actualLines?[2]);
    }
}