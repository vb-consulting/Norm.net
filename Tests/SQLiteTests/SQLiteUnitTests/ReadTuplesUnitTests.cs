using System;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Xunit;

namespace SQLiteUnitTests
{
    [Collection("SQLiteDatabase")]
    public class ReadTuplesUnitTests
    {
        private readonly SqLiteFixture fixture;

        private const string Query = @"
            with cte(id, foo, day, bool, bar) as (
                 select * from (
                        values
                              (1, 'foo1', '1977-05-19', true, null),
                              (2, 'foo2', '1978-05-19', false, 'bar2'),
                              (3, 'foo3', '1979-05-19', null, 'bar3')
                 )
            )
            select * from cte;";

        public ReadTuplesUnitTests(SqLiteFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Read<long?>("select null").ToList().First();
            Assert.Null(result);
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = (await connection.ReadAsync<long?>("select null").ToListAsync()).First();
            Assert.Null(result);
        }


        [Fact]
        public void Single_Value_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Read<long>(Query).ToList();
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
        }

        [Fact]
        public void Two_Tuples_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Read<long, string>(Query).ToList();
            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
        }


        [Fact]
        public async Task Single_Value_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<long>(Query).ToListAsync();
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
        }

        [Fact]
        public async Task Two_Tuples_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<long, string>(Query).ToListAsync();
            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
        }

        [Fact]
        public void Three_Tuples_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Read<long, string, string>(Query).ToList();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal("1977-05-19", result[0].Item3);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal("1978-05-19", result[1].Item3);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal("1979-05-19", result[2].Item3);
        }

        [Fact]
        public void Four_Tuples_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Read<long, string, string, long?>(Query).ToList();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal("1977-05-19", result[0].Item3);
            Assert.Equal(1, result[0].Item4);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal("1978-05-19", result[1].Item3);
            Assert.Equal(0, result[1].Item4);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal("1979-05-19", result[2].Item3);
            Assert.Null(result[2].Item4);
        }

        [Fact]
        public void Five_Tuples_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Read<long, string, string, long?, string>(Query).ToList();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal("1977-05-19", result[0].Item3);
            Assert.Equal(1, result[0].Item4);
            Assert.Null(result[0].Item5);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal("1978-05-19", result[1].Item3);
            Assert.Equal(0, result[1].Item4);
            Assert.Equal("bar2", result[1].Item5);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal("1979-05-19", result[2].Item3);
            Assert.Null(result[2].Item4);
            Assert.Equal("bar3", result[2].Item5);
        }

        [Fact]
        public async Task Three_Tuples_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<long, string, string>(Query).ToListAsync();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal("1977-05-19", result[0].Item3);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal("1978-05-19", result[1].Item3);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal("1979-05-19", result[2].Item3);
        }

        [Fact]
        public async Task Four_Tuples_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<long, string, string, long?>(Query).ToListAsync();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal("1977-05-19", result[0].Item3);
            Assert.Equal(1, result[0].Item4);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal("1978-05-19", result[1].Item3);
            Assert.Equal(0, result[1].Item4);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal("1979-05-19", result[2].Item3);
            Assert.Null(result[2].Item4);
        }

        [Fact]
        public async Task Five_Tuples_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<long, string, string, long?, string>(Query).ToListAsync();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal("1977-05-19", result[0].Item3);
            Assert.Equal(1, result[0].Item4);
            Assert.Null(result[0].Item5);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal("1978-05-19", result[1].Item3);
            Assert.Equal(0, result[1].Item4);
            Assert.Equal("bar2", result[1].Item5);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal("1979-05-19", result[2].Item3);
            Assert.Null(result[2].Item4);
            Assert.Equal("bar3", result[2].Item5);
        }

    }
}
