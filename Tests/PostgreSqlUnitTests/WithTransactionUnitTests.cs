using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class WithTransactionUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public WithTransactionUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void WithTransaction_Rollback_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString)
                .Execute("create temp table transaction_test1 (i int);");

            using var transaction = connection.BeginTransaction();

            connection
                .WithTransaction(transaction)
                .Execute("insert into transaction_test1 values (1),(2),(3);");

            var result1 = connection.Read("select * from transaction_test1").ToArray();
            Assert.Equal(3, result1.Length);

            transaction.Rollback();

            var result2 = connection.Read("select * from transaction_test1").ToArray();
            Assert.Empty(result2);
        }

        [Fact]
        public async Task WithTransaction_Rollback_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString)
                .Execute("create temp table transaction_test2 (i int);");

            await using var transaction = await connection.BeginTransactionAsync();

            await connection
                .WithTransaction(transaction)
                .ExecuteAsync("insert into transaction_test2 values (1),(2),(3);");

            var result1 = await connection.ReadAsync("select * from transaction_test2").ToArrayAsync();
            Assert.Equal(3, result1.Length);

            await transaction.RollbackAsync();

            var result2 = await connection.ReadAsync("select * from transaction_test2").ToArrayAsync();
            Assert.Empty(result2);
        }
    }
}
