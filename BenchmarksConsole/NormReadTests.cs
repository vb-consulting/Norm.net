using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using System.Diagnostics;

namespace BenchmarksConsole
{
    public class NormReadTests : TestBase
    {
        public NormReadTests(NpgsqlConnection connection) : base(connection) { }

        public TimeSpan NormPocoElapsedSync { get; private set; }
        public double NormPocoKbSync { get; private set; }
        public TimeSpan NormPocoElapsedAsync { get; private set; }
        public double NormPocoKbAsync { get; private set; }
        public TimeSpan NormRecordElapsedSync { get; private set; }
        public double NormRecordKbSync { get; private set; }
        public TimeSpan NormRecordElapsedAsync { get; private set; }
        public double NormRecordKbAsync { get; private set; }

        public void Run(int runs = Config.Runs, int records = Config.Records)
        {
            Console.WriteLine("## Norm Read Class and Record Tests");
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

            NormPocoElapsedSync = normPocoElapsedAvg;
            NormPocoKbSync = Kb(normPocoBytesAvg);

            NormRecordElapsedSync = normRecordAvg;
            NormRecordKbSync = Kb(normRecordBytesAvg);

            Console.WriteLine($"|**AVG**|**{normPocoElapsedAvg}**|**{Kb(normPocoBytesAvg)}**|**{normRecordAvg}**|**{Kb(normRecordBytesAvg)}**|");
            Console.WriteLine();
            Console.WriteLine("Finished.");
            Console.WriteLine();
        }

        public async ValueTask RunAsync(int runs = Config.Runs, int records = Config.Records)
        {
            Console.WriteLine("## Norm Read Class and Record Async Tests");
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
                var normPoco = await connection.ReadAsync<PocoClass>(query).ToListAsync();
                sw.Stop();
                long normPocoBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
                var normPocoElapsed = sw.Elapsed;
                values[0] = normPocoElapsed.Ticks;
                values[1] = normPocoBytes;
                normPoco = null;
                GC.Collect(int.MaxValue, GCCollectionMode.Forced);

                sw.Reset();
                sw.Start();
                var normRecord = await connection.ReadAsync<Record>(query).ToListAsync();
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

            NormPocoElapsedAsync = normPocoElapsedAvg;
            NormPocoKbAsync = Kb(normPocoBytesAvg);

            NormRecordElapsedAsync = normRecordAvg;
            NormRecordKbAsync = Kb(normRecordBytesAvg);

            Console.WriteLine($"|**AVG**|**{NormPocoElapsedAsync}**|**{NormPocoKbAsync}**|**{NormRecordElapsedAsync}**|**{NormRecordKbAsync}**|");
            Console.WriteLine();
            Console.WriteLine("Finished.");
            Console.WriteLine();
        }
    }
}
