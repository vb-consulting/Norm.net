namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_IncludeParameters_Text_Overflow_DbCommandCallback_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "Npgsql Text Command. Timeout: 30 seconds.",
        "$1 text = \"foo",
        "bar\"",
        "$2 text = \"foo\r",
        "bar\"",
        "*/",
        "select $1, $2"
        };

        string? actual = null;
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
        NormOptions.Configure(options =>
        {
            options.CommandCommentHeader.Enabled = true;
            options.CommandCommentHeader.IncludeCallerInfo = false;
            options.DbCommandCallback = cmd =>
            {
                actual = cmd.CommandText;
            };
        });

        connection
            .WithParameters("foo\nbar", $"foo{Environment.NewLine}bar")
            .Execute("select $1, $2");

        Assert.Equal(string.Join("\n", expected), actual);
    }
}