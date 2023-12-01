using Norm;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark()]
    public void Norm_PocoClass_Instances_ReaderCallback()
    {
        foreach (var i in connection
            .WithReaderCallback(o => o.Ordinal switch
            {
                0 => o.Reader.GetInt32(o.Ordinal),
                _ => null
            })
            .Read<PocoClass>(query))
        {
            var c = i;
        }
    }
}
