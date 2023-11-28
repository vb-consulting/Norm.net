using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests;

[Collection("PostgreSqlDatabase")]
public class RowsAffectedUnitTests
{
    private readonly PostgreSqlFixture fixture;

    public RowsAffectedUnitTests(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Execute_Format_Test()
    {
        using var connection = new NpgsqlConnection(fixture.ConnectionString);
        connection.Execute("create temp table rows_affected_test (t text)");

        var rowsAffected = connection
            .Execute("insert into rows_affected_test values ('foo')")
            .GetRecordsAffected();

        Assert.Equal(1, rowsAffected);

        var inst = connection.Norm();
        inst.Read("select * from rows_affected_test").ToList();
        rowsAffected = inst.GetRecordsAffected();

        Assert.Equal(-1, rowsAffected);
    }
}
