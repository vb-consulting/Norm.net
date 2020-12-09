using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Xunit;

namespace SQLiteUnitTests
{
    [CollectionDefinition("SQLiteDatabase")]
    public class DatabaseFixtureCollection : ICollectionFixture<SqLiteFixture> { }

    public class SqLiteFixture : IDisposable
    {
        private readonly TestConfig.TestConfig config;

        public SqLiteFixture()
        {
            config = TestConfig.Config.Value;

            var sb = new SQLiteConnectionStringBuilder
            {
                DataSource = config.Default
            };
            ConnectionString = sb.ToString();
            CreateTestDatabase();
        }

        public string ConnectionString { get; }

        public void Dispose()
        {
            DropTestDatabase();
        }

        private void CreateTestDatabase()
        {
            if (File.Exists(config.Default))
            {
                File.Delete(config.Default);
            }

            using var connection = new SQLiteConnection(ConnectionString);
            try
            {
                connection.Open();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void DropTestDatabase()
        {
            if (File.Exists(config.Default))
            {
                File.Delete(config.Default);
            }
        }
    }
}
