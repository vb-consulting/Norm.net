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

        class SnakeCaseMapTestClass
        {
            public int MyId { get; private set; }
            public string MyFoo { get; private set; }
            public DateTime MyDay { get; private set; }
            public bool? MyBool { get; private set; }
            public string MyBar { get; private set; }
        }

        class ArraysTestClass
        {
            public int[] Id { get; private set; }
            public string[] Foo { get; private set; }
            public DateTime[] Day { get; private set; }
            public bool[] Bool { get; private set; }
            public string[] Bar { get; private set; }
        }

        private const string Query = @"
                            select *
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(id, foo, day, bool, bar)";

        private const string SnakeCaseQuery = @"
                            select *
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(my_id, my_foo, my_day, my_bool, my_bar)";

        private const string ArraysQuery = @"
                            select 
                                array_agg(id) as id,
                                array_agg(foo) as foo,
                                array_agg(day) as day,
                                array_agg(bool) as bool,
                                array_agg(bar) as bar
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3'),
                                (null, null, null, null, null)
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
        public void SelectEmpty_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read($"select * from ({Query}) q where id = 999").Select<TestClass>().ToList();
            Assert.Empty(result);
        }


        [Fact]
        public async Task SelectMap_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(Query).Select<TestClass>().ToListAsync();

            AssertTestClass(result);
        }

        [Fact]
        public void SelectSnakeCaseMap_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(SnakeCaseQuery).Select<SnakeCaseMapTestClass>().ToList();

            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].MyId);
            Assert.Equal(2, result[1].MyId);
            Assert.Equal(3, result[2].MyId);

            Assert.Equal("foo1", result[0].MyFoo);
            Assert.Equal("foo2", result[1].MyFoo);
            Assert.Equal("foo3", result[2].MyFoo);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].MyDay);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].MyDay);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].MyDay);

            Assert.Equal(true, result[0].MyBool);
            Assert.Equal(false, result[1].MyBool);
            Assert.Null(result[2].MyBool);

            Assert.Null(result[0].MyBar);
            Assert.Equal("bar2", result[1].MyBar);
            Assert.Equal("bar3", result[2].MyBar);
        }

        [Fact]
        public void SelectArraysMap_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(ArraysQuery).Select<ArraysTestClass>().ToList();

            Assert.Single(result);

            Assert.Equal(1, result[0].Id[0]);
            Assert.Equal(2, result[0].Id[1]);
            Assert.Equal(3, result[0].Id[2]);
            Assert.Equal(0, result[0].Id[3]);

            Assert.Equal("foo1", result[0].Foo[0]);
            Assert.Equal("foo2", result[0].Foo[1]);
            Assert.Equal("foo3", result[0].Foo[2]);
            Assert.Null(result[0].Foo[3]);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Day[0]);
            Assert.Equal(new DateTime(1978, 5, 19), result[0].Day[1]);
            Assert.Equal(new DateTime(1979, 5, 19), result[0].Day[2]);
            Assert.Equal(new DateTime(), result[0].Day[3]);

            Assert.Equal(true, result[0].Bool[0]);
            Assert.Equal(false, result[0].Bool[1]);
            Assert.Equal(false, result[0].Bool[2]);
            Assert.Equal(false, result[0].Bool[3]);

            Assert.Null(result[0].Bar[0]);
            Assert.Equal("bar2", result[0].Bar[1]);
            Assert.Equal("bar3", result[0].Bar[2]);
            Assert.Null(result[0].Bar[3]);
        }
    }
}
