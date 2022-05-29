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

        class TestClass1
        {
            public int i { get; set; }
            public int j { get; set; }
        }

        class TestClass2
        {
            public string i { get; set; }
            public int j { get; set; }
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

        class TestClass33
        {
            public int F1 { get; set; }
            public int F2 { get; set; }
            public int F3 { get; set; }
        }

        public CallbackUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Map_Callback_Values_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .Read<int, int>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].Item1);
            Assert.Equal(1, result[0].Item2);

            Assert.Equal(3, result[1].Item1);
            Assert.Equal(2, result[1].Item2);

            Assert.Equal(4, result[2].Item1);
            Assert.Equal(3, result[2].Item2);
        }

        [Fact]
        public void Map_Callback_By_Name_Values_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Name switch
                {
                    "i" => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .Read<int, int>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArray();

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
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => Convert.ToString(r.Reader.GetInt32(0) + 1),
                    _ => null
                })
                .Read<string, int>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArray();

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
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .Read<(int i, int j)>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArray();

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
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => Convert.ToString(r.Reader.GetInt32(0) + 1),
                    _ => null
                })
                .Read<(string i, int j)>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public void Map_Callback_Class_Instance_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .Read<TestClass1>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal(3, result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal(4, result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public void Map_Callback_Class_Instance__Change_Type_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => Convert.ToString(r.Reader.GetInt32(0) + 1),
                    _ => null
                })
                .Read<TestClass2>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public void Map_Callback_Class_Instance_Switch_Exp_Pattern_Matching_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r switch
                {
                    { Ordinal: 0, Name: "i" } => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .Read<TestClass1>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)").ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal(3, result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal(4, result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public void Map_Callback_Class_Instance__Change_To_Complex_Type_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => new TestClass3 { SomeString = Convert.ToString(r.Reader.GetInt32(0) + 1) },
                    _ => null
                })
                .Read<ComplexTestClass>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(test, j)")
                .ToArray();

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
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => new TestClass3 { SomeString = Convert.ToString(r.Reader.GetInt32(0) + 1) },
                    _ => null
                })
                .Read<(TestClass3 Test, int j)>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArray();

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
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => new TestClass3 { SomeString = Convert.ToString(r.Reader.GetInt32(0) + 1) },
                    _ => null
                })
                .Read<TestClass3, int>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].Item1.SomeString);
            Assert.Equal(1, result[0].Item2);

            Assert.Equal("3", result[1].Item1.SomeString);
            Assert.Equal(2, result[1].Item2);

            Assert.Equal("4", result[2].Item1.SomeString);
            Assert.Equal(3, result[2].Item2);
        }

        [Fact]
        public void Map_Callback_Values_1_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .Read<int>(@"
                    select * from (values 
                        (1),
                        (2),
                        (3)
                    ) t(f1)")
                .ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(2, result[0]);
            Assert.Equal(3, result[1]);
            Assert.Equal(4, result[2]);
        }

        [Fact]
        public void Map_Callback_Values_3_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    2 => r.Reader.GetInt32(r.Ordinal) + 1,
                    _ => null
                })
                .Read<int, int, int>(@"
                    select * from (values 
                        (1, 1, 1),
                        (2, 2, 2),
                        (3, 3, 3)
                    ) t(f1, f2, f3)")
                .ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(2, result[0].Item3);
            Assert.Equal(3, result[1].Item3);
            Assert.Equal(4, result[2].Item3);
        }

        [Fact]
        public void Map_Callback_Instance_3_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    2 => r.Reader.GetInt32(r.Ordinal) + 1,
                    _ => null
                })
                .Read<TestClass33>(@"
                    select * from (values 
                        (1, 1, 1),
                        (2, 2, 2),
                        (3, 3, 3)
                    ) t(f1, f2, f3)")
                .ToArray();

            Assert.Equal(3, result.Length);

            Assert.Equal(1, result[0].F1);
            Assert.Equal(2, result[1].F1);
            Assert.Equal(3, result[2].F1);

            Assert.Equal(1, result[0].F2);
            Assert.Equal(2, result[1].F2);
            Assert.Equal(3, result[2].F2);

            Assert.Equal(2, result[0].F3);
            Assert.Equal(3, result[1].F3);
            Assert.Equal(4, result[2].F3);
        }

        [Fact]
        public async Task Map_Callback_Values_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .ReadAsync<int, int>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].Item1);
            Assert.Equal(1, result[0].Item2);

            Assert.Equal(3, result[1].Item1);
            Assert.Equal(2, result[1].Item2);

            Assert.Equal(4, result[2].Item1);
            Assert.Equal(3, result[2].Item2);
        }

        [Fact]
        public async Task Map_Callback_By_Name_Values_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Name switch
                {
                    "i" => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .ReadAsync<int, int>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].Item1);
            Assert.Equal(1, result[0].Item2);

            Assert.Equal(3, result[1].Item1);
            Assert.Equal(2, result[1].Item2);

            Assert.Equal(4, result[2].Item1);
            Assert.Equal(3, result[2].Item2);
        }

        [Fact]
        public async Task Map_Callback_Values__Change_Type_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => Convert.ToString(r.Reader.GetInt32(0) + 1),
                    _ => null
                })
                .ReadAsync<string, int>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].Item1);
            Assert.Equal(1, result[0].Item2);

            Assert.Equal("3", result[1].Item1);
            Assert.Equal(2, result[1].Item2);

            Assert.Equal("4", result[2].Item1);
            Assert.Equal(3, result[2].Item2);
        }

        [Fact]
        public async Task Map_Callback_Named_Tuple_Values_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .ReadAsync<(int i, int j)>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal(3, result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal(4, result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public async Task Map_Callback_Named_Tuple_Values__Change_Type_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => Convert.ToString(r.Reader.GetInt32(0) + 1),
                    _ => null
                })
                .ReadAsync<(string i, int j)>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public async Task Map_Callback_Class_Instance_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .ReadAsync<TestClass1>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal(3, result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal(4, result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public async Task Map_Callback_Class_Instance__Change_Type_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => Convert.ToString(r.Reader.GetInt32(0) + 1),
                    _ => null
                })
                .ReadAsync<TestClass2>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public async Task Map_Callback_Class_Instance_Switch_Exp_Pattern_Matching_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r switch
                {
                    { Ordinal: 0, Name: "i" } => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .ReadAsync<TestClass1>(@"select * from (values 
                    (1, 1),
                    (2, 2),
                    (3, 3)
                ) t(i, j)").ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal(2, result[0].i);
            Assert.Equal(1, result[0].j);

            Assert.Equal(3, result[1].i);
            Assert.Equal(2, result[1].j);

            Assert.Equal(4, result[2].i);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public async Task Map_Callback_Class_Instance__Change_To_Complex_Type_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => new TestClass3 { SomeString = Convert.ToString(r.Reader.GetInt32(0) + 1) },
                    _ => null
                })
                .ReadAsync<ComplexTestClass>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(test, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].Test.SomeString);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].Test.SomeString);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].Test.SomeString);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public async Task Map_Callback_Named_Tuple_Values__Change_To_Complex_Type_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => new TestClass3 { SomeString = Convert.ToString(r.Reader.GetInt32(0) + 1) },
                    _ => null
                })
                .ReadAsync<(TestClass3 Test, int j)>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].Test.SomeString);
            Assert.Equal(1, result[0].j);

            Assert.Equal("3", result[1].Test.SomeString);
            Assert.Equal(2, result[1].j);

            Assert.Equal("4", result[2].Test.SomeString);
            Assert.Equal(3, result[2].j);
        }

        [Fact]
        public async Task Map_Callback_Tuple_Values__Change_To_Complex_Type_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => new TestClass3 { SomeString = Convert.ToString(r.Reader.GetInt32(0) + 1) },
                    _ => null
                })
                .ReadAsync<TestClass3, int>(@"
                    select * from (values 
                        (1, 1),
                        (2, 2),
                        (3, 3)
                    ) t(i, j)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal("2", result[0].Item1.SomeString);
            Assert.Equal(1, result[0].Item2);

            Assert.Equal("3", result[1].Item1.SomeString);
            Assert.Equal(2, result[1].Item2);

            Assert.Equal("4", result[2].Item1.SomeString);
            Assert.Equal(3, result[2].Item2);
        }

        [Fact]
        public async Task Map_Callback_Values_1_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => r.Reader.GetInt32(0) + 1,
                    _ => null
                })
                .ReadAsync<int>(@"
                    select * from (values 
                        (1),
                        (2),
                        (3)
                    ) t(f1)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);
            Assert.Equal(2, result[0]);
            Assert.Equal(3, result[1]);
            Assert.Equal(4, result[2]);
        }

        [Fact]
        public async Task Map_Callback_Values_3_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    2 => r.Reader.GetInt32(r.Ordinal) + 1,
                    _ => null
                })
                .ReadAsync<int, int, int>(@"
                    select * from (values 
                        (1, 1, 1),
                        (2, 2, 2),
                        (3, 3, 3)
                    ) t(f1, f2, f3)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);
            Assert.Equal(2, result[0].Item3);
            Assert.Equal(3, result[1].Item3);
            Assert.Equal(4, result[2].Item3);
        }

        [Fact]
        public async Task Map_Callback_Instance_3_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    2 => r.Reader.GetInt32(r.Ordinal) + 1,
                    _ => null
                })
                .ReadAsync<TestClass33>(@"
                    select * from (values 
                        (1, 1, 1),
                        (2, 2, 2),
                        (3, 3, 3)
                    ) t(f1, f2, f3)")
                .ToArrayAsync();

            Assert.Equal(3, result.Length);

            Assert.Equal(1, result[0].F1);
            Assert.Equal(2, result[1].F1);
            Assert.Equal(3, result[2].F1);

            Assert.Equal(1, result[0].F2);
            Assert.Equal(2, result[1].F2);
            Assert.Equal(3, result[2].F2);

            Assert.Equal(2, result[0].F3);
            Assert.Equal(3, result[1].F3);
            Assert.Equal(4, result[2].F3);
        }
    }
}
