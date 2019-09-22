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
        public PostgreSqlFixture()
        {
            ConnectionString = "Server=localhost;Database=no_orm_unit_tests;Port=5432;User Id=postgres;Password=postgres;";
            CreateTestDatabase();
        }

        public string ConnectionString { get; }

        public void Dispose()
        {
            DropTestDatabase();
        }

        private void CreateTestDatabase()
        {
            void DoCreate() => Execute("create database no_orm_unit_tests;");
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
            Execute(@"
            revoke connect on database no_orm_unit_tests from public;
            
            select pid, pg_terminate_backend(pid) from pg_stat_activity 
            where datname = 'no_orm_unit_tests' and pid <> pg_backend_pid();

            drop database no_orm_unit_tests;");
        }


        private void Execute(string command)
        {
            using (var conn = new NpgsqlConnection(Default))
            {
                using (var cmd = new NpgsqlCommand(command, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
