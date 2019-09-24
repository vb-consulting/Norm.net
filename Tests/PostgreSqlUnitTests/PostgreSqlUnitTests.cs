using System;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [CollectionDefinition("PostgreSqlDatabase")]
    public class DatabaseFixtureCollection : ICollectionFixture<PostgreSqlFixture> { }

    public class PostgreSqlFixture : IDisposable
    {
        private const string Default =
            "Server=localhost;Database=postgres;Port=5432;User Id=postgres;Password=postgres;";

        private const string TestDatabase = "no_orm_unit_tests";

        public PostgreSqlFixture()
        {
            ConnectionString = $"Server=localhost;Database={TestDatabase};Port=5432;User Id=postgres;Password=postgres;";
            CreateTestDatabase();
        }

        public string ConnectionString { get; }

        public void Dispose()
        {
            DropTestDatabase();
        }

        private void CreateTestDatabase()
        {
            void DoCreate() => Execute($"create database {TestDatabase}");
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
            revoke connect on database {TestDatabase} from public;
            
            select pid, pg_terminate_backend(pid) from pg_stat_activity 
            where datname = '{TestDatabase}' and pid <> pg_backend_pid();

            drop database {TestDatabase};");
        }


        private void Execute(string command)
        {
            using var conn = new NpgsqlConnection(Default);
            using var cmd = new NpgsqlCommand(command, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
