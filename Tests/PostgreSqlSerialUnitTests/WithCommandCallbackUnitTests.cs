using Norm;
using Npgsql;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PostgreSqlSerialUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class WithCommandCallbackUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public WithCommandCallbackUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void WithCommandCallback_Test()
        {
            bool withCommentHeaderCalled = false;
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            
            connection
                .WithCommandCallback(cmd => withCommentHeaderCalled = true)
                .Execute("select 1");
            
            Assert.True(withCommentHeaderCalled);
        }

        [Fact]
        public void WithCommandCallback_CallCount_Test()
        {
            int callCount = 0;
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommandCallback(cmd => callCount++)
                .Execute("select 1");

            connection
                .WithCommandCallback(cmd => callCount++)
                .Execute("select 2");

            connection.Execute("select 3");

            connection.Execute("select 4");

            Assert.Equal(2, callCount);
        }

        [Fact]
        public void WithCommentHeader_CommandText_Test()
        {
            string expected = "select 1";
            string? actual = null;
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommandCallback(cmd => actual = cmd.CommandText)
                .Execute("select 1");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithCommandCallback_And_Options_Test()
        {
            bool withCommentHeaderCalled = false;
            bool optionsCommentHeaderCalled = false;
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            NormOptions.Configure(options =>
            {
                options.DbCommandCallback = cmd => optionsCommentHeaderCalled = true;
            });

            connection
                .WithCommandCallback(cmd => withCommentHeaderCalled = true)
                .Execute("select 1");

            Assert.True(withCommentHeaderCalled);
            Assert.True(optionsCommentHeaderCalled);
        }
    }
}