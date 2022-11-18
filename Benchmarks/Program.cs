using BenchmarkDotNet.Running;
using Benchmarks6;

Console.WriteLine("Norm version: {0}", typeof(Norm.Norm).Assembly.GetName().Version);

if (args.Contains("e") || 
    args.Contains("-e") || 
    args.Contains("--e") || 
    args.Contains("enum") || 
    args.Contains("--enum"))
{
    BenchmarkRunner.Run<EnumBenchmarks>(args: args);
}
else
{
    BenchmarkRunner.Run<Benchmarks>(args: args);
}


