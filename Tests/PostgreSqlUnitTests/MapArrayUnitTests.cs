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
    public class MapArrayUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public MapArrayUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        class ArraysTestClass
        {
            public int[] Id { get; set; }
            public string[] Foo { get; set; }
            public DateTime[] Day { get; set; }
            public bool[] Bool { get; set; }
            public string[] Bar { get; set; }
        }


        const string ArraysQuery = @"
            select 
                array_agg(id) as id,
                array_agg(foo) as foo,
                array_agg(day) as day,
                array_agg(bool) as bool,
                array_agg(bar) as bar
            from (
            values 
                (1, 'foo1', '1977-05-19'::date, true, 'bar1'),
                (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                (3, 'foo3', '1979-05-19'::date, true, 'bar3'),
                (4, 'foo4', '1980-05-19'::date, false, 'bar4')
            ) t(id, foo, day, bool, bar)";

        [Fact]
        public void Map_Array_To_Class_Instance_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<ArraysTestClass>(ArraysQuery).ToList();

            Assert.Single(result);

            Assert.Equal(1, result[0].Id[0]);
            Assert.Equal(2, result[0].Id[1]);
            Assert.Equal(3, result[0].Id[2]);
            Assert.Equal(4, result[0].Id[3]);

            Assert.Equal("foo1", result[0].Foo[0]);
            Assert.Equal("foo2", result[0].Foo[1]);
            Assert.Equal("foo3", result[0].Foo[2]);
            Assert.Equal("foo4", result[0].Foo[3]);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Day[0]);
            Assert.Equal(new DateTime(1978, 5, 19), result[0].Day[1]);
            Assert.Equal(new DateTime(1979, 5, 19), result[0].Day[2]);
            Assert.Equal(new DateTime(1980, 5, 19), result[0].Day[3]);

            Assert.True(result[0].Bool[0]);
            Assert.False(result[0].Bool[1]);
            Assert.True(result[0].Bool[2]);
            Assert.False(result[0].Bool[3]);

            Assert.Equal("bar1", result[0].Bar[0]);
            Assert.Equal("bar2", result[0].Bar[1]);
            Assert.Equal("bar3", result[0].Bar[2]);
            Assert.Equal("bar4", result[0].Bar[3]);
        }

        public class NullableIntsArrayClass { public int?[] Ints { get; set; } }

        [Fact]
        public void Map_Nullable_Ints_Array_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Reader.GetFieldValue<int?[]>(0))
                .Read<NullableIntsArrayClass>(@"
                            select array_agg(e) as Ints
                            from (
                            values 
                                (1),
                                (null),
                                (2)
                            ) t(e)")
                .ToArray();

            Assert.Single(result);
            Assert.Equal(1, result[0].Ints[0]);
            Assert.Null(result[0].Ints[1]);
            Assert.Equal(2, result[0].Ints[2]);
        }

        [Fact]
        public void Map_Array_To_Single_Value_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int[]>(ArraysQuery).ToList();

            Assert.Single(result);

            Assert.Equal(1, result[0][0]);
            Assert.Equal(2, result[0][1]);
            Assert.Equal(3, result[0][2]);
            Assert.Equal(4, result[0][3]);
        }

        [Fact]
        public void Map_Array_To_Multiple_Values_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int[], string[], DateTime[], bool[], string[]>(ArraysQuery).ToList();

            Assert.Single(result);

            Assert.Equal(1, result[0].Item1[0]);
            Assert.Equal(2, result[0].Item1[1]);
            Assert.Equal(3, result[0].Item1[2]);
            Assert.Equal(4, result[0].Item1[3]);


            Assert.Equal("foo1", result[0].Item2[0]);
            Assert.Equal("foo2", result[0].Item2[1]);
            Assert.Equal("foo3", result[0].Item2[2]);
            Assert.Equal("foo4", result[0].Item2[3]);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Item3[0]);
            Assert.Equal(new DateTime(1978, 5, 19), result[0].Item3[1]);
            Assert.Equal(new DateTime(1979, 5, 19), result[0].Item3[2]);
            Assert.Equal(new DateTime(1980, 5, 19), result[0].Item3[3]);

            Assert.True(result[0].Item4[0]);
            Assert.False(result[0].Item4[1]);
            Assert.True(result[0].Item4[2]);
            Assert.False(result[0].Item4[3]);

            Assert.Equal("bar1", result[0].Item5[0]);
            Assert.Equal("bar2", result[0].Item5[1]);
            Assert.Equal("bar3", result[0].Item5[2]);
            Assert.Equal("bar4", result[0].Item5[3]);
        }

        [Fact]
        public void Map_Arrays_To_Named_Tuple_Values_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<(int[] Id, string[] Foo, DateTime[] Day, bool[] Bool, string[] Bar)>(ArraysQuery).ToList();

            Assert.Single(result);


            Assert.Equal(1, result[0].Id[0]);
            Assert.Equal(2, result[0].Id[1]);
            Assert.Equal(3, result[0].Id[2]);
            Assert.Equal(4, result[0].Id[3]);

            Assert.Equal("foo1", result[0].Foo[0]);
            Assert.Equal("foo2", result[0].Foo[1]);
            Assert.Equal("foo3", result[0].Foo[2]);
            Assert.Equal("foo4", result[0].Foo[3]);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Day[0]);
            Assert.Equal(new DateTime(1978, 5, 19), result[0].Day[1]);
            Assert.Equal(new DateTime(1979, 5, 19), result[0].Day[2]);
            Assert.Equal(new DateTime(1980, 5, 19), result[0].Day[3]);

            Assert.True(result[0].Bool[0]);
            Assert.False(result[0].Bool[1]);
            Assert.True(result[0].Bool[2]);
            Assert.False(result[0].Bool[3]);

            Assert.Equal("bar1", result[0].Bar[0]);
            Assert.Equal("bar2", result[0].Bar[1]);
            Assert.Equal("bar3", result[0].Bar[2]);
            Assert.Equal("bar4", result[0].Bar[3]);
        }

        [Fact]
        public void Map_Named_Tuples_Array_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<(int[] i, string[] s)>(@"
                            select array_agg(i), array_agg(s)
                            from (
                            values 
                                (1, 'foo1'),
                                (2, 'foo2'),
                                (3, 'foo3')
                            ) t(i, s)").ToArray();

            Assert.Single(result);

            Assert.Equal(1, result[0].i[0]);
            Assert.Equal(2, result[0].i[1]);
            Assert.Equal(3, result[0].i[2]);

            Assert.Equal("foo1", result[0].s[0]);
            Assert.Equal("foo2", result[0].s[1]);
            Assert.Equal("foo3", result[0].s[2]);
        }

        [Fact]
        public void Map_Nullable_Int_Array_Values_Array_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int?[]>(@"
                            select array_agg(e) as Ints
                            from (
                            values 
                                (1),
                                (null),
                                (2)
                            ) t(e)").ToArray();

            Assert.Single(result);
            Assert.Equal(1, result[0][0]);
            Assert.Null(result[0][1]);
            Assert.Equal(2, result[0][2]);
        }

        [Fact]
        public void Map_Nullable_Value_Tuples_Array_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read<int?[], string[]>(@"
                            select array_agg(i), array_agg(s)
                            from (
                            values 
                                (1, 'foo1'),
                                (null, 'foo2'),
                                (2, null)
                            ) t(i, s)").ToArray();

            Assert.Single(result);
            
            Assert.Equal(1, result[0].Item1[0]);
            Assert.Null(result[0].Item1[1]);
            Assert.Equal(2, result[0].Item1[2]);

            Assert.Equal("foo1", result[0].Item2[0]);
            Assert.Equal("foo2", result[0].Item2[1]);
            Assert.Null(result[0].Item2[2]);
        }

        [Fact]
        public void Map_Nullable_Value_Named_Tuples_Array_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection
                .WithReaderCallback(r => r.Ordinal switch
                {
                    0 => r.Reader.GetFieldValue<int?[]>(0),
                    _ => null
                })
                .Read<(int?[] Ints, string[] Strings)>(@"
                    select array_agg(i), array_agg(s)
                    from (
                    values 
                        (1, 'foo1'),
                        (null, 'foo2'),
                        (2, null)
                    ) t(i, s)")
                .ToArray();

            Assert.Single(result);

            Assert.Equal(1, result[0].Ints[0]);
            Assert.Null(result[0].Ints[1]);
            Assert.Equal(2, result[0].Ints[2]);

            Assert.Equal("foo1", result[0].Strings[0]);
            Assert.Equal("foo2", result[0].Strings[1]);
            Assert.Null(result[0].Strings[2]);
        }

    }
}
