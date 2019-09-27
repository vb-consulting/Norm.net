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
    public class SingleTuplesUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        private const string Query = "values (1, 'foo1', '1977-05-19'::date)";

        public SingleTuplesUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Single_Value_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Single<int>(Query);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Single<int?>("values (null)");
            Assert.Null(result);
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.SingleAsync<int?>("values (null)");
            Assert.Null(result);
        }

        [Fact]
        public void Two_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (r1, r2) = connection.Single<int, string>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
        }

        [Fact]
        public async Task Single_Value_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.SingleAsync<int>(Query);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Two_Tuples_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (r1, r2) = await connection.SingleAsync<int, string>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
        }
    }
}
