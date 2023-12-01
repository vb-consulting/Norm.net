using Dapper;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark(Baseline = true)]
    public void Dapper()
    {
        foreach (var i in connection.Query<PocoClass>(query))
        {
            var c = i;
        }
    }
}
