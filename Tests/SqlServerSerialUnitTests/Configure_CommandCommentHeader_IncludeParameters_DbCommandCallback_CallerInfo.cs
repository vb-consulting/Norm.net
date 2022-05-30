namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_IncludeParameters_DbCommandCallback_CallerInfo_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
        "-- Sql Text Command. Timeout: 30 seconds.",
        "-- at Configure_CommandCommentHeader_IncludeParameters_DbCommandCallback_CallerInfo_Test",
        "-- @1 int = 1",
        "-- @2 nvarchar = \"foo\"",
        "-- @3 bit = false",
        "-- @4 datetime = \"2022-05-19T00:00:00.0000000\"",
        "select @1, @2, @3, @4"
        };

        string? actual = null;
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);
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


        var actualLines = actual?.Split(Environment.NewLine);
        Assert.Equal(expected?.Length, actualLines?.Length);
        Assert.Equal(actualLines?[0], expected?[0]);
        Assert.StartsWith(expected?[1], actualLines?[1]);
        Assert.Equal(expected?[2], actualLines?[2]);
        Assert.Equal(expected?[3], actualLines?[3]);
        Assert.Equal(expected?[4], actualLines?[4]);
        Assert.Equal(expected?[5], actualLines?[5]);
        Assert.Equal(expected?[6], actualLines?[6]);
    }
}