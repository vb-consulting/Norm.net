namespace SqlSerialUnitTests;

public partial class SqlSerialUnitTest
{
    [Fact]
    public void WithCommandCallback_CallCount_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        int callCount = 0;
        using var connection = new SqlConnection(_DatabaseFixture.ConnectionString);

        connection
            .WithCommandCallback(cmd => callCount++)
            .Execute("select 1");

        connection
            .WithCommandCallback(cmd => callCount++)
            .Execute("select 2");

        connection.Execute("select 3");

        connection.Execute("select 4");

        Assert.Equal(2, callCount);
    }
}
