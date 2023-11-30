# docker build -t norm_benchmarks .
# docker run --rm -it --name norm_benchmarks norm_benchmarks:latest

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS sdk
RUN git clone https://github.com/vb-consulting/Norm.net.git

WORKDIR "/Norm.net/Benchmarks"
RUN dotnet restore Benchmarks.csproj
RUN dotnet build Benchmarks.csproj -c Release

RUN apt-get update && apt-get install -y wget gnupg
RUN sh -c 'echo "deb http://apt.postgresql.org/pub/repos/apt bullseye-pgdg main" > /etc/apt/sources.list.d/pgdg.list'
RUN wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | apt-key add -
RUN apt-get update && apt-get install -y postgresql

RUN echo '\
local   all             postgres                                trust\n\
local   all             all                                     trust\n\
host    all             all             127.0.0.1/32            trust\n\
host    all             all             ::1/128                 trust\n\
local   replication     all                                     trust\n\
host    replication     all             127.0.0.1/32            trust\n\
host    replication     all             ::1/128                 trust'\
> /etc/postgresql/15/main/pg_hba.conf

RUN mkdir /results/

ENTRYPOINT service postgresql start && \
/Norm.net/Benchmarks/bin/Release/net7.0/Benchmarks && \ 
cp /Norm.net/Benchmarks/BenchmarkDotNet.Artifacts/results/Benchmarks6.Benchmarks-report-github.md /results/round1.md && \ 
/Norm.net/Benchmarks/bin/Release/net7.0/Benchmarks && \ 
cp /Norm.net/Benchmarks/BenchmarkDotNet.Artifacts/results/Benchmarks6.Benchmarks-report-github.md /results/round2.md && \
/Norm.net/Benchmarks/bin/Release/net7.0/Benchmarks && \ 
cp /Norm.net/Benchmarks/BenchmarkDotNet.Artifacts/results/Benchmarks6.Benchmarks-report-github.md /results/round3.md && \
cd /results/ && \
/bin/bash
