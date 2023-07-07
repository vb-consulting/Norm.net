# PERFOMANCE BENCHMARKS

## 1. Standard set of tests, serialization to class instances and tuples with various fields types

### Test Query and Test Class

10 fields wide query on PostgreSQL 13 with parametrized number of records.

- Query:

```sql
    select 
        i as id1, 
        'foo' || i::text as foo1, 
        'bar' || i::text as bar1, 
        ('2000-01-01'::date) + (i::text || ' days')::interval as datetime1, 
        i+1 as id2, 
        'foo' || (i+1)::text as foo2, 
        'bar' || (i+1)::text as bar2, 
        ('2000-01-01'::date) + ((i+1)::text || ' days')::interval as datetime2,
        'long_foo_bar_' || (i+2)::text as longfoobar, 
        (i % 2)::boolean as isfoobar
    from 
        generate_series(1, {records}) as i
```

- Test Class

```csharp
    public class PocoClass
    {
        public int Id1 { get; set; }
        public string? Foo1 { get; set; }
        public string? Bar1 { get; set; }
        public DateTime DateTime1 { get; set; }
        public int Id2 { get; set; }
        public string? Foo2 { get; set; }
        public string? Bar2 { get; set; }
        public DateTime DateTime2 { get; set; }
        public string? LongFooBar { get; set; }
        public bool IsFooBar { get; set; }
    }
```

### Test Methods

#### `Dapper`

Normal Dapper serialization, used as baseline for ratio.

```csharp
foreach (var i in connection.Query<PocoClass>(query))
{
}
```

#### `Dapper_Buffered_False`

Unbuffered Dapper serialization.

```csharp
foreach (var i in connection.Query<PocoClass>(query, buffered: false))
{
}
```

#### `Norm_NameValue_Array`

Non-generic Read method that yields name and value tuple array.

```csharp
foreach (var i in connection.Read(query))
{
}
```

#### `Norm_PocoClass_Instances`

Standard Norm serialization to class instances. Equivalent to unbuffered dapper query.

```csharp
foreach (var i in connection.Read<PocoClass>(query))
{
}
```

#### `Norm_Tuples`

Norm serialization to unnamed tuples.

```csharp
foreach (var i in connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query))
{
}
```

#### `Norm_Anonymous_Types`

Norm serialization to anonymous type instances.

```csharp
foreach (var i in connection.Read(new
{
    id1 = default(int),
    foo1 = default(string),
    bar1 = default(string),
    datetime1 = default(DateTime),
    id2 = default(int),
    foo2 = default(string),
    bar2 = default(string),
    datetime2 = default(DateTime),
    longFooBar = default(string),
    isFooBar = default(bool),
}, 
query))
{
}
```

#### `Norm_Named_Tuples`

Norm serialization to named tuples.

```csharp
foreach (var i in connection.Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query))
{
}
```

#### `Norm_PocoClass_Instances_ReaderCallback`

Standard Norm serialization to class instances (equivalent to unbuffered dapper query), but, containing a reader callback lambda function which only uses a reader to read the first field.

```csharp
foreach (var i in connection.Read<PocoClass>(query, o => o.Ordinal switch
{
    0 => o.Reader.GetInt32(o.Ordinal),
    _ => null
}))
{
}
```

#### `Norm_Tuples_ReaderCallback`

Standard norm serialization to unnamed tuples, but, containing a reader callback lambda function which only uses a reader to read the first field.

```csharp
foreach (var i in connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query, o => o.Ordinal switch
{
    0 => o.Reader.GetInt32(o.Ordinal),
    _ => null
}))
{
}
```

#### `Norm_Named_Tuples_ReaderCallback`

Stanard norm serialization to named tuples, but, containing a reader callback lambda function which only uses a reader to read the first field.

```csharp
foreach (var i in connection.Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query, o => o.Ordinal switch
{
    0 => o.Reader.GetInt32(o.Ordinal),
    _ => null
}))
{
}
```

