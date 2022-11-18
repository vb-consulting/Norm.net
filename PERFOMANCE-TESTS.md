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

#### Round 1, 2022-11-18, Dapper  2.0.123, Norm 5.3.1

``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|                                  Method | Records |           Mean |        Error |       StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|-------------:|-------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **301.8 μs** |      **5.96 μs** |     **14.50 μs** |       **302.0 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       292.7 μs |      6.25 μs |     18.43 μs |       295.6 μs |  0.97 |    0.07 |
|                    Norm_NameValue_Array |      10 |       254.6 μs |      4.98 μs |      7.46 μs |       254.0 μs |  0.86 |    0.04 |
|                Norm_PocoClass_Instances |      10 |       274.1 μs |      5.11 μs |      4.53 μs |       273.2 μs |  0.95 |    0.06 |
|                             Norm_Tuples |      10 |       257.7 μs |      4.96 μs |      7.27 μs |       257.7 μs |  0.88 |    0.05 |
|                       Norm_Named_Tuples |      10 |       262.7 μs |      4.76 μs |      3.97 μs |       263.4 μs |  0.91 |    0.06 |
|                    Norm_Anonymous_Types |      10 |       260.2 μs |      5.10 μs |      9.06 μs |       260.9 μs |  0.88 |    0.06 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       282.2 μs |      5.45 μs |      6.89 μs |       280.2 μs |  0.96 |    0.06 |
|              Norm_Tuples_ReaderCallback |      10 |       261.9 μs |      5.15 μs |      7.55 μs |       261.2 μs |  0.89 |    0.05 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       271.5 μs |      5.40 μs |      6.22 μs |       271.0 μs |  0.93 |    0.06 |
|                          Command_Reader |      10 |       249.2 μs |      4.88 μs |      7.74 μs |       247.7 μs |  0.85 |    0.05 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |    **1000** |     **2,064.1 μs** |     **41.22 μs** |     **66.57 μs** |     **2,056.9 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     2,104.5 μs |     41.95 μs |     81.82 μs |     2,112.7 μs |  1.01 |    0.06 |
|                    Norm_NameValue_Array |    1000 |     2,032.9 μs |     40.63 μs |    103.42 μs |     2,006.5 μs |  0.97 |    0.06 |
|                Norm_PocoClass_Instances |    1000 |     2,095.5 μs |     40.68 μs |     65.68 μs |     2,087.4 μs |  1.02 |    0.05 |
|                             Norm_Tuples |    1000 |     2,037.9 μs |     38.40 μs |     42.68 μs |     2,034.3 μs |  0.99 |    0.03 |
|                       Norm_Named_Tuples |    1000 |     2,088.0 μs |     40.78 μs |     63.49 μs |     2,083.1 μs |  1.01 |    0.05 |
|                    Norm_Anonymous_Types |    1000 |     2,228.2 μs |     43.88 μs |     60.06 μs |     2,223.7 μs |  1.08 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     2,133.5 μs |     41.90 μs |     55.93 μs |     2,146.1 μs |  1.03 |    0.05 |
|              Norm_Tuples_ReaderCallback |    1000 |     2,078.7 μs |     40.24 μs |     44.73 μs |     2,080.1 μs |  1.01 |    0.04 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     2,082.1 μs |     40.58 μs |     58.20 μs |     2,078.1 μs |  1.01 |    0.05 |
|                          Command_Reader |    1000 |     1,805.3 μs |     19.00 μs |     16.84 μs |     1,803.3 μs |  0.87 |    0.03 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |   **10000** |    **20,575.0 μs** |    **408.74 μs** |    **853.20 μs** |    **20,799.3 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    18,005.5 μs |    341.77 μs |    393.58 μs |    17,926.7 μs |  0.88 |    0.05 |
|                    Norm_NameValue_Array |   10000 |    17,193.1 μs |    335.52 μs |    481.19 μs |    17,167.2 μs |  0.84 |    0.05 |
|                Norm_PocoClass_Instances |   10000 |    22,499.5 μs |  1,907.86 μs |  5,625.36 μs |    23,458.9 μs |  1.13 |    0.32 |
|                             Norm_Tuples |   10000 |    17,250.0 μs |    336.99 μs |    563.04 μs |    17,053.7 μs |  0.85 |    0.06 |
|                       Norm_Named_Tuples |   10000 |    23,786.4 μs |  2,120.62 μs |  6,152.32 μs |    24,380.4 μs |  1.10 |    0.27 |
|                    Norm_Anonymous_Types |   10000 |    20,821.9 μs |    406.02 μs |    434.44 μs |    20,782.5 μs |  1.02 |    0.07 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    27,888.0 μs |  4,606.65 μs | 13,582.80 μs |    19,989.5 μs |  1.40 |    0.75 |
|              Norm_Tuples_ReaderCallback |   10000 |    17,238.2 μs |    332.28 μs |    382.66 μs |    17,213.2 μs |  0.85 |    0.05 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    27,102.5 μs |  4,628.70 μs | 13,647.81 μs |    19,258.5 μs |  1.28 |    0.63 |
|                          Command_Reader |   10000 |    15,658.3 μs |    308.47 μs |    579.39 μs |    15,597.6 μs |  0.76 |    0.05 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |  **100000** |   **222,759.2 μs** |  **4,360.53 μs** |  **5,968.74 μs** |   **221,530.1 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   183,451.6 μs |  3,630.72 μs |  5,544.49 μs |   181,684.7 μs |  0.82 |    0.03 |
|                    Norm_NameValue_Array |  100000 |   177,737.5 μs |  3,497.90 μs |  6,217.52 μs |   177,842.2 μs |  0.80 |    0.03 |
|                Norm_PocoClass_Instances |  100000 |   188,564.1 μs |  3,485.20 μs |  3,089.54 μs |   188,941.0 μs |  0.85 |    0.03 |
|                             Norm_Tuples |  100000 |   176,152.2 μs |  3,492.11 μs |  5,008.28 μs |   175,254.9 μs |  0.79 |    0.03 |
|                       Norm_Named_Tuples |  100000 |   193,612.4 μs |  3,622.52 μs |  3,720.07 μs |   193,848.0 μs |  0.87 |    0.03 |
|                    Norm_Anonymous_Types |  100000 |   208,688.7 μs |  4,002.41 μs |  5,343.10 μs |   208,827.9 μs |  0.94 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   196,356.0 μs |  3,837.34 μs |  6,411.33 μs |   194,949.0 μs |  0.88 |    0.03 |
|              Norm_Tuples_ReaderCallback |  100000 |   174,899.8 μs |  3,094.04 μs |  2,894.17 μs |   174,497.8 μs |  0.79 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   188,772.8 μs |  3,724.71 μs |  3,484.10 μs |   188,650.1 μs |  0.85 |    0.03 |
|                          Command_Reader |  100000 |   171,936.1 μs |  2,580.94 μs |  2,534.83 μs |   171,386.4 μs |  0.78 |    0.02 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** | **1000000** | **2,067,672.2 μs** | **39,919.17 μs** | **42,713.07 μs** | **2,074,890.1 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 1,801,574.8 μs | 20,505.18 μs | 19,180.56 μs | 1,803,371.5 μs |  0.87 |    0.02 |
|                    Norm_NameValue_Array | 1000000 | 1,794,352.5 μs | 18,079.73 μs | 16,027.21 μs | 1,797,735.4 μs |  0.87 |    0.02 |
|                Norm_PocoClass_Instances | 1000000 | 1,830,892.5 μs | 36,152.31 μs | 58,379.24 μs | 1,837,587.1 μs |  0.87 |    0.03 |
|                             Norm_Tuples | 1000000 | 1,770,553.8 μs | 24,437.63 μs | 22,858.98 μs | 1,767,929.9 μs |  0.86 |    0.02 |
|                       Norm_Named_Tuples | 1000000 | 1,882,155.0 μs | 24,193.77 μs | 22,630.86 μs | 1,884,455.0 μs |  0.91 |    0.02 |
|                    Norm_Anonymous_Types | 1000000 | 1,747,250.3 μs | 23,433.89 μs | 21,920.08 μs | 1,756,469.0 μs |  0.84 |    0.02 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 1,690,083.8 μs | 32,666.45 μs | 32,082.82 μs | 1,698,721.7 μs |  0.82 |    0.02 |
|              Norm_Tuples_ReaderCallback | 1000000 | 1,755,843.1 μs | 27,769.37 μs | 25,975.49 μs | 1,767,595.8 μs |  0.85 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 1,842,431.8 μs | 32,975.90 μs | 29,232.28 μs | 1,849,087.0 μs |  0.89 |    0.03 |
|                          Command_Reader | 1000000 | 1,614,467.9 μs | 22,133.68 μs | 19,620.93 μs | 1,620,604.4 μs |  0.78 |    0.02 |

