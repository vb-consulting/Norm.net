using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Norm;
using Xunit;

namespace OracleUnitTests
{
    [Collection("OracleDatabase")]
    public class QueryUnitTests
    {
        private readonly OracleSqlFixture fixture;

        class TestClass
        {
            public decimal Id { get; private set; }
            public string Foo { get; private set; }
            public DateTime? Day { get; private set; }
            public string Bar { get; private set; }
        }

        private const string Query = @"
            SELECT CAST(1 AS INT) AS id, 'foo1' AS foo, TO_DATE('1977-05-19', 'YYYY-MM-DD') AS day, null AS bar FROM DUAL
            UNION ALL
            SELECT CAST(2 AS INT) AS id, 'foo2' AS foo, TO_DATE('1978-05-19', 'YYYY-MM-DD') AS day, 'bar2' AS bar FROM DUAL
            UNION ALL
            SELECT CAST(3 AS INT) AS id, 'foo3' AS foo, null AS day, 'bar3' AS bar FROM DUAL";

        public QueryUnitTests(OracleSqlFixture fixture)
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
            Assert.Null(result[2].Day);

            Assert.Null(result[0].Bar);
            Assert.Equal("bar2", result[1].Bar);
            Assert.Equal("bar3", result[2].Bar);
        }

        [Fact]
        public void Query_Sync()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result = connection.Read<TestClass>(Query).ToList();
            AssertTestClass(result);
        }

        /*
        [Fact]
        public void Query_Param1_Sync()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result1 = connection.Read<TestClass>($"{Query} where id = @id", 1).ToList();
            var result2 = connection.Read<TestClass>($"{Query} where id = @id and foo = @foo", 1, "foo1").ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param1__SqlParam_Sync()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result1 = connection.Read<TestClass>(
                $"{Query} where id = @id",
                new OracleParameter("id", 1)).ToList();

            // switch position
            var result2 = connection.Read<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                new OracleParameter("foo", "foo1"),
                new OracleParameter("id", 1)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param2_Sync()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result1 = connection.Read<TestClass>($"{Query} where id = @id", ("id", 1)).ToList();
            // switch position
            var result2 = connection.Read<TestClass>($"{Query} where id = @id and foo = @foo", ("foo", "foo1"), ("id", 1)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param3_Sync()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
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
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result1 = connection.Read<TestClass>($"{Query} where id = @id", ("id", 1, OracleDbType.Integer)).ToList();
            // switch position
            var result2 = connection.Read<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", OracleDbType.Varchar), ("id", 1, OracleDbType.Integer)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param4_Mixed_Sync()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result2 = connection.Read<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", OracleDbType.Varchar), ("id", 1, DbType.Int32)).ToList();
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Async()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<TestClass>(Query).ToListAsync();
            AssertTestClass(result);
        }

        [Fact]
        public async Task Query_Param1_Async()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<TestClass>($"{Query} where id = @id", 1).ToListAsync();
            var result2 = await connection.ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo", 1, "foo1").ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param1__SqlParam_Async()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<TestClass>(
                $"{Query} where id = @id",
                new OracleParameter("id", 1)).ToListAsync();

            // switch position
            var result2 = await connection.ReadAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                new OracleParameter("foo", "foo1"),
                new OracleParameter("id", 1)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param2_Async()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<TestClass>($"{Query} where id = @id", ("id", 1)).ToListAsync();
            // switch position
            var result2 = await connection.ReadAsync<TestClass>($"{Query} where id = @id and foo = @foo", ("foo", "foo1"), ("id", 1)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param3_Async()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
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
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<TestClass>($"{Query} where id = @id", ("id", 1, OracleDbType.Integer)).ToListAsync();
            // switch position
            var result2 = await connection.ReadAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", OracleDbType.Varchar), ("id", 1, OracleDbType.Integer)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param4_Mixed_Async()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            var result2 = await connection.ReadAsync<TestClass>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", OracleDbType.Varchar), ("id", 1, DbType.Int32)).ToListAsync();
            AssertSingleTestClass(result2);
        }
        */
    }
}
