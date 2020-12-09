using System;
using Xunit;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace MySqlUnitTests
{
    [CollectionDefinition("MySqlDatabase")]
    public class DatabaseFixtureCollection : ICollectionFixture<MySqlFixture> { }

    public class MySqlFixture : IDisposable
    {
        private readonly TestConfig.TestConfig config;

        public string ConnectionString { get; }

        public MySqlFixture()
        {
            config = TestConfig.Config.Value;
            var parts = new List<string>(config.Default.Split(';'));
            parts.Add($"Initial Catalog={config.TestDatabase}");
            ConnectionString = string.Join(';', parts);
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
            catch (MySqlException e)
            {
                switch (e.Number)
                {

                    case 1007:
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
            Execute($"drop database {config.TestDatabase};");
        }

        private void Execute(string command)
        {
            using var conn = new MySqlConnection(config.Default);
            using var cmd = new MySqlCommand(command, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
