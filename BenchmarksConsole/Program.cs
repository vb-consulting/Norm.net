using System;
using DapperQuery = Dapper.SqlMapper;

using Npgsql;
using PostgreSqlUnitTests;
using Norm;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using var fixture = new PostgreSqlFixture();
using var connection = new NpgsqlConnection(fixture.ConnectionString);


DapperBufferedQueryTests();
DapperUnbufferedQueryTests();
NormReadTests();
NormReadValuesTests();
IterationBenchmarks();

void DapperBufferedQueryTests(int runs = 10, int records = 1000000)
{
    Console.WriteLine("## Dapper Buffered Query Class and Record Tests");
    Console.WriteLine();

    Console.WriteLine("|#|Class Elapsed Sec|Class Allocated KB|Record Elapsed Sec|Record Allocated KB|");
    Console.WriteLine("|-|-----------------|------------------|------------------|-------------------|");

    var sw = new Stopwatch();
    var query = GetQuery(records);
    var list = new List<long[]>();

    for (int i = 0; i < runs; i++)
    {
        var values = new long[4];
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        sw.Reset();
        sw.Start();
        var dapperPoco = DapperQuery.Query<PocoClass>(connection, query);
        sw.Stop();
        long dapperPocoBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
        var dapperPocoElapsed = sw.Elapsed;
        values[0] = dapperPocoElapsed.Ticks;
        values[1] = dapperPocoBytes;
        dapperPoco = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        sw.Reset();
        sw.Start();
        var dapperRecord = DapperQuery.Query<Record>(connection, query);
        sw.Stop();
        long dapperRecordBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
        var dapperRecordElapsed = sw.Elapsed;
        values[2] = dapperRecordElapsed.Ticks;
        values[3] = dapperRecordBytes;
        dapperRecord = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        list.Add(values);
        Console.WriteLine($"|{i + 1}|{dapperPocoElapsed}|{Kb(dapperPocoBytes)}|{dapperRecordElapsed}|{Kb(dapperRecordBytes)}|");
    }

    var dapperPocoElapsedAvg = new TimeSpan((long)list.Select(v => v[0]).Average());
    var dapperPocoBytesAvg = (long)list.Select(v => v[1]).Average();

    var dapperRecordAvg = new TimeSpan((long)list.Select(v => v[2]).Average());
    var dapperRecordBytesAvg = (long)list.Select(v => v[3]).Average();

    Console.WriteLine($"|**AVG**|**{dapperPocoElapsedAvg}**|**{Kb(dapperPocoBytesAvg)}**|**{dapperRecordAvg}**|**{Kb(dapperRecordBytesAvg)}**|");
    Console.WriteLine();
    Console.WriteLine("Finished.");
    Console.WriteLine();
}

void DapperUnbufferedQueryTests(int runs = 10, int records = 1000000)
{
    Console.WriteLine("## Dapper Unbuffered Query Class and Record Tests");
    Console.WriteLine();

    Console.WriteLine("|#|Class Elapsed Sec|Class Allocated KB|Record Elapsed Sec|Record Allocated KB|");
    Console.WriteLine("|-|-----------------|------------------|------------------|-------------------|");

    var sw = new Stopwatch();
    var query = GetQuery(records);
    var list = new List<long[]>();

    for (int i = 0; i < runs; i++)
    {
        var values = new long[4];
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        sw.Reset();
        sw.Start();
        var dapperPoco = DapperQuery.Query<PocoClass>(connection, query, buffered: false).ToList();
        sw.Stop();
        long dapperPocoBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
        var dapperPocoElapsed = sw.Elapsed;
        values[0] = dapperPocoElapsed.Ticks;
        values[1] = dapperPocoBytes;
        dapperPoco = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        sw.Reset();
        sw.Start();
        var dapperRecord = DapperQuery.Query<Record>(connection, query, buffered: false).ToList();
        sw.Stop();
        long dapperRecordBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
        var dapperRecordElapsed = sw.Elapsed;
        values[2] = dapperRecordElapsed.Ticks;
        values[3] = dapperRecordBytes;
        dapperRecord = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        list.Add(values);
        Console.WriteLine($"|{i + 1}|{dapperPocoElapsed}|{Kb(dapperPocoBytes)}|{dapperRecordElapsed}|{Kb(dapperRecordBytes)}|");
    }

    var dapperPocoElapsedAvg = new TimeSpan((long)list.Select(v => v[0]).Average());
    var dapperPocoBytesAvg = (long)list.Select(v => v[1]).Average();

    var dapperRecordAvg = new TimeSpan((long)list.Select(v => v[2]).Average());
    var dapperRecordBytesAvg = (long)list.Select(v => v[3]).Average();

    Console.WriteLine($"|**AVG**|**{dapperPocoElapsedAvg}**|**{Kb(dapperPocoBytesAvg)}**|**{dapperRecordAvg}**|**{Kb(dapperRecordBytesAvg)}**|");
    Console.WriteLine();
    Console.WriteLine("Finished.");
    Console.WriteLine();
}

