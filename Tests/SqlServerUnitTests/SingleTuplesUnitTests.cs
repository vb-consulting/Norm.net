using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using NoOrm.Extensions;
using Xunit;

namespace SqlServerUnitTests
{
    [Collection("SqlClientDatabase")]
    public class SingleTuplesUnitTests
    {
        private readonly SqlClientFixture fixture;

        private const string Query = @"select * 
                                        from (
                                            values (1, 'foo1', cast('1977-05-19' as date), cast(1 as bit), null)
                                        ) as t(a,b,c,d,e)";

        public SingleTuplesUnitTests(SqlClientFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Single_Value_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Single<int>(Query);
            Assert.Equal(1, result);
        }


        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = await connection.SingleAsync<int?>("select null");
            Assert.Null(result);
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Single<int?>("select null");
            Assert.Null(result);
        }

        [Fact]
        public void Two_Tuples_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2) = connection.Single<int, string>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
        }

        [Fact]
        public async Task Single_Value_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = await connection.SingleAsync<int>(Query);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Two_Tuples_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2) = await connection.SingleAsync<int, string>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
        }

        [Fact]
        public void Three_Tuples_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3) = connection.Single<int, string, DateTime>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
        }

        [Fact]
        public void Four_Tuples_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4) = connection.Single<int, string, DateTime, bool>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
            Assert.True(r4);
        }

        [Fact]
        public void Five_Tuples_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4, r5) = connection.Single<int, string, DateTime, bool, string>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
            Assert.True(r4);
            Assert.Null(r5);
        }

        [Fact]
        public async Task Three_Tuples_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3) = await connection.SingleAsync<int, string, DateTime>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
        }

        [Fact]
        public async Task Four_Tuples_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4) = await connection.SingleAsync<int, string, DateTime, bool>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
            Assert.True(r4);
        }

        [Fact]
        public async Task Five_Tuples_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4, r5) = await connection.SingleAsync<int, string, DateTime, bool, string>(Query);
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
            Assert.True(r4);
            Assert.Null(r5);
        }
    }
}
