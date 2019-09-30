using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NoOrm;
using NoOrm.Extensions;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class SingleJsonUnitTests
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

        private const string UpperCaseQuery = @"
                            select json_agg(t)
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(""Id"", ""Foo"", ""Day"", ""Bool"", ""Bar"")";

        private const string LowerCaseQuery = @"
                            select json_agg(t)
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(id, foo, day, bool, bar)";

        public SingleJsonUnitTests(PostgreSqlFixture fixture)
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
        public void DeserializeSingle_TestList_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.SingleJson<IList<TestClass>>(UpperCaseQuery);

            AssertTestClass(result);
        }

        [Fact]
        public void DeserializeSingle_TestEnumerable_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.SingleJson<IEnumerable<TestClass>>(UpperCaseQuery).ToList();

            AssertTestClass(result);
        }

        [Fact]
        public void DeserializeSingle_TestJsonOptions_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithJsonOptions(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                .SingleJson<IList<TestClass>>(LowerCaseQuery);

            AssertTestClass(result);
        }

        [Fact]
        public async Task DeserializeSingle_TestList_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.SingleJsonAsync<IList<TestClass>>(UpperCaseQuery);

            AssertTestClass(result);
        }

        [Fact]
        public async Task DeserializeSingle_TestEnumerable_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = (await connection.SingleJsonAsync<IEnumerable<TestClass>>(UpperCaseQuery)).ToList();

            AssertTestClass(result);
        }

        [Fact]
        public async Task DeserializeSingle_TestJsonOptions_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithJsonOptions(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                .SingleJsonAsync<IList<TestClass>>(LowerCaseQuery);

            AssertTestClass(result);
        }
    }
}
