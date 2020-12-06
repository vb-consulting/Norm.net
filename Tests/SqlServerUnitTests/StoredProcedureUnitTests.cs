using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Norm.Extensions;
using Xunit;


namespace SqlServerUnitTests
{
    [Collection("SqlClientDatabase")]
    public class StoredProcedureUnitTests
    {
        private readonly SqlClientFixture fixture;

        public StoredProcedureUnitTests(SqlClientFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Execute_Create_Empty_Proc_Execute_And_Drop_Procedure()
        {
            using var connection = new SqlConnection(fixture.ConnectionString)
                .Execute("create procedure EmptyStoreProc as /* empty */")
                .Execute("EmptyStoreProc")
                .Execute("drop procedure EmptyStoreProc");

            var procMissing = false;
            try
            {
                connection.Execute("EmptyStoreProc");
            }
            catch (SqlException)
            {
                procMissing = true;
            }
            Assert.True(procMissing);
        }

        [Fact]
        public void Execute_Create_Procedure_And_Read_Results()
        {
            using var connection = new SqlConnection(fixture.ConnectionString)
                .Execute(@"
                        create procedure TestStoredProcedure(@id int)
                        as
                        select * from (
                        values 
                            (1, 'foo1', cast('1977-05-19' as date)),
                            (2, 'foo2', cast('1978-05-19' as date)),
                            (3, 'foo3', cast('1979-05-19' as date))
                        ) t(first, bar, day)
                        where first = @id
                    ");

            var results = connection
                .AsProcedure()
                .Read("TestStoredProcedure", ("id", 1))
                .Select(tuples => tuples.ToDictionary(t => t.name, t => t.value))
                .ToList();

            Assert.Single(results);
            Assert.Equal(1, results[0].Values.First());
            Assert.Equal("foo1", results[0]["bar"]);
            Assert.Equal(new DateTime(1977, 5, 19), results[0]["day"]);
        }

        /*
        [Fact]
        public void Output_Parameters_Function_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            connection
                .Execute(@"
                    create procedure TestOutParamProc(@TestParam nvarchar(30) output)
                    as
                    set @TestParam = 'I am output value!';
                    ")
                .AsProcedure()
                .WithOutParameter("TestParam")
                .Execute("TestOutParamProc");

            Assert.Equal("I am output value!", connection.GetOutParameterValue("TestParam"));
        }

        [Fact]
        public void InputOutput_Parameters_Function_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            connection
                .Execute(@"
                    create procedure TestInOutParamProc(@TestParam nvarchar(max) output)
                    as
                    set @TestParam = concat(@TestParam, ' returned from procedure');
                    ")
                .AsProcedure()
                .WithOutParameter("TestParam", "I am output value")
                .Execute("TestInOutParamProc");

            Assert.Equal("I am output value returned from procedure", connection.GetOutParameterValue("TestParam"));
        }
        */
    }
}
