using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class ReadInstanceUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public ReadInstanceUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ReadAnonymous_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new 
            { 
                first = default(int), 
                bar = default(string), 
                day = default(DateTime?),
                @bool = default(bool?),
                s = default(string),
            }, @"

                select * from (
                    values 
                    (1, 'foo1', '1977-05-19'::date, true, null),
                    (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                    (3, 'foo3', null::date, null, 'bar3')
                ) t(first, bar, day, bool, s)
                
            ").ToList();

            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].first);

            Assert.Equal("foo1", result[0].bar);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].day);
            Assert.Equal(true, result[0].@bool);
            Assert.Null(result[0].s);

            Assert.Equal(2, result[1].first);
            Assert.Equal("foo2", result[1].bar);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].day);
            Assert.Equal(false, result[1].@bool);
            Assert.Equal("bar2", result[1].s);

            Assert.Equal(3, result[2].first);
            Assert.Equal("foo3", result[2].bar);
            Assert.Null(result[2].day);
            Assert.Null(result[2].@bool);
            Assert.Equal("bar3", result[2].s);
        }

        [Fact]
        public void ReadAnonymous_Unordered_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                s = default(string),
                bar = default(string),
                @bool = default(bool?),
                first = default(int),
                day = default(DateTime?),
                missing = default(string),
            }, @"

                select * from (
                    values 
                    (1, 'foo1', '1977-05-19'::date, true, null, 'x'),
                    (2, 'foo2', '1978-05-19'::date, false, 'bar2', 'x'),
                    (3, 'foo3', null::date, null, 'bar3', 'x')
                ) t(first, bar, day, bool, s, x)
                
            ").ToList();

            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].first);

            Assert.Equal("foo1", result[0].bar);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].day);
            Assert.Equal(true, result[0].@bool);
            Assert.Null(result[0].s);
            Assert.Null(result[0].missing);

            Assert.Equal(2, result[1].first);
            Assert.Equal("foo2", result[1].bar);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].day);
            Assert.Equal(false, result[1].@bool);
            Assert.Equal("bar2", result[1].s);
            Assert.Null(result[1].missing);

            Assert.Equal(3, result[2].first);
            Assert.Equal("foo3", result[2].bar);
            Assert.Null(result[2].day);
            Assert.Null(result[2].@bool);
            Assert.Equal("bar3", result[2].s);
            Assert.Null(result[2].missing);
        }


    }
}
