using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class SingleUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public SingleUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Single_Without_Parameters_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Single(
                "select 1, 'foo' as bar, cast('1977-05-19' as date) as day, null as \"null\""
            ).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var value = connection.Single("values (null)").Select(t => t.value).First();
            Assert.Null(value);
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var value = (await connection.SingleAsync("values (null)")).Select(t => t.value).First();
            Assert.Null(value);
        }

        [Fact]
        public void Single_With_Positional_Parameters_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Single(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                1, "foo", new DateTime(1977, 5, 19)).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public void Single_With_Named_Parameters_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Single(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                ("3", new DateTime(1977, 5, 19)), ("2", "foo"), ("1", 1)).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_Without_Parameters_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = (await connection.SingleAsync(
                "select 1, 'foo' as bar, cast('1977-05-19' as date) as day, null as \"null\"")).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_With_Positional_Parameters_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = (await connection.SingleAsync(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                1, "foo", new DateTime(1977, 5, 19))).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_With_Named_Parameters_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = (await connection.SingleAsync(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                ("3", new DateTime(1977, 5, 19)), ("2", "foo"), ("1", 1))).ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public void Single_No_Result_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var tuple = connection.Single(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar
                    ) as sub
                    where first = 2
                    "
                );

            Assert.Empty(tuple);
        }

        [Fact]
        public void Single_No_Result_Generic_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (first, bar) = connection.Single<int, string>(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar
                    ) as sub
                    where first = 2
                    "
                );

            Assert.Equal(default, first);
            Assert.Equal(default, bar);
        }
    }
}
