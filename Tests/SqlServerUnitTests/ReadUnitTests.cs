using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Norm.Extensions;
using Xunit;

namespace SqlServerUnitTests
{
    [Collection("SqlClientDatabase")]
    public class ReadUnitTests
    {
        private readonly SqlClientFixture fixture;

        public ReadUnitTests(SqlClientFixture fixture)
        {
            this.fixture = fixture;
        }

        private void AssertResult(IEnumerable<IDictionary<string, object>> result)
        {
            var list = result.ToList();
            Assert.Equal(3, list.Count);

            Assert.Equal(1, list[0].Values.First());
            Assert.Equal("foo1", list[0]["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), list[0]["day"]);

            Assert.Equal(2, list[1].Values.First());
            Assert.Equal("foo2", list[1]["bar"]);
            Assert.Equal(new DateTime(1978, 5, 19), list[1]["day"]);

            Assert.Equal(3, list[2].Values.First());
            Assert.Equal("foo3", list[2]["bar"]);
            Assert.Equal(new DateTime(1979, 5, 19), list[2]["day"]);
        }

        private async Task AssertResultAsync(IAsyncEnumerable<IDictionary<string, object>> result)
        {
            var list = await result.ToListAsync();
            AssertResult(list);
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var list = connection.Read("select null").SelectValues().ToList();
            var value = list.First().First();
            Assert.Equal(DBNull.Value, value);
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var list = (await connection.ReadAsync("select null").ToListAsync()).SelectValues();
            var value = list.First().First();
            Assert.Equal(DBNull.Value, value);
        }

        [Fact]
        public void Read_Without_Parameters_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Read(
                    @"
                          select * from (
                          values 
                            (1, 'foo1', cast('1977-05-19' as date)),
                            (2, 'foo2', cast('1978-05-19' as date)),
                            (3, 'foo3', cast('1979-05-19' as date))
                          ) t(first, bar, day)")
                .SelectDictionaries(); 

            AssertResult(result);
        }

        [Fact]
        public void Read_With_Positional_Parameters_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Read(
                @"
                            select * from(
                            values
                                (@1, @t1, @d1),
                                (@2, @t2, @d2),
                                (@3, @t3, @d3)
                            ) t(first, bar, day)",
                1, "foo1", new DateTime(1977, 5, 19),
                2, "foo2", new DateTime(1978, 5, 19),
                3, "foo3", new DateTime(1979, 5, 19));

            AssertResult(result.SelectDictionaries());
        }

        [Fact]
        public void Read_With_Named_Parameters_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Read(
                    @"
                          select * from (
                          values 
                            (@1, @t1, @d1),
                            (@2, @t2, @d2),
                            (@3, @t3, @d3)
                          ) t(first, bar, day)",
                    ("1", 1),
                    ("t1", "foo1"),
                    ("d1", new DateTime(1977, 5, 19)),
                    ("2", 2),
                    ("t2", "foo2"),
                    ("d2", new DateTime(1978, 5, 19)),
                    ("3", 3),
                    ("t3", "foo3"),
                    ("d3", new DateTime(1979, 5, 19)))
                .SelectDictionaries();

            AssertResult(result);
        }

        [Fact]
        public async Task Read_Without_Parameters_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.ReadAsync(
                @"
                          select * from (
                          values 
                            (1, 'foo1', cast('1977-05-19' as date)),
                            (2, 'foo2', cast('1978-05-19' as date)),
                            (3, 'foo3', cast('1979-05-19' as date))
                          ) t(first, bar, day)");

            await AssertResultAsync(result.SelectDictionaries());
        }

        [Fact]
        public async Task Read_With_Positional_Parameters_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.ReadAsync(
                @"
                            select * from(
                            values
                                (@1, @t1, @d1),
                                (@2, @t2, @d2),
                                (@3, @t3, @d3)
                            ) t(first, bar, day)",
                1, "foo1", new DateTime(1977, 5, 19),
                2, "foo2", new DateTime(1978, 5, 19),
                3, "foo3", new DateTime(1979, 5, 19));

            await AssertResultAsync(result.SelectDictionaries());
        }

        [Fact]
        public async Task Read_With_Named_Parameters_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.ReadAsync(
                @"
                          select * from (
                          values 
                            (@1, @t1, @d1),
                            (@2, @t2, @d2),
                            (@3, @t3, @d3)
                          ) t(first, bar, day)",
                ("1", 1),
                ("t1", "foo1"),
                ("d1", new DateTime(1977, 5, 19)),
                ("2", 2),
                ("t2", "foo2"),
                ("d2", new DateTime(1978, 5, 19)),
                ("3", 3),
                ("t3", "foo3"),
                ("d3", new DateTime(1979, 5, 19)));

            await AssertResultAsync(result.SelectDictionaries());
        }
    }
}
