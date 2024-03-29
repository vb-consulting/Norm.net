using System;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Xunit;

namespace SQLiteUnitTests
{
    [Collection("SQLiteDatabase")]
    public class ExecuteUnitTests
    {
        private readonly SqLiteFixture fixture;

        public ExecuteUnitTests(SqLiteFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Execute_Without_Parameters_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString)
                .Execute("begin")
                .Execute("create table test (t text)")
                .Execute("insert into test values ('foo')");

            var result = connection
                .Read("select * from test")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal("foo", result.Values.First());

            connection.Execute("rollback");
            var tableMissing = false;
            try
            {
                connection.Read("select * from test").Single();
            }
            catch (SQLiteException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }


        [Fact]
        public void Execute_With_Positional_Parameters_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection
                .Execute("begin")
                .Execute("create table test (i int, t text, d date)")
                .WithParameters(1, "foo", new DateTime(1977, 5, 19))
                .Execute("insert into test values (@i, @t, @d)")
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
            catch (SQLiteException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }


        [Fact]
        public void Execute_With_Named_Parameters_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection
                .Execute("begin")
                .Execute("create table test (i int, t text, d date)")
                .WithParameters(new
                {
                    d = new DateTime(1977, 5, 19),
                    t = "foo",
                    i = 1
                })
                .Execute("insert into test values (@i, @t, @d)")
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
            catch (SQLiteException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public async Task Execute_Without_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("begin");
            await connection.ExecuteAsync("create table test (t text)");
            await connection.ExecuteAsync("insert into test values ('foo')");
            var result = (await connection.ReadAsync("select * from test").SingleAsync())
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal("foo", result.Values.First());

            connection.Execute("rollback");
            var tableMissing = false;
            try
            {
                await connection.ReadAsync("select * from test").SingleAsync();
            }
            catch (SQLiteException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public async Task Execute_With_Positional_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("begin");
            await connection.ExecuteAsync("create table test (i int, t text, d date)");
            await connection
                .WithParameters(new
                {
                    d = new DateTime(1977, 5, 19),
                    t = "foo",
                    i = 1
                })
                .ExecuteAsync("insert into test values (@i, @t, @d)");
            var result = (await connection.ReadAsync("select * from test").SingleAsync())
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            await connection.ExecuteAsync("rollback");
            var tableMissing = false;
            try
            {
                await connection.ReadAsync("select * from test").SingleAsync();
            }
            catch (SQLiteException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }

        [Fact]
        public async Task Execute_With_Named_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("begin");
            await connection.ExecuteAsync("create table test (i int, t text, d date)");
            await connection
                .WithParameters(1, "foo", new DateTime(1977, 5, 19))
                .ExecuteAsync("insert into test values (@i, @t, @d)");

            var result = (await connection.ReadAsync("select * from test").SingleAsync()).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result["i"]);
            Assert.Equal("foo", result["t"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

            await connection.ExecuteAsync("rollback");
            var tableMissing = false;
            try
            {
                await connection.ReadAsync("select * from test").SingleAsync();
            }
            catch (SQLiteException)
            {
                tableMissing = true;
            }
            Assert.True(tableMissing);
        }
    }
}
