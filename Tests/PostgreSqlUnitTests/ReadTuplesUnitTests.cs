using System;
using System.Linq;
using System.Threading.Tasks;
using NoOrm;
using NoOrm.Extensions;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class ReadTuplesUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        private const string Query = @"
                          select * from (
                          values 
                            (1, 'foo1', '1977-05-19'::date, true, null),
                            (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                            (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                          ) t(first, bar, day, bool, s)";

        public ReadTuplesUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int?>("values (null)").ToList().First();
            Assert.Null(result);
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = (await connection.ReadAsync<int?>("values (null)").ToListAsync()).First();
            Assert.Null(result);
        }

        [Fact]
        public void Single_Value_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int>(Query).ToList();
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
        }

        [Fact]
        public void Two_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int, string>(Query).ToList();
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
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<int>(Query).ToListAsync();
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
        }

        [Fact]
        public async Task Two_Tuples_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<int, string>(Query).ToListAsync();
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
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int, string, DateTime>(Query).ToList();
            
            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].Item3);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Item3);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Item3);
        }

        [Fact]
        public void Four_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int, string, DateTime, bool?>(Query).ToList();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].Item3);
            Assert.Equal(true, result[0].Item4);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Item3);
            Assert.Equal(false, result[1].Item4);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Item3);
            Assert.Null(result[2].Item4);
        }

        [Fact]
        public void Five_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int, string, DateTime, bool?, string>(Query).ToList();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].Item3);
            Assert.Equal(true, result[0].Item4);
            Assert.Null(result[0].Item5);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Item3);
            Assert.Equal(false, result[1].Item4);
            Assert.Equal("bar2", result[1].Item5);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Item3);
            Assert.Null(result[2].Item4);
            Assert.Equal("bar3", result[2].Item5);
        }

        [Fact]
        public async Task Three_Tuples_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<int, string, DateTime>(Query).ToListAsync();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].Item3);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Item3);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Item3);
        }

        [Fact]
        public async Task Four_Tuples_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<int, string, DateTime, bool?>(Query).ToListAsync();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].Item3);
            Assert.Equal(true, result[0].Item4);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Item3);
            Assert.Equal(false, result[1].Item4);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Item3);
            Assert.Null(result[2].Item4);
        }


        [Fact]
        public async Task Five_Tuples_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<int, string, DateTime, bool?, string>(Query).ToListAsync();

            Assert.Equal(1, result[0].Item1);
            Assert.Equal("foo1", result[0].Item2);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].Item3);
            Assert.Equal(true, result[0].Item4);
            Assert.Null(result[0].Item5);

            Assert.Equal(2, result[1].Item1);
            Assert.Equal("foo2", result[1].Item2);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Item3);
            Assert.Equal(false, result[1].Item4);
            Assert.Equal("bar2", result[1].Item5);

            Assert.Equal(3, result[2].Item1);
            Assert.Equal("foo3", result[2].Item2);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Item3);
            Assert.Null(result[2].Item4);
            Assert.Equal("bar3", result[2].Item5);
        }
    }
}
