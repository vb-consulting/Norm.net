using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm.Extensions;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class ObjectMappingUnitTests
    {
        private readonly PostgreSqlFixture fixture;

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
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(id, foo, day, bool, bar)";

        public ObjectMappingUnitTests(PostgreSqlFixture fixture)
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
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(Query).Select<TestClass>().ToList();

            AssertTestClass(result);
        }

       
        [Fact]
        public async Task SelectMap_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(Query).Select<TestClass>().ToListAsync();

            AssertTestClass(result);
        }


        [Fact]
        public void Select_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            //connection.Select(nameof(TestClass.Bool), nameof(TestClass.Bar));
            //connection.Select<TestClass>(p => p.Bar, p.Bool);
        }
    }
}
