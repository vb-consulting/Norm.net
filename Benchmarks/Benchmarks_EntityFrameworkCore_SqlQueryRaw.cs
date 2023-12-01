using Microsoft.EntityFrameworkCore;
namespace NormBenchmarks;
public partial class Benchmarks
{
    [BenchmarkDotNet.Attributes.Benchmark()]
    public void EntityFrameworkCore_SqlQueryRaw()
    {
        foreach (var i in dbcontext.Database.SqlQueryRaw<PocoClass>(query))
        {
            var c = i;
        }
    }
}
