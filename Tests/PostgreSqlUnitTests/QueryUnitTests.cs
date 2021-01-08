using System;
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
            var result1 = connection.Read<TestClass>($"{Query} where id = @id", 1).ToList();
            var result2 = connection.Read<TestClass>($"{Query} where id = @id and foo = @foo", 1, "foo1").ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param1__SqlParam_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection.Read<TestClass>(
                $"{Query} where id = @id",
                new NpgsqlParameter("id", 1)).ToList();

            // switch position
            var result2 = connection.Read<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                new NpgsqlParameter("foo", "foo1"),
                new NpgsqlParameter("id", 1)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param2_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection.Read<TestClass>($"{Query} where id = @id", ("id", 1)).ToList();
            // switch position
            var result2 = connection.Read<TestClass>($"{Query} where id = @id and foo = @foo", ("foo", "foo1"), ("id", 1)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param3_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection.Read<TestClass>($"{Query} where id = @id", ("id", 1, DbType.Int32)).ToList();
            // switch position
            var result2 = connection.Read<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", DbType.String), ("id", 1, DbType.Int32)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param4_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection.Read<TestClass>($"{Query} where id = @id", ("id", 1, NpgsqlDbType.Integer)).ToList();
            // switch position
            var result2 = connection.Read<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", NpgsqlDbType.Varchar), ("id", 1, NpgsqlDbType.Integer)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param4_Mixed_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result2 = connection.Read<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", NpgsqlDbType.Varchar), ("id", 1, DbType.Int32)).ToList();
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
            var result1 = await connection.ReadAsync<TestClass>($"{Query} where id = @id", 1).ToListAsync();
            var result2 = await connection.ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo", 1, "foo1").ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param1__SqlParam_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<TestClass>(
                $"{Query} where id = @id",
                new NpgsqlParameter("id", 1)).ToListAsync();

            // switch position
            var result2 = await connection.ReadAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                new NpgsqlParameter("foo", "foo1"),
                new NpgsqlParameter("id", 1)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param2_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<TestClass>($"{Query} where id = @id", ("id", 1)).ToListAsync();
            // switch position
            var result2 = await connection.ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo", ("foo", "foo1"), ("id", 1)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param3_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<TestClass>($"{Query} where id = @id", ("id", 1, DbType.Int32)).ToListAsync();
            // switch position
            var result2 = await connection.ReadAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", DbType.String), ("id", 1, DbType.Int32)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param4_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<TestClass>($"{Query} where id = @id", ("id", 1, NpgsqlDbType.Integer)).ToListAsync();
            // switch position
            var result2 = await connection.ReadAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", NpgsqlDbType.Varchar), ("id", 1, NpgsqlDbType.Integer)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param4_Mixed_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result2 = await connection.ReadAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", NpgsqlDbType.Varchar), ("id", 1, DbType.Int32)).ToListAsync();
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
    }
}
