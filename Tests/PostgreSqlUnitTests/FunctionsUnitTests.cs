using System;
using System.Collections.Generic;
using System.Data;
using NoOrm.Extensions;
using Npgsql;
using PostgreSqlUnitTests;
using Xunit;

namespace SqlServerUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class FunctionsUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public FunctionsUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Execute_And_Drop_Text_Function_Test_Procedure()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Execute(@"
                    create function test_func() returns text as
                    $$
                    begin
                        return 'I am function result!';
                    end
                    $$
                    language plpgsql")
                .AsProcedure()
                .Single<string>("test_func");

            Assert.Equal("I am function result!", result);

            connection.AsText().Execute("drop function test_func()");

            var procMissing = false;
            try
            {
                connection.AsProcedure().Execute("test_func");
            }
            catch (NpgsqlException)
            {
                procMissing = true;
            }
            Assert.True(procMissing);
        }

        /*
        [Fact]
        public void Execute_Create_Procedure_And_Read_Results()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
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
        */
    }
}
