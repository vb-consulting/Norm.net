using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class CommandBehaviorUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public CommandBehaviorUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Close_Connection_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection.Read("select 1").ToArray();
            Assert.True(connection.State == System.Data.ConnectionState.Open);

            connection
                .WithCommandBehavior(System.Data.CommandBehavior.CloseConnection)
                .Read("select 2")
                .ToArray();

            Assert.True(connection.State == System.Data.ConnectionState.Closed);
        }
    }
}
