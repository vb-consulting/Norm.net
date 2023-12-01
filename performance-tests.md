# PERFOMANCE BENCHMARKS

Norm version: 5.4.0.0
Dapper version: 2.0.0.0
EntityFrameworkCore version: 8.0.0.0
.NET SDK 8.0.100

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
| **Dapper**                                  | **10**      |     **639.7 μs** |     **14.71 μs** |     **42.67 μs** |     **631.4 μs** |  **1.00** |    **0.00** |     **0.9766** |         **-** |         **-** |     **8.26 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10      |     628.5 μs |     12.68 μs |     36.58 μs |     624.3 μs |  0.99 |    0.08 |          - |         - |         - |     7.89 KB |        0.96 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     707.2 μs |     14.41 μs |     41.57 μs |     704.6 μs |  1.11 |    0.10 |     1.9531 |         - |         - |     16.2 KB |        1.96 |
| Norm_NameValue_Array                    | 10      |     512.8 μs |     12.07 μs |     35.02 μs |     509.0 μs |  0.81 |    0.08 |     0.9766 |         - |         - |     8.29 KB |        1.00 |
| Norm_PocoClass_Instances                | 10      |     558.8 μs |     13.87 μs |     40.23 μs |     548.5 μs |  0.88 |    0.08 |     0.9766 |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples                             | 10      |     526.5 μs |     14.00 μs |     40.83 μs |     517.9 μs |  0.83 |    0.08 |          - |         - |         - |      2.9 KB |        0.35 |
| Norm_Named_Tuples                       | 10      |     518.8 μs |     11.95 μs |     34.48 μs |     514.0 μs |  0.82 |    0.08 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_Anonymous_Types                    | 10      |     544.5 μs |     16.23 μs |     47.60 μs |     540.2 μs |  0.86 |    0.09 |          - |         - |         - |     9.78 KB |        1.18 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     557.5 μs |     11.74 μs |     34.07 μs |     558.7 μs |  0.88 |    0.08 |     0.9766 |         - |         - |    10.29 KB |        1.25 |
| Norm_Tuples_ReaderCallback              | 10      |     515.7 μs |     11.22 μs |     32.36 μs |     511.2 μs |  0.81 |    0.07 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     516.6 μs |     10.97 μs |     31.65 μs |     514.9 μs |  0.81 |    0.07 |          - |         - |         - |     9.15 KB |        1.11 |
| Command_Reader                          | 10      |     495.0 μs |     10.28 μs |     29.82 μs |     492.7 μs |  0.78 |    0.07 |          - |         - |         - |     3.56 KB |        0.43 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **1000**    |   **5,423.3 μs** |    **125.09 μs** |    **368.83 μs** |   **5,391.2 μs** |  **1.00** |    **0.00** |    **85.9375** |   **23.4375** |         **-** |   **740.57 KB** |        **1.00** |
| Dapper_Buffered_False                   | 1000    |   5,500.2 μs |    113.02 μs |    324.27 μs |   5,504.6 μs |  1.02 |    0.09 |    85.9375 |         - |         - |   724.71 KB |        0.98 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   6,237.7 μs |    180.34 μs |    531.75 μs |   6,205.5 μs |  1.15 |    0.12 |    46.8750 |         - |         - |   493.03 KB |        0.67 |
| Norm_NameValue_Array                    | 1000    |   5,383.2 μs |    113.10 μs |    331.71 μs |   5,310.5 μs |  1.00 |    0.09 |    78.1250 |         - |         - |   686.64 KB |        0.93 |
| Norm_PocoClass_Instances                | 1000    |   5,679.0 μs |    122.86 μs |    356.44 μs |   5,670.9 μs |  1.05 |    0.09 |    70.3125 |         - |         - |   594.99 KB |        0.80 |
| Norm_Tuples                             | 1000    |   5,911.4 μs |    140.66 μs |    414.74 μs |   5,920.1 μs |  1.10 |    0.11 |    23.4375 |         - |         - |   209.09 KB |        0.28 |
| Norm_Named_Tuples                       | 1000    |   5,679.5 μs |    128.81 μs |    373.70 μs |   5,671.5 μs |  1.05 |    0.10 |    85.9375 |         - |         - |   764.59 KB |        1.03 |
| Norm_Anonymous_Types                    | 1000    |   5,941.0 μs |    117.62 μs |    335.58 μs |   5,963.5 μs |  1.10 |    0.10 |    78.1250 |         - |         - |   695.52 KB |        0.94 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,915.6 μs |    130.20 μs |    381.85 μs |   5,900.7 μs |  1.09 |    0.10 |    78.1250 |         - |         - |   672.94 KB |        0.91 |
| Norm_Tuples_ReaderCallback              | 1000    |   5,906.6 μs |    117.62 μs |    279.54 μs |   5,878.8 μs |  1.08 |    0.08 |    23.4375 |         - |         - |   232.56 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   5,382.4 μs |    139.10 μs |    401.33 μs |   5,334.3 μs |  0.99 |    0.09 |    85.9375 |         - |         - |   764.57 KB |        1.03 |
| Command_Reader                          | 1000    |   5,034.1 μs |    116.80 μs |    340.70 μs |   5,035.9 μs |  0.93 |    0.08 |    31.2500 |         - |         - |   295.39 KB |        0.40 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **10000**   |  **58,851.2 μs** |  **1,170.56 μs** |  **3,083.71 μs** |  **58,303.6 μs** |  **1.00** |    **0.00** |   **888.8889** |  **555.5556** |  **222.2222** |  **7520.34 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10000   |  40,917.8 μs |    794.79 μs |  2,008.52 μs |  40,512.5 μs |  0.70 |    0.05 |   833.3333 |         - |         - |  7264.62 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  46,829.8 μs |    931.79 μs |  2,159.57 μs |  46,687.0 μs |  0.80 |    0.06 |   545.4545 |         - |         - |  4855.65 KB |        0.65 |
| Norm_NameValue_Array                    | 10000   |  40,645.9 μs |    804.78 μs |  2,283.03 μs |  40,407.3 μs |  0.70 |    0.05 |   785.7143 |         - |         - |  6875.33 KB |        0.91 |
| Norm_PocoClass_Instances                | 10000   |  44,330.7 μs |    879.12 μs |  2,536.47 μs |  43,882.0 μs |  0.76 |    0.06 |   666.6667 |         - |         - |  5940.65 KB |        0.79 |
| Norm_Tuples                             | 10000   |  47,527.9 μs |    943.30 μs |  2,721.63 μs |  47,133.4 μs |  0.81 |    0.07 |   200.0000 |         - |         - |  2109.96 KB |        0.28 |
| Norm_Named_Tuples                       | 10000   |  44,481.9 μs |    885.35 μs |  2,453.30 μs |  44,166.5 μs |  0.76 |    0.06 |   900.0000 |         - |         - |   7657.8 KB |        1.02 |
| Norm_Anonymous_Types                    | 10000   |  48,383.6 μs |    967.57 μs |  2,713.16 μs |  47,602.5 μs |  0.83 |    0.06 |   818.1818 |         - |         - |  6954.98 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  45,489.3 μs |    949.16 μs |  2,768.74 μs |  45,163.6 μs |  0.77 |    0.06 |   750.0000 |         - |         - |  6721.42 KB |        0.89 |
| Norm_Tuples_ReaderCallback              | 10000   |  46,553.7 μs |    959.16 μs |  2,813.06 μs |  46,260.0 μs |  0.80 |    0.06 |   272.7273 |         - |         - |  2342.84 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  43,128.2 μs |    860.34 μs |  2,340.62 μs |  42,709.4 μs |  0.73 |    0.06 |   909.0909 |         - |         - |  7655.82 KB |        1.02 |
| Command_Reader                          | 10000   |  37,130.9 μs |    788.72 μs |  2,300.74 μs |  37,130.0 μs |  0.63 |    0.06 |   357.1429 |         - |         - |  2972.29 KB |        0.40 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **100000**  | **552,605.7 μs** | **10,953.86 μs** | **24,273.00 μs** | **546,726.1 μs** |  **1.00** |    **0.00** | **10000.0000** | **5000.0000** | **2000.0000** |  **75409.6 KB** |        **1.00** |
| Dapper_Buffered_False                   | 100000  | 394,465.7 μs | 12,332.04 μs | 36,167.74 μs | 391,702.2 μs |  0.71 |    0.07 |  8000.0000 |         - |         - | 73358.65 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 426,856.1 μs | 11,049.26 μs | 32,055.92 μs | 422,545.4 μs |  0.78 |    0.06 |  6000.0000 |         - |         - | 49150.92 KB |        0.65 |
| Norm_NameValue_Array                    | 100000  | 373,566.0 μs | 11,720.82 μs | 34,559.10 μs | 369,428.5 μs |  0.68 |    0.07 |  8000.0000 |         - |         - | 69496.16 KB |        0.92 |
| Norm_PocoClass_Instances                | 100000  | 382,996.1 μs |  9,381.33 μs | 27,067.27 μs | 377,155.3 μs |  0.69 |    0.05 |  7000.0000 |         - |         - | 60081.75 KB |        0.80 |
| Norm_Tuples                             | 100000  | 422,511.1 μs | 12,006.41 μs | 35,401.16 μs | 418,459.5 μs |  0.76 |    0.08 |  2000.0000 |         - |         - | 21817.65 KB |        0.29 |
| Norm_Named_Tuples                       | 100000  | 402,126.4 μs | 13,455.89 μs | 39,674.99 μs | 395,413.2 μs |  0.73 |    0.07 |  9000.0000 |         - |         - | 77267.04 KB |        1.02 |
| Norm_Anonymous_Types                    | 100000  | 395,228.9 μs | 10,278.68 μs | 29,983.38 μs | 389,971.9 μs |  0.72 |    0.06 |  8000.0000 |         - |         - | 70242.55 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 385,371.0 μs | 10,468.77 μs | 30,204.79 μs | 380,155.5 μs |  0.70 |    0.07 |  8000.0000 |         - |         - | 67908.06 KB |        0.90 |
| Norm_Tuples_ReaderCallback              | 100000  | 411,576.1 μs | 12,899.51 μs | 37,628.49 μs | 405,998.3 μs |  0.73 |    0.08 |  2000.0000 |         - |         - | 24143.21 KB |        0.32 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 386,632.2 μs | 10,411.95 μs | 30,372.13 μs | 382,153.4 μs |  0.71 |    0.06 |  9000.0000 |         - |         - | 77276.04 KB |        1.02 |
| Command_Reader                          | 100000  | 339,296.5 μs | 11,778.26 μs | 34,543.60 μs | 335,112.3 μs |  0.62 |    0.07 |  3000.0000 |         - |         - |  30389.8 KB |        0.40 |

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
| **Dapper**                                  | **10**      |     **603.8 μs** |     **11.42 μs** |     **28.87 μs** |     **600.2 μs** |  **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **8.27 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10      |     609.9 μs |     12.11 μs |     26.07 μs |     606.7 μs |  1.01 |    0.06 |          - |         - |         - |     7.88 KB |        0.95 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     695.6 μs |     16.99 μs |     49.82 μs |     690.9 μs |  1.16 |    0.10 |     1.9531 |         - |         - |    16.26 KB |        1.97 |
| Norm_NameValue_Array                    | 10      |     501.3 μs |     10.67 μs |     31.29 μs |     491.2 μs |  0.84 |    0.06 |     0.9766 |         - |         - |     8.29 KB |        1.00 |
| Norm_PocoClass_Instances                | 10      |     557.2 μs |     14.58 μs |     42.75 μs |     543.5 μs |  0.93 |    0.09 |     0.9766 |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples                             | 10      |     536.4 μs |     11.46 μs |     33.43 μs |     541.1 μs |  0.89 |    0.06 |          - |         - |         - |      2.9 KB |        0.35 |
| Norm_Named_Tuples                       | 10      |     526.4 μs |     11.71 μs |     34.16 μs |     521.5 μs |  0.87 |    0.07 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_Anonymous_Types                    | 10      |     529.1 μs |     11.16 μs |     32.03 μs |     522.1 μs |  0.87 |    0.06 |     0.9766 |         - |         - |     9.77 KB |        1.18 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     555.8 μs |     15.35 μs |     44.78 μs |     547.7 μs |  0.92 |    0.08 |          - |         - |         - |     10.3 KB |        1.25 |
| Norm_Tuples_ReaderCallback              | 10      |     524.5 μs |     12.61 μs |     36.78 μs |     521.1 μs |  0.87 |    0.08 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     514.1 μs |     10.23 μs |     26.95 μs |     517.9 μs |  0.85 |    0.06 |     0.9766 |         - |         - |     9.14 KB |        1.11 |
| Command_Reader                          | 10      |     490.5 μs |      9.97 μs |     28.45 μs |     485.0 μs |  0.81 |    0.06 |          - |         - |         - |     3.56 KB |        0.43 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **1000**    |   **5,648.4 μs** |    **111.92 μs** |    **276.65 μs** |   **5,658.3 μs** |  **1.00** |    **0.00** |    **85.9375** |   **23.4375** |         **-** |   **740.93 KB** |        **1.00** |
| Dapper_Buffered_False                   | 1000    |   5,392.0 μs |    131.53 μs |    385.75 μs |   5,358.9 μs |  0.96 |    0.08 |    85.9375 |         - |         - |   724.64 KB |        0.98 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   5,815.1 μs |    135.32 μs |    394.72 μs |   5,762.7 μs |  1.03 |    0.09 |    46.8750 |         - |         - |   493.08 KB |        0.67 |
| Norm_NameValue_Array                    | 1000    |   5,163.4 μs |    123.46 μs |    362.10 μs |   5,115.8 μs |  0.91 |    0.08 |    78.1250 |         - |         - |   686.53 KB |        0.93 |
| Norm_PocoClass_Instances                | 1000    |   5,569.4 μs |    153.73 μs |    450.85 μs |   5,535.4 μs |  0.97 |    0.09 |    70.3125 |         - |         - |   594.85 KB |        0.80 |
| Norm_Tuples                             | 1000    |   5,926.5 μs |    143.31 μs |    420.29 μs |   5,911.5 μs |  1.05 |    0.08 |    23.4375 |         - |         - |   209.08 KB |        0.28 |
| Norm_Named_Tuples                       | 1000    |   5,839.0 μs |    122.76 μs |    361.96 μs |   5,847.6 μs |  1.03 |    0.08 |    85.9375 |         - |         - |   764.56 KB |        1.03 |
| Norm_Anonymous_Types                    | 1000    |   6,128.8 μs |    139.46 μs |    411.20 μs |   6,094.8 μs |  1.08 |    0.10 |    78.1250 |         - |         - |   695.54 KB |        0.94 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,932.8 μs |    133.91 μs |    392.72 μs |   5,925.3 μs |  1.05 |    0.09 |    78.1250 |         - |         - |   672.89 KB |        0.91 |
| Norm_Tuples_ReaderCallback              | 1000    |   6,026.4 μs |    147.93 μs |    431.51 μs |   5,992.3 μs |  1.07 |    0.10 |    23.4375 |         - |         - |   232.51 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   5,711.6 μs |    134.51 μs |    394.50 μs |   5,739.0 μs |  1.02 |    0.09 |    85.9375 |         - |         - |   764.56 KB |        1.03 |
| Command_Reader                          | 1000    |   5,040.9 μs |    108.61 μs |    315.11 μs |   5,037.1 μs |  0.90 |    0.08 |    31.2500 |         - |         - |   295.56 KB |        0.40 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **10000**   |  **57,642.8 μs** |  **1,189.96 μs** |  **3,433.32 μs** |  **57,150.3 μs** |  **1.00** |    **0.00** |   **875.0000** |  **500.0000** |  **250.0000** |  **7520.81 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10000   |  42,711.8 μs |  1,061.42 μs |  3,028.28 μs |  42,025.2 μs |  0.74 |    0.07 |   800.0000 |         - |         - |  7263.87 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  48,379.3 μs |    963.35 μs |  2,363.11 μs |  48,415.4 μs |  0.84 |    0.06 |   545.4545 |         - |         - |  4853.67 KB |        0.65 |
| Norm_NameValue_Array                    | 10000   |  40,909.0 μs |    812.82 μs |  2,169.57 μs |  40,694.6 μs |  0.71 |    0.05 |   833.3333 |         - |         - |  6875.91 KB |        0.91 |
| Norm_PocoClass_Instances                | 10000   |  45,154.4 μs |    899.65 μs |  2,273.53 μs |  44,720.4 μs |  0.78 |    0.06 |   666.6667 |         - |         - |  5939.85 KB |        0.79 |
| Norm_Tuples                             | 10000   |  46,279.2 μs |    912.06 μs |  1,923.85 μs |  45,983.8 μs |  0.81 |    0.05 |   200.0000 |         - |         - |  2110.28 KB |        0.28 |
| Norm_Named_Tuples                       | 10000   |  45,122.0 μs |    987.13 μs |  2,879.49 μs |  44,691.9 μs |  0.78 |    0.07 |   900.0000 |         - |         - |  7655.79 KB |        1.02 |
| Norm_Anonymous_Types                    | 10000   |  48,255.9 μs |  1,002.27 μs |  2,907.76 μs |  47,776.0 μs |  0.84 |    0.07 |   833.3333 |         - |         - |  6956.58 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  45,801.2 μs |    881.80 μs |  2,487.12 μs |  45,503.2 μs |  0.80 |    0.07 |   800.0000 |         - |         - |   6723.9 KB |        0.89 |
| Norm_Tuples_ReaderCallback              | 10000   |  45,016.6 μs |    891.85 μs |    790.60 μs |  45,020.8 μs |  0.80 |    0.05 |   272.7273 |         - |         - |  2342.96 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  42,796.1 μs |    850.48 μs |  2,225.55 μs |  42,468.5 μs |  0.74 |    0.05 |   916.6667 |         - |         - |  7655.87 KB |        1.02 |
| Command_Reader                          | 10000   |  36,444.5 μs |    724.90 μs |  1,818.63 μs |  36,071.7 μs |  0.63 |    0.05 |   333.3333 |         - |         - |  2967.71 KB |        0.39 |
|                                         |         |              |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **100000**  | **565,775.6 μs** | **11,137.67 μs** | **18,608.54 μs** | **566,808.5 μs** |  **1.00** |    **0.00** | **10000.0000** | **5000.0000** | **2000.0000** | **75409.98 KB** |        **1.00** |
| Dapper_Buffered_False                   | 100000  | 377,739.9 μs | 10,720.52 μs | 31,272.25 μs | 370,433.8 μs |  0.67 |    0.06 |  8000.0000 |         - |         - | 73358.65 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 409,886.1 μs |  8,422.90 μs | 23,756.93 μs | 406,357.6 μs |  0.73 |    0.06 |  6000.0000 |         - |         - | 49150.92 KB |        0.65 |
| Norm_NameValue_Array                    | 100000  | 384,842.3 μs | 12,571.61 μs | 36,870.36 μs | 388,473.7 μs |  0.69 |    0.06 |  8000.0000 |         - |         - | 69453.34 KB |        0.92 |
| Norm_PocoClass_Instances                | 100000  | 393,516.5 μs | 12,287.33 μs | 36,036.60 μs | 387,514.2 μs |  0.68 |    0.05 |  7000.0000 |         - |         - | 60083.06 KB |        0.80 |
| Norm_Tuples                             | 100000  | 394,831.0 μs | 10,067.94 μs | 29,368.66 μs | 392,216.9 μs |  0.69 |    0.06 |  2000.0000 |         - |         - | 21801.88 KB |        0.29 |
| Norm_Named_Tuples                       | 100000  | 408,759.5 μs | 13,851.77 μs | 40,842.26 μs | 401,102.0 μs |  0.71 |    0.07 |  9000.0000 |         - |         - | 77268.14 KB |        1.02 |
| Norm_Anonymous_Types                    | 100000  | 412,283.2 μs | 10,077.83 μs | 29,397.49 μs | 410,956.1 μs |  0.74 |    0.06 |  8000.0000 |         - |         - | 70235.99 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 394,188.2 μs | 10,193.87 μs | 29,248.13 μs | 392,364.3 μs |  0.70 |    0.05 |  8000.0000 |         - |         - | 67895.31 KB |        0.90 |
| Norm_Tuples_ReaderCallback              | 100000  | 412,371.4 μs | 13,393.06 μs | 39,489.74 μs | 415,893.9 μs |  0.73 |    0.08 |  2000.0000 |         - |         - | 24142.65 KB |        0.32 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 394,198.5 μs | 12,043.19 μs | 34,747.35 μs | 384,753.5 μs |  0.71 |    0.07 |  9000.0000 |         - |         - | 77274.35 KB |        1.02 |
| Command_Reader                          | 100000  | 343,798.5 μs | 11,974.96 μs | 35,308.45 μs | 336,803.0 μs |  0.63 |    0.07 |  3000.0000 |         - |         - | 30442.28 KB |        0.40 |

