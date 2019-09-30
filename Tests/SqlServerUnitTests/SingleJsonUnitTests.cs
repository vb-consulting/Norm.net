using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NoOrm;
using NoOrm.Extensions;
using Xunit;

namespace SqlServerUnitTests
{
    [Collection("SqlClientDatabase")]
    public class SingleJsonUnitTests
    {
        private readonly SqlClientFixture fixture;

        class TestClass
        {
            public int Id { get; set; }
            public string Foo { get; set; }
            public DateTime Day { get; set; }
            public bool? Bool { get; set; }
            public string Bar { get; set; }
        }

        private const string Query = @"
                            select *
                            from (
                            values 
                                (1, 'foo1', cast('1977-05-19' as date), cast(1 as bit) , null),
                                (2, 'foo2', cast('1978-05-19' as date), cast(0 as bit), 'bar2'),
                                (3, 'foo3', cast('1979-05-19' as date), null, 'bar3')
                            ) t(Id, Foo, Day, Bool, Bar)
                            for json auto";

        public SingleJsonUnitTests(SqlClientFixture fixture)
        {
            this.fixture = fixture;
        }

        private void AssertTestClass(IList<TestClass> result)
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
        public void DeserializeSingle_TestList_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.SingleJson<IList<TestClass>>(Query);

            AssertTestClass(result);
        }

        [Fact]
        public void DeserializeSingle_TestEnumerable_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.SingleJson<IEnumerable<TestClass>>(Query).ToList();

            AssertTestClass(result);
        }

        [Fact]
        public async Task DeserializeSingle_TestList_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = await connection.SingleJsonAsync<IList<TestClass>>(Query);

            AssertTestClass(result);
        }

        [Fact]
        public async Task DeserializeSingle_TestEnumerable_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = (await connection.SingleJsonAsync<IEnumerable<TestClass>>(Query)).ToList();

            AssertTestClass(result);
        }
    }
}
