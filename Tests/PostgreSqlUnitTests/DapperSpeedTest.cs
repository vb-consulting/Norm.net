using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NoOrm;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace PostgreSqlUnitTests
{     /*
    [Collection("PostgreSqlDatabase")]
    public class DapperSpeedTest
    {
        private readonly ITestOutputHelper output;
        private readonly PostgreSqlFixture fixture;

        private const string TestQuery = @"
            select 
                i as id, 
                'foo' || i::text as foo, 
                'bar' || i::text as bar, 
                ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
            from generate_series(1, 1000000) as i
        ";

        class TestClass
        {
            public int Id { get; set; }
            public string Foo { get; set; }
            public string Bar { get; set; }
            public DateTime Datetime { get; set; }
        }

        public DapperSpeedTest(ITestOutputHelper output, PostgreSqlFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        [Fact]
        public void Test_Serialization_Speed_Sync()
        {
            var sw = new Stopwatch();
            using (var connection = new NpgsqlConnection(fixture.ConnectionString))
            {
                sw.Reset();
                sw.Start();
                var dapper = connection.Query<TestClass>(TestQuery);
                sw.Stop();

                //  Dapper count 1000000 in 00:00:02.0442267
                output.WriteLine("Dapper count {0} in {1}", dapper.ToList().Count, sw.Elapsed);

                sw.Reset();
                sw.Start();
                var noorm1 = connection.Read(TestQuery);
                sw.Stop();

                // NoOrm tuples count 1000000 in 00:00:00.0005807
                output.WriteLine("NoOrm tuples count {0} in {1}", noorm1.ToList().Count, sw.Elapsed);

                sw.Reset();
                sw.Start();
                var noorm2 = connection.Read(TestQuery).ToDictionaries();
                sw.Stop();

                // NoOrm dictionary count 1000000 in 00:00:00.0004038
                output.WriteLine("NoOrm dictionary count {0} in {1}", noorm2.ToList().Count, sw.Elapsed);


                sw.Reset();
                sw.Start();
                var noormSerialized = connection.Read(TestQuery).ToDictionaries().Select(d => new TestClass
                {
                    Id = (int)d["id"],
                    Foo = (string)d["foo"],
                    Bar = (string)d["bar"],
                    Datetime = (DateTime)d["datetime"]
                });
                sw.Stop();

                // NoOrm objects count 1000000 in 00:00:00.0002275
                output.WriteLine("NoOrm objects count {0} in {1}", noormSerialized.ToList().Count, sw.Elapsed);
            }
        }

        [Fact]
        public async Task Test_Serialization_Speed_Async()
        {
            var sw = new Stopwatch();
            using (var connection = new NpgsqlConnection(fixture.ConnectionString))
            {
                // Dapper count 1000000 in 00:00:02.8605490
                sw.Reset();
                sw.Start();
                var dapper = await connection.QueryAsync<TestClass>(TestQuery);
                sw.Stop();

                output.WriteLine("Dapper count {0} in {1}", dapper.ToList().Count, sw.Elapsed);

                // 
                var noormSerialized = new List<TestClass>();
                sw.Reset();
                sw.Start();
                await connection.ReadAsync(TestQuery, row =>
                {
                    noormSerialized.Add(new TestClass
                    {
                        Id = (int) row[0],
                        Foo = (string) row[1],
                        Bar = (string) row[2],
                        Datetime = (DateTime) row[3]
                    });
                });
                sw.Stop();

                output.WriteLine("NoOrm objects count {0} in {1}", noormSerialized.ToList().Count, sw.Elapsed);
            }
        }
    }
    */
}
