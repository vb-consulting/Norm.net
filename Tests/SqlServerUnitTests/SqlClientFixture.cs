using System;
using System.Data.SqlClient;
using Xunit;

namespace SqlServerUnitTests
{
    [CollectionDefinition("SqlClientDatabase")]
    public class DatabaseFixtureCollection : ICollectionFixture<SqlClientFixture> { }

    public class SqlClientFixture : IDisposable
    {
        private const string Default =
            "Data Source=.\\SQLExpress;Initial Catalog=master;Integrated Security=True;";
        private const string TestDatabase = "NoOrmTest";

        public SqlClientFixture()
        {
            ConnectionString = $"Data Source=.\\SQLExpress;Initial Catalog={TestDatabase};Integrated Security=True;";
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
            Execute($"alter database {TestDatabase} set single_user with rollback immediate;");
            Execute($"drop database {TestDatabase};");
        }


        private void Execute(string command)
        {
            using (var conn = new SqlConnection(Default))
            {
                using (var cmd = new SqlCommand(command, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
