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

        private async Task<(TimeSpan, T)> MeasureAsync<T>(Func<Task<T>> func)
        {
            sw.Reset();
            sw.Start();
            var t = await func();
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

            output.WriteLine("Dapper objects count {0} in {1}", e1Count, c1);
            output.WriteLine("NoOrm tuples count {0} in {1}", e2Count, c2);
            output.WriteLine("NoOrm dictionary count {0} in {1}", e3Count, c3);
            output.WriteLine("NoOrm objects count {0} in {1}", e4Count, c4);
            /*
                Dapper objects query in 00:00:02.1777959
                NoOrm tuples query in 00:00:00.0007307
                NoOrm dictionary query in 00:00:00.0004022
                NoOrm objects query in 00:00:00.0003019
                
                Dapper objects count 00:00:00.0010818 in 1000000
                NoOrm tuples count 00:00:01.5827927 in 1000000
                NoOrm dictionary count 00:00:03.1796682 in 1000000
                NoOrm objects count 00:00:02.3438606 in 1000000
             */
        }

        /*
        [Fact]
        public async Task Test_Serialization_Speed_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (e1, r1) = await MeasureAsync(async () => await connection.QueryAsync<TestClass>(TestQuery));
            var (e2, r2) = await MeasureAsync(() => connection.ReadAsync(TestQuery));
            var (e3, r3) = await MeasureAsync(() => connection.ReadAsync(TestQuery).ToDictionariesAsync());
            var (e4, r4) = await MeasureAsync(() => connection.ReadAsync(TestQuery).ToDictionaries().Select(d => new TestClass
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

            output.WriteLine("Dapper objects count {0} in {1}", e1Count, c1);
            output.WriteLine("NoOrm tuples count {0} in {1}", e2Count, c2);
            output.WriteLine("NoOrm dictionary count {0} in {1}", e3Count, c3);
            output.WriteLine("NoOrm objects count {0} in {1}", e4Count, c4);
        }
        */
    }
}
