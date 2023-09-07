using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class HstoreMapUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        const string query = "select 'foo => 123, bar => 456' as i, 'foo => 123, bar => 456'::hstore as j";

        public HstoreMapUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Hstore_Read_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute("create extension if not exists hstore");
            connection.ReloadTypes();

            var result = connection
                .Read(query)
                .Single();

            Assert.IsType<string>(result[0].value);
            Assert.IsType<Dictionary<string, string>>(result[1].value);
            Assert.Equal("123", (result[1].value as Dictionary<string, string>)["foo"]);
        }

        [Fact]
        public void Hstore_Read_Tuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute("create extension if not exists hstore");
            connection.ReloadTypes();

            var result = connection
                .Read<string, Dictionary<string, string>>(query)
                .Single();

            Assert.IsType<string>(result.Item1);
            Assert.IsType<Dictionary<string, string>>(result.Item2);
            Assert.Equal("123", result.Item2["foo"]);
        }

        [Fact]
        public void Hstore_Read_Named_Tuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute("create extension if not exists hstore");
            connection.ReloadTypes();

            var result = connection
                .Read<(string I, Dictionary<string, string> J)>(query)
                .Single();

            Assert.IsType<string>(result.I);
            Assert.IsType<Dictionary<string, string>>(result.J);
            Assert.Equal("123", result.J["foo"]);
        }

        public class HstoreTest
        {
            public string I { get; set; }
            public Dictionary<string, string> J { get; set; }
        }
        
        [Fact]
        public void Hstore_Read_Class_Instance_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            connection.Execute("create extension if not exists hstore");
            connection.ReloadTypes();

            var result = connection
                .Read<HstoreTest>(query)
                .Single();

            Assert.IsType<string>(result.I);
            Assert.IsType<Dictionary<string, string>>(result.J);
            Assert.Equal("123", result.J["foo"]);
        }
    }
}
