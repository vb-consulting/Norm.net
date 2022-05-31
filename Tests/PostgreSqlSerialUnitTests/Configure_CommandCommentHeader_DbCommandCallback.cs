namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_DbCommandCallback_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "-- Npgsql Text Command. Timeout: 30 seconds.",
            "select 1"
        };
        string? actual = null;
        int? timeout = null;
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
        NormOptions.Configure(options =>
        {
            options.CommandCommentHeader.Enabled = true;
            options.CommandCommentHeader.IncludeCallerInfo = false;
            options.DbCommandCallback = cmd =>
            {
                actual = cmd.CommandText;
                timeout = cmd.CommandTimeout;
            };
        });

        connection.Execute("select 1");
        Assert.Equal(string.Join(Environment.NewLine, expected), actual);
        Assert.Equal(30, timeout); // the default

        NormOptions.Configure(options =>
        {
            options.CommandCommentHeader.Enabled = true;
            options.CommandCommentHeader.IncludeCallerInfo = false;
            options.DbCommandCallback = cmd =>
            {
                actual = cmd.CommandText;
                timeout = cmd.CommandTimeout;
            };
        });

    }
}