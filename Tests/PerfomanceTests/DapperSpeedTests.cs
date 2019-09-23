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
            using (var connection = new NpgsqlConnection(fixture.ConnectionString))
            {
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
                var (e4Count, c4) = Measure(() => r3.ToList().Count);

                output.WriteLine("Dapper objects query in {0}", e1);
                output.WriteLine("NoOrm tuples query in {0}", e2);
                output.WriteLine("NoOrm dictionary query in {0}", e3);
                output.WriteLine("NoOrm objects query in {0}", e4);

                output.WriteLine("Dapper objects count {0} in {1}", e1Count, c1);
                output.WriteLine("NoOrm tuples count {0} in {1}", e2Count, c2);
                output.WriteLine("NoOrm dictionary count {0} in {1}", e3Count, c3);
                output.WriteLine("NoOrm objects count {0} in {1}", e3Count, c3);
                /*
                Dapper objects query in 00:00:02.9885583
                NoOrm tuples query in 00:00:00.0008709
                NoOrm dictionary query in 00:00:00.0006338
                NoOrm objects query in 00:00:00.0007070
                Dapper objects count 00:00:00.0015846 in 1000000
                NoOrm tuples count 00:00:02.2944091 in 1000000
                NoOrm dictionary count 00:00:04.2392856 in 1000000
                NoOrm objects count 00:00:04.2392856 in 1000000

                Dapper objects query in 00:00:02.6294721
                NoOrm tuples query in 00:00:00.0008265
                NoOrm dictionary query in 00:00:00.0005559
                NoOrm objects query in 00:00:00.0005464
                Dapper objects count 00:00:00.0013546 in 1000000
                NoOrm tuples count 00:00:02.1651769 in 1000000
                NoOrm dictionary count 00:00:03.8501540 in 1000000
                NoOrm objects count 00:00:03.8501540 in 1000000

                Dapper objects query in 00:00:02.8566054
                NoOrm tuples query in 00:00:00.0008514
                NoOrm dictionary query in 00:00:00.0005754
                NoOrm objects query in 00:00:00.0005101
                Dapper objects count 00:00:00.0012731 in 1000000
                NoOrm tuples count 00:00:01.8429251 in 1000000
                NoOrm dictionary count 00:00:03.6058111 in 1000000
                NoOrm objects count 00:00:03.6058111 in 1000000
                 */
            }
        }
    }
}
