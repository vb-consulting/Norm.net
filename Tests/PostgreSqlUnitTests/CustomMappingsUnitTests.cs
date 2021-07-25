using System;
using System.Collections.Generic;
using System.Linq;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{

    public class BigGuidClass
    {
        public Guid GuidId1 { get; set; }
        public Guid GuidId2 { get; set; }
        public Guid GuidId3 { get; set; }
        public Guid GuidId4 { get; set; }
        public Guid GuidId5 { get; set; }
        public Guid GuidId6 { get; set; }
        public Guid GuidId7 { get; set; }
        public Guid GuidId8 { get; set; }
        public Guid GuidId9 { get; set; }
        public Guid GuidId10 { get; set; }
        public Guid GuidId11 { get; set; }
        public Guid GuidId12 { get; set; }
        public Guid GuidId13 { get; set; }
        public Guid GuidId14 { get; set; }
        public string StringId1 { get; set; }
        public string StringId2 { get; set; }
        public string StringId3 { get; set; }
        public string StringId4 { get; set; }
        public string StringId5 { get; set; }
        public string StringId6 { get; set; }
        public string StringId7 { get; set; }
        public string StringId8 { get; set; }
        public string StringId9 { get; set; }
        public string StringId10 { get; set; }
        public string StringId11 { get; set; }
        public string StringId12 { get; set; }
        public string StringId13 { get; set; }
        public string StringId14 { get; set; }
    }

    public static class MappingExtensions
    {
        public static IEnumerable<BigGuidClass> BigGuidCustomMapping(this IEnumerable<(string name, object value)[]> values)
        {
            foreach (var value in values)
            {
                yield return new BigGuidClass
                {
                    GuidId1 = (Guid)value[0].value,
                    GuidId2 = (Guid)value[1].value,
                    GuidId3 = (Guid)value[2].value,
                    GuidId4 = (Guid)value[3].value,
                    GuidId5 = (Guid)value[4].value,
                    GuidId6 = (Guid)value[5].value,
                    GuidId7 = (Guid)value[6].value,
                    GuidId8 = (Guid)value[7].value,
                    GuidId9 = (Guid)value[8].value,
                    GuidId10 = (Guid)value[9].value,
                    GuidId11 = (Guid)value[10].value,
                    GuidId12 = (Guid)value[11].value,
                    GuidId13 = (Guid)value[12].value,
                    GuidId14 = (Guid)value[13].value,

                    StringId1 = value[0].value.ToString(),
                    StringId2 = value[1].value.ToString(),
                    StringId3 = value[2].value.ToString(),
                    StringId4 = value[3].value.ToString(),
                    StringId5 = value[4].value.ToString(),
                    StringId6 = value[5].value.ToString(),
                    StringId7 = value[6].value.ToString(),
                    StringId8 = value[7].value.ToString(),
                    StringId9 = value[8].value.ToString(),
                    StringId10 = value[9].value.ToString(),
                    StringId11 = value[10].value.ToString(),
                    StringId12 = value[11].value.ToString(),
                    StringId13 = value[12].value.ToString(),
                    StringId14 = value[13].value.ToString(),
                };
            }
        }
    }

    [Collection("PostgreSqlDatabase")]
    public class CustomMappingsUnitTests
    {
        private readonly PostgreSqlFixture fixture;

        public CustomMappingsUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Test_Custom_Mapping_Of_Guids_And_Strings_To_BigClass_Example()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection.Execute(@"create extension if not exists ""uuid-ossp""");

            var result = connection
                .Read(@"select 
                    uuid_generate_v4() id1,
                    uuid_generate_v4() id2,
                    uuid_generate_v4() id3,
                    uuid_generate_v4() id4,
                    uuid_generate_v4() id5,
                    uuid_generate_v4() id6,
                    uuid_generate_v4() id7,
                    uuid_generate_v4() id8,
                    uuid_generate_v4() id9,
                    uuid_generate_v4() id10,
                    uuid_generate_v4() id11,
                    uuid_generate_v4() id12,
                    uuid_generate_v4() id13,
                    uuid_generate_v4() id14")
                .BigGuidCustomMapping()
                .First();

            Assert.Equal(result.GuidId1.ToString(), result.StringId1);
            Assert.Equal(result.GuidId2.ToString(), result.StringId2);
            Assert.Equal(result.GuidId3.ToString(), result.StringId3);
            Assert.Equal(result.GuidId4.ToString(), result.StringId4);
            Assert.Equal(result.GuidId5.ToString(), result.StringId5);
            Assert.Equal(result.GuidId6.ToString(), result.StringId6);
            Assert.Equal(result.GuidId7.ToString(), result.StringId7);
            Assert.Equal(result.GuidId8.ToString(), result.StringId8);
            Assert.Equal(result.GuidId9.ToString(), result.StringId9);
            Assert.Equal(result.GuidId10.ToString(), result.StringId10);
            Assert.Equal(result.GuidId11.ToString(), result.StringId11);
            Assert.Equal(result.GuidId12.ToString(), result.StringId12);
            Assert.Equal(result.GuidId13.ToString(), result.StringId13);
            Assert.Equal(result.GuidId14.ToString(), result.StringId14);
        }

        class CustomMapTestClass
        {
            public string Str1 { get; set; }
            public Guid Guid1 { get; set; }
            public string Str2 { get; set; }
            public Guid Guid2 { get; set; }
        }

        [Fact]
        public void Test_Custom_Mapping_Tuples_Handler()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            SqlMapper.AddTypeByTuples(row => new CustomMapTestClass
            {
                Str1 = row.First(r => r.name == "id1").value.ToString(),
                Guid1 = (Guid)row.First(r => r.name == "id1").value,
                Str2 = row.First(r => r.name == "id2").value.ToString(),
                Guid2 = (Guid)row.First(r => r.name == "id2").value
            });

            connection.Execute(@"create extension if not exists ""uuid-ossp""");
            var result = connection
                .Read<CustomMapTestClass>("select uuid_generate_v4() id1, uuid_generate_v4() id2")
                .ToList();

            Assert.Single(result);
            Assert.Equal(result[0].Guid1.ToString(), result[0].Str1);
            Assert.Equal(result[0].Guid2.ToString(), result[0].Str2);
        }

        [Fact]
        public void Test_Custom_Mapping_Values_Handler()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            SqlMapper.AddTypeByValues(row => new CustomMapTestClass
            {
                Str1 = row[0].ToString(),
                Guid1 = (Guid)row[0],
                Str2 = row[1].ToString(),
                Guid2 = (Guid)row[1]
            });

            connection.Execute(@"create extension if not exists ""uuid-ossp""");
            var result = connection
                .Read<CustomMapTestClass>("select uuid_generate_v4() id1, uuid_generate_v4() id2")
                .ToList();

            Assert.Single(result);
            Assert.Equal(result[0].Guid1.ToString(), result[0].Str1);
            Assert.Equal(result[0].Guid2.ToString(), result[0].Str2);
        }

        [Fact]
        public void Test_Custom_Mapping_Dict_Handler()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            SqlMapper.AddTypeByDict(row => new CustomMapTestClass
            {
                Str1 = row["id1"].ToString(),
                Guid1 = (Guid)row["id1"],
                Str2 = row["id2"].ToString(),
                Guid2 = (Guid)row["id2"]
            });

            connection.Execute(@"create extension if not exists ""uuid-ossp""");
            var result = connection
                .Read<CustomMapTestClass>("select uuid_generate_v4() id1, uuid_generate_v4() id2")
                .ToList();

            Assert.Single(result);
            Assert.Equal(result[0].Guid1.ToString(), result[0].Str1);
            Assert.Equal(result[0].Guid2.ToString(), result[0].Str2);
        }

        class GuidClass
        {
            public Guid Id1 { get; set; }
            public Guid Id2 { get; set; }
        }

        [Fact]
        public void Test_Guids_Mapping()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection.Execute(@"create extension if not exists ""uuid-ossp""");

            var result = connection
                .Read<GuidClass>("select '2f5814c8-94a5-43ca-9436-573df999bb50'::uuid id1, '73fe0dfe-05a2-479b-adda-31e7d662b677'::uuid id2")
                .ToList();

            Assert.Single(result);
            Assert.Equal(new Guid("2f5814c8-94a5-43ca-9436-573df999bb50"), result[0].Id1);
            Assert.Equal(new Guid("73fe0dfe-05a2-479b-adda-31e7d662b677"), result[0].Id2);
        }

        class GuidArrayClass
        {
            public Guid[] Guids { get; set; }
        }

        [Fact]
        public void Test_Guids_Array_Mapping()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection.Execute(@"create extension if not exists ""uuid-ossp""");

            var result = connection
                .Read<GuidArrayClass>("select array['2f5814c8-94a5-43ca-9436-573df999bb50'::uuid, '73fe0dfe-05a2-479b-adda-31e7d662b677'::uuid] guids")
                .ToList();

            Assert.Single(result);
            Assert.Equal(2, result[0].Guids.Length);
            Assert.Equal(new Guid("2f5814c8-94a5-43ca-9436-573df999bb50"), result[0].Guids[0]);
            Assert.Equal(new Guid("73fe0dfe-05a2-479b-adda-31e7d662b677"), result[0].Guids[1]);
        }

        [Fact]
        public void Test_Guids_Mapping_Tuple_Values()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection.Execute(@"create extension if not exists ""uuid-ossp""");

            var result = connection
                .Read<Guid, Guid>("select '2f5814c8-94a5-43ca-9436-573df999bb50'::uuid id1, '73fe0dfe-05a2-479b-adda-31e7d662b677'::uuid id2")
                .ToList();

            Assert.Single(result);
            Assert.Equal(new Guid("2f5814c8-94a5-43ca-9436-573df999bb50"), result[0].Item1);
            Assert.Equal(new Guid("73fe0dfe-05a2-479b-adda-31e7d662b677"), result[0].Item2);
        }

        [Fact]
        public void Test_Guids_Mapping_Named_Tuple_Values()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            connection.Execute(@"create extension if not exists ""uuid-ossp""");

            var result = connection
                .Read<(Guid id1, Guid id2)>("select '2f5814c8-94a5-43ca-9436-573df999bb50'::uuid id1, '73fe0dfe-05a2-479b-adda-31e7d662b677'::uuid id2")
                .ToList();

            Assert.Single(result);
            Assert.Equal(new Guid("2f5814c8-94a5-43ca-9436-573df999bb50"), result[0].id1);
            Assert.Equal(new Guid("73fe0dfe-05a2-479b-adda-31e7d662b677"), result[0].id2);
        }
    }
}
