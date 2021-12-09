using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Npgsql;
using PostgreSqlUnitTests;
//using DapperQuery = Dapper.SqlMapper;
using Dapper;
using Norm;

namespace Benchmarks6
{
    public class PocoClass
    {
        public int Id1 { get; set; }
        public string? Foo1 { get; set; }
        public string? Bar1 { get; set; }
        public DateTime DateTime1 { get; set; }
        public int Id2 { get; set; }
        public string? Foo2 { get; set; }
        public string? Bar2 { get; set; }
        public DateTime DateTime2 { get; set; }
        public string? LongFooBar { get; set; }
        public bool IsFooBar { get; set; }
    }


    [KeepBenchmarkFiles]
    [MarkdownExporter]
    [MarkdownExporterAttribute.GitHub]    
    public class Benchmarks
    {
        private static string GetQuery(int records)
        {
            return $@"
    select 
        i as id1, 
        'foo' || i::text as foo1, 
        'bar' || i::text as bar1, 
        ('2000-01-01'::date) + (i::text || ' days')::interval as datetime1, 
        i+1 as id2, 
        'foo' || (i+1)::text as foo2, 
        'bar' || (i+1)::text as bar2, 
        ('2000-01-01'::date) + ((i+1)::text || ' days')::interval as datetime2,
        'long_foo_bar_' || (i+2)::text as longfoobar, 
        (i % 2)::boolean as isfoobar
    from generate_series(1, {records}) as i
";
        }

        private string query = default!;
        private NpgsqlConnection connection = default!;
        private PostgreSqlFixture fixture = default!;

        [Params(10, 1_000, 10_000, 100_000, 1_000_000)]
        public int Records { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            query = GetQuery(Records);
            fixture = new PostgreSqlFixture();
            connection = new NpgsqlConnection(fixture.ConnectionString);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            fixture.Dispose();
            connection.Dispose();
        }

        [Benchmark(Baseline = true)]
        public void Dapper()
        {
            foreach (var i in connection.Query<PocoClass>(query))
            {
            }
        }

        [Benchmark()]
        public void Dapper_Buffered_False()
        {
            foreach (var i in connection.Query<PocoClass>(query, buffered: false))
            {
            }
        }

        [Benchmark()]
        public void Norm_NameValue_Array()
        {
            foreach (var i in connection.Read(query))
            {
            }
        }

        [Benchmark()]
        public void Norm_PocoClass_Instances()
        {
            foreach (var i in connection.Read<PocoClass>(query))
            {
            }
        }

        [Benchmark()]
        public void Norm_Tuples()
        {
            foreach (var i in connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query))
            {
            }
        }

        [Benchmark()]
        public void Norm_Named_Tuples()
        {
            foreach (var i in connection.Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query))
            {
            }
        }

        [Benchmark()]
        public void Command_Reader()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            using var command = new NpgsqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id1 = reader.GetInt32(0);
                var foo1 = reader.GetString(1);
                var bar1 = reader.GetString(2);
                var dateTime1 = reader.GetDateTime(3);
                var id2 = reader.GetInt32(4);
                var foo2 = reader.GetString(5);
                var bar2 = reader.GetString(6);
                var dateTime2 = reader.GetDateTime(7);
                var longFooBar = reader.GetString(8);
                var isFooBar = reader.GetBoolean(9);
            }
        }
    }
}
