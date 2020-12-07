using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Xunit;

namespace SqlServerUnitTests
{
    [Collection("SqlClientDatabase")]
    public class ObjectMappingUnitTests
    {
        private readonly SqlClientFixture fixture;

        class TestClass
        {
            public int Id { get; private set; }
            public string Foo { get; private set; }
            public DateTime Day { get; private set; }
            public bool? Bool { get; private set; }
            public string Bar { get; private set; }
        }

        private const string Query = @"
                            select *
                            from (values
                              (1, 'foo1', cast('1977-05-19' as date), cast(1 as bit) , null),
                              (2, 'foo2', cast('1978-05-19' as date), cast(0 as bit), 'bar2'),
                              (3, 'foo3', cast('1979-05-19' as date), null, 'bar3')
                            ) t (id, foo, day, bool, bar)";
        public ObjectMappingUnitTests(SqlClientFixture fixture)
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
        public void SelectMap_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Read(Query).Map<TestClass>().ToList();

            AssertTestClass(result);
        }


        [Fact]
        public async Task SelectMap_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(Query).Map<TestClass>().ToListAsync();

            AssertTestClass(result);
        }
    }
}
