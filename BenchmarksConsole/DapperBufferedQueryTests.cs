using System;
using System.Collections.Generic;
using System.Linq;
using DapperQuery = Dapper.SqlMapper;
using Npgsql;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BenchmarksConsole
{
    public class DapperBufferedQueryTests : TestBase
    {
        public DapperBufferedQueryTests(NpgsqlConnection connection) : base(connection) { }

        public void Run(int runs = 10, int records = 1000000)
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

        public async ValueTask RunAsync(int runs = 10, int records = 1000000)
        {
            Console.WriteLine("## Dapper Buffered Query Class and Record Async Tests");
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
                var dapperPoco = await DapperQuery.QueryAsync<PocoClass>(connection, query);
                sw.Stop();
                long dapperPocoBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
                var dapperPocoElapsed = sw.Elapsed;
                values[0] = dapperPocoElapsed.Ticks;
                values[1] = dapperPocoBytes;
                dapperPoco = null;
                GC.Collect(int.MaxValue, GCCollectionMode.Forced);

                sw.Reset();
                sw.Start();
                var dapperRecord = await DapperQuery.QueryAsync<Record>(connection, query);
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
    }
}
