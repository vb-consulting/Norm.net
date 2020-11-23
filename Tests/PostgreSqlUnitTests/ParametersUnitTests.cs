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
            var (s, i, b, d, @null) = connection.Single<string, int, bool, DateTime, string>(
                "select @s, @i, @b, @d, @null", "str", 999, true, new DateTime(1977, 5, 19), null);

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void NamedParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection.Single<string, int, bool, DateTime, string>(
                "select @s, @i, @b, @d, @null",
                ("d", new DateTime(1977, 5, 19)), ("b", true), ("i", 999), ("s", "str"), ("null", null));

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
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

        [Fact]
        public void Nullable_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            long? one = 1;
            long? two = null;

            var (result1, result2) = connection.Single<long?, long?>(
                "select @one, @two", ("one", one), ("two", two));

            Assert.Equal(one, result1);
            Assert.Equal(two, result2);
        }

        [Fact]
        public void Undetermined_Param_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            long? id = null;

            var result = connection.Read<long>("select * from (values (1),(2),(3)) t(id) where @id is null or id = @id",
                ("id", id, DbType.Int64)).ToList();
            //new NpgsqlParameter("id", id.HasValue ? id as object : DBNull.Value) { DbType = DbType.Int64 }).ToList();
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Array_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection
                .Execute(@"
                    create function array_params_test(_p int[]) returns int[] as
                    $$
                    begin
                        return _p;
                    end
                    $$
                    language plpgsql");

            var result = connection
                .AsProcedure()
                .Single<int[]>("array_params_test", ("_p", new[] { 3, 6, 9 }));

            Assert.Equal(new[] { 3, 6, 9 }, result);
        }

        [Fact]
        public void Array_Params_Types_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i, j) = connection.Single<int, string>("select @i, @j->>'test'", 
                ("i", 1, NpgsqlDbType.Integer), ("j", "{\"test\": \"value\"}", NpgsqlDbType.Json));
            Assert.Equal(1, i);
            Assert.Equal("value", j);
        }

        [Fact]
        public void Array_Params_Mixed_Types_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i, j) = connection.Single<int, string>("select @i, @j->>'test'",
                ("i", 1, DbType.Int32), ("j", "{\"test\": \"value\"}", NpgsqlDbType.Json));
            Assert.Equal(1, i);
            Assert.Equal("value", j);
        }
    }
}
