using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Norm;
using Npgsql;
using NpgsqlTypes;
using Xunit;

#nullable enable

namespace PostgreSqlUnitTests;

[Collection("PostgreSqlDatabase")]
public class NullableInstancesUnitTests
{
    private readonly PostgreSqlFixture fixture;

    class TestClass
    {
        public int? Foo { get; set; }
        public string? Bar { get; set; }
    }
    
    public NullableInstancesUnitTests(PostgreSqlFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Nullable_Instance_Test()
    {
        using var connection = new NpgsqlConnection(fixture.ConnectionString);
        var result = connection.Read<TestClass?>("select 1 as foo, 'bar' as bar").Single();
        Assert.NotNull(result);
        Assert.Equal(1, result?.Foo);
        Assert.Equal("bar", result?.Bar);
    }

    [Fact]
    public void Nullable_All_Nulls_Instance_Test()
    {
        using var connection = new NpgsqlConnection(fixture.ConnectionString);
        var result = connection.Read<TestClass?>("select null as foo, null as bar").Single();
        Assert.NotNull(result);
        Assert.Null(result?.Foo);
        Assert.Null(result?.Bar);
    }
}