void NormReadTests(int runs = 10, int records = 1000000)
{
    Console.WriteLine("## Norm Read Class and Record Tests");
    Console.WriteLine();

    Console.WriteLine("|#|Class Elapsed Sec|Class Allocated KB|Record Elapsed Sec|Record Allocated KB|");
    Console.WriteLine("|-|-----------------|------------------|------------------|-------------------|");

    var sw = new Stopwatch();
    var query = GetQuery(records);
    var list = new List<long[]>();

    for (int i = 0; i < 10; i++)
    {
        var values = new long[4];
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        sw.Reset();
        sw.Start();
        var normPoco = connection.Read<PocoClass>(query).ToList();
        sw.Stop();
        long normPocoBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
        var normPocoElapsed = sw.Elapsed;
        values[0] = normPocoElapsed.Ticks;
        values[1] = normPocoBytes;
        normPoco = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        sw.Reset();
        sw.Start();
        var normRecord = connection.Read<Record>(query).ToList();
        sw.Stop();
        long normRecordBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
        var normRecordElapsed = sw.Elapsed;
        values[2] = normRecordElapsed.Ticks;
        values[3] = normRecordBytes;
        normRecord = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        list.Add(values);
        Console.WriteLine($"|{i + 1}|{normPocoElapsed}|{Kb(normPocoBytes)}|{normRecordElapsed}|{Kb(normRecordBytes)}|");
    }

    var normPocoElapsedAvg = new TimeSpan((long)list.Select(v => v[0]).Average());
    var normPocoBytesAvg = (long)list.Select(v => v[1]).Average();

    var normRecordAvg = new TimeSpan((long)list.Select(v => v[2]).Average());
    var normRecordBytesAvg = (long)list.Select(v => v[3]).Average();

    Console.WriteLine($"|**AVG**|**{normPocoElapsedAvg}**|**{Kb(normPocoBytesAvg)}**|**{normRecordAvg}**|**{Kb(normRecordBytesAvg)}**|");
    Console.WriteLine();
    Console.WriteLine("Finished.");
    Console.WriteLine();
}

