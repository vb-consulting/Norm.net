using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class DynamicMapUnitTests
    {
        private readonly PostgreSqlFixture fixture;

      
        public DynamicMapUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Map_Dynamic_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<dynamic>(@"select *
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(id, foo, day, bool, foo_bar)")
                .ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0].id);
            Assert.Equal(2, result[1].id);
            Assert.Equal(3, result[2].id);

            Assert.Equal("foo1", result[0].foo);
            Assert.Equal("foo2", result[1].foo);
            Assert.Equal("foo3", result[2].foo);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].day);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].day);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].day);

            Assert.Equal(true, result[0].@bool);
            Assert.Equal(false, result[1].@bool);
            Assert.Equal(null, result[2].@bool);

            Assert.Equal(null, result[0].foobar);
            Assert.Equal("bar2", result[1].foobar);
            Assert.Equal("bar3", result[2].foobar);
        }

        public class Class1
        {
            public int Id { get; set; }
            public string Foo { get; set; }
        }

        [Fact]
        public void Map_Dynamic_Mixed_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<Class1, dynamic>(@"select *
                            from (
                            values 
                                (1, 'foo1', '1977-05-19'::date, true, null),
                                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                                (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                            ) t(id, foo, day, bool, foo_bar)")
                .ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0].Item1.Id);
            Assert.Equal(2, result[1].Item1.Id);
            Assert.Equal(3, result[2].Item1.Id);

            Assert.Equal("foo1", result[0].Item1.Foo);
            Assert.Equal("foo2", result[1].Item1.Foo);
            Assert.Equal("foo3", result[2].Item1.Foo);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Item2.day);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Item2.day);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Item2.day);

            Assert.Equal(true, result[0].Item2.@bool);
            Assert.Equal(false, result[1].Item2.@bool);
            Assert.Equal(null, result[2].Item2.@bool);

            Assert.Equal(null, result[0].Item2.foobar);
            Assert.Equal("bar2", result[1].Item2.foobar);
            Assert.Equal("bar3", result[2].Item2.foobar);
        }

    }
}
