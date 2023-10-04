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
            var (s, i, b, d, @null) = connection
                .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
                .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Postgres_PositionalParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection
                .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
                .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Postgres_Mixed_PositionalParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            Assert.Throws<NotSupportedException>(() =>
            {
                var (s, i, b, d, @null) = connection
                    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
                    .Read<string, int, bool, DateTime, string>("select $1, @i, $3, $4, $5")
                    .Single();
            });
        }

        [Fact]
        public void Postgres_PositionalParams_MultipleCommand_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            Assert.Throws<PostgresException>(() =>
            {
                var (s, i, b, d, @null) = connection
                    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
                    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5; select 1")
                    .Single();
            });
        }

        [Fact]
        public void Postgres_PositionalParams_Instances_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection
                .WithParameters(new NpgsqlParameter{ Value = "str" },
                    new NpgsqlParameter { Value = 999 },
                    new NpgsqlParameter { Value = true },
                    new NpgsqlParameter { Value = new DateTime(1977, 5, 19) },
                    new NpgsqlParameter { Value = DBNull.Value })
                .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Postgres_PositionalParams_ParamType_Test()
        {
            var p1 = "_b_";
            var p2 = ((string)null, DbType.AnsiString);
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithParameters(p1, p2)
                .Read<string>(@"select * from 
                    (values ('abc'), ('bcd')) t (t1) 
                    where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
                .Single();

            Assert.Equal("abc", result);
        }

        [Fact]
        public void Postgres_PositionalParams_NpgsqlDbType_Test()
        {
            var p1 = "_b_";
            var p2 = ((string)null, NpgsqlDbType.Text);
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithParameters(p1, p2)
                .Read<string>(@"select * from 
                    (values ('abc'), ('bcd')) t (t1) 
                    where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
                .Single();

            Assert.Equal("abc", result);
        }

        [Fact]
        public void Postgres_PositionalParams_WithTypes_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection
                .WithParameters(
                    ("str", DbType.String), 
                    (999, DbType.Int32), 
                    (true, DbType.Boolean), 
                    (new DateTime(1977, 5, 19), DbType.Date), 
                    ((string)null, DbType.String))
                .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Postgres_PositionalParams_WithNpgsqlDbTypes_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection
                .WithParameters(
                    ("str", NpgsqlDbType.Text),
                    (999, NpgsqlDbType.Bigint),
                    (true, NpgsqlDbType.Boolean),
                    (new DateTime(1977, 5, 19), NpgsqlDbType.Date),
                    ((string)null, NpgsqlDbType.Text))
                .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
        }

        [Fact]
        public void Postgres_MultipleInstancePositionalParams_Test()
        {
            var p1 = new NpgsqlParameter { Value = "_b_", DbType = DbType.AnsiString };
            var p2 = new NpgsqlParameter { Value = DBNull.Value, DbType = DbType.AnsiString };

            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithParameters(p1, p2)
                .Read<string>(@"select * from 
                    (values ('abc'), ('bcd')) t (t1) 
                    where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
                .Single();

            Assert.Equal("abc", result);
        }


        class TestClass
        {
            public string S { get; set; }
            public int I { get; set; }
            public bool B { get; set; }
            public DateTime D { get; set; }
            public string Null { get; set; }
        }

        [Fact]
        public void NamedParams_Instance_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection
                .WithParameters(new TestClass
                {
                    D = new DateTime(1977, 5, 19),
                    B = true,
                    I = 999,
                    S = "str",
                    Null = (string)null
                })
                .Read<string, int, bool, DateTime, string>(
                "select @s, @i, @b, @d, @null")
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
            var (s, i, b, d, @null) = connection
                .WithParameters(new
                {
                    d = new DateTime(1977, 5, 19),
                    b = true,
                    i = 999,
                    s = "str",
                    @null = (string)null
                })
                .Read<string, int, bool, DateTime, string>(
                "select @s, @i, @b, @d, @null")
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
            var (s, i, b, d) = connection
                .WithParameters(new NpgsqlParameter("s", "str"),
                    new NpgsqlParameter("i", 999),
                    new NpgsqlParameter("b", true),
                    new NpgsqlParameter("d", new DateTime(1977, 5, 19)))
                .Read<string, int, bool, DateTime>("select @s, @i, @b, @d")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection
                .WithParameters(new NpgsqlParameter("d", new DateTime(1977, 5, 19)),
                    new NpgsqlParameter("b", true),
                    new NpgsqlParameter("i", 999),
                    new NpgsqlParameter("s", "str"))
                .Read<string, int, bool, DateTime>("select @s, @i, @b, @d")
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
            var (s, i, b, d) = connection
                .WithParameters(new NpgsqlParameter("d", new DateTime(1977, 5, 19)), "str", 999, true)
                .Read<string, int, bool, DateTime>("select @s, @i, @b, @d")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection
                .WithParameters(new NpgsqlParameter("s", "str"), new NpgsqlParameter("i", 999), true, new DateTime(1977, 5, 19))
                .Read<string, int, bool, DateTime>("select @s, @i, @b, @d")
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
            var result = connection
                .WithParameters(new NpgsqlParameter("p", NpgsqlDbType.Array | NpgsqlDbType.Integer)
                {
                    Value = new List<int> { 1, 2, 3 }
                })
                .Read<int>("select unnest(@p)")
                .ToList();
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
                .WithParameters(p)
                .Execute("test_inout_param_func_1");

            Assert.Equal("I am output value returned from function", p.Value);
        }

        [Fact]
        public void Nullable_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            long? one = 1;
            long? two = null;

            var (result1, result2) = connection
                .WithParameters(new
                {
                    one,
                    two
                })
                .Read<long?, long?>("select @one, @two")
                .Single();

            Assert.Equal(one, result1);
            Assert.Equal(two, result2);
        }

        [Fact]
        public void Undetermined_Param_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            long? id = null;

            var result = connection
                .WithParameters(new { id = (id, DbType.Int64) })
                .Read<long>("select * from (values (1),(2),(3)) t(id) where @id is null or id = @id")
                .ToList();
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
                .WithParameters(new
                {
                    _p = new[] { 3, 6, 9 }
                })
                .Read<int[]>("array_params_test")
                .Single();

            Assert.Equal(new[] { 3, 6, 9 }, result);
        }

        [Fact]
        public void Custom_Params_Types_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i, j) = connection
                .WithParameters(new
                {
                    i = (1, NpgsqlDbType.Integer),
                    j = ("{\"test\": \"value\"}", NpgsqlDbType.Json)
                })
                .Read<int, string>("select @i, @j->>'test'")
                .Single();
            Assert.Equal(1, i);
            Assert.Equal("value", j);
        }

        [Fact]
        public void Custom_Params_Mixed_Types_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (i, j) = connection
                .WithParameters(new
                {
                    i = (1, DbType.Int32),
                    j = ("{\"test\": \"value\"}", NpgsqlDbType.Json),
                })
                .Read<int, string>("select @i, @j->>'test'")
                .Single();
            Assert.Equal(1, i);
            Assert.Equal("value", j);
        }

        [Fact]
        public void Custom_Params_Aray_Types_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithParameters(new
                {
                    p = (new List<int> { 1, 2, 3 }, NpgsqlDbType.Array | NpgsqlDbType.Integer)
                })
                .Read<int>("select unnest(@p)").ToList();
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
            var (s, i, b, d, @null) = connection
                .WithParameters(new PocoClassParams())
                .Read<string, int, bool, DateTime, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
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
            var (s, i, b, d, @null, p1) = connection
                .WithParameters(new PocoClassParams(), "pos1")
                .Read<string, int, bool, DateTime, string, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);

            string p2;
            (s, i, b, d, @null, p1, p2) = connection
                .WithParameters(new PocoClassParams(), "pos1", "pos2")
                .Read<string, int, bool, DateTime, string, string, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1, @pos2")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            Assert.Equal("pos2", p2);
            
            (p1, s, i, b, d, @null) = connection
                .WithParameters(new PocoClassParams(), "pos1")
                .Read<string, string, int, bool, DateTime, string>(
                    "select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            
            (p1, s, i, b, d, @null) = connection
                .WithParameters("pos1", new PocoClassParams())
                .Read<string, string, int, bool, DateTime, string>("select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            
            (p1, s, i, b, d, @null, p2) = connection
                .WithParameters("pos1", new PocoClassParams(), "pos2")
                .Read<string, string, int, bool, DateTime, string, string>(
                    "select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos2")
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
            var (s, i, b, d, @null, p1) = connection
                .WithParameters(new PocoClassParams(), new NpgsqlParameter("pos1", "pos1"))
                .Read<string, int, bool, DateTime, string, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);

            string p2;
            (s, i, b, d, @null, p1, p2) = connection
                .WithParameters(new PocoClassParams(), new NpgsqlParameter("pos1", "pos1"), new NpgsqlParameter("pos2", "pos2"))
                .Read<string, int, bool, DateTime, string, string, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1, @pos2")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            Assert.Equal("pos2", p2);
            
            (p1, s, i, b, d, @null) = connection
                .WithParameters(new PocoClassParams(), new NpgsqlParameter("pos1", "pos1"))
                .Read<string, string, int, bool, DateTime, string>("select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            
            (p1, s, i, b, d, @null) = connection
                .WithParameters(new NpgsqlParameter("pos1", "pos1"), new PocoClassParams())
                .Read<string, string, int, bool, DateTime, string>("select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
            Assert.Null(@null);
            Assert.Equal("pos1", p1);
            
            (p1, s, i, b, d, @null, p2) = connection
                .WithParameters(new NpgsqlParameter("pos1", "pos1"), new PocoClassParams(), new NpgsqlParameter("pos2", "pos2"))
                .Read<string, string, int, bool, DateTime, string, string>(
                    "select @pos1, @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos2")
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
            var (s, i, b, d, @null) = connection
                .WithParameters(new
                {
                    StrValue = "str",
                    IntValue = 999,
                    BoolValue = true,
                    DateTimeValue = new DateTime(1977, 5, 19),
                    NullValue = (string)null,
                })
                .Read<string, int, bool, DateTime, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
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
            var (s, i, b, d, @null) = connection
                .WithParameters(new
                {
                    strValue = "str",
                    intValue = 999,
                    boolValue = true,
                    dateTimeValue = new DateTime(1977, 5, 19),
                    nullValue = (string)null,
                })
                .Read<string, int, bool, DateTime, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
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
            var (s, i, b, d, @null) = connection
                .WithParameters(new { strValue, intValue, boolValue, dateTimeValue, nullValue})
                .Read<string, int, bool, DateTime, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
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
            var (s, i, b, d, @null) = connection
                .WithParameters(new
                {
                    strValue = new NpgsqlParameter("StrValue", strValue),
                    intValue,
                    boolValue,
                    dateTimeValue,
                    nullValue,
                })
                .Read<string, int, bool, DateTime, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
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
                .WithParameters(new { p })
                .Execute("test_inout_param_func_2");

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
            var (s, i, b, d, @null) = connection
                .WithParameters(new
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
                .Read<string, int, bool, DateTime, string>("select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue")
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
                connection.WithParameters(1, 2).Read<int, int>("select @p, @p").Single());

            Assert.Equal("Parameter name \"p\" appears more than once. Parameter names must be unique when using positional parameters. Try using named parameters instead.", exception.Message);
        }

        [Fact]
        public void Keyworded_NamedParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i) = connection
                .WithParameters(new
                {
                    @string = "str",
                    @int = 999
                })
                .Read<string, int>("select @string, @int")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
        }

        [Fact]
        public void Read_Params_NamedParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection
                .Read<string, int, bool, DateTime, string>(
                    "select @strValue, @intValue, @boolValue, @dateTimeValue, @nullValue",
                    new
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
        public void Read_Mixed_Params_NamedParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d, @null) = connection
                .WithParameters(new
                {
                    strValue = "str",
                    intValue = 999,
                })
                .Read<string, int, bool, DateTime, string>(
                    "select @strValue, @intValue, @boolValue, @dateTimeValue, @nullValue",
                    new
                    {
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
        public void Read_DbParams_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection
                .Read<string, int, bool, DateTime>(
                    "select @s, @i, @b, @d",
                    new NpgsqlParameter[] 
                    {
                        new NpgsqlParameter("s", "str"),
                        new NpgsqlParameter("i", 999),
                        new NpgsqlParameter("b", true),
                        new NpgsqlParameter("d", new DateTime(1977, 5, 19))
                    })
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);

           
            (s, i, b, d) = connection
                .Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                    new NpgsqlParameter[]
                    {
                        new NpgsqlParameter("d", new DateTime(1977, 5, 19)),
                        new NpgsqlParameter("b", true),
                        new NpgsqlParameter("i", 999),
                        new NpgsqlParameter("s", "str")
                    })
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }
    }
}
