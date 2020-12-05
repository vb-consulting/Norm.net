using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;
using Xunit;

namespace SQLiteUnitTests
{
    [CollectionDefinition("SQLiteDatabase")]
    public class DatabaseFixtureCollection : ICollectionFixture<SqLiteFixture> { }

    public class SqLiteFixture : IDisposable
    {
        private const string FileName = "norm_unit_tests.db";

        public SqLiteFixture()
        {
            var sb = new SQLiteConnectionStringBuilder
            {
                DataSource = FileName
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
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
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
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
        }
    }
}
