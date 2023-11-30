# docker build -t norm_benchmarks .
#
# home/vbilopav/norm is dir with results, must exists
# network is network with postgres server
#
# docker run --rm -it --network=network -v /home/vbilopav/norm/:/results/ --name norm_benchmarks norm_benchmarks:latest
#

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS sdk
RUN git clone https://github.com/vb-consulting/Norm.net.git

#
# Update this connections string as you see fit
# 
RUN echo '\
namespace NormBenchmarks;\n\
\n\
public class Connection\n\
{\n\
    public static string String = "Server=localhost;Database=postgres;Port=5432;User Id=postgres;Password=postgres;";\n\
}'\
> /Norm.net/Benchmarks/Connection.cs

WORKDIR "/Norm.net/Benchmarks"
RUN dotnet restore Benchmarks.csproj
RUN dotnet build Benchmarks.csproj -c Release

RUN mkdir /Norm.net/Benchmarks/BenchmarkDotNet.Artifacts/
RUN mkdir /Norm.net/Benchmarks/BenchmarkDotNet.Artifacts/results/
WORKDIR "/Norm.net/Benchmarks/bin/Release/net8.0/"

RUN mkdir /results/

ENTRYPOINT ./Benchmarks && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round1.md && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round1.html && \
./Benchmarks && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round2.md && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round2.html && \
./Benchmarks && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round3.md && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round3.html && \
./Benchmarks && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round4.md && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round4.html && \
./Benchmarks && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round5.md && \
cp ./BenchmarkDotNet.Artifacts/results/NormBenchmarks.Benchmarks-report-github.md /results/round5.html
