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
            var (e3, r3) = Measure(() => connection.Read<int, string, string, DateTime>(TestQuery));
            //var (e3, r3) = Measure(() => connection.Read(TestQuery).SelectDictionaries());
            var (e4, r4) = Measure(() => connection.Read(TestQuery).SelectDictionaries().Select(d => new TestClass(d)));
            var (e5, r5) = Measure(() => connection.Read<int, string, string, DateTime>(TestQuery).Select(tuple => new TestClass(tuple)));
            //var (e6, r6) = Measure(() => connection.Json<TestClass>(JsonTestQuery));
            var (e7, r7) = Measure(() => connection.Read(TestQuery).Select<TestClass>());


            var (e1Count, c1) = Measure(() => r1.ToList().Count);
            var (e2Count, c2) = Measure(() => r2.ToList().Count);
            var (e3Count, c3) = Measure(() => r3.ToList().Count);
            var (e4Count, c4) = Measure(() => r4.ToList().Count);
            var (e5Count, c5) = Measure(() => r5.ToList().Count);
            //var (e6Count, c6) = Measure(() => r6.ToList().Count);
            var (e7Count, c7) = Measure(() => r7.ToList().Count);

            //output.WriteLine($"{e1},    {e2},   {e3},   {e7},   {e1Count},  {e2Count},  {e3Count},  {e7Count}");
            //output.WriteLine($"{e1},{e2},{e3},{e4},{e5},{e6},{e7},{e1Count},{e2Count},{e3Count},{e4Count},{e5Count},{e6Count},{e7Count}");
            output.WriteLine($"{e1},{e2},{e3},{e4},{e5},{e7},{e1Count},{e2Count},{e3Count},{e4Count},{e5Count},{e7Count}");
            /*
             00:00:02.9073922,00:00:00.0018315,00:00:00.0009539,00:00:00.0008372,00:00:00.0014880,00:00:00.0006290,00:00:00.0009730,00:00:00.0016179,00:00:02.5370354,00:00:02.3914171,00:00:02.9139682,00:00:02.3039374,00:00:04.2189928,00:00:03.0418838
             00:00:02.7778788,00:00:00.0016037,00:00:00.0009633,00:00:00.0007911,00:00:00.0019141,00:00:00.0006245,00:00:00.0006308,00:00:00.0013831,00:00:02.3186422,00:00:02.0883494,00:00:02.9988069,00:00:01.9987436,00:00:03.6312376,00:00:02.9569528
             00:00:02.9922965,00:00:00.0020729,00:00:00.0010498,00:00:00.0008464,00:00:00.0013850,00:00:00.0006577,00:00:00.0006808,00:00:00.0013855,00:00:02.2317930,00:00:01.8623077,00:00:02.4878521,00:00:02.1570018,00:00:03.9488608,00:00:02.7193154
             00:00:02.7645056,00:00:00.0016334,00:00:00.0010005,00:00:00.0009045,00:00:00.0019346,00:00:00.0006213,00:00:00.0007797,00:00:00.0015391,00:00:02.4389429,00:00:03.1113979,00:00:03.2083198,00:00:02.8778438,00:00:04.7992508,00:00:03.0809233
             00:00:02.8126910,00:00:00.0015091,00:00:00.0009630,00:00:00.0008886,00:00:00.0013356,00:00:00.0007109,00:00:00.0006442,00:00:00.0013658,00:00:02.2157751,00:00:02.0692832,00:00:02.5782982,00:00:02.1793221,00:00:03.9661094,00:00:02.5317759
             00:00:02.5401622,00:00:00.0014043,00:00:00.0010956,00:00:00.0007724,00:00:00.0016877,00:00:00.0006198,00:00:00.0006396,00:00:00.0014491,00:00:02.1128445,00:00:01.9103390,00:00:02.5411998,00:00:02.0069723,00:00:03.5766844,00:00:02.5056395
             00:00:02.8127986,00:00:00.0014433,00:00:00.0009400,00:00:00.0007827,00:00:00.0013140,00:00:00.0006038,00:00:00.0006054,00:00:00.0012384,00:00:02.1178469,00:00:02.2014343,00:00:03.0684528,00:00:03.1069895,00:00:05.9652374,00:00:03.7053895
             00:00:02.9333143,00:00:00.0022856,00:00:00.0009590,00:00:00.0008360,00:00:00.0013865,00:00:00.0006247,00:00:00.0008642,00:00:00.0015726,00:00:02.5956918,00:00:02.3379632,00:00:02.7809542,00:00:02.4827695,00:00:04.6017882,00:00:03.7555821
             00:00:02.7622011,00:00:00.0014918,00:00:00.0010868,00:00:00.0008599,00:00:00.0014201,00:00:00.0006255,00:00:00.0006602,00:00:00.0015362,00:00:02.2492930,00:00:02.1903100,00:00:02.7799773,00:00:02.7001556,00:00:04.2131197,00:00:02.8656352
             00:00:03.2822476,00:00:00.0017639,00:00:00.0010472,00:00:00.0008869,00:00:00.0021456,00:00:00.0007310,00:00:00.0006559,00:00:00.0015051,00:00:02.3720875,00:00:02.2289755,00:00:02.6542241,00:00:02.1897287,00:00:03.9113875,00:00:03.0563991
             */
        }
    }
}
