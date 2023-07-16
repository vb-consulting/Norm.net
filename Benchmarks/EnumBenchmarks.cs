using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Npgsql;
using PostgreSqlUnitTests;

using Dapper;
using Norm;

namespace Benchmarks6
{

    public enum TestEnum { EnumValue1, EnumValue2, EnumValue3, EnumValue4 }

    public class TestEnumClass 
    {
        public int Id { get; set; }
        public TestEnum Value1 { get; set; }
        public TestEnum Value2 { get; set; }
    }

    [KeepBenchmarkFiles]
    [MarkdownExporter]
    [MarkdownExporterAttribute.GitHub]
    public class EnumBenchmarks
    {
        private static string GetQuery(int records)
        {
            return $@"
            select 
                (id * 10) + Value2 as id,
                value1, 
                value2
            from
                generate_series(1, {records}) id
                cross join (
                    select * from (values
                        ('EnumValue1', 1),
                        ('EnumValue2', 2),
                        ('EnumValue3', 3),
                        ('EnumValue4', 4)
                    ) t(value1, value2)
                ) e
            ";
        }

        private string query = default!;
        private NpgsqlConnection connection = default!;
        private PostgreSqlFixture fixture = default!;

        [Params(10, 100, 1000, 10_000, 100_000)]
        public int Records { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            query = GetQuery(Records);
            fixture = new PostgreSqlFixture();
            connection = new NpgsqlConnection(Connection.ConnectionString ?? fixture.ConnectionString);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            fixture.Dispose();
            connection.Dispose();
        }

        [Benchmark(Baseline = true)]
        public void Dapper()
        {
            foreach (var i in connection.Query<TestEnumClass>(query))
            {
            }
        }

        [Benchmark()]
        public void Norm_PocoClass_Instances()
        {
            foreach (var i in connection.Read<TestEnumClass>(query))
            {
            }
        }

        [Benchmark()]
        public void Norm_Tuples()
        {
            foreach (var i in connection.Read<int, TestEnum, TestEnum>(query))
            {
            }
        }

        [Benchmark()]
        public void Command_Reader()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            using var command = new NpgsqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var instance = new TestEnumClass
                {
                    Id = reader.GetInt32(0),
                    Value1 = Enum.Parse<TestEnum>(reader.GetString(1)),
                    Value2 = (TestEnum)Enum.ToObject(typeof(TestEnum), reader.GetInt32(2)),
                };
            }
        }
    }
}
