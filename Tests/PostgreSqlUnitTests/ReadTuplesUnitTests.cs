using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
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

        [Fact]
        public void Six_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i1, i2, i3, i4, i5, i6) = connection.Read<int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6").Single();

            Assert.Equal(1, i1);
            Assert.Equal(2, i2);
            Assert.Equal(3, i3);
            Assert.Equal(4, i4);
            Assert.Equal(5, i5);
            Assert.Equal(6, i6);
        }

        [Fact]
        public void Seven_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i1, i2, i3, i4, i5, i6, i7) = connection.Read<int, int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6, 7").Single();

            Assert.Equal(1, i1);
            Assert.Equal(2, i2);
            Assert.Equal(3, i3);
            Assert.Equal(4, i4);
            Assert.Equal(5, i5);
            Assert.Equal(6, i6);
            Assert.Equal(7, i7);
        }

        [Fact]
        public void Eight_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i1, i2, i3, i4, i5, i6, i7, i8) = connection.Read<int, int, int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6, 7, 8").Single();

            Assert.Equal(1, i1);
            Assert.Equal(2, i2);
            Assert.Equal(3, i3);
            Assert.Equal(4, i4);
            Assert.Equal(5, i5);
            Assert.Equal(6, i6);
            Assert.Equal(7, i7);
            Assert.Equal(8, i8);
        }

        [Fact]
        public void Eight_Undestructed_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<int, int, int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6, 7, 8").Single();

            Assert.Equal(1, t.Item1);
            Assert.Equal(2, t.Item2);
            Assert.Equal(3, t.Item3);
            Assert.Equal(4, t.Item4);
            Assert.Equal(5, t.Item5);
            Assert.Equal(6, t.Item6);
            Assert.Equal(7, t.Item7);
            Assert.Equal(8, t.Item8);
        }

        [Fact]
        public void Nine_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i1, i2, i3, i4, i5, i6, i7, i8, i9) = connection.Read<int, int, int, int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6, 7, 8, 9").Single();

            Assert.Equal(1, i1);
            Assert.Equal(2, i2);
            Assert.Equal(3, i3);
            Assert.Equal(4, i4);
            Assert.Equal(5, i5);
            Assert.Equal(6, i6);
            Assert.Equal(7, i7);
            Assert.Equal(8, i8);
            Assert.Equal(9, i9);
        }

        [Fact]
        public void Nine_Undestructed_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<int, int, int, int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6, 7, 8, 9").Single();

            Assert.Equal(1, t.Item1);
            Assert.Equal(2, t.Item2);
            Assert.Equal(3, t.Item3);
            Assert.Equal(4, t.Item4);
            Assert.Equal(5, t.Item5);
            Assert.Equal(6, t.Item6);
            Assert.Equal(7, t.Item7);
            Assert.Equal(8, t.Item8);
            Assert.Equal(9, t.Item9);
        }

        [Fact]
        public void Ten_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i1, i2, i3, i4, i5, i6, i7, i8, i9, i10) = connection.Read<int, int, int, int, int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10").Single();

            Assert.Equal(1, i1);
            Assert.Equal(2, i2);
            Assert.Equal(3, i3);
            Assert.Equal(4, i4);
            Assert.Equal(5, i5);
            Assert.Equal(6, i6);
            Assert.Equal(7, i7);
            Assert.Equal(8, i8);
            Assert.Equal(9, i9);
            Assert.Equal(10, i10);
        }

        [Fact]
        public void Eleven_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11) = connection.Read<int, int, int, int, int, int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11").Single();

            Assert.Equal(1, i1);
            Assert.Equal(2, i2);
            Assert.Equal(3, i3);
            Assert.Equal(4, i4);
            Assert.Equal(5, i5);
            Assert.Equal(6, i6);
            Assert.Equal(7, i7);
            Assert.Equal(8, i8);
            Assert.Equal(9, i9);
            Assert.Equal(10, i10);
            Assert.Equal(11, i11);
        }

        [Fact]
        public void Twelve_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12) = connection.Read<int, int, int, int, int, int, int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12").Single();

            Assert.Equal(1, i1);
            Assert.Equal(2, i2);
            Assert.Equal(3, i3);
            Assert.Equal(4, i4);
            Assert.Equal(5, i5);
            Assert.Equal(6, i6);
            Assert.Equal(7, i7);
            Assert.Equal(8, i8);
            Assert.Equal(9, i9);
            Assert.Equal(10, i10);
            Assert.Equal(11, i11);
            Assert.Equal(12, i12);
        }

        [Fact]
        public void Twelve_Undestructed_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<int, int, int, int, int, int, int, int, int, int, int, int>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12").Single();

            Assert.Equal(1, t.Item1);
            Assert.Equal(2, t.Item2);
            Assert.Equal(3, t.Item3);
            Assert.Equal(4, t.Item4);
            Assert.Equal(5, t.Item5);
            Assert.Equal(6, t.Item6);
            Assert.Equal(7, t.Item7);
            Assert.Equal(8, t.Item8);
            Assert.Equal(9, t.Item9);
            Assert.Equal(10, t.Item10);
            Assert.Equal(11, t.Item11);
            Assert.Equal(12, t.Item12);
        }
    }
}
