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

        public TestClass()
        {
        }

        public TestClass(IDictionary<string, object> dictionary)
        {
            Id = (int) dictionary["Id"];
            Foo = (string) dictionary["Foo"];
            Bar = (string) dictionary["Bar"];
            Datetime = (DateTime) dictionary["Datetime"];
        }
            
        public TestClass((int id, string foo, string bar, DateTime dateTime) tuple)
        {
            Id = tuple.id;
            Foo = tuple.foo;
            Bar = tuple.bar;
            Datetime = tuple.dateTime;
        }
    }

    [Collection("PostgreSqlDatabase")]
    public class PerfomanceSpeedTests
    {
        private readonly ITestOutputHelper output;
        private readonly PostgreSqlFixture fixture;
        private readonly Stopwatch sw = new Stopwatch();

        private const string TestQuery = @"
            select 
                i as ""Id"", 
                'foo' || i::text as ""Foo"", 
                'bar' || i::text as ""Bar"", 
                ('2000-01-01'::date) + (i::text || ' days')::interval as ""Datetime""
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
            var (e3, r3) = Measure(() => connection.Read(TestQuery).SelectDictionaries());
            var (e4, r4) = Measure(() => connection.Read(TestQuery).SelectDictionaries().Select(d => new TestClass(d)));
            var (e5, r5) = Measure(() => connection.Read<int, string, string, DateTime>(TestQuery).Select(tuple => new TestClass(tuple)));
            var (e6, r6) = Measure(() => connection.Json<TestClass>(JsonTestQuery));
            var (e7, r7) = Measure(() => connection.Read(TestQuery).Select<TestClass>());


            var (e1Count, c1) = Measure(() => r1.ToList().Count);
            var (e2Count, c2) = Measure(() => r2.ToList().Count);
            var (e3Count, c3) = Measure(() => r3.ToList().Count);
            var (e4Count, c4) = Measure(() => r4.ToList().Count);
            var (e5Count, c5) = Measure(() => r5.ToList().Count);
            var (e6Count, c6) = Measure(() => r6.ToList().Count);
            var (e7Count, c7) = Measure(() => r7.ToList().Count);


            output.WriteLine($"{e1},{e2},{e3},{e4},{e5},{e6},{e7},{e1Count},{e2Count},{e3Count},{e4Count},{e5Count},{e6Count},{e7Count}");
            /**

00:00:03.3752950,00:00:00.0025598,00:00:00.0006772,00:00:00.0006989,00:00:00.0022935,00:00:00.0007031,00:00:00.0008550,00:00:00.0018595,00:00:02.8637453,00:00:03.7792868,00:00:03.6100517,00:00:04.5741329,00:00:05.1103832,00:00:03.2302929
00:00:03.0415078,00:00:00.0024166,00:00:00.0007101,00:00:00.0005985,00:00:00.0031619,00:00:00.0007754,00:00:00.0012049,00:00:00.0026828,00:00:02.8800147,00:00:03.9677773,00:00:04.0447490,00:00:03.5522813,00:00:04.2972600,00:00:02.9316158
00:00:03.2452560,00:00:00.0032193,00:00:00.0006805,00:00:00.0005830,00:00:00.0021970,00:00:00.0008820,00:00:00.0010584,00:00:00.0017753,00:00:02.5111109,00:00:03.5520368,00:00:03.0283845,00:00:02.5383521,00:00:04.4486203,00:00:03.4632417
00:00:02.9803222,00:00:00.0026597,00:00:00.0006636,00:00:00.0005223,00:00:00.0021911,00:00:00.0007316,00:00:00.0008133,00:00:00.0019028,00:00:02.4975842,00:00:03.6029344,00:00:02.9978021,00:00:02.6323984,00:00:04.1697686,00:00:03.1088790
00:00:04.2485572,00:00:00.0026039,00:00:00.0007918,00:00:00.0007257,00:00:00.0035843,00:00:00.0008177,00:00:00.0008636,00:00:00.0014821,00:00:02.7933007,00:00:03.7870573,00:00:03.8797356,00:00:03.1265490,00:00:05.0243688,00:00:03.8503477
00:00:03.4473896,00:00:00.0024689,00:00:00.0009009,00:00:00.0005666,00:00:00.0034545,00:00:00.0007081,00:00:00.0008028,00:00:00.0018032,00:00:02.6095270,00:00:04.2870604,00:00:03.2902466,00:00:02.4776056,00:00:04.9150692,00:00:03.2962260
00:00:03.9070679,00:00:00.0025340,00:00:00.0007605,00:00:00.0008123,00:00:00.0023278,00:00:00.0007847,00:00:00.0009933,00:00:00.0018624,00:00:03.1660909,00:00:03.7884559,00:00:03.3306854,00:00:03.1666028,00:00:05.5989298,00:00:03.2163334
00:00:03.3050129,00:00:00.0025335,00:00:00.0008778,00:00:00.0005671,00:00:00.0034135,00:00:00.0008028,00:00:00.0008808,00:00:00.0019943,00:00:03.0209209,00:00:05.7025867,00:00:04.9959240,00:00:02.4584816,00:00:04.4241120,00:00:02.6890962
00:00:03.0581420,00:00:00.0022742,00:00:00.0006804,00:00:00.0005627,00:00:00.0022258,00:00:00.0010480,00:00:00.0008435,00:00:00.0017910,00:00:02.3538676,00:00:04.1760797,00:00:03.1734550,00:00:02.4060259,00:00:04.0451822,00:00:03.1164555
00:00:03.0750567,00:00:00.0028665,00:00:00.0009022,00:00:00.0008439,00:00:00.0044506,00:00:00.0008151,00:00:00.0009868,00:00:00.0032063,00:00:03.3263054,00:00:04.8390286,00:00:03.2985729,00:00:02.9437614,00:00:04.7721182,00:00:03.1037891

output.WriteLine("Dapper objects query in {0}", e1);
output.WriteLine("NoOrm tuples query in {0}", e2);
output.WriteLine("NoOrm dictionary query in {0}", e3);
output.WriteLine("NoOrm objects from select dictionaries query in {0}", e4);
output.WriteLine("NoOrm objects from select tuples query in {0}", e5);
output.WriteLine("NoOrm json query in {0}", e6);
output.WriteLine("NoOrm object map query in {0}", e7);
output.WriteLine("");
output.WriteLine("");
output.WriteLine("Dapper objects count {0} in {1}", c1, e1Count);
output.WriteLine("NoOrm tuples count {0} in {1}", c2, e2Count);
output.WriteLine("NoOrm dictionary count {0} in {1}", c3, e3Count);
output.WriteLine("NoOrm objects from select dictionaries count {0} in {1}", c4, e4Count);
output.WriteLine("NoOrm objects from select tuples count {0} in {1}", c5, e5Count);
output.WriteLine("NoOrm json query count {0} in {1}", c6, e6Count);
output.WriteLine("NoOrm object map query count {0} in {1}", c7, e7Count);

/**
Dapper objects query in 00:00:03.6980529
NoOrm tuples query in 00:00:00.0024362
NoOrm dictionary query in 00:00:00.0018505
NoOrm objects from select dictionaries query in 00:00:00.0008361
NoOrm objects from select tuples query in 00:00:00.0025307
NoOrm json query in 00:00:00.0008752
NoOrm object map query in 00:00:00.0009277


Dapper objects count 1000000 in 00:00:00.0016769
NoOrm tuples count 1000000 in 00:00:02.8809081
NoOrm dictionary count 1000000 in 00:00:04.9122459
NoOrm objects from select dictionaries count 1000000 in 00:00:03.5541537
NoOrm objects from select tuples count 1000000 in 00:00:02.5150648
NoOrm json query count 1000000 in 00:00:05.3408321
NoOrm object map query count 1000000 in 00:00:03.2142914


Dapper objects query in 00:00:03.0591550
NoOrm tuples query in 00:00:00.0032917
NoOrm dictionary query in 00:00:00.0007843
NoOrm objects from select dictionaries query in 00:00:00.0006268
NoOrm objects from select tuples query in 00:00:00.0030985
NoOrm json query in 00:00:00.0006851
NoOrm object map query in 00:00:00.0007803


Dapper objects count 1000000 in 00:00:00.0019062
NoOrm tuples count 1000000 in 00:00:02.3969892
NoOrm dictionary count 1000000 in 00:00:03.5179588
NoOrm objects from select dictionaries count 1000000 in 00:00:02.9841878
NoOrm objects from select tuples count 1000000 in 00:00:02.5612648
NoOrm json query count 1000000 in 00:00:05.3084148
NoOrm object map query count 1000000 in 00:00:03.1208641

 */
        }
    }
}
