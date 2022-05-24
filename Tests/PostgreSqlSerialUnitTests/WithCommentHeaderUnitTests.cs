using Norm;
using Npgsql;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace PostgreSqlSerialUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class WithCommentHeaderUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public WithCommentHeaderUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void WithCommentHeader_Default_Test()
        {
            var expected = new string[]
            {
                "-- Npgsql Text Command. Timeout: 30 seconds.",
                "-- at WithCommentHeader_Default_Test in ",
                "select 1"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommentHeader()
                .WithCommandCallback(c => actual = c.CommandText)
                .Execute("select 1");

            var actualLines = actual.Split(Environment.NewLine);

            Assert.Equal(3, actualLines.Length);
            Assert.Equal(expected.Length, actualLines?.Length);
            Assert.Equal(expected[0], actualLines?[0]);
            Assert.StartsWith(expected[1], actualLines?[1]);
            Assert.EndsWith(" 36", actualLines?[1]);
            Assert.Equal(expected[2], actualLines?[2]);

            connection
                .WithCommandCallback(c => actual = c.CommandText)
                .Execute("select 1");

            Assert.Equal("select 1", actual);
        }

        [Fact]
        public void WithCommentHeader_Comment_Test()
        {
            var expected = new string[]
            {
                "-- This is my comment",
                "select 1"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommentHeader("This is my comment", includeCommandAttributes: false, includeParameters: false, includeCallerInfo: false, includeTimestamp: false)
                .WithCommandCallback(c => actual = c.CommandText)
                .Execute("select 1");

            Assert.Equal(string.Join(Environment.NewLine, expected), actual);

            var expected2 = new string[]
            {
                "-- This is my second comment",
                "select 2"
            };

            connection
                .WithComment("This is my second comment")
                .WithCommandCallback(c => actual = c.CommandText)
                .Execute("select 2");

            Assert.Equal(string.Join(Environment.NewLine, expected2), actual);
        }

        [Fact]
        public void WithCommentHeader_CommandAttributes_Test()
        {
            var expected = new string[]
            {
                "-- Npgsql Text Command. Timeout: 30 seconds.",
                "select 1"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommentHeader(comment: null, includeCommandAttributes: true, includeParameters: false, includeCallerInfo: false, includeTimestamp: false)
                .WithCommandCallback(c => actual = c.CommandText)
                .Execute("select 1");

            Assert.Equal(string.Join(Environment.NewLine, expected), actual);
        }

        [Fact]
        public void WithCommentHeader_Parameters_Test()
        {
            var expected = new string[]
            {
                "-- @1 integer = 1",
                "-- @2 text = \"foo\"",
                "-- @3 boolean = false",
                "-- @4 timestamp = \"2022-05-19T00:00:00.0000000\"",
                "select @1, @2, @3, @4"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommentHeader(comment: null, includeCommandAttributes: false, includeParameters: true, includeCallerInfo: false, includeTimestamp: false)
                .WithCommandCallback(c => actual = c.CommandText)
                .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
                .Execute("select @1, @2, @3, @4");

            Assert.Equal(string.Join(Environment.NewLine, expected), actual);

            var expected2 = new string[]
            {
                "-- @1 integer = 2",
                "-- @2 text = \"bar\"",
                "-- @3 boolean = false",
                "-- @4 timestamp = \"1977-05-19T00:00:00.0000000\"",
                "select @1, @2, @3, @4"
            };

            connection
                .WithCommentParameters()
                .WithCommandCallback(c => actual = c.CommandText)
                .WithParameters(2, "bar", false, new DateTime(1977, 5, 19))
                .Execute("select @1, @2, @3, @4");

            Assert.Equal(string.Join(Environment.NewLine, expected2), actual);
        }

        [Fact]
        public void WithCommentHeader_Comment_And_Parameters_Test()
        {
            var expected = new string[]
            {
                "-- This is my comment",
                "-- @1 integer = 1",
                "-- @2 text = \"foo\"",
                "-- @3 boolean = false",
                "-- @4 timestamp = \"2022-05-19T00:00:00.0000000\"",
                "select @1, @2, @3, @4"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommentParameters()
                .WithComment("This is my comment")
                .WithCommandCallback(c => actual = c.CommandText)
                .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
                .Execute("select @1, @2, @3, @4");

            Assert.Equal(string.Join(Environment.NewLine, expected), actual);
        }

        [Fact]
        public void WithCommentHeader_CallerInfo_Test()
        {
            var expected = new string[]
            {
                "-- at WithCommentHeader_CallerInfo_Test in ",
                "select 1"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommentHeader(comment: null, includeCommandAttributes: false, includeParameters: false, includeCallerInfo: true, includeTimestamp: false)
                .WithCommandCallback(c => actual = c.CommandText)
                .Execute("select 1");

            var actualLines = actual.Split(Environment.NewLine);

            Assert.Equal(2, actualLines.Length);
            Assert.Equal(expected.Length, actualLines?.Length);
            Assert.StartsWith(expected[0], actualLines?[0]);
            //Assert.EndsWith(" 184", actualLines?[0]);
            Assert.Equal(expected[1], actualLines?[1]);

            connection
                .WithCommentCallerInfo()
                .WithCommandCallback(c => actual = c.CommandText)
                .Execute("select 1");

            actualLines = actual.Split(Environment.NewLine);

            Assert.Equal(2, actualLines.Length);
            Assert.Equal(expected.Length, actualLines?.Length);
            Assert.StartsWith(expected[0], actualLines?[0]);
            //Assert.EndsWith(" 197", actualLines?[0]);
            Assert.Equal(expected[1], actualLines?[1]);
        }

        [Fact]
        public void WithCommentHeader_Timestamp_Test()
        {
            var expected = new string[]
            {
                $"-- Timestamp: {DateTime.Now.ToString("o")[..11]}",
                "select 1"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommentHeader(comment: null, includeCommandAttributes: false, includeParameters: false, includeCallerInfo: false, includeTimestamp: true)
                .WithCommandCallback(c => actual = c.CommandText)
                .Execute("select 1");

            var actualLines = actual.Split(Environment.NewLine);

            Assert.Equal(2, actualLines.Length);
            Assert.Equal(expected.Length, actualLines?.Length);
            Assert.StartsWith(expected[0], actualLines?[0]);
            Assert.Equal(expected[1], actualLines?[1]);
        }

        [Fact]
        public void WithCommentHeader_AllHeaders_Test()
        {
            var expected = new string[]
            {
                "-- This is my comment",
                "-- Npgsql Text Command. Timeout: 30 seconds.",
                $"-- Timestamp: {DateTime.Now.ToString("o")[..11]}",
                "-- @1 integer = 1",
                "-- @2 text = \"foo\"",
                "-- @3 boolean = false",
                "-- @4 timestamp = \"2022-05-19T00:00:00.0000000\"",
                "-- at WithCommentHeader_AllHeaders_Test in ",
                "select @1, @2, @3, @4"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection
                .WithCommentHeader(comment: "This is my comment", includeCommandAttributes: true, includeParameters: true, includeCallerInfo: true, includeTimestamp: true)
                .WithCommandCallback(c => actual = c.CommandText)
                .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
                .Execute("select @1, @2, @3, @4");

            var actualLines = actual.Split(Environment.NewLine);

            Assert.Equal(9, actualLines.Length);
            Assert.Equal(expected.Length, actualLines?.Length);
            Assert.Equal(expected[0], actualLines?[0]);
            Assert.Equal(expected[1], actualLines?[1]);
            Assert.StartsWith(expected[2], actualLines?[2]);

            Assert.StartsWith(expected[3], actualLines?[3]);
            Assert.StartsWith(expected[4], actualLines?[4]);
            Assert.StartsWith(expected[5], actualLines?[5]);
            Assert.StartsWith(expected[6], actualLines?[6]);

            Assert.StartsWith(expected[7], actualLines?[7]);
            //Assert.EndsWith(" 253", actualLines?[7]);

            Assert.Equal(expected[8], actualLines?[8]);
        }

        [Fact]
        public void WithCommentHeader_Parallel_Test()
        {
            string actual1 = "", expected1 = $"-- comment1{Environment.NewLine}select 1";
            string actual2 = "", expected2 = $"-- comment2{Environment.NewLine}select 2";
            string actual3 = "", expected3 = $"-- comment3{Environment.NewLine}select 3";
            string actual4 = "", expected4 = $"-- comment4{Environment.NewLine}select 4";

            Task.WaitAll(
                Task.Factory.StartNew(() =>
                {
                    using var connection = new NpgsqlConnection(fixture.ConnectionString);
                    connection.WithComment("comment1").WithCommandCallback(c => actual1 = c.CommandText).Execute("select 1");
                }),
                Task.Factory.StartNew(() =>
                {
                    using var connection = new NpgsqlConnection(fixture.ConnectionString);
                    connection.WithComment("comment2").WithCommandCallback(c => actual2 = c.CommandText).Execute("select 2");
                }),
                Task.Factory.StartNew(() =>
                {
                    using var connection = new NpgsqlConnection(fixture.ConnectionString);
                    connection.WithComment("comment3").WithCommandCallback(c => actual3 = c.CommandText).Execute("select 3");
                }),
                Task.Factory.StartNew(() =>
                {
                    using var connection = new NpgsqlConnection(fixture.ConnectionString);
                    connection.WithComment("comment4").WithCommandCallback(c => actual4 = c.CommandText).Execute("select 4");
                }));

            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
            Assert.Equal(expected3, actual3);
            Assert.Equal(expected4, actual4);
        }

        [Fact]
        public void WithCommentHeader_And_Options_Test()
        {
            var expected = new string[]
            {
                "-- This is my comment",
                "-- Npgsql Text Command. Timeout: 30 seconds.",
                $"-- Timestamp: {DateTime.Now.ToString("o")[..11]}",
                "-- @1 integer = 1",
                "-- @2 text = \"foo\"",
                "-- @3 boolean = false",
                "-- @4 timestamp = \"2022-05-19T00:00:00.0000000\"",
                "-- at WithCommentHeader_And_Options_Test in ",
                "select @1, @2, @3, @4"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            NormOptions.Configure(options =>
            {
                options.CommandCommentHeader.Enabled = true;
                options.CommandCommentHeader.IncludeCommandAttributes = true;
                options.CommandCommentHeader.IncludeTimestamp = true;
                options.DbCommandCallback = cmd => actual = cmd.CommandText;
            });

            connection
                .WithComment("This is my comment")
                .WithCommentCallerInfo()
                .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
                .Execute("select @1, @2, @3, @4");

            var actualLines = actual.Split(Environment.NewLine);

            Assert.Equal(9, actualLines.Length);
            Assert.Equal(expected.Length, actualLines?.Length);
            Assert.Equal(expected[0], actualLines?[0]);
            Assert.Equal(expected[1], actualLines?[1]);
            Assert.StartsWith(expected[2], actualLines?[2]);

            Assert.StartsWith(expected[3], actualLines?[3]);
            Assert.StartsWith(expected[4], actualLines?[4]);
            Assert.StartsWith(expected[5], actualLines?[5]);
            Assert.StartsWith(expected[6], actualLines?[6]);

            Assert.StartsWith(expected[7], actualLines?[7]);
            //Assert.EndsWith(" 341", actualLines?[7]);

            Assert.Equal(expected[8], actualLines?[8]);
        }

        [Fact]
        public void WithCommentHeader_StoredProcedure_Test()
        {
            var expected = new string[]
            {
                "-- Npgsql StoredProcedure Command. Timeout: 30 seconds.",
                "-- @test_param text = \"foo\"",
                "-- at WithCommentHeader_StoredProcedure_Test in ",
                "comment_header_test_func"
            };
            string actual = "";
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection
                .Execute(@"
                create function comment_header_test_func(test_param text) returns text language plpgsql as
                $$
                begin
                    return test_param || ' bar';
                end
                $$")
                .AsProcedure()
                .WithCommentHeader()
                .WithCommandCallback(c => actual = c.CommandText)
                .Read<string?>("comment_header_test_func", new { test_param = "foo" })
                .FirstOrDefault();

            var actualLines = actual.Split(Environment.NewLine);

            Assert.Equal(4, actualLines.Length);
            Assert.Equal(expected.Length, actualLines?.Length);
            
            Assert.Equal(expected[0], actualLines?[0]);
            Assert.Equal(expected[1], actualLines?[1]);
            Assert.StartsWith(expected[2], actualLines?[2]);
            //Assert.EndsWith(" 386", actualLines?[2]);
            Assert.Equal(expected[3], actualLines?[3]);
        }
    }
}