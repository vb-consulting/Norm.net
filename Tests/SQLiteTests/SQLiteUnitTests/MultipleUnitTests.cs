using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Xunit;

namespace SQLiteUnitTests
{
    [Collection("SQLiteDatabase")]
    public class MultipleUnitTests
    {
        public record Record1(long Id1, string Foo1, string Bar1);
        public record Record2(long Id2, string Foo2, string Bar2);

        private readonly SqLiteFixture fixture;
        private readonly string Queires = @"
            select 1 as id1, 'foo1' as foo1, 'bar1' as bar1; 
            select 2 as id2, 'foo2' as foo2, 'bar2' as bar2";
        private readonly string QueiresWithParams = @"
            select @id1 as id1, 'foo1' as foo1, 'bar1' as bar1; 
            select 2 as id2, 'foo2' as foo2, @bar2 as bar2";

        public MultipleUnitTests(SqLiteFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void MapTwoRecords_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            using var multiple = connection.Multiple(Queires);
            
            var result1 = multiple.Read<Record1>().Single();
            var next1 = multiple.Next();
            var result2 = multiple.Read<Record2>().Single();
            var next2 = multiple.Next();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public void MapTwoRecords_PositionalParams_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            using var multiple = connection.WithParameters(1, "bar2").Multiple(QueiresWithParams);

            var result1 = multiple.Read<Record1>().Single();
            var next1 = multiple.Next();
            var result2 = multiple.Read<Record2>().Single();
            var next2 = multiple.Next();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public void MapTwoRecords_NamedParams_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            using var multiple = connection
                .WithParameters(new { bar2 = "bar2", id1 = 1 })
                .Multiple(QueiresWithParams);

            var result1 = multiple.Read<Record1>().Single();
            var next1 = multiple.Next();
            var result2 = multiple.Read<Record2>().Single();
            var next2 = multiple.Next();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public void MapTwoRecords_NamedTypedParams_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            using var multiple = connection
                .WithParameters(new { bar2 = ("bar2", DbType.String), id1 = (1, DbType.Int32) })
                .Multiple(QueiresWithParams);

            var result1 = multiple.Read<Record1>().Single();
            var next1 = multiple.Next();
            var result2 = multiple.Read<Record2>().Single();
            var next2 = multiple.Next();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public async Task MapTwoRecords_Async()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            using var multiple = await connection.MultipleAsync(Queires);

            var result1 = await multiple.ReadAsync<Record1>().SingleAsync();
            var next1 = await multiple.NextAsync();
            var result2 = await multiple.ReadAsync<Record2>().SingleAsync();
            var next2 = await multiple.NextAsync();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public void MapTwoNamedTuples_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            using var multiple = connection.Multiple(Queires);

            var result1 = multiple.Read<(long Id1, string Foo1, string Bar1)>().Single();
            var next1 = multiple.Next();
            var result2 = multiple.Read<(long Id2, string Foo2, string Bar2)>().Single();
            var next2 = multiple.Next();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public async Task MapTwoNamedTuples_Async()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            using var multiple = await connection.MultipleAsync(Queires);

            var result1 = await multiple.ReadAsync<(long Id1, string Foo1, string Bar1)>().SingleAsync();
            var next1 = await multiple.NextAsync();
            var result2 = await multiple.ReadAsync<(long Id2, string Foo2, string Bar2)>().SingleAsync();
            var next2 = await multiple.NextAsync();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public void MapTwoUnnamedTuples_Sync()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            using var multiple = connection.Multiple(Queires);

            var (id1, foo1, bar1) = multiple.Read<long, string, string>().Single();
            var next1 = multiple.Next();
            var (id2, foo2, bar2) = multiple.Read<long, string, string>().Single();
            var next2 = multiple.Next();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, id1);
            Assert.Equal("foo1", foo1);
            Assert.Equal("bar1", bar1);

            Assert.Equal(2, id2);
            Assert.Equal("foo2", foo2);
            Assert.Equal("bar2", bar2);
        }

        [Fact]
        public async Task MapTwoUnnamedTuples_Async()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            using var multiple = await connection.MultipleAsync(Queires);

            var (id1, foo1, bar1) = await multiple.ReadAsync<long, string, string>().SingleAsync();
            var next1 = await multiple.NextAsync();
            var (id2, foo2, bar2) = await multiple.ReadAsync<long, string, string>().SingleAsync();
            var next2 = await multiple.NextAsync();

            Assert.True(next1);
            Assert.False(next2);

            Assert.Equal(1, id1);
            Assert.Equal("foo1", foo1);
            Assert.Equal("bar1", bar1);

            Assert.Equal(2, id2);
            Assert.Equal("foo2", foo2);
            Assert.Equal("bar2", bar2);
        }
    }
}
