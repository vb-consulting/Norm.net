using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class BatchUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public BatchUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Batch_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            using var batch = connection.CreateBatch();
            connection.Open();

            batch.BatchCommands.Add(new NpgsqlBatchCommand("select 1"));
            batch.BatchCommands.Add(new NpgsqlBatchCommand("select 2"));
            batch.BatchCommands.Add(new NpgsqlBatchCommand("select 3"));

            using var reader = batch.ExecuteReader();
            reader.Read();
            var one = reader.GetFieldValue<int>(0);

            reader.NextResult();

            reader.Read();
            var two = reader.GetFieldValue<int>(0);

            reader.NextResult();

            reader.Read();
            var three = reader.GetFieldValue<int>(0);

            Assert.Equal(1, one);
            Assert.Equal(2, two);
            Assert.Equal(3, three);
        }
    }
}
