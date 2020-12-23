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
                3 as id3, 4 as id4, 5 as id5, 6 as id6, 7 as id7, 8 as id8, 9 as id9, 10 as id10;";

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
        public void MapThreeRecords_Sync()
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
        public void Map_Mistmatch1_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            Assert.Throws<NormMultipleMappingsException>(() => connection.Read<Record1, int>(Qurey).Single());
        }

        [Fact]
        public void Map_Mistmatch2_Sync()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            Assert.Throws<NormMultipleMappingsException>(() => connection.Read<int, Record1>(Qurey).Single());
        }
    }
}
