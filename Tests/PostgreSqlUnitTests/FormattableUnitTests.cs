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

            connection
                .Execute("begin")
                .Execute("create table test (i int, t text, d date)")
                .ExecuteFormat($"insert into test values ({1}, {new NpgsqlParameter("", "foo")}, {new DateTime(1977, 5, 19)})");

            var result = connection
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

        [Fact]
        public void Read_Format_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            
            Assert.Equal(new (string name, object value)[]{("1", 1), ("2", 2)}, connection.ReadFormat($"select {1} as \"1\", {2} as \"2\"").Single());
            Assert.Equal(1, connection.ReadFormat<int>($"select {1}").Single());
            Assert.Equal((1, 2), connection.ReadFormat<int, int>($"select {1}, {2}").Single());
            Assert.Equal((1, 2, 3), connection.ReadFormat<int, int, int>($"select {1}, {2}, {3}").Single());
            Assert.Equal((1, 2, 3, 4), connection.ReadFormat<int, int, int, int>($"select {1}, {2}, {3}, {4}").Single());
            Assert.Equal((1, 2, 3, 4, 5), connection.ReadFormat<int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}").Single());
            Assert.Equal((1, 2, 3, 4, 5, 6), connection.ReadFormat<int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}").Single());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7), connection.ReadFormat<int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}").Single());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), connection.ReadFormat<int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}").Single());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8, 9), connection.ReadFormat<int, int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}").Single());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8, 9, 10), connection.ReadFormat<int, int, int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}").Single());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11), connection.ReadFormat<int, int, int, int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}").Single());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12), connection.ReadFormat<int, int, int, int, int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}").Single());
        }

        [Fact]
        public async Task Read_Format_Test_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            Assert.Equal(new (string name, object value)[] { ("1", 1), ("2", 2) }, await connection.ReadFormatAsync($"select {1} as \"1\", {2} as \"2\"").SingleAsync());
            Assert.Equal(1, await connection.ReadFormatAsync<int>($"select {1}").SingleAsync());
            Assert.Equal((1, 2), await connection.ReadFormatAsync<int, int>($"select {1}, {2}").SingleAsync());
            Assert.Equal((1, 2, 3), await connection.ReadFormatAsync<int, int, int>($"select {1}, {2}, {3}").SingleAsync());
            Assert.Equal((1, 2, 3, 4), await connection.ReadFormatAsync<int, int, int, int>($"select {1}, {2}, {3}, {4}").SingleAsync());
            Assert.Equal((1, 2, 3, 4, 5), await connection.ReadFormatAsync<int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}").SingleAsync());
            Assert.Equal((1, 2, 3, 4, 5, 6), await connection.ReadFormatAsync<int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}").SingleAsync());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7), await connection.ReadFormatAsync<int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}").SingleAsync());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), await connection.ReadFormatAsync<int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}").SingleAsync());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8, 9), await connection.ReadFormatAsync<int, int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}").SingleAsync());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8, 9, 10), await connection.ReadFormatAsync<int, int, int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}").SingleAsync());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11), await connection.ReadFormatAsync<int, int, int, int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}").SingleAsync());
            Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12), await connection.ReadFormatAsync<int, int, int, int, int, int, int, int, int, int, int, int>($"select {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}").SingleAsync());
        }


        [Fact]
        public void Raw_Parameters_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var (s, i, b, d, @null) = connection.ReadFormat<string, int, bool, DateTime, string>(
                $"{"select":raw} {"str"}, {999}, {true}, {new DateTime(1977, 5, 19)}, {null}").Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public async Task Raw_Parameters_Test_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var (s, i, b, d, @null) = await connection.ReadFormatAsync<string, int, bool, DateTime, string>(
                $"{"select":raw} {"str"}, {999}, {true}, {new DateTime(1977, 5, 19)}, {null}").SingleAsync();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }
    }
}
