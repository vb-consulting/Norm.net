using System;
using System.Collections.Generic;
using System.Data;
using Norm.Extensions;
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

        [Fact]
        public void Output_Parameters_Function_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection
                .Execute(@"
                    create function test_out_param_func(out test_param text) returns text as
                    $$
                    begin
                        test_param := 'I am output value!';
                    end
                    $$
                    language plpgsql")
                .AsProcedure()
                .WithOutParameter("test_param")
                .Execute("test_out_param_func");

            Assert.Equal("I am output value!", connection.GetOutParameterValue("test_param"));
        }

        [Fact]
        public void InputOutput_Parameters_Function_Test()
        {
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
                .WithOutParameter("test_param", "I am output value")
                .Execute("test_inout_param_func");

            Assert.Equal("I am output value returned from function", connection.GetOutParameterValue("test_param"));
        }
    }
}
