using System;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Norm.Extensions;
using Xunit;

namespace SQLiteUnitTests
{
    [Collection("SQLiteDatabase")]
    public class SingleUnitTests
    {
        private readonly SqLiteFixture fixture;

        public SingleUnitTests(SqLiteFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Single_Without_Parameters_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Single(
                "select 1, 'foo' as bar, date('1977-05-19') as day, null as \"null\""
            ).ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var value = connection.Single("select null").Select(t => t.value).First();
            Assert.Null(value);
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var value = (await connection.SingleAsync("select null")).Select(t => t.value).First();
            Assert.Null(value);
        }

        [Fact]
        public void Single_With_Positional_Parameters_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Single(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, date('1977-05-19') as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                1, "foo", "1977-05-19").ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public void Single_With_Named_Parameters_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Single(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, date('1977-05-19') as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                ("3", "1977-05-19"), ("2", "foo"), ("1", 1)).ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_Without_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = (await connection.SingleAsync(
                "select 1, 'foo' as bar, date('1977-05-19') as day, null as \"null\"")).ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_With_Positional_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = (await connection.SingleAsync(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, date('1977-05-19') as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                1, "foo", "1977-05-19")).ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_With_Named_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = (await connection.SingleAsync(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, date('1977-05-19') as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                ("3", "1977-05-19"), ("2", "foo"), ("1", 1))).ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }
    }
}