void NormReadValuesTests(int runs = 10, int records = 1000000)
{
    Console.WriteLine("## Norm Read Values and Tuples vs Raw Data Reader Tests");
    Console.WriteLine();

    Console.WriteLine("|#|Values Elapsed Sec|Values Allocated KB|Tuples Elapsed Sec|Tuples Allocated KB|Raw Reader Elapsed Sec|Raw Reader Allocated KB|");
    Console.WriteLine("|-|------------------|-------------------|------------------|-------------------|----------------------|-----------------------|");

    var sw = new Stopwatch();
    var query = GetQuery(records);
    var list = new List<long[]>();

    for (int i = 0; i < runs; i++)
    {
        var values = new long[6];
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        sw.Reset();
        sw.Start();
        var normValues = connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query).ToList();
        sw.Stop();
        long normValuesBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
        var normValuesElapsed = sw.Elapsed;
        values[0] = normValuesElapsed.Ticks;
        values[1] = normValuesBytes;
        normValues = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        sw.Reset();
        sw.Start();
        var normTuples = connection.Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query).ToList();
        sw.Stop();
        long normTuplesBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
        var normTuplesElapsed = sw.Elapsed;
        values[2] = normTuplesElapsed.Ticks;
        values[3] = normTuplesBytes;
        normTuples = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        sw.Reset();
        sw.Start();
        var rawReader = new List<PocoClass>();
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
            rawReader.Add(new PocoClass
            {
                Id1 = id1,
                Foo1 = (foo1 as object == DBNull.Value ? null : foo1),
                Bar1 = (bar1 as object == DBNull.Value ? null : bar1),
                DateTime1 = dateTime1,
                Foo2 = (foo2 as object == DBNull.Value ? null : foo2),
                Bar2 = (bar2 as object == DBNull.Value ? null : bar2),
                DateTime2 = dateTime2,
                LongFooBar = (longFooBar as object == DBNull.Value ? null : longFooBar),
                IsFooBar = isFooBar,
            });
        }
        sw.Stop();
        long rawReaderBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
        var rawReaderElapsed = sw.Elapsed;
        values[4] = rawReaderElapsed.Ticks;
        values[5] = rawReaderBytes;
        rawReader = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);

        list.Add(values);
        Console.WriteLine($"|{i + 1}|{normValuesElapsed}|{Kb(normValuesBytes)}|{normTuplesElapsed}|{Kb(normTuplesBytes)}|{rawReaderElapsed}|{Kb(rawReaderBytes)}|");
    }

    var normValuesElapsedAvg = new TimeSpan((long)list.Select(v => v[0]).Average());
    var normValuesBytesAvg = (long)list.Select(v => v[1]).Average();

    var normTuplesAvg = new TimeSpan((long)list.Select(v => v[2]).Average());
    var normTuplesBytesAvg = (long)list.Select(v => v[3]).Average();

    var rawReaderAvg = new TimeSpan((long)list.Select(v => v[4]).Average());
    var rawReaderBytesAvg = (long)list.Select(v => v[5]).Average();

    Console.WriteLine($"|**AVG**|**{normValuesElapsedAvg}**|**{Kb(normValuesBytesAvg)}**|**{normTuplesAvg}**|**{Kb(normTuplesBytesAvg)}**|**{rawReaderAvg}**|**{Kb(rawReaderBytesAvg)}**|");
    Console.WriteLine();
    Console.WriteLine("Finished.");
    Console.WriteLine();
}

