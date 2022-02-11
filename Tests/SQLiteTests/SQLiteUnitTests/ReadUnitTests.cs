using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Xunit;

namespace SQLiteUnitTests
{
    [Collection("SQLiteDatabase")]
    public class ReadUnitTests
    {
        private readonly SqLiteFixture fixture;

        public ReadUnitTests(SqLiteFixture fixture)
        {
            this.fixture = fixture;
        }

        private void AssertResult(IEnumerable<IDictionary<string, object>> result)
        {
            var list = result.ToList();
            Assert.Equal(3, list.Count);

            Assert.Equal((long)1, list[0].Values.First());
            Assert.Equal("foo1", list[0]["bar"]);
            Assert.Equal("1977-05-19 00:00:00", list[0]["day"]);

            Assert.Equal((long)2, list[1].Values.First());
            Assert.Equal("foo2", list[1]["bar"]);
            Assert.Equal("1978-05-19 00:00:00", list[1]["day"]);

            Assert.Equal((long)3, list[2].Values.First());
            Assert.Equal("foo3", list[2]["bar"]);
            Assert.Equal("1979-05-19 00:00:00", list[2]["day"]);
        }

        private async Task AssertResultAsync(IAsyncEnumerable<IDictionary<string, object>> result)
        {
            var list = await result.ToListAsync();
            AssertResult(list);
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var list = connection.Read("select null").Select(tuples => tuples.Select(t => t.value)).ToList();
            var value = list.First().First();
            Assert.Null(value); ;
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var list = (await connection.ReadAsync("values (null)").ToListAsync()).Select(tuples => tuples.Select(t => t.value));
            var value = list.First().First();
            Assert.Null(value);
        }

       [Fact]
       public void Read_Without_Parameters_Test()
       {
           using var connection = new SQLiteConnection(fixture.ConnectionString);
           var result = connection.Read(@"
                with cte(first, bar, day) as (
                     select * from (
                            values
                                  (1, 'foo1', '1977-05-19 00:00:00'),
                                  (2, 'foo2', '1978-05-19 00:00:00'),
                                  (3, 'foo3', '1979-05-19 00:00:00')
                     )
                )
                select * from cte;")
               .Select(tuples => tuples.ToDictionary(t => t.name, t => t.value));

           AssertResult(result);
       }

       [Fact]
       public void Read_With_Positional_Parameters_Test()
       {
           using var connection = new SQLiteConnection(fixture.ConnectionString);
           var result = connection.Read(
               @"

                with cte(first, bar, day) as (
                     select * from (
                            values
                                (@1, @t1, @d1),
                                (@2, @t2, @d2),
                                (@3, @t3, @d3)
                     )
                )
                select * from cte",
               1, "foo1", new DateTime(1977, 5, 19),
               2, "foo2", new DateTime(1978, 5, 19),
               3, "foo3", new DateTime(1979, 5, 19));

           AssertResult(result.Select(tuples => tuples.ToDictionary(t => t.name, t => t.value)));
       }

       [Fact]
       public void Read_With_Named_Parameters_Test()
       {
           using var connection = new SQLiteConnection(fixture.ConnectionString);
           var result = connection.Read(
                @"
                          with cte(first, bar, day) as (
                             select * from (
                                    values
                                        (@p1, @t1, @d1),
                                        (@p2, @t2, @d2),
                                        (@p3, @t3, @d3)
                             )
                        )
                        select * from cte",
                new
                {
                    @p1 = 1,
                    t1 = "foo1",
                    d1 = new DateTime(1977, 5, 19),
                    @p2 = 2,
                    t2 = "foo2",
                    d2 = new DateTime(1978, 5, 19),
                    @p3 = 3,
                    t3 = "foo3",
                    d3 = new DateTime(1979, 5, 19)
                });

           AssertResult(result.Select(tuples => tuples.ToDictionary(t => t.name, t => t.value)));
        }

        [Fact]
        public async Task Read_Without_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.ReadAsync(
             @"
                     with cte(first, bar, day) as (
                     select * from (
                            values
                                  (1, 'foo1', '1977-05-19 00:00:00'),
                                  (2, 'foo2', '1978-05-19 00:00:00'),
                                  (3, 'foo3', '1979-05-19 00:00:00')
                     )
                )
                select * from cte;");

            await AssertResultAsync(result.Select(tuples => tuples.ToDictionary(t => t.name, t => t.value)));
        }

        [Fact]
        public async Task Read_With_Positional_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.ReadAsync(
            @"
                       with cte(first, bar, day) as (
                             select * from (
                                    values
                                        (@1, @t1, @d1),
                                        (@2, @t2, @d2),
                                        (@3, @t3, @d3)
                             )
                        )
                        select * from cte",
            1, "foo1", new DateTime(1977, 5, 19),
            2, "foo2", new DateTime(1978, 5, 19),
            3, "foo3", new DateTime(1979, 5, 19));

            await AssertResultAsync(result.Select(tuples => tuples.ToDictionary(t => t.name, t => t.value)));
        }


        [Fact]
        public async Task Read_With_Named_Parameters_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.ReadAsync(
            @"
                     with cte(first, bar, day) as (
                             select * from (
                                    values
                                        (@p1, @t1, @d1),
                                        (@p2, @t2, @d2),
                                        (@p3, @t3, @d3)
                             )
                        )
                        select * from cte",
            new
            {
                @p1 = 1,
                t1 = "foo1",
                d1 = new DateTime(1977, 5, 19),
                @p2 = 2,
                t2 = "foo2",
                d2 = new DateTime(1978, 5, 19),
                @p3 = 3,
                t3 = "foo3",
                d3 = new DateTime(1979, 5, 19)
            });

            await AssertResultAsync(result.Select(tuples => tuples.ToDictionary(t => t.name, t => t.value)));
        }
    }
}
