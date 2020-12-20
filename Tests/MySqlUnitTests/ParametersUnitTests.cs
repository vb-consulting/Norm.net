using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using Norm;
using Xunit;

namespace MySqlUnitTests
{
    [Collection("MySqlDatabase")]
    public class ParametersUnitTests
    {
        private readonly MySqlFixture fixture;

        public ParametersUnitTests(MySqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void PositionalParams_Test()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection.Read<string, long, long, DateTime, string>(
                "select @s, @i, @b, @d, @null", "str", (long)999, Convert.ToInt64(true), new DateTime(1977, 5, 19), null)
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(Convert.ToBoolean(b));
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void NamedParams_Test()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection.Read<string, long, long, DateTime, string>(
                "select @s, @i, @b, @d, @null",
                ("d", new DateTime(1977, 5, 19)), ("b", Convert.ToInt64(true)), ("i", (long)999), ("s", "str"), ("null", null))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(Convert.ToBoolean(b));
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void DbParams_Test()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Read<string, long, long, DateTime>(
                "select @s, @i, @b, @d",
                new MySqlParameter("s", "str"),
                new MySqlParameter("i", 999),
                new MySqlParameter("b", true),
                new MySqlParameter("d", new DateTime(1977, 5, 19)))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(Convert.ToBoolean(b));
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection.Read<string, long, long, DateTime>(
                "select @s, @i, @b, @d",
                new MySqlParameter("d", new DateTime(1977, 5, 19)),
                new MySqlParameter("b", true),
                new MySqlParameter("i", 999),
                new MySqlParameter("s", "str"))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(Convert.ToBoolean(b));
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void MixedParams_Test()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Read<string, long, long, DateTime>(
                "select @s, @i, @b, @d",
                new MySqlParameter("d", new DateTime(1977, 5, 19)), "str", 999, true)
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(Convert.ToBoolean(b));
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection.Read<string, long, long, DateTime>(
                "select @s, @i, @b, @d",
                new MySqlParameter("s", "str"), new MySqlParameter("i", 999), true, new DateTime(1977, 5, 19))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(Convert.ToBoolean(b));
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void InputOutput_Parameters_Procedure_Test()
        {
            var name = "inputoutput_parameters_procedure_test";
            var p = new MySqlParameter("test_param", "I am output value") { Direction = ParameterDirection.InputOutput };

            using var connection = new MySqlConnection(fixture.ConnectionString);
            new MySqlScript(connection)
            {
                Query = @$"
                delimiter $$
                create procedure {name}(inout test_param text)
                begin
                    set test_param = concat(test_param, ' returned from procedure');
                    select @test_param;
                end$$
                delimiter ;
            "
            }.Execute();

            connection
                .AsProcedure()
                .Execute(name, p);

            Assert.Equal("I am output value returned from procedure", p.Value);
        }

        [Fact]
        public void Nullable_Params_Test()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            long? one = 1;
            long? two = null;

            var (result1, result2) = connection.Read<long?, long?>(
                "select @one, @two", ("one", one), ("two", two))
                .Single();

            Assert.Equal(one, result1);
            Assert.Equal(two, result2);
        }

        [Fact]
        public void Undetermined_Param_Test()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            long? id = null;

            var result = connection.Read<long>(@" 
                select * 
                from (
                    select 1 as id union all select 2 as id union all select 3 as id
                ) t
                where @id is null or id = @id",
                ("id", id, DbType.Int64)).ToList();
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Custom_Params_Types_Test()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            var (i, j) = connection.Read<long, string>("select @i, json_unquote(json_extract(@j, '$.test'))", 
                ("i", 1, MySqlDbType.Int32), ("j", "{\"test\": \"value\"}", MySqlDbType.JSON))
                .Single();
            Assert.Equal(1, i);
            Assert.Equal("value", j);
        }

        [Fact]
        public void Custom_Params_Mixed_Types_Test()
        {
            using var connection = new MySqlConnection(fixture.ConnectionString);
            var (i, j) = connection.Read<long, string>("select @i, json_unquote(json_extract(@j, '$.test'))",
                ("i", 1, DbType.Int32), ("j", "{\"test\": \"value\"}", MySqlDbType.JSON))
                .Single();
            Assert.Equal(1, i);
            Assert.Equal("value", j);
        }
    }
}
