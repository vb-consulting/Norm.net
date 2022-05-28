namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_DbCommandCallback_CommandTimeout_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
        "-- Npgsql Text Command. Timeout: 60 seconds.",
        "select 1"
        };
        string? actual = null;
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
        NormOptions.Configure(options =>
        {
            options.CommandTimeout = 60;
            options.CommandCommentHeader.Enabled = true;
            options.CommandCommentHeader.IncludeCallerInfo = false;
            options.DbCommandCallback = cmd =>
            {
                actual = cmd.CommandText;
            };
        });

        connection.Execute("select 1");
        Assert.Equal(string.Join(Environment.NewLine, expected), actual);
    }
}