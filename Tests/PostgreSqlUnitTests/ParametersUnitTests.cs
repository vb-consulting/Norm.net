using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Norm;
using Npgsql;
using NpgsqlTypes;
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
            var (s, i, b, d, @null) = connection.Read<string, int, bool, DateTime, string>(
                "select @s, @i, @b, @d, @null", 
                "str", 999, true, new DateTime(1977, 5, 19), null)
                .Single();

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
            var (s, i, b, d, @null) = connection.Read<string, int, bool, DateTime, string>(
                "select @s, @i, @b, @d, @null",
                new
                {
                    d = new DateTime(1977, 5, 19),
                    b = true,
                    i = 999,
                    s = "str",
                    @null = (string)null
                })
                .Single();

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
            var (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new NpgsqlParameter("s", "str"),
                new NpgsqlParameter("i", 999),
                new NpgsqlParameter("b", true),
                new NpgsqlParameter("d", new DateTime(1977, 5, 19)))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new NpgsqlParameter("d", new DateTime(1977, 5, 19)),
                new NpgsqlParameter("b", true),
                new NpgsqlParameter("i", 999),
                new NpgsqlParameter("s", "str"))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void MixedParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new NpgsqlParameter("d", new DateTime(1977, 5, 19)), "str", 999, true)
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new NpgsqlParameter("s", "str"), new NpgsqlParameter("i", 999), true, new DateTime(1977, 5, 19))
                .Single();

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
                    create function test_inout_param_func_1(inout test_param text) returns text as
                    $$
                    begin
                        test_param := test_param || ' returned from function';
                    end
                    $$
                    language plpgsql")
                .AsProcedure()
                //.WithOutParameter("test_param", "I am output value")
                .Execute("test_inout_param_func_1", p);

            Assert.Equal("I am output value returned from function", p.Value);
        }

        [Fact]
        public void Nullable_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            long? one = 1;
            long? two = null;

            var (result1, result2) = connection.Read<long?, long?>(
                "select @one, @two", 
                new
                {
                    one, two
                })
                .Single();

            Assert.Equal(one, result1);
            Assert.Equal(two, result2);
        }

        [Fact]
        public void Undetermined_Param_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            long? id = null;

            var result = connection.Read<long>("select * from (values (1),(2),(3)) t(id) where @id is null or id = @id",
                new { id = (id, DbType.Int64) }).ToList();
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
                .Read<int[]>("array_params_test",
                new
                {
                    _p = new[] { 3, 6, 9 }
                })
                .Single();

            Assert.Equal(new[] { 3, 6, 9 }, result);
        }

        [Fact]
        public void Custom_Params_Types_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i, j) = connection.Read<int, string>("select @i, @j->>'test'", 
                new
                {
                    i = (1, NpgsqlDbType.Integer),
                    j = ("{\"test\": \"value\"}", NpgsqlDbType.Json)
                })
                .Single();
            Assert.Equal(1, i);
            Assert.Equal("value", j);
        }

        [Fact]
        public void Custom_Params_Mixed_Types_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i, j) = connection.Read<int, string>("select @i, @j->>'test'",
                new
                {
                    i = (1, DbType.Int32),
                    j = ("{\"test\": \"value\"}", NpgsqlDbType.Json),
                })
                .Single();
            Assert.Equal(1, i);
            Assert.Equal("value", j);
        }

        [Fact]
        public void Custom_Params_Aray_Types_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int>("select unnest(@p)", 
                new
                {
                    p = (new List<int> { 1, 2, 3 }, NpgsqlDbType.Array | NpgsqlDbType.Integer)
                }).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
            Assert.Equal(3, result[2]);
        }

        class PocoClassParams
        {
            public string StrValue { get; set; } = "str";
            public int IntValue { get; set; } = 999;
            public bool BoolValue { get; set; } = true;
            public DateTime DateTimeValue { get; set; } = new DateTime(1977, 5, 19);
            public string NullValue { get; set; } = null;
        }
        
        [Fact]
        public void PocoClassParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection.Read<string, int, bool, DateTime, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue", new PocoClassParams())
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }
        
        [Fact]
        public void PocoClassParams_Positional_Mixed_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null, p1) = connection.Read<string, int, bool, DateTime, string, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1", 
                    new PocoClassParams(), "pos1")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);

            string p2;
            (s, i, b, d, @null, p1, p2) = connection.Read<string, int, bool, DateTime, string, string, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1, @pos2", 
                    new PocoClassParams(), "pos1", "pos2")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            Assert.Equal("pos2", p2);
            
            (p1, s, i, b, d, @null) = connection.Read<string, string, int, bool, DateTime, string>(
                    "select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue", 
                    new PocoClassParams(), "pos1")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            
            (p1, s, i, b, d, @null) = connection.Read<string, string, int, bool, DateTime, string>(
                    "select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue", 
                    "pos1", new PocoClassParams())
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            
            (p1, s, i, b, d, @null, p2) = connection.Read<string, string, int, bool, DateTime, string, string>(
                    "select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos2", 
                    "pos1", new PocoClassParams(), "pos2")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            Assert.Equal("pos2", p2);
        }
        
                [Fact]
        public void PocoClassParams_DbParams_Mixed_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null, p1) = connection.Read<string, int, bool, DateTime, string, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1", 
                    new PocoClassParams(), new NpgsqlParameter("pos1", "pos1"))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);

            string p2;
            (s, i, b, d, @null, p1, p2) = connection.Read<string, int, bool, DateTime, string, string, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1, @pos2", 
                    new PocoClassParams(), new NpgsqlParameter("pos1", "pos1"), new NpgsqlParameter("pos2", "pos2"))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            Assert.Equal("pos2", p2);
            
            (p1, s, i, b, d, @null) = connection.Read<string, string, int, bool, DateTime, string>(
                    "select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue", 
                    new PocoClassParams(), new NpgsqlParameter("pos1", "pos1"))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            
            (p1, s, i, b, d, @null) = connection.Read<string, string, int, bool, DateTime, string>(
                    "select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue", 
                    new NpgsqlParameter("pos1", "pos1"), new PocoClassParams())
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            
            (p1, s, i, b, d, @null, p2) = connection.Read<string, string, int, bool, DateTime, string, string>(
                    "select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos2", 
                    new NpgsqlParameter("pos1", "pos1"), new PocoClassParams(), new NpgsqlParameter("pos2", "pos2"))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            Assert.Equal("pos2", p2);
        }

        /*
        class PocoClassParams
        {
            public string StrValue { get; set; } = "str";
            public int IntValue { get; set; } = 999;
            public bool BoolValue { get; set; } = true;
            public DateTime DateTimeValue { get; set; } = new DateTime(1977, 5, 19);
            public string NullValue { get; set; } = null;
        }
         * */

        [Fact]
        public void Anonymous_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection.Read<string, int, bool, DateTime, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue", new { 
                        StrValue = "str",
                        IntValue = 999,
                        BoolValue = true,
                        DateTimeValue = new DateTime(1977, 5, 19),
                        NullValue = (string)null,
                    })
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Anonymous_Params_CaseMistmatch_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection.Read<string, int, bool, DateTime, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue", new
                    {
                        strValue = "str",
                        intValue = 999,
                        boolValue = true,
                        dateTimeValue = new DateTime(1977, 5, 19),
                        nullValue = (string)null,
                    })
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Anonymous_Params_ShortVersion_Test()
        {
            var strValue = "str";
            var intValue = 999;
            var boolValue = true;
            var dateTimeValue = new DateTime(1977, 5, 19);
            var nullValue = (string)null;

            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection.Read<string, int, bool, DateTime, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue", 
                    new {strValue, intValue, boolValue, dateTimeValue, nullValue,})
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Anonymous_Params_DbType_Test()
        {
            var strValue = "str";
            var intValue = 999;
            var boolValue = true;
            var dateTimeValue = new DateTime(1977, 5, 19);
            var nullValue = (string)null;

            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection.Read<string, int, bool, DateTime, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue",
                    new 
                    { 
                        strValue = new NpgsqlParameter("StrValue", strValue), 
                        intValue, 
                        boolValue, 
                        dateTimeValue, nullValue, 
                    })
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Anonymous_InputOutput_Parameters_Function_Test()
        {
            var p = new NpgsqlParameter("test_param", "I am output value") { Direction = ParameterDirection.InputOutput };

            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection
                .Execute(@"
                    create function test_inout_param_func_2(inout test_param text) returns text as
                    $$
                    begin
                        test_param := test_param || ' returned from function';
                    end
                    $$
                    language plpgsql")
                .AsProcedure()
                //.WithOutParameter("test_param", "I am output value")
                .Execute("test_inout_param_func_2", new{ p });

            Assert.Equal("I am output value returned from function", p.Value);
        }

        [Fact]
        public void Multiple_Anonymous_Params_DbType_Test()
        {
            var strValue = "str";
            var intValue = 999;
            var boolValue = true;
            var dateTimeValue = new DateTime(1977, 5, 19);
            var nullValue = (string)null;

            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection.Read<string, int, bool, DateTime, string>(
                    "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue",
                    new
                    {
                        strValue = new NpgsqlParameter("StrValue", strValue),
                        intValue,
                    },
                    new
                    {
                        boolValue,
                        dateTimeValue,
                    },
                    new
                    {
                        nullValue,
                    })
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Positional_Params_Error_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var exception = Assert.Throws<NormParametersException>(() => 
                connection.Read<int, int>("select @p, @p", 1, 2).Single());

            Assert.Equal("Parameter name \"p\" appears more than once. Parameter names must be unique.", exception.Message);
        }
    }
}
