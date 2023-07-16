# docker build -t norm_benchmarks .
# docker run --rm -it --name norm_benchmarks norm_benchmarks:latest

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS sdk
RUN git clone -b 5.3.7 https://github.com/vb-consulting/Norm.net.git

WORKDIR "/Norm.net/Benchmarks"
RUN dotnet restore Benchmarks.csproj
RUN dotnet build Benchmarks.csproj -c Release

RUN apt-get update && apt-get install -y wget gnupg
RUN sh -c 'echo "deb http://apt.postgresql.org/pub/repos/apt bullseye-pgdg main" > /etc/apt/sources.list.d/pgdg.list'
RUN wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | apt-key add -
RUN apt-get update && apt-get install -y postgresql

RUN echo '\
{\n\
  "db": {\n\
    "Default": "Server=localhost;Database=postgres;Port=5432;User Id=postgres;Password=postgres;",\n\
    "TestDatabase": "norm_postgresql_unit_tests"\n\
  }\n\
}'\
> /Norm.net/Benchmarks/bin/Release/net7.0/testsettings.local.json

ENTRYPOINT service postgresql start && \ 
/Norm.net/Benchmarks/bin/Release/net7.0/Benchmarks -c "Server=localhost;Database=postgres;Port=5432;User Id=postgres;Password=postgres;" && \ 
/bin/bash
