using NpgsqlTypes;

namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_CommandCommentHeader_IncludeParameters_Array_DbCommandCallback_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "Npgsql Text Command. Timeout: 30 seconds.",
        "$1  = {a, b, c}",
        "*/",
        "select $1"
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
            .WithParameters((new string[]{ "a", "b", "c" }, NpgsqlDbType.Text | NpgsqlDbType.Array))
            .Execute("select $1");

        Assert.Equal(string.Join("\n", expected), actual);
    }
}