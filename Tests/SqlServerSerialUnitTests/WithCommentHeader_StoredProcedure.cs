namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_StoredProcedure_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "Sql StoredProcedure Command. Timeout: 30 seconds.",
        "at WithCommentHeader_StoredProcedure_Test in ",
        "@test_param nvarchar = \"foo\"",
        "*/",
        "comment_header_test_func"
        };
        string actual = "";
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);

        var result = connection
            .Execute(@"
            create procedure comment_header_test_func(@test_param nvarchar(max))
            as
            select @test_param + ' bar';
            ")
            .AsProcedure()
            .WithCommentHeader()
            .WithCommandCallback(c => actual = c.CommandText)
            .WithParameters(new { test_param = "foo" })
            .Read<string?>(@"comment_header_test_func")
            .FirstOrDefault();

        var actualLines = actual.Split("\n");

        Assert.Equal(6, actualLines.Length);
        Assert.Equal(expected.Length, actualLines?.Length);

        Assert.Equal(expected[1], actualLines?[1]);
        Assert.StartsWith(expected[2], actualLines?[2]);
        Assert.Equal(expected[3], actualLines?[3]);
        Assert.Equal(expected[4], actualLines?[4]);
    }
}