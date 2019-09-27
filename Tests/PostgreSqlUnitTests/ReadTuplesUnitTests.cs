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
                            (1, 'foo1', '1977-05-19'::date),
                            (2, 'foo2', '1978-05-19'::date),
                            (3, 'foo3', '1979-05-19'::date)
                          ) t(first, bar, day)";

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
    }
}
