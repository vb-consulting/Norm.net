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

Console.WriteLine("Running...");
Console.WriteLine();



//RunSerializationBenchmarks();
//RunLinqExpBenchmarks();


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

#pragma warning disable CS8321 // Local function is declared but never used
void RunSerializationBenchmarks()
#pragma warning restore CS8321 // Local function is declared but never used
{
    var sw = new Stopwatch();
    var query = GetQuery(1000000);

    Console.WriteLine("|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|Norm TUPLES|Raw DataReader|");
    Console.WriteLine("|-|-----------|-------------|---------|-----------|-----------|--------------|");

    var list = new List<long[]>();

    for (int i = 0; i<10; i++)
    {
        var values = new long[6];

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

        GC.Collect();
        sw.Reset();
        sw.Start();
        IEnumerable<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)> normTuples = 
            connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query).ToList();
        sw.Stop();
        var normTuplesElapsed = sw.Elapsed;
        values[4] = sw.Elapsed.Ticks;

        GC.Collect();
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
        var rawDataReaderElapsed = sw.Elapsed;
        values[5] = sw.Elapsed.Ticks;

        list.Add(values);
        Console.WriteLine($"|{i+1}|{dapperPocoElapsed}|{dapperRecordElapsed}|{normPocoElapsed}|{normRecordElapsed}|{normTuplesElapsed}|{rawDataReaderElapsed}|");
    }

    var dapperPocoAvg = new TimeSpan((long)list.Select(v => v[0]).Average());
    var dapperRecordAvg = new TimeSpan((long)list.Select(v => v[1]).Average());
    var normPocoAvg = new TimeSpan((long)list.Select(v => v[2]).Average());
    var normRecordAvg = new TimeSpan((long)list.Select(v => v[3]).Average());
    var normTuplesAvg = new TimeSpan((long)list.Select(v => v[4]).Average());
    var rawDataReaderAvg = new TimeSpan((long)list.Select(v => v[5]).Average());

    Console.WriteLine($"|AVG|{dapperPocoAvg}|{dapperRecordAvg}|{normPocoAvg}|{normRecordAvg}|{normTuplesAvg}|{rawDataReaderAvg}|");
    Console.WriteLine();
}

#pragma warning disable CS8321 // Local function is declared but never used
void RunLinqExpBenchmarks()
{
    var sw = new Stopwatch();
    var query = GetQuery(1000000);

    Console.WriteLine("|#|Dapper POCO to Dict|Norm TUPLES to Dict|Norm POCO to Dict|");
    Console.WriteLine("|-|-------------------|-------------------|-----------------|");

    var list = new List<long[]>();

    for (int i = 0; i < 10; i++)
    {
        var values = new long[3];

        GC.Collect();
        sw.Reset();
        sw.Start();
        var dapper = DapperQuery.Query<PocoClass>(connection, query).ToDictionary(r => r.Id1, r => r.DateTime1);
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
        var normRecord = connection.Query<PocoClass>(query).ToDictionary(r => r.Id1, r => r.DateTime1);
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



