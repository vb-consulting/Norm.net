using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using NpgsqlTypes;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class QueryMultipleMapsUnitTests
    {
        public record Record1(int Id1, string Foo1, string Bar1);
        public record Record2(int Id2, string Foo2, string Bar2);
        public record Record3(string Foo3, string Bar3, int Id3);

        private readonly PostgreSqlFixture fixture;
        private readonly string Qurey = @"
            select 
                1 as id1, 
                'foo1' as foo1, 
                'bar1' as bar1, 
                2 as id2, 
                'foo2' as foo2, 
                'bar2' as bar2,
                'foo3' as foo3, 
                'bar3' as bar3,
                3 as id3, 
                4 as id4, 
                5 as id5, 
                6 as id6, 
                7 as id7, 
                8 as id8, 9 as id9, 
                10 as id10;";

        public QueryMultipleMapsUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void MapTwoRecords_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (result1, result2) = connection.Read<Record1, Record2>(Qurey).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public void MapTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result1 = connection.Read<(int Id1, string Foo1, string Bar1)>(Qurey).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            var result2 = connection.Read<(int Id1, string Foo1, string Bar1, int Id2, string Foo2, string Bar2, string Foo3, string Bar3)>(Qurey).Single();

            Assert.Equal(1, result2.Id1);
            Assert.Equal("foo1", result2.Foo1);
            Assert.Equal("bar1", result2.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);

            Assert.Equal("foo3", result2.Foo3);
            Assert.Equal("bar3", result2.Bar3);
        }

        [Fact]
        public void Map_Two_Simple_Tuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (result1, result2) = connection.Read<(int Id1, string Foo1, string Bar1), (int Id2, string Foo2, string Bar2)>(Qurey).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);
        }

        [Fact]
        public void Map_Two_Complex_Simple_Tuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (result3, result4) = connection.Read<(int Id1, string Foo1, string Bar1, int Id2, string Foo2, string Bar2, string Foo3), (string Bar3, int Id3)>(Qurey).Single();

            Assert.Equal(1, result3.Id1);
            Assert.Equal("foo1", result3.Foo1);
            Assert.Equal("bar1", result3.Bar1);

            Assert.Equal(2, result3.Id2);
            Assert.Equal("foo2", result3.Foo2);
            Assert.Equal("bar2", result3.Bar2);
            Assert.Equal("foo3", result3.Foo3);

            Assert.Equal("bar3", result4.Bar3);
            Assert.Equal(3, result4.Id3);
        }

        [Fact]
        public void Map_Two_Complex_Simple_Tuples_Edge_Case_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (result5, result6) = connection.Read<(int Id1, string Foo1, string Bar1, int Id2, string Foo2, string Bar2, string Foo3, string Bar3), (int Id3, int Id4)>(Qurey).Single();

            Assert.Equal(1, result5.Id1);
            Assert.Equal("foo1", result5.Foo1);
            Assert.Equal("bar1", result5.Bar1);

            Assert.Equal(2, result5.Id2);
            Assert.Equal("foo2", result5.Foo2);
            Assert.Equal("bar2", result5.Bar2);
            Assert.Equal("foo3", result5.Foo3);
            Assert.Equal("bar3", result5.Bar3);

            Assert.Equal(3, result6.Id3);
            Assert.Equal(4, result6.Id4);
        }

        [Fact]
        public void Map_Two_Complex_Complex_Tuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (result7, result8) = connection.Read<
                (int Id1, string Foo1, string Bar1, int Id2, string Foo2, string Bar2, string Foo3, string Bar3), 
                (int Id3, int Id4, int Id5, int Id6, int Id7, int Id8, int Id9, int Id10) >(Qurey).Single();

            Assert.Equal(1, result7.Id1);
            Assert.Equal("foo1", result7.Foo1);
            Assert.Equal("bar1", result7.Bar1);

            Assert.Equal(2, result7.Id2);
            Assert.Equal("foo2", result7.Foo2);
            Assert.Equal("bar2", result7.Bar2);
            Assert.Equal("foo3", result7.Foo3);
            Assert.Equal("bar3", result7.Bar3);

            Assert.Equal(3, result8.Id3);
            Assert.Equal(4, result8.Id4);
            Assert.Equal(5, result8.Id5);
            Assert.Equal(6, result8.Id6);
            Assert.Equal(7, result8.Id7);
            Assert.Equal(8, result8.Id8);
            Assert.Equal(9, result8.Id9);
            Assert.Equal(10, result8.Id10);
        }

        [Fact]
        public void Map_Three_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var (result1, result2, result3) = connection.Read<Record1, Record2, Record3>(Qurey).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal("foo1", result1.Foo1);
            Assert.Equal("bar1", result1.Bar1);

            Assert.Equal(2, result2.Id2);
            Assert.Equal("foo2", result2.Foo2);
            Assert.Equal("bar2", result2.Bar2);

            Assert.Equal("foo3", result3.Foo3);
            Assert.Equal("bar3", result3.Bar3);
            Assert.Equal(3, result3.Id3);
        }

        [Fact]
        public void Map_Three_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ", 
                string.Join(", ", Enumerable.Repeat(1, 6).Select((e, i) => $"{e+i} as id{e + i}")));

            var (result1, result2, result3) = connection.Read<
                (int Id1, int Id2), 
                (int Id3, int Id4), 
                (int Id5, int Id6)>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);
            
            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);
        }

        [Fact]
        public void Map_Three_NamedTuples_TooShort_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 6).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4)>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);
        }

        [Fact]
        public void Map_Three_NamedTuples_TooLong_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 5).Select((e, i) => $"{e + i} as id{e + i}")));

            Assert.Throws<IndexOutOfRangeException>(() => connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6)>(sql).Single());
        }

        [Fact]
        public void Map_Four_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 8).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6),
                (int Id7, int Id8)>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);
        }

        record R12(int Id1, int Id2);
        record R34(int Id3, int Id4);
        record R56(int Id5, int Id6);
        record R78(int Id7, int Id8);

        [Fact]
        public void Map_Four_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 8).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4) = connection.Read<
                R12,
                R34,
                R56,
                R78>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);
        }

        [Fact]
        public void Map_Five_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 10).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6),
                (int Id7, int Id8),
                (int Id9, int Id10) > (sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);
        }

        record R910(int Id9, int Id10);

        [Fact]
        public void Map_Five_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 10).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5) = connection.Read<
                R12,
                R34,
                R56,
                R78,
                R910>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);
        }

        [Fact]
        public void Map_Six_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 12).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6),
                (int Id7, int Id8),
                (int Id9, int Id10),
                (int Id11, int Id12)>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);
        }

        record R1112(int Id11, int Id12);

        [Fact]
        public void Map_Six_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 12).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6) = connection.Read<
                R12,
                R34,
                R56,
                R78,
                R910,
                R1112>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);
        }

        [Fact]
        public void Map_Seven_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 14).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6),
                (int Id7, int Id8),
                (int Id9, int Id10),
                (int Id11, int Id12),
                (int Id13, int Id14)> (sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);
        }

        record R1314(int Id13, int Id14);

        [Fact]
        public void Map_Seven_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 14).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7) = connection.Read<
                R12,
                R34,
                R56,
                R78,
                R910,
                R1112,
                R1314>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);
        }

        [Fact]
        public void Map_Eight_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 16).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6),
                (int Id7, int Id8),
                (int Id9, int Id10),
                (int Id11, int Id12),
                (int Id13, int Id14),
                (int Id15, int Id16)>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);
        }

        record R1516(int Id15, int Id16);

        [Fact]
        public void Map_Eight_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 16).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8) = connection.Read<
                R12,
                R34,
                R56,
                R78,
                R910,
                R1112,
                R1314,
                R1516>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);
        }

        [Fact]
        public void Map_Nine_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 18).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8, result9) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6),
                (int Id7, int Id8),
                (int Id9, int Id10),
                (int Id11, int Id12),
                (int Id13, int Id14),
                (int Id15, int Id16),
                (int Id17, int Id18)>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);

            Assert.Equal(17, result9.Id17);
            Assert.Equal(18, result9.Id18);
        }

        record R1718(int Id17, int Id18);

        [Fact]
        public void Map_Nine_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 18).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8, result9) = connection.Read<
                R12,
                R34,
                R56,
                R78,
                R910,
                R1112,
                R1314,
                R1516,
                R1718>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);

            Assert.Equal(17, result9.Id17);
            Assert.Equal(18, result9.Id18);
        }

        [Fact]
        public void Map_Ten_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 20).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8, result9, result10) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6),
                (int Id7, int Id8),
                (int Id9, int Id10),
                (int Id11, int Id12),
                (int Id13, int Id14),
                (int Id15, int Id16),
                (int Id17, int Id18),
                (int Id19, int Id20)>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);

            Assert.Equal(17, result9.Id17);
            Assert.Equal(18, result9.Id18);

            Assert.Equal(19, result10.Id19);
            Assert.Equal(20, result10.Id20);
        }

        record R1920(int Id19, int Id20);

        [Fact]
        public void Map_Ten_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 20).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8, result9, result10) = connection.Read<
                R12,
                R34,
                R56,
                R78,
                R910,
                R1112,
                R1314,
                R1516,
                R1718,
                R1920>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);

            Assert.Equal(17, result9.Id17);
            Assert.Equal(18, result9.Id18);

            Assert.Equal(19, result10.Id19);
            Assert.Equal(20, result10.Id20);
        }

        [Fact]
        public void Map_Eleven_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 22).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8, result9, result10, result11) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6),
                (int Id7, int Id8),
                (int Id9, int Id10),
                (int Id11, int Id12),
                (int Id13, int Id14),
                (int Id15, int Id16),
                (int Id17, int Id18),
                (int Id19, int Id20),
                (int Id21, int Id22)>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);

            Assert.Equal(17, result9.Id17);
            Assert.Equal(18, result9.Id18);

            Assert.Equal(19, result10.Id19);
            Assert.Equal(20, result10.Id20);

            Assert.Equal(21, result11.Id21);
            Assert.Equal(22, result11.Id22);
        }

        record R2122(int Id21, int Id22);

        [Fact]
        public void Map_Eleven_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 22).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8, result9, result10, result11) = connection.Read<
                R12,
                R34,
                R56,
                R78,
                R910,
                R1112,
                R1314,
                R1516,
                R1718,
                R1920,
                R2122>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);

            Assert.Equal(17, result9.Id17);
            Assert.Equal(18, result9.Id18);

            Assert.Equal(19, result10.Id19);
            Assert.Equal(20, result10.Id20);

            Assert.Equal(21, result11.Id21);
            Assert.Equal(22, result11.Id22);
        }

        [Fact]
        public void Map_Twelve_NamedTuples_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 24).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8, result9, result10, result11, result12) = connection.Read<
                (int Id1, int Id2),
                (int Id3, int Id4),
                (int Id5, int Id6),
                (int Id7, int Id8),
                (int Id9, int Id10),
                (int Id11, int Id12),
                (int Id13, int Id14),
                (int Id15, int Id16),
                (int Id17, int Id18),
                (int Id19, int Id20),
                (int Id21, int Id22),
                (int Id23, int Id24)>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);

            Assert.Equal(17, result9.Id17);
            Assert.Equal(18, result9.Id18);

            Assert.Equal(19, result10.Id19);
            Assert.Equal(20, result10.Id20);

            Assert.Equal(21, result11.Id21);
            Assert.Equal(22, result11.Id22);

            Assert.Equal(23, result12.Id23);
            Assert.Equal(24, result12.Id24);
        }

        record R2324(int Id23, int Id24);

        [Fact]
        public void Map_Twelve_Records_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var sql = string.Concat("select ",
                string.Join(", ", Enumerable.Repeat(1, 24).Select((e, i) => $"{e + i} as id{e + i}")));

            var (result1, result2, result3, result4, result5, result6, result7, result8, result9, result10, result11, result12) = connection.Read<
                R12,
                R34,
                R56,
                R78,
                R910,
                R1112,
                R1314,
                R1516,
                R1718,
                R1920,
                R2122,
                R2324>(sql).Single();

            Assert.Equal(1, result1.Id1);
            Assert.Equal(2, result1.Id2);

            Assert.Equal(3, result2.Id3);
            Assert.Equal(4, result2.Id4);

            Assert.Equal(5, result3.Id5);
            Assert.Equal(6, result3.Id6);

            Assert.Equal(7, result4.Id7);
            Assert.Equal(8, result4.Id8);

            Assert.Equal(9, result5.Id9);
            Assert.Equal(10, result5.Id10);

            Assert.Equal(11, result6.Id11);
            Assert.Equal(12, result6.Id12);

            Assert.Equal(13, result7.Id13);
            Assert.Equal(14, result7.Id14);

            Assert.Equal(15, result8.Id15);
            Assert.Equal(16, result8.Id16);

            Assert.Equal(17, result9.Id17);
            Assert.Equal(18, result9.Id18);

            Assert.Equal(19, result10.Id19);
            Assert.Equal(20, result10.Id20);

            Assert.Equal(21, result11.Id21);
            Assert.Equal(22, result11.Id22);

            Assert.Equal(23, result12.Id23);
            Assert.Equal(24, result12.Id24);
        }

    }
}
