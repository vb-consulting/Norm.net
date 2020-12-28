using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperQuery = Dapper.SqlMapper;
using Norm;
using Npgsql;
using System.Diagnostics;

namespace BenchmarksConsole
{
    public class IterationBenchmarks : TestBase
    {
        public IterationBenchmarks(NpgsqlConnection connection) : base(connection) { }

        public void Run(int runs = 10, int records = 1000000)
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

        public async ValueTask RunAsync(int runs = 10, int records = 1000000)
        {
            var sw = new Stopwatch();
            var query = GetQuery(records);

            Console.WriteLine("## Dapper Buffered Query and Iteration vs Norm Read and Iteration Async Tests");
            Console.WriteLine();

            Console.WriteLine("|#|Dapper Buffered Query|Dapper Buffered Iteration|Daper Buffered Total|Dapper Buffered Allocated KB|Norm Read|Norm Iteration|Norm Total|Norm Allocated KB|");
            Console.WriteLine("|-|---------------------|-------------------------|--------------------|----------------------------|---------|--------------|----------|-----------------|");

            var list = new List<long[]>();

            for (int i = 0; i < runs; i++)
            {
                var values = new long[8];

                sw.Reset();
                sw.Start();
                var dapperQuery = await DapperQuery.QueryAsync<PocoClass>(connection, query);
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
                long dapperQueryBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
                var dapperIterationElapsed = sw.Elapsed;
                values[1] = sw.Elapsed.Ticks;

                var dapperTotal = dapperQueryElapsed + dapperIterationElapsed;
                values[2] = dapperTotal.Ticks;
                values[3] = dapperQueryBytes;
                dapperQuery = null;
                GC.Collect(int.MaxValue, GCCollectionMode.Forced);


                sw.Reset();
                sw.Start();
                var normRead = connection.ReadAsync<PocoClass>(query);
                sw.Stop();

                var normReadElapsed = sw.Elapsed;
                values[4] = sw.Elapsed.Ticks;

                sw.Reset();
                sw.Start();
                await foreach (var row in normRead)
                {
                    // do something
                }
                sw.Stop();
                long normReadBytes = GC.GetTotalMemory(false); //GC.GetAllocatedBytesForCurrentThread();
                var normReadIteration = sw.Elapsed;
                var normTotal = normReadElapsed + normReadIteration;
                values[5] = sw.Elapsed.Ticks;
                values[6] = normTotal.Ticks;
                values[7] = normReadBytes;
                normRead = null;
                GC.Collect(int.MaxValue, GCCollectionMode.Forced);


                list.Add(values);
                Console.WriteLine($"|{i + 1}|{dapperQueryElapsed}|{dapperIterationElapsed}|{dapperTotal}|{Kb(dapperQueryBytes)}|{normReadElapsed}|{normReadIteration}|{normTotal}|{Kb(normReadBytes)}|");
            }

            var dapperQueryAvg = new TimeSpan((long)list.Select(v => v[0]).Average());
            var dapperIterationAvg = new TimeSpan((long)list.Select(v => v[1]).Average());
            var dapperTotalAvg = new TimeSpan((long)list.Select(v => v[2]).Average());
            var dapperBytesAvg = (long)list.Select(v => v[3]).Average();

            var normReadAvg = new TimeSpan((long)list.Select(v => v[4]).Average());
            var normIterationAvg = new TimeSpan((long)list.Select(v => v[5]).Average());
            var normTotalAvg = new TimeSpan((long)list.Select(v => v[6]).Average());
            var normBytesAvg = (long)list.Select(v => v[7]).Average();

            Console.WriteLine($"|**AVG**|**{dapperQueryAvg}**|**{dapperIterationAvg}**|**{dapperTotalAvg}**|**{Kb(dapperBytesAvg)}**|**{normReadAvg}**|**{normIterationAvg}**|**{normTotalAvg}**|**{Kb(normBytesAvg)}**|");
            Console.WriteLine();
        }
    }
}
