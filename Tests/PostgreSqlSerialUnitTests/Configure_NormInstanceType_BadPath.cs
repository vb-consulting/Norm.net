
namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    [Fact]
    public void Configure_NormInstanceType_BadPath_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        Assert.Throws<ArgumentException>(() =>
        {
            NormOptions.Configure(options =>
            {
                options.NormInstanceType = typeof(PostgreSqlSerialUnitTest);
            });
        });
    }
}