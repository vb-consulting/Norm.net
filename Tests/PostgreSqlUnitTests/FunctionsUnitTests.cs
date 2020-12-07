using System;
using System.Collections.Generic;
using System.Data;
using Norm;
using Npgsql;
using PostgreSqlUnitTests;
using Xunit;

namespace PostgreSqlUnitTests
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
        public void Execute_And_Drop_Text_Function_Test()
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
    }
}
