using BenchmarkDotNet.Attributes;
using Npgsql;

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
    [MemoryDiagnoser]
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

        [Params(10, 1_000, 10_000, 100_000)]
        public int Records { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            query = GetQuery(Records);
            connection = new NpgsqlConnection(Connection.ConnectionString);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            connection.Dispose();
        }

        [Benchmark(Baseline = true)]
        public void Dapper()
        {
            foreach (var i in connection.Query<PocoClass>(query))
            {
                var c = i;
            }
        }

        [Benchmark()]
        public void Dapper_Buffered_False()
        {
            foreach (var i in connection.Query<PocoClass>(query, buffered: false))
            {
                var c = i;
            }
        }

        [Benchmark()]
        public void Norm_NameValue_Array()
        {
            foreach (var i in connection.Read(query))
            {
                var c = i;
            }
        }

        [Benchmark()]
        public void Norm_PocoClass_Instances()
        {
            foreach (var i in connection.Read<PocoClass>(query))
            {
                var c = i;
            }
        }

        [Benchmark()]
        public void Norm_Tuples()
        {
            foreach (var i in connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query))
            {
                var c = i;
            }
        }

        [Benchmark()]
        public void Norm_Named_Tuples()
        {
            foreach (var i in connection.Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query))
            {
                var c = i;
            }
        }

        [Benchmark()]
        public void Norm_Anonymous_Types()
        {
            foreach (var i in connection.Read(new
            {
                id1 = default(int),
                foo1 = default(string),
                bar1 = default(string),
                datetime1 = default(DateTime),
                id2 = default(int),
                foo2 = default(string),
                bar2 = default(string),
                datetime2 = default(DateTime),
                longFooBar = default(string),
                isFooBar = default(bool),
            }, 
            query))
            {
                var c = i;
            }
        }

        [Benchmark()]
        public void Norm_PocoClass_Instances_ReaderCallback()
        {
            foreach (var i in connection
                .WithReaderCallback(o => o.Ordinal switch
                {
                    0 => o.Reader.GetInt32(o.Ordinal),
                    _ => null
                })
                .Read<PocoClass>(query))
            {
                var c = i;
            }
        }

        [Benchmark()]
        public void Norm_Tuples_ReaderCallback()
        {
            foreach (var i in connection
                .WithReaderCallback(o => o.Ordinal switch
                {
                    0 => o.Reader.GetInt32(o.Ordinal),
                    _ => null
                })
                .Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query))
            {
                var c = i;
            }
        }

        [Benchmark()]
        public void Norm_Named_Tuples_ReaderCallback()
        {
            foreach (var i in connection
                .WithReaderCallback(o => o.Ordinal switch
                {
                    0 => o.Reader.GetInt32(o.Ordinal),
                    _ => null
                })
                .Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query))
            {
                var c = i;
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
                var i = new PocoClass
                {
                    Id1 = reader.GetInt32(0),
                    Foo1 = reader.GetString(1),
                    Bar1 = reader.GetString(2),
                    DateTime1 = reader.GetDateTime(3),
                    Id2 = reader.GetInt32(4),
                    Foo2 = reader.GetString(5),
                    Bar2 = reader.GetString(6),
                    DateTime2 = reader.GetDateTime(7),
                    LongFooBar = reader.GetString(8),
                    IsFooBar = reader.GetBoolean(9),
                };

                var c = i;
            }
        }
    }
}
