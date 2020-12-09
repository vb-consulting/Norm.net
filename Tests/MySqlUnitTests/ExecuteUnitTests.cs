using System;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Norm;
using Xunit;

namespace MySqlUnitTests
{
    [Collection("MySqlDatabase")]
    public class ExecuteUnitTests
    {
        private readonly MySqlFixture fixture;

        public ExecuteUnitTests(MySqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Execute_Without_Parameters_Test()
        {
            var table = "execute_Without_parameters_test_sync";
            using var connection = new MySqlConnection(fixture.ConnectionString)
                .Execute($"create table {table} (t text)");

            try
            {
                connection
                    .Execute("start transaction")
                    .Execute($"insert into {table} values ('foo')");

                var result = connection
                    .Single($"select * from {table}")
                    .ToDictionary(t => t.name, t => t.value);

                Assert.Equal("foo", result.Values.First());

                connection.Execute("rollback");

                result = connection
                    .Single($"select * from {table}")
                    .ToDictionary(t => t.name, t => t.value);

                Assert.Empty(result.Values);
            }
            finally
            {
                 connection.Execute($"drop table {table}");
            }
        }

        [Fact]
        public void Execute_With_Positional_Parameters_Test()
        {
            var table = "execute_with_positional_parameters_test_sync";
            using var connection = new MySqlConnection(fixture.ConnectionString)
                .Execute($"create table {table} (i int, t text, d date)");

            try
            {
                var result = connection
                    .Execute("start transaction")
                    .Execute($"insert into {table} values (@i, @t, @d)",1, "foo", new DateTime(1977, 5, 19))
                    .Single($"select * from {table}")
                    .ToDictionary(t => t.name, t => t.value);

                Assert.Equal(1, result["i"]);
                Assert.Equal("foo", result["t"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

                connection.Execute("rollback");

                result = connection
                    .Single($"select * from {table}")
                    .ToDictionary(t => t.name, t => t.value);

                Assert.Empty(result.Values);
            }
            finally
            {
                connection.Execute($"drop table {table}");
            }
        }

        [Fact]
        public void Execute_With_Named_Parameters_Test()
        {
            var table = "execute_with_positional_parameters_test";
            using var connection = new MySqlConnection(fixture.ConnectionString)
                .Execute($"create table {table} (i int, t text, d date)");

            try
            {
                var result = connection
                    .Execute("start transaction")
                    .Execute($"insert into {table} values (@i, @t, @d)", ("d", new DateTime(1977, 5, 19)), ("t", "foo"), ("i", 1))
                    .Single($"select * from {table}")
                    .ToDictionary(t => t.name, t => t.value);

                Assert.Equal(1, result["i"]);
                Assert.Equal("foo", result["t"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

                connection.Execute("rollback");

                result = connection
                    .Single($"select * from {table}")
                    .ToDictionary(t => t.name, t => t.value);

                Assert.Empty(result.Values);
            }
            finally
            {
                connection.Execute($"drop table {table}");
            }
        }

        [Fact]
        public async Task Execute_Without_Parameters_Test_Async()
        {
            var table = "execute_Without_parameters_test_async";
            await using var connection = new MySqlConnection(fixture.ConnectionString);
            await connection.ExecuteAsync($"create table {table} (t text)");

            try
            {
                await connection.ExecuteAsync("start transaction");
                await connection.ExecuteAsync($"insert into {table} values ('foo')");

                var result = (await connection.SingleAsync($"select * from {table}")).ToDictionary(t => t.name, t => t.value);

                Assert.Equal("foo", result.Values.First());

                await connection.ExecuteAsync("rollback");

                result = (await connection.SingleAsync($"select * from {table}")).ToDictionary(t => t.name, t => t.value);

                Assert.Empty(result.Values);
            }
            finally
            {
                await connection.ExecuteAsync($"drop table {table}");
            }
        }

        [Fact]
        public async Task Execute_With_Named_Parameters_Test_Async()
        {
            var table = "execute_with_positional_parameters_test";
            await using var connection = new MySqlConnection(fixture.ConnectionString);
            await connection.ExecuteAsync($"create table {table} (i int, t text, d date)");

            try
            {
                await connection.ExecuteAsync("start transaction");
                await connection.ExecuteAsync($"insert into {table} values (@i, @t, @d)", ("d", new DateTime(1977, 5, 19)), ("t", "foo"), ("i", 1));
                var result = (await connection.SingleAsync($"select * from {table}")).ToDictionary(t => t.name, t => t.value);

                Assert.Equal(1, result["i"]);
                Assert.Equal("foo", result["t"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

                await connection.ExecuteAsync("rollback");

                result = (await connection.SingleAsync($"select * from {table}")).ToDictionary(t => t.name, t => t.value);

                Assert.Empty(result.Values);
            }
            finally
            {
                await connection.ExecuteAsync($"drop table {table}");
            }
        }

        [Fact]
        public async Task Execute_With_Positional_Parameters_Test_Async()
        {
            var table = "execute_with_named_parameters_test_async";
            await using var connection = new MySqlConnection(fixture.ConnectionString);
            await connection.ExecuteAsync($"create table {table} (i int, t text, d date)");

            try
            {
                await connection.ExecuteAsync("start transaction");
                await connection.ExecuteAsync($"insert into {table} values (@i, @t, @d)", 1, "foo", new DateTime(1977, 5, 19));
                var result = (await connection.SingleAsync($"select * from {table}")).ToDictionary(t => t.name, t => t.value);

                Assert.Equal(1, result["i"]);
                Assert.Equal("foo", result["t"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["d"]);

                await connection.ExecuteAsync("rollback");

                result = (await connection.SingleAsync($"select * from {table}")).ToDictionary(t => t.name, t => t.value);

                Assert.Empty(result.Values);
            }
            finally
            {
                await connection.ExecuteAsync($"drop table {table}");
            }
        }
    }
}
