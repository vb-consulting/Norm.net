using System.Linq;
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
            var r = connection.Read(@"
            SELECT CAST(1 AS INT) AS id, 'foo1' AS foo, TO_DATE('1977-05-19', 'YYYY-MM-DD') AS day, null AS bar FROM DUAL
            UNION ALL
            SELECT CAST(2 AS INT) AS id, 'foo2' AS foo, TO_DATE('1978-05-19', 'YYYY-MM-DD') AS day, 'bar2' AS bar FROM DUAL
            UNION ALL
            SELECT CAST(3 AS INT) AS id, 'foo3' AS foo, null AS day, 'bar3' AS bar FROM DUAL").ToList();
        }
    }
}
