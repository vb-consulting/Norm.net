namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void WithCommandCallback_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        bool withCommentHeaderCalled = false;
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommandCallback(cmd => withCommentHeaderCalled = true)
            .Execute("select 1");

        Assert.True(withCommentHeaderCalled);
    }
}
