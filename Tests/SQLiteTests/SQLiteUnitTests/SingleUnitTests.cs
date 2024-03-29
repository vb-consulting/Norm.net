﻿using System;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Norm;
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
            var result = connection.Read("select 1, 'foo' as bar, date('1977-05-19') as day, null as \"null\"")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var value = connection.Read("select null").Single().Select(t => t.value).First();
            Assert.Null(value);
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var value = (await connection.ReadAsync("select null").SingleAsync()).Select(t => t.value).First();
            Assert.Null(value);
        }

        [Fact]
        public void Single_With_Positional_Parameters_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection
                .WithParameters(1, "foo", "1977-05-19")
                .Read(@"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, date('1977-05-19') as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public void Single_With_Named_Parameters_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection
                .WithParameters(new
                {
                    p3 = "1977-05-19",
                    p2 = "foo",
                    p1 = 1
                })
                .Read(@"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, date('1977-05-19') as day, null as ""null""
                    ) as sub
                    where first = @p1 and bar = @p2 and day = @p3
                    ")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_Without_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = (await connection.ReadAsync(
                "select 1, 'foo' as bar, date('1977-05-19') as day, null as \"null\"").SingleAsync()).ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_With_Positional_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = (await connection
                .WithParameters(1, "foo", "1977-05-19")
                .ReadAsync(@"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, date('1977-05-19') as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ").SingleAsync()).ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_With_Named_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = (await connection
                .WithParameters(new
                {
                    p3 = "1977-05-19",
                    p2 = "foo",
                    p1 = 1
                })
                .ReadAsync(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, date('1977-05-19') as day, null as ""null""
                    ) as sub
                    where first = @p1 and bar = @p2 and day = @p3
                    ")
                .SingleAsync())
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal((long)1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal("1977-05-19", result["day"]);
            Assert.Null(result["null"]);
        }
    }
}
