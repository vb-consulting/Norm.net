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
            var (e4, r4) = Measure(() => connection.Read(TestQuery).ToDictionaries().Select(d => new TestClass(d)));
            
            var (e5, r5) = Measure(() => connection.Read<int, string, string, DateTime>(TestQuery).Select(tuple => new TestClass(tuple)));

            var (e6, r6) = Measure(() => connection.Json<TestClass>(JsonTestQuery));


            var (e1Count, c1) = Measure(() => r1.ToList().Count);
            var (e2Count, c2) = Measure(() => r2.ToList().Count);
            var (e3Count, c3) = Measure(() => r3.ToList().Count);
            var (e4Count, c4) = Measure(() => r4.ToList().Count);

            var (e5Count, c5) = Measure(() => r5.ToList().Count);
            var (e6Count, c6) = Measure(() => r6.ToList().Count);

            output.WriteLine("Dapper objects query in {0}", e1);
            output.WriteLine("NoOrm tuples query in {0}", e2);
            output.WriteLine("NoOrm dictionary query in {0}", e3);
            output.WriteLine("NoOrm objects from select dictionaries query in {0}", e4);
            output.WriteLine("NoOrm objects from select tuples query in {0}", e5);
            output.WriteLine("NoOrm json query in {0}", e6);

            output.WriteLine("Dapper objects count {0} in {1}", c1, e1Count);
            output.WriteLine("NoOrm tuples count {0} in {1}", c2, e2Count);
            output.WriteLine("NoOrm dictionary count {0} in {1}", c3, e3Count);
            output.WriteLine("NoOrm objects from select dictionaries count {0} in {1}", c4, e4Count);
            output.WriteLine("NoOrm objects from select tuples count {0} in {1}", c5, e5Count);
            output.WriteLine("NoOrm json query count {0} in {1}", c6, e6Count);


            /*
            Dapper objects query in 00:00:02.0330651
            NoOrm tuples query in 00:00:00.0012584
            NoOrm dictionary query in 00:00:00.0003814
            NoOrm objects from select dictionaries query in 00:00:00.0003062
            NoOrm objects from select tuples query in 00:00:00.0015642
            NoOrm json query in 00:00:00.0003986

            Dapper objects count 1000000 in 00:00:00.0010743
            NoOrm tuples count 1000000 in 00:00:01.5443295
            NoOrm dictionary count 1000000 in 00:00:03.0255267
            NoOrm objects from select dictionaries count 1000000 in 00:00:02.1764616
            NoOrm objects from select tuples count 1000000 in 00:00:01.7178515
            NoOrm json query count 1000000 in 00:00:03.0253324


            Dapper objects query in 00:00:02.2862309
            NoOrm tuples query in 00:00:00.0012932
            NoOrm dictionary query in 00:00:00.0003971
            NoOrm objects from select dictionaries query in 00:00:00.0003061
            NoOrm objects from select tuples query in 00:00:00.0015795
            NoOrm json query in 00:00:00.0004037

            Dapper objects count 1000000 in 00:00:00.0010754
            NoOrm tuples count 1000000 in 00:00:01.5473563
            NoOrm dictionary count 1000000 in 00:00:03.1644884
            NoOrm objects from select dictionaries count 1000000 in 00:00:02.3931478
            NoOrm objects from select tuples count 1000000 in 00:00:01.9789882
            NoOrm json query count 1000000 in 00:00:02.9349006
             */
        }
    }
}
