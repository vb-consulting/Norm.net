using System;
using System.Linq;
using System.Threading.Tasks;
using Norm.Extensions;
using Norm.Extensions.PostgreSQL;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class ExtensionsUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public ExtensionsUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void TableExists_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            Assert.False(connection.TableExists("test"));

            connection.Execute("create table test (t text)");

            Assert.True(connection.TableExists("test"));

            Assert.True(connection.TableExists("test", "public"));
            Assert.False(connection.TableExists("test", "schema"));
        }
    }
}
