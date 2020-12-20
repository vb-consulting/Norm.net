using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class PreparedStatementsUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public PreparedStatementsUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void SimpleStatement_Test()
        {
            var statement = "select 1, 2, 3";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection.Prepared().Read(statement).Single();

            var result = connection.Read<string, uint[], bool>(
                $"select statement, parameter_types, from_sql from pg_prepared_statements where statement = '{statement}'").ToArray();

            Assert.Single(result);
            Assert.Equal(statement, result[0].Item1);
            Assert.Empty(result[0].Item2);
            Assert.False(result[0].Item3);

            connection.Prepared().Read(statement).Single();

            result = connection.Read<string, uint[], bool>(
                $"select statement, parameter_types, from_sql from pg_prepared_statements where statement = '{statement}'").ToArray();

            Assert.Single(result);
            Assert.Equal(statement, result[0].Item1);
            Assert.Empty(result[0].Item2);
            Assert.False(result[0].Item3);
        }

        [Fact]
        public void StatementWithPositionalParams_Test()
        {
            var statement = "select @p1, @p2, @p3";
            var expected = "select $1, $2, $3";

            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection.Prepared().Read(statement, 1, 2, 3).Single();

            var result = connection.Read<string, uint[], bool>(
                $"select statement, parameter_types, from_sql from pg_prepared_statements where statement = '{expected}'").ToArray();

            Assert.Single(result);
            Assert.Equal(expected, result[0].Item1);
            Assert.Equal(3, result[0].Item2.Length);
            Assert.False(result[0].Item3);
        }

        [Fact]
        public void StatementWithNamedParams_Test()
        {
            var statement = "select * from (values (@p1, @p2, @p3)) t (t1, t2, t3)";
            var expected = "select * from (values ($1, $2, $3)) t (t1, t2, t3)";

            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection.Prepared().Read(statement, ("p1", 1), ("p2", 2), ("p3", 2)).Single();

            var result = connection.Read<string, uint[], bool>(
                $"select statement, parameter_types, from_sql from pg_prepared_statements where statement = '{expected}'").ToArray();

            Assert.Single(result);
            Assert.Equal(expected, result[0].Item1);
            Assert.Equal(3, result[0].Item2.Length);
            Assert.False(result[0].Item3);
        }
    }
}