#### Round 3

```

BenchmarkDotNet v0.13.10, Debian GNU/Linux 12 (bookworm) (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                  | Records | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|-------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
| **Dapper**                                  | **10**      |     **619.6 μs** |     **12.39 μs** |     **28.95 μs** |  **1.00** |    **0.00** |     **0.9766** |         **-** |         **-** |     **8.28 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10      |     626.4 μs |     12.48 μs |     24.92 μs |  1.01 |    0.06 |          - |         - |         - |     7.89 KB |        0.95 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     719.2 μs |     16.04 μs |     46.78 μs |  1.16 |    0.09 |     1.9531 |         - |         - |    16.21 KB |        1.96 |
| Norm_NameValue_Array                    | 10      |     513.6 μs |     10.91 μs |     31.29 μs |  0.83 |    0.06 |     0.9766 |         - |         - |     8.29 KB |        1.00 |
| Norm_PocoClass_Instances                | 10      |     564.6 μs |     11.19 μs |     30.44 μs |  0.91 |    0.06 |     0.9766 |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples                             | 10      |     522.7 μs |     12.44 μs |     36.28 μs |  0.85 |    0.07 |          - |         - |         - |      2.9 KB |        0.35 |
| Norm_Named_Tuples                       | 10      |     523.2 μs |     11.24 μs |     32.78 μs |  0.85 |    0.06 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_Anonymous_Types                    | 10      |     535.8 μs |     11.98 μs |     34.57 μs |  0.87 |    0.07 |     0.9766 |         - |         - |     9.77 KB |        1.18 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     553.5 μs |     12.37 μs |     35.30 μs |  0.90 |    0.07 |          - |         - |         - |     10.3 KB |        1.24 |
| Norm_Tuples_ReaderCallback              | 10      |     518.4 μs |     11.83 μs |     33.95 μs |  0.84 |    0.08 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     528.3 μs |     13.91 μs |     40.58 μs |  0.85 |    0.07 |          - |         - |         - |     9.15 KB |        1.11 |
| Command_Reader                          | 10      |     503.5 μs |     11.68 μs |     34.24 μs |  0.82 |    0.07 |          - |         - |         - |     3.56 KB |        0.43 |
|                                         |         |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **1000**    |   **5,662.8 μs** |    **134.81 μs** |    **397.49 μs** |  **1.00** |    **0.00** |    **85.9375** |   **23.4375** |         **-** |   **740.81 KB** |        **1.00** |
| Dapper_Buffered_False                   | 1000    |   5,574.7 μs |    136.56 μs |    400.52 μs |  0.99 |    0.10 |    85.9375 |         - |         - |   724.68 KB |        0.98 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   6,063.8 μs |    145.39 μs |    424.10 μs |  1.08 |    0.10 |    46.8750 |         - |         - |   493.09 KB |        0.67 |
| Norm_NameValue_Array                    | 1000    |   5,336.6 μs |    129.19 μs |    378.88 μs |  0.95 |    0.10 |    78.1250 |         - |         - |   686.55 KB |        0.93 |
| Norm_PocoClass_Instances                | 1000    |   5,596.5 μs |    149.24 μs |    440.03 μs |  0.99 |    0.10 |    70.3125 |         - |         - |   594.86 KB |        0.80 |
| Norm_Tuples                             | 1000    |   5,560.2 μs |    134.99 μs |    398.02 μs |  0.99 |    0.09 |    23.4375 |         - |         - |   209.09 KB |        0.28 |
| Norm_Named_Tuples                       | 1000    |   5,271.3 μs |    135.67 μs |    400.03 μs |  0.94 |    0.09 |    85.9375 |         - |         - |   764.38 KB |        1.03 |
| Norm_Anonymous_Types                    | 1000    |   5,715.7 μs |    120.64 μs |    353.81 μs |  1.01 |    0.10 |    78.1250 |         - |         - |   695.46 KB |        0.94 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,578.1 μs |    131.42 μs |    385.42 μs |  0.99 |    0.09 |    78.1250 |         - |         - |   672.99 KB |        0.91 |
| Norm_Tuples_ReaderCallback              | 1000    |   5,847.1 μs |    147.58 μs |    432.84 μs |  1.04 |    0.10 |    23.4375 |         - |         - |   232.51 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   5,754.4 μs |    134.40 μs |    396.28 μs |  1.02 |    0.10 |    85.9375 |         - |         - |   764.54 KB |        1.03 |
| Command_Reader                          | 1000    |   5,151.3 μs |    122.96 μs |    360.62 μs |  0.91 |    0.09 |    31.2500 |         - |         - |   295.45 KB |        0.40 |
|                                         |         |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **10000**   |  **59,366.8 μs** |  **1,252.54 μs** |  **3,693.14 μs** |  **1.00** |    **0.00** |   **888.8889** |  **555.5556** |  **222.2222** |  **7523.42 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10000   |  43,085.0 μs |    860.90 μs |  2,282.98 μs |  0.73 |    0.06 |   833.3333 |         - |         - |  7264.46 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  48,504.1 μs |    967.37 μs |  2,696.63 μs |  0.82 |    0.06 |   555.5556 |         - |         - |  4854.72 KB |        0.65 |
| Norm_NameValue_Array                    | 10000   |  41,294.6 μs |    886.80 μs |  2,586.84 μs |  0.70 |    0.06 |   818.1818 |         - |         - |  6874.61 KB |        0.91 |
| Norm_PocoClass_Instances                | 10000   |  45,148.0 μs |    954.95 μs |  2,800.70 μs |  0.76 |    0.06 |   625.0000 |         - |         - |  5941.46 KB |        0.79 |
| Norm_Tuples                             | 10000   |  47,641.5 μs |    948.50 μs |  2,140.92 μs |  0.80 |    0.07 |   200.0000 |         - |         - |  2109.96 KB |        0.28 |
| Norm_Named_Tuples                       | 10000   |  45,055.9 μs |    897.24 μs |  2,530.69 μs |  0.76 |    0.07 |   916.6667 |         - |         - |  7656.21 KB |        1.02 |
| Norm_Anonymous_Types                    | 10000   |  48,978.0 μs |  1,022.63 μs |  2,999.19 μs |  0.83 |    0.08 |   818.1818 |         - |         - |  6953.85 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  45,740.8 μs |    910.17 μs |  2,016.88 μs |  0.77 |    0.06 |   750.0000 |         - |         - |  6721.11 KB |        0.89 |
| Norm_Tuples_ReaderCallback              | 10000   |  49,761.0 μs |  1,645.23 μs |  4,825.16 μs |  0.84 |    0.10 |          - |         - |         - |  2342.31 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  43,528.6 μs |  1,082.86 μs |  3,071.89 μs |  0.74 |    0.06 |   750.0000 |         - |         - |  7662.28 KB |        1.02 |
| Command_Reader                          | 10000   |  37,967.7 μs |    796.16 μs |  2,297.10 μs |  0.64 |    0.06 |   333.3333 |         - |         - |  2968.66 KB |        0.39 |
|                                         |         |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **100000**  | **563,630.2 μs** | **11,137.69 μs** | **26,898.78 μs** |  **1.00** |    **0.00** | **10000.0000** | **5000.0000** | **2000.0000** | **75409.66 KB** |        **1.00** |
| Dapper_Buffered_False                   | 100000  | 405,477.0 μs | 14,984.02 μs | 43,471.36 μs |  0.72 |    0.08 |  8000.0000 |         - |         - | 73358.65 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 426,661.6 μs |  9,893.75 μs | 28,545.74 μs |  0.76 |    0.06 |  6000.0000 |         - |         - | 49160.86 KB |        0.65 |
| Norm_NameValue_Array                    | 100000  | 382,557.4 μs | 14,093.76 μs | 41,112.16 μs |  0.68 |    0.07 |  8000.0000 |         - |         - | 69453.34 KB |        0.92 |
| Norm_PocoClass_Instances                | 100000  | 415,766.6 μs | 13,177.29 μs | 38,646.71 μs |  0.74 |    0.08 |  7000.0000 |         - |         - | 60081.38 KB |        0.80 |
| Norm_Tuples                             | 100000  | 409,934.5 μs | 11,882.45 μs | 35,035.68 μs |  0.72 |    0.06 |  2000.0000 |         - |         - | 21815.02 KB |        0.29 |
| Norm_Named_Tuples                       | 100000  | 394,158.0 μs | 10,851.07 μs | 31,480.93 μs |  0.70 |    0.06 |  9000.0000 |         - |         - | 77265.91 KB |        1.02 |
| Norm_Anonymous_Types                    | 100000  | 410,724.4 μs | 10,578.55 μs | 30,521.52 μs |  0.73 |    0.06 |  8000.0000 |         - |         - | 70247.62 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 405,210.5 μs | 13,808.10 μs | 40,278.88 μs |  0.72 |    0.07 |  8000.0000 |         - |         - | 67895.31 KB |        0.90 |
| Norm_Tuples_ReaderCallback              | 100000  | 418,376.9 μs | 13,423.90 μs | 38,731.03 μs |  0.74 |    0.08 |  2000.0000 |         - |         - | 24140.59 KB |        0.32 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 399,111.3 μs | 11,732.32 μs | 34,223.73 μs |  0.70 |    0.06 |  9000.0000 |         - |         - | 77274.73 KB |        1.02 |
| Command_Reader                          | 100000  | 359,733.4 μs | 10,793.50 μs | 31,655.48 μs |  0.64 |    0.07 |  3000.0000 |         - |         - | 30442.68 KB |        0.40 |

#### Round 4

```

