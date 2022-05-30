namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_IncludeTimestamp_DbCommandCallback_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
        "-- Sql Text Command. Timeout: 30 seconds.",
        $"-- Timestamp: {DateTime.Now.ToString("o")[..11]}",
        "select 1"
        };
        string? actual = null;
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);
        NormOptions.Configure(options =>
        {
            options.CommandCommentHeader.Enabled = true;
            options.CommandCommentHeader.IncludeTimestamp = true;
            options.CommandCommentHeader.IncludeCallerInfo = false;
            options.DbCommandCallback = cmd =>
            {
                actual = cmd.CommandText;
            };
        });

        connection.Execute("select 1");

        var actualLines = actual?.Split(Environment.NewLine);
        Assert.Equal(3, actualLines?.Length);
        Assert.Equal(expected.Length, actualLines?.Length);
        Assert.Equal(expected[0], actualLines?[0]);
        Assert.StartsWith(expected?[1], actualLines?[1]);
        Assert.Equal(expected?[2], actualLines?[2]);
    }
}