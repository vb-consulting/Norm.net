
namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_CommandAttributes_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var expected = new string[]
        {
            "/*",
        "Npgsql Text Command. Timeout: 30 seconds.",
        "*/",
        "select 1"
        };
        string actual = "";
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommentHeader(comment: null, includeCommandAttributes: true, includeParameters: false, includeCallerInfo: false, includeTimestamp: false)
            .WithCommandCallback(c => actual = c.CommandText)
            .Execute("select 1");

        Assert.Equal(string.Join("\n", expected), actual);
    }
}