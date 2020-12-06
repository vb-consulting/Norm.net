using System;
using DapperQuery = Dapper.SqlMapper;

using Npgsql;
using PostgreSqlUnitTests;
using Norm.Extensions;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using var fixture = new PostgreSqlFixture();
using var connection = new NpgsqlConnection(fixture.ConnectionString);

Console.WriteLine("Running...");
Console.WriteLine();


//RunSerializationBenchmarks();
RunLinqExpBenchmarks();

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

void RunSerializationBenchmarks()
{
    var sw = new Stopwatch();
    var query = GetQuery(1000000);

    Console.WriteLine("|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|");
    Console.WriteLine("|-|-----------|-------------|---------|-----------|");

    var list = new List<long[]>();

    for (int i = 0; i<10; i++)
    {
        var values = new long[4];

        GC.Collect();
        sw.Reset();
        sw.Start();
        var dapperPoco = DapperQuery.Query<PocoClass>(connection, query);
        sw.Stop();
        var dapperPocoElapsed = sw.Elapsed;
        values[0] = sw.Elapsed.Ticks;

        GC.Collect();
        sw.Reset();
        sw.Start();
        var dapperRecord = DapperQuery.Query<Record>(connection, query);
        sw.Stop();
        var dapperRecordElapsed = sw.Elapsed;
        values[1] = sw.Elapsed.Ticks;

        GC.Collect();
        sw.Reset();
        sw.Start();
        var normPoco = connection.Query<PocoClass>(query).ToList();
        sw.Stop();
        var normPocoElapsed = sw.Elapsed;
        values[2] = sw.Elapsed.Ticks;

        GC.Collect();
        sw.Reset();
        sw.Start();
        var normRecord = connection.Query<Record>(query).ToList();
        sw.Stop();
        var normRecordElapsed = sw.Elapsed;
        values[3] = sw.Elapsed.Ticks;

        list.Add(values);
        Console.WriteLine($"|{i+1}|{dapperPocoElapsed}|{dapperRecordElapsed}|{normPocoElapsed}|{normRecordElapsed}|");
    }

    var dapperPocoAvg = new TimeSpan((long)list.Select(v => v[0]).Average());
    var dapperRecordAvg = new TimeSpan((long)list.Select(v => v[1]).Average());
    var normPocoAvg = new TimeSpan((long)list.Select(v => v[2]).Average());
    var normRecordAvg = new TimeSpan((long)list.Select(v => v[3]).Average());

    Console.WriteLine($"|AVG|{dapperPocoAvg}|{dapperRecordAvg}|{normPocoAvg}|{normRecordAvg}|");
    Console.WriteLine();
}

void RunLinqExpBenchmarks()
{
    var sw = new Stopwatch();
    var query = GetQuery(1000000);

    Console.WriteLine("|#|Dapper RECORD to Dict|Norm TUPLES to Dict|Norm RECORD to Dict|");
    Console.WriteLine("|-|---------------------|-------------------|-------------------|");

    var list = new List<long[]>();

    for (int i = 0; i < 10; i++)
    {
        var values = new long[3];

        GC.Collect();
        sw.Reset();
        sw.Start();
        var dapper = DapperQuery.Query<Record>(connection, query).ToDictionary(r => r.Id1, r => r.DateTime1);
        sw.Stop();
        var dapperRecordElapsed = sw.Elapsed;
        values[0] = sw.Elapsed.Ticks;

        GC.Collect();
        sw.Reset();
        sw.Start();
        var normTuples = connection.Read(query).ToDictionary(t => t[0].value, t => (DateTime)t[3].value);
        sw.Stop();
        var dapperTuplesElapsed = sw.Elapsed;
        values[1] = sw.Elapsed.Ticks;

        GC.Collect();
        sw.Reset();
        sw.Start();
        var normRecord = connection.Query<Record>(query).ToDictionary(r => r.Id1, r => r.DateTime1);
        sw.Stop();
        var normRecordElapsed = sw.Elapsed;
        values[2] = sw.Elapsed.Ticks;

        list.Add(values);
        Console.WriteLine($"|{i + 1}|{dapperRecordElapsed}|{dapperTuplesElapsed}|{normRecordElapsed}|");
    }

    var dapperAvg = new TimeSpan((long)list.Select(v => v[0]).Average());
    var normTuplesAvg = new TimeSpan((long)list.Select(v => v[1]).Average());
    var normRecordAvg = new TimeSpan((long)list.Select(v => v[2]).Average());

    Console.WriteLine($"|AVG|{dapperAvg}|{normTuplesAvg}|{normRecordAvg}|");
    Console.WriteLine();
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

record Record(
    int Id1, 
    string Foo1, 
    string Bar1, 
    DateTime DateTime1, 
    int Id2, 
    string Foo2, 
    string Bar2, 
    DateTime DateTime2,
    string LongFooBar,
    bool IsFooBar);



