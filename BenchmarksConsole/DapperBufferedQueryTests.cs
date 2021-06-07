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

        public TimeSpan DapperPocoElapsedSync { get; private set; }
        public double DapperPocoKbSync { get; private set; }
        public TimeSpan DapperPocoElapsedAsync { get; private set; }
        public double DapperPocoKbAsync { get; private set; }
        public TimeSpan DapperRecordElapsedSync { get; private set; }
        public double DapperRecordKbSync { get; private set; }
        public TimeSpan DapperRecordElapsedAsync { get; private set; }
        public double DapperRecordKbAsync { get; private set; }

        public void Run(int runs = Config.Runs, int records = Config.Records)
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

            DapperPocoElapsedSync = dapperPocoElapsedAvg;
            DapperPocoKbSync = Kb(dapperPocoBytesAvg);

            DapperRecordElapsedSync = dapperRecordAvg;
            DapperRecordKbSync = Kb(dapperRecordBytesAvg);

            Console.WriteLine($"|**AVG**|**{DapperPocoElapsedSync}**|**{DapperPocoKbSync}**|**{DapperRecordElapsedSync}**|**{DapperRecordElapsedSync}**|");
            Console.WriteLine();
            Console.WriteLine("Finished.");
            Console.WriteLine();

            
        }

        public async ValueTask RunAsync(int runs = Config.Runs, int records = Config.Records)
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


            DapperPocoElapsedAsync = dapperPocoElapsedAvg;
            DapperPocoKbAsync = Kb(dapperPocoBytesAvg);

            DapperRecordElapsedAsync = dapperRecordAvg;
            DapperRecordKbAsync = Kb(dapperRecordBytesAvg);

            Console.WriteLine($"|**AVG**|**{DapperPocoElapsedAsync}**|**{DapperPocoKbAsync}**|**{DapperRecordElapsedAsync}**|**{DapperRecordKbAsync}**|");
            Console.WriteLine();
            Console.WriteLine("Finished.");
            Console.WriteLine();
        }
    }
}
