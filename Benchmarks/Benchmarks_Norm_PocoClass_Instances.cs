using Norm;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark()]
    public void Norm_PocoClass_Instances()
    {
        foreach (var i in connection.Read<PocoClass>(query))
        {
            var c = i;
        }
    }
}
