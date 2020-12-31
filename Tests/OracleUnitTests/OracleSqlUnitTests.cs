using System;
using Xunit;
using Oracle.ManagedDataAccess.Client;

namespace OracleUnitTests
{
    [CollectionDefinition("OracleDatabase")]
    public class DatabaseFixtureCollection : ICollectionFixture<OracleSqlFixture> { }

    public class OracleSqlFixture : IDisposable
    {
        private readonly TestConfig.TestConfig config;

        public string ConnectionString { get; }

        public OracleSqlFixture()
        {
            config = TestConfig.Config.Value;
            ConnectionString = config.Default;
            //CreateTestDatabase();
        }

        public void Dispose()
        {
            //DropTestDatabase();
        }
    }
}
