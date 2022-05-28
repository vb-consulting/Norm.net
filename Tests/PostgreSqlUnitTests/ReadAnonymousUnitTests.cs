using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using Xunit;

namespace PostgreSqlUnitTests
{
    [Collection("PostgreSqlDatabase")]
    public class ReadAnonymousUnitTests
    {
        private readonly PostgreSqlFixture fixture;
        private enum TestEnum { Value1, Value2, Value3 }

        public ReadAnonymousUnitTests(PostgreSqlFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ReadAnonymous_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                first = default(int),
                bar = default(string),
                day = default(DateTime?),
                @bool = default(bool?),
                s = default(string),
            }, @"

                select * from (
                    values 
                    (1, 'foo1', '1977-05-19'::date, true, null),
                    (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                    (3, 'foo3', null::date, null, 'bar3')
                ) t(first, bar, day, bool, s)
                
            ").ToList();

            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].first);

            Assert.Equal("foo1", result[0].bar);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].day);
            Assert.Equal(true, result[0].@bool);
            Assert.Null(result[0].s);

            Assert.Equal(2, result[1].first);
            Assert.Equal("foo2", result[1].bar);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].day);
            Assert.Equal(false, result[1].@bool);
            Assert.Equal("bar2", result[1].s);

            Assert.Equal(3, result[2].first);
            Assert.Equal("foo3", result[2].bar);
            Assert.Null(result[2].day);
            Assert.Null(result[2].@bool);
            Assert.Equal("bar3", result[2].s);
        }

        [Fact]
        public void ReadAnonymous_Unordered_DifferentCase_Unmached_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                s = default(string),
                Bar = default(string),
                @bool = default(bool?),
                first = default(int),
                dAy = default(DateTime?),
                missing = default(string),
            }, @"

                select * from (
                    values 
                    (1, 'foo1', '1977-05-19'::date, true, null, 'x'),
                    (2, 'foo2', '1978-05-19'::date, false, 'bar2', 'x'),
                    (3, 'foo3', null::date, null, 'bar3', 'x')
                ) t(first, bar, day, bo_ol, s, x)
                
            ").ToList();

            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].first);

            Assert.Equal("foo1", result[0].Bar);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].dAy);
            Assert.Equal(true, result[0].@bool);
            Assert.Null(result[0].s);
            Assert.Null(result[0].missing);

            Assert.Equal(2, result[1].first);
            Assert.Equal("foo2", result[1].Bar);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].dAy);
            Assert.Equal(false, result[1].@bool);
            Assert.Equal("bar2", result[1].s);
            Assert.Null(result[1].missing);

            Assert.Equal(3, result[2].first);
            Assert.Equal("foo3", result[2].Bar);
            Assert.Null(result[2].dAy);
            Assert.Null(result[2].@bool);
            Assert.Equal("bar3", result[2].s);
            Assert.Null(result[2].missing);
        }

        [Fact]
        public void ReadAnonymous_Guid_Test()
        {
            var guid = Guid.NewGuid();

            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                guid1 = default(Guid),
                guid2 = default(Guid?)
            },
                $"select '{guid}'::uuid as guid1, null::uuid as guid2").FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal(guid, result.guid1);
            Assert.Null(result.guid2);
        }

        [Fact]
        public void ReadAnonymous_Timespan_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                oneday = default(TimeSpan),
                twodays = default(TimeSpan),
                nullable = default(TimeSpan?)
            }, "select interval '1 days' as oneday, interval '2 days' as twodays, null::interval as nullable")
                .FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal(TimeSpan.FromDays(1), result.oneday);
            Assert.Equal(TimeSpan.FromDays(2), result.twodays);
            Assert.Null(result.nullable);
        }

        [Fact]
        public void ReadAnonymous_Array_Test()
        {
            var query = @"
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

            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                Id = default(int[]),
                Foo = default(string[]),
                Day = default(DateTime[]),
                Bool = default(bool[]),
                Bar = default(string[]),
            },
                query).ToList();

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
        public void ReadAnonymous_Nullable_Array_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new { Ints = default(int?[]) }, @"
                            select array_agg(e) as Ints
                            from (
                            values 
                                (1),
                                (null),
                                (2)
                            ) t(e)",
                            r => r.Reader.GetFieldValue<int?[]>(0)).ToArray();

            Assert.Single(result);
            Assert.Equal(1, result[0].Ints[0]);
            Assert.Null(result[0].Ints[1]);
            Assert.Equal(2, result[0].Ints[2]);
        }

        [Fact]
        public void ReadAnonymous_Enums_From_Ints_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                Item1 = default(TestEnum),
                Item2 = default(TestEnum)
            }, @"
                select *
                from (
                values 
                    (0, 2),
                    (1, 1),
                    (2, 0)
                ) t(Item1, Item2)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value3, result[0].Item2);

            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Equal(TestEnum.Value2, result[1].Item2);

            Assert.Equal(TestEnum.Value3, result[2].Item1);
            Assert.Equal(TestEnum.Value1, result[2].Item2);
        }

        [Fact]
        public void ReadAnonymous_Enums_From_Strings_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                Item1 = default(TestEnum),
                Item2 = default(TestEnum)
            }, @"
                select *
                from (
                values 
                    ('Value1', 'Value3'),
                    ('Value2', 'Value2'),
                    ('Value3', 'Value1')
                ) t(Item1, Item2)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value3, result[0].Item2);

            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Equal(TestEnum.Value2, result[1].Item2);

            Assert.Equal(TestEnum.Value3, result[2].Item1);
            Assert.Equal(TestEnum.Value1, result[2].Item2);
        }

        [Fact]
        public void ReadAnonymous_Nullable_Enums_From_Ints_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                Item1 = default(TestEnum?),
            }, @"
                select *
                from (
                values 
                    (0),
                    (1),
                    (null)
                ) t(Item1)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Null(result[2].Item1);
        }

        [Fact]
        public void ReadAnonymous_Nullable_Enums_From_Strings_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                Item1 = default(TestEnum?),
            }, @"
                select *
                from (
                values 
                    ('Value1'),
                    ('Value2'),
                    (null)
                ) t(Item1)").ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Null(result[2].Item1);
        }

        [Fact]
        public void ReadAnonymous_Enum_Array_From_Ints_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                Item1 = default(TestEnum[])
            }, @"
            select 
                array_agg(Item1) as Item1
            from (
            values 
                (0),
                (1),
                (2)
            ) t(Item1)").ToList();

            Assert.Single(result);

            Assert.Equal(TestEnum.Value1, result[0].Item1[0]);
            Assert.Equal(TestEnum.Value2, result[0].Item1[1]);
            Assert.Equal(TestEnum.Value3, result[0].Item1[2]);
        }

        [Fact]
        public void ReadAnonymous_Enum_Array_From_Strings_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                Item1 = default(TestEnum[])
            }, @"
            select 
                array_agg(Item1) as Item1
            from (
            values 
                ('Value1'),
                ('Value2'),
                ('Value3')
            ) t(Item1)").ToList();

            Assert.Single(result);

            Assert.Equal(TestEnum.Value1, result[0].Item1[0]);
            Assert.Equal(TestEnum.Value2, result[0].Item1[1]);
            Assert.Equal(TestEnum.Value3, result[0].Item1[2]);
        }

        [Fact]
        public void ReadAnonymous_Nullable_Enum_Array_From_Ints_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                Item1 = default(TestEnum?[])
            }, @"
            select 
                array_agg(Item1) as Item1
            from (
            values 
                (0),
                (null),
                (2)
            ) t(Item1)", r =>
            {
                var result = new List<TestEnum?>();
                foreach (var value in r.Reader.GetFieldValue<int?[]>(0))
                {
                    if (value == null)
                    {
                        result.Add(null);
                    }
                    else
                    {
                        result.Add((TestEnum)Enum.ToObject(typeof(TestEnum), value));
                    }
                }
                return result.ToArray();
            }).ToList();

            Assert.Single(result);

            Assert.Equal(TestEnum.Value1, result[0].Item1[0]);
            Assert.Null(result[0].Item1[1]);
            Assert.Equal(TestEnum.Value3, result[0].Item1[2]);
        }

        [Fact]
        public void ReadAnonymous_Nullable_Enum_Array_From_Strings_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = connection.Read(new
            {
                Item1 = default(TestEnum?[])
            }, @"
            select 
                array_agg(Item1) as Item1
            from (
            values 
                ('Value1'),
                (null),
                ('Value3')
            ) t(Item1)", r =>
            {
                var result = new List<TestEnum?>();
                foreach (var value in r.Reader.GetFieldValue<string[]>(0))
                {
                    if (value == null)
                    {
                        result.Add(null);
                    }
                    else
                    {
                        result.Add((TestEnum)Enum.Parse(typeof(TestEnum), value));
                    }
                }
                return result.ToArray();
            }).ToList();

            Assert.Single(result);

            Assert.Equal(TestEnum.Value1, result[0].Item1[0]);
            Assert.Null(result[0].Item1[1]);
            Assert.Equal(TestEnum.Value3, result[0].Item1[2]);
        }

        [Fact]
        public async Task ReadAnonymous_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                first = default(int),
                bar = default(string),
                day = default(DateTime?),
                @bool = default(bool?),
                s = default(string),
            }, @"

                select * from (
                    values 
                    (1, 'foo1', '1977-05-19'::date, true, null),
                    (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                    (3, 'foo3', null::date, null, 'bar3')
                ) t(first, bar, day, bool, s)
                
            ").ToListAsync();

            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].first);

            Assert.Equal("foo1", result[0].bar);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].day);
            Assert.Equal(true, result[0].@bool);
            Assert.Null(result[0].s);

            Assert.Equal(2, result[1].first);
            Assert.Equal("foo2", result[1].bar);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].day);
            Assert.Equal(false, result[1].@bool);
            Assert.Equal("bar2", result[1].s);

            Assert.Equal(3, result[2].first);
            Assert.Equal("foo3", result[2].bar);
            Assert.Null(result[2].day);
            Assert.Null(result[2].@bool);
            Assert.Equal("bar3", result[2].s);
        }

        [Fact]
        public async Task ReadAnonymous_Unordered_DifferentCase_Unmached_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                s = default(string),
                Bar = default(string),
                @bool = default(bool?),
                first = default(int),
                dAy = default(DateTime?),
                missing = default(string),
            }, @"

                select * from (
                    values 
                    (1, 'foo1', '1977-05-19'::date, true, null, 'x'),
                    (2, 'foo2', '1978-05-19'::date, false, 'bar2', 'x'),
                    (3, 'foo3', null::date, null, 'bar3', 'x')
                ) t(first, bar, day, bo_ol, s, x)
                
            ").ToListAsync();

            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].first);

            Assert.Equal("foo1", result[0].Bar);
            Assert.Equal(new DateTime(1977, 5, 19), result[0].dAy);
            Assert.Equal(true, result[0].@bool);
            Assert.Null(result[0].s);
            Assert.Null(result[0].missing);

            Assert.Equal(2, result[1].first);
            Assert.Equal("foo2", result[1].Bar);
            Assert.Equal(new DateTime(1978, 5, 19), result[1].dAy);
            Assert.Equal(false, result[1].@bool);
            Assert.Equal("bar2", result[1].s);
            Assert.Null(result[1].missing);

            Assert.Equal(3, result[2].first);
            Assert.Equal("foo3", result[2].Bar);
            Assert.Null(result[2].dAy);
            Assert.Null(result[2].@bool);
            Assert.Equal("bar3", result[2].s);
            Assert.Null(result[2].missing);
        }

        [Fact]
        public async Task ReadAnonymous_Guid_Test_Async()
        {
            var guid = Guid.NewGuid();

            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                guid1 = default(Guid),
                guid2 = default(Guid?)
            },
                $"select '{guid}'::uuid as guid1, null::uuid as guid2").FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.Equal(guid, result.guid1);
            Assert.Null(result.guid2);
        }

        [Fact]
        public async Task ReadAnonymous_Timespan_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                oneday = default(TimeSpan),
                twodays = default(TimeSpan),
                nullable = default(TimeSpan?)
            }, "select interval '1 days' as oneday, interval '2 days' as twodays, null::interval as nullable")
                .FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.Equal(TimeSpan.FromDays(1), result.oneday);
            Assert.Equal(TimeSpan.FromDays(2), result.twodays);
            Assert.Null(result.nullable);
        }

        [Fact]
        public async Task ReadAnonymous_Array_Test_Async()
        {
            var query = @"
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

            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                Id = default(int[]),
                Foo = default(string[]),
                Day = default(DateTime[]),
                Bool = default(bool[]),
                Bar = default(string[]),
            },
                query).ToListAsync();

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
        public async Task ReadAnonymous_Nullable_Array_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new { Ints = default(int?[]) }, @"
                            select array_agg(e) as Ints
                            from (
                            values 
                                (1),
                                (null),
                                (2)
                            ) t(e)",
                            r => r.Reader.GetFieldValue<int?[]>(0)).ToArrayAsync();

            Assert.Single(result);
            Assert.Equal(1, result[0].Ints[0]);
            Assert.Null(result[0].Ints[1]);
            Assert.Equal(2, result[0].Ints[2]);
        }

        [Fact]
        public async Task ReadAnonymous_Enums_From_Ints_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                Item1 = default(TestEnum),
                Item2 = default(TestEnum)
            }, @"
                select *
                from (
                values 
                    (0, 2),
                    (1, 1),
                    (2, 0)
                ) t(Item1, Item2)").ToArrayAsync();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value3, result[0].Item2);

            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Equal(TestEnum.Value2, result[1].Item2);

            Assert.Equal(TestEnum.Value3, result[2].Item1);
            Assert.Equal(TestEnum.Value1, result[2].Item2);
        }

        [Fact]
        public async Task ReadAnonymous_Enums_From_Strings_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                Item1 = default(TestEnum),
                Item2 = default(TestEnum)
            }, @"
                select *
                from (
                values 
                    ('Value1', 'Value3'),
                    ('Value2', 'Value2'),
                    ('Value3', 'Value1')
                ) t(Item1, Item2)").ToArrayAsync();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value3, result[0].Item2);

            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Equal(TestEnum.Value2, result[1].Item2);

            Assert.Equal(TestEnum.Value3, result[2].Item1);
            Assert.Equal(TestEnum.Value1, result[2].Item2);
        }

        [Fact]
        public async Task ReadAnonymous_Nullable_Enums_From_Ints_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                Item1 = default(TestEnum?),
            }, @"
                select *
                from (
                values 
                    (0),
                    (1),
                    (null)
                ) t(Item1)").ToArrayAsync();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Null(result[2].Item1);
        }

        [Fact]
        public async Task ReadAnonymous_Nullable_Enums_From_Strings_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                Item1 = default(TestEnum?),
            }, @"
                select *
                from (
                values 
                    ('Value1'),
                    ('Value2'),
                    (null)
                ) t(Item1)").ToArrayAsync();

            Assert.Equal(3, result.Length);
            Assert.Equal(TestEnum.Value1, result[0].Item1);
            Assert.Equal(TestEnum.Value2, result[1].Item1);
            Assert.Null(result[2].Item1);
        }

        [Fact]
        public async Task ReadAnonymous_Enum_Array_From_Ints_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                Item1 = default(TestEnum[])
            }, @"
            select 
                array_agg(Item1) as Item1
            from (
            values 
                (0),
                (1),
                (2)
            ) t(Item1)").ToListAsync();

            Assert.Single(result);

            Assert.Equal(TestEnum.Value1, result[0].Item1[0]);
            Assert.Equal(TestEnum.Value2, result[0].Item1[1]);
            Assert.Equal(TestEnum.Value3, result[0].Item1[2]);
        }

        [Fact]
        public async Task ReadAnonymous_Enum_Array_From_Strings_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                Item1 = default(TestEnum[])
            }, @"
            select 
                array_agg(Item1) as Item1
            from (
            values 
                ('Value1'),
                ('Value2'),
                ('Value3')
            ) t(Item1)").ToListAsync();

            Assert.Single(result);

            Assert.Equal(TestEnum.Value1, result[0].Item1[0]);
            Assert.Equal(TestEnum.Value2, result[0].Item1[1]);
            Assert.Equal(TestEnum.Value3, result[0].Item1[2]);
        }

        [Fact]
        public async Task ReadAnonymous_Nullable_Enum_Array_From_Ints_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                Item1 = default(TestEnum?[])
            }, @"
            select 
                array_agg(Item1) as Item1
            from (
            values 
                (0),
                (null),
                (2)
            ) t(Item1)", r =>
            {
                var result = new List<TestEnum?>();
                foreach (var value in r.Reader.GetFieldValue<int?[]>(0))
                {
                    if (value == null)
                    {
                        result.Add(null);
                    }
                    else
                    {
                        result.Add((TestEnum)Enum.ToObject(typeof(TestEnum), value));
                    }
                }
                return result.ToArray();
            }).ToListAsync();

            Assert.Single(result);

            Assert.Equal(TestEnum.Value1, result[0].Item1[0]);
            Assert.Null(result[0].Item1[1]);
            Assert.Equal(TestEnum.Value3, result[0].Item1[2]);
        }

        [Fact]
        public async Task ReadAnonymous_Nullable_Enum_Array_From_Strings_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);
            var result = await connection.ReadAsync(new
            {
                Item1 = default(TestEnum?[])
            }, @"
            select 
                array_agg(Item1) as Item1
            from (
            values 
                ('Value1'),
                (null),
                ('Value3')
            ) t(Item1)", r =>
            {
                var result = new List<TestEnum?>();
                foreach (var value in r.Reader.GetFieldValue<string[]>(0))
                {
                    if (value == null)
                    {
                        result.Add(null);
                    }
                    else
                    {
                        result.Add((TestEnum)Enum.Parse(typeof(TestEnum), value));
                    }
                }
                return result.ToArray();
            }).ToListAsync();

            Assert.Single(result);

            Assert.Equal(TestEnum.Value1, result[0].Item1[0]);
            Assert.Null(result[0].Item1[1]);
            Assert.Equal(TestEnum.Value3, result[0].Item1[2]);
        }

        [Fact]
        public void ReadAnonymous_Changelog_Example_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var query = @"select * from (values
                (1, 'foo1', cast('2022-01-01' as date)), 
                (2, 'foo2', cast('2022-01-10' as date)), 
                (3, 'foo3', cast('2022-01-20' as date))
            ) t(id, foo, date)";

            var result = connection.Read(new { 
                id = default(int), 
                foo = default(string), 
                date = default(DateTime) 
            }, query).ToList();

            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].id);
            Assert.Equal("foo1", result[0].foo);
            Assert.Equal(new DateTime(2022, 1, 1), result[0].date);

            Assert.Equal(2, result[1].id);
            Assert.Equal("foo2", result[1].foo);
            Assert.Equal(new DateTime(2022, 1, 10), result[1].date);

            Assert.Equal(3, result[2].id);
            Assert.Equal("foo3", result[2].foo);
            Assert.Equal(new DateTime(2022, 1, 20), result[2].date);
        }

        [Fact]
        public void ReadAnonymous_Named_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var query = @"select * from (values
                (1, 'foo1', cast('2022-01-01' as date)), 
                (2, 'foo2', cast('2022-01-10' as date)), 
                (3, 'foo3', cast('2022-01-20' as date))
            ) t(id, foo, date)
            where id = @id and foo = @foo";

            var result = connection
                .WithParameters(new { id = 1, foo = "foo1" })
                .Read(new
            {
                id = default(int),
                foo = default(string),
                date = default(DateTime)
            }, query).ToList();

            Assert.Single(result);

            Assert.Equal(1, result[0].id);
            Assert.Equal("foo1", result[0].foo);
            Assert.Equal(new DateTime(2022, 1, 1), result[0].date);
        }

        [Fact]
        public void ReadAnonymous_Format_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = connection.ReadFormat(new
            {
                id = default(int),
                foo = default(string),
                date = default(DateTime)
            }, @$"select * from (values
                (1, 'foo1', cast('2022-01-01' as date)), 
                (2, 'foo2', cast('2022-01-10' as date)), 
                (3, 'foo3', cast('2022-01-20' as date))
            ) t(id, foo, date)
            where id = {1} and foo = {"foo1"}").ToList();

            Assert.Single(result);

            Assert.Equal(1, result[0].id);
            Assert.Equal("foo1", result[0].foo);
            Assert.Equal(new DateTime(2022, 1, 1), result[0].date);
        }

        [Fact]
        public void ReadAnonymous_Positional_Params_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var query = @"select * from (values
                (1, 'foo1', cast('2022-01-01' as date)), 
                (2, 'foo2', cast('2022-01-10' as date)), 
                (3, 'foo3', cast('2022-01-20' as date))
            ) t(id, foo, date)
            where id = @id and foo = @foo";

            var result = connection.WithParameters(1, "foo1").Read(new
            {
                id = default(int),
                foo = default(string),
                date = default(DateTime)
            }, query).ToList();

            Assert.Single(result);

            Assert.Equal(1, result[0].id);
            Assert.Equal("foo1", result[0].foo);
            Assert.Equal(new DateTime(2022, 1, 1), result[0].date);
        }

        [Fact]
        public async Task ReadAnonymous_Named_Params_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var query = @"select * from (values
                (1, 'foo1', cast('2022-01-01' as date)), 
                (2, 'foo2', cast('2022-01-10' as date)), 
                (3, 'foo3', cast('2022-01-20' as date))
            ) t(id, foo, date)
            where id = @id and foo = @foo";

            var result = await connection
                .WithParameters(new { id = 1, foo = "foo1" })
                .ReadAsync(new
            {
                id = default(int),
                foo = default(string),
                date = default(DateTime)
            }, query)
                .ToListAsync();

            Assert.Single(result);

            Assert.Equal(1, result[0].id);
            Assert.Equal("foo1", result[0].foo);
            Assert.Equal(new DateTime(2022, 1, 1), result[0].date);
        }

        [Fact]
        public async Task ReadAnonymous_Format_Params_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var result = await connection.ReadFormatAsync(new
            {
                id = default(int),
                foo = default(string),
                date = default(DateTime)
            }, @$"select * from (values
                (1, 'foo1', cast('2022-01-01' as date)), 
                (2, 'foo2', cast('2022-01-10' as date)), 
                (3, 'foo3', cast('2022-01-20' as date))
            ) t(id, foo, date)
            where id = {1} and foo = {"foo1"}").ToListAsync();

            Assert.Single(result);

            Assert.Equal(1, result[0].id);
            Assert.Equal("foo1", result[0].foo);
            Assert.Equal(new DateTime(2022, 1, 1), result[0].date);
        }

        [Fact]
        public async Task ReadAnonymous_Positional_Params_Test_Async()
        {
            await using var connection = new NpgsqlConnection(fixture.ConnectionString);

            var query = @"select * from (values
                (1, 'foo1', cast('2022-01-01' as date)), 
                (2, 'foo2', cast('2022-01-10' as date)), 
                (3, 'foo3', cast('2022-01-20' as date))
            ) t(id, foo, date)
            where id = @id and foo = @foo";

            var result = await connection.WithParameters(1, "foo1").ReadAsync(new
            {
                id = default(int),
                foo = default(string),
                date = default(DateTime)
            }, query).ToListAsync();

            Assert.Single(result);

            Assert.Equal(1, result[0].id);
            Assert.Equal("foo1", result[0].foo);
            Assert.Equal(new DateTime(2022, 1, 1), result[0].date);
        }

        [Fact]
        public void ReadAnonymous_Multiple_Test()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            using var multiple = connection.Multiple(@"
                select * from (
                    values 
                    (1, 'foo1'),
                    (2, 'foo2'),
                    (3, 'foo3')
                ) t(first, bar);

                select * from (
                    values 
                    ('1977-05-19'::date, true, null),
                    ('1978-05-19'::date, false, 'bar2'),
                    (null::date, null, 'bar3')
                ) t(day, bool, s)
            ");

            var first = multiple.Read(new
            {
                first = default(int),
                bar = default(string)
            }).ToList();

            multiple.Next();

            var second = multiple.Read(new
            {
                day = default(DateTime?),
                @bool = default(bool?),
                s = default(string),
            }).ToList();

            Assert.Equal(3, first.Count);
            Assert.Equal(3, second.Count);

            Assert.Equal(1, first[0].first);
            Assert.Equal("foo1", first[0].bar);

            Assert.Equal(new DateTime(1977, 5, 19), second[0].day);
            Assert.Equal(true, second[0].@bool);
            Assert.Null(second[0].s);

            Assert.Equal(2, first[1].first);
            Assert.Equal("foo2", first[1].bar);

            Assert.Equal(new DateTime(1978, 5, 19), second[1].day);
            Assert.Equal(false, second[1].@bool);
            Assert.Equal("bar2", second[1].s);

            Assert.Equal(3, first[2].first);
            Assert.Equal("foo3", first[2].bar);

            Assert.Null(second[2].day);
            Assert.Null(second[2].@bool);
            Assert.Equal("bar3", second[2].s);
        }

        [Fact]
        public async Task ReadAnonymous_Multiple_Test_Async()
        {
            using var connection = new NpgsqlConnection(fixture.ConnectionString);

            using var multiple = await connection.MultipleAsync(@"
                select * from (
                    values 
                    (1, 'foo1'),
                    (2, 'foo2'),
                    (3, 'foo3')
                ) t(first, bar);

                select * from (
                    values 
                    ('1977-05-19'::date, true, null),
                    ('1978-05-19'::date, false, 'bar2'),
                    (null::date, null, 'bar3')
                ) t(day, bool, s)
            ");

            var first = await multiple.ReadAsync(new
            {
                first = default(int),
                bar = default(string)
            }).ToListAsync();

            await multiple.NextAsync();

            var second = await multiple.ReadAsync(new
            {
                day = default(DateTime?),
                @bool = default(bool?),
                s = default(string),
            }).ToListAsync();

            Assert.Equal(3, first.Count);
            Assert.Equal(3, second.Count);

            Assert.Equal(1, first[0].first);
            Assert.Equal("foo1", first[0].bar);

            Assert.Equal(new DateTime(1977, 5, 19), second[0].day);
            Assert.Equal(true, second[0].@bool);
            Assert.Null(second[0].s);

            Assert.Equal(2, first[1].first);
            Assert.Equal("foo2", first[1].bar);

            Assert.Equal(new DateTime(1978, 5, 19), second[1].day);
            Assert.Equal(false, second[1].@bool);
            Assert.Equal("bar2", second[1].s);

            Assert.Equal(3, first[2].first);
            Assert.Equal("foo3", first[2].bar);

            Assert.Null(second[2].day);
            Assert.Null(second[2].@bool);
            Assert.Equal("bar3", second[2].s);
        }
    }
}
