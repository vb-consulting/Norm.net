using System;
using System.Linq;
using System.Threading.Tasks;
using Norm.Extensions;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class ExecuteUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public ExecuteUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Execute_Without_Parameters_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString)
                .Execute("begin")
                .Execute("create table test (t text)")
                .Execute("insert into test values ('foo')");

            var result = connection
                .Single("select * from test")
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal("foo", result.Values.First());

            connection.Execute("rollback");
            var tableMissing = false;
            try
            {
                connection.Single("select * from test");
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public void Execute_With_Positional_Parameters_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Execute("begin")
                .Execute("create table test (i int, t text, d date)")
                .Execute("insert into test values (@i, @t, @d)",
                    1, "foo", new DateTime(1977, 5, 19))
                .Single("select * from test")
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            connection.Execute("rollback");
            var tableMissing = false;
            try
            {
                connection.Single("select * from test");
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public void Execute_With_Named_Parameters_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Execute("begin")
                .Execute("create table test (i int, t text, d date)")
                .Execute("insert into test values (@i, @t, @d)", ("d", new DateTime(1977, 5, 19)), ("t", "foo"), ("i", 1))
                .Single("select * from test")
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            connection.Execute("rollback");
            var tableMissing = false;
            try
            {
                connection.Single("select * from test");
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public async Task Execute_Without_Parameters_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("begin");
            await connection.ExecuteAsync("create table test (t text)");
            await connection.ExecuteAsync("insert into test values ('foo')");
            var result = (await connection.SingleAsync("select * from test")).ToDictionary(t => t.name, t => t.value);

            Assert.Equal("foo", result.Values.First());

            connection.Execute("rollback");
            var tableMissing = false;
            try
            {
                await connection.SingleAsync("select * from test");
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public async Task Execute_With_Positional_Parameters_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("begin");
            await connection.ExecuteAsync("create table test (i int, t text, d date)");
            await connection.ExecuteAsync("insert into test values (@i, @t, @d)",
                ("d", new DateTime(1977, 5, 19)), ("t", "foo"), ("i", 1));
            var result = (await connection.SingleAsync("select * from test")).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            await connection.ExecuteAsync("rollback");
            var tableMissing = false;
            try
            {
                await connection.SingleAsync("select * from test");
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public async Task Execute_With_Named_Parameters_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("begin");
            await connection.ExecuteAsync("create table test (i int, t text, d date)");
            await connection.ExecuteAsync("insert into test values (@i, @t, @d)",
                1, "foo", new DateTime(1977, 5, 19));

            var result = (await connection.SingleAsync("select * from test")).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            await connection.ExecuteAsync("rollback");
            var tableMissing = false;
            try
            {
                await connection.SingleAsync("select * from test");
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }
    }
}
