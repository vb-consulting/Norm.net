using BenchmarkDotNet.Running;
using NormBenchmarks;

Console.WriteLine("Norm version: {0}", typeof(Norm.Norm).Assembly.GetName().Version);
Console.WriteLine("Dapper version: {0}", typeof(Dapper.SqlMapper).Assembly.GetName().Version);
Console.WriteLine("EntityFrameworkCore version: {0}", typeof(Microsoft.EntityFrameworkCore.DbContext).Assembly.GetName().Version);


Console.WriteLine("Using {0}", Connection.String);

BenchmarkRunner.Run<Benchmarks>();

