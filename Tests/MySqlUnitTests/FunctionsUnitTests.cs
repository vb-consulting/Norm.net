using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Norm;
using Xunit;

namespace MySqlUnitTests
{
    [Collection("MySqlDatabase")]
    public class FunctionsUnitTests
    {
        private readonly MySqlFixture fixture;

        public FunctionsUnitTests(MySqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Execute_And_Drop_Text_Function_Test()
        {
            var name = "execute_and_drop_text_function_test";
            using var connection = new MySqlConnection(fixture.ConnectionString);
            new MySqlScript(connection)
            {
                Query = @$"
                    delimiter $$
                    create function {name}() returns text
                    deterministic
                    begin
                        return 'I am procedure result!';
                    end$$
                    delimiter ;
            "
            }.Execute();

            var result = connection.Single<string>($"select {name}()");
            Assert.Equal("I am procedure result!", result);

            connection.Execute($"drop function {name}");

            var procMissing = false;
            try
            {
                connection.Single<string>($"select {name}()");
            }
            catch (MySqlException)
            {
                procMissing = true;
            }
            Assert.True(procMissing);
        }
    }
}
