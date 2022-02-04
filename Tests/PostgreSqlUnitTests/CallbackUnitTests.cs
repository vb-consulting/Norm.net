using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;
using static PostgreSqlUnitTests.MapEnumsUnitTests;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class CallbackUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public CallbackUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Map_Callback_Values_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Read<int, int>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)", r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                }).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].Item1);
            Assert.Equal(1, result[0].Item2);

            Assert.Equal(3, result[1].Item1);
            Assert.Equal(2, result[1].Item2);

            Assert.Equal(4, result[2].Item1);
            Assert.Equal(3, result[2].Item2);
        }

        [Fact]
        public void Map_Callback_Values__Change_Type_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Read<string, int>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)", r => r.Ordinal switch
                {
                    0 => Convert.ToString(r.Reader.GetInt32(0) + 1),
                    _ => null
                }).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].Item1);
            Assert.Equal(1, result[0].Item2);

            Assert.Equal("3", result[1].Item1);
            Assert.Equal(2, result[1].Item2);

            Assert.Equal("4", result[2].Item1);
            Assert.Equal(3, result[2].Item2);
        }

        [Fact]
        public void Map_Callback_Named_Tuple_Values_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Read<(int i, int j)>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)", r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                }).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal(3, result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal(4, result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public void Map_Callback_Named_Tuple_Values__Change_Type_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Read<(string i, int j)>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)", r => r.Ordinal switch
                {
                    0 => Convert.ToString(r.Reader.GetInt32(0) + 1),
                    _ => null
                }).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].i);
            Assert.Equal(3, result[2].j);
        }

        class TestClass1
        {
            public int i { get; set; }
            public int j { get; set; }
        }

        [Fact]
        public void Map_Callback_Class_Instance_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Read<TestClass1>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)", r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                }).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal(3, result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal(4, result[2].i);
            Assert.Equal(3, result[2].j);
        }

        class TestClass2
        {
            public string i { get; set; }
            public int j { get; set; }
        }

        [Fact]
        public void Map_Callback_Class_Instance__Change_Type_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Read<TestClass2>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)", r => r.Ordinal switch
                {
                    0 => Convert.ToString(r.Reader.GetInt32(0) + 1),
                    _ => null
                }).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].i);
            Assert.Equal(3, result[2].j);
        }


        class TestClass3
        {
            public string SomeString { get; set; }
        }

        class ComplexTestClass
        {
            public TestClass3 Test { get; set; }
            public int j { get; set; }
        }

        [Fact]
        public void Map_Callback_Class_Instance__Change_To_Complex_Type_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Read<ComplexTestClass>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(test, j)", r => r.Ordinal switch
                {
                    0 => new TestClass3 { SomeString = Convert.ToString(r.Reader.GetInt32(0) + 1) },
                    _ => null
                }).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].Test.SomeString);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].Test.SomeString);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].Test.SomeString);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public void Map_Callback_Named_Tuple_Values__Change_To_Complex_Type_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Read<(TestClass3 Test, int j)> (@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)", r => r.Ordinal switch
                {
                    0 => new TestClass3 { SomeString = Convert.ToString(r.Reader.GetInt32(0) + 1) },
                    _ => null
                }).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].Test.SomeString);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].Test.SomeString);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].Test.SomeString);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public void Map_Callback_Tuple_Values__Change_To_Complex_Type_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .Read<TestClass3, int>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)", r => r.Ordinal switch
                {
                    0 => new TestClass3 { SomeString = Convert.ToString(r.Reader.GetInt32(0) + 1) },
                    _ => null
                }).ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].Item1.SomeString);
            Assert.Equal(1, result[0].Item2);

            Assert.Equal("3", result[1].Item1.SomeString);
            Assert.Equal(2, result[1].Item2);

            Assert.Equal("4", result[2].Item1.SomeString);
            Assert.Equal(3, result[2].Item2);
        }

    }
}
