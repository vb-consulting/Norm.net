#pragma warning disable CS8604 // Possible null reference argument.
using System.Reflection;

namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{

    public class TestMapPrivateProps
    {
        public int PublicInt { get; set; }
        private int PrivateInt { get; set; }
        public int PrivateSetInt { get; private set; }
        protected int ProtectedInt { get; set; }
        public int ProtectedSetInt { get; protected set; }
        public int MissingSetInt { get; }
    }

    [Fact]
    public void Configure_MapPrivate_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        var statement = @"select 
            1 as public_int, 
            2 as private_int, 
            3 as private_set_int,
            4 as protected_int,
            5 as protected_set_int,
            6 as missing_set_int
";
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        var result1 = connection.Read<TestMapPrivateProps>(statement).Single();

        Assert.Equal(1, result1.PublicInt);
        Assert.Equal(0, GetPrivateProp(result1, "PrivateInt"));
        Assert.Equal(0, result1?.PrivateSetInt);
        Assert.Equal(0, GetPrivateProp(result1, "ProtectedInt"));
        Assert.Equal(0, result1?.ProtectedSetInt);
        Assert.Equal(0, result1?.MissingSetInt);

        NormOptions.Configure(options => options.MapPrivateSetters = true);

        var result2 = connection.Read<TestMapPrivateProps>(statement).Single();

        Assert.Equal(1, result2.PublicInt);
        Assert.Equal(0, GetPrivateProp(result2, "PrivateInt"));
        Assert.Equal(3, result2?.PrivateSetInt);
        Assert.Equal(0, GetPrivateProp(result2, "ProtectedInt"));
        Assert.Equal(5, result2?.ProtectedSetInt);
        Assert.Equal(0, result2?.MissingSetInt);

        static int? GetPrivateProp(TestMapPrivateProps inst, string name)
        {
            var prop =
                typeof(TestMapPrivateProps)
                .GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);
            var getter = prop?.GetGetMethod(nonPublic: true);
            return (int?)getter?.Invoke(inst, null);
        }
    }
}