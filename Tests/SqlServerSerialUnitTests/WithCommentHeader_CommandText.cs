
namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void WithCommentHeader_CommandText_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        string expected = "select 1";
        string? actual = null;
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommandCallback(cmd => actual = cmd.CommandText)
            .Execute("select 1");

        Assert.Equal(expected, actual);
    }
}
