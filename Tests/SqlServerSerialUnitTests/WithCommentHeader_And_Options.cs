namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_And_Options_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
        "-- This is my comment",
        "-- Sql Text Command. Timeout: 30 seconds.",
        "-- at WithCommentHeader_And_Options_Test in ",
        $"-- Timestamp: {DateTime.Now.ToString("o")[..11]}",
        "-- @1 int = 1",
        "-- @2 nvarchar = \"foo\"",
        "-- @3 bit = false",
        "-- @4 datetime = \"2022-05-19T00:00:00.0000000\"",
        "select @1, @2, @3, @4"
        };
        string actual = "";
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);

        NormOptions.Configure(options =>
        {
            options.CommandCommentHeader.Enabled = true;
            options.CommandCommentHeader.IncludeCommandAttributes = true;
            options.CommandCommentHeader.IncludeTimestamp = true;
            options.DbCommandCallback = cmd => actual = cmd.CommandText;
        });

        connection
            .WithComment("This is my comment")
            .WithCommentCallerInfo()
            .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
            .Execute("select @1, @2, @3, @4");

        var actualLines = actual.Split(Environment.NewLine);

        Assert.Equal(9, actualLines.Length);
        Assert.Equal(expected.Length, actualLines?.Length);
        Assert.Equal(expected[0], actualLines?[0]);
        Assert.Equal(expected[1], actualLines?[1]);

        Assert.StartsWith(expected?[2], actualLines?[2]);
        Assert.StartsWith(expected?[3], actualLines?[3]);

        Assert.Equal(expected?[4], actualLines?[4]);
        Assert.Equal(expected?[5], actualLines?[5]);
        Assert.Equal(expected?[6], actualLines?[6]);
        Assert.Equal(expected?[7], actualLines?[7]);
        Assert.Equal(expected?[8], actualLines?[8]);
    }
}