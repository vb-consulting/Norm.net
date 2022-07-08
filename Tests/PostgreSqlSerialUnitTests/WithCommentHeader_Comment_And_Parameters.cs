namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_Comment_And_Parameters_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "This is my comment",
        "@1 integer = 1",
        "@2 text = \"foo\"",
        "@3 boolean = false",
        "@4 timestamp = \"2022-05-19T00:00:00.0000000\"",
        "*/",
        "select @1, @2, @3, @4"
        };
        string actual = "";
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommentParameters()
            .WithComment("This is my comment")
            .WithCommandCallback(c => actual = c.CommandText)
            .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
            .Execute("select @1, @2, @3, @4");

        Assert.Equal(string.Join("\n", expected), actual);
    }
}