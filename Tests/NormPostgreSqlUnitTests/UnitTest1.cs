using System;
using Npgsql;
using Norm;
using Xunit;

namespace NormPostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class UnitTest1
    {
        private readonly PostgreSqlFixture fixture;

        public UnitTest1(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Test1()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute("");
        }
    }
}
