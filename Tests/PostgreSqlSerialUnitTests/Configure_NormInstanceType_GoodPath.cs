using System.Data.Common;

namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    private class NormExtension : Norm.Norm
    {
        public static Action<NormOptions>? ApplyOptionsCallback = null;

        public NormExtension(DbConnection connection) : base(connection)
        {
        }

        protected override void ApplyOptions(DbCommand cmd)
        {
            ApplyOptionsCallback?.Invoke(NormOptions.Value);
        }
    }

    [Fact]
    public void Configure_NormInstanceType_GoodPath_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        NormOptions.Configure(options =>
        {
            options.NormInstanceType = typeof(NormExtension);
        });

        var applyOptionsCallbackCalled = false;

        NormExtension.ApplyOptionsCallback = _ =>
        {
            applyOptionsCallbackCalled = true;
        };

        Assert.False(applyOptionsCallbackCalled);

        connection.Execute("select 1");

        Assert.True(applyOptionsCallbackCalled);

        NormExtension.ApplyOptionsCallback = null;
    }

    public class NormCustomOptions : NormOptions
    {
        public string? CustomSettings { get; set; }
    }

    [Fact]
    public void Configure_NormInstanceType_GoodPath_CutomOption_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        NormOptions.Configure<NormCustomOptions>(options =>
        {
            options.CustomSettings = "called";
            options.NormInstanceType = typeof(NormExtension);
        });

        var applyOptionsCallbackCalled = false;

        NormExtension.ApplyOptionsCallback = options =>
        {
            if ((options as NormCustomOptions).CustomSettings == "called")
            {
                applyOptionsCallbackCalled = true;
            }
        };

        Assert.False(applyOptionsCallbackCalled);

        connection.Execute("select 1");

        Assert.True(applyOptionsCallbackCalled);

        NormExtension.ApplyOptionsCallback = null;
    }
}