BenchmarkDotNet v0.13.10, Debian GNU/Linux 12 (bookworm) (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                  | Records | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|-------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
| **Dapper**                                  | **10**      |     **621.2 μs** |     **14.11 μs** |     **40.72 μs** |  **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **8.27 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10      |     625.3 μs |     12.46 μs |     33.05 μs |  1.01 |    0.09 |          - |         - |         - |      7.9 KB |        0.95 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     731.7 μs |     19.43 μs |     57.00 μs |  1.18 |    0.11 |     1.9531 |         - |         - |    16.25 KB |        1.97 |
| Norm_NameValue_Array                    | 10      |     516.4 μs |     12.16 μs |     35.08 μs |  0.83 |    0.07 |     0.9766 |         - |         - |     8.29 KB |        1.00 |
| Norm_PocoClass_Instances                | 10      |     571.5 μs |     16.03 μs |     47.25 μs |  0.93 |    0.10 |     0.9766 |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples                             | 10      |     528.3 μs |     13.24 μs |     38.63 μs |  0.85 |    0.08 |          - |         - |         - |      2.9 KB |        0.35 |
| Norm_Named_Tuples                       | 10      |     519.4 μs |     11.21 μs |     32.00 μs |  0.84 |    0.07 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_Anonymous_Types                    | 10      |     543.0 μs |     12.74 μs |     37.17 μs |  0.88 |    0.08 |     0.9766 |         - |         - |     9.77 KB |        1.18 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     565.9 μs |     12.51 μs |     36.70 μs |  0.92 |    0.09 |     0.9766 |         - |         - |     10.3 KB |        1.25 |
| Norm_Tuples_ReaderCallback              | 10      |     499.7 μs |      9.99 μs |     28.02 μs |  0.81 |    0.07 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     501.3 μs |     10.90 μs |     30.91 μs |  0.81 |    0.07 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Command_Reader                          | 10      |     494.2 μs |     10.69 μs |     30.68 μs |  0.80 |    0.08 |          - |         - |         - |     3.56 KB |        0.43 |
|                                         |         |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **1000**    |   **5,420.0 μs** |    **107.82 μs** |    **247.74 μs** |  **1.00** |    **0.00** |    **85.9375** |   **23.4375** |         **-** |   **740.79 KB** |        **1.00** |
| Dapper_Buffered_False                   | 1000    |   5,454.1 μs |    160.76 μs |    468.95 μs |  1.00 |    0.11 |    78.1250 |         - |         - |   724.72 KB |        0.98 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   6,103.2 μs |    150.34 μs |    438.54 μs |  1.13 |    0.09 |    46.8750 |         - |         - |   493.08 KB |        0.67 |
| Norm_NameValue_Array                    | 1000    |   5,412.2 μs |    119.62 μs |    352.69 μs |  1.00 |    0.09 |    78.1250 |         - |         - |   686.64 KB |        0.93 |
| Norm_PocoClass_Instances                | 1000    |   5,767.4 μs |    160.27 μs |    467.52 μs |  1.07 |    0.10 |    62.5000 |         - |         - |   594.78 KB |        0.80 |
| Norm_Tuples                             | 1000    |   5,953.9 μs |    136.03 μs |    396.79 μs |  1.11 |    0.09 |    23.4375 |         - |         - |   209.16 KB |        0.28 |
| Norm_Named_Tuples                       | 1000    |   5,586.0 μs |    122.69 μs |    357.90 μs |  1.04 |    0.08 |    85.9375 |         - |         - |   764.55 KB |        1.03 |
| Norm_Anonymous_Types                    | 1000    |   6,041.6 μs |    140.91 μs |    413.26 μs |  1.12 |    0.08 |    78.1250 |         - |         - |   695.42 KB |        0.94 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,643.9 μs |    164.48 μs |    479.79 μs |  1.07 |    0.09 |    78.1250 |         - |         - |   672.77 KB |        0.91 |
| Norm_Tuples_ReaderCallback              | 1000    |   6,082.9 μs |    143.17 μs |    422.14 μs |  1.13 |    0.09 |    23.4375 |         - |         - |   232.61 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   5,749.0 μs |    135.31 μs |    392.56 μs |  1.06 |    0.07 |    85.9375 |         - |         - |   764.69 KB |        1.03 |
| Command_Reader                          | 1000    |   5,139.8 μs |     99.83 μs |    271.61 μs |  0.95 |    0.07 |    31.2500 |         - |         - |   295.81 KB |        0.40 |
|                                         |         |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **10000**   |  **59,657.4 μs** |  **1,173.00 μs** |  **3,191.22 μs** |  **1.00** |    **0.00** |   **888.8889** |  **555.5556** |  **222.2222** |  **7522.04 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10000   |  43,165.1 μs |    861.37 μs |  2,429.50 μs |  0.73 |    0.06 |   833.3333 |         - |         - |  7264.19 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  48,193.4 μs |    961.85 μs |  2,728.62 μs |  0.81 |    0.07 |   555.5556 |         - |         - |  4853.24 KB |        0.65 |
| Norm_NameValue_Array                    | 10000   |  40,643.0 μs |    811.16 μs |  2,274.57 μs |  0.68 |    0.06 |   818.1818 |         - |         - |  6874.44 KB |        0.91 |
| Norm_PocoClass_Instances                | 10000   |  45,447.8 μs |    964.59 μs |  2,813.77 μs |  0.76 |    0.06 |   727.2727 |         - |         - |  5941.67 KB |        0.79 |
| Norm_Tuples                             | 10000   |  47,301.2 μs |  1,009.17 μs |  2,911.67 μs |  0.79 |    0.07 |   200.0000 |         - |         - |  2108.84 KB |        0.28 |
| Norm_Named_Tuples                       | 10000   |  45,484.2 μs |  1,016.21 μs |  2,948.20 μs |  0.76 |    0.06 |   909.0909 |         - |         - |  7657.92 KB |        1.02 |
| Norm_Anonymous_Types                    | 10000   |  51,005.8 μs |  1,191.41 μs |  3,494.19 μs |  0.86 |    0.07 |   818.1818 |         - |         - |  6955.63 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  47,099.1 μs |  1,049.80 μs |  3,078.87 μs |  0.79 |    0.06 |   777.7778 |         - |         - |  6722.03 KB |        0.89 |
| Norm_Tuples_ReaderCallback              | 10000   |  49,264.3 μs |  1,078.47 μs |  3,145.94 μs |  0.83 |    0.08 |   250.0000 |         - |         - |  2344.02 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  45,958.9 μs |  1,073.37 μs |  3,131.06 μs |  0.76 |    0.06 |   909.0909 |         - |         - |  7656.22 KB |        1.02 |
| Command_Reader                          | 10000   |  41,697.1 μs |    940.69 μs |  2,714.10 μs |  0.70 |    0.06 |   333.3333 |         - |         - |  2974.49 KB |        0.40 |
|                                         |         |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **100000**  | **608,459.7 μs** | **13,263.22 μs** | **38,267.41 μs** |  **1.00** |    **0.00** | **10000.0000** | **5000.0000** | **2000.0000** |  **75409.6 KB** |        **1.00** |
| Dapper_Buffered_False                   | 100000  | 408,837.4 μs | 13,472.61 μs | 39,086.48 μs |  0.67 |    0.08 |  8000.0000 |         - |         - | 73358.65 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 431,404.6 μs | 11,643.15 μs | 32,839.70 μs |  0.71 |    0.07 |  6000.0000 |         - |         - | 49150.92 KB |        0.65 |
| Norm_NameValue_Array                    | 100000  | 398,627.5 μs | 14,010.01 μs | 40,197.35 μs |  0.66 |    0.08 |  8000.0000 |         - |         - | 69476.56 KB |        0.92 |
| Norm_PocoClass_Instances                | 100000  | 405,483.5 μs | 12,293.26 μs | 36,054.00 μs |  0.67 |    0.07 |  7000.0000 |         - |         - | 60081.94 KB |        0.80 |
| Norm_Tuples                             | 100000  | 404,735.8 μs | 10,031.74 μs | 28,621.11 μs |  0.67 |    0.06 |  2000.0000 |         - |         - | 21811.63 KB |        0.29 |
| Norm_Named_Tuples                       | 100000  | 400,658.8 μs | 13,949.79 μs | 40,470.88 μs |  0.66 |    0.08 |  9000.0000 |         - |         - | 77265.91 KB |        1.02 |
| Norm_Anonymous_Types                    | 100000  | 419,708.1 μs | 12,988.79 μs | 37,267.29 μs |  0.69 |    0.07 |  8000.0000 |         - |         - | 70239.18 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 412,384.3 μs | 12,165.22 μs | 34,510.69 μs |  0.68 |    0.07 |  8000.0000 |         - |         - |  67895.5 KB |        0.90 |
| Norm_Tuples_ReaderCallback              | 100000  | 434,122.1 μs | 14,035.32 μs | 41,163.17 μs |  0.72 |    0.09 |  2000.0000 |         - |         - | 24140.59 KB |        0.32 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 420,950.5 μs | 16,645.44 μs | 48,555.54 μs |  0.69 |    0.09 |  9000.0000 |         - |         - | 77269.48 KB |        1.02 |
| Command_Reader                          | 100000  | 372,974.6 μs | 14,265.49 μs | 41,838.20 μs |  0.61 |    0.07 |  3000.0000 |         - |         - | 30447.51 KB |        0.40 |

#### Round 5

```

BenchmarkDotNet v0.13.10, Debian GNU/Linux 12 (bookworm) (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                  | Records | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|-------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
| **Dapper**                                  | **10**      |     **672.1 μs** |     **17.02 μs** |     **49.39 μs** |  **1.00** |    **0.00** |     **0.9766** |         **-** |         **-** |     **8.26 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10      |     667.8 μs |     16.84 μs |     49.12 μs |  1.00 |    0.11 |          - |         - |         - |     7.91 KB |        0.96 |
| EntityFrameworkCore_SqlQueryRaw         | 10      |     743.6 μs |     19.51 μs |     56.91 μs |  1.11 |    0.12 |     1.9531 |         - |         - |    16.27 KB |        1.97 |
| Norm_NameValue_Array                    | 10      |     539.3 μs |     14.10 μs |     41.56 μs |  0.81 |    0.08 |     0.9766 |         - |         - |     8.29 KB |        1.00 |
| Norm_PocoClass_Instances                | 10      |     598.1 μs |     14.30 μs |     41.49 μs |  0.89 |    0.09 |          - |         - |         - |     9.58 KB |        1.16 |
| Norm_Tuples                             | 10      |     528.6 μs |     13.52 μs |     39.23 μs |  0.79 |    0.08 |          - |         - |         - |      2.9 KB |        0.35 |
| Norm_Named_Tuples                       | 10      |     522.5 μs |     11.12 μs |     32.08 μs |  0.78 |    0.07 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Norm_Anonymous_Types                    | 10      |     541.3 μs |     11.72 μs |     34.00 μs |  0.81 |    0.08 |     0.9766 |         - |         - |     9.78 KB |        1.18 |
| Norm_PocoClass_Instances_ReaderCallback | 10      |     553.9 μs |     13.11 μs |     38.02 μs |  0.83 |    0.08 |     0.9766 |         - |         - |     10.3 KB |        1.25 |
| Norm_Tuples_ReaderCallback              | 10      |     523.8 μs |     12.94 μs |     37.75 μs |  0.78 |    0.08 |          - |         - |         - |     3.13 KB |        0.38 |
| Norm_Named_Tuples_ReaderCallback        | 10      |     521.4 μs |     11.96 μs |     35.07 μs |  0.78 |    0.08 |     0.9766 |         - |         - |     9.15 KB |        1.11 |
| Command_Reader                          | 10      |     500.7 μs |     12.71 μs |     37.08 μs |  0.75 |    0.07 |          - |         - |         - |     3.56 KB |        0.43 |
|                                         |         |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **1000**    |   **5,580.5 μs** |    **115.89 μs** |    **339.88 μs** |  **1.00** |    **0.00** |    **85.9375** |   **23.4375** |         **-** |   **740.94 KB** |        **1.00** |
| Dapper_Buffered_False                   | 1000    |   5,506.1 μs |    127.70 μs |    372.50 μs |  0.99 |    0.09 |    85.9375 |         - |         - |   724.68 KB |        0.98 |
| EntityFrameworkCore_SqlQueryRaw         | 1000    |   5,841.9 μs |    141.69 μs |    413.32 μs |  1.05 |    0.10 |    46.8750 |         - |         - |   493.14 KB |        0.67 |
| Norm_NameValue_Array                    | 1000    |   5,214.8 μs |    124.28 μs |    366.46 μs |  0.94 |    0.08 |    78.1250 |         - |         - |   686.57 KB |        0.93 |
| Norm_PocoClass_Instances                | 1000    |   5,775.5 μs |    131.82 μs |    386.60 μs |  1.04 |    0.10 |    70.3125 |         - |         - |    594.8 KB |        0.80 |
| Norm_Tuples                             | 1000    |   5,878.7 μs |    142.85 μs |    418.97 μs |  1.06 |    0.11 |    23.4375 |         - |         - |   209.12 KB |        0.28 |
| Norm_Named_Tuples                       | 1000    |   5,625.0 μs |    132.60 μs |    388.90 μs |  1.01 |    0.10 |    85.9375 |         - |         - |   764.64 KB |        1.03 |
| Norm_Anonymous_Types                    | 1000    |   6,013.6 μs |    133.93 μs |    394.89 μs |  1.08 |    0.09 |    78.1250 |         - |         - |   695.46 KB |        0.94 |
| Norm_PocoClass_Instances_ReaderCallback | 1000    |   5,849.5 μs |    133.14 μs |    390.47 μs |  1.05 |    0.11 |    78.1250 |         - |         - |   672.85 KB |        0.91 |
| Norm_Tuples_ReaderCallback              | 1000    |   5,708.0 μs |    137.51 μs |    401.12 μs |  1.03 |    0.10 |    23.4375 |         - |         - |   232.44 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 1000    |   5,599.4 μs |    118.81 μs |    350.31 μs |  1.01 |    0.09 |    85.9375 |         - |         - |    764.6 KB |        1.03 |
| Command_Reader                          | 1000    |   5,032.7 μs |    100.59 μs |    273.65 μs |  0.91 |    0.08 |    31.2500 |         - |         - |   295.36 KB |        0.40 |
|                                         |         |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **10000**   |  **59,944.7 μs** |  **1,281.00 μs** |  **3,736.74 μs** |  **1.00** |    **0.00** |   **888.8889** |  **555.5556** |  **222.2222** |  **7521.22 KB** |        **1.00** |
| Dapper_Buffered_False                   | 10000   |  42,938.3 μs |    883.74 μs |  2,549.81 μs |  0.72 |    0.06 |   833.3333 |         - |         - |  7266.14 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 10000   |  49,094.9 μs |  1,115.51 μs |  3,271.59 μs |  0.82 |    0.07 |   500.0000 |         - |         - |  4853.69 KB |        0.65 |
| Norm_NameValue_Array                    | 10000   |  41,960.5 μs |  1,012.02 μs |  2,936.05 μs |  0.70 |    0.07 |   833.3333 |         - |         - |  6875.38 KB |        0.91 |
| Norm_PocoClass_Instances                | 10000   |  45,965.6 μs |  1,067.73 μs |  3,131.47 μs |  0.77 |    0.08 |   727.2727 |         - |         - |  5940.27 KB |        0.79 |
| Norm_Tuples                             | 10000   |  48,978.2 μs |  1,077.76 μs |  3,109.59 μs |  0.82 |    0.07 |   181.8182 |         - |         - |  2108.35 KB |        0.28 |
| Norm_Named_Tuples                       | 10000   |  45,620.3 μs |    906.59 μs |  2,419.88 μs |  0.76 |    0.06 |   916.6667 |         - |         - |  7656.68 KB |        1.02 |
| Norm_Anonymous_Types                    | 10000   |  49,638.8 μs |  1,163.05 μs |  3,411.03 μs |  0.83 |    0.08 |   800.0000 |         - |         - |  6954.97 KB |        0.92 |
| Norm_PocoClass_Instances_ReaderCallback | 10000   |  47,264.8 μs |  1,530.80 μs |  4,489.57 μs |  0.79 |    0.09 |   750.0000 |         - |         - |  6721.48 KB |        0.89 |
| Norm_Tuples_ReaderCallback              | 10000   |  46,994.9 μs |    975.69 μs |  2,799.45 μs |  0.79 |    0.07 |   222.2222 |         - |         - |  2343.95 KB |        0.31 |
| Norm_Named_Tuples_ReaderCallback        | 10000   |  45,752.0 μs |    947.35 μs |  2,763.46 μs |  0.77 |    0.06 |   909.0909 |         - |         - |  7656.95 KB |        1.02 |
| Command_Reader                          | 10000   |  38,818.4 μs |    882.84 μs |  2,589.23 μs |  0.65 |    0.06 |   357.1429 |         - |         - |  2973.42 KB |        0.40 |
|                                         |         |              |              |              |       |         |            |           |           |             |             |
| **Dapper**                                  | **100000**  | **571,801.5 μs** | **10,898.19 μs** | **25,688.32 μs** |  **1.00** |    **0.00** | **10000.0000** | **5000.0000** | **2000.0000** | **75409.98 KB** |        **1.00** |
| Dapper_Buffered_False                   | 100000  | 426,777.0 μs | 11,653.41 μs | 33,622.76 μs |  0.74 |    0.07 |  8000.0000 |         - |         - | 73358.65 KB |        0.97 |
| EntityFrameworkCore_SqlQueryRaw         | 100000  | 436,627.3 μs | 11,860.24 μs | 34,408.70 μs |  0.77 |    0.07 |  6000.0000 |         - |         - | 49150.92 KB |        0.65 |
| Norm_NameValue_Array                    | 100000  | 389,170.4 μs | 11,517.16 μs | 33,596.09 μs |  0.68 |    0.06 |  8000.0000 |         - |         - | 69454.09 KB |        0.92 |
| Norm_PocoClass_Instances                | 100000  | 399,363.9 μs | 11,839.11 μs | 34,535.23 μs |  0.70 |    0.07 |  7000.0000 |         - |         - | 60081.19 KB |        0.80 |
| Norm_Tuples                             | 100000  | 399,155.3 μs |  9,306.42 μs | 26,551.73 μs |  0.70 |    0.06 |  2000.0000 |         - |         - | 21796.46 KB |        0.29 |
| Norm_Named_Tuples                       | 100000  | 398,164.0 μs | 12,541.76 μs | 36,782.81 μs |  0.71 |    0.07 |  9000.0000 |         - |         - |  77267.6 KB |        1.02 |
| Norm_Anonymous_Types                    | 100000  | 433,731.5 μs | 10,540.86 μs | 30,580.93 μs |  0.76 |    0.06 |  8000.0000 |         - |         - | 70247.99 KB |        0.93 |
| Norm_PocoClass_Instances_ReaderCallback | 100000  | 394,015.2 μs |  9,554.68 μs | 27,871.46 μs |  0.69 |    0.06 |  8000.0000 |         - |         - | 67896.63 KB |        0.90 |
| Norm_Tuples_ReaderCallback              | 100000  | 400,945.2 μs | 10,465.22 μs | 30,361.51 μs |  0.71 |    0.06 |  2000.0000 |         - |         - | 24156.29 KB |        0.32 |
| Norm_Named_Tuples_ReaderCallback        | 100000  | 426,810.4 μs | 10,628.45 μs | 31,003.68 μs |  0.74 |    0.07 |  9000.0000 |         - |         - | 77271.16 KB |        1.02 |
| Command_Reader                          | 100000  | 364,026.0 μs | 10,124.01 μs | 28,884.37 μs |  0.64 |    0.07 |  3000.0000 |         - |         - |  30389.8 KB |        0.40 |


