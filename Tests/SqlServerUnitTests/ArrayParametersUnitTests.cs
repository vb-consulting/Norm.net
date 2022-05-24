using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Xunit;


namespace SqlServerUnitTests
{
    [Collection("SqlClientDatabase")]
    public class ArrayParametersUnitTests
    {
        private readonly SqlClientFixture fixture;

        public ArrayParametersUnitTests(SqlClientFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void PositionalParams_Array_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (p1, p2, p3) = connection
                .WithParameters(new[] { 1, 2, 3 })
                .Read<int, int, int>("select @p")
                .Single();

            Assert.Equal(1, p1);
            Assert.Equal(2, p2);
            Assert.Equal(3, p3);
        }

        [Fact]
        public void PositionalParams_List_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (p1, p2, p3) = connection
                .WithParameters(new List<int>() { 1, 2, 3 })
                .Read<int, int, int>("select @p")
                .Single();

            Assert.Equal(1, p1);
            Assert.Equal(2, p2);
            Assert.Equal(3, p3);
        }

        [Fact]
        public void NamedParams_Array_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (p1, p2, p3) = connection
                .WithParameters(new { p = new[] { 1, 2, 3 } })
                .Read<int, int, int>("select @p")
                .Single();

            Assert.Equal(1, p1);
            Assert.Equal(2, p2);
            Assert.Equal(3, p3);
        }

        [Fact]
        public void NamedParams_List_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (p1, p2, p3) = connection
                .WithParameters(new { p = new[] { 1, 2, 3 } })
                .Read<int, int, int>("select @p")
                .Single();

            Assert.Equal(1, p1);
            Assert.Equal(2, p2);
            Assert.Equal(3, p3);
        }

        [Fact]
        public void NamedParams_Array_ParamNames_Test()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var (p1, p2, p3) = connection
                .WithParameters(new { p = new[] { 1, 2, 3 } })
                .Read<int, int, int>("select @__p0, @__p1, @__p2")
                .Single();

            Assert.Equal(1, p1);
            Assert.Equal(2, p2);
            Assert.Equal(3, p3);
        }
    }
}