void IterationBenchmarks(int runs = 10, int records = 1000000)
{
    var sw = new Stopwatch();
    var query = GetQuery(records);

    Console.WriteLine("## Dapper Query and Iteration (Buffered and Unbuffered) vs Norm Read and Iteration Tests");
    Console.WriteLine();

    Console.WriteLine("|#|Dapper Buffered Query|Dapper Buffered Iteration|Daper Buffered Total|Dapper Unbuffered Query|Dapper Unbuffered Iteration|Daper Unbuffered Total|Norm Read|Norm Iteration|Norm Total|");
    Console.WriteLine("|-|---------------------|-------------------------|--------------------|-----------------------|---------------------------|----------------------|------------------------|----------|");

    var list = new List<long[]>();

    for (int i = 0; i < runs; i++)
    {
        var values = new long[9];

        sw.Reset();
        sw.Start();
        var dapperQuery = DapperQuery.Query<PocoClass>(connection, query);
        sw.Stop();

        var dapperQueryElapsed = sw.Elapsed;
        values[0] = sw.Elapsed.Ticks;

        sw.Reset();
        sw.Start();
        foreach (var row in dapperQuery)
        {
            // do something
        }
        sw.Stop();
        var dapperIterationElapsed = sw.Elapsed;
        values[1] = sw.Elapsed.Ticks;

        var dapperTotal = dapperQueryElapsed + dapperIterationElapsed;
        values[2] = dapperTotal.Ticks;
        dapperQuery = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);



        sw.Reset();
        sw.Start();
        var dapperUnbufferedQuery = DapperQuery.Query<PocoClass>(connection, query, buffered: false);
        sw.Stop();

        var dapperUnbufferedQueryElapsed = sw.Elapsed;
        values[3] = sw.Elapsed.Ticks;

        sw.Reset();
        sw.Start();
        foreach (var row in dapperUnbufferedQuery)
        {
            // do something
        }
        sw.Stop();
        var dapperUnbufferedIterationElapsed = sw.Elapsed;
        values[4] = sw.Elapsed.Ticks;

        var dapperUnbufferedTotal = dapperUnbufferedQueryElapsed + dapperUnbufferedIterationElapsed;
        values[5] = dapperUnbufferedTotal.Ticks;
        dapperUnbufferedQuery = null;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);



        sw.Reset();
        sw.Start();
        var normRead = connection.Read<PocoClass>(query);
        sw.Stop();

        var normReadElapsed = sw.Elapsed;
        values[6] = sw.Elapsed.Ticks;

        sw.Reset();
        sw.Start();
        foreach (var row in normRead)
        {
            // do something
        }
        sw.Stop();
        var normReadIteration = sw.Elapsed;
        values[7] = sw.Elapsed.Ticks;
        normRead = null;

        var normTotal = normReadElapsed + normReadIteration;
        values[8] = normTotal.Ticks;
        GC.Collect(int.MaxValue, GCCollectionMode.Forced);


        list.Add(values);
        Console.WriteLine($"|{i + 1}|{dapperQueryElapsed}|{dapperIterationElapsed}|{dapperTotal}|{dapperUnbufferedQueryElapsed}|{dapperUnbufferedIterationElapsed}|{dapperUnbufferedTotal}|{normReadElapsed}|{normReadIteration}|{normTotal}|");
    }

    var dapperQueryAvg = new TimeSpan((long)list.Select(v => v[0]).Average());
    var dapperIterationAvg = new TimeSpan((long)list.Select(v => v[1]).Average());
    var dapperTotalAvg = new TimeSpan((long)list.Select(v => v[2]).Average());

    var dapperBufferedQueryAvg = new TimeSpan((long)list.Select(v => v[3]).Average());
    var dapperBufferedIterationAvg = new TimeSpan((long)list.Select(v => v[4]).Average());
    var dapperBufferedTotalAvg = new TimeSpan((long)list.Select(v => v[5]).Average());

    var normReadAvg = new TimeSpan((long)list.Select(v => v[6]).Average());
    var normIterationAvg = new TimeSpan((long)list.Select(v => v[7]).Average());
    var normTotalAvg = new TimeSpan((long)list.Select(v => v[8]).Average());

    Console.WriteLine($"|**AVG**|**{dapperQueryAvg}**|**{dapperIterationAvg}**|**{dapperTotalAvg}**|**{dapperBufferedQueryAvg}**|**{dapperBufferedIterationAvg}**|**{dapperBufferedTotalAvg}**|**{normReadAvg}**|**{normIterationAvg}**|**{normTotalAvg}**|");
    Console.WriteLine();
    Console.WriteLine("Finished.");
    Console.WriteLine();
}


string GetQuery(int records)
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

static double Kb(long kilobytes)
{
    return kilobytes / 1024f;
}

class PocoClass
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

record Record(int Id1, string Foo1, string Bar1, DateTime DateTime1, int Id2, string Foo2, string Bar2, DateTime DateTime2, string LongFooBar, bool IsFooBar);
