using Npgsql;
using Xunit;
using Norm;
using System.Linq;
using System;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class UnknownResultTypeTests
    {
        private readonly PostgreSqlFixture fixture;

        public UnknownResultTypeTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void TypeMistmach_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            Assert.Throws<InvalidCastException>(() =>
            {
                var i = connection.Read<string>("select 1").Single();
            });
        }

        [Fact]
        public void AllUnknownResultTypes1_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var i = connection
                .WithUnknownResultType()
                .Read<string>("select 1").Single();

            Assert.Equal("1", i);
        }

        [Fact]
        public void AllUnknownResultTypes5_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = "select 1::int, true::bool, '1977-05-19'::date, 3.14::numeric, '{\"x\": \"y\"}'::json";
            var (@int, @bool, @date, @num, @json) = connection
                .WithUnknownResultType()
                .Read<string, string, string, string, string>(sql).Single();

            Assert.Equal("1", @int);
            Assert.Equal("t", @bool);
            Assert.Equal("1977-05-19", @date);
            Assert.Equal("3.14", @num);
            Assert.Equal("{\"x\": \"y\"}", @json);
        }

        [Fact]
        public void SomeUnknownResultTypes_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = "select 1::int, true::bool, '1977-05-19'::date, 3.14::numeric, '{\"x\": \"y\"}'::json";
            var (@int, @bool, @date, @num, @json) = connection
                .WithUnknownResultType(true, false, true, false, true)
                .Read<string, bool, string, decimal, string>(sql).Single();

            Assert.Equal("1", @int);
            Assert.Equal(true, @bool);
            Assert.Equal("1977-05-19", @date);
            Assert.Equal(3.14m, @num);
            Assert.Equal("{\"x\": \"y\"}", @json);
        }
    }
}
