﻿using System;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Xunit;

namespace SQLiteUnitTests
{
    [Collection("SQLiteDatabase")]
    public class SingleTuplesUnitTests
    {
        private readonly SqLiteFixture fixture;

        private const string Query = "select 1, 'foo1', date('1977-05-19'), true, null";

        public SingleTuplesUnitTests(SqLiteFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Single_Value_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Read<long>(Query).Single();
            Assert.Equal(1, result);
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = connection.Read<long?>("select null").Single();
            Assert.Null(result);
        }

        [Fact]
        public async Task Null_Value_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<long?>("select null").SingleAsync();
            Assert.Null(result);
        }

        [Fact]
        public void Two_Tuples_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (r1, r2) = connection.Read<long, string>(Query).Single();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
        }

        [Fact]
        public async Task Single_Value_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<long>(Query).SingleAsync();
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Two_Tuples_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (r1, r2) = await connection.ReadAsync<long, string>(Query).SingleAsync();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
        }

        [Fact]
        public void Three_Tuples_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (r1, r2, r3) = connection.Read<long, string, string>(Query).Single();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal("1977-05-19", r3);
        }

        [Fact]
        public void Four_Tuples_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4) = connection.Read<long, string, string, long>(Query).Single();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal("1977-05-19", r3);
            Assert.Equal(1, r4);
        }

        [Fact]
        public void Five_Tuples_Test_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4, r5) = connection.Read<long, string, string, long, string>(Query).Single();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal("1977-05-19", r3);
            Assert.Equal(1, r4);
            Assert.Null(r5);
        }

        [Fact]
        public async Task Three_Tuples_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (r1, r2, r3) = await connection.ReadAsync<long, string, string>(Query).SingleAsync();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal("1977-05-19", r3);
        }

        [Fact]
        public async Task Four_Tuples_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4) = await connection.ReadAsync<long, string, string, long>(Query).SingleAsync();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal("1977-05-19", r3);
            Assert.Equal(1, r4);
        }

        [Fact]
        public async Task Five_Tuples_Test_Async()
        {
            await using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (r1, r2, r3, r4, r5) = await connection.ReadAsync<long, string, string, long, string>(Query).SingleAsync();
            Assert.Equal(1, r1);
            Assert.Equal("foo1", r2);
            Assert.Equal("1977-05-19", r3);
            Assert.Equal(1, r4);
            Assert.Null(r5);
        }
    }
}
