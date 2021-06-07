using System;
using Npgsql;
using PostgreSqlUnitTests;
using BenchmarksConsole;

using var fixture = new PostgreSqlFixture();
using var connection = new NpgsqlConnection(fixture.ConnectionString);
connection.Open();

new DapperBufferedQueryTests(connection).Run();
new DapperUnbufferedQueryTests(connection).Run();
new NormReadTests(connection).Run();
new NormReadValuesTests(connection).Run();
new IterationBenchmarks(connection).Run();


new DapperBufferedQueryTests(connection).RunAsync().GetAwaiter().GetResult();
//new DapperUnbufferedQueryTests(connection).RunAsync().GetAwaiter().GetResult();
new NormReadTests(connection).RunAsync().GetAwaiter().GetResult();
new NormReadValuesTests(connection).RunAsync().GetAwaiter().GetResult();
new IterationBenchmarks(connection).RunAsync().GetAwaiter().GetResult();

public class PocoClass
{
    public int Id1 { get; set; }
    public string Foo1 { get; set; }
    public string Bar1 { get; set; }
    public DateTime DateTime1 { get; set; }
    public int Id2 { get; set; }
    public string Foo2 { get; set; }
    public string Bar2 { get; set; }
    public DateTime DateTime2 { get; set; }
    public string LongFooBar { get; set; }
    public bool IsFooBar { get; set; }
}

public record Record(int Id1, string Foo1, string Bar1, DateTime DateTime1, int Id2, string Foo2, string Bar2, DateTime DateTime2, string LongFooBar, bool IsFooBar);
