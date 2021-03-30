using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class FormattableUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public FormattableUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Execute_Format_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Execute("begin")
                .Execute("create table test (i int, t text, d date)")
                .ExecuteFormat($"insert into test values ({1}, {"foo"}, {new DateTime(1977, 5, 19)})")
                .Read("select * from test")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            connection.Execute("rollback");
            var tableMissing = false;
            try
            {
                connection.Read("select * from test").Single();
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public void Execute_Format_With_Sql_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Execute("begin")
                .Execute("create table test (i int, t text, d date)")
                .ExecuteFormat($"insert into test values ({new NpgsqlParameter("", 1)}, {new NpgsqlParameter("", "foo")}, {new NpgsqlParameter("", new DateTime(1977, 5, 19))})")
                .Read("select * from test")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            connection.Execute("rollback");
            var tableMissing = false;
            try
            {
                connection.Read("select * from test").Single();
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public void Execute_Format_Mixed_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Execute("begin")
                .Execute("create table test (i int, t text, d date)")
                .ExecuteFormat($"insert into test values ({1}, {new NpgsqlParameter("", "foo")}, {new DateTime(1977, 5, 19)})")
                .Read("select * from test")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            connection.Execute("rollback");
            var tableMissing = false;
            try
            {
                connection.Read("select * from test").Single();
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        public async Task Execute_Format_Test_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("begin");
            await connection.ExecuteAsync("create table test (i int, t text, d date)");
            
            await connection.ExecuteFormatAsync($"insert into test values ({1}, {"foo"}, {new DateTime(1977, 5, 19)})");

            var result = (await connection.ReadAsync("select * from test").SingleAsync()).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            await connection.ExecuteAsync("rollback");

            var tableMissing = false;
            try
            {
                connection.Read("select * from test").Single();
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public async Task Execute_Format_With_Sql_Params_Test_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("begin");
            await connection.ExecuteAsync("create table test (i int, t text, d date)");

            await connection.ExecuteFormatAsync($"insert into test values ({new NpgsqlParameter("", 1)}, {new NpgsqlParameter("", "foo")}, {new NpgsqlParameter("", new DateTime(1977, 5, 19))})");

            var result = (await connection.ReadAsync("select * from test").SingleAsync()).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            await connection.ExecuteAsync("rollback");

            var tableMissing = false;
            try
            {
                connection.Read("select * from test").Single();
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public async Task Execute_Format_Mixed_Params_Test_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("begin");
            await connection.ExecuteAsync("create table test (i int, t text, d date)");

            await connection.ExecuteFormatAsync($"insert into test values ({1}, {new NpgsqlParameter("", "foo")}, {new DateTime(1977, 5, 19)})");

            var result = (await connection.ReadAsync("select * from test").SingleAsync()).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            await connection.ExecuteAsync("rollback");

            var tableMissing = false;
            try
            {
                connection.Read("select * from test").Single();
            }
            catch (PostgresException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        record Record1(int Id1, string Foo1, string Bar1);
        record Record2(int Id2, string Foo2, string Bar2);

        [Fact]
        public void Multiple_Format_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            using var multiple = connection.MultipleFormat(@$"
            select {1} as id1, 'foo1' as foo1, 'bar1' as bar1; 
            select 2 as id2, 'foo2' as foo2, {"bar2"} as bar2");

            var result1 = multiple.Read<Record1>().Single();
            var next1 = multiple.Next();
            var result2 = multiple.Read<Record2>().Single();
            var next2 = multiple.Next();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public async Task Multiple_Format_Test_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            using var multiple = await connection.MultipleFormatAsync(@$"
            select {1} as id1, 'foo1' as foo1, 'bar1' as bar1; 
            select 2 as id2, 'foo2' as foo2, {"bar2"} as bar2");

            var result1 = await multiple.ReadAsync<Record1>().SingleAsync();
            var next1 = await multiple.NextAsync();
            var result2 = await multiple.ReadAsync<Record2>().SingleAsync();
            var next2 = await multiple.NextAsync();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }
    }
}
