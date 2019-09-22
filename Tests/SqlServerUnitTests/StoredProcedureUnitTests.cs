using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NoOrm;
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
            using (var connection = new SqlConnection(fixture.ConnectionString))
            {
                connection
                    .Execute("create procedure EmptyStoreProc as /* empty */")
                    .As(CommandType.StoredProcedure)
                    .Execute("EmptyStoreProc")
                    .As(CommandType.Text)
                    .Execute("drop procedure EmptyStoreProc");

                var procMissing = false;
                try
                {
                    connection.As(CommandType.StoredProcedure).Execute("EmptyStoreProc");
                }
                catch (SqlException)
                {
                    procMissing = true;
                }
                Assert.True(procMissing);
            }
        }

        [Fact]
        public void Execute_Create_Procedure_And_Read_Results()
        {
            using (var connection = new SqlConnection(fixture.ConnectionString))
            {
                var results = connection
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
                    ")
                    .As(CommandType.StoredProcedure)
                    .Read("TestStoredProcedure", ("id", 1))
                    .ToDictionaries()
                    .ToList();

                Assert.Single(results);
                Assert.Equal(1, results[0].Values.First());
                Assert.Equal("foo1", results[0]["bar"]);
                Assert.Equal(new DateTime(1977, 5, 19), results[0]["day"]);
            }
        }
    }
}
