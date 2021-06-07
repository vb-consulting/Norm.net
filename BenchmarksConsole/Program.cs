using System;
using Npgsql;
using PostgreSqlUnitTests;
using BenchmarksConsole;


using var fixture = new PostgreSqlFixture();
using var connection = new NpgsqlConnection(fixture.ConnectionString);
connection.Open();


var dapperBufferedQueryTests = new DapperBufferedQueryTests(connection);
var dapperUnbufferedQueryTests = new DapperUnbufferedQueryTests(connection);
var normReadTests = new NormReadTests(connection);
var normReadValuesTests = new NormReadValuesTests(connection);
var iterationBenchmarks = new IterationBenchmarks(connection);

dapperBufferedQueryTests.Run();
dapperUnbufferedQueryTests.Run();
normReadTests.Run();
normReadValuesTests.Run();
iterationBenchmarks.Run();


dapperBufferedQueryTests.RunAsync().GetAwaiter().GetResult();
//new DapperUnbufferedQueryTests(connection).RunAsync().GetAwaiter().GetResult();
normReadTests.RunAsync().GetAwaiter().GetResult();
normReadValuesTests.RunAsync().GetAwaiter().GetResult();
iterationBenchmarks.RunAsync().GetAwaiter().GetResult();


Console.WriteLine("***********************************************************************************************");
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("## Aggregated Results");
Console.WriteLine();
Console.WriteLine("### Dapper");
Console.WriteLine();
Console.WriteLine("|Operation|Average Time in Sec.|Average Memory Consuption in KB|");
Console.WriteLine("|---------|-----------------------|----------------------------|");
Console.WriteLine($"|Buffered Dapper `Query<class>`|{dapperBufferedQueryTests.DapperPocoElapsedSync.TotalSeconds}.{dapperBufferedQueryTests.DapperPocoElapsedSync.Milliseconds}|{dapperBufferedQueryTests.DapperPocoKbSync.ToString("0")}|");
Console.WriteLine($"|Buffered Dapper `Query<record>`|{dapperBufferedQueryTests.DapperRecordElapsedSync.TotalSeconds}.{dapperBufferedQueryTests.DapperRecordElapsedSync.Milliseconds}|{dapperBufferedQueryTests.DapperRecordKbSync.ToString("0")}|");
Console.WriteLine($"|Unbuffered Dapper `Query<class>`|{dapperUnbufferedQueryTests.DapperPocoElapsedSync.TotalSeconds}.{dapperUnbufferedQueryTests.DapperPocoElapsedSync.Milliseconds}|{dapperUnbufferedQueryTests.DapperPocoKbSync.ToString("0")}|");
Console.WriteLine($"|Unbuffered Dapper `Query<record>`|{dapperUnbufferedQueryTests.DapperRecordElapsedSync.TotalSeconds}.{dapperUnbufferedQueryTests.DapperRecordElapsedSync.Milliseconds}|{dapperUnbufferedQueryTests.DapperRecordKbSync.ToString("0")}|");
Console.WriteLine($"|Async Buffered Dapper `QueryAsync<class>`|{dapperBufferedQueryTests.DapperPocoElapsedAsync.TotalSeconds}.{dapperBufferedQueryTests.DapperPocoElapsedAsync.Milliseconds}|{dapperBufferedQueryTests.DapperPocoKbAsync.ToString("0")}|");
Console.WriteLine($"|Async Buffered Dapper `QueryAsync<record>`|{dapperBufferedQueryTests.DapperRecordElapsedAsync.TotalSeconds}.{dapperBufferedQueryTests.DapperRecordElapsedAsync.Milliseconds}|{dapperBufferedQueryTests.DapperRecordKbAsync.ToString("0")}|");
Console.WriteLine($"|Async Unbuffered Dapper `QueryAsync<class>`|Not Available|Not Available|");
Console.WriteLine($"|Async Unbuffered Dapper `QueryAsync<record>`|Not Available|Not Available|");
Console.WriteLine();
Console.WriteLine($"### Norm");
Console.WriteLine();
Console.WriteLine($"|Operation|Average Time in Sec.|Average Memory Consuption in KB|");
Console.WriteLine($"|---------|-----------------------|----------------------------|");
Console.WriteLine($"|Norm `Read<class>`|{normReadTests.NormPocoElapsedSync.TotalSeconds}.{normReadTests.NormPocoElapsedSync.Milliseconds}|{normReadTests.NormPocoKbSync.ToString("0")}|");
Console.WriteLine($"|Norm `Read<record>`|{normReadTests.NormRecordElapsedSync.TotalSeconds}.{normReadTests.NormRecordElapsedSync.Milliseconds}|{normReadTests.NormRecordKbSync.ToString("0")}|");
Console.WriteLine($"|Norm `Read<built-in types>`|{normReadValuesTests.NormBuiltInElapsedSync.TotalSeconds}.{normReadValuesTests.NormBuiltInElapsedSync.Milliseconds}|{normReadValuesTests.NormBuiltInKbSync.ToString("0")}|");
Console.WriteLine($"|Norm `Read<tuple>`|{normReadValuesTests.NormTuplesElapsedSync.TotalSeconds}.{normReadValuesTests.NormTuplesElapsedSync.Milliseconds}|{normReadValuesTests.NormTuplesKbSync.ToString("0")}|");
Console.WriteLine($"|Norm `ReadAsync<class>`|{normReadTests.NormPocoElapsedAsync.TotalSeconds}.{normReadTests.NormPocoElapsedAsync.Milliseconds}|{normReadTests.NormPocoKbAsync.ToString("0")}|");
Console.WriteLine($"|Norm `ReadAsync<record>`|{normReadTests.NormRecordElapsedAsync.TotalSeconds}.{normReadTests.NormRecordElapsedAsync.Milliseconds}|{normReadTests.NormRecordKbAsync.ToString("0")}|");
Console.WriteLine($"|Norm `ReadAsync<built-in types>`|{normReadValuesTests.NormBuiltInElapsedAsync.TotalSeconds}.{normReadValuesTests.NormBuiltInElapsedAsync.Milliseconds}|{normReadValuesTests.NormBuiltInKbAsync.ToString("0")}|");
Console.WriteLine($"|Norm `ReadAsync<tuple>`|{normReadValuesTests.NormTuplesElapsedAsync.TotalSeconds}.{normReadValuesTests.NormTuplesElapsedAsync.Milliseconds}|{normReadValuesTests.NormTuplesKbAsync.ToString("0")}|");
Console.WriteLine();
Console.WriteLine($"### Raw Data Reader");
Console.WriteLine();
Console.WriteLine($"|Operation|Average Time in Sec.|Average Memory Consuption in KB|");
Console.WriteLine($"|---------|-----------------------|----------------------------|");
Console.WriteLine($"|Raw Data Reader|{normReadValuesTests.RawElapsedSync.TotalSeconds}.{normReadValuesTests.RawElapsedSync.Milliseconds}|{normReadValuesTests.RawKbSync.ToString("0")}|");
Console.WriteLine($"|Raw Data Reader Async|{normReadValuesTests.RawElapsedAsync.TotalSeconds}.{normReadValuesTests.RawElapsedAsync.Milliseconds}|{normReadValuesTests.RawKbAsync.ToString("0")}|");
Console.WriteLine();
Console.WriteLine($"### Dapper Buffered and Unbuffered vs Norm execution and iteration");
Console.WriteLine();
Console.WriteLine($"|Operation|Execution Sec.|Iteration Sec.|Total Sec.|");
Console.WriteLine($"|---------|--------------|--------------|----------|");
Console.WriteLine($"|Buffered Dapper `Query`|{iterationBenchmarks.DapperBufferedQueryAvg}|{iterationBenchmarks.DapperBufferedIterationAvg}|{iterationBenchmarks.DapperBufferedTotalAvg}|");
Console.WriteLine($"|Unbuffered Dapper `Query`|{iterationBenchmarks.DapperQueryAvg}|{iterationBenchmarks.DapperIterationAvg}|{iterationBenchmarks.DapperQueryAvg}|");
Console.WriteLine($"|Norm `Read`|00.0000125|{iterationBenchmarks.NormReadAvg}|{iterationBenchmarks.NormIterationAvg}|{iterationBenchmarks.NormTotalAvg}|");
Console.WriteLine();

class Config
{
    public const int Runs = 10;
    public const int Records = 1000000;
}

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
