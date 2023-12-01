using BenchmarkDotNet.Attributes;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace NormBenchmarks;

[KeepBenchmarkFiles]
[MarkdownExporter]
[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
public partial class Benchmarks
{
    public static string GetQuery(int records)
    {
        return $@"select 
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
    private DbContext dbcontext = default!;

    //[Params(10, 1_000, 10_000, 100_000)]
    [Params(1)]
    public int Records { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        query = GetQuery(Records);
        connection = new NpgsqlConnection(Connection.String);
        dbcontext = new DbContext(
            new DbContextOptionsBuilder()
            .UseNpgsql(Connection.String)
            .Options);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        connection.Dispose();
        dbcontext.Dispose();
    }
}
