using System.Threading;
using System;

namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_NpgsqlEnableSqlRewriting_Test()
    {
        // reset to default
        //NormOptions.Configure(o => { });
        //NormOptions.Configure(options => options.NpgsqlEnableSqlRewriting = false);

        //using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

/*
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
Assert.Null(@null);

var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
Assert.Null(@null);
*/


//System.NotSupportedException : Named parameters are not supported when Npgsql.EnableSqlRewriting is disabledSystem.NotSupportedException : Named parameters are not supported when Npgsql.EnableSqlRewriting is disabled

// System.TypeInitializationException : The type initializer for '<Module>' threw an exception.
// ---- Npgsql.PostgresException : 42601: cannot insert multiple commands into a prepared statement



    //var (s, i, b, d, @null) = connection
    //        .WithParameters(new
    //        {
    //            s = "str", i = 999, b = true, d = new DateTime(1977, 5, 19), @null = (string)null
    //        })
    //        .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null; select @s, @i, @b, @d, @null")
    //        .Single();

    //    Assert.Equal("str", s);
    //    Assert.Equal(999, i);
    //    Assert.True(b);
    //    Assert.Equal(new DateTime(1977, 5, 19), d);
    //    Assert.Null(@null);
    }
}