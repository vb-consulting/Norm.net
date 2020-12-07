using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm.Extensions;
using Npgsql;
using NpgsqlTypes;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class SelectUnitTests
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
                            into temp TestClass
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(Id, Foo, Day, Bool, Bar)";


        public SelectUnitTests(PostgreSqlFixture fixture)
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
        public void Get_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute(Query);
            var result = connection.Select<TestClass>().ToList();
            AssertTestClass(result);
        }

        [Fact]
        public void Get_Param1_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute(Query);
            var result1 = connection.Select<TestClass>(1).ToList();
            var result2 = connection.Select<TestClass>(1, "foo1").ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param2_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute(Query);
            var result1 = connection.Select<TestClass>(("id", 1)).ToList();
            // switch position
            var result2 = connection.Select<TestClass>(("foo", "foo1"), ("id", 1)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Get_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute(Query);
            var result = await connection.SelectAsync<TestClass>().ToListAsync();
            AssertTestClass(result);
        }

        [Fact]
        public async Task Get_Param1_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute(Query);
            var result1 = await connection.SelectAsync<TestClass>(1).ToListAsync();
            var result2 = await connection.SelectAsync<TestClass>(1, "foo1").ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param2_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute(Query);
            var result1 = await connection.SelectAsync<TestClass>(("id", 1)).ToListAsync();
            // switch position
            var result2 = await connection.SelectAsync<TestClass>(("foo", "foo1"), ("id", 1)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param2__NameOf_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute(Query);
            var result1 = await connection.SelectAsync<TestClass>((nameof(TestClass.Id), 1)).ToListAsync();
            // switch position
            var result2 = await connection.SelectAsync<TestClass>((nameof(TestClass.Foo), "foo1"), (nameof(TestClass.Id), 1)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }
    }
}
