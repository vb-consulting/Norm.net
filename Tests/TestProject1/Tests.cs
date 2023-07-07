using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using FluentAssertions;
using Norm.Mapper;
using Xunit.Abstractions;

namespace TestProject1;

public class Tests
{
    private readonly ITestOutputHelper output;

    public Tests(ITestOutputHelper output)
    {
        this.output = output;
    }

    //[MemoryDiagnoser]
    //public class PerfBenchmarks
    //{
        
    //    [Benchmark]
    //    public void Call_Method1()
    //    {
    //    }

    //    [Benchmark]
    //    public void Call_Method2()
    //    {
    //    }
    //}

    //[Fact]
    //public void Perf_Tests()
    //{
    //    var logger = new AccumulationLogger();

    //    var config = ManualConfig.Create(DefaultConfig.Instance)
    //        .AddLogger(logger)
    //        .WithOptions(ConfigOptions.DisableOptimizationsValidator);

    //    BenchmarkRunner.Run<PerfBenchmarks>(config);

    //    // write benchmark summary
    //    output.WriteLine(logger.GetLog());
    //}
}