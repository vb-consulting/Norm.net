using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Xunit;

namespace SQLiteUnitTests
{
    [Collection("SQLiteDatabase")]
    public class ObjectMappingUnitTests
    {
        private readonly SqLiteFixture fixture;

        class TestClass
        {
            public long Id { get; private set; }
            public string Foo { get; private set; }
            public string Day { get; private set; }
            public long? Bool { get; private set; }
            public string Bar { get; private set; }
        }

        class TestClass2
        {
            public long Id { get; private set; }
            public string Foo { get; private set; }
            public DateTime Day { get; private set; }
            public bool? Bool { get; private set; }
            public string Bar { get; private set; }
        }

        private const string Query = @"
            with cte(id, foo, day, bool, bar) as (
            select * from (
                values
                  (1, 'foo1', date('1977-05-19'), true, null),
                  (2, 'foo2', date('1978-05-19'), false, 'bar2'),
                  (3, 'foo3', date('1979-05-19'), null, 'bar3')
                )
            )
            select * from cte";

        public ObjectMappingUnitTests(SqLiteFixture fixture)
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

            Assert.Equal("1977-05-19", result[0].Day);
            Assert.Equal("1978-05-19", result[1].Day);
            Assert.Equal("1979-05-19", result[2].Day);

            Assert.Equal(1, result[0].Bool);
            Assert.Equal(0, result[1].Bool);
            Assert.Null(result[2].Bool);

            Assert.Null(result[0].Bar);
            Assert.Equal("bar2", result[1].Bar);
            Assert.Equal("bar3", result[2].Bar);
        }

        private void AssertTestClass2(IList<TestClass2> result)
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
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Read(Query).Map<TestClass>().ToList();

            AssertTestClass(result);
        }

        [Fact]
        public async Task SelectMap_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(Query).Map<TestClass>().ToListAsync();

            AssertTestClass(result);
        }

        [Fact]
        public void SelectMapFromTable_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            connection
                .Execute("create table test_class2 (id integer, foo string, day datetime, bool boolean, bar string);")
                .Execute(@"
                    insert into test_class2 
                    (id, foo, day, bool, bar)
                    values
                    (@id1, @foo1, @day1, @bool1, @bar1),
                    (@id2, @foo2, @day2, @bool2, @bar2),
                    (@id3, @foo3, @day3, @bool3, @bar3);",
                    1, "foo1", new DateTime(1977, 5, 19), true, null,
                    2, "foo2", new DateTime(1978, 5, 19), false, "bar2",
                    3, "foo3", new DateTime(1979, 5, 19), null, "bar3");

            var result = connection.Read("select * from test_class2").Map<TestClass2>().ToList();

            AssertTestClass2(result);

            connection.Execute("drop table test_class2;");
        }

        [Fact]
        public async Task SelectMapFromTable_ASync()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            await connection.ExecuteAsync("create table test_class2 (id integer, foo string, day datetime, bool boolean, bar string);"); 
            await connection.ExecuteAsync(@"
                    insert into test_class2 
                    (id, foo, day, bool, bar)
                    values
                    (@id1, @foo1, @day1, @bool1, @bar1),
                    (@id2, @foo2, @day2, @bool2, @bar2),
                    (@id3, @foo3, @day3, @bool3, @bar3);",
                    1, "foo1", new DateTime(1977, 5, 19), true, null,
                    2, "foo2", new DateTime(1978, 5, 19), false, "bar2",
                    3, "foo3", new DateTime(1979, 5, 19), null, "bar3");

            var result = await connection.ReadAsync("select * from test_class2").Map<TestClass2>().ToListAsync();

            AssertTestClass2(result);

            await connection.ExecuteAsync("drop table test_class2;");
        }

    }
}
