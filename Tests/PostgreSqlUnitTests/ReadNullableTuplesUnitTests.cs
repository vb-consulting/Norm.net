using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class ReadNullableTuplesUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public ReadNullableTuplesUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Null_Value_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<(int?, int?)>("select null, 1").Single();
            Assert.Null(result.Item1);
            Assert.Equal(1, result.Item2);
        }

        [Fact]
        public void Nullable_Tuple_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result1 = connection.Read<(int one, string two)>("select 1, 'name'").Single();
            Assert.Equal(1, result1.one);
            Assert.Equal("name", result1.two);

            //var result2 = connection.Read<(int one, string two)?>("select 1, 'name'").Single();
            //Assert.Equal(1, result2?.one);
            //Assert.Equal("name", result2?.two);
        }
    }
}
