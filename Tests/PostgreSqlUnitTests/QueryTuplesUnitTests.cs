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
    public class QueryTuplesUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        private const string Query = @"
                            select *
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(id, foo, day, bool, bar)";


        public QueryTuplesUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        private void AssertTestClass(IList<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)> result)
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

        private void AssertSingleTestClass(IList<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)> result)
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
            var result = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(Query).ToList();
            AssertTestClass(result);
        }

        [Fact]
        public void Query_Param1_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id", 1).ToList();
            var result2 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id and foo = @foo", 1, "foo1").ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param1__SqlParam_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
                $"{Query} where id = @id",
                new NpgsqlParameter("id", 1)).ToList();

            // switch position
            var result2 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
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
            var result1 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id", ("id", 1)).ToList();
            // switch position
            var result2 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id and foo = @foo", ("foo", "foo1"), ("id", 1)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param3_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id", ("id", 1, DbType.Int32)).ToList();
            // switch position
            var result2 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
                $"{Query} where id = @id and foo = @foo", 
                ("foo", "foo1", DbType.String), ("id", 1, DbType.Int32)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param4_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id", ("id", 1, NpgsqlDbType.Integer)).ToList();
            // switch position
            var result2 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", NpgsqlDbType.Varchar), ("id", 1, NpgsqlDbType.Integer)).ToList();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Query_Param4_Mixed_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result2 = connection.Read<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", NpgsqlDbType.Varchar), ("id", 1, DbType.Int32)).ToList();
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(Query).ToListAsync();
            AssertTestClass(result);
        }

        [Fact]
        public async Task Query_Param1_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id", 1).ToListAsync();
            var result2 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id and foo = @foo", 1, "foo1").ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param1__SqlParam_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
                $"{Query} where id = @id",
                new NpgsqlParameter("id", 1)).ToListAsync();

            // switch position
            var result2 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
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
            var result1 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id", ("id", 1)).ToListAsync();
            // switch position
            var result2 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id and foo = @foo", ("foo", "foo1"), ("id", 1)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param3_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id", ("id", 1, DbType.Int32)).ToListAsync();
            // switch position
            var result2 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", DbType.String), ("id", 1, DbType.Int32)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param4_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>($"{Query} where id = @id", ("id", 1, NpgsqlDbType.Integer)).ToListAsync();
            // switch position
            var result2 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", NpgsqlDbType.Varchar), ("id", 1, NpgsqlDbType.Integer)).ToListAsync();
            AssertSingleTestClass(result1);
            AssertSingleTestClass(result2);
        }

        [Fact]
        public async Task Query_Param4_Mixed_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result2 = await connection.ReadAsync<(int Id, string Foo, DateTime Day, bool? Bool, string Bar)>(
                $"{Query} where id = @id and foo = @foo",
                ("foo", "foo1", NpgsqlDbType.Varchar), ("id", 1, DbType.Int32)).ToListAsync();
            AssertSingleTestClass(result2);
        }

        [Fact]
        public void Six_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6)>("select 1, 2, 3, 4, 5, 6").Single();

            Assert.Equal(1, t.i1);
            Assert.Equal(2, t.i2);
            Assert.Equal(3, t.i3);
            Assert.Equal(4, t.i4);
            Assert.Equal(5, t.i5);
            Assert.Equal(6, t.i6);
        }


        [Fact]
        public void Seven_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6, int i7)>("select 1, 2, 3, 4, 5, 6, 7").Single();

            Assert.Equal(1, t.i1);
            Assert.Equal(2, t.i2);
            Assert.Equal(3, t.i3);
            Assert.Equal(4, t.i4);
            Assert.Equal(5, t.i5);
            Assert.Equal(6, t.i6);
            Assert.Equal(7, t.i7);
        }

        [Fact]
        public void Eight_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8)>("select 1, 2, 3, 4, 5, 6, 7, 8").Single();

            Assert.Equal(1, t.i1);
            Assert.Equal(2, t.i2);
            Assert.Equal(3, t.i3);
            Assert.Equal(4, t.i4);
            Assert.Equal(5, t.i5);
            Assert.Equal(6, t.i6);
            Assert.Equal(7, t.i7);
            Assert.Equal(8, t.i8);
        }

        [Fact]
        public void Nine_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9)>("select 1, 2, 3, 4, 5, 6, 7, 8, 9").Single();

            Assert.Equal(1, t.i1);
            Assert.Equal(2, t.i2);
            Assert.Equal(3, t.i3);
            Assert.Equal(4, t.i4);
            Assert.Equal(5, t.i5);
            Assert.Equal(6, t.i6);
            Assert.Equal(7, t.i7);
            Assert.Equal(8, t.i8);
            Assert.Equal(9, t.i9);
        }

        [Fact]
        public void Ten_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9, int i10)>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10").Single();

            Assert.Equal(1, t.i1);
            Assert.Equal(2, t.i2);
            Assert.Equal(3, t.i3);
            Assert.Equal(4, t.i4);
            Assert.Equal(5, t.i5);
            Assert.Equal(6, t.i6);
            Assert.Equal(7, t.i7);
            Assert.Equal(8, t.i8);
            Assert.Equal(9, t.i9);
            Assert.Equal(10, t.i10);
        }

        [Fact]
        public void Eleven_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9, int i10, int i11)>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11").Single();

            Assert.Equal(1, t.i1);
            Assert.Equal(2, t.i2);
            Assert.Equal(3, t.i3);
            Assert.Equal(4, t.i4);
            Assert.Equal(5, t.i5);
            Assert.Equal(6, t.i6);
            Assert.Equal(7, t.i7);
            Assert.Equal(8, t.i8);
            Assert.Equal(9, t.i9);
            Assert.Equal(10, t.i10);
            Assert.Equal(11, t.i11);
        }

        [Fact]
        public void Twelve_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9, int i10, int i11, int i12)>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12").Single();

            Assert.Equal(1, t.i1);
            Assert.Equal(2, t.i2);
            Assert.Equal(3, t.i3);
            Assert.Equal(4, t.i4);
            Assert.Equal(5, t.i5);
            Assert.Equal(6, t.i6);
            Assert.Equal(7, t.i7);
            Assert.Equal(8, t.i8);
            Assert.Equal(9, t.i9);
            Assert.Equal(10, t.i10);
            Assert.Equal(11, t.i11);
            Assert.Equal(12, t.i12);
        }

        [Fact]
        public void Thirteen_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9, int i10, int i11, int i12, int i13)>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13").Single();

            Assert.Equal(1, t.i1);
            Assert.Equal(2, t.i2);
            Assert.Equal(3, t.i3);
            Assert.Equal(4, t.i4);
            Assert.Equal(5, t.i5);
            Assert.Equal(6, t.i6);
            Assert.Equal(7, t.i7);
            Assert.Equal(8, t.i8);
            Assert.Equal(9, t.i9);
            Assert.Equal(10, t.i10);
            Assert.Equal(11, t.i11);
            Assert.Equal(12, t.i12);
            Assert.Equal(13, t.i13);
        }

        [Fact]
        public void Fourteen_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var t = connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9, int i10, int i11, int i12, int i13, int i14)>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14").Single();

            Assert.Equal(1, t.i1);
            Assert.Equal(2, t.i2);
            Assert.Equal(3, t.i3);
            Assert.Equal(4, t.i4);
            Assert.Equal(5, t.i5);
            Assert.Equal(6, t.i6);
            Assert.Equal(7, t.i7);
            Assert.Equal(8, t.i8);
            Assert.Equal(9, t.i9);
            Assert.Equal(10, t.i10);
            Assert.Equal(11, t.i11);
            Assert.Equal(12, t.i12);
            Assert.Equal(13, t.i13);
            Assert.Equal(14, t.i14);
        }

        [Fact]
        public void Fifteen_Tuples_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            Assert.Throws<ArgumentException>(() => connection.Read<(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9, int i10, int i11, int i12, int i13, int i14, int i15)>("select 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15").Single());
        }
    }
}
