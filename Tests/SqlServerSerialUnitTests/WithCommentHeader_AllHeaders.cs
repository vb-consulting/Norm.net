namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_AllHeaders_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "This is my comment",
        "Sql Text Command. Timeout: 30 seconds.",
        "at WithCommentHeader_AllHeaders_Test in ",
        $"Timestamp: {DateTime.Now.ToString("o")[..11]}",
        "@1 int = 1",
        "@2 nvarchar = \"foo\"",
        "@3 bit = false",
        "@4 datetime = \"2022-05-19T00:00:00.0000000\"",
        "*/",
        "select @1, @2, @3, @4"
        };
        string actual = "";
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommentHeader(comment: "This is my comment", includeCommandAttributes: true, includeParameters: true, includeCallerInfo: true, includeTimestamp: true)
            .WithCommandCallback(c => actual = c.CommandText)
            .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
            .Execute("select @1, @2, @3, @4");

        var actualLines = actual.Split("\n");

        Assert.Equal(11, actualLines.Length);
        Assert.Equal(expected.Length, actualLines?.Length);
        Assert.Equal(expected[1], actualLines?[1]);
        Assert.Equal(expected[2], actualLines?[2]);

        Assert.StartsWith(expected[3], actualLines?[3]);
        Assert.StartsWith(expected[4], actualLines?[4]);

        Assert.Equal(expected[5], actualLines?[5]);
        Assert.Equal(expected[6], actualLines?[6]);
        Assert.Equal(expected[7], actualLines?[7]);
        Assert.Equal(expected[8], actualLines?[8]);
        Assert.Equal(expected[9], actualLines?[9]);
    }
}