namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_IncludeParameters_Positional_DbCommandCallback_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "Npgsql Text Command. Timeout: 30 seconds.",
        "$1 integer = 1",
        "$2 text = \"foo\"",
        "$3 boolean = false",
        "$4 timestamp = \"2022-05-19T00:00:00.0000000\"",
        "*/",
        "select $1, $2, $3, $4"
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
            .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
            .Execute("select $1, $2, $3, $4");
        Assert.Equal(string.Join("\n", expected), actual);
    }
}