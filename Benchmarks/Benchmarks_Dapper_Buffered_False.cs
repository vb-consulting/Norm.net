using Dapper;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark()]
    public void Dapper_Buffered_False()
    {
        foreach (var i in connection.Query<PocoClass>(query, buffered: false))
        {
            var c = i;
        }
    }
}
