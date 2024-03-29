﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using NpgsqlTypes;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class QueryUnitTests
    {
        private readonly PostgreSqlFixture fixture;

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
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(id, foo, day, bool, bar)";


        public QueryUnitTests(PostgreSqlFixture fixture)
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

        private void AssertSingleTestClass(IList<TestClass> result)
        {
            Assert.Equal(1, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("foo1", result[0].Foo);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].Day);
            Assert.Equal(true, result[0].Bool);
            Assert.Null(result[0].Bar);
        }

        [Fact]
        public void Query_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClass>(Query).ToList();
            AssertTestClass(result);
        }

        [Fact]
        public void Query_Param1_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result1 = connection
                .WithParameters(1)
                .Read<TestClass>($"{Query} where id = @id")
                .ToList();

            var result2 = connection
                .WithParameters(1, "foo1")
                .Read<TestClass>($"{Query} where id = @id and foo = @foo")
                .ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param1__SqlParam_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result1 = connection
                .WithParameters(new NpgsqlParameter("id", 1))
                .Read<TestClass>($"{Query} where id = @id")
                .ToList();

            // switch position
            var result2 = connection
                .WithParameters(new NpgsqlParameter("foo", "foo1"), new NpgsqlParameter("id", 1))
                .Read<TestClass>(
                $"{Query} where id = @id and foo = @foo").ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param2_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection
                .WithParameters(new { id = 1 })
                .Read<TestClass>($"{Query} where id = @id")
                .ToList();

            // switch position
            var result2 = connection
                .WithParameters(new { id = 1, foo = "foo1" })
                .Read<TestClass>($"{Query} where id = @id and foo = @foo")
                .ToList();

            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param3_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result1 = connection
                .WithParameters(new { id = (1, DbType.Int32) })
                .Read<TestClass>($"{Query} where id = @id")
                .ToList();

            // switch position
            var result2 = connection
                .WithParameters(new { id = (1, DbType.Int32), foo = ("foo1", DbType.String) })
                .Read<TestClass>($"{Query} where id = @id and foo = @foo")
                .ToList();

            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param4_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection
                .WithParameters(new { id = (1, NpgsqlDbType.Integer) })
                .Read<TestClass>($"{Query} where id = @id")
                .ToList();

            // switch position
            var result2 = connection
                .WithParameters(new { foo = ("foo1", NpgsqlDbType.Varchar), id = (1, NpgsqlDbType.Integer) })
                .Read<TestClass>($"{Query} where id = @id and foo = @foo")
                .ToList();

            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param4_Mixed_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result2 = connection
                .WithParameters(new { foo = ("foo1", NpgsqlDbType.Varchar), id = (1, DbType.Int32) })
                .Read<TestClass>($"{Query} where id = @id and foo = @foo")
                .ToList();

            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<TestClass>(Query).ToListAsync();
            AssertTestClass(result);
        }

        [Fact]
        public async Task Query_Param1_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection
                .WithParameters(1)
                .ReadAsync<TestClass>($"{Query} where id = @id")
                .ToListAsync();
            var result2 = await connection
                .WithParameters(1, "foo1")
                .ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo")
                .ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param1__SqlParam_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection
                .WithParameters(new NpgsqlParameter("id", 1))
                .ReadAsync<TestClass>(
                $"{Query} where id = @id")
                .ToListAsync();

            // switch position
            var result2 = await connection
                .WithParameters(new NpgsqlParameter("foo", "foo1"), new NpgsqlParameter("id", 1))
                .ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo").ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param2_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection
                .WithParameters(new { id = 1 })
                .ReadAsync<TestClass>(
                $"{Query} where id = @id")
                .ToListAsync();

            // switch position
            var result2 = await connection
                .WithParameters(new { id = 1, foo = "foo1" })
                .ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo")
                .ToListAsync();

            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param3_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection
                .WithParameters(new { id = (1, DbType.Int32) })
                .ReadAsync<TestClass>($"{Query} where id = @id")
                .ToListAsync();

            // switch position
            var result2 = await connection
                .WithParameters(new { id = (1, DbType.Int32), foo = ("foo1", DbType.String) })
                .ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo")
                .ToListAsync();

            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param4_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection
                .WithParameters(new { id = (1, NpgsqlDbType.Integer) })
                .ReadAsync<TestClass>($"{Query} where id = @id")
                .ToListAsync();

            // switch position
            var result2 = await connection
                .WithParameters(new { id = (1, NpgsqlDbType.Integer), foo = ("foo1", NpgsqlDbType.Varchar) })
                .ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo")
                .ToListAsync();

            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param4_Mixed_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result2 = await connection
                .WithParameters(new { id = (1, DbType.Int32), foo = ("foo1", NpgsqlDbType.Varchar) })
                .ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo").ToListAsync();
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Parallel_Sync()
        {
            var query = $"select * from ({Query}) t1 cross join ({Query}) t2 cross join ({Query}) t3"; // 27 records
            var task = new Action(() =>
            {
                using var connection = new NpgsqlConnection(fixture.ConnectionString);
                var result = connection.Read<TestClass>(query).ToList();
            });

            Task.WaitAll(
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task),
                Task.Factory.StartNew(task));
        }

        class Regression_Issue1
        {
            public string Foo { get; set; }
            public TimeSpan? Interval1 { get; set; }
            public TimeSpan? Interval2 { get; set; }
            public string Bar { get; set; }
            public bool Bool { get; set; }
        }

        [Fact]
        public async Task ReadAsync_Regression_Issue1()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var q = @"
            select * from (
            values 
                ('foo1', interval '1 days', interval '2 days', 'bar1', true),
                ('foo2', null, null, 'bar2', false)
            ) t(foo, interval1, interval2, bar, bool)
            ";
            var t = await connection.ReadAsync<Regression_Issue1>(q).ToListAsync();

            Assert.Equal("foo1", t[0].Foo);
            Assert.Equal(TimeSpan.FromDays(1), t[0].Interval1);
            Assert.Equal(TimeSpan.FromDays(2), t[0].Interval2);
            Assert.Equal("bar1", t[0].Bar);
            Assert.True(t[0].Bool);

            Assert.Equal("foo2", t[1].Foo);
            Assert.Null(t[1].Interval1);
            Assert.Null(t[1].Interval2);
            Assert.Equal("bar2", t[1].Bar);
            Assert.False(t[1].Bool);
        }

        class Regression_Issue1_Array
        {
            public TimeSpan[] IntervalArray { get; set; }
        }

        [Fact]
        public async Task ReadAsync_Regression_Issue1_Array()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = await connection.ReadAsync<Regression_Issue1_Array>("select ARRAY[interval '1 days', interval '2 days'] as intervalArray").SingleAsync();

            Assert.Equal(new TimeSpan[] { TimeSpan.FromDays(1), TimeSpan.FromDays(2) }, t.IntervalArray);
        }


        class OffsetTestClass
        {
            public DateTimeOffset Offset { get; set; }
            public DateTime Timestamp { get; set; }
        }

        [Fact]
        public void Test_DateTimeOffsetType_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<OffsetTestClass>(@"select ""Offset"", ""Offset"" as Timestamp
                            from(
                            values
                                ('1977-05-19'::timestamp),
                                ('1978-05-19'::timestamp),
                                ('1979-05-19'::timestamp)
                            ) t(""Offset"")").ToList();

            Assert.Equal(new DateTimeOffset(new DateTime(1977, 5, 19)), result[0].Offset);
            Assert.Equal(new DateTimeOffset(new DateTime(1978, 5, 19)), result[1].Offset);
            Assert.Equal(new DateTimeOffset(new DateTime(1979, 5, 19)), result[2].Offset);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Timestamp);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Timestamp);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Timestamp);
        }

        class OffsetTestClassNullable
        {
            public DateTimeOffset? Offset { get; set; }
            public DateTime? Timestamp { get; set; }
        }

        [Fact]
        public void Test_DateTimeOffsetNullableType_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<OffsetTestClassNullable>(@"select ""Offset"", ""Offset"" as Timestamp
                            from(
                            values
                                ('1977-05-19'::timestamp),
                                (null),
                                ('1979-05-19'::timestamp)
                            ) t(""Offset"")").ToList();

            Assert.Equal(new DateTimeOffset(new DateTime(1977, 5, 19)), result[0].Offset);
            Assert.Null(result[1].Offset);
            Assert.Equal(new DateTimeOffset(new DateTime(1979, 5, 19)), result[2].Offset);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Timestamp);
            Assert.Null(result[1].Timestamp);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Timestamp);
        }

        class OffsetArrayClass
        {
            public DateTimeOffset[] OffsetArray { get; set; }
        }

        [Fact]
        public void Test_DateTimeOffset_Array_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<OffsetArrayClass>(@"select ARRAY['1977-05-19'::timestamp, '1978-05-19'::timestamp, '1979-05-19'::timestamp] as offset_array").ToList();

            Assert.Equal(new DateTimeOffset(new DateTime(1977, 5, 19)), result[0].OffsetArray[0]);
            Assert.Equal(new DateTimeOffset(new DateTime(1978, 5, 19)), result[0].OffsetArray[1]);
            Assert.Equal(new DateTimeOffset(new DateTime(1979, 5, 19)), result[0].OffsetArray[2]);
        }

        public class MyClassWithPrivateSetters
        {
            public int Value1 { get; set; }
            public int Value2 { get; set; }
            public int Value3;
            public int Value4 { get; }
            public int Value5 { get; private set; }
            public int Value6 { get; protected set; }
            public int Value7 { get; internal set; }
            public int Value8 { get; init; }
        }

        [Fact]
        public void Test_Private_Setters_Skip_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<MyClassWithPrivateSetters>(@"select 1 as value1, 2 as value2, 3 as value3, 4 as value4, 5 as value5, 6 as value6, 7 as value7, 8 as value8").First();
            Assert.Equal(1, result.Value1);
            Assert.Equal(2, result.Value2);
            Assert.Equal(0, result.Value3);
            Assert.Equal(0, result.Value4);
            Assert.Equal(0, result.Value5);
            Assert.Equal(0, result.Value6);
            Assert.Equal(0, result.Value7);
            Assert.Equal(8, result.Value8);
        }


        public class BaseMyRefClass
        {
        }

        public class MyRefClass : BaseMyRefClass
        {
            public int Value2 { get; set; }
        }

        public class MyClassWithVirtualRef
        {
            public int Value1 { get; set; }
            public virtual BaseMyRefClass Ref1 { get; set; }
            public virtual MyRefClass Ref2 { get; set; }
            public virtual int VirtualValue { get; set; }
            public virtual DateTime Date { get; set; }
            public virtual string Str { get; set; }
            public virtual Guid Guid { get; set; }
        }

        [Fact]
        public void Test_Class_With_Virtual_Ref_Sync()
        {
            var guid = Guid.NewGuid().ToString();
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute(@"create extension if not exists ""uuid-ossp""");
            var result = connection.Read<MyClassWithVirtualRef>(@$"
                select 
                    1 as Value1, 
                    2 as VirtualValue, 
                    '2022-01-01'::timestamp as Date, 
                    'str' as Str, 
                    '{guid}'::uuid as Guid").First();
            Assert.Equal(1, result.Value1);
            Assert.Null(result.Ref1);
            Assert.Null(result.Ref2);
            Assert.Equal(2, result.VirtualValue);
            Assert.Equal(new DateTime(2022, 1, 1), result.Date);
            Assert.Equal("str", result.Str);
            Assert.Equal(guid, result.Guid.ToString());
        }
    }
}
