namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_Prepared_WithHeaderComment_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var statement = "select 1";
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);
        NormOptions.Configure(options =>
        {
            options.CommandCommentHeader.Enabled = true;
            options.Prepared = true;
        });

        connection.Execute("select 1");

        var result = connection.Read<string, uint[], bool>(
    $"select statement, parameter_types, from_sql from pg_prepared_statements where statement = '{statement}'").ToArray();

        Assert.Single(result);
        Assert.Equal(statement, result[0].Item1);
        Assert.Empty(result[0].Item2);
        Assert.False(result[0].Item3);
    }
}