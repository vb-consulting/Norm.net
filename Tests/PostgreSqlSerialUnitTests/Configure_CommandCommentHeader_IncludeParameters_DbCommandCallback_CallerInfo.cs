namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_IncludeParameters_DbCommandCallback_CallerInfo_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "Npgsql Text Command. Timeout: 30 seconds.",
        "at Configure_CommandCommentHeader_IncludeParameters_DbCommandCallback_CallerInfo_Test",
        "@1 integer = 1",
        "@2 text = \"foo\"",
        "@3 boolean = false",
        "@4 timestamp = \"2022-05-19T00:00:00.0000000\"",
        "*/",
        "select @1, @2, @3, @4"
        };

        string? actual = null;
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
        NormOptions.Configure(options =>
        {
            options.CommandCommentHeader.Enabled = true;
            options.DbCommandCallback = cmd =>
            {
                actual = cmd.CommandText;
            };
        });

        connection
            .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
            .Execute("select @1, @2, @3, @4");


        var actualLines = actual?.Split("\n");
        Assert.Equal(expected?.Length, actualLines?.Length);
        Assert.Equal(actualLines?[1], expected?[1]);
        Assert.StartsWith(expected?[2], actualLines?[2]);
        Assert.Equal(expected?[3], actualLines?[3]);
        Assert.Equal(expected?[4], actualLines?[4]);
        Assert.Equal(expected?[5], actualLines?[5]);
        Assert.Equal(expected?[6], actualLines?[6]);
        Assert.Equal(expected?[7], actualLines?[7]);
    }
}