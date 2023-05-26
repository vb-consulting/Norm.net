using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using FluentAssertions;
using Norm.Mapper;
using Xunit.Abstractions;

namespace TestProject1;

public class NameParserTests
{
    private readonly ITestOutputHelper output;

    public NameParserTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    //[MemoryDiagnoser]
    //public class PerfBenchmarks
    //{
    //    [Benchmark]
    //    public void OldParse()
    //    {
    //        var result = NameParser.OldParse("The_quick_brown_fox_jumps_over_the_lazy_dog_The_quick_brown_fox_jumps_over_the_lazy_dog");
    //    }

    //    [Benchmark]
    //    public void Parse()
    //    {
    //        var result = "The_quick_brown_fox_jumps_over_the_lazy_dog_The_quick_brown_fox_jumps_over_the_lazy_dog";
    //        NameParser.Parse(ref result);
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

    [Fact]
    public void Parse_Name_Tests()
    {
        var result = "@The_quick_brown_fox_jumps_over_the_lazy_dog_@The_quick_brown_fox_jumps_over_the_lazy_dog@";
        NameParser.Parse(ref result);
        result.Should().Be("thequickbrownfoxjumpsoverthelazydogthequickbrownfoxjumpsoverthelazydog");
    }
}