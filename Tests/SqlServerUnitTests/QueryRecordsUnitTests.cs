using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm.Extensions;
using Xunit;

namespace SqlServerUnitTests
{
    public record TestRecord(int Id, string Foo, DateTime Day, bool? Bool, string Bar);

    [Collection("SqlClientDatabase")]
    public class QueryRecordsUnitTests
    {
        private readonly SqlClientFixture fixture;

        private const string Query = @"
                            select *
                            from (values
                              (1, 'foo1', cast('1977-05-19' as date), cast(1 as bit) , null),
                              (2, 'foo2', cast('1978-05-19' as date), cast(0 as bit), 'bar2'),
                              (3, 'foo3', cast('1979-05-19' as date), null, 'bar3')
                            ) t (id, foo, day, bool, bar)";

        public QueryRecordsUnitTests(SqlClientFixture fixture)
        {
            this.fixture = fixture;
        }

        private void AssertTestRecord(IList<TestRecord> result)
        {
            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
            Assert.Equal(3, result[2].Id);

            Assert.Equal("foo1", result[0].Foo);
            Assert.Equal("foo2", result[1].Foo);
            Assert.Equal("foo3", result[2].Foo);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Day);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Day);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Day);

            Assert.Equal(true, result[0].Bool);
            Assert.Equal(false, result[1].Bool);
            Assert.Null(result[2].Bool);

            Assert.Null(result[0].Bar);
            Assert.Equal("bar2", result[1].Bar);
            Assert.Equal("bar3", result[2].Bar);
        }



        [Fact]
        public void SelectMap_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Query<TestRecord>(Query).ToList();
            AssertTestRecord(result);
        }

        [Fact]
        public void SelectEmpty_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Query<TestRecord>($"select * from ({Query}) q where id = 999").ToList();
            Assert.Empty(result);
        }


        [Fact]
        public async Task SelectMap_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = await connection.QueryAsync<TestRecord>(Query).ToListAsync();
            AssertTestRecord(result);
        }
    }
}
