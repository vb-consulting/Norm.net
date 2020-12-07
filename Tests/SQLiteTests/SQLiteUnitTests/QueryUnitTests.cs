﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm.Extensions;
using Xunit;

namespace SQLiteUnitTests
{
    [Collection("SQLiteDatabase")]
    public class QueryUnitTests
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


        public QueryUnitTests(SqLiteFixture fixture)
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

        private void AssertSingleTestClass(IList<TestClass> result)
        {
            Assert.Equal(1, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("foo1", result[0].Foo);
            Assert.Equal("1977-05-19", result[0].Day);
            Assert.Equal(1, result[0].Bool);
            Assert.Null(result[0].Bar);

        }

        [Fact]
        public void Query_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Query<TestClass>(Query).ToList();
            AssertTestClass(result);
        }

        [Fact]
        public void Query_Param1_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = connection.Query<TestClass>($"{Query} where id = @id", 1).ToList();
            var result2 = connection.Query<TestClass>($"{Query} where id = @id and foo = @foo", 1, "foo1").ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param1__SqlParam_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = connection.Query<TestClass>(
                $"{Query} where id = @id",
                new SQLiteParameter("id", 1)).ToList();

            // switch position
            var result2 = connection.Query<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                new SQLiteParameter("foo", "foo1"),
                new SQLiteParameter("id", 1)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param2_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = connection.Query<TestClass>($"{Query} where id = @id", ("id", 1)).ToList();
            // switch position
            var result2 = connection.Query<TestClass>($"{Query} where id = @id and foo = @foo", ("foo", "foo1"), ("id", 1)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param3_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = connection.Query<TestClass>($"{Query} where id = @id", ("id", 1, DbType.Int32)).ToList();
            // switch position
            var result2 = connection.Query<TestClass>(
                $"{Query} where id = @id and foo = @foo", 
                ("foo", "foo1", DbType.String), ("id", 1, DbType.Int32)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param4_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = connection.Query<TestClass>($"{Query} where id = @id", ("id", 1, DbType.Int32)).ToList();
            // switch position
            var result2 = connection.Query<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", DbType.String), ("id", 1, DbType.Int32)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Async()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = await connection.QueryAsync<TestClass>(Query).ToListAsync();
            AssertTestClass(result);
        }

        [Fact]
        public async Task Query_Param1_Async()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = await connection.QueryAsync<TestClass>($"{Query} where id = @id", 1).ToListAsync();
            var result2 = await connection.QueryAsync<TestClass>($"{Query} where id = @id and foo = @foo", 1, "foo1").ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param1__SqlParam_Async()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = await connection.QueryAsync<TestClass>(
                $"{Query} where id = @id",
                new SQLiteParameter("id", 1)).ToListAsync();

            // switch position
            var result2 = await connection.QueryAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                new SQLiteParameter("foo", "foo1"),
                new SQLiteParameter("id", 1)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param2_Async()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = await connection.QueryAsync<TestClass>($"{Query} where id = @id", ("id", 1)).ToListAsync();
            // switch position
            var result2 = await connection.QueryAsync<TestClass>($"{Query} where id = @id and foo = @foo", ("foo", "foo1"), ("id", 1)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param3_Async()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = await connection.QueryAsync<TestClass>($"{Query} where id = @id", ("id", 1, DbType.Int32)).ToListAsync();
            // switch position
            var result2 = await connection.QueryAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", DbType.String), ("id", 1, DbType.Int32)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param4_Async()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result1 = await connection.QueryAsync<TestClass>($"{Query} where id = @id", ("id", 1, DbType.Int32)).ToListAsync();
            // switch position
            var result2 = await connection.QueryAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", DbType.String), ("id", 1, DbType.Int32)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

    }
}