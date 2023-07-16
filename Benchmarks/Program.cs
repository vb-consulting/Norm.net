using BenchmarkDotNet.Running;
using Benchmarks6;

Console.WriteLine("Norm version: {0}", typeof(Norm.Norm).Assembly.GetName().Version);

if (args.Contains("-c"))
{
    var list = args.ToList();
    var index = list.IndexOf("-c");
    if (index != -1 && list.Count > index + 1)
    {
        Connection.ConnectionString = list[index + 1];
    }
}

Console.WriteLine("Using {0}", Connection.ConnectionString);

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

public static class Connection
{
    public static string? ConnectionString = null;
}

