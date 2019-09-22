using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NoOrm;
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
        public void Single_Without_Parameters_Test()
        {
            using (var connection = new SqlConnection(fixture.ConnectionString))
            {
                var result = connection.Single(
                    "select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as \"null\""
                    );

                Assert.Equal(1, result.Values.First());
                Assert.Equal("foo", result["bar"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
                Assert.Equal(DBNull.Value, result["null"]);
            }
        }

        [Fact]
        public void Single_With_Positional_Parameters_Test()
        {
            using (var connection = new SqlConnection(fixture.ConnectionString))
            {
                var result = connection.Single(
                    @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                    1, "foo", new DateTime(1977, 5, 19));

                Assert.Equal(1, result.Values.First());
                Assert.Equal("foo", result["bar"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
                Assert.Equal(DBNull.Value, result["null"]);
            }
        }

        [Fact]
        public void Single_With_Named_Parameters_Test()
        {
            using (var connection = new SqlConnection(fixture.ConnectionString))
            {
                var result = connection.Single(
                    @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                    ("3", new DateTime(1977, 5, 19)), ("2", "foo"), ("1", 1));

                Assert.Equal(1, result.Values.First());
                Assert.Equal("foo", result["bar"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
                Assert.Equal(DBNull.Value, result["null"]);
            }
        }

        [Fact]
        public async Task Single_Without_Parameters_Test_Async()
        {
            using (var connection = new SqlConnection(fixture.ConnectionString))
            {
                var result = await connection.SingleAsync(
                    "select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as \"null\""
                );

                Assert.Equal(1, result.Values.First());
                Assert.Equal("foo", result["bar"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
                Assert.Equal(DBNull.Value, result["null"]);
            }
        }

        [Fact]
        public async Task Single_With_Positional_Parameters_Test_Async()
        {
            using (var connection = new SqlConnection(fixture.ConnectionString))
            {
                var result = await connection.SingleAsync(
                    @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                    1, "foo", new DateTime(1977, 5, 19));

                Assert.Equal(1, result.Values.First());
                Assert.Equal("foo", result["bar"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
                Assert.Equal(DBNull.Value, result["null"]);
            }
        }

        [Fact]
        public async Task Single_With_Named_Parameters_Test_Async()
        {
            using (var connection = new SqlConnection(fixture.ConnectionString))
            {
                var result = await connection.SingleAsync(
                    @"
                    select *
                    from (
                        select 1 as first, 'foo' as bar, cast('1977-05-19' as date) as day, null as ""null""
                    ) as sub
                    where first = @1 and bar = @2 and day = @3
                    ",
                    ("3", new DateTime(1977, 5, 19)), ("2", "foo"), ("1", 1));

                Assert.Equal(1, result.Values.First());
                Assert.Equal("foo", result["bar"]);
                Assert.Equal(new DateTime(1977, 5, 19), result["day"]);
                Assert.Equal(DBNull.Value, result["null"]);
            }
        }
    }
}
