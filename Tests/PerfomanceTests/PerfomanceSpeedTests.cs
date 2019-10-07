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
            Id = (int) dictionary["id"];
            Foo = (string) dictionary["foo"];
            Bar = (string) dictionary["bar"];
            Datetime = (DateTime) dictionary["datetime"];
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
            /*
00:00:04.1004485,00:00:00.0055361,00:00:00.0008458,00:00:00.0007060,00:00:00.0036273,00:00:00.0007996,00:00:00.0008978,00:00:00.0020399,00:00:03.6790587,00:00:03.9891292,00:00:03.8134027,00:00:02.6386995,00:00:04.9873844,00:00:04.0077863
00:00:03.1871759,00:00:00.0032804,00:00:00.0008542,00:00:00.0005863,00:00:00.0031553,00:00:00.0006987,00:00:00.0007774,00:00:00.0020880,00:00:02.7713559,00:00:04.2636360,00:00:03.1684147,00:00:02.5706317,00:00:04.1174111,00:00:03.4627616
00:00:03.0506175,00:00:00.0022917,00:00:00.0006699,00:00:00.0005851,00:00:00.0022343,00:00:00.0009680,00:00:00.0007622,00:00:00.0022979,00:00:03.1977368,00:00:04.9397645,00:00:04.1291919,00:00:03.2770808,00:00:05.5130375,00:00:03.3462841
00:00:03.3951452,00:00:00.0024023,00:00:00.0010357,00:00:00.0005774,00:00:00.0021762,00:00:00.0007061,00:00:00.0007477,00:00:00.0015150,00:00:02.4292607,00:00:03.6473209,00:00:03.1280059,00:00:02.7692829,00:00:05.2674816,00:00:04.9929943
00:00:04.0385472,00:00:00.0026012,00:00:00.0006770,00:00:00.0007473,00:00:00.0023183,00:00:00.0009142,00:00:00.0008669,00:00:00.0022856,00:00:02.7607025,00:00:03.9503938,00:00:03.4708754,00:00:02.6413343,00:00:05.0184947,00:00:04.2698972
00:00:03.0974953,00:00:00.0032253,00:00:00.0006641,00:00:00.0008592,00:00:00.0022611,00:00:00.0008639,00:00:00.0008777,00:00:00.0016625,00:00:03.0425242,00:00:03.6981886,00:00:02.9195795,00:00:02.1945205,00:00:04.4696205,00:00:03.9750615
00:00:04.5609368,00:00:00.0033506,00:00:00.0011177,00:00:00.0053134,00:00:00.0042086,00:00:00.0011150,00:00:00.0021742,00:00:00.0026756,00:00:03.8701060,00:00:05.2818798,00:00:03.5859765,00:00:02.8524685,00:00:05.3484611,00:00:04.0662025
00:00:03.6101020,00:00:00.0024450,00:00:00.0007045,00:00:00.0008993,00:00:00.0051645,00:00:00.0010999,00:00:00.0008874,00:00:00.0020277,00:00:02.5486982,00:00:03.6352266,00:00:03.6592063,00:00:02.8599932,00:00:04.4954969,00:00:03.8242962
00:00:03.2167656,00:00:00.0023488,00:00:00.0007870,00:00:00.0005800,00:00:00.0026838,00:00:00.0007562,00:00:00.0007856,00:00:00.0025372,00:00:02.3891269,00:00:03.5073871,00:00:02.9740766,00:00:02.2686103,00:00:04.9168846,00:00:03.7230326
00:00:02.9158137,00:00:00.0025282,00:00:00.0007283,00:00:00.0010551,00:00:00.0028500,00:00:00.0009405,00:00:00.0008487,00:00:00.0017518,00:00:02.6536674,00:00:03.6794895,00:00:02.7775728,00:00:02.9262192,00:00:04.2033549,00:00:03.5633669
            */
        }
    }
}
