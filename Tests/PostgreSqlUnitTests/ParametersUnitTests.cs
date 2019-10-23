using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using Norm.Extensions;
using Npgsql;
using NpgsqlTypes;
using PostgreSqlUnitTests;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class ParametersUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public ParametersUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void PositionalParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Single<string, int, bool, DateTime>(
                "select @s, @i, @b, @d", "str", 999, true, new DateTime(1977, 5, 19));

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void NamedParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Single<string, int, bool, DateTime>(
                "select @s, @i, @b, @d", 
                ("d", new DateTime(1977, 5, 19)), ("b", true), ("i", 999), ("s", "str"));

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void DbParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Single<string, int, bool, DateTime>(
                "select @s, @i, @b, @d", 
                new NpgsqlParameter("s", "str"), 
                new NpgsqlParameter("i", 999), 
                new NpgsqlParameter("b", true), 
                new NpgsqlParameter("d", new DateTime(1977, 5, 19)));

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection.Single<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new NpgsqlParameter("d", new DateTime(1977, 5, 19)),
                new NpgsqlParameter("b", true),
                new NpgsqlParameter("i", 999),
                new NpgsqlParameter("s", "str"));

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void MixedParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Single<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new NpgsqlParameter("d", new DateTime(1977, 5, 19)), "str", 999, true);

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection.Single<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new NpgsqlParameter("s", "str"), new NpgsqlParameter("i", 999), true, new DateTime(1977, 5, 19));

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void PgArrayParam_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var p = new NpgsqlParameter("p", NpgsqlDbType.Array | NpgsqlDbType.Integer)
            {
                Value = new List<int> { 1, 2, 3 }
            };
            var result = connection.Read<int>("select unnest(@p)", p).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
            Assert.Equal(3, result[2]);
        }

        [Fact]
        public void InputOutput_Parameters_Function_Test()
        {
            var p = new NpgsqlParameter("test_param", "I am output value") { Direction = ParameterDirection.InputOutput };

            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection
                .Execute(@"
                    create function test_inout_param_func(inout test_param text) returns text as
                    $$
                    begin
                        test_param := test_param || ' returned from function';
                    end
                    $$
                    language plpgsql")
                .AsProcedure()
                //.WithOutParameter("test_param", "I am output value")
                .Execute("test_inout_param_func", p);

            Assert.Equal("I am output value returned from function", p.Value);
        }
    }
}