#### Round 2, 2022-11-18, Dapper  2.0.123, Norm 5.3.1

``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|                                  Method | Records |           Mean |        Error |       StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|-------------:|-------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **310.4 μs** |      **6.15 μs** |     **13.62 μs** |       **310.0 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       292.9 μs |      6.01 μs |     17.63 μs |       290.5 μs |  0.96 |    0.08 |
|                    Norm_NameValue_Array |      10 |       255.4 μs |      5.09 μs |     11.06 μs |       258.0 μs |  0.82 |    0.05 |
|                Norm_PocoClass_Instances |      10 |       286.8 μs |      5.73 μs |      8.40 μs |       285.1 μs |  0.92 |    0.06 |
|                             Norm_Tuples |      10 |       254.4 μs |      5.01 μs |      9.65 μs |       252.1 μs |  0.82 |    0.05 |
|                       Norm_Named_Tuples |      10 |       262.0 μs |      4.80 μs |      4.93 μs |       261.9 μs |  0.82 |    0.03 |
|                    Norm_Anonymous_Types |      10 |       258.3 μs |      4.87 μs |      4.32 μs |       258.1 μs |  0.81 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       276.8 μs |      5.44 μs |      8.31 μs |       276.0 μs |  0.89 |    0.05 |
|              Norm_Tuples_ReaderCallback |      10 |       256.8 μs |      4.94 μs |      6.25 μs |       255.7 μs |  0.82 |    0.04 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       267.8 μs |      4.47 μs |      4.59 μs |       268.1 μs |  0.84 |    0.02 |
|                          Command_Reader |      10 |       247.3 μs |      4.85 μs |      9.79 μs |       248.8 μs |  0.80 |    0.05 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |    **1000** |     **2,055.8 μs** |     **40.59 μs** |     **82.92 μs** |     **2,065.5 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     2,139.9 μs |     42.17 μs |     90.77 μs |     2,160.8 μs |  1.04 |    0.06 |
|                    Norm_NameValue_Array |    1000 |     2,009.5 μs |     21.25 μs |     17.74 μs |     2,013.7 μs |  0.98 |    0.03 |
|                Norm_PocoClass_Instances |    1000 |     2,083.1 μs |     41.16 μs |     47.40 μs |     2,081.3 μs |  1.01 |    0.04 |
|                             Norm_Tuples |    1000 |     2,028.1 μs |     33.40 μs |     35.74 μs |     2,033.9 μs |  0.99 |    0.03 |
|                       Norm_Named_Tuples |    1000 |     2,103.9 μs |     42.01 μs |     73.58 μs |     2,099.4 μs |  1.02 |    0.06 |
|                    Norm_Anonymous_Types |    1000 |     2,224.8 μs |     38.41 μs |     37.72 μs |     2,212.4 μs |  1.08 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     2,121.0 μs |     38.12 μs |     40.79 μs |     2,124.9 μs |  1.04 |    0.03 |
|              Norm_Tuples_ReaderCallback |    1000 |     2,095.3 μs |     40.34 μs |     55.22 μs |     2,115.4 μs |  1.01 |    0.04 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     2,115.0 μs |     41.39 μs |     55.26 μs |     2,118.2 μs |  1.02 |    0.05 |
|                          Command_Reader |    1000 |     1,790.4 μs |     35.60 μs |     41.00 μs |     1,775.1 μs |  0.87 |    0.04 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |   **10000** |    **20,557.0 μs** |    **394.51 μs** |    **405.13 μs** |    **20,555.2 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    17,428.7 μs |    338.72 μs |    403.23 μs |    17,379.5 μs |  0.85 |    0.03 |
|                    Norm_NameValue_Array |   10000 |    16,963.2 μs |    334.36 μs |    659.99 μs |    16,929.0 μs |  0.83 |    0.04 |
|                Norm_PocoClass_Instances |   10000 |    23,659.5 μs |  3,550.74 μs | 10,413.71 μs |    18,433.8 μs |  1.05 |    0.42 |
|                             Norm_Tuples |   10000 |    16,992.0 μs |    292.38 μs |    244.15 μs |    17,074.1 μs |  0.83 |    0.02 |
|                       Norm_Named_Tuples |   10000 |    18,740.6 μs |    370.00 μs |    935.04 μs |    18,768.2 μs |  0.91 |    0.05 |
|                    Norm_Anonymous_Types |   10000 |    20,775.8 μs |    231.80 μs |    193.56 μs |    20,802.6 μs |  1.01 |    0.02 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    24,350.6 μs |  1,977.46 μs |  5,768.35 μs |    20,232.0 μs |  1.10 |    0.26 |
|              Norm_Tuples_ReaderCallback |   10000 |    17,164.5 μs |    202.15 μs |    189.09 μs |    17,157.5 μs |  0.83 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    18,783.8 μs |    374.79 μs |    675.83 μs |    18,703.7 μs |  0.92 |    0.04 |
|                          Command_Reader |   10000 |    16,982.0 μs |    333.17 μs |    370.32 μs |    16,884.5 μs |  0.83 |    0.03 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |  **100000** |   **247,149.5 μs** |  **4,907.74 μs** |  **5,251.23 μs** |   **247,537.6 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   180,742.4 μs |  3,023.32 μs |  2,828.02 μs |   181,414.7 μs |  0.73 |    0.02 |
|                    Norm_NameValue_Array |  100000 |   176,762.2 μs |  3,319.98 μs |  3,409.38 μs |   177,415.8 μs |  0.72 |    0.02 |
|                Norm_PocoClass_Instances |  100000 |   189,443.9 μs |  3,548.69 μs |  3,485.29 μs |   190,000.8 μs |  0.77 |    0.02 |
|                             Norm_Tuples |  100000 |   175,482.1 μs |  3,404.50 μs |  2,842.91 μs |   175,562.2 μs |  0.71 |    0.01 |
|                       Norm_Named_Tuples |  100000 |   192,542.5 μs |  3,472.69 μs |  3,859.89 μs |   191,952.4 μs |  0.78 |    0.02 |
|                    Norm_Anonymous_Types |  100000 |   210,775.3 μs |  4,146.89 μs |  3,879.01 μs |   211,686.9 μs |  0.85 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   195,251.4 μs |  3,819.81 μs |  5,099.33 μs |   194,868.6 μs |  0.79 |    0.03 |
|              Norm_Tuples_ReaderCallback |  100000 |   176,553.0 μs |  3,335.06 μs |  4,991.76 μs |   175,752.6 μs |  0.71 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   191,756.5 μs |  3,699.82 μs |  4,112.34 μs |   191,681.7 μs |  0.78 |    0.02 |
|                          Command_Reader |  100000 |   173,333.7 μs |  3,228.02 μs |  2,695.54 μs |   173,577.5 μs |  0.70 |    0.02 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** | **1000000** | **2,049,905.3 μs** | **33,458.46 μs** | **31,297.07 μs** | **2,057,428.1 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 1,796,729.6 μs | 22,632.87 μs | 21,170.80 μs | 1,804,471.7 μs |  0.88 |    0.02 |
|                    Norm_NameValue_Array | 1000000 | 1,812,709.9 μs | 23,177.18 μs | 21,679.95 μs | 1,805,231.0 μs |  0.88 |    0.02 |
|                Norm_PocoClass_Instances | 1000000 | 1,808,099.0 μs | 36,136.65 μs | 66,077.91 μs | 1,816,041.1 μs |  0.87 |    0.04 |
|                             Norm_Tuples | 1000000 | 1,766,056.2 μs | 16,714.77 μs | 15,635.01 μs | 1,771,148.0 μs |  0.86 |    0.01 |
|                       Norm_Named_Tuples | 1000000 | 1,863,799.6 μs | 31,141.26 μs | 29,129.56 μs | 1,859,565.6 μs |  0.91 |    0.02 |
|                    Norm_Anonymous_Types | 1000000 | 1,728,686.8 μs | 20,193.05 μs | 18,888.59 μs | 1,724,794.4 μs |  0.84 |    0.02 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 1,662,997.6 μs | 31,993.00 μs | 40,460.90 μs | 1,665,202.4 μs |  0.81 |    0.03 |
|              Norm_Tuples_ReaderCallback | 1000000 | 1,753,109.9 μs | 21,026.29 μs | 18,639.26 μs | 1,750,415.6 μs |  0.86 |    0.01 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 1,868,014.7 μs | 31,990.29 μs | 29,923.74 μs | 1,858,046.6 μs |  0.91 |    0.02 |
|                          Command_Reader | 1000000 | 1,651,307.8 μs | 32,862.09 μs | 70,739.12 μs | 1,628,811.6 μs |  0.86 |    0.03 |
