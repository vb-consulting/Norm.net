﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Xunit;

namespace SqlServerUnitTests
{
    [Collection("SqlClientDatabase")]
    public class MapUnitTests
    {
        private readonly SqlClientFixture fixture;

        class TestClass
        {
            public int Id { get; set; }
            public string Foo { get; set; }
            public DateTime Day { get; set; }
            public bool? Bool { get; set; }
            public string Bar { get; set; }
        }

        private const string Query = @"
                            select *
                            from (values
                              (1, 'foo1', cast('1977-05-19' as date), cast(1 as bit) , null),
                              (2, 'foo2', cast('1978-05-19' as date), cast(0 as bit), 'bar2'),
                              (3, 'foo3', cast('1979-05-19' as date), null, 'bar3')
                            ) t (id, foo, day, bool, bar)";
        public MapUnitTests(SqlClientFixture fixture)
        {
            this.fixture = fixture;
        }

        private void AssertTestClass(IList<TestClass> result)
        {
            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
            Assert.Equal(3, result[2].Id);

            Assert.Equal("foo1", result[0].Foo);
            Assert.Equal("foo2", result[1].Foo);
            Assert.Equal("foo3", result[2].Foo);

            Assert.Equal(new DateTime(1977, 5, 19), result[0].Day);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].Day);
            Assert.Equal(new DateTime(1979, 5, 19), result[2].Day);

            Assert.Equal(true, result[0].Bool);
            Assert.Equal(false, result[1].Bool);
            Assert.Null(result[2].Bool);

            Assert.Null(result[0].Bar);
            Assert.Equal("bar2", result[1].Bar);
            Assert.Equal("bar3", result[2].Bar);
        }

        [Fact]
        public void Map_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);
            var result = connection.Read<TestClass>(Query).ToList();

            AssertTestClass(result);
        }


        [Fact]
        public async Task Map_Async()
        {
            await using var connection = new SqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync<TestClass>(Query).ToListAsync();

            AssertTestClass(result);
        }

        public class DateTimeOffsetClass
        {
            public DateTimeOffset Value { get; set; }
        }

        [Fact]
        public void Test_Actual_DateTimeOffset_Sync()
        {
            using var connection = new SqlConnection(fixture.ConnectionString);

            connection.Execute(@"
                create table DateTimeOffsetTest (Value [datetimeoffset](7));
                insert into DateTimeOffsetTest values ('2022-06-16');");

            var result = connection.Read<DateTimeOffsetClass>("select * from DateTimeOffsetTest").ToList();

            var expected = new DateTimeOffset(new DateTime(2022, 6, 16));
            var actual = result.FirstOrDefault().Value;
            Assert.IsType<DateTimeOffset>(actual);
            Assert.Equal(expected.DateTime, actual.DateTime);
        }
    }
}
