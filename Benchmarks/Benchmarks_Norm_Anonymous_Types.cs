using Norm;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark()]
    public void Norm_Anonymous_Types()
    {
        foreach (var i in connection.Read(new
        {
            id1 = default(int),
            foo1 = default(string),
            bar1 = default(string),
            datetime1 = default(DateTime),
            id2 = default(int),
            foo2 = default(string),
            bar2 = default(string),
            datetime2 = default(DateTime),
            longFooBar = default(string),
            isFooBar = default(bool),
        },
        query))
        {
            var c = i;
        }
    }
}
