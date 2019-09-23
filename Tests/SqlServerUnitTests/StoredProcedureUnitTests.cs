using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NoOrm;
using Xunit;
using ConnectionExtensions = NoOrm.Extensions.ConnectionExtensions;
using NoOrmExtensions = NoOrm.Extensions.NoOrmExtensions;

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
                ConnectionExtensions.Execute(ConnectionExtensions.As(ConnectionExtensions.Execute(ConnectionExtensions.As(ConnectionExtensions.Execute(connection, "create procedure EmptyStoreProc as /* empty */"), CommandType.StoredProcedure), "EmptyStoreProc"), CommandType.Text), "drop procedure EmptyStoreProc");

                var procMissing = false;
                try
                {
                    ConnectionExtensions.Execute(ConnectionExtensions.As(connection, CommandType.StoredProcedure), "EmptyStoreProc");
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
                var results = NoOrmExtensions.ToDictionaries(ConnectionExtensions.Read(ConnectionExtensions.As(ConnectionExtensions.Execute(connection, @"
                        create procedure TestStoredProcedure(@id int)
                        as
                        select * from (
                        values 
                            (1, 'foo1', cast('1977-05-19' as date)),
                            (2, 'foo2', cast('1978-05-19' as date)),
                            (3, 'foo3', cast('1979-05-19' as date))
                        ) t(first, bar, day)
                        where first = @id
                    "), CommandType.StoredProcedure), "TestStoredProcedure", ("id", 1)))
                    .ToList();

                Assert.Single(results);
                Assert.Equal(1, results[0].Values.First());
                Assert.Equal("foo1", results[0]["bar"]);
                Assert.Equal(new DateTime(1977, 5, 19), results[0]["day"]);
            }
        }
    }
}
