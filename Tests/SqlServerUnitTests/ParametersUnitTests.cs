using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Xunit;


namespace SqlServerUnitTests
{
    [Collection("SqlClientDatabase")]
    public class ParametersUnitTests
    {
        private readonly SqlClientFixture fixture;

        public ParametersUnitTests(SqlClientFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void PositionalParams_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d", "str", 999, true, new DateTime(1977, 5, 19))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void NamedParams_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                ("d", new DateTime(1977, 5, 19)), ("b", true), ("i", 999), ("s", "str"))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void DbParams_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new SqlParameter("s", "str"),
                new SqlParameter("i", 999),
                new SqlParameter("b", true),
                new SqlParameter("d", new DateTime(1977, 5, 19)))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new SqlParameter("d", new DateTime(1977, 5, 19)),
                new SqlParameter("b", true),
                new SqlParameter("i", 999),
                new SqlParameter("s", "str"))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void MixedParams_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new SqlParameter("d", new DateTime(1977, 5, 19)), "str", 999, true)
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);

            (s, i, b, d) = connection.Read<string, int, bool, DateTime>(
                "select @s, @i, @b, @d",
                new SqlParameter("s", "str"), new SqlParameter("i", 999), true, new DateTime(1977, 5, 19))
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.True(b);
            Assert.Equal(new DateTime(1977, 5, 19), d);
        }

        [Fact]
        public void InputOutput_Parameters_Function_Test()
        {
            var p = new SqlParameter("TestParam", "I am output value") 
            { 
                Direction = ParameterDirection.InputOutput,
                DbType = DbType.AnsiString,
                Size = int.MaxValue
            };

            using var connection = new SqlConnection(fixture.ConnectionString);
            connection
                  .Execute(@"
                    create procedure TestInOutParamProc(@TestParam nvarchar(max) output)
                    as
                    set @TestParam = concat(@TestParam, ' returned from procedure');
                    ")
                  .AsProcedure()
                  .Execute("TestInOutParamProc", p);

            Assert.Equal("I am output value returned from procedure", p.Value);
        }

        [Fact]
        public void Positional_Parameters_When_Using_RowCount_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (v1, v2, rc) = connection.Read<string, string, int>("select @v1, @v2, @@rowcount", "v1", "v2").Single();
            Assert.Equal("v1", v1);
            Assert.Equal("v2", v2);
            Assert.Equal(0, rc);
            
            (rc, v1, v2) = connection.Read<int, string, string>("select @@rowcount, @v1, @v2", "v1", "v2").Single();
            Assert.Equal("v1", v1);
            Assert.Equal("v2", v2);
            Assert.Equal(1, rc);
            
            (v1, rc, v2) = connection.Read<string, int, string>("select @v1, @@rowcount, @v2", "v1", "v2").Single();
            Assert.Equal("v1", v1);
            Assert.Equal("v2", v2);
            Assert.Equal(1, rc);
        }
    }
}
