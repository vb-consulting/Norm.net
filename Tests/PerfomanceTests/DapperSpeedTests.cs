using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NoOrm;
using NoOrm.Extensions;
using Npgsql;
using PostgreSqlUnitTests;
using Xunit;
using Xunit.Abstractions;

namespace PerfomanceTests
{
    [CollectionDefinition("PostgreSqlDatabase")]
    public class DatabaseFixtureCollection : ICollectionFixture<PostgreSqlFixture> { }

    class TestClass
    {
        public int Id { get; set; }
        public string Foo { get; set; }
        public string Bar { get; set; }
        public DateTime Datetime { get; set; }
    }

    [Collection("PostgreSqlDatabase")]
    public class DapperSpeedTests
    {
        private readonly ITestOutputHelper output;
        private readonly PostgreSqlFixture fixture;
        private readonly Stopwatch sw = new Stopwatch();

        private const string TestQuery = @"
            select 
                i as id, 
                'foo' || i::text as foo, 
                'bar' || i::text as bar, 
                ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
            from generate_series(1, 1000000) as i
        ";

        public DapperSpeedTests(ITestOutputHelper output, PostgreSqlFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        private (TimeSpan, T) Measure<T>(Func<T> func)
        {
            sw.Reset();
            sw.Start();
            var t = func();
            sw.Stop();
            return (sw.Elapsed, t);
        }

        [Fact]
        public void Test_Serialization_Speed_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (e1, r1) = Measure(() => connection.Query<TestClass>(TestQuery));
            var (e2, r2) = Measure(() => connection.Read(TestQuery));
            var (e3, r3) = Measure(() => connection.Read(TestQuery).ToDictionaries());
            var (e4, r4) = Measure(() => connection.Read(TestQuery).ToDictionaries().Select(d => new TestClass
            {
                Id = (int)d["id"],
                Foo = (string)d["foo"],
                Bar = (string)d["bar"],
                Datetime = (DateTime)d["datetime"]
            }));
            var (e1Count, c1) = Measure(() => r1.ToList().Count);
            var (e2Count, c2) = Measure(() => r2.ToList().Count);
            var (e3Count, c3) = Measure(() => r3.ToList().Count);
            var (e4Count, c4) = Measure(() => r4.ToList().Count);

            output.WriteLine("Dapper objects query in {0}", e1);
            output.WriteLine("NoOrm tuples query in {0}", e2);
            output.WriteLine("NoOrm dictionary query in {0}", e3);
            output.WriteLine("NoOrm objects query in {0}", e4);

            output.WriteLine("Dapper objects count {0} in {1}", c1, e1Count);
            output.WriteLine("NoOrm tuples count {0} in {1}", c2, e2Count);
            output.WriteLine("NoOrm dictionary count {0} in {1}", c3, e3Count);
            output.WriteLine("NoOrm objects count {0} in {1}", c4, e4Count);
            /*
            Dapper objects query in 00:00:02.2823730
            NoOrm tuples query in 00:00:00.0008002
            NoOrm dictionary query in 00:00:00.0004362
            NoOrm objects query in 00:00:00.0003425

            Dapper objects count 1000000 in 00:00:00.0012348
            NoOrm tuples count 1000000 in 00:00:01.6170625
            NoOrm dictionary count 1000000 in 00:00:03.5499609
            NoOrm objects count 1000000 in 00:00:02.6214033

            Dapper objects query in 00:00:02.4485043
            NoOrm tuples query in 00:00:00.0011741
            NoOrm dictionary query in 00:00:00.0003935
            NoOrm objects query in 00:00:00.0003090

            Dapper objects count 1000000 in 00:00:00.0010914
            NoOrm tuples count 1000000 in 00:00:01.8909399
            NoOrm dictionary count 1000000 in 00:00:04.9397652
            NoOrm objects count 1000000 in 00:00:02.8572146
             */
        }

        private async Task<(TimeSpan, T)> MeasureAsync<T>(Func<Task<T>> func)
        {
            sw.Reset();
            sw.Start();
            var t = await func();
            sw.Stop();
            return (sw.Elapsed, t);
        }

        [Fact]
        public async Task Test_Serialization_Speed_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var (e1, r1) = await MeasureAsync(async () => await connection.QueryAsync<TestClass>(TestQuery));
            var (e2, r2) = Measure(() => connection.ReadAsync(TestQuery));
            var (e3, r3) = Measure(() => connection.ReadAsync(TestQuery).ToDictionariesAsync());
            var (e4, r4) = Measure(() => connection.ReadAsync(TestQuery).ToDictionariesAsync().Select(d => new TestClass
            {
                Id = (int)d["id"],
                Foo = (string)d["foo"],
                Bar = (string)d["bar"],
                Datetime = (DateTime)d["datetime"]
            }));

            var (e1Count, c1) = Measure(() => r1.ToList().Count);
            var (e2Count, c2) = await MeasureAsync(async () => (await r2.ToListAsync()).Count);
            var (e3Count, c3) = await MeasureAsync(async () => (await r3.ToListAsync()).Count);
            var (e4Count, c4) = await MeasureAsync(async () => (await r4.ToListAsync()).Count);

            output.WriteLine("Dapper objects query in {0}", e1);
            output.WriteLine("NoOrm tuples query in {0}", e2);
            output.WriteLine("NoOrm dictionary query in {0}", e3);
            output.WriteLine("NoOrm objects query in {0}", e4);

            output.WriteLine("Dapper objects count {0} in {1}", c1, e1Count);
            output.WriteLine("NoOrm tuples count {0} in {1}", c2, e2Count);
            output.WriteLine("NoOrm dictionary count {0} in {1}", c3, e3Count);
            output.WriteLine("NoOrm objects count {0} in {1}", c4, e4Count);

            /*
            Dapper objects query in 00:00:02.3368823
            NoOrm tuples query in 00:00:00.0006862
            NoOrm dictionary query in 00:00:00.0002211
            NoOrm objects query in 00:00:00.0010171

            Dapper objects count 1000000 in 00:00:00.0015824
            NoOrm tuples count 1000000 in 00:00:01.7804074
            NoOrm dictionary count 1000000 in 00:00:05.0739030
            NoOrm objects count 1000000 in 00:00:05.6172080
             */
        }
    }
}
