using Norm;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark()]
    public void Norm_NameValue_Array()
    {
        foreach (var i in connection.Read(query))
        {
            var c = i;
        }
    }
}
