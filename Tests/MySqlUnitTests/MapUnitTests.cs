using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Norm;
using Xunit;

namespace MySqlUnitTests
{
    [Collection("MySqlDatabase")]
    public class MapUnitTests
    {
        private readonly MySqlFixture fixture;

        class TestClass
        {
            public long Id { get; init; }
            public string Foo { get; init; }
            public DateTime Day { get; init; }
            public long? Bool { get; init; }
            public string Bar { get; init; }
        }

        private const string Query = @"
        select 1 as id, 'foo1' as foo, cast('1977-05-19' as datetime) as day, true as bool, null as bar
        union all
        select 2 as id, 'foo2' as foo, cast('1978-05-19' as datetime) as day, false as bool, 'bar2' as bar
        union all
        select 3 as id, 'foo3' as foo, cast('1979-05-19' as datetime) as day, null as bool, 'bar3' as bar";

        public MapUnitTests(MySqlFixture fixture)
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

            Assert.Equal(1, result[0].Bool);
            Assert.Equal(0, result[1].Bool);
            Assert.Null(result[2].Bool);

            Assert.Null(result[0].Bar);
            Assert.Equal("bar2", result[1].Bar);
            Assert.Equal("bar3", result[2].Bar);
        }

        [Fact]
        public void Map_Sync()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClass>(Query).ToList();
            AssertTestClass(result);
        }

        [Fact]
        public void Map_Empty_Sync()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClass>($"select * from ({Query}) q where id = 999").ToList();
            Assert.Empty(result);
        }


        [Fact]
        public async Task Map_Async()
        {
            await using var connection = new MySqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<TestClass>(Query).ToListAsync();

            AssertTestClass(result);
        }
    }
}
