using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class MapEnumsUnitTests
    {
        private readonly PostgreSqlFixture fixture;


        public MapEnumsUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }
 
        public enum TestEnum { Value1, Value2, Value3 }

        public class TestEnumClass 
        { 
            public TestEnum Item1 { get; set; }
            public TestEnum? Item2 { get; set; }
        }

        public class TestEnumArrayClass
        {
            public TestEnum[] MyEnums { get; set; }
        }

        public class TestNullableEnumArrayClass
        {
            public TestEnum?[] MyEnums { get; set; }
        }

        [Fact]
        public void Map_Enum_In_Instance_From_Strings_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnumClass>(@"
                            select *
                            from (
                            values 
                                ('Value1', 'Value1'),
                                ('Value2', null),
                                ('Value3', 'Value3')
                            ) t(Item1, Item2)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value1, result[0].Item2);
            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Null(result[1].Item2);
            Assert.Equal(TestEnum.Value3, result[2].Item1);
            Assert.Equal(TestEnum.Value3, result[2].Item2);
        }

        [Fact]
        public void Map_Enum_In_Instance_From_Ints_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnumClass>(@"
                            select *
                            from (
                            values 
                                (0, 0),
                                (1, null),
                                (2, 2)
                            ) t(Item1, Item2)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value1, result[0].Item2);
            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Null(result[1].Item2);
            Assert.Equal(TestEnum.Value3, result[2].Item1);
            Assert.Equal(TestEnum.Value3, result[2].Item2);
        }

        [Fact]
        public void Map_Enum_In_Instance_From_String_Array_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnumArrayClass>(@"
                            select array_agg(e) as MyEnums
                            from (
                            values 
                                ('Value1'),
                                ('Value2'),
                                ('Value3')
                            ) t(e)").ToArray();

            Assert.Single(result);
            Assert.Equal(TestEnum.Value1, result[0].MyEnums[0]);
            Assert.Equal(TestEnum.Value2, result[0].MyEnums[1]);
            Assert.Equal(TestEnum.Value3, result[0].MyEnums[2]);
        }

        [Fact]
        public void Map_Enum_In_Instance_From_Int_Array_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnumArrayClass>(@"
                            select array_agg(e) as MyEnums
                            from (
                            values 
                                (0),
                                (1),
                                (2)
                            ) t(e)").ToArray();

            Assert.Single(result);
            Assert.Equal(TestEnum.Value1, result[0].MyEnums[0]);
            Assert.Equal(TestEnum.Value2, result[0].MyEnums[1]);
            Assert.Equal(TestEnum.Value3, result[0].MyEnums[2]);
        }

        /*
        [Fact]
        public void Map_Nullable_Enum_From_String_Array_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestNullableEnumArrayClass>(@"
                            select array_agg(e) as MyEnums
                            from (
                            values 
                                ('Value1'),
                                (null),
                                ('Value3')
                            ) t(e)").ToArray();

            Assert.Single(result);
            Assert.Equal(TestEnum.Value1, result[0].MyEnums[0]);
            Assert.Null(result[0].MyEnums[1]);
            Assert.Equal(TestEnum.Value3, result[0].MyEnums[2]);
        }
        */

        [Fact]
        public void Map_Enum_Value_From_String_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnum>(@"
                            select *
                            from (
                            values 
                                ('Value1'),
                                ('Value2'),
                                ('Value3')
                            ) t(Item1)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0]);
            Assert.Equal(TestEnum.Value2, result[1]);
            Assert.Equal(TestEnum.Value3, result[2]);
        }

        [Fact]
        public void Map_Enum_Nullable_Value_From_String_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnum?>(@"
                            select *
                            from (
                            values 
                                ('Value1'),
                                (null),
                                ('Value3')
                            ) t(Item1)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0]);
            Assert.Null(result[1]);
            Assert.Equal(TestEnum.Value3, result[2]);
        }
        
        [Fact]
        public void Map_Enum_Value_From_Int_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnum>(@"
                            select *
                            from (
                            values 
                                (0),
                                (1),
                                (2)
                            ) t(Item1)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0]);
            Assert.Equal(TestEnum.Value2, result[1]);
            Assert.Equal(TestEnum.Value3, result[2]);
        }

        [Fact]
        public void Map_Enum_Nullable_Value_From_Int_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnum?>(@"
                            select *
                            from (
                            values 
                                (0),
                                (null),
                                (2)
                            ) t(Item1)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0]);
            Assert.Null(result[1]);
            Assert.Equal(TestEnum.Value3, result[2]);
        }

        /*
        [Fact]
        public void Map_Enum_Array_Value_From_String_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnum[]>(@"
                            select array_agg(e) as MyEnums
                            from (
                            values 
                                ('Value1'),
                                ('Value2'),
                                ('Value3')
                            ) t(e)").ToArray();

            Assert.Single(result);
            Assert.Equal(TestEnum.Value1, result[0][0]);
            Assert.Equal(TestEnum.Value2, result[0][1]);
            Assert.Equal(TestEnum.Value3, result[0][2]);
        }

        [Fact]
        public void Map_Enum_Nullable_Array_Value_From_String_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnum?[]>(@"
                            select array_agg(e) as MyEnums
                            from (
                            values 
                                ('Value1'),
                                (null),
                                ('Value3')
                            ) t(e)").ToArray();

            Assert.Single(result);
            Assert.Equal(TestEnum.Value1, result[0][0]);
            Assert.Null(result[0][1]);
            Assert.Equal(TestEnum.Value3, result[0][2]);
        }

        [Fact]
        public void Map_Enum_Array_Value_From_Int_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnum[]>(@"
                            select array_agg(e) as MyEnums
                            from (
                            values 
                                (0),
                                (1),
                                (2)
                            ) t(e)").ToArray();

            Assert.Single(result);
            Assert.Equal(TestEnum.Value1, result[0][0]);
            Assert.Equal(TestEnum.Value2, result[0][1]);
            Assert.Equal(TestEnum.Value3, result[0][2]);
        }

        [Fact]
        public void Map_Enum_Nullable_Array_Value_From_Int_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<TestEnum?[]>(@"
                            select array_agg(e) as MyEnums
                            from (
                            values 
                                (0),
                                (null),
                                (2)
                            ) t(e)").ToArray();

            Assert.Single(result);
            Assert.Equal(TestEnum.Value1, result[0][0]);
            Assert.Null(result[0][1]);
            Assert.Equal(TestEnum.Value3, result[0][2]);
        }
        */

        /*
        [Fact]
        public void Map_Enum_Named_Value_From_String_Test_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.Read<(TestEnum Enum1, TestEnum Enum2)>(@"
                            select *
                            from (
                            values 
                                ('Value1', 'Value3'),
                                ('Value2', 'Value2'),
                                ('Value3', 'Value1')
                            ) t(Enum1, Enum2)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Enum1);
            Assert.Equal(TestEnum.Value2, result[1].Enum1);
            Assert.Equal(TestEnum.Value3, result[2].Enum1);

            Assert.Equal(TestEnum.Value3, result[0].Enum2);
            Assert.Equal(TestEnum.Value2, result[1].Enum2);
            Assert.Equal(TestEnum.Value1, result[2].Enum2);
        }
        */
    }
}
