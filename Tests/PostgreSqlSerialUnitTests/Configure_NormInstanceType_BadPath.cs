
namespace PostgreSqlSerialUnitTests;

public partial class PostgreSqlSerialUnitTest
{
    public class NormCustomBadPathOptions : NormOptions
    {
        protected override Type NormInstanceType { get; set; } = typeof(PostgreSqlSerialUnitTest);
    }

    [Fact]
    public void Configure_NormInstanceType_BadPath_Test()
    {
        // reset to default
        NormOptions.Configure(o => { });

        Assert.Throws<ArgumentException>(() =>
        {
            NormOptions.Configure<NormCustomBadPathOptions>(options =>
            {
            });
        });
    }
}