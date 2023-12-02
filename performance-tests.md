# PERFOMANCE BENCHMARKS

Norm version: 5.4.0.0
Dapper version: 2.0.0.0
EntityFrameworkCore version: 8.0.0.0
.NET SDK 8.0.100

See [Benchmark project here](https://github.com/vb-consulting/Norm.net/tree/master/Benchmarks)

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

Standard Norm serialization to named tuples, but, containing a reader callback lambda function which only uses a reader to read the first field.

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

#### Round 1

```

BenchmarkDotNet v0.13.10, Debian GNU/Linux 12 (bookworm) (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                  | Records | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
| **Command_Reader**                          | **10**      |     **517.6 μs** |     **11.86 μs** |     **34.98 μs** |     **510.7 μs** |  **0.82** |    **0.07** |          **-** |         **-** |         **-** |     **3.56 KB** |        **0.43** |
| Dapper_Buffered_False                   | 10      |     640.9 μs |     12.78 μs |     34.99 μs |     633.2 μs |  1.01 |    0.08 |          - |         - |         - |     7.88 KB |        0.95 |
| Dapper                                  | 10      |     634.3 μs |     12.64 μs |     31.94 μs |     632.4 μs |  1.00 |    0.00 |     0.9766 |         - |         - |     8.26 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     731.3 μs |     17.24 μs |     49.73 μs |     726.5 μs |  1.15 |    0.09 |     1.9531 |         - |         - |    16.24 KB |        1.96 |
| Norm_Anonymous_Types                    | 10      |     549.6 μs |     11.12 μs |     32.27 μs |     545.3 μs |  0.87 |    0.06 |     0.9766 |         - |         - |     9.76 KB |        1.18 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     538.7 μs |     14.41 μs |     41.58 μs |     532.5 μs |  0.85 |    0.07 |          - |         - |         - |     9.15 KB |        1.11 |
| Norm_Named_Tuples                       | 10      |     530.4 μs |     10.52 μs |     28.62 μs |     529.5 μs |  0.84 |    0.07 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_NameValue_Array                    | 10      |     535.1 μs |     14.10 μs |     41.13 μs |     524.2 μs |  0.84 |    0.07 |     0.9766 |         - |         - |     8.29 KB |        1.00 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     578.6 μs |     12.38 μs |     35.71 μs |     577.7 μs |  0.91 |    0.07 |     0.9766 |         - |         - |    10.29 KB |        1.25 |
| Norm_PocoClass_Instances                | 10      |     569.9 μs |     13.84 μs |     40.16 μs |     562.9 μs |  0.90 |    0.07 |          - |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples_ReaderCallback              | 10      |     529.4 μs |     10.38 μs |     29.44 μs |     526.3 μs |  0.84 |    0.06 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Tuples                             | 10      |     544.7 μs |     15.78 μs |     46.54 μs |     534.2 μs |  0.86 |    0.08 |          - |         - |         - |      2.9 KB |        0.35 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100**     |   **1,082.0 μs** |     **25.63 μs** |     **75.57 μs** |   **1,071.4 μs** |  **0.93** |    **0.11** |     **1.9531** |         **-** |         **-** |    **27.57 KB** |        **0.38** |
| Dapper_Buffered_False                   | 100     |   1,183.8 μs |     27.81 μs |     80.67 μs |   1,181.4 μs |  1.01 |    0.11 |     7.8125 |         - |         - |    70.55 KB |        0.97 |
| Dapper                                  | 100     |   1,181.3 μs |     40.83 μs |    119.11 μs |   1,155.6 μs |  1.00 |    0.00 |     7.8125 |         - |         - |     72.7 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100     |   1,264.0 μs |     33.98 μs |     99.65 μs |   1,254.3 μs |  1.08 |    0.14 |     3.9063 |         - |         - |    57.06 KB |        0.78 |
| Norm_Anonymous_Types                    | 100     |   1,187.9 μs |     29.33 μs |     86.49 μs |   1,177.7 μs |  1.02 |    0.12 |     7.8125 |         - |         - |    69.62 KB |        0.96 |
| Norm_Named_Tuples_ReaderCallback        | 100     |   1,168.7 μs |     28.51 μs |     83.17 μs |   1,164.9 μs |  1.00 |    0.12 |     7.8125 |         - |         - |    75.31 KB |        1.04 |
| Norm_Named_Tuples                       | 100     |   1,141.8 μs |     29.73 μs |     87.66 μs |   1,127.3 μs |  0.98 |    0.12 |     7.8125 |         - |         - |    75.34 KB |        1.04 |
| Norm_NameValue_Array                    | 100     |   1,131.6 μs |     35.53 μs |    104.20 μs |   1,126.5 μs |  0.97 |    0.13 |     7.8125 |         - |         - |    67.46 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100     |   1,172.5 μs |     30.57 μs |     90.13 μs |   1,162.5 μs |  1.00 |    0.12 |     7.8125 |         - |         - |    67.99 KB |        0.94 |
| Norm_PocoClass_Instances                | 100     |   1,159.3 μs |     28.19 μs |     82.23 μs |   1,152.2 μs |  0.99 |    0.11 |     5.8594 |         - |         - |    60.29 KB |        0.83 |
| Norm_Tuples_ReaderCallback              | 100     |   1,168.2 μs |     30.65 μs |     89.90 μs |   1,163.5 μs |  1.00 |    0.12 |     1.9531 |         - |         - |    21.49 KB |        0.30 |
| Norm_Tuples                             | 100     |   1,151.3 μs |     37.92 μs |    110.63 μs |   1,132.8 μs |  0.98 |    0.12 |          - |         - |         - |    19.14 KB |        0.26 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **1000**    |   **5,213.1 μs** |    **117.43 μs** |    **342.54 μs** |   **5,182.4 μs** |  **0.92** |    **0.08** |    **31.2500** |         **-** |         **-** |   **295.41 KB** |        **0.40** |
| Dapper_Buffered_False                   | 1000    |   5,652.6 μs |    130.18 μs |    383.83 μs |   5,651.8 μs |  1.00 |    0.09 |    85.9375 |         - |         - |   724.73 KB |        0.98 |
| Dapper                                  | 1000    |   5,664.2 μs |    119.52 μs |    337.10 μs |   5,697.0 μs |  1.00 |    0.00 |    85.9375 |   23.4375 |         - |   740.91 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   6,188.4 μs |    171.32 μs |    502.45 μs |   6,073.5 μs |  1.10 |    0.11 |    46.8750 |         - |         - |   493.08 KB |        0.67 |
| Norm_Anonymous_Types                    | 1000    |   6,178.6 μs |    128.68 μs |    377.40 μs |   6,138.1 μs |  1.10 |    0.10 |    78.1250 |         - |         - |   695.44 KB |        0.94 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   5,745.9 μs |    124.33 μs |    360.70 μs |   5,762.3 μs |  1.02 |    0.08 |    85.9375 |         - |         - |   764.51 KB |        1.03 |
| Norm_Named_Tuples                       | 1000    |   5,805.5 μs |    136.91 μs |    403.68 μs |   5,810.5 μs |  1.03 |    0.09 |    85.9375 |         - |         - |   764.46 KB |        1.03 |
| Norm_NameValue_Array                    | 1000    |   5,441.5 μs |    119.22 μs |    351.53 μs |   5,460.1 μs |  0.97 |    0.08 |    78.1250 |         - |         - |   686.66 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,903.2 μs |    137.69 μs |    403.82 μs |   5,878.9 μs |  1.04 |    0.09 |    78.1250 |         - |         - |   672.88 KB |        0.91 |
| Norm_PocoClass_Instances                | 1000    |   5,768.0 μs |    131.37 μs |    387.34 μs |   5,758.0 μs |  1.02 |    0.09 |    70.3125 |         - |         - |   594.89 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 1000    |   6,012.2 μs |    131.75 μs |    384.34 μs |   5,999.7 μs |  1.06 |    0.10 |    23.4375 |         - |         - |    232.6 KB |        0.31 |
| Norm_Tuples                             | 1000    |   6,068.6 μs |    131.16 μs |    384.66 μs |   6,085.6 μs |  1.07 |    0.09 |    23.4375 |         - |         - |   209.06 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **10000**   |  **39,086.7 μs** |    **843.15 μs** |  **2,472.83 μs** |  **38,714.8 μs** |  **0.65** |    **0.06** |   **307.6923** |         **-** |         **-** |  **2968.31 KB** |        **0.39** |
| Dapper_Buffered_False                   | 10000   |  43,874.3 μs |    874.83 μs |  2,304.65 μs |  43,873.8 μs |  0.73 |    0.06 |   818.1818 |         - |         - |  7264.21 KB |        0.97 |
| Dapper                                  | 10000   |  60,626.5 μs |  1,204.16 μs |  3,416.01 μs |  60,509.7 μs |  1.00 |    0.00 |   888.8889 |  555.5556 |  222.2222 |  7521.52 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  49,088.0 μs |    976.52 μs |  2,754.30 μs |  48,755.4 μs |  0.81 |    0.06 |   571.4286 |         - |         - |     4854 KB |        0.65 |
| Norm_Anonymous_Types                    | 10000   |  49,914.9 μs |    996.89 μs |  2,892.15 μs |  49,842.3 μs |  0.83 |    0.06 |   777.7778 |         - |         - |  6953.85 KB |        0.92 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  45,222.7 μs |    913.07 μs |  2,663.46 μs |  44,800.9 μs |  0.75 |    0.06 |   909.0909 |         - |         - |  7655.62 KB |        1.02 |
| Norm_Named_Tuples                       | 10000   |  45,584.7 μs |    971.98 μs |  2,850.66 μs |  45,002.8 μs |  0.75 |    0.07 |   909.0909 |         - |         - |  7655.89 KB |        1.02 |
| Norm_NameValue_Array                    | 10000   |  41,510.4 μs |    826.64 μs |  2,089.02 μs |  41,186.1 μs |  0.69 |    0.06 |   833.3333 |         - |         - |  6875.11 KB |        0.91 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  46,989.1 μs |  1,060.14 μs |  3,075.66 μs |  46,498.9 μs |  0.78 |    0.07 |   818.1818 |         - |         - |   6722.9 KB |        0.89 |
| Norm_PocoClass_Instances                | 10000   |  46,987.1 μs |  1,321.93 μs |  3,856.12 μs |  46,315.5 μs |  0.78 |    0.08 |   500.0000 |         - |         - |  5941.23 KB |        0.79 |
| Norm_Tuples_ReaderCallback              | 10000   |  47,840.8 μs |    955.95 μs |  2,696.26 μs |  47,634.9 μs |  0.79 |    0.06 |   272.7273 |         - |         - |  2342.78 KB |        0.31 |
| Norm_Tuples                             | 10000   |  48,000.4 μs |    959.40 μs |  2,406.96 μs |  47,712.6 μs |  0.80 |    0.06 |   181.8182 |         - |         - |  2108.96 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100000**  | **360,967.3 μs** | **13,542.97 μs** | **39,931.76 μs** | **360,128.5 μs** |  **0.62** |    **0.08** |  **3000.0000** |         **-** |         **-** | **30390.18 KB** |        **0.40** |
| Dapper_Buffered_False                   | 100000  | 408,091.0 μs | 11,757.30 μs | 34,482.13 μs | 407,562.3 μs |  0.69 |    0.07 |  8000.0000 |         - |         - | 73358.65 KB |        0.97 |
| Dapper                                  | 100000  | 591,684.0 μs | 11,703.89 μs | 28,488.89 μs | 592,490.3 μs |  1.00 |    0.00 | 10000.0000 | 5000.0000 | 2000.0000 | 75439.47 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 430,804.8 μs | 12,971.44 μs | 37,838.31 μs | 425,081.4 μs |  0.73 |    0.07 |  6000.0000 |         - |         - | 49150.92 KB |        0.65 |
| Norm_Anonymous_Types                    | 100000  | 409,893.5 μs | 11,396.52 μs | 32,881.55 μs | 408,157.2 μs |  0.69 |    0.06 |  8000.0000 |         - |         - | 70242.37 KB |        0.93 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 437,003.3 μs | 14,050.01 μs | 41,426.78 μs | 431,296.2 μs |  0.74 |    0.08 |  9000.0000 |         - |         - | 77272.29 KB |        1.02 |
| Norm_Named_Tuples                       | 100000  | 421,935.6 μs | 13,088.49 μs | 37,553.34 μs | 418,119.5 μs |  0.71 |    0.08 |  9000.0000 |         - |         - | 77267.79 KB |        1.02 |
| Norm_NameValue_Array                    | 100000  | 402,777.6 μs | 11,877.34 μs | 34,458.32 μs | 403,955.8 μs |  0.68 |    0.07 |  8000.0000 |         - |         - | 69454.65 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 446,899.3 μs | 13,511.26 μs | 39,838.24 μs | 445,233.2 μs |  0.76 |    0.08 |  8000.0000 |         - |         - | 67937.85 KB |        0.90 |
| Norm_PocoClass_Instances                | 100000  | 399,019.8 μs | 10,339.23 μs | 29,995.97 μs | 392,905.9 μs |  0.68 |    0.06 |  7000.0000 |         - |         - | 60101.44 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 100000  | 410,133.2 μs | 11,173.29 μs | 32,237.51 μs | 404,015.3 μs |  0.70 |    0.07 |  2000.0000 |         - |         - | 24142.09 KB |        0.32 |
| Norm_Tuples                             | 100000  | 407,943.4 μs | 12,391.90 μs | 35,753.46 μs | 410,471.0 μs |  0.69 |    0.07 |  2000.0000 |         - |         - | 21809.59 KB |        0.29 |

#### Round 2

```

BenchmarkDotNet v0.13.10, Debian GNU/Linux 12 (bookworm) (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                  | Records | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
| **Command_Reader**                          | **10**      |     **524.0 μs** |     **12.37 μs** |     **35.30 μs** |     **519.2 μs** |  **0.81** |    **0.07** |          **-** |         **-** |         **-** |     **3.56 KB** |        **0.43** |
| Dapper_Buffered_False                   | 10      |     658.4 μs |     17.95 μs |     52.07 μs |     649.0 μs |  1.02 |    0.10 |          - |         - |         - |     7.89 KB |        0.95 |
| Dapper                                  | 10      |     649.9 μs |     13.27 μs |     38.51 μs |     642.1 μs |  1.00 |    0.00 |     0.9766 |         - |         - |     8.26 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     734.2 μs |     16.31 μs |     47.05 μs |     735.7 μs |  1.13 |    0.09 |     1.9531 |         - |         - |    16.25 KB |        1.97 |
| Norm_Anonymous_Types                    | 10      |     575.0 μs |     15.35 μs |     44.77 μs |     567.4 μs |  0.89 |    0.09 |     0.9766 |         - |         - |     9.77 KB |        1.18 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     542.1 μs |     13.07 μs |     37.51 μs |     539.6 μs |  0.84 |    0.07 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_Named_Tuples                       | 10      |     545.0 μs |     11.97 μs |     34.93 μs |     542.7 μs |  0.84 |    0.07 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_NameValue_Array                    | 10      |     542.2 μs |     14.43 μs |     41.88 μs |     535.7 μs |  0.84 |    0.08 |     0.9766 |         - |         - |     8.29 KB |        1.00 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     578.4 μs |     14.83 μs |     43.03 μs |     573.3 μs |  0.89 |    0.08 |          - |         - |         - |     10.3 KB |        1.25 |
| Norm_PocoClass_Instances                | 10      |     568.7 μs |     15.16 μs |     44.23 μs |     554.4 μs |  0.88 |    0.09 |          - |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples_ReaderCallback              | 10      |     540.5 μs |     11.88 μs |     34.08 μs |     535.2 μs |  0.83 |    0.07 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Tuples                             | 10      |     536.9 μs |     10.64 μs |     29.11 μs |     534.1 μs |  0.83 |    0.07 |          - |         - |         - |      2.9 KB |        0.35 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100**     |   **1,092.7 μs** |     **25.83 μs** |     **76.16 μs** |   **1,091.7 μs** |  **0.91** |    **0.11** |     **1.9531** |         **-** |         **-** |    **27.59 KB** |        **0.38** |
| Dapper_Buffered_False                   | 100     |   1,196.8 μs |     35.81 μs |    105.03 μs |   1,181.7 μs |  0.99 |    0.13 |     7.8125 |         - |         - |    70.52 KB |        0.97 |
| Dapper                                  | 100     |   1,216.8 μs |     40.45 μs |    119.27 μs |   1,202.4 μs |  1.00 |    0.00 |     7.8125 |         - |         - |     72.7 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100     |   1,296.1 μs |     31.13 μs |     91.30 μs |   1,302.2 μs |  1.07 |    0.13 |     3.9063 |         - |         - |    57.02 KB |        0.78 |
| Norm_Anonymous_Types                    | 100     |   1,211.1 μs |     39.81 μs |    116.76 μs |   1,203.6 μs |  1.00 |    0.14 |     7.8125 |         - |         - |     69.6 KB |        0.96 |
| Norm_Named_Tuples_ReaderCallback        | 100     |   1,168.4 μs |     33.22 μs |     97.95 μs |   1,144.9 μs |  0.97 |    0.12 |     7.8125 |         - |         - |    75.31 KB |        1.04 |
| Norm_Named_Tuples                       | 100     |   1,158.5 μs |     26.40 μs |     76.18 μs |   1,152.7 μs |  0.96 |    0.11 |     7.8125 |         - |         - |    75.33 KB |        1.04 |
| Norm_NameValue_Array                    | 100     |   1,138.7 μs |     34.98 μs |    102.58 μs |   1,125.6 μs |  0.94 |    0.12 |     7.8125 |         - |         - |    67.43 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100     |   1,184.2 μs |     32.86 μs |     96.38 μs |   1,186.1 μs |  0.98 |    0.12 |     7.8125 |         - |         - |    68.03 KB |        0.94 |
| Norm_PocoClass_Instances                | 100     |   1,200.1 μs |     32.02 μs |     94.40 μs |   1,191.6 μs |  1.00 |    0.13 |     5.8594 |         - |         - |     60.3 KB |        0.83 |
| Norm_Tuples_ReaderCallback              | 100     |   1,168.6 μs |     30.01 μs |     88.47 μs |   1,156.6 μs |  0.97 |    0.11 |     1.9531 |         - |         - |    21.51 KB |        0.30 |
| Norm_Tuples                             | 100     |   1,184.4 μs |     27.76 μs |     81.86 μs |   1,172.9 μs |  0.98 |    0.11 |     1.9531 |         - |         - |    19.14 KB |        0.26 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **1000**    |   **5,379.7 μs** |    **124.28 μs** |    **366.44 μs** |   **5,354.2 μs** |  **0.93** |    **0.08** |    **31.2500** |         **-** |         **-** |   **295.35 KB** |        **0.40** |
| Dapper_Buffered_False                   | 1000    |   5,794.2 μs |    125.93 μs |    369.33 μs |   5,753.3 μs |  1.01 |    0.09 |    85.9375 |         - |         - |   724.59 KB |        0.98 |
| Dapper                                  | 1000    |   5,777.2 μs |    132.38 μs |    390.33 μs |   5,778.0 μs |  1.00 |    0.00 |    85.9375 |   23.4375 |         - |   740.93 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   6,258.6 μs |    165.77 μs |    483.56 μs |   6,237.2 μs |  1.09 |    0.12 |    46.8750 |         - |         - |   493.11 KB |        0.67 |
| Norm_Anonymous_Types                    | 1000    |   6,354.1 μs |    126.63 μs |    324.60 μs |   6,391.4 μs |  1.11 |    0.09 |    78.1250 |         - |         - |   695.57 KB |        0.94 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   6,035.2 μs |    136.39 μs |    402.16 μs |   5,972.8 μs |  1.05 |    0.10 |    85.9375 |         - |         - |   764.57 KB |        1.03 |
| Norm_Named_Tuples                       | 1000    |   6,017.9 μs |    129.50 μs |    377.77 μs |   6,029.9 μs |  1.05 |    0.10 |    85.9375 |         - |         - |   764.49 KB |        1.03 |
| Norm_NameValue_Array                    | 1000    |   5,522.8 μs |    129.09 μs |    380.62 μs |   5,529.8 μs |  0.96 |    0.09 |    78.1250 |         - |         - |   686.67 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,912.5 μs |    117.77 μs |    310.24 μs |   5,884.0 μs |  1.03 |    0.09 |    78.1250 |         - |         - |   672.93 KB |        0.91 |
| Norm_PocoClass_Instances                | 1000    |   6,019.7 μs |    145.84 μs |    430.01 μs |   5,968.1 μs |  1.05 |    0.10 |    70.3125 |         - |         - |   594.98 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 1000    |   6,132.2 μs |    123.98 μs |    365.57 μs |   6,100.0 μs |  1.07 |    0.10 |    23.4375 |         - |         - |   232.56 KB |        0.31 |
| Norm_Tuples                             | 1000    |   5,865.6 μs |    154.11 μs |    449.55 μs |   5,831.0 μs |  1.02 |    0.11 |    23.4375 |         - |         - |   209.12 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **10000**   |  **39,097.3 μs** |    **941.30 μs** |  **2,730.87 μs** |  **38,843.4 μs** |  **0.66** |    **0.06** |   **333.3333** |         **-** |         **-** |  **2969.41 KB** |        **0.39** |
| Dapper_Buffered_False                   | 10000   |  43,982.5 μs |    872.55 μs |  2,298.64 μs |  44,217.2 μs |  0.75 |    0.05 |   888.8889 |         - |         - |  7264.42 KB |        0.97 |
| Dapper                                  | 10000   |  58,930.5 μs |  1,167.33 μs |  2,728.60 μs |  58,831.5 μs |  1.00 |    0.00 |   888.8889 |  555.5556 |  222.2222 |  7520.98 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  48,248.9 μs |    942.45 μs |  1,946.33 μs |  48,353.4 μs |  0.82 |    0.05 |   500.0000 |         - |         - |  4854.97 KB |        0.65 |
| Norm_Anonymous_Types                    | 10000   |  50,031.2 μs |  1,710.60 μs |  5,016.88 μs |  48,771.3 μs |  0.85 |    0.09 |   750.0000 |         - |         - |  6954.21 KB |        0.92 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  44,675.2 μs |    876.94 μs |  2,340.74 μs |  44,458.3 μs |  0.76 |    0.05 |   916.6667 |         - |         - |  7657.06 KB |        1.02 |
| Norm_Named_Tuples                       | 10000   |  44,790.4 μs |    868.85 μs |  1,427.54 μs |  44,881.4 μs |  0.76 |    0.04 |   909.0909 |         - |         - |   7657.1 KB |        1.02 |
| Norm_NameValue_Array                    | 10000   |  41,447.6 μs |    826.99 μs |  2,412.36 μs |  41,482.1 μs |  0.71 |    0.06 |   818.1818 |         - |         - |  6877.55 KB |        0.91 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  46,959.8 μs |    999.96 μs |  2,916.92 μs |  46,534.5 μs |  0.80 |    0.06 |   818.1818 |         - |         - |  6721.43 KB |        0.89 |
| Norm_PocoClass_Instances                | 10000   |  46,338.4 μs |    916.94 μs |  2,601.21 μs |  45,756.5 μs |  0.80 |    0.05 |   727.2727 |         - |         - |   5942.9 KB |        0.79 |
| Norm_Tuples_ReaderCallback              | 10000   |  47,650.0 μs |    945.40 μs |  2,712.54 μs |  47,083.3 μs |  0.81 |    0.06 |   272.7273 |         - |         - |  2344.48 KB |        0.31 |
| Norm_Tuples                             | 10000   |  48,488.9 μs |    964.88 μs |  1,992.65 μs |  48,335.6 μs |  0.82 |    0.05 |   181.8182 |         - |         - |  2109.42 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100000**  | **367,873.0 μs** | **13,381.43 μs** | **39,455.44 μs** | **364,312.1 μs** |  **0.63** |    **0.08** |  **3000.0000** |         **-** |         **-** | **30420.18 KB** |        **0.40** |
| Dapper_Buffered_False                   | 100000  | 412,763.2 μs | 12,854.91 μs | 37,701.24 μs | 416,303.6 μs |  0.69 |    0.07 |  8000.0000 |         - |         - | 73358.65 KB |        0.97 |
| Dapper                                  | 100000  | 592,008.8 μs | 11,804.43 μs | 25,661.86 μs | 590,591.4 μs |  1.00 |    0.00 | 10000.0000 | 5000.0000 | 2000.0000 |  75409.6 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 408,393.7 μs |  9,431.60 μs | 27,512.43 μs | 404,653.7 μs |  0.70 |    0.05 |  6000.0000 |         - |         - | 49150.92 KB |        0.65 |
| Norm_Anonymous_Types                    | 100000  | 431,558.5 μs | 10,866.97 μs | 31,699.45 μs | 423,294.3 μs |  0.74 |    0.06 |  8000.0000 |         - |         - | 70255.87 KB |        0.93 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 387,114.8 μs |  8,190.00 μs | 22,830.44 μs | 386,057.0 μs |  0.66 |    0.05 |  9000.0000 |         - |         - | 77267.79 KB |        1.02 |
| Norm_Named_Tuples                       | 100000  | 425,086.2 μs | 13,883.44 μs | 40,935.63 μs | 430,734.3 μs |  0.71 |    0.08 |  9000.0000 |         - |         - | 77268.35 KB |        1.02 |
| Norm_NameValue_Array                    | 100000  | 385,006.6 μs | 12,363.11 μs | 36,258.86 μs | 388,578.4 μs |  0.65 |    0.07 |  8000.0000 |         - |         - | 69478.79 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 405,695.4 μs | 10,537.25 μs | 30,570.48 μs | 405,727.6 μs |  0.68 |    0.06 |  8000.0000 |         - |         - | 67900.94 KB |        0.90 |
| Norm_PocoClass_Instances                | 100000  | 414,667.6 μs | 14,325.69 μs | 42,239.61 μs | 410,302.0 μs |  0.70 |    0.08 |  7000.0000 |         - |         - | 60081.19 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 100000  | 407,700.0 μs | 10,057.58 μs | 29,178.85 μs | 404,587.9 μs |  0.70 |    0.07 |  2000.0000 |         - |         - | 24147.52 KB |        0.32 |
| Norm_Tuples                             | 100000  | 408,334.9 μs | 11,782.65 μs | 34,370.55 μs | 404,867.4 μs |  0.70 |    0.07 |  2000.0000 |         - |         - | 21799.09 KB |        0.29 |

#### Round 3

```

BenchmarkDotNet v0.13.10, Debian GNU/Linux 12 (bookworm) (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                  | Records | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
| **Command_Reader**                          | **10**      |     **524.8 μs** |     **11.09 μs** |     **32.35 μs** |     **523.9 μs** |  **0.80** |    **0.07** |          **-** |         **-** |         **-** |     **3.56 KB** |        **0.43** |
| Dapper_Buffered_False                   | 10      |     657.3 μs |     13.26 μs |     37.61 μs |     658.2 μs |  1.00 |    0.09 |          - |         - |         - |     7.89 KB |        0.96 |
| Dapper                                  | 10      |     658.5 μs |     15.11 μs |     44.54 μs |     649.5 μs |  1.00 |    0.00 |     0.9766 |         - |         - |     8.25 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     733.3 μs |     17.94 μs |     52.62 μs |     726.6 μs |  1.12 |    0.11 |     1.9531 |         - |         - |    16.25 KB |        1.97 |
| Norm_Anonymous_Types                    | 10      |     555.9 μs |     14.76 μs |     42.59 μs |     554.5 μs |  0.85 |    0.09 |          - |         - |         - |     9.76 KB |        1.18 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     540.2 μs |     12.23 μs |     35.68 μs |     535.2 μs |  0.82 |    0.07 |     0.9766 |         - |         - |     9.14 KB |        1.11 |
| Norm_Named_Tuples                       | 10      |     542.0 μs |     12.34 μs |     36.19 μs |     542.9 μs |  0.83 |    0.08 |     0.9766 |         - |         - |     9.14 KB |        1.11 |
| Norm_NameValue_Array                    | 10      |     521.0 μs |     10.35 μs |     29.38 μs |     517.0 μs |  0.79 |    0.07 |     0.9766 |         - |         - |     8.29 KB |        1.00 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     574.0 μs |     11.38 μs |     30.57 μs |     573.3 μs |  0.87 |    0.08 |     0.9766 |         - |         - |     10.3 KB |        1.25 |
| Norm_PocoClass_Instances                | 10      |     554.7 μs |     12.34 μs |     35.98 μs |     552.2 μs |  0.85 |    0.09 |     0.9766 |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples_ReaderCallback              | 10      |     532.7 μs |     12.23 μs |     35.49 μs |     529.4 μs |  0.81 |    0.08 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Tuples                             | 10      |     537.0 μs |     13.16 μs |     38.80 μs |     532.9 μs |  0.82 |    0.08 |          - |         - |         - |      2.9 KB |        0.35 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100**     |   **1,057.3 μs** |     **25.18 μs** |     **72.65 μs** |   **1,059.0 μs** |  **0.89** |    **0.10** |     **1.9531** |         **-** |         **-** |    **27.56 KB** |        **0.38** |
| Dapper_Buffered_False                   | 100     |   1,183.6 μs |     30.79 μs |     90.79 μs |   1,179.3 μs |  1.00 |    0.10 |     7.8125 |         - |         - |    70.48 KB |        0.97 |
| Dapper                                  | 100     |   1,190.3 μs |     30.11 μs |     88.30 μs |   1,180.2 μs |  1.00 |    0.00 |     7.8125 |         - |         - |    72.72 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100     |   1,263.4 μs |     35.82 μs |    105.05 μs |   1,250.8 μs |  1.07 |    0.11 |     3.9063 |         - |         - |    57.04 KB |        0.78 |
| Norm_Anonymous_Types                    | 100     |   1,178.0 μs |     42.91 μs |    125.17 μs |   1,154.6 μs |  0.99 |    0.12 |     7.8125 |         - |         - |    69.61 KB |        0.96 |
| Norm_Named_Tuples_ReaderCallback        | 100     |   1,214.0 μs |     42.33 μs |    124.13 μs |   1,210.2 μs |  1.02 |    0.13 |     7.8125 |         - |         - |    75.32 KB |        1.04 |
| Norm_Named_Tuples                       | 100     |   1,158.2 μs |     34.98 μs |    102.05 μs |   1,157.2 μs |  0.98 |    0.10 |     7.8125 |         - |         - |    75.31 KB |        1.04 |
| Norm_NameValue_Array                    | 100     |   1,119.5 μs |     27.06 μs |     79.35 μs |   1,110.1 μs |  0.94 |    0.09 |     7.8125 |         - |         - |    67.48 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100     |   1,169.7 μs |     29.47 μs |     86.44 μs |   1,157.0 μs |  0.99 |    0.11 |     7.8125 |         - |         - |    68.03 KB |        0.94 |
| Norm_PocoClass_Instances                | 100     |   1,184.1 μs |     39.70 μs |    116.45 μs |   1,174.6 μs |  1.00 |    0.11 |     3.9063 |         - |         - |    60.28 KB |        0.83 |
| Norm_Tuples_ReaderCallback              | 100     |   1,166.1 μs |     29.04 μs |     85.17 μs |   1,167.5 μs |  0.98 |    0.09 |     1.9531 |         - |         - |    21.52 KB |        0.30 |
| Norm_Tuples                             | 100     |   1,171.7 μs |     33.13 μs |     97.17 μs |   1,162.5 μs |  0.99 |    0.12 |     1.9531 |         - |         - |    19.14 KB |        0.26 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **1000**    |   **5,212.0 μs** |    **115.98 μs** |    **341.96 μs** |   **5,145.4 μs** |  **0.92** |    **0.08** |    **31.2500** |         **-** |         **-** |   **295.62 KB** |        **0.40** |
| Dapper_Buffered_False                   | 1000    |   5,609.7 μs |    116.06 μs |    338.55 μs |   5,624.7 μs |  0.99 |    0.09 |    85.9375 |         - |         - |   724.64 KB |        0.98 |
| Dapper                                  | 1000    |   5,697.9 μs |    124.80 μs |    364.04 μs |   5,685.2 μs |  1.00 |    0.00 |    85.9375 |   23.4375 |         - |   740.82 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   6,200.3 μs |    155.95 μs |    457.38 μs |   6,259.2 μs |  1.09 |    0.10 |    46.8750 |         - |         - |   493.18 KB |        0.67 |
| Norm_Anonymous_Types                    | 1000    |   6,241.5 μs |    148.48 μs |    437.80 μs |   6,201.5 μs |  1.10 |    0.11 |    78.1250 |         - |         - |   695.51 KB |        0.94 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   5,818.9 μs |    141.94 μs |    418.51 μs |   5,779.4 μs |  1.03 |    0.10 |    85.9375 |         - |         - |   764.57 KB |        1.03 |
| Norm_Named_Tuples                       | 1000    |   5,719.6 μs |    117.69 μs |    339.57 μs |   5,709.3 μs |  1.01 |    0.09 |    85.9375 |         - |         - |    764.6 KB |        1.03 |
| Norm_NameValue_Array                    | 1000    |   5,391.8 μs |    112.18 μs |    330.78 μs |   5,354.3 μs |  0.95 |    0.08 |    78.1250 |         - |         - |   686.52 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,900.2 μs |    129.28 μs |    381.18 μs |   5,908.8 μs |  1.04 |    0.10 |    78.1250 |         - |         - |   672.97 KB |        0.91 |
| Norm_PocoClass_Instances                | 1000    |   5,883.7 μs |    160.04 μs |    471.87 μs |   5,914.5 μs |  1.04 |    0.11 |    62.5000 |         - |         - |   594.95 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 1000    |   5,736.5 μs |    135.88 μs |    400.65 μs |   5,681.5 μs |  1.01 |    0.09 |    23.4375 |         - |         - |   232.42 KB |        0.31 |
| Norm_Tuples                             | 1000    |   5,846.5 μs |    133.63 μs |    391.91 μs |   5,796.7 μs |  1.03 |    0.09 |    23.4375 |         - |         - |    209.1 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **10000**   |  **38,727.8 μs** |    **747.55 μs** |  **2,156.86 μs** |  **38,448.5 μs** |  **0.66** |    **0.05** |   **333.3333** |         **-** |         **-** |  **2970.95 KB** |        **0.40** |
| Dapper_Buffered_False                   | 10000   |  43,421.0 μs |    864.68 μs |  2,153.35 μs |  43,270.6 μs |  0.73 |    0.05 |   800.0000 |         - |         - |  7264.48 KB |        0.97 |
| Dapper                                  | 10000   |  59,447.5 μs |  1,179.13 μs |  3,043.71 μs |  58,885.7 μs |  1.00 |    0.00 |   888.8889 |  555.5556 |  222.2222 |  7521.16 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  49,114.7 μs |    976.84 μs |  2,521.55 μs |  48,750.5 μs |  0.83 |    0.06 |   500.0000 |         - |         - |  4853.49 KB |        0.65 |
| Norm_Anonymous_Types                    | 10000   |  48,528.6 μs |    963.42 μs |  2,289.66 μs |  48,428.4 μs |  0.82 |    0.06 |   800.0000 |         - |         - |  6955.49 KB |        0.92 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  44,138.1 μs |    881.64 μs |  1,655.94 μs |  44,127.6 μs |  0.75 |    0.05 |   909.0909 |         - |         - |  7657.17 KB |        1.02 |
| Norm_Named_Tuples                       | 10000   |  45,105.4 μs |    892.28 μs |  2,254.90 μs |  45,080.1 μs |  0.76 |    0.06 |   909.0909 |         - |         - |  7655.52 KB |        1.02 |
| Norm_NameValue_Array                    | 10000   |  40,575.9 μs |    810.17 μs |  1,893.75 μs |  40,204.4 μs |  0.68 |    0.05 |   833.3333 |         - |         - |  6877.66 KB |        0.91 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  45,900.5 μs |    877.54 μs |  1,711.58 μs |  46,140.8 μs |  0.78 |    0.05 |   818.1818 |         - |         - |  6721.89 KB |        0.89 |
| Norm_PocoClass_Instances                | 10000   |  45,102.2 μs |    921.42 μs |  2,687.84 μs |  44,483.4 μs |  0.76 |    0.06 |   666.6667 |         - |         - |   5940.4 KB |        0.79 |
| Norm_Tuples_ReaderCallback              | 10000   |  47,174.2 μs |  1,081.07 μs |  3,153.52 μs |  46,639.6 μs |  0.80 |    0.07 |   272.7273 |         - |         - |  2342.73 KB |        0.31 |
| Norm_Tuples                             | 10000   |  46,517.7 μs |    928.96 μs |  2,709.82 μs |  45,723.2 μs |  0.78 |    0.06 |   181.8182 |         - |         - |  2111.59 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100000**  | **361,561.3 μs** | **10,555.65 μs** | **30,957.89 μs** | **360,004.4 μs** |  **0.64** |    **0.06** |  **3000.0000** |         **-** |         **-** | **30411.51 KB** |        **0.40** |
| Dapper_Buffered_False                   | 100000  | 431,940.5 μs | 10,864.93 μs | 31,693.52 μs | 431,351.5 μs |  0.75 |    0.06 |  8000.0000 |         - |         - | 73358.65 KB |        0.97 |
| Dapper                                  | 100000  | 565,696.9 μs | 11,188.21 μs | 22,084.44 μs | 561,812.2 μs |  1.00 |    0.00 | 10000.0000 | 5000.0000 | 2000.0000 | 75438.92 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 462,485.9 μs | 13,044.75 μs | 38,462.73 μs | 461,067.2 μs |  0.83 |    0.08 |  6000.0000 |         - |         - | 49149.66 KB |        0.65 |
| Norm_Anonymous_Types                    | 100000  | 411,138.9 μs |  8,824.24 μs | 25,600.72 μs | 405,562.2 μs |  0.73 |    0.05 |  8000.0000 |         - |         - |  70237.3 KB |        0.93 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 412,334.6 μs | 12,134.40 μs | 35,588.08 μs | 409,751.4 μs |  0.73 |    0.07 |  9000.0000 |         - |         - | 77275.83 KB |        1.02 |
| Norm_Named_Tuples                       | 100000  | 400,954.0 μs | 11,779.82 μs | 34,548.18 μs | 396,187.0 μs |  0.71 |    0.07 |  9000.0000 |         - |         - | 77267.93 KB |        1.02 |
| Norm_NameValue_Array                    | 100000  | 381,007.9 μs | 11,533.27 μs | 34,006.12 μs | 381,350.3 μs |  0.67 |    0.06 |  8000.0000 |         - |         - | 69453.71 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 411,183.4 μs | 10,474.09 μs | 30,387.24 μs | 410,445.9 μs |  0.72 |    0.06 |  8000.0000 |         - |         - | 67896.06 KB |        0.90 |
| Norm_PocoClass_Instances                | 100000  | 403,805.5 μs | 11,469.35 μs | 33,456.62 μs | 399,211.1 μs |  0.71 |    0.06 |  7000.0000 |         - |         - |    60099 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 100000  | 417,068.0 μs | 13,121.63 μs | 38,276.40 μs | 410,823.0 μs |  0.74 |    0.09 |  2000.0000 |         - |         - | 24164.02 KB |        0.32 |
| Norm_Tuples                             | 100000  | 420,217.3 μs | 10,734.63 μs | 31,482.80 μs | 413,948.9 μs |  0.75 |    0.05 |  2000.0000 |         - |         - | 21804.71 KB |        0.29 |

#### Round 4

```

BenchmarkDotNet v0.13.10, Debian GNU/Linux 12 (bookworm) (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                  | Records | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
| **Command_Reader**                          | **10**      |     **512.8 μs** |     **11.17 μs** |     **32.75 μs** |     **510.0 μs** |  **0.80** |    **0.06** |          **-** |         **-** |         **-** |     **3.56 KB** |        **0.43** |
| Dapper_Buffered_False                   | 10      |     641.5 μs |     12.72 μs |     33.96 μs |     636.2 μs |  1.00 |    0.07 |          - |         - |         - |      7.9 KB |        0.96 |
| Dapper                                  | 10      |     643.3 μs |     12.84 μs |     33.14 μs |     639.5 μs |  1.00 |    0.00 |     0.9766 |         - |         - |     8.23 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     725.3 μs |     19.95 μs |     57.55 μs |     711.5 μs |  1.14 |    0.13 |     1.9531 |         - |         - |    16.24 KB |        1.97 |
| Norm_Anonymous_Types                    | 10      |     541.7 μs |     10.80 μs |     29.37 μs |     540.0 μs |  0.84 |    0.06 |     0.9766 |         - |         - |     9.77 KB |        1.19 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     538.7 μs |     12.89 μs |     37.79 μs |     534.1 μs |  0.84 |    0.06 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_Named_Tuples                       | 10      |     527.3 μs |     11.87 μs |     34.62 μs |     520.4 μs |  0.82 |    0.06 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_NameValue_Array                    | 10      |     519.1 μs |     10.25 μs |     28.57 μs |     515.5 μs |  0.81 |    0.07 |     0.9766 |         - |         - |     8.29 KB |        1.01 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     560.4 μs |     11.12 μs |     29.10 μs |     557.9 μs |  0.87 |    0.06 |     0.9766 |         - |         - |     10.3 KB |        1.25 |
| Norm_PocoClass_Instances                | 10      |     570.7 μs |     12.13 μs |     35.59 μs |     564.5 μs |  0.89 |    0.07 |     0.9766 |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples_ReaderCallback              | 10      |     530.2 μs |     10.59 μs |     25.97 μs |     525.4 μs |  0.83 |    0.06 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Tuples                             | 10      |     525.4 μs |     10.47 μs |     28.30 μs |     523.5 μs |  0.82 |    0.06 |          - |         - |         - |      2.9 KB |        0.35 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100**     |   **1,046.0 μs** |     **25.36 μs** |     **73.99 μs** |   **1,043.9 μs** |  **0.84** |    **0.08** |     **1.9531** |         **-** |         **-** |    **27.58 KB** |        **0.38** |
| Dapper_Buffered_False                   | 100     |   1,196.7 μs |     30.99 μs |     91.37 μs |   1,193.6 μs |  0.96 |    0.09 |     7.8125 |         - |         - |    70.47 KB |        0.97 |
| Dapper                                  | 100     |   1,249.4 μs |     29.23 μs |     85.26 μs |   1,253.5 μs |  1.00 |    0.00 |     7.8125 |         - |         - |    72.67 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100     |   1,297.3 μs |     31.48 μs |     91.82 μs |   1,289.6 μs |  1.04 |    0.10 |     3.9063 |         - |         - |    57.07 KB |        0.79 |
| Norm_Anonymous_Types                    | 100     |   1,172.2 μs |     32.68 μs |     95.83 μs |   1,160.2 μs |  0.94 |    0.10 |     7.8125 |         - |         - |     69.6 KB |        0.96 |
| Norm_Named_Tuples_ReaderCallback        | 100     |   1,163.5 μs |     30.97 μs |     90.82 μs |   1,143.1 μs |  0.94 |    0.10 |     7.8125 |         - |         - |    75.31 KB |        1.04 |
| Norm_Named_Tuples                       | 100     |   1,178.1 μs |     31.90 μs |     93.04 μs |   1,181.8 μs |  0.95 |    0.11 |     7.8125 |         - |         - |    75.34 KB |        1.04 |
| Norm_NameValue_Array                    | 100     |   1,163.7 μs |     25.53 μs |     74.07 μs |   1,162.7 μs |  0.94 |    0.09 |     7.8125 |         - |         - |    67.47 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100     |   1,237.6 μs |     26.38 μs |     77.36 μs |   1,241.1 μs |  0.99 |    0.09 |     7.8125 |         - |         - |    68.03 KB |        0.94 |
| Norm_PocoClass_Instances                | 100     |   1,248.5 μs |     29.89 μs |     87.67 μs |   1,243.0 μs |  1.00 |    0.09 |     5.8594 |         - |         - |    60.29 KB |        0.83 |
| Norm_Tuples_ReaderCallback              | 100     |   1,233.2 μs |     29.19 μs |     85.16 μs |   1,230.5 μs |  0.99 |    0.10 |     1.9531 |         - |         - |    21.48 KB |        0.30 |
| Norm_Tuples                             | 100     |   1,221.0 μs |     34.73 μs |    101.31 μs |   1,230.4 μs |  0.98 |    0.10 |     1.9531 |         - |         - |    19.13 KB |        0.26 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **1000**    |   **5,681.0 μs** |    **133.45 μs** |    **391.40 μs** |   **5,738.6 μs** |  **0.92** |    **0.09** |    **31.2500** |         **-** |         **-** |   **295.28 KB** |        **0.40** |
| Dapper_Buffered_False                   | 1000    |   6,186.1 μs |    129.01 μs |    374.30 μs |   6,165.9 μs |  1.01 |    0.09 |    85.9375 |         - |         - |    724.5 KB |        0.98 |
| Dapper                                  | 1000    |   6,180.0 μs |    122.87 μs |    350.56 μs |   6,207.7 μs |  1.00 |    0.00 |    85.9375 |   23.4375 |         - |   741.02 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   6,608.4 μs |    209.83 μs |    612.08 μs |   6,557.1 μs |  1.07 |    0.12 |    31.2500 |         - |         - |   493.34 KB |        0.67 |
| Norm_Anonymous_Types                    | 1000    |   6,594.0 μs |    149.66 μs |    438.93 μs |   6,542.8 μs |  1.07 |    0.10 |    78.1250 |         - |         - |   695.54 KB |        0.94 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   6,084.2 μs |    130.49 μs |    384.77 μs |   6,047.0 μs |  0.99 |    0.08 |    85.9375 |         - |         - |   764.74 KB |        1.03 |
| Norm_Named_Tuples                       | 1000    |   6,090.3 μs |    129.31 μs |    375.14 μs |   6,065.7 μs |  0.99 |    0.09 |    85.9375 |         - |         - |   764.54 KB |        1.03 |
| Norm_NameValue_Array                    | 1000    |   5,339.5 μs |    127.44 μs |    373.75 μs |   5,291.7 μs |  0.87 |    0.08 |    78.1250 |         - |         - |   686.78 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,954.2 μs |    124.99 μs |    368.54 μs |   5,935.0 μs |  0.97 |    0.08 |    78.1250 |         - |         - |   672.96 KB |        0.91 |
| Norm_PocoClass_Instances                | 1000    |   5,821.9 μs |    139.85 μs |    405.72 μs |   5,800.7 μs |  0.95 |    0.08 |    70.3125 |         - |         - |   594.84 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 1000    |   5,975.2 μs |    118.69 μs |    336.70 μs |   5,979.2 μs |  0.97 |    0.08 |    23.4375 |         - |         - |   232.51 KB |        0.31 |
| Norm_Tuples                             | 1000    |   5,829.5 μs |    161.93 μs |    474.91 μs |   5,868.2 μs |  0.95 |    0.10 |    15.6250 |         - |         - |   209.06 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **10000**   |  **38,436.2 μs** |    **881.81 μs** |  **2,530.07 μs** |  **38,171.4 μs** |  **0.66** |    **0.06** |   **357.1429** |         **-** |         **-** |  **2970.36 KB** |        **0.39** |
| Dapper_Buffered_False                   | 10000   |  43,221.2 μs |    861.91 μs |  2,270.62 μs |  43,108.5 μs |  0.74 |    0.05 |   833.3333 |         - |         - |  7264.03 KB |        0.97 |
| Dapper                                  | 10000   |  58,674.0 μs |  1,154.84 μs |  2,722.10 μs |  58,974.4 μs |  1.00 |    0.00 |   888.8889 |  555.5556 |  222.2222 |   7521.1 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  47,979.2 μs |    958.04 μs |  2,332.00 μs |  47,830.5 μs |  0.82 |    0.06 |   500.0000 |         - |         - |  4854.78 KB |        0.65 |
| Norm_Anonymous_Types                    | 10000   |  48,940.2 μs |  1,469.92 μs |  4,311.02 μs |  48,272.6 μs |  0.84 |    0.09 |   750.0000 |         - |         - |  6957.48 KB |        0.93 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  44,583.9 μs |    888.83 μs |  2,112.40 μs |  44,363.6 μs |  0.76 |    0.04 |   916.6667 |         - |         - |   7657.1 KB |        1.02 |
| Norm_Named_Tuples                       | 10000   |  45,144.3 μs |    959.04 μs |  2,782.34 μs |  44,962.2 μs |  0.78 |    0.06 |   900.0000 |         - |         - |  7657.48 KB |        1.02 |
| Norm_NameValue_Array                    | 10000   |  41,034.2 μs |    870.12 μs |  2,551.91 μs |  40,672.7 μs |  0.70 |    0.05 |   833.3333 |         - |         - |     6876 KB |        0.91 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  46,033.3 μs |    916.51 μs |  2,299.34 μs |  45,725.6 μs |  0.78 |    0.05 |   750.0000 |         - |         - |  6722.17 KB |        0.89 |
| Norm_PocoClass_Instances                | 10000   |  44,340.5 μs |    878.53 μs |  1,650.09 μs |  44,338.8 μs |  0.76 |    0.05 |   666.6667 |         - |         - |  5940.01 KB |        0.79 |
| Norm_Tuples_ReaderCallback              | 10000   |  48,033.2 μs |    950.97 μs |  2,682.22 μs |  47,743.2 μs |  0.82 |    0.07 |   272.7273 |         - |         - |  2343.13 KB |        0.31 |
| Norm_Tuples                             | 10000   |  48,177.7 μs |  1,344.34 μs |  3,921.52 μs |  48,097.8 μs |  0.82 |    0.08 |   250.0000 |         - |         - |  2108.04 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100000**  | **359,674.2 μs** |  **9,994.94 μs** | **29,470.31 μs** | **362,255.8 μs** |  **0.64** |    **0.05** |  **3000.0000** |         **-** |         **-** | **30395.05 KB** |        **0.40** |
| Dapper_Buffered_False                   | 100000  | 398,751.4 μs | 12,587.55 μs | 36,518.75 μs | 395,857.6 μs |  0.69 |    0.07 |  8000.0000 |         - |         - | 73358.65 KB |        0.97 |
| Dapper                                  | 100000  | 578,149.0 μs | 11,524.17 μs | 24,806.98 μs | 577,925.2 μs |  1.00 |    0.00 | 10000.0000 | 5000.0000 | 2000.0000 | 75409.66 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 404,648.0 μs |  8,060.56 μs | 21,234.73 μs | 398,263.5 μs |  0.70 |    0.04 |  6000.0000 |         - |         - | 49150.92 KB |        0.65 |
| Norm_Anonymous_Types                    | 100000  | 432,315.6 μs | 10,213.20 μs | 29,467.43 μs | 427,846.4 μs |  0.75 |    0.07 |  8000.0000 |         - |         - | 70240.87 KB |        0.93 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 407,369.0 μs | 12,974.87 μs | 37,848.29 μs | 402,257.3 μs |  0.70 |    0.06 |  9000.0000 |         - |         - | 77267.18 KB |        1.02 |
| Norm_Named_Tuples                       | 100000  | 396,422.1 μs |  9,692.11 μs | 27,963.96 μs | 393,970.1 μs |  0.69 |    0.06 |  9000.0000 |         - |         - | 77266.66 KB |        1.02 |
| Norm_NameValue_Array                    | 100000  | 384,060.6 μs | 14,389.02 μs | 42,200.50 μs | 383,714.6 μs |  0.66 |    0.09 |  8000.0000 |         - |         - | 69457.09 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 390,101.5 μs | 12,022.58 μs | 35,070.43 μs | 382,506.8 μs |  0.67 |    0.06 |  8000.0000 |         - |         - | 67899.63 KB |        0.90 |
| Norm_PocoClass_Instances                | 100000  | 411,144.2 μs | 11,408.31 μs | 32,732.57 μs | 406,436.4 μs |  0.72 |    0.07 |  7000.0000 |         - |         - | 60081.19 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 100000  | 401,123.2 μs | 12,564.04 μs | 37,045.35 μs | 392,164.8 μs |  0.71 |    0.07 |  2000.0000 |         - |         - |  24140.4 KB |        0.32 |
| Norm_Tuples                             | 100000  | 399,769.8 μs | 10,745.48 μs | 31,174.60 μs | 393,917.8 μs |  0.69 |    0.06 |  2000.0000 |         - |         - | 21797.02 KB |        0.29 |

#### Round 5

```

BenchmarkDotNet v0.13.10, Debian GNU/Linux 12 (bookworm) (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                  | Records | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
| **Command_Reader**                          | **10**      |     **526.3 μs** |     **15.29 μs** |     **44.59 μs** |     **520.2 μs** |  **0.84** |    **0.09** |          **-** |         **-** |         **-** |     **3.56 KB** |        **0.43** |
| Dapper_Buffered_False                   | 10      |     641.4 μs |     12.69 μs |     33.21 μs |     636.4 μs |  1.03 |    0.06 |          - |         - |         - |     7.92 KB |        0.96 |
| Dapper                                  | 10      |     629.5 μs |     12.45 μs |     28.85 μs |     625.4 μs |  1.00 |    0.00 |     0.9766 |         - |         - |     8.27 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     730.2 μs |     15.46 μs |     44.35 μs |     725.3 μs |  1.16 |    0.09 |     1.9531 |         - |         - |    16.25 KB |        1.97 |
| Norm_Anonymous_Types                    | 10      |     540.2 μs |     12.98 μs |     37.87 μs |     531.9 μs |  0.86 |    0.06 |     0.9766 |         - |         - |     9.77 KB |        1.18 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     530.4 μs |     10.57 μs |     30.17 μs |     526.0 μs |  0.84 |    0.06 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_Named_Tuples                       | 10      |     533.5 μs |     11.97 μs |     34.73 μs |     529.3 μs |  0.85 |    0.07 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_NameValue_Array                    | 10      |     530.6 μs |     14.15 μs |     41.06 μs |     525.7 μs |  0.84 |    0.07 |     0.9766 |         - |         - |     8.29 KB |        1.00 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     570.4 μs |     10.99 μs |     28.95 μs |     570.7 μs |  0.90 |    0.07 |     0.9766 |         - |         - |     10.3 KB |        1.25 |
| Norm_PocoClass_Instances                | 10      |     557.3 μs |     11.08 μs |     30.90 μs |     550.5 μs |  0.89 |    0.07 |     0.9766 |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples_ReaderCallback              | 10      |     523.4 μs |     11.78 μs |     34.37 μs |     517.6 μs |  0.83 |    0.07 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Tuples                             | 10      |     531.9 μs |     12.07 μs |     35.21 μs |     528.3 μs |  0.85 |    0.07 |          - |         - |         - |      2.9 KB |        0.35 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100**     |   **1,073.3 μs** |     **27.12 μs** |     **79.10 μs** |   **1,061.6 μs** |  **0.92** |    **0.10** |     **1.9531** |         **-** |         **-** |    **27.58 KB** |        **0.38** |
| Dapper_Buffered_False                   | 100     |   1,197.3 μs |     37.40 μs |    109.69 μs |   1,182.9 μs |  1.03 |    0.12 |     7.8125 |         - |         - |    70.54 KB |        0.97 |
| Dapper                                  | 100     |   1,170.4 μs |     28.45 μs |     82.99 μs |   1,164.8 μs |  1.00 |    0.00 |     7.8125 |         - |         - |    72.68 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100     |   1,226.4 μs |     28.52 μs |     82.29 μs |   1,214.4 μs |  1.05 |    0.11 |     3.9063 |         - |         - |    57.03 KB |        0.78 |
| Norm_Anonymous_Types                    | 100     |   1,145.9 μs |     28.84 μs |     84.57 μs |   1,143.7 μs |  0.98 |    0.10 |     7.8125 |         - |         - |    69.63 KB |        0.96 |
| Norm_Named_Tuples_ReaderCallback        | 100     |   1,100.8 μs |     26.22 μs |     76.47 μs |   1,094.0 μs |  0.95 |    0.10 |     7.8125 |         - |         - |    75.31 KB |        1.04 |
| Norm_Named_Tuples                       | 100     |   1,117.8 μs |     35.34 μs |    102.52 μs |   1,106.3 μs |  0.96 |    0.11 |     7.8125 |         - |         - |    75.32 KB |        1.04 |
| Norm_NameValue_Array                    | 100     |   1,098.9 μs |     28.47 μs |     83.50 μs |   1,086.8 μs |  0.94 |    0.10 |     7.8125 |         - |         - |    67.44 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100     |   1,149.2 μs |     37.75 μs |    110.71 μs |   1,139.6 μs |  0.99 |    0.12 |     7.8125 |         - |         - |    68.01 KB |        0.94 |
| Norm_PocoClass_Instances                | 100     |   1,192.4 μs |     33.49 μs |     97.70 μs |   1,185.7 μs |  1.02 |    0.10 |     5.8594 |         - |         - |    60.32 KB |        0.83 |
| Norm_Tuples_ReaderCallback              | 100     |   1,191.4 μs |     30.61 μs |     89.29 μs |   1,190.8 μs |  1.02 |    0.11 |     1.9531 |         - |         - |    21.49 KB |        0.30 |
| Norm_Tuples                             | 100     |   1,198.1 μs |     27.31 μs |     79.24 μs |   1,208.2 μs |  1.03 |    0.10 |     1.9531 |         - |         - |    19.17 KB |        0.26 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **1000**    |   **5,034.8 μs** |    **117.71 μs** |    **345.22 μs** |   **5,038.4 μs** |  **0.91** |    **0.09** |    **31.2500** |         **-** |         **-** |   **295.65 KB** |        **0.40** |
| Dapper_Buffered_False                   | 1000    |   5,450.3 μs |    127.21 μs |    373.10 μs |   5,437.0 μs |  0.98 |    0.09 |    85.9375 |         - |         - |   724.81 KB |        0.98 |
| Dapper                                  | 1000    |   5,584.5 μs |    137.14 μs |    404.36 μs |   5,605.7 μs |  1.00 |    0.00 |    85.9375 |   23.4375 |         - |    740.9 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   6,108.4 μs |    189.97 μs |    551.14 μs |   6,037.8 μs |  1.10 |    0.13 |    31.2500 |         - |         - |   493.03 KB |        0.67 |
| Norm_Anonymous_Types                    | 1000    |   6,194.7 μs |    149.13 μs |    439.72 μs |   6,138.1 μs |  1.12 |    0.12 |    78.1250 |         - |         - |   695.46 KB |        0.94 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   5,634.6 μs |    128.42 μs |    372.57 μs |   5,603.4 μs |  1.01 |    0.10 |    85.9375 |         - |         - |    764.6 KB |        1.03 |
| Norm_Named_Tuples                       | 1000    |   5,564.5 μs |    133.72 μs |    385.82 μs |   5,589.2 μs |  1.00 |    0.09 |    85.9375 |         - |         - |   764.63 KB |        1.03 |
| Norm_NameValue_Array                    | 1000    |   5,309.5 μs |    119.51 μs |    344.83 μs |   5,330.1 μs |  0.96 |    0.09 |    78.1250 |         - |         - |    686.9 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,964.5 μs |    207.29 μs |    604.69 μs |   5,927.9 μs |  1.07 |    0.14 |    62.5000 |         - |         - |   672.92 KB |        0.91 |
| Norm_PocoClass_Instances                | 1000    |   5,841.8 μs |    186.61 μs |    550.23 μs |   5,819.4 μs |  1.05 |    0.13 |    70.3125 |         - |         - |   594.81 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 1000    |   6,639.8 μs |    196.94 μs |    577.58 μs |   6,699.1 μs |  1.19 |    0.13 |    15.6250 |         - |         - |    232.5 KB |        0.31 |
| Norm_Tuples                             | 1000    |   6,590.3 μs |    185.83 μs |    547.91 μs |   6,630.3 μs |  1.19 |    0.12 |    23.4375 |         - |         - |   209.03 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **10000**   |  **42,847.0 μs** |  **1,449.27 μs** |  **4,227.60 μs** |  **41,311.2 μs** |  **0.70** |    **0.08** |   **333.3333** |         **-** |         **-** |  **2969.96 KB** |        **0.39** |
| Dapper_Buffered_False                   | 10000   |  45,589.5 μs |  1,068.79 μs |  3,117.72 μs |  45,839.6 μs |  0.75 |    0.07 |   800.0000 |         - |         - |  7264.05 KB |        0.97 |
| Dapper                                  | 10000   |  61,134.9 μs |  1,334.88 μs |  3,851.42 μs |  60,656.5 μs |  1.00 |    0.00 |   875.0000 |  500.0000 |  250.0000 |   7520.6 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  51,986.0 μs |  1,581.31 μs |  4,637.70 μs |  51,606.4 μs |  0.85 |    0.09 |   555.5556 |         - |         - |  4855.92 KB |        0.65 |
| Norm_Anonymous_Types                    | 10000   |  51,557.8 μs |  1,380.84 μs |  4,071.43 μs |  51,302.5 μs |  0.85 |    0.08 |   818.1818 |         - |         - |  6954.64 KB |        0.92 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  47,377.2 μs |  1,190.96 μs |  3,492.87 μs |  46,780.2 μs |  0.78 |    0.07 |   916.6667 |         - |         - |  7655.88 KB |        1.02 |
| Norm_Named_Tuples                       | 10000   |  47,648.4 μs |  1,431.72 μs |  4,221.46 μs |  46,841.3 μs |  0.78 |    0.09 |   909.0909 |         - |         - |  7655.76 KB |        1.02 |
| Norm_NameValue_Array                    | 10000   |  42,906.1 μs |  1,055.22 μs |  3,044.54 μs |  42,535.0 μs |  0.70 |    0.07 |   833.3333 |         - |         - |  6876.75 KB |        0.91 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  49,118.1 μs |  1,283.91 μs |  3,745.22 μs |  48,464.3 μs |  0.81 |    0.08 |   818.1818 |         - |         - |  6722.78 KB |        0.89 |
| Norm_PocoClass_Instances                | 10000   |  48,545.4 μs |  1,231.89 μs |  3,612.92 μs |  48,997.8 μs |  0.80 |    0.08 |   700.0000 |         - |         - |  5940.02 KB |        0.79 |
| Norm_Tuples_ReaderCallback              | 10000   |  49,695.8 μs |  1,746.90 μs |  5,095.78 μs |  49,061.9 μs |  0.82 |    0.10 |   250.0000 |         - |         - |  2342.88 KB |        0.31 |
| Norm_Tuples                             | 10000   |  51,634.4 μs |  1,322.77 μs |  3,879.46 μs |  51,104.5 μs |  0.85 |    0.08 |   200.0000 |         - |         - |  2108.18 KB |        0.28 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Command_Reader**                          | **100000**  | **367,371.1 μs** | **13,404.83 μs** | **39,102.52 μs** | **364,920.8 μs** |  **0.61** |    **0.08** |  **3000.0000** |         **-** |         **-** |  **30389.8 KB** |        **0.40** |
| Dapper_Buffered_False                   | 100000  | 415,139.4 μs | 11,557.83 μs | 32,975.15 μs | 420,270.7 μs |  0.69 |    0.08 |  8000.0000 |         - |         - | 73359.56 KB |        0.97 |
| Dapper                                  | 100000  | 601,452.6 μs | 15,229.75 μs | 44,666.22 μs | 588,319.7 μs |  1.00 |    0.00 | 10000.0000 | 5000.0000 | 2000.0000 | 75412.97 KB |        1.00 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 426,235.9 μs | 13,262.73 μs | 38,266.02 μs | 416,885.2 μs |  0.71 |    0.08 |  6000.0000 |         - |         - | 49150.92 KB |        0.65 |
| Norm_Anonymous_Types                    | 100000  | 419,724.8 μs | 11,028.45 μs | 31,819.61 μs | 413,269.3 μs |  0.70 |    0.07 |  8000.0000 |         - |         - | 70237.87 KB |        0.93 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 417,967.8 μs | 12,610.13 μs | 36,584.28 μs | 411,899.0 μs |  0.70 |    0.08 |  9000.0000 |         - |         - | 77271.73 KB |        1.02 |
| Norm_Named_Tuples                       | 100000  | 426,983.9 μs | 14,398.10 μs | 41,541.82 μs | 425,401.9 μs |  0.71 |    0.09 |  9000.0000 |         - |         - |  77275.1 KB |        1.02 |
| Norm_NameValue_Array                    | 100000  | 396,666.1 μs | 14,071.33 μs | 41,046.72 μs | 397,882.4 μs |  0.66 |    0.09 |  8000.0000 |         - |         - | 69492.31 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 421,287.6 μs | 12,009.39 μs | 34,649.82 μs | 416,715.8 μs |  0.70 |    0.08 |  8000.0000 |         - |         - | 67895.69 KB |        0.90 |
| Norm_PocoClass_Instances                | 100000  | 447,736.0 μs | 17,941.68 μs | 52,901.45 μs | 444,205.4 μs |  0.75 |    0.11 |  7000.0000 |         - |         - | 60087.19 KB |        0.80 |
| Norm_Tuples_ReaderCallback              | 100000  | 476,570.5 μs | 18,287.74 μs | 53,921.82 μs | 468,322.6 μs |  0.80 |    0.11 |  2000.0000 |         - |         - | 24149.59 KB |        0.32 |
| Norm_Tuples                             | 100000  | 442,534.9 μs | 16,208.38 μs | 47,790.78 μs | 439,318.3 μs |  0.74 |    0.09 |  2000.0000 |         - |         - | 21800.21 KB |        0.29 |
