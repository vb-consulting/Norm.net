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
            var expected = "select\r\n    id,\r\n    my_value,\r\n    foo_bar\r\nfrom\r\n    test_table1\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2()
        {
            var result = Sql
                .Select<TestTable1>(t => new { t.Id, t.MyValue })
                .ToString();
            var expected = "select\r\n    id,\r\n    my_value\r\nfrom\r\n    test_table1\r\n";
            Assert.Equal(expected, result);
        }
    }
}
