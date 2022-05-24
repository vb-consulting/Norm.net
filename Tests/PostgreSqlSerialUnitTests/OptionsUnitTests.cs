using Norm;
using Npgsql;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PostgreSqlSerialUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class OptionsUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public OptionsUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Configure_CommandCommentHeader_DbCommandCallback_Test()
        {
            var expected = new string[]
            {
                "-- Npgsql Text Command. Timeout: 30 seconds.",
                "select 1"
            };
            string? actual = null;
            int? timeout = null;
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            NormOptions.Configure(options =>
            {
                options.CommandCommentHeader.Enabled = true;
                options.DbCommandCallback = cmd =>
                {
                    actual = cmd.CommandText;
                    timeout = cmd.CommandTimeout;
                };
            });

            connection.Execute("select 1");
            Assert.Equal(string.Join(Environment.NewLine, expected), actual);
            Assert.Equal(30, timeout); // the default
        }

        [Fact]
        public void Configure_CommandCommentHeader_IncludeTimestamp_DbCommandCallback_Test()
        {
            var expected = new string[]
            {
                "-- Npgsql Text Command. Timeout: 30 seconds.",
                $"-- Timestamp: {DateTime.Now.ToString("o")[..11]}",
                "select 1"
            };
            string? actual = null;
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            NormOptions.Configure(options =>
            {
                options.CommandCommentHeader.Enabled = true;
                options.CommandCommentHeader.IncludeTimestamp = true;
                options.DbCommandCallback = cmd =>
                {
                    actual = cmd.CommandText;
                };
            });

            connection.Execute("select 1");

            var actualLines = actual?.Split(Environment.NewLine);
            Assert.Equal(3, actualLines?.Length);
            Assert.Equal(expected.Length, actualLines?.Length);
            Assert.Equal(expected[0], actualLines?[0]);
            Assert.True(actualLines?[1].StartsWith(expected[1]));
            Assert.Equal(expected[2], actualLines?[2]);
        }

        [Fact]
        public void Configure_CommandCommentHeader_DbCommandCallback_CommandTimeout_Test()
        {
            var expected = new string[]
            {
                "-- Npgsql Text Command. Timeout: 60 seconds.",
                "select 1"
            };
            string? actual = null;
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            NormOptions.Configure(options =>
            {
                options.CommandTimeout = 60;
                options.CommandCommentHeader.Enabled = true;
                options.DbCommandCallback = cmd =>
                {
                    actual = cmd.CommandText;
                };
            });

            connection.Execute("select 1");
            Assert.Equal(string.Join(Environment.NewLine, expected), actual);
        }

        [Fact]
        public void Configure_Prepared_Test()
        {
            var statement = "select 1";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            NormOptions.Configure(options =>
            {
                options.Prepared = true;
            });

            connection.Execute("select 1");

            var result = connection.Read<string, uint[], bool>(
        $"select statement, parameter_types, from_sql from pg_prepared_statements where statement = '{statement}'").ToArray();

            Assert.Single(result);
            Assert.Equal(statement, result[0].Item1);
            Assert.Empty(result[0].Item2);
            Assert.False(result[0].Item3);
        }

        [Fact]
        public void Configure_Prepared_DbCommandCallback_Test()
        {
            var statement = "select 1";
            bool prepared = false;
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            NormOptions.Configure(options =>
            {
                options.Prepared = true;
                options.DbCommandCallback = cmd =>
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    prepared = (cmd as NpgsqlCommand).IsPrepared;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                };
            });

            connection.Execute("select 1");

            var result = connection.Read<string, uint[], bool>(
        $"select statement, parameter_types, from_sql from pg_prepared_statements where statement = '{statement}'").ToArray();

            Assert.Single(result);
            Assert.Equal(statement, result[0].Item1);
            Assert.Empty(result[0].Item2);
            Assert.False(result[0].Item3);
            Assert.True(prepared);
        }

        [Fact]
        public void Configure_Prepared_WithHeaderComment_Test()
        {
            var statement = "select 1";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            NormOptions.Configure(options =>
            {
                options.CommandCommentHeader.Enabled = true;
                options.Prepared = true;
            });

            connection.Execute("select 1");

            var result = connection.Read<string, uint[], bool>(
        $"select statement, parameter_types, from_sql from pg_prepared_statements where statement = '{statement}'").ToArray();

            Assert.Single(result);
            Assert.Equal(statement, result[0].Item1);
            Assert.Empty(result[0].Item2);
            Assert.False(result[0].Item3);
        }

        [Fact]
        public void Configure_CommandCommentHeader_IncludeParameters_DbCommandCallback_Test()
        {
            var expected = new string[]
            {
                "-- Npgsql Text Command. Timeout: 30 seconds.",
                "-- @1 integer = 1",
                "-- @2 text = \"foo\"",
                "-- @3 boolean = false",
                "-- @4 timestamp = \"2022-05-19T00:00:00.0000000\"",
                "select @1, @2, @3, @4"
            };

            string? actual = null;
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            NormOptions.Configure(options =>
            {
                options.CommandCommentHeader.Enabled = true;
                options.DbCommandCallback = cmd =>
                {
                    actual = cmd.CommandText;
                };
            });

            connection.WithParameters(1, "foo", false, new DateTime(2022, 5, 19)).Execute("select @1, @2, @3, @4");
            Assert.Equal(string.Join(Environment.NewLine, expected), actual);
        }

        [Fact]
        public void Configure_NormInstanceType_BadPath_Test()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                NormOptions.Configure(options =>
                {
                    options.NormInstanceType = typeof(OptionsUnitTests);
                });
            });
        }


        private class NormExtension : Norm.Norm
        {
            public static Action? ApplyOptionsCallback = null;

            public NormExtension(DbConnection connection) : base(connection)
            {
            }

            protected override void ApplyOptions(DbCommand cmd)
            {
                ApplyOptionsCallback?.Invoke();
            }
        }

        [Fact]
        public void Configure_NormInstanceType_GoodPath_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            NormOptions.Configure(options =>
            {
                options.NormInstanceType = typeof(NormExtension);
            });

            var applyOptionsCallbackCalled = false;

            NormExtension.ApplyOptionsCallback = () =>
            {
                applyOptionsCallbackCalled = true;
            };

            Assert.False(applyOptionsCallbackCalled);

            connection.Execute("select 1");

            Assert.True(applyOptionsCallbackCalled);
        }
    }
}