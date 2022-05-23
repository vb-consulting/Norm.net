using System;
using Npgsql;
using Xunit;

namespace PostgreSqlSerialUnitTests
{
    [CollectionDefinition("PostgreSqlDatabase")]
    public class DatabaseFixtureCollection : ICollectionFixture<PostgreSqlFixture> { }

    public class PostgreSqlFixture : IDisposable
    {
        private readonly TestConfig.TestConfig config;

        public string ConnectionString { get; }

        public PostgreSqlFixture()
        {
            config = TestConfig.Config.Value;
            var builder = new NpgsqlConnectionStringBuilder(config.Default)
            {
                Database = config.TestDatabase
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
            catch (PostgresException e)
            {
                switch (e.SqlState)
                {

                    case "42P04":
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
            Execute($@"
            revoke connect on database {config.TestDatabase} from public;
            
            select pid, pg_terminate_backend(pid) from pg_stat_activity 
            where datname = '{config.TestDatabase}' and pid <> pg_backend_pid();

            drop database {config.TestDatabase};");
        }

        private void Execute(string command)
        {
            using var conn = new NpgsqlConnection(config.Default);
            using var cmd = new NpgsqlCommand(command, conn);
            cmd.CommandTimeout = 60 * 5; // 5 mins
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
