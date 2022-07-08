namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_Comment_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "This is my comment",
        "*/",
        "select 1"
        };
        string actual = "";
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommentHeader("This is my comment", includeCommandAttributes: false, includeParameters: false, includeCallerInfo: false, includeTimestamp: false)
            .WithCommandCallback(c => actual = c.CommandText)
            .Execute("select 1");

        Assert.Equal(string.Join("\n", expected), actual);

        var expected2 = new string[]
        {
            "/*",
        "This is my second comment",
        "*/",
        "select 2"
        };

        connection
            .WithComment("This is my second comment")
            .WithCommandCallback(c => actual = c.CommandText)
            .Execute("select 2");

        Assert.Equal(string.Join("\n", expected2), actual);
    }
}