#### `Command_Reader`

Raw data reader approach.

```csharp
using var command = new NpgsqlCommand(query, connection);
using var reader = command.ExecuteReader();
while (reader.Read())
{
    var instance = new PocoClass
    {
        Id1 = reader.GetInt32(0),
        Foo1 = reader.GetString(1),
        Bar1 = reader.GetString(2),
        DateTime1 = reader.GetDateTime(3),
        Id2 = reader.GetInt32(4),
        Foo2 = reader.GetString(5),
        Bar2 = reader.GetString(6),
        DateTime2 = reader.GetDateTime(7),
        LongFooBar = reader.GetString(8),
        IsFooBar = reader.GetBoolean(9),
    };
}
```

#### Round 1, 2023-07-05, Dapper  2.0.123, Norm 5.3.7



#### Round 2, 2023-05-26, Dapper  2.0.123, Norm 5.3.4

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1702/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK=7.0.203
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2


```
|                                  Method | Records |         Mean |       Error |      StdDev |       Median | Ratio | RatioSD |      Gen0 |      Gen1 |      Gen2 |   Allocated | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|------------:|------------:|-------------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
|                                  **Dapper** |      **10** |     **281.2 μs** |     **5.59 μs** |    **13.07 μs** |     **281.1 μs** |  **1.00** |    **0.00** |    **0.4883** |         **-** |         **-** |     **5.83 KB** |        **1.00** |
|                   Dapper_Buffered_False |      10 |     281.8 μs |     5.45 μs |     8.64 μs |     284.3 μs |  0.99 |    0.05 |    0.4883 |         - |         - |     5.47 KB |        0.94 |
|                    Norm_NameValue_Array |      10 |     248.2 μs |     4.96 μs |     6.62 μs |     247.8 μs |  0.88 |    0.03 |    0.4883 |         - |         - |     8.26 KB |        1.42 |
|                Norm_PocoClass_Instances |      10 |     264.0 μs |     4.33 μs |     3.84 μs |     263.7 μs |  0.93 |    0.03 |    0.9766 |         - |         - |    10.27 KB |        1.76 |
|                             Norm_Tuples |      10 |     240.5 μs |     2.68 μs |     2.24 μs |     241.2 μs |  0.84 |    0.02 |    0.2441 |         - |         - |     3.69 KB |        0.63 |
|                       Norm_Named_Tuples |      10 |     236.7 μs |     2.80 μs |     2.48 μs |     235.4 μs |  0.83 |    0.03 |    0.9766 |         - |         - |     9.98 KB |        1.71 |
|                    Norm_Anonymous_Types |      10 |     244.0 μs |     4.75 μs |     6.50 μs |     245.1 μs |  0.86 |    0.04 |    0.9766 |         - |         - |    11.45 KB |        1.96 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |     262.3 μs |     5.24 μs |     8.76 μs |     261.1 μs |  0.93 |    0.05 |    0.9766 |         - |         - |    10.99 KB |        1.88 |
|              Norm_Tuples_ReaderCallback |      10 |     274.3 μs |    10.73 μs |    30.96 μs |     269.5 μs |  0.95 |    0.12 |         - |         - |         - |     3.92 KB |        0.67 |
|        Norm_Named_Tuples_ReaderCallback |      10 |     336.0 μs |    10.77 μs |    31.25 μs |     328.5 μs |  1.22 |    0.11 |    0.9766 |         - |         - |     9.98 KB |        1.71 |
|                          Command_Reader |      10 |     294.1 μs |     5.80 μs |    14.33 μs |     291.6 μs |  1.05 |    0.07 |    0.4883 |         - |         - |      4.7 KB |        0.81 |
|                                         |         |              |             |             |              |       |         |           |           |           |             |             |
|                                  **Dapper** |    **1000** |   **2,294.4 μs** |    **45.82 μs** |    **82.63 μs** |   **2,266.5 μs** |  **1.00** |    **0.00** |   **42.9688** |   **15.6250** |         **-** |   **428.89 KB** |        **1.00** |
|                   Dapper_Buffered_False |    1000 |   2,165.5 μs |    52.72 μs |   154.61 μs |   2,148.6 μs |  1.00 |    0.06 |   42.9688 |         - |         - |   412.64 KB |        0.96 |
|                    Norm_NameValue_Array |    1000 |   2,082.9 μs |    41.53 μs |    92.89 μs |   2,078.6 μs |  0.90 |    0.05 |   74.2188 |         - |         - |   686.15 KB |        1.60 |
|                Norm_PocoClass_Instances |    1000 |   2,416.3 μs |    47.83 μs |    65.47 μs |   2,418.4 μs |  1.04 |    0.05 |   62.5000 |         - |         - |   595.37 KB |        1.39 |
|                             Norm_Tuples |    1000 |   1,969.8 μs |    38.76 μs |    85.08 μs |   1,959.6 μs |  0.87 |    0.05 |   19.5313 |         - |         - |   209.77 KB |        0.49 |
|                       Norm_Named_Tuples |    1000 |   2,362.0 μs |    46.63 μs |    75.30 μs |   2,363.6 μs |  1.03 |    0.04 |   89.8438 |         - |         - |   850.29 KB |        1.98 |
|                    Norm_Anonymous_Types |    1000 |   2,566.2 μs |    45.04 μs |    50.07 μs |   2,582.6 μs |  1.10 |    0.05 |   85.9375 |         - |         - |   797.62 KB |        1.86 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |   2,426.0 μs |    44.60 μs |    39.53 μs |   2,430.6 μs |  1.04 |    0.05 |   70.3125 |         - |         - |   673.44 KB |        1.57 |
|              Norm_Tuples_ReaderCallback |    1000 |   2,006.6 μs |    40.12 μs |    97.65 μs |   1,985.6 μs |  0.87 |    0.06 |   23.4375 |         - |         - |   233.21 KB |        0.54 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |   2,355.7 μs |    45.04 μs |    60.13 μs |   2,369.7 μs |  1.02 |    0.05 |   89.8438 |         - |         - |   850.29 KB |        1.98 |
|                          Command_Reader |    1000 |   1,885.4 μs |    33.09 μs |    38.11 μs |   1,889.1 μs |  0.81 |    0.03 |   31.2500 |         - |         - |   295.85 KB |        0.69 |
|                                         |         |              |             |             |              |       |         |           |           |           |             |             |
|                                  **Dapper** |   **10000** |  **22,613.6 μs** |   **449.43 μs** |   **977.02 μs** |  **22,627.5 μs** |  **1.00** |    **0.00** |  **545.4545** |  **454.5455** |  **181.8182** |  **4395.84 KB** |        **1.00** |
|                   Dapper_Buffered_False |   10000 |  17,675.4 μs |   346.42 μs |   642.11 μs |  17,677.7 μs |  0.79 |    0.05 |  437.5000 |         - |         - |  4139.25 KB |        0.94 |
|                    Norm_NameValue_Array |   10000 |  17,319.1 μs |   303.53 μs |   253.46 μs |  17,265.5 μs |  0.77 |    0.03 |  718.7500 |         - |         - |  6873.83 KB |        1.56 |
|                Norm_PocoClass_Instances |   10000 |  24,150.7 μs | 2,092.38 μs | 6,037.00 μs |  25,684.6 μs |  1.08 |    0.27 |  625.0000 |         - |         - |  5939.55 KB |        1.35 |
|                             Norm_Tuples |   10000 |  16,860.8 μs |   322.88 μs |   502.68 μs |  16,883.5 μs |  0.75 |    0.04 |  218.7500 |         - |         - |   2108.3 KB |        0.48 |
|                       Norm_Named_Tuples |   10000 |  24,729.2 μs | 1,989.67 μs | 5,676.65 μs |  25,860.0 μs |  1.07 |    0.26 |  906.2500 |         - |         - |  8514.57 KB |        1.94 |
|                    Norm_Anonymous_Types |   10000 |  20,096.7 μs |   396.94 μs |   569.28 μs |  20,037.5 μs |  0.89 |    0.04 |  843.7500 |         - |         - |  7969.71 KB |        1.81 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |  20,443.9 μs | 1,533.15 μs | 4,496.45 μs |  18,877.6 μs |  0.94 |    0.21 |  718.7500 |         - |         - |  6720.78 KB |        1.53 |
|              Norm_Tuples_ReaderCallback |   10000 |  15,264.5 μs |   290.89 μs |   311.25 μs |  15,233.0 μs |  0.68 |    0.03 |  250.0000 |         - |         - |  2342.67 KB |        0.53 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |  20,431.1 μs | 3,335.76 μs | 9,835.56 μs |  14,353.0 μs |  0.95 |    0.46 |  857.1429 |         - |         - |  8514.62 KB |        1.94 |
|                          Command_Reader |   10000 |  15,227.2 μs |   254.25 μs |   356.43 μs |  15,191.4 μs |  0.68 |    0.03 |  312.5000 |         - |         - |  2967.76 KB |        0.68 |
|                                         |         |              |             |             |              |       |         |           |           |           |             |             |
|                                  **Dapper** |  **100000** | **210,351.7 μs** | **4,111.68 μs** | **6,026.84 μs** | **209,806.1 μs** |  **1.00** |    **0.00** | **5666.6667** | **5333.3333** | **1333.3333** | **44160.62 KB** |        **1.00** |
|                   Dapper_Buffered_False |  100000 | 161,574.1 μs | 3,179.26 μs | 2,654.82 μs | 162,750.9 μs |  0.77 |    0.03 | 4333.3333 |         - |         - | 42108.34 KB |        0.95 |
|                    Norm_NameValue_Array |  100000 | 161,674.0 μs | 3,193.05 μs | 5,422.03 μs | 162,517.1 μs |  0.77 |    0.03 | 7333.3333 |         - |         - | 69452.34 KB |        1.57 |
|                Norm_PocoClass_Instances |  100000 | 169,274.3 μs | 3,231.58 μs | 3,968.67 μs | 169,347.9 μs |  0.80 |    0.03 | 6333.3333 |         - |         - | 60080.73 KB |        1.36 |
|                             Norm_Tuples |  100000 | 160,031.7 μs | 3,195.65 μs | 4,783.10 μs | 159,523.9 μs |  0.76 |    0.04 | 2250.0000 |         - |         - |  21796.2 KB |        0.49 |
|                       Norm_Named_Tuples |  100000 | 167,959.7 μs | 3,344.13 μs | 6,906.20 μs | 166,906.6 μs |  0.79 |    0.05 | 9333.3333 |         - |         - | 85858.67 KB |        1.94 |
|                    Norm_Anonymous_Types |  100000 | 181,734.2 μs | 3,591.05 μs | 4,410.13 μs | 181,085.0 μs |  0.86 |    0.04 | 8666.6667 |         - |         - | 80391.95 KB |        1.82 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 | 172,462.8 μs | 2,429.04 μs | 2,153.28 μs | 172,543.7 μs |  0.82 |    0.03 | 7333.3333 |         - |         - | 67893.17 KB |        1.54 |
|              Norm_Tuples_ReaderCallback |  100000 | 160,037.3 μs | 3,136.66 μs | 5,240.66 μs | 160,082.0 μs |  0.76 |    0.03 | 2500.0000 |         - |         - | 24139.95 KB |        0.55 |
|        Norm_Named_Tuples_ReaderCallback |  100000 | 172,712.6 μs | 3,395.41 μs | 6,293.61 μs | 172,227.7 μs |  0.83 |    0.04 | 9333.3333 |         - |         - | 85858.67 KB |        1.94 |
|                          Command_Reader |  100000 | 158,053.3 μs | 3,129.17 μs | 5,562.10 μs | 157,887.8 μs |  0.75 |    0.03 | 3250.0000 |         - |         - | 30389.89 KB |        0.69 |

#### Round 3, 2023-06-22, Dapper  2.0.123, Norm 5.3.4

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1848/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK=7.0.304
  [Host]     : .NET 7.0.7 (7.0.723.27404), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.7 (7.0.723.27404), X64 RyuJIT AVX2


```
|                                  Method | Records |         Mean |       Error |      StdDev |       Median | Ratio | RatioSD |      Gen0 |      Gen1 |      Gen2 |   Allocated | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|------------:|------------:|-------------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
|                                  **Dapper** |      **10** |     **260.9 μs** |     **4.78 μs** |     **3.99 μs** |     **259.2 μs** |  **1.00** |    **0.00** |    **0.4883** |         **-** |         **-** |     **5.83 KB** |        **1.00** |
|                   Dapper_Buffered_False |      10 |     296.6 μs |     5.61 μs |     5.25 μs |     296.1 μs |  1.14 |    0.02 |    0.4883 |         - |         - |     5.47 KB |        0.94 |
|                    Norm_NameValue_Array |      10 |     259.8 μs |     5.19 μs |     9.62 μs |     260.7 μs |  0.98 |    0.05 |    0.4883 |         - |         - |     8.26 KB |        1.42 |
|                Norm_PocoClass_Instances |      10 |     249.4 μs |     4.96 μs |     6.45 μs |     248.8 μs |  0.96 |    0.03 |    0.9766 |         - |         - |    10.27 KB |        1.76 |
|                             Norm_Tuples |      10 |     248.3 μs |     4.94 μs |     9.75 μs |     246.9 μs |  0.97 |    0.03 |         - |         - |         - |     3.69 KB |        0.63 |
|                       Norm_Named_Tuples |      10 |     249.1 μs |     4.79 μs |    10.11 μs |     247.6 μs |  0.96 |    0.04 |    0.9766 |         - |         - |     9.98 KB |        1.71 |
|                    Norm_Anonymous_Types |      10 |     265.1 μs |     5.25 μs |     8.78 μs |     265.1 μs |  1.01 |    0.03 |    0.9766 |         - |         - |    11.45 KB |        1.96 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |     268.0 μs |     4.84 μs |     5.76 μs |     267.5 μs |  1.03 |    0.03 |    0.9766 |         - |         - |    10.99 KB |        1.88 |
|              Norm_Tuples_ReaderCallback |      10 |     248.4 μs |     4.79 μs |     4.48 μs |     249.2 μs |  0.95 |    0.02 |         - |         - |         - |     3.92 KB |        0.67 |
|        Norm_Named_Tuples_ReaderCallback |      10 |     259.7 μs |     5.02 μs |     6.17 μs |     260.6 μs |  1.00 |    0.02 |    0.9766 |         - |         - |     9.98 KB |        1.71 |
|                          Command_Reader |      10 |     225.3 μs |     4.34 μs |     9.35 μs |     224.9 μs |  0.88 |    0.05 |    0.4883 |         - |         - |      4.7 KB |        0.81 |
|                                         |         |              |             |             |              |       |         |           |           |           |             |             |
|                                  **Dapper** |    **1000** |   **1,901.9 μs** |    **34.65 μs** |    **59.76 μs** |   **1,903.2 μs** |  **1.00** |    **0.00** |   **42.9688** |   **15.6250** |         **-** |   **428.89 KB** |        **1.00** |
|                   Dapper_Buffered_False |    1000 |   1,858.1 μs |    34.35 μs |    32.13 μs |   1,863.2 μs |  0.98 |    0.04 |   42.9688 |         - |         - |   412.63 KB |        0.96 |
|                    Norm_NameValue_Array |    1000 |   2,169.7 μs |    25.05 μs |    19.55 μs |   2,174.5 μs |  1.14 |    0.04 |   74.2188 |         - |         - |   686.14 KB |        1.60 |
|                Norm_PocoClass_Instances |    1000 |   2,509.9 μs |    49.99 μs |    44.31 μs |   2,514.4 μs |  1.32 |    0.05 |   62.5000 |         - |         - |   595.37 KB |        1.39 |
|                             Norm_Tuples |    1000 |   2,094.0 μs |    40.59 μs |    39.87 μs |   2,088.2 μs |  1.10 |    0.05 |   19.5313 |         - |         - |   209.77 KB |        0.49 |
|                       Norm_Named_Tuples |    1000 |   2,461.6 μs |    43.58 μs |    36.39 μs |   2,468.6 μs |  1.29 |    0.05 |   89.8438 |         - |         - |   850.29 KB |        1.98 |
|                    Norm_Anonymous_Types |    1000 |   2,704.3 μs |    53.81 μs |    66.09 μs |   2,708.3 μs |  1.42 |    0.05 |   85.9375 |         - |         - |   797.62 KB |        1.86 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |   2,598.0 μs |    50.93 μs |    95.65 μs |   2,582.9 μs |  1.37 |    0.07 |   70.3125 |         - |         - |   673.44 KB |        1.57 |
|              Norm_Tuples_ReaderCallback |    1000 |   2,136.7 μs |    38.31 μs |    39.34 μs |   2,153.6 μs |  1.13 |    0.05 |   23.4375 |         - |         - |   233.21 KB |        0.54 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |   2,529.6 μs |    49.74 μs |    62.90 μs |   2,534.1 μs |  1.33 |    0.07 |   89.8438 |         - |         - |   850.29 KB |        1.98 |
|                          Command_Reader |    1000 |   2,037.2 μs |    40.66 μs |    69.04 μs |   2,017.6 μs |  1.07 |    0.05 |   31.2500 |         - |         - |   295.85 KB |        0.69 |
|                                         |         |              |             |             |              |       |         |           |           |           |             |             |
|                                  **Dapper** |   **10000** |  **23,903.8 μs** |   **465.06 μs** |   **516.92 μs** |  **24,081.4 μs** |  **1.00** |    **0.00** |  **555.5556** |  **444.4444** |  **111.1111** |  **4396.36 KB** |        **1.00** |
|                   Dapper_Buffered_False |   10000 |  18,365.3 μs |   360.62 μs |   659.42 μs |  18,321.2 μs |  0.77 |    0.04 |  437.5000 |         - |         - |  4139.25 KB |        0.94 |
|                    Norm_NameValue_Array |   10000 |  18,612.8 μs |   371.31 μs |   688.25 μs |  18,691.7 μs |  0.77 |    0.04 |  718.7500 |         - |         - |  6873.83 KB |        1.56 |
|                Norm_PocoClass_Instances |   10000 |  20,200.4 μs | 1,325.60 μs | 3,760.50 μs |  19,017.2 μs |  0.91 |    0.14 |  625.0000 |         - |         - |  5939.55 KB |        1.35 |
|                             Norm_Tuples |   10000 |  16,000.1 μs |   311.02 μs |   621.15 μs |  15,958.4 μs |  0.69 |    0.03 |  218.7500 |         - |         - |   2108.3 KB |        0.48 |
|                       Norm_Named_Tuples |   10000 |  15,378.9 μs |   485.28 μs | 1,226.37 μs |  14,950.8 μs |  0.66 |    0.06 |  875.0000 |         - |         - |   8514.6 KB |        1.94 |
|                    Norm_Anonymous_Types |   10000 |  18,598.3 μs |   371.88 μs |   689.30 μs |  18,770.8 μs |  0.77 |    0.05 |  843.7500 |         - |         - |  7969.71 KB |        1.81 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |  17,933.0 μs |   386.00 μs | 1,056.66 μs |  18,001.0 μs |  0.75 |    0.05 |  692.3077 |         - |         - |  6720.81 KB |        1.53 |
|              Norm_Tuples_ReaderCallback |   10000 |  15,592.9 μs |   288.13 μs |   255.42 μs |  15,581.4 μs |  0.65 |    0.02 |  250.0000 |         - |         - |  2342.67 KB |        0.53 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |  16,851.7 μs |   451.34 μs | 1,181.07 μs |  17,128.4 μs |  0.69 |    0.06 |  906.2500 |         - |         - |  8514.57 KB |        1.94 |
|                          Command_Reader |   10000 |  15,078.3 μs |   140.71 μs |   124.74 μs |  15,112.0 μs |  0.63 |    0.02 |  312.5000 |         - |         - |  2967.76 KB |        0.68 |
|                                         |         |              |             |             |              |       |         |           |           |           |             |             |
|                                  **Dapper** |  **100000** | **215,127.8 μs** | **4,281.88 μs** | **6,276.33 μs** | **213,279.9 μs** |  **1.00** |    **0.00** | **5666.6667** | **5333.3333** | **1333.3333** | **44160.51 KB** |        **1.00** |
|                   Dapper_Buffered_False |  100000 | 164,599.4 μs | 3,254.91 μs | 3,617.82 μs | 164,750.7 μs |  0.76 |    0.02 | 4333.3333 |         - |         - | 42108.34 KB |        0.95 |
|                    Norm_NameValue_Array |  100000 | 166,116.1 μs | 3,310.46 μs | 4,531.39 μs | 165,567.2 μs |  0.77 |    0.03 | 7500.0000 |         - |         - | 69452.25 KB |        1.57 |
|                Norm_PocoClass_Instances |  100000 | 171,953.4 μs | 3,409.34 μs | 4,186.97 μs | 170,895.9 μs |  0.80 |    0.03 | 6333.3333 |         - |         - | 60080.73 KB |        1.36 |
|                             Norm_Tuples |  100000 | 161,715.3 μs | 3,138.11 μs | 4,080.43 μs | 161,725.5 μs |  0.75 |    0.02 | 2250.0000 |         - |         - |  21796.2 KB |        0.49 |
|                       Norm_Named_Tuples |  100000 | 173,533.1 μs | 3,400.83 μs | 4,048.44 μs | 173,140.0 μs |  0.81 |    0.03 | 9333.3333 |         - |         - | 85858.67 KB |        1.94 |
|                    Norm_Anonymous_Types |  100000 | 184,877.0 μs | 3,600.76 μs | 4,286.45 μs | 185,272.2 μs |  0.86 |    0.03 | 8666.6667 |         - |         - | 80391.95 KB |        1.82 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 | 177,147.7 μs | 3,515.18 μs | 4,692.67 μs | 175,333.3 μs |  0.82 |    0.03 | 7333.3333 |         - |         - | 67893.17 KB |        1.54 |
|              Norm_Tuples_ReaderCallback |  100000 | 160,681.3 μs | 3,173.08 μs | 4,940.10 μs | 160,085.6 μs |  0.75 |    0.03 | 2500.0000 |         - |         - | 24139.95 KB |        0.55 |
|        Norm_Named_Tuples_ReaderCallback |  100000 | 173,583.4 μs | 3,080.98 μs | 3,667.69 μs | 173,864.1 μs |  0.81 |    0.02 | 9333.3333 |         - |         - | 85858.67 KB |        1.94 |
|                          Command_Reader |  100000 | 157,358.5 μs | 3,146.23 μs | 4,611.71 μs | 156,209.5 μs |  0.73 |    0.03 | 3250.0000 |         - |         - | 30389.89 KB |        0.69 |
