namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_Parameters_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
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
            .WithCommentHeader(comment: null, includeCommandAttributes: false, includeParameters: true, includeCallerInfo: false, includeTimestamp: false)
            .WithCommandCallback(c => actual = c.CommandText)
            .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
            .Execute("select @1, @2, @3, @4");

        Assert.Equal(string.Join("\n", expected), actual);

        var expected2 = new string[]
        {
            "/*",
        "@1 int = 2",
        "@2 nvarchar = \"bar\"",
        "@3 bit = false",
        "@4 datetime = \"1977-05-19T00:00:00.0000000\"",
        "*/",
        "select @1, @2, @3, @4"
        };

        connection
            .WithCommentParameters()
            .WithCommandCallback(c => actual = c.CommandText)
            .WithParameters(2, "bar", false, new DateTime(1977, 5, 19))
            .Execute("select @1, @2, @3, @4");

        Assert.Equal(string.Join("\n", expected2), actual);
    }
}