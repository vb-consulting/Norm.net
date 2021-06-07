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
    public class NormReadValuesTests : TestBase
    {
        public NormReadValuesTests(NpgsqlConnection connection) : base(connection) { }

        public TimeSpan NormBuiltInElapsedSync { get; private set; }
        public double NormBuiltInKbSync { get; private set; }
        public TimeSpan NormBuiltInElapsedAsync { get; private set; }
        public double NormBuiltInKbAsync { get; private set; }

        public TimeSpan NormTuplesElapsedSync { get; private set; }
        public double NormTuplesKbSync { get; private set; }
        public TimeSpan NormTuplesElapsedAsync { get; private set; }
        public double NormTuplesKbAsync { get; private set; }

        public TimeSpan RawElapsedSync { get; private set; }
        public double RawKbSync { get; private set; }
        public TimeSpan RawElapsedAsync { get; private set; }
        public double RawKbAsync { get; private set; }

        public void Run(int runs = Config.Runs, int records = Config.Records)
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

            NormBuiltInElapsedSync = normValuesElapsedAvg;
            NormBuiltInKbSync = Kb(normValuesBytesAvg);
            NormTuplesElapsedSync = normTuplesAvg;
            NormTuplesKbSync = Kb(normTuplesBytesAvg);
            RawElapsedSync = rawReaderAvg;
            RawKbSync = Kb(rawReaderBytesAvg);

            Console.WriteLine($"|**AVG**|**{NormBuiltInElapsedSync}**|**{NormBuiltInKbSync}**|**{NormTuplesElapsedSync}**|**{NormTuplesKbSync}**|**{RawElapsedSync}**|**{RawKbSync}**|");
            Console.WriteLine();
            Console.WriteLine("Finished.");
            Console.WriteLine();
        }

        public async ValueTask RunAsync(int runs = Config.Runs, int records = Config.Records)
        {
            Console.WriteLine("## Norm Read Values and Tuples vs Raw Data Reader Async Tests");
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
                var normValues = await connection.ReadAsync<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query).ToListAsync();
                sw.Stop();
                long normValuesBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
                var normValuesElapsed = sw.Elapsed;
                values[0] = normValuesElapsed.Ticks;
                values[1] = normValuesBytes;
                normValues = null;
                GC.Collect(int.MaxValue, GCCollectionMode.Forced);

                sw.Reset();
                sw.Start();
                var normTuples = await connection.ReadAsync<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query).ToListAsync();
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
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
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

            NormBuiltInElapsedAsync = normValuesElapsedAvg;
            NormBuiltInKbAsync = Kb(normValuesBytesAvg);
            NormTuplesElapsedAsync = normTuplesAvg;
            NormTuplesKbAsync = Kb(normTuplesBytesAvg);
            RawElapsedAsync = rawReaderAvg;
            RawKbAsync = Kb(rawReaderBytesAvg);

            Console.WriteLine($"|**AVG**|**{NormBuiltInElapsedAsync}**|**{NormBuiltInKbAsync}**|**{NormTuplesElapsedAsync}**|**{NormTuplesKbAsync}**|**{RawElapsedAsync}**|**{RawKbAsync}**|");
            Console.WriteLine();
            Console.WriteLine("Finished.");
            Console.WriteLine();
        }
    }
}
