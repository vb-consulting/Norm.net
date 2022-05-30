namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_Timestamp_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            $"-- Timestamp: {DateTime.Now.ToString("o")[..11]}",
            "select 1"
        };
        string actual = "";
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommentHeader(comment: null, includeCommandAttributes: false, includeParameters: false, includeCallerInfo: false, includeTimestamp: true)
            .WithCommandCallback(c => actual = c.CommandText)
            .Execute("select 1");

        var actualLines = actual.Split(Environment.NewLine);

        Assert.Equal(2, actualLines.Length);
        Assert.Equal(expected.Length, actualLines?.Length);
        Assert.StartsWith(expected[0], actualLines?[0]);
        Assert.Equal(expected[1], actualLines?[1]);
    }
}