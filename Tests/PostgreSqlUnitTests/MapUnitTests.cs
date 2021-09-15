using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class MapUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        class TestClass
        {
            public int Id { get; init; }
            public string Foo { get; init; }
            public DateTime Day { get; init; }
            public bool? Bool { get; init; }
            public string Bar { get; init; }
        }

        class TestClassPrivateFields
        {
            public int Id { get; private set; }
            public string Foo { get; private set; }
            public DateTime Day { get; private set; }
            public bool? Bool { get; private set; }
            public string Bar { get; private set; }
        }

        class TestClassSettersOnly
        {
            public int Id { get; }
            public string Foo { get; }
            public DateTime Day { get; }
            public bool? Bool { get; }
            public string Bar { get; }
        }

        class SnakeCaseMapTestClass
        {
            public int MyId { get; init; }
            public string MyFoo { get; init; }
            public DateTime MyDay { get; init; }
            public bool? MyBool { get; init; }
            public string MyBar { get; init; }
        }

        class ArraysTestClass
        {
            public int[] Id { get; set; }
            public string[] Foo { get; set; }
            public DateTime[] Day { get; set; }
            public bool[] Bool { get; set; }
            public string[] Bar { get; set; }
        }

        class TestClassChangedPosition
        {
            public string Bar { get; init; }
            public bool? Bool { get; init; }
            public DateTime Day { get; init; }
            public string Foo { get; init; }
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
                                (1, 'foo1', '1977-05-19'::date, true, 'bar1'),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, true, 'bar3'),
                                (4, 'foo4', '1980-05-19'::date, false, 'bar4')
                            ) t(id, foo, day, bool, bar)";

        public MapUnitTests(PostgreSqlFixture fixture)
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

        private void AssertTestClassPrivateFields(IList<TestClassPrivateFields> result)
        {
            Assert.Equal(3, result.Count);

            Assert.Equal(default, result[0].Id);
            Assert.Equal(default, result[1].Id);
            Assert.Equal(default, result[2].Id);

            Assert.Equal(default, result[0].Foo);
            Assert.Equal(default, result[1].Foo);
            Assert.Equal(default, result[2].Foo);

            Assert.Equal(default, result[0].Day);
            Assert.Equal(default, result[1].Day);
            Assert.Equal(default, result[2].Day);

            Assert.Equal(default, result[0].Bool);
            Assert.Equal(default, result[1].Bool);
            Assert.Equal(default, result[2].Bool);

            Assert.Equal(default, result[0].Bar);
            Assert.Equal(default, result[1].Bar);
            Assert.Equal(default, result[2].Bar);
        }

        private void AssertTestClassSettersOnly(IList<TestClassSettersOnly> result)
        {
            Assert.Equal(3, result.Count);

            Assert.Equal(default, result[0].Id);
            Assert.Equal(default, result[1].Id);
            Assert.Equal(default, result[2].Id);

            Assert.Equal(default, result[0].Foo);
            Assert.Equal(default, result[1].Foo);
            Assert.Equal(default, result[2].Foo);

            Assert.Equal(default, result[0].Day);
            Assert.Equal(default, result[1].Day);
            Assert.Equal(default, result[2].Day);

            Assert.Equal(default, result[0].Bool);
            Assert.Equal(default, result[1].Bool);
            Assert.Equal(default, result[2].Bool);

            Assert.Equal(default, result[0].Bar);
            Assert.Equal(default, result[1].Bar);
            Assert.Equal(default, result[2].Bar);
        }

        [Fact]
        public void Map_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClass>(Query).ToList();
            AssertTestClass(result);
        }

        [Fact]
        public void Map_PrivateFields_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClassPrivateFields>(Query).ToList();
            AssertTestClassPrivateFields(result);
        }

        [Fact]
        public void Map_SettersOnly_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClassSettersOnly>(Query).ToList();
            AssertTestClassSettersOnly(result);
        }

        [Fact]
        public void Map_Empty_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClass>($"select * from ({Query}) q where id = 999").ToList();
            Assert.Empty(result);
        }


        [Fact]
        public async Task Map_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<TestClass>(Query).ToListAsync();

            AssertTestClass(result);
        }

        [Fact]
        public void Map_Snake_Case_Names_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<SnakeCaseMapTestClass>(SnakeCaseQuery).ToList();

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
        public void Map_Array_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<ArraysTestClass>(ArraysQuery).ToList();

            Assert.Single(result);

            Assert.Equal(1, result[0].Id[0]);
            Assert.Equal(2, result[0].Id[1]);
            Assert.Equal(3, result[0].Id[2]);
            Assert.Equal(4, result[0].Id[3]);

            Assert.Equal("foo1", result[0].Foo[0]);
            Assert.Equal("foo2", result[0].Foo[1]);
            Assert.Equal("foo3", result[0].Foo[2]);
            Assert.Equal("foo4", result[0].Foo[3]);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Day[0]);
            Assert.Equal(new DateTime(1978, 5, 19), result[0].Day[1]);
            Assert.Equal(new DateTime(1979, 5, 19), result[0].Day[2]);
            Assert.Equal(new DateTime(1980, 5, 19), result[0].Day[3]);

            Assert.True(result[0].Bool[0]);
            Assert.False(result[0].Bool[1]);
            Assert.True(result[0].Bool[2]);
            Assert.False(result[0].Bool[3]);

            Assert.Equal("bar1", result[0].Bar[0]);
            Assert.Equal("bar2", result[0].Bar[1]);
            Assert.Equal("bar3", result[0].Bar[2]);
            Assert.Equal("bar4", result[0].Bar[3]);
        }

        [Fact]
        public void Map_Changed_Position_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClassChangedPosition>(Query).ToList();

            Assert.Equal(3, result.Count);

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
        public void Map_And_Ignore_Not_Mapped_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClass>(@"
                            select *
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true),
                                (2, 'foo2', '1978-05-19'::date, false),
                                (3, 'foo3', '1979-05-19'::date, null)
                            ) t(id, foo, day, bool)").ToList();


            Assert.Equal(3, result.Count);

            // TestClass.Bar is not mapped, it is always default (null)
            Assert.Null(result[0].Bar);
            Assert.Null(result[0].Bar);
            Assert.Null(result[0].Bar);
        }

        private record DateTimeRecord(DateTime? LockoutEnd);
        private record DateTimeRecordOffset(DateTimeOffset? LockoutEnd);

        [Fact]
        public void Map_DateTime_And_DateTimeOffset_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            // Map normal DateTimeRecord record
            var result1 = connection.Read<DateTimeRecord>(@"
                            select *
                            from (
                            values 
                                ('1977-05-19'::timestamp),
                                (null),
                                ('1979-05-19'::timestamp)
                            ) t(lock_out_end)").ToList();

            // Should map ok
            Assert.Equal(3, result1.Count);
            Assert.Equal(new DateTime(1977, 5, 19), result1[0].LockoutEnd);
            Assert.Null(result1[1].LockoutEnd);
            Assert.Equal(new DateTime(1979, 5, 19), result1[2].LockoutEnd);

            // Map to DateTimeRecordOffset by using Select projection from DateTime? value
            var result3 = connection.Read<DateTime?>(@"
                            select *
                            from (
                            values 
                                ('1977-05-19'::timestamp),
                                (null),
                                ('1979-05-19'::timestamp)
                            ) t(lock_out_end)")
                .Select(d => d == null ? new DateTimeRecordOffset(null) : new DateTimeRecordOffset(new DateTimeOffset(d.Value)))
                .ToList();

            Assert.Equal(3, result3.Count);
            Assert.Equal(new DateTimeOffset(new DateTime(1977, 5, 19)), result3[0].LockoutEnd);
            Assert.Null(result3[1].LockoutEnd);
            Assert.Equal(new DateTimeOffset(new DateTime(1979, 5, 19)), result3[2].LockoutEnd);

            // Map to DateTimeRecordOffset by using Select projection from DateTimeRecord record
            var result4 = connection.Read<DateTimeRecord>(@"
                            select *
                            from (
                            values 
                                ('1977-05-19'::timestamp),
                                (null),
                                ('1979-05-19'::timestamp)
                            ) t(lock_out_end)")
                .Select(d => d.LockoutEnd == null ? new DateTimeRecordOffset(null) : new DateTimeRecordOffset(new DateTimeOffset(d.LockoutEnd.Value)))
                .ToList();

            Assert.Equal(3, result4.Count);
            Assert.Equal(new DateTimeOffset(new DateTime(1977, 5, 19)), result4[0].LockoutEnd);
            Assert.Null(result4[1].LockoutEnd);
            Assert.Equal(new DateTimeOffset(new DateTime(1979, 5, 19)), result4[2].LockoutEnd);

        }

        [Fact]
        public void Test_Map_SameClass_DifferentQuery_Parallel_Sync()
        {
            var task1 = new Action(() =>
            {
                using var connection = new NpgsqlConnection(fixture.ConnectionString);
                var result1 = connection.Read<TestClass>(@"
                            select *
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date),
                                (2, 'foo2', '1978-05-19'::date),
                                (3, 'foo3', '1979-05-19'::date)
                            ) t(id, foo, day)").ToList();
            });

            var task2 = new Action(() =>
            {
                using var connection = new NpgsqlConnection(fixture.ConnectionString);
                var result2 = connection.Read<TestClass>(@"
                            select *
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(id, foo, day, bool, bar)").ToList();
            });

            Task.WaitAll(
                Task.Factory.StartNew(task1), Task.Factory.StartNew(task1), Task.Factory.StartNew(task1),
                Task.Factory.StartNew(task2), Task.Factory.StartNew(task2), Task.Factory.StartNew(task2)
                );
        }
    }
}
