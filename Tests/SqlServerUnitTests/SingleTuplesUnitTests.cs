using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Norm;
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
            var result = connection.Read<int>(Query).Single();
            Assert.Equal(1, result);
        }


        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<int?>("select null").SingleAsync();
            Assert.Null(result);
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Read<int?>("select null").Single();
            Assert.Null(result);
        }

        [Fact]
        public void Two_Tuples_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2) = connection.Read<int, string>(Query).Single();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
        }

        [Fact]
        public async Task Single_Value_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<int>(Query).SingleAsync();
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Two_Tuples_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2) = await connection.ReadAsync<int, string>(Query).SingleAsync();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
        }

        [Fact]
        public void Three_Tuples_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3) = connection.Read<int, string, DateTime>(Query).Single();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
        }

        [Fact]
        public void Four_Tuples_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4) = connection.Read<int, string, DateTime, bool>(Query).Single();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
            Assert.True(r4);
        }

        [Fact]
        public void Five_Tuples_Test_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4, r5) = connection.Read<int, string, DateTime, bool, string>(Query).Single();
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
            var (r1, r2, r3) = await connection.ReadAsync<int, string, DateTime>(Query).SingleAsync();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
        }

        [Fact]
        public async Task Four_Tuples_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4) = await connection.ReadAsync<int, string, DateTime, bool>(Query).SingleAsync();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
            Assert.True(r4);
        }

        [Fact]
        public async Task Five_Tuples_Test_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4, r5) = await connection.ReadAsync<int, string, DateTime, bool, string>(Query).SingleAsync();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal(new DateTime(1977, 5, 19), r3);
            Assert.True(r4);
            Assert.Null(r5);
        }
    }
}
