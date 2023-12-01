using Norm;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark()]
    public void Norm_Tuples()
    {
        foreach (var i in connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query))
        {
            var c = i;
        }
    }
}
