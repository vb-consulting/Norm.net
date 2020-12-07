using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using NpgsqlTypes;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class InsertUnitTests
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

        private const string CreateTable = "create table testclass (id integer, foo varchar, day date, bool boolean, bar varchar)";

        public InsertUnitTests(PostgreSqlFixture fixture)
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

        [Fact]
        public void Insert_Values_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            try
            {
                connection
                    .Execute("begin")
                    .Execute(CreateTable);

                connection.Insert<TestClass>()
                    .Values(1, "foo1", new DateTime(1977, 5, 19), true, null)
                    .Values(2, "foo2", new DateTime(1978, 5, 19), false, "bar2")
                    .Values(3, "foo3", new DateTime(1979, 5, 19), null, "bar3")
                    .Execute();

                var result = connection.Select<TestClass>().ToList();
                AssertTestClass(result);
            }
            finally
            {
                connection.Execute("rollback");
            }
        }

        [Fact]
        public void Insert_Named_Values_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            try
            {
                connection
                    .Execute("begin")
                    .Execute(CreateTable);

                connection.Insert<TestClass>()
                    .Values(("foo", "foo1"), ("day", new DateTime(1977, 5, 19)), ("bool", true), ("bar", null), ("id", 1))
                    .Values(("foo", "foo2"), ("day", new DateTime(1978, 5, 19)), ("bool", false), ("bar", "bar2"), ("id", 2))
                    .Values(("foo", "foo3"), ("day", new DateTime(1979, 5, 19)), ("bool", null), ("bar", "bar3"), ("id", 3))
                    .Execute();

                var result = connection.Select<TestClass>().ToList();
                AssertTestClass(result);
            }
            finally
            {
                connection.Execute("rollback");
            }
        }

        [Fact]
        public void Insert_Instances_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            try
            {
                connection
                    .Execute("begin")
                    .Execute(CreateTable);

                connection.Insert<TestClass>()
                    .Values(new TestClass { Id = 1, Foo = "foo1", Day = new DateTime(1977, 5, 19), Bool = true, Bar = null})
                    .Values(new TestClass { Id = 2, Foo = "foo2", Day = new DateTime(1978, 5, 19), Bool = false, Bar = "bar2" })
                    .Values(new TestClass { Id = 3, Foo = "foo3", Day = new DateTime(1979, 5, 19), Bool = null, Bar = "bar3" })
                    .Execute();

                var result = connection.Select<TestClass>().ToList();
                AssertTestClass(result);
            }
            finally
            {
                connection.Execute("rollback");
            }
        }

        [Fact]
        public void Insert_Mixed_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            try
            {
                connection
                    .Execute("begin")
                    .Execute(CreateTable);

                connection.Insert<TestClass>()
                    .Values(1, "foo1", new DateTime(1977, 5, 19), true, null)
                    .Values(("id", 2), ("foo", "foo2"), ("day", new DateTime(1978, 5, 19)), ("bool", false), ("bar", "bar2"))
                    .Values(new TestClass { Id = 3, Foo = "foo3", Day = new DateTime(1979, 5, 19), Bool = null, Bar = "bar3" })
                    .Execute();

                var result = connection.Select<TestClass>().ToList();
                AssertTestClass(result);
            }
            finally
            {
                connection.Execute("rollback");
            }
        }


        [Fact]
        public void Insert_Specific_Fields_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            try
            {
                connection
                    .Execute("begin")
                    .Execute("create table testclass(id integer generated always as identity (start with 1 increment by 1), foo varchar, day date, bool boolean, bar varchar)");

                connection.Insert<TestClass>("foo", "day", "bool", "bar")
                    .Values("foo1", new DateTime(1977, 5, 19), true, null)
                    .Values("foo2", new DateTime(1978, 5, 19), false, "bar2")
                    .Values("foo3", new DateTime(1979, 5, 19), null, "bar3")
                    .Execute();

                var result = connection.Select<TestClass>().ToList();
                AssertTestClass(result);
            }
            finally
            {
                connection.Execute("rollback");
            }
        }

        [Fact]
        public void Insert_Returning_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            try
            {
                connection
                    .Execute("begin")
                    .Execute("create table testclass(id integer generated always as identity (start with 1 increment by 1), foo varchar, day date, bool boolean, bar varchar)");

                var result = connection.Insert<TestClass>("foo", "day", "bool", "bar")
                    .Values(new TestClass { Foo = "foo1", Day = new DateTime(1977, 5, 19), Bool = true, Bar = null })
                    .Values(new TestClass { Foo = "foo2", Day = new DateTime(1978, 5, 19), Bool = false, Bar = "bar2" })
                    .Values(new TestClass { Foo = "foo3", Day = new DateTime(1979, 5, 19), Bool = null, Bar = "bar3" })
                    .Returning()
                    .ToList();

                AssertTestClass(result);
            }
            finally
            {
                connection.Execute("rollback");
            }
        }
    }
}
