using Norm;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark()]
    public void Norm_Named_Tuples_ReaderCallback()
    {
        foreach (var i in connection
            .WithReaderCallback(o => o.Ordinal switch
            {
                0 => o.Reader.GetInt32(o.Ordinal),
                _ => null
            })
            .Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query))
        {
            var c = i;
        }
    }
}
