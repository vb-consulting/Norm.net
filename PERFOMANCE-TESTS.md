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

#### Round 1, 2022-06-30, Dapper  2.0.123, Norm 5.2.2.0

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT


```
|                                  Method | Records |           Mean |         Error |        StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|--------------:|--------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **390.0 μs** |       **7.56 μs** |      **11.08 μs** |       **391.5 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       400.2 μs |       7.83 μs |       8.70 μs |       399.6 μs |  1.03 |    0.04 |
|                    Norm_NameValue_Array |      10 |       341.6 μs |       6.16 μs |       5.76 μs |       343.1 μs |  0.87 |    0.03 |
|                Norm_PocoClass_Instances |      10 |       374.6 μs |       7.17 μs |       9.57 μs |       374.1 μs |  0.96 |    0.04 |
|                             Norm_Tuples |      10 |       341.0 μs |       6.61 μs |       6.79 μs |       340.8 μs |  0.87 |    0.03 |
|                       Norm_Named_Tuples |      10 |       361.2 μs |       4.80 μs |       4.26 μs |       361.1 μs |  0.92 |    0.02 |
|                    Norm_Anonymous_Types |      10 |       385.2 μs |       7.54 μs |      13.21 μs |       383.8 μs |  0.98 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       477.9 μs |      22.72 μs |      66.28 μs |       500.3 μs |  1.01 |    0.12 |
|              Norm_Tuples_ReaderCallback |      10 |       466.3 μs |       9.31 μs |      24.03 μs |       467.0 μs |  1.21 |    0.09 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       503.2 μs |       9.84 μs |      13.13 μs |       501.7 μs |  1.29 |    0.05 |
|                          Command_Reader |      10 |       439.1 μs |       8.74 μs |      15.76 μs |       437.5 μs |  1.13 |    0.06 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |    **1000** |     **6,032.7 μs** |     **243.58 μs** |     **710.52 μs** |     **5,718.1 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     5,266.1 μs |     100.71 μs |     280.74 μs |     5,200.7 μs |  0.88 |    0.11 |
|                    Norm_NameValue_Array |    1000 |     5,099.6 μs |     100.41 μs |     103.12 μs |     5,105.3 μs |  0.89 |    0.07 |
|                Norm_PocoClass_Instances |    1000 |     5,246.2 μs |      80.70 μs |      99.11 μs |     5,259.7 μs |  0.89 |    0.10 |
|                             Norm_Tuples |    1000 |     5,116.8 μs |      70.85 μs |      66.27 μs |     5,111.0 μs |  0.91 |    0.03 |
|                       Norm_Named_Tuples |    1000 |     5,466.3 μs |     107.86 μs |     110.77 μs |     5,451.6 μs |  0.96 |    0.07 |
|                    Norm_Anonymous_Types |    1000 |     6,182.6 μs |     110.14 μs |     161.44 μs |     6,187.1 μs |  1.01 |    0.13 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     5,391.6 μs |     101.76 μs |     145.94 μs |     5,349.3 μs |  0.89 |    0.11 |
|              Norm_Tuples_ReaderCallback |    1000 |     5,162.5 μs |     101.91 μs |     100.09 μs |     5,169.7 μs |  0.92 |    0.04 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     5,411.7 μs |      87.89 μs |      77.92 μs |     5,420.6 μs |  0.97 |    0.02 |
|                          Command_Reader |    1000 |     4,948.4 μs |      70.29 μs |      62.31 μs |     4,951.0 μs |  0.89 |    0.03 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |   **10000** |    **57,416.4 μs** |   **1,076.02 μs** |   **2,639.49 μs** |    **57,102.3 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    50,336.0 μs |   1,228.25 μs |   3,524.08 μs |    49,512.6 μs |  0.89 |    0.08 |
|                    Norm_NameValue_Array |   10000 |    49,354.3 μs |     986.85 μs |   2,247.55 μs |    49,328.7 μs |  0.86 |    0.05 |
|                Norm_PocoClass_Instances |   10000 |    49,226.6 μs |     982.87 μs |   2,277.95 μs |    49,230.7 μs |  0.86 |    0.06 |
|                             Norm_Tuples |   10000 |    49,689.8 μs |     992.93 μs |   2,767.90 μs |    49,427.3 μs |  0.87 |    0.07 |
|                       Norm_Named_Tuples |   10000 |    50,687.7 μs |   1,009.62 μs |   2,258.15 μs |    50,242.9 μs |  0.88 |    0.05 |
|                    Norm_Anonymous_Types |   10000 |    55,427.8 μs |   1,107.16 μs |   3,049.43 μs |    55,024.7 μs |  0.97 |    0.08 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    51,385.9 μs |   1,021.33 μs |   2,930.39 μs |    51,010.3 μs |  0.90 |    0.06 |
|              Norm_Tuples_ReaderCallback |   10000 |    47,630.0 μs |     818.37 μs |   1,199.56 μs |    47,698.4 μs |  0.82 |    0.04 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    51,537.8 μs |   1,004.27 μs |   2,697.92 μs |    51,528.1 μs |  0.90 |    0.07 |
|                          Command_Reader |   10000 |    46,848.3 μs |     931.33 μs |   2,580.70 μs |    46,268.5 μs |  0.82 |    0.06 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |  **100000** |   **523,435.0 μs** |  **10,381.31 μs** |  **20,732.60 μs** |   **520,636.6 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   504,862.9 μs |  10,070.35 μs |  27,397.12 μs |   499,353.8 μs |  0.97 |    0.07 |
|                    Norm_NameValue_Array |  100000 |   500,811.3 μs |   9,993.56 μs |  27,525.17 μs |   494,625.2 μs |  0.97 |    0.06 |
|                Norm_PocoClass_Instances |  100000 |   515,072.5 μs |  10,746.71 μs |  31,006.72 μs |   507,953.3 μs |  1.00 |    0.08 |
|                             Norm_Tuples |  100000 |   493,064.4 μs |   9,732.11 μs |  20,098.49 μs |   490,621.3 μs |  0.94 |    0.05 |
|                       Norm_Named_Tuples |  100000 |   517,607.7 μs |  10,240.87 μs |  25,312.89 μs |   514,522.9 μs |  0.99 |    0.07 |
|                    Norm_Anonymous_Types |  100000 |   605,958.6 μs |  14,878.35 μs |  42,688.77 μs |   596,414.6 μs |  1.21 |    0.09 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   522,591.1 μs |  11,261.04 μs |  33,026.67 μs |   518,088.2 μs |  1.01 |    0.07 |
|              Norm_Tuples_ReaderCallback |  100000 |   495,477.2 μs |   9,878.37 μs |  27,700.01 μs |   490,939.6 μs |  0.96 |    0.07 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   527,374.7 μs |  11,526.75 μs |  33,986.88 μs |   521,204.9 μs |  1.01 |    0.07 |
|                          Command_Reader |  100000 |   480,753.3 μs |   9,561.00 μs |  18,420.80 μs |   478,690.5 μs |  0.92 |    0.05 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** | **1000000** | **5,642,419.5 μs** |  **94,685.93 μs** |  **88,569.28 μs** | **5,661,449.6 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 5,244,191.6 μs | 101,715.03 μs | 164,250.81 μs | 5,223,746.4 μs |  0.93 |    0.03 |
|                    Norm_NameValue_Array | 1000000 | 5,273,184.2 μs | 103,962.19 μs | 149,099.47 μs | 5,244,191.2 μs |  0.93 |    0.03 |
|                Norm_PocoClass_Instances | 1000000 | 5,234,361.1 μs |  84,508.21 μs |  65,978.45 μs | 5,234,409.5 μs |  0.93 |    0.02 |
|                             Norm_Tuples | 1000000 | 4,696,701.4 μs |  77,153.25 μs |  72,169.20 μs | 4,687,920.2 μs |  0.83 |    0.02 |
|                       Norm_Named_Tuples | 1000000 | 4,823,519.6 μs |  57,687.51 μs |  48,171.68 μs | 4,816,799.5 μs |  0.85 |    0.01 |
|                    Norm_Anonymous_Types | 1000000 | 4,959,759.7 μs |  64,960.43 μs |  57,585.73 μs | 4,965,050.7 μs |  0.88 |    0.02 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 4,749,057.3 μs |  44,341.12 μs |  37,026.84 μs | 4,751,650.6 μs |  0.84 |    0.01 |
|              Norm_Tuples_ReaderCallback | 1000000 | 4,621,069.7 μs |  51,713.33 μs |  45,842.52 μs | 4,617,570.0 μs |  0.82 |    0.01 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 4,895,052.1 μs |  96,751.38 μs | 169,452.24 μs | 4,835,532.6 μs |  0.87 |    0.04 |
|                          Command_Reader | 1000000 | 4,484,632.4 μs |  48,895.12 μs |  40,829.64 μs | 4,478,536.2 μs |  0.79 |    0.01 |

#### Round 2, 2022-06-30, Dapper  2.0.123, Norm 5.2.2.0

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT


```
|                                  Method | Records |           Mean |         Error |        StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|--------------:|--------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **398.1 μs** |       **7.52 μs** |      **11.26 μs** |       **398.9 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       403.0 μs |       7.84 μs |      10.19 μs |       403.1 μs |  1.02 |    0.04 |
|                    Norm_NameValue_Array |      10 |       360.4 μs |       7.00 μs |       6.88 μs |       360.6 μs |  0.91 |    0.03 |
|                Norm_PocoClass_Instances |      10 |       418.4 μs |       8.24 μs |      14.21 μs |       419.2 μs |  1.05 |    0.05 |
|                             Norm_Tuples |      10 |       366.8 μs |       6.91 μs |       9.23 μs |       366.3 μs |  0.93 |    0.04 |
|                       Norm_Named_Tuples |      10 |       400.3 μs |       7.82 μs |      12.84 μs |       396.9 μs |  1.01 |    0.04 |
|                    Norm_Anonymous_Types |      10 |       458.2 μs |       9.10 μs |      26.12 μs |       466.3 μs |  1.09 |    0.07 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       477.4 μs |       9.53 μs |      15.38 μs |       480.0 μs |  1.20 |    0.04 |
|              Norm_Tuples_ReaderCallback |      10 |       439.9 μs |       8.52 μs |      12.21 μs |       438.6 μs |  1.11 |    0.04 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       464.3 μs |       8.44 μs |      12.88 μs |       469.4 μs |  1.17 |    0.04 |
|                          Command_Reader |      10 |       428.7 μs |       8.42 μs |      10.65 μs |       428.6 μs |  1.08 |    0.04 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |    **1000** |     **5,523.3 μs** |     **107.06 μs** |     **160.24 μs** |     **5,492.9 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     5,354.2 μs |     105.47 μs |      98.66 μs |     5,348.0 μs |  0.96 |    0.04 |
|                    Norm_NameValue_Array |    1000 |     5,272.9 μs |      84.24 μs |      78.80 μs |     5,244.7 μs |  0.95 |    0.03 |
|                Norm_PocoClass_Instances |    1000 |     5,407.3 μs |     106.89 μs |     123.10 μs |     5,429.2 μs |  0.97 |    0.04 |
|                             Norm_Tuples |    1000 |     5,253.4 μs |      95.00 μs |      84.22 μs |     5,247.0 μs |  0.94 |    0.03 |
|                       Norm_Named_Tuples |    1000 |     5,526.2 μs |     101.68 μs |     108.79 μs |     5,541.0 μs |  0.99 |    0.03 |
|                    Norm_Anonymous_Types |    1000 |     6,280.5 μs |      88.41 μs |      78.37 μs |     6,291.3 μs |  1.13 |    0.04 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     5,575.2 μs |     110.48 μs |     131.52 μs |     5,547.3 μs |  1.00 |    0.03 |
|              Norm_Tuples_ReaderCallback |    1000 |     5,270.8 μs |     100.36 μs |     107.38 μs |     5,271.4 μs |  0.95 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     5,603.9 μs |      90.81 μs |      84.94 μs |     5,587.3 μs |  1.01 |    0.03 |
|                          Command_Reader |    1000 |     5,108.1 μs |      85.71 μs |      80.17 μs |     5,107.0 μs |  0.92 |    0.03 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |   **10000** |    **55,563.6 μs** |   **1,107.31 μs** |   **2,878.05 μs** |    **54,925.4 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    52,770.5 μs |   1,050.93 μs |   2,372.12 μs |    52,151.0 μs |  0.95 |    0.07 |
|                    Norm_NameValue_Array |   10000 |    53,702.7 μs |   1,217.84 μs |   3,552.50 μs |    52,451.9 μs |  0.97 |    0.08 |
|                Norm_PocoClass_Instances |   10000 |    53,815.5 μs |   1,069.25 μs |   2,541.17 μs |    53,518.5 μs |  0.97 |    0.06 |
|                             Norm_Tuples |   10000 |    52,129.6 μs |   1,022.27 μs |   1,177.25 μs |    52,237.0 μs |  0.95 |    0.06 |
|                       Norm_Named_Tuples |   10000 |    53,599.5 μs |   1,064.33 μs |   2,728.30 μs |    53,066.9 μs |  0.97 |    0.06 |
|                    Norm_Anonymous_Types |   10000 |    56,114.4 μs |   1,098.41 μs |   1,952.42 μs |    55,679.8 μs |  1.02 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    52,202.8 μs |   1,038.00 μs |   2,788.52 μs |    51,812.6 μs |  0.94 |    0.07 |
|              Norm_Tuples_ReaderCallback |   10000 |    49,824.3 μs |     990.39 μs |   2,315.00 μs |    49,453.2 μs |  0.90 |    0.06 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    52,217.1 μs |   1,040.31 μs |   2,968.06 μs |    51,647.7 μs |  0.94 |    0.07 |
|                          Command_Reader |   10000 |    47,109.7 μs |     890.27 μs |   1,877.88 μs |    47,145.5 μs |  0.85 |    0.06 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |  **100000** |   **541,235.0 μs** |  **10,757.91 μs** |  **22,925.99 μs** |   **539,406.5 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   517,038.4 μs |  10,391.75 μs |  29,982.59 μs |   508,283.8 μs |  0.96 |    0.07 |
|                    Norm_NameValue_Array |  100000 |   515,816.3 μs |  10,304.45 μs |  28,724.68 μs |   510,654.2 μs |  0.95 |    0.07 |
|                Norm_PocoClass_Instances |  100000 |   522,243.4 μs |  10,394.24 μs |  29,823.01 μs |   516,928.3 μs |  0.96 |    0.06 |
|                             Norm_Tuples |  100000 |   512,047.4 μs |  11,020.41 μs |  31,972.20 μs |   503,421.0 μs |  0.94 |    0.06 |
|                       Norm_Named_Tuples |  100000 |   530,111.3 μs |  10,737.63 μs |  31,660.14 μs |   531,026.4 μs |  0.97 |    0.07 |
|                    Norm_Anonymous_Types |  100000 |   566,525.8 μs |  11,300.73 μs |  30,163.94 μs |   561,549.9 μs |  1.05 |    0.07 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   525,869.7 μs |  10,843.72 μs |  31,459.59 μs |   521,628.7 μs |  0.97 |    0.07 |
|              Norm_Tuples_ReaderCallback |  100000 |   516,970.8 μs |  11,468.75 μs |  33,454.89 μs |   508,915.2 μs |  0.96 |    0.07 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   540,491.5 μs |  13,079.63 μs |  38,360.29 μs |   534,325.8 μs |  1.00 |    0.09 |
|                          Command_Reader |  100000 |   498,181.1 μs |  11,516.61 μs |  33,411.77 μs |   490,365.3 μs |  0.93 |    0.08 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** | **1000000** | **5,654,881.6 μs** | **112,127.83 μs** | **129,126.58 μs** | **5,653,428.0 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 5,397,699.8 μs | 107,401.71 μs | 154,032.32 μs | 5,409,303.3 μs |  0.96 |    0.03 |
|                    Norm_NameValue_Array | 1000000 | 5,297,470.6 μs | 101,847.36 μs | 125,077.71 μs | 5,301,107.7 μs |  0.94 |    0.03 |
|                Norm_PocoClass_Instances | 1000000 | 5,405,450.2 μs | 106,220.28 μs | 104,322.52 μs | 5,375,050.5 μs |  0.96 |    0.03 |
|                             Norm_Tuples | 1000000 | 5,294,499.2 μs | 102,894.94 μs | 169,059.34 μs | 5,285,476.0 μs |  0.94 |    0.04 |
|                       Norm_Named_Tuples | 1000000 | 5,486,893.7 μs |  60,390.13 μs |  53,534.28 μs | 5,475,638.2 μs |  0.97 |    0.03 |
|                    Norm_Anonymous_Types | 1000000 | 5,772,597.8 μs |  95,869.14 μs |  84,985.49 μs | 5,757,173.0 μs |  1.02 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 5,480,501.2 μs | 107,020.46 μs | 160,183.18 μs | 5,464,735.8 μs |  0.97 |    0.03 |
|              Norm_Tuples_ReaderCallback | 1000000 | 5,240,427.1 μs | 104,121.79 μs | 155,844.60 μs | 5,225,990.8 μs |  0.93 |    0.04 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 5,541,664.8 μs | 107,433.45 μs | 131,937.93 μs | 5,557,608.2 μs |  0.98 |    0.02 |
|                          Command_Reader | 1000000 | 5,051,930.4 μs |  85,830.23 μs | 114,580.89 μs | 5,018,072.8 μs |  0.90 |    0.03 |

