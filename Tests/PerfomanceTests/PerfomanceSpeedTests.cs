using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Norm.Extensions;
using Npgsql;
using PostgreSqlUnitTests;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

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

    public record TestRecord(int Id, string Foo, string Bar, DateTime Datetime);

    [Collection("PostgreSqlDatabase")]
    public class PerfomanceSpeedTests
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

        private const string JsonTestQuery = @"
                select to_json(t)
                from (
                    select 
                        i as id, 
                        'foo' || i::text as foo, 
                        'bar' || i::text as bar, 
                        ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
                    from generate_series(1, 1000000) as i
                ) t
        ";

        public PerfomanceSpeedTests(ITestOutputHelper output, PostgreSqlFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        private (TimeSpan, T) Measure<T>(Func<T> func)
        {
            GC.Collect();
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
            var (e3, r3) = Measure(() => connection.Read(TestQuery).Select<TestClass>());
            var (e4, r4) = Measure(() => connection.Read(TestQuery).Select<TestRecord>());


            var (e1Count, c1) = Measure(() => r1.ToList().Count);
            var (e2Count, c2) = Measure(() => r2.ToList().Count);
            var (e3Count, c3) = Measure(() => r3.ToList().Count);
            var (e4Count, c4) = Measure(() => r4.ToList().Count);

            output.WriteLine($"{e1}|{e2Count}|{e3Count}|{e4Count}");

            /*
                00:00:02.7631820|00:00:01.9683174|00:00:02.6515440|00:00:02.4122985
                00:00:02.6781349|00:00:01.9326094|00:00:02.9011784|00:00:02.4464330
                00:00:02.7270078|00:00:01.9002550|00:00:02.6752303|00:00:02.4246634
                00:00:02.7237423|00:00:01.9675548|00:00:02.7367278|00:00:02.5056493
                00:00:02.7481510|00:00:01.9454488|00:00:02.6408709|00:00:02.5578935
             */

        }
    }
}
