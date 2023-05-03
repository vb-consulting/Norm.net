using System;
using Microsoft.Data.SqlClient;
using Xunit;

namespace SqlServerUnitTests
{
    [CollectionDefinition("SqlClientDatabase")]
    public class DatabaseFixtureCollection : ICollectionFixture<SqlClientFixture> { }

    public class SqlClientFixture : IDisposable
    {
        private readonly TestConfig.TestConfig config;

        public string ConnectionString { get; }

        public SqlClientFixture()
        {
            config = TestConfig.Config.Value;
            var builder = new SqlConnectionStringBuilder(config.Default)
            {
                InitialCatalog = config.TestDatabase
            };
            ConnectionString = builder.ToString();
            CreateTestDatabase();
        }

        public void Dispose()
        {
            DropTestDatabase();
        }

        private void CreateTestDatabase()
        {
            void DoCreate() => Execute($"create database {config.TestDatabase}");
            try
            {
                DoCreate();
            }
            catch (SqlException e)
            {
                switch (e.Number)
                {

                    case 1801:
                        DropTestDatabase();
                        DoCreate();
                        break;
                    default:
                        throw;
                }
            }
        }

        private void DropTestDatabase()
        {
            Execute($"alter database {config.TestDatabase} set single_user with rollback immediate;");
            Execute($"drop database {config.TestDatabase};");
        }

        private void Execute(string command)
        {
            using var conn = new SqlConnection(config.Default);
            using var cmd = new SqlCommand(command, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
