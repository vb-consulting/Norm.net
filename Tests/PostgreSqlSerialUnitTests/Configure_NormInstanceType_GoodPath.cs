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
    public class NormCustomOptions : NormOptions
    {
        public static Action? OnConfiguredCallback = null;

        public string? CustomSettings { get; set; }
        protected override Type NormInstanceType { get; set; } = typeof(NormExtension);

        protected override void OnConfigured()
        {
            OnConfiguredCallback?.Invoke();
        }
    }

    [Fact]
    public void Configure_NormInstanceType_GoodPath_CustomOptions_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });
        using var connection = new NpgsqlConnection(_DatabaseFixture.ConnectionString);

        var applyOptionsCallbackCalled = false;
        var onConfiguredCallbackCalled = false;

        NormExtension.ApplyOptionsCallback = options =>
        {
            if ((options as NormCustomOptions)?.CustomSettings == "called")
            {
                applyOptionsCallbackCalled = true;
            }
        };
        NormCustomOptions.OnConfiguredCallback = () =>
        {
            onConfiguredCallbackCalled = true;
        };


        Assert.False(onConfiguredCallbackCalled);
        NormOptions.Configure<NormCustomOptions>(options =>
        {
            options.CustomSettings = "called";
        });
        Assert.True(onConfiguredCallbackCalled);

        Assert.False(applyOptionsCallbackCalled);
        connection.Execute("select 1");
        Assert.True(applyOptionsCallbackCalled);

        NormExtension.ApplyOptionsCallback = null;
        NormCustomOptions.OnConfiguredCallback = null;
    }
}