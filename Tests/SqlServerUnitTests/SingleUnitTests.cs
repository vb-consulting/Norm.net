using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Xunit;

namespace SqlServerUnitTests
{
    [Collection("SqlClientDatabase")]
    public class SingleUnitTests
    {
        private readonly SqlClientFixture fixture;

        public SingleUnitTests(SqlClientFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var value = connection.Read("select null").Single().Select(t => t.value).First();
            Assert.Null(value);
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var value = (await connection.ReadAsync("select null").SingleAsync()).Select(t => t.value).First();
            Assert.Null(value);
        }

        [Fact]
        public void Single_Without_Parameters_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Read("select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as \"null\"")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public void Single_With_Positional_Parameters_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection
                .WithParameters(1, "foo", new DateTime(1977, 5, 19))
                .Read(@"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public void Single_With_Named_Parameters_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection
                .WithParameters(new
                {
                    p3 = new DateTime(1977, 5, 19),
                    p2 = "foo",
                    p1 = 1
                })
                .Read(
                @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @p1 and bar = @p2 and day = @p3
                    ")
                .Single()
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_Without_Parameters_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = (await connection.ReadAsync("select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as \"null\"").SingleAsync())
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_With_Positional_Parameters_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = (await connection
                .WithParameters(1, "foo", new DateTime(1977, 5, 19))
                .ReadAsync(@"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ")
                .SingleAsync())
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }

        [Fact]
        public async Task Single_With_Named_Parameters_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = (await connection
                .WithParameters(new
                {
                    p3 = new DateTime(1977, 5, 19),
                    p2 = "foo",
                    p1 = 1
                })
                .ReadAsync(@"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @p1 and bar = @p2 and day = @p3
                    ")
                .SingleAsync())
                .ToDictionary(t => t.name, t => t.value);

            Assert.Equal(1, result.Values.First());
            Assert.Equal("foo", result["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
            Assert.Null(result["null"]);
        }
    }
}
