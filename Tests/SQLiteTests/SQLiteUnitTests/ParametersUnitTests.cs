﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Norm;
using Xunit;

namespace SQLiteUnitTests
{
    [Collection("SQLiteDatabase")]
    public class ParametersUnitTests
    {
        private readonly SqLiteFixture fixture;

        public ParametersUnitTests(SqLiteFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void PositionalParams_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection
                .WithParameters("str", 999, true, new DateTime(1977, 5, 19))
                .Read<string, long, long, string>("select @s, @i, @b, @d")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.Equal(1, b);
            Assert.Equal("1977-05-19 00:00:00", d);
        }

        [Fact]
        public void NamedParams_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection
                .WithParameters(new
                {
                    d = new DateTime(1977, 5, 19),
                    b = true,
                    i = 999,
                    s = "str"
                })
                .Read<string, long, long, string>("select @s, @i, @b, @d")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.Equal(1, b);
            Assert.Equal("1977-05-19 00:00:00", d);
        }

        [Fact]
        public void DbParams_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection
                .WithParameters(new SQLiteParameter("s", "str"),
                    new SQLiteParameter("i", 999),
                    new SQLiteParameter("b", true),
                    new SQLiteParameter("d", new DateTime(1977, 5, 19)))
                .Read<string, long, long, string>(
                "select @s, @i, @b, @d")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.Equal(1, b);
            Assert.Equal("1977-05-19 00:00:00", d);

            (s, i, b, d) = connection
                .WithParameters(new SQLiteParameter("d", new DateTime(1977, 5, 19)),
                    new SQLiteParameter("b", true),
                    new SQLiteParameter("i", 999),
                    new SQLiteParameter("s", "str"))
                .Read<string, long, long, string>(
                "select @s, @i, @b, @d")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.Equal(1, b);
            Assert.Equal("1977-05-19 00:00:00", d); 
        }

        [Fact]
        public void MixedParams_Test()
        {
            using var connection = new SQLiteConnection(fixture.ConnectionString);
            var (s, i, b, d) = connection
                .WithParameters(new SQLiteParameter("d", new DateTime(1977, 5, 19)), "str", 999, true)
                .Read<string, long, long, string>("select @s, @i, @b, @d")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.Equal(1, b);
            Assert.Equal("1977-05-19 00:00:00", d);

            (s, i, b, d) = connection
                .WithParameters(new SQLiteParameter("s", "str"), new SQLiteParameter("i", 999), true, new DateTime(1977, 5, 19))
                .Read<string, long, long, string>("select @s, @i, @b, @d")
                .Single();

            Assert.Equal("str", s);
            Assert.Equal(999, i);
            Assert.Equal(1, b);
            Assert.Equal("1977-05-19 00:00:00", d);
        }
    }
}
