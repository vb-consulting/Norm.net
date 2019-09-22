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
{
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

                // Dapper count 1000000 in 00:00:02.7984192
                output.WriteLine("Dapper count {0} in {1}", dapper.ToList().Count, sw.Elapsed);

                sw.Reset();
                sw.Start();
                var noorm = connection.Read(TestQuery);
                sw.Stop();

                // NoOrm dictionary dict count 1000000 in 00:00:00.0007977
                output.WriteLine("NoOrm dictionary dict count {0} in {1}", noorm.ToList().Count, sw.Elapsed);
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


                // NoOrm dict count 1000000 in 00:00:03.5419549
                var noorm = new List<IDictionary<string, object>>();
                sw.Reset();
                sw.Start();
                await connection.ReadAsync(TestQuery, r => noorm.Add(r));
                sw.Stop();

                output.WriteLine("NoOrm dict count {0} in {1}", noorm.ToList().Count, sw.Elapsed);


                // NoOrm objects count 1000000 in 00:00:02.8857702
                var noormSerialized = new List<TestClass>();
                sw.Reset();
                sw.Start();
                await connection.ReadAsync(TestQuery, r =>
                    noormSerialized.Add(new TestClass
                    {
                        Id = (int)r["id"],
                        Foo = (string)r["foo"],
                        Bar = (string)r["bar"],
                        Datetime = (DateTime)r["datetime"]
                    }));
                sw.Stop();

                output.WriteLine("NoOrm objects count {0} in {1}", noormSerialized.ToList().Count, sw.Elapsed);
            }
        }
    }
}