#### Round 3, 2022-06-30, Dapper  2.0.123, Norm 5.2.2.0

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT


```
|                                  Method | Records |           Mean |        Error |       StdDev | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|-------------:|-------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **324.8 μs** |      **6.14 μs** |      **6.82 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       340.1 μs |      6.32 μs |      5.61 μs |  1.05 |    0.03 |
|                    Norm_NameValue_Array |      10 |       311.6 μs |      5.73 μs |      5.36 μs |  0.97 |    0.02 |
|                Norm_PocoClass_Instances |      10 |       348.7 μs |      6.80 μs |      8.10 μs |  1.07 |    0.03 |
|                             Norm_Tuples |      10 |       326.4 μs |      5.41 μs |      8.57 μs |  1.01 |    0.04 |
|                       Norm_Named_Tuples |      10 |       352.9 μs |      6.72 μs |      7.47 μs |  1.09 |    0.03 |
|                    Norm_Anonymous_Types |      10 |       368.5 μs |      7.32 μs |     12.23 μs |  1.13 |    0.04 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       376.9 μs |      7.49 μs |     12.09 μs |  1.16 |    0.04 |
|              Norm_Tuples_ReaderCallback |      10 |       401.3 μs |      7.99 μs |     18.98 μs |  1.19 |    0.08 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       429.7 μs |      8.45 μs |     14.11 μs |  1.31 |    0.05 |
|                          Command_Reader |      10 |       396.1 μs |      7.90 μs |     14.45 μs |  1.23 |    0.05 |
|                                         |         |                |              |              |       |         |
|                                  **Dapper** |    **1000** |     **5,046.4 μs** |     **99.00 μs** |     **92.60 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     5,048.3 μs |     92.63 μs |    141.46 μs |  1.01 |    0.04 |
|                    Norm_NameValue_Array |    1000 |     4,963.0 μs |     98.08 μs |     96.33 μs |  0.98 |    0.03 |
|                Norm_PocoClass_Instances |    1000 |     5,310.1 μs |    101.06 μs |    232.19 μs |  1.06 |    0.07 |
|                             Norm_Tuples |    1000 |     4,666.8 μs |     90.34 μs |     88.73 μs |  0.93 |    0.03 |
|                       Norm_Named_Tuples |    1000 |     4,825.1 μs |     70.94 μs |     66.36 μs |  0.96 |    0.02 |
|                    Norm_Anonymous_Types |    1000 |     5,545.6 μs |    110.72 μs |    108.74 μs |  1.10 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     4,750.7 μs |     78.14 μs |     73.09 μs |  0.94 |    0.02 |
|              Norm_Tuples_ReaderCallback |    1000 |     4,634.9 μs |     89.67 μs |    103.26 μs |  0.92 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     4,840.1 μs |     74.64 μs |     97.06 μs |  0.96 |    0.03 |
|                          Command_Reader |    1000 |     4,436.0 μs |     78.03 μs |     72.99 μs |  0.88 |    0.02 |
|                                         |         |                |              |              |       |         |
|                                  **Dapper** |   **10000** |    **46,754.5 μs** |    **906.41 μs** |  **1,328.60 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    44,576.1 μs |    870.37 μs |  1,524.38 μs |  0.96 |    0.05 |
|                    Norm_NameValue_Array |   10000 |    43,765.1 μs |    864.27 μs |    808.44 μs |  0.93 |    0.04 |
|                Norm_PocoClass_Instances |   10000 |    44,588.9 μs |    556.20 μs |    493.05 μs |  0.95 |    0.03 |
|                             Norm_Tuples |   10000 |    43,889.1 μs |    856.05 μs |  1,382.36 μs |  0.94 |    0.05 |
|                       Norm_Named_Tuples |   10000 |    44,881.8 μs |    620.91 μs |    580.80 μs |  0.96 |    0.04 |
|                    Norm_Anonymous_Types |   10000 |    48,445.5 μs |    800.17 μs |  1,095.29 μs |  1.04 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    45,680.1 μs |    843.11 μs |  1,096.28 μs |  0.98 |    0.03 |
|              Norm_Tuples_ReaderCallback |   10000 |    44,268.4 μs |    876.42 μs |    937.76 μs |  0.95 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    45,038.5 μs |    760.45 μs |    674.12 μs |  0.96 |    0.04 |
|                          Command_Reader |   10000 |    42,600.6 μs |    462.44 μs |    432.56 μs |  0.91 |    0.03 |
|                                         |         |                |              |              |       |         |
|                                  **Dapper** |  **100000** |   **480,257.7 μs** |  **8,597.66 μs** | **10,558.70 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   454,310.9 μs |  8,552.10 μs |  8,399.31 μs |  0.95 |    0.03 |
|                    Norm_NameValue_Array |  100000 |   454,081.2 μs |  7,204.90 μs |  6,386.95 μs |  0.95 |    0.02 |
|                Norm_PocoClass_Instances |  100000 |   461,273.6 μs |  8,373.44 μs |  7,832.52 μs |  0.96 |    0.02 |
|                             Norm_Tuples |  100000 |   460,372.5 μs |  9,088.87 μs | 16,155.45 μs |  0.97 |    0.04 |
|                       Norm_Named_Tuples |  100000 |   464,267.2 μs |  6,554.20 μs |  5,473.05 μs |  0.97 |    0.03 |
|                    Norm_Anonymous_Types |  100000 |   503,924.3 μs | 10,036.61 μs | 14,711.53 μs |  1.05 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   470,819.2 μs |  8,800.13 μs | 15,179.78 μs |  0.99 |    0.04 |
|              Norm_Tuples_ReaderCallback |  100000 |   454,710.2 μs |  8,913.78 μs | 12,201.29 μs |  0.95 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   469,960.7 μs |  9,128.62 μs |  9,374.42 μs |  0.98 |    0.03 |
|                          Command_Reader |  100000 |   444,605.3 μs |  8,803.84 μs | 12,341.75 μs |  0.93 |    0.03 |
|                                         |         |                |              |              |       |         |
|                                  **Dapper** | **1000000** | **4,998,891.0 μs** | **69,641.30 μs** | **61,735.20 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 4,729,129.9 μs | 51,946.08 μs | 43,377.33 μs |  0.95 |    0.02 |
|                    Norm_NameValue_Array | 1000000 | 4,708,587.5 μs | 34,096.93 μs | 30,226.04 μs |  0.94 |    0.01 |
|                Norm_PocoClass_Instances | 1000000 | 4,851,510.6 μs | 53,416.67 μs | 47,352.49 μs |  0.97 |    0.02 |
|                             Norm_Tuples | 1000000 | 4,646,575.5 μs | 55,013.16 μs | 45,938.48 μs |  0.93 |    0.02 |
|                       Norm_Named_Tuples | 1000000 | 4,854,064.5 μs | 48,644.40 μs | 40,620.28 μs |  0.97 |    0.02 |
|                    Norm_Anonymous_Types | 1000000 | 5,067,576.1 μs | 80,572.86 μs | 71,425.74 μs |  1.01 |    0.02 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 4,877,758.0 μs | 53,738.04 μs | 44,873.70 μs |  0.98 |    0.02 |
|              Norm_Tuples_ReaderCallback | 1000000 | 4,716,567.7 μs | 37,541.77 μs | 31,349.08 μs |  0.94 |    0.01 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 4,983,155.1 μs | 71,137.73 μs | 89,966.45 μs |  0.99 |    0.02 |
|                          Command_Reader | 1000000 | 4,544,724.7 μs | 52,568.85 μs | 46,600.92 μs |  0.91 |    0.01 |
