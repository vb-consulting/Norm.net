using System;
using Xunit;
using Norm;

namespace NormPostgreSqlUnitTests
{
    public class SelectUnitTests
    {
        record TestTable1(int Id, string MyValue, bool FooBar);

        [Fact]
        public void Test1()
        {
            var result = Sql
                .Select<TestTable1>()
                .ToString();
            var expected = "select id, my_value, foo_bar from test_table1";
            Assert.Equal(expected, result);
        }


        [Fact]
        public void Test1_1()
        {
            var result = Sql.Select<TestTable1>(
                nameof(TestTable1.Id), nameof(TestTable1.MyValue), nameof(TestTable1.FooBar))
                .ToString();
            var expected = "select id, my_value, foo_bar from test_table1";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test1_2()
        {
            var result = Sql
                .Select<TestTable1>(nameof(TestTable1.Id), nameof(TestTable1.MyValue), nameof(TestTable1.FooBar).As("foo_bar2"))
                .ToString();
            var expected = "select id, my_value, foo_bar as foo_bar2 from test_table1";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test1_3()
        {
            var result = Sql
                .Select<TestTable1>(Sql.Fun.String.Md5(nameof(TestTable1.FooBar)))
                .ToString();
            var expected = "select md5(foo_bar) as foo_bar from test_table1";
            Assert.Equal(expected, result);
        }


        [Fact]
        public void Test1_4()
        {
            var result = Sql
                .Select<TestTable1>(Sql.Fun.String.Md5(nameof(TestTable1.FooBar)).As("foo_bar2"))
                .ToString();
            var expected = "select md5(foo_bar) as foo_bar2 from test_table1";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test1_5()
        {
            var result = Sql.Select<TestTable1>(
                Sql.Value(1), 
                Sql.Value(2).As("two"), 
                Sql.String("str1"), 
                Sql.String("str2").As("str"))
                .ToString();
            var expected = "select 1, 2 as two, 'str1', 'str2' as str from test_table1";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test1_6()
        { 
            var result = Sql.Select(
                Sql.Value(1),
                Sql.Value(2).As("two"),
                Sql.String("str1"),
                Sql.String("str2").As("str"))
                .ToString();
            var expected = "select 1, 2 as two, 'str1', 'str2' as str";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2()
        {
            var result = Sql
                .Select<TestTable1>(t => new { t.Id, t.MyValue })
                .ToString();
            var expected = "select id, my_value from test_table1";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2_1()
        {
            var result = Sql
                .Select<TestTable1>(t => new { t.Id, t.MyValue, FooBar2 = nameof(t.FooBar) })
                .ToString();
            var expected = "select id, my_value, foo_bar as foo_bar2 from test_table1";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2_2()
        {
            var result = Sql
                .Select<TestTable1>(t => new { FooBar = Sql.Fun.String.Md5(nameof(t.FooBar)) })
                .ToString();
            var expected = "select md5(foo_bar) as foo_bar from test_table1";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2_3()
        {
            var result = Sql
                .Select<TestTable1>(t => new { 
                    one = Sql.Value(1),
                    two = Sql.Value(2),
                    str1 = Sql.String("strvalue1"),
                    str2 = Sql.String("strvalue2")
                })
                .ToString();
            var expected = "select 1 as one, 2 as two, 'strvalue1' as str1, 'strvalue2' as str2 from test_table1";
            Assert.Equal(expected, result);
        }
    }
}
