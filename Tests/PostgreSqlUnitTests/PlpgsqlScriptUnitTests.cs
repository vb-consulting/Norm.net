using System;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class PlpgsqlScriptUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public PlpgsqlScriptUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void SimpleScript_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString)
                .Execute("create table plpgsql_test1 (t text)")
                .Execute(@"

                    do $$
                    begin

                        insert into plpgsql_test1 values ('foo1');
                        insert into plpgsql_test1 values ('foo2');

                    end
                    $$;

                ");

            var result = connection
                .Read<string>("select t from plpgsql_test1")
                .ToArray();

            Assert.Equal("foo1", result[0]);
            Assert.Equal("foo2", result[1]);
        }

        [Fact]
        public void ParametersInScriptManual_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString)
                .Execute("create table plpgsql_test2 (t text)")
                .Execute($@"

                    do $$
                    begin

                        insert into plpgsql_test2 values (format('%s', '{"foo1"}'));
                        insert into plpgsql_test2 values (format('%s', '{"foo2"}'));

                    end
                    $$;

                ");

            var result = connection
                .Read<string>("select t from plpgsql_test2")
                .ToArray();

            Assert.Equal("foo1", result[0]);
            Assert.Equal("foo2", result[1]);
        }
    }
}
