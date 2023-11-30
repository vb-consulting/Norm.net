# docker build -t norm_benchmarks .
# docker run --rm -it --network=network_name -v /home/user/norm:/Norm.net/Benchmarks/BenchmarkDotNet.Artifacts/results/ --entrypoint /bin/bash --name norm_benchmarks norm_benchmarks:latest
# ./Benchmarks -c -c Server=localhost;Database=postgres;Port=5432;User Id=postgres;Password=postgres;

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS sdk
RUN git clone https://github.com/vb-consulting/Norm.net.git

WORKDIR "/Norm.net/Benchmarks"
RUN dotnet restore Benchmarks.csproj
RUN dotnet build Benchmarks.csproj -c Release

RUN mkdir /Norm.net/Benchmarks/BenchmarkDotNet.Artifacts/
RUN mkdir /Norm.net/Benchmarks/BenchmarkDotNet.Artifacts/results/
WORKDIR "/Norm.net/Benchmarks/bin/Release/net8.0/"