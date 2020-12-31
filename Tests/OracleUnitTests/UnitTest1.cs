using System;
using Xunit;
using Oracle.ManagedDataAccess.Client;
using Norm;

namespace OracleUnitTests
{
    [Collection("OracleDatabase")]
    public class UnitTest1
    {
        private readonly OracleSqlFixture fixture;

        public UnitTest1(OracleSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Test1()
        {
            using var connection = new OracleConnection(fixture.ConnectionString);
            connection.Execute("select 1 from dual");
        }
    }
}
