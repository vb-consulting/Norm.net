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

#### Round 1, 2022-10-16, Dapper  2.0.123, Norm 5.3.0

``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.674)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2


```
|                                  Method | Records |           Mean |        Error |       StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|-------------:|-------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **277.1 μs** |      **7.27 μs** |     **21.43 μs** |       **278.6 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       272.8 μs |      5.61 μs |     16.55 μs |       270.9 μs |  0.99 |    0.10 |
|                    Norm_NameValue_Array |      10 |       247.2 μs |      2.35 μs |      1.96 μs |       247.5 μs |  0.88 |    0.05 |
|                Norm_PocoClass_Instances |      10 |       269.2 μs |      5.22 μs |      6.61 μs |       271.6 μs |  0.99 |    0.07 |
|                             Norm_Tuples |      10 |       241.1 μs |      3.79 μs |      3.55 μs |       241.6 μs |  0.86 |    0.04 |
|                       Norm_Named_Tuples |      10 |       263.4 μs |      5.26 μs |      7.38 μs |       261.0 μs |  0.99 |    0.08 |
|                    Norm_Anonymous_Types |      10 |       275.1 μs |      5.47 μs |      6.91 μs |       274.9 μs |  1.01 |    0.08 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       277.9 μs |      5.41 μs |      8.10 μs |       277.5 μs |  1.05 |    0.08 |
|              Norm_Tuples_ReaderCallback |      10 |       255.8 μs |      5.09 μs |      7.77 μs |       256.2 μs |  0.97 |    0.08 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       269.9 μs |      5.24 μs |      8.00 μs |       270.6 μs |  1.02 |    0.07 |
|                          Command_Reader |      10 |       238.5 μs |      3.61 μs |      3.20 μs |       238.8 μs |  0.85 |    0.04 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |    **1000** |     **1,960.4 μs** |     **22.99 μs** |     **21.51 μs** |     **1,961.8 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     1,939.8 μs |     37.72 μs |     37.04 μs |     1,939.3 μs |  0.99 |    0.02 |
|                    Norm_NameValue_Array |    1000 |     1,903.0 μs |     37.15 μs |     42.79 μs |     1,903.0 μs |  0.97 |    0.02 |
|                Norm_PocoClass_Instances |    1000 |     2,116.3 μs |     32.02 μs |     28.38 μs |     2,107.6 μs |  1.08 |    0.02 |
|                             Norm_Tuples |    1000 |     2,059.4 μs |     39.58 μs |     48.61 μs |     2,063.1 μs |  1.05 |    0.02 |
|                       Norm_Named_Tuples |    1000 |     2,670.5 μs |     47.90 μs |     42.46 μs |     2,675.4 μs |  1.36 |    0.03 |
|                    Norm_Anonymous_Types |    1000 |     3,041.1 μs |     59.75 μs |     58.68 μs |     3,042.9 μs |  1.55 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     2,271.8 μs |     39.43 μs |     49.87 μs |     2,261.4 μs |  1.16 |    0.03 |
|              Norm_Tuples_ReaderCallback |    1000 |     2,048.2 μs |     33.03 μs |     30.89 μs |     2,059.1 μs |  1.04 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     2,578.6 μs |     49.02 μs |     45.86 μs |     2,575.8 μs |  1.32 |    0.02 |
|                          Command_Reader |    1000 |     1,811.6 μs |     30.81 μs |     40.06 μs |     1,805.7 μs |  0.93 |    0.02 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |   **10000** |    **20,705.5 μs** |    **408.82 μs** |    **922.76 μs** |    **20,678.5 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    16,888.4 μs |    332.38 μs |    648.29 μs |    16,754.2 μs |  0.82 |    0.05 |
|                    Norm_NameValue_Array |   10000 |    16,938.0 μs |    338.29 μs |    555.82 μs |    16,915.9 μs |  0.82 |    0.05 |
|                Norm_PocoClass_Instances |   10000 |    24,141.2 μs |  3,060.37 μs |  8,829.86 μs |    19,896.3 μs |  1.17 |    0.47 |
|                             Norm_Tuples |   10000 |    17,198.3 μs |    331.05 μs |    339.97 μs |    17,275.1 μs |  0.82 |    0.03 |
|                       Norm_Named_Tuples |   10000 |    24,664.1 μs |    470.75 μs |    462.34 μs |    24,654.2 μs |  1.18 |    0.04 |
|                    Norm_Anonymous_Types |   10000 |    25,481.6 μs |    460.70 μs |    430.94 μs |    25,418.8 μs |  1.22 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    20,592.8 μs |    402.26 μs |    430.42 μs |    20,507.8 μs |  0.99 |    0.04 |
|              Norm_Tuples_ReaderCallback |   10000 |    17,355.3 μs |    293.08 μs |    300.97 μs |    17,471.3 μs |  0.83 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    25,436.6 μs |    481.64 μs |    450.53 μs |    25,383.3 μs |  1.22 |    0.05 |
|                          Command_Reader |   10000 |    15,755.3 μs |    310.83 μs |    598.87 μs |    15,656.0 μs |  0.77 |    0.05 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |  **100000** |   **216,870.5 μs** |  **4,288.99 μs** |  **4,939.21 μs** |   **216,671.5 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   173,186.1 μs |  3,382.94 μs |  4,278.34 μs |   173,606.9 μs |  0.80 |    0.03 |
|                    Norm_NameValue_Array |  100000 |   181,935.6 μs |  3,252.84 μs |  2,883.55 μs |   182,290.2 μs |  0.84 |    0.02 |
|                Norm_PocoClass_Instances |  100000 |   192,985.6 μs |  3,825.28 μs |  6,495.61 μs |   192,485.2 μs |  0.90 |    0.03 |
|                             Norm_Tuples |  100000 |   179,060.5 μs |  2,682.66 μs |  2,094.44 μs |   179,445.8 μs |  0.83 |    0.01 |
|                       Norm_Named_Tuples |  100000 |   225,811.6 μs |  3,980.67 μs |  5,834.81 μs |   225,722.0 μs |  1.04 |    0.04 |
|                    Norm_Anonymous_Types |  100000 |   244,522.4 μs |  3,255.92 μs |  2,886.29 μs |   244,588.1 μs |  1.13 |    0.02 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   191,398.6 μs |  3,757.31 μs |  6,173.36 μs |   191,210.7 μs |  0.89 |    0.04 |
|              Norm_Tuples_ReaderCallback |  100000 |   179,875.8 μs |  3,187.92 μs |  2,981.98 μs |   179,605.5 μs |  0.83 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   218,557.0 μs |  4,219.47 μs |  6,051.43 μs |   219,170.7 μs |  1.01 |    0.04 |
|                          Command_Reader |  100000 |   159,751.6 μs |  3,075.96 μs |  3,158.79 μs |   159,177.2 μs |  0.74 |    0.02 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** | **1000000** | **2,156,952.7 μs** | **21,417.74 μs** | **20,034.17 μs** | **2,157,620.2 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 1,778,832.3 μs | 34,689.08 μs | 38,556.82 μs | 1,781,673.3 μs |  0.82 |    0.02 |
|                    Norm_NameValue_Array | 1000000 | 1,850,533.5 μs | 13,967.10 μs | 12,381.47 μs | 1,848,987.0 μs |  0.86 |    0.01 |
|                Norm_PocoClass_Instances | 1000000 | 1,699,804.8 μs | 33,655.06 μs | 38,757.21 μs | 1,700,570.4 μs |  0.79 |    0.02 |
|                             Norm_Tuples | 1000000 | 1,810,592.5 μs | 19,861.08 μs | 18,578.06 μs | 1,815,672.8 μs |  0.84 |    0.01 |
|                       Norm_Named_Tuples | 1000000 | 2,119,601.7 μs | 17,774.10 μs | 16,625.90 μs | 2,117,022.9 μs |  0.98 |    0.01 |
|                    Norm_Anonymous_Types | 1000000 | 2,300,904.2 μs | 17,366.94 μs | 15,395.34 μs | 2,305,223.8 μs |  1.07 |    0.01 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 1,710,794.6 μs | 25,248.57 μs | 23,617.53 μs | 1,709,183.3 μs |  0.79 |    0.01 |
|              Norm_Tuples_ReaderCallback | 1000000 | 1,818,130.6 μs | 23,571.21 μs | 22,048.52 μs | 1,813,565.5 μs |  0.84 |    0.01 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 2,036,047.8 μs | 20,351.54 μs | 15,889.14 μs | 2,034,021.7 μs |  0.94 |    0.01 |
|                          Command_Reader | 1000000 | 1,619,176.0 μs | 32,152.32 μs | 33,018.07 μs | 1,608,994.5 μs |  0.75 |    0.02 |


#### Round 2, 2022-10-16, Dapper  2.0.123, Norm 5.3.0

``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.674)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2


```
|                                  Method | Records |           Mean |        Error |       StdDev | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|-------------:|-------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **274.3 μs** |      **5.76 μs** |     **16.98 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       274.5 μs |      5.67 μs |     16.71 μs |  1.00 |    0.08 |
|                    Norm_NameValue_Array |      10 |       248.2 μs |      4.89 μs |      9.42 μs |  0.91 |    0.07 |
|                Norm_PocoClass_Instances |      10 |       261.7 μs |      5.19 μs |     11.06 μs |  0.96 |    0.08 |
|                             Norm_Tuples |      10 |       240.2 μs |      4.28 μs |      4.93 μs |  0.94 |    0.07 |
|                       Norm_Named_Tuples |      10 |       249.0 μs |      4.30 μs |      4.02 μs |  1.01 |    0.03 |
|                    Norm_Anonymous_Types |      10 |       273.4 μs |      5.45 μs |      7.09 μs |  1.05 |    0.09 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       278.6 μs |      5.49 μs |      6.53 μs |  1.08 |    0.08 |
|              Norm_Tuples_ReaderCallback |      10 |       251.9 μs |      4.86 μs |      6.97 μs |  0.96 |    0.08 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       261.9 μs |      4.95 μs |      5.29 μs |  1.04 |    0.07 |
|                          Command_Reader |      10 |       242.1 μs |      2.19 μs |      2.05 μs |  0.98 |    0.03 |
|                                         |         |                |              |              |       |         |
|                                  **Dapper** |    **1000** |     **1,914.2 μs** |     **34.22 μs** |     **47.98 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     1,933.9 μs |     37.61 μs |     55.13 μs |  1.01 |    0.04 |
|                    Norm_NameValue_Array |    1000 |     1,859.5 μs |     34.73 μs |     42.65 μs |  0.97 |    0.03 |
|                Norm_PocoClass_Instances |    1000 |     2,123.7 μs |     42.32 μs |     43.46 μs |  1.11 |    0.03 |
|                             Norm_Tuples |    1000 |     2,028.5 μs |     40.12 μs |     56.24 μs |  1.06 |    0.04 |
|                       Norm_Named_Tuples |    1000 |     2,520.4 μs |     49.89 μs |     71.55 μs |  1.32 |    0.05 |
|                    Norm_Anonymous_Types |    1000 |     2,874.6 μs |     56.73 μs |     53.06 μs |  1.49 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     2,195.7 μs |     42.54 μs |     50.64 μs |  1.14 |    0.04 |
|              Norm_Tuples_ReaderCallback |    1000 |     2,036.4 μs |     18.24 μs |     15.23 μs |  1.05 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     2,775.3 μs |     57.27 μs |    162.47 μs |  1.45 |    0.12 |
|                          Command_Reader |    1000 |     1,786.0 μs |     34.84 μs |     44.06 μs |  0.93 |    0.04 |
|                                         |         |                |              |              |       |         |
|                                  **Dapper** |   **10000** |    **21,025.5 μs** |    **368.29 μs** |    **478.88 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    17,529.8 μs |    345.96 μs |    517.82 μs |  0.83 |    0.04 |
|                    Norm_NameValue_Array |   10000 |    18,241.2 μs |    354.11 μs |    496.41 μs |  0.87 |    0.03 |
|                Norm_PocoClass_Instances |   10000 |    19,100.4 μs |    408.29 μs |  1,068.44 μs |  0.92 |    0.06 |
|                             Norm_Tuples |   10000 |    17,220.0 μs |    344.22 μs |    726.07 μs |  0.82 |    0.04 |
|                       Norm_Named_Tuples |   10000 |    24,462.8 μs |    454.71 μs |    425.34 μs |  1.16 |    0.04 |
|                    Norm_Anonymous_Types |   10000 |    25,913.4 μs |    352.62 μs |    275.30 μs |  1.22 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    20,941.5 μs |    413.62 μs |    619.09 μs |  1.00 |    0.04 |
|              Norm_Tuples_ReaderCallback |   10000 |    17,456.8 μs |    341.25 μs |    531.29 μs |  0.83 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    26,502.7 μs |    311.85 μs |    291.70 μs |  1.25 |    0.02 |
|                          Command_Reader |   10000 |    15,615.4 μs |    302.16 μs |    479.26 μs |  0.75 |    0.03 |
|                                         |         |                |              |              |       |         |
|                                  **Dapper** |  **100000** |   **215,286.8 μs** |  **3,887.69 μs** |  **5,321.51 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   174,462.5 μs |  3,348.62 μs |  3,856.27 μs |  0.81 |    0.03 |
|                    Norm_NameValue_Array |  100000 |   179,994.7 μs |  3,345.05 μs |  3,285.28 μs |  0.84 |    0.03 |
|                Norm_PocoClass_Instances |  100000 |   194,720.8 μs |  3,882.52 μs |  5,690.94 μs |  0.90 |    0.04 |
|                             Norm_Tuples |  100000 |   178,218.1 μs |  2,133.41 μs |  1,781.49 μs |  0.84 |    0.02 |
|                       Norm_Named_Tuples |  100000 |   227,822.3 μs |  4,491.94 μs |  7,867.26 μs |  1.06 |    0.04 |
|                    Norm_Anonymous_Types |  100000 |   252,200.0 μs |  4,943.86 μs |  8,914.80 μs |  1.17 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   195,687.6 μs |  3,805.53 μs |  6,145.22 μs |  0.91 |    0.03 |
|              Norm_Tuples_ReaderCallback |  100000 |   178,147.7 μs |  3,128.18 μs |  2,773.05 μs |  0.83 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   230,739.8 μs |  4,574.88 μs |  8,704.19 μs |  1.08 |    0.05 |
|                          Command_Reader |  100000 |   162,266.1 μs |  3,188.05 μs |  4,868.49 μs |  0.76 |    0.03 |
|                                         |         |                |              |              |       |         |
|                                  **Dapper** | **1000000** | **2,164,212.0 μs** | **20,397.95 μs** | **18,082.25 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 1,761,886.3 μs | 33,103.52 μs | 33,994.87 μs |  0.81 |    0.01 |
|                    Norm_NameValue_Array | 1000000 | 1,826,548.2 μs | 15,658.06 μs | 14,646.56 μs |  0.84 |    0.01 |
|                Norm_PocoClass_Instances | 1000000 | 1,689,767.4 μs | 30,240.79 μs | 47,965.09 μs |  0.79 |    0.02 |
|                             Norm_Tuples | 1000000 | 1,779,124.4 μs |  9,901.91 μs |  8,777.78 μs |  0.82 |    0.01 |
|                       Norm_Named_Tuples | 1000000 | 2,092,909.8 μs |  8,672.25 μs |  8,112.03 μs |  0.97 |    0.01 |
|                    Norm_Anonymous_Types | 1000000 | 2,271,836.3 μs | 16,165.28 μs | 14,330.10 μs |  1.05 |    0.01 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 1,705,750.6 μs | 27,579.03 μs | 25,797.44 μs |  0.79 |    0.02 |
|              Norm_Tuples_ReaderCallback | 1000000 | 1,808,350.0 μs | 19,052.34 μs | 17,821.57 μs |  0.84 |    0.01 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 2,047,625.8 μs | 15,432.34 μs | 14,435.42 μs |  0.95 |    0.01 |
|                          Command_Reader | 1000000 | 1,601,444.3 μs | 21,027.94 μs | 19,669.55 μs |  0.74 |    0.01 |

#### Round 3, 2022-10-16, Dapper  2.0.123, Norm 5.3.0

``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.674)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2


```
|                                  Method | Records |           Mean |        Error |       StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|-------------:|-------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **246.5 μs** |      **4.29 μs** |      **3.58 μs** |       **247.2 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       278.6 μs |      6.99 μs |     20.60 μs |       281.5 μs |  1.12 |    0.05 |
|                    Norm_NameValue_Array |      10 |       261.2 μs |      5.01 μs |      4.92 μs |       261.0 μs |  1.06 |    0.03 |
|                Norm_PocoClass_Instances |      10 |       269.0 μs |      5.37 μs |      6.98 μs |       266.5 μs |  1.10 |    0.04 |
|                             Norm_Tuples |      10 |       242.2 μs |      4.77 μs |      8.23 μs |       244.4 μs |  0.97 |    0.04 |
|                       Norm_Named_Tuples |      10 |       262.2 μs |      5.12 μs |      4.27 μs |       261.4 μs |  1.06 |    0.02 |
|                    Norm_Anonymous_Types |      10 |       261.2 μs |      5.19 μs |     13.32 μs |       260.8 μs |  1.04 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       280.8 μs |      5.35 μs |      6.76 μs |       280.1 μs |  1.15 |    0.02 |
|              Norm_Tuples_ReaderCallback |      10 |       254.0 μs |      4.92 μs |      7.22 μs |       253.0 μs |  1.02 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       266.1 μs |      5.22 μs |      6.21 μs |       265.6 μs |  1.08 |    0.03 |
|                          Command_Reader |      10 |       240.4 μs |      2.36 μs |      2.21 μs |       240.2 μs |  0.98 |    0.02 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |    **1000** |     **1,966.7 μs** |     **33.78 μs** |     **43.93 μs** |     **1,951.7 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     2,002.3 μs |     39.88 μs |     68.79 μs |     1,995.2 μs |  1.03 |    0.04 |
|                    Norm_NameValue_Array |    1000 |     1,933.8 μs |     37.74 μs |     56.48 μs |     1,925.9 μs |  0.98 |    0.03 |
|                Norm_PocoClass_Instances |    1000 |     2,116.9 μs |     40.78 μs |     45.33 μs |     2,109.8 μs |  1.07 |    0.04 |
|                             Norm_Tuples |    1000 |     2,050.0 μs |     27.56 μs |     25.78 μs |     2,049.5 μs |  1.04 |    0.03 |
|                       Norm_Named_Tuples |    1000 |     2,689.9 μs |     51.34 μs |     64.93 μs |     2,689.4 μs |  1.37 |    0.05 |
|                    Norm_Anonymous_Types |    1000 |     2,889.4 μs |     55.52 μs |     70.22 μs |     2,885.4 μs |  1.47 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     2,226.8 μs |     43.14 μs |     49.68 μs |     2,224.9 μs |  1.13 |    0.04 |
|              Norm_Tuples_ReaderCallback |    1000 |     2,054.3 μs |     39.95 μs |     51.94 μs |     2,054.4 μs |  1.05 |    0.04 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     2,598.5 μs |     46.83 μs |     41.51 μs |     2,596.7 μs |  1.31 |    0.04 |
|                          Command_Reader |    1000 |     1,775.3 μs |     19.29 μs |     18.04 μs |     1,771.6 μs |  0.90 |    0.03 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |   **10000** |    **20,675.9 μs** |    **403.12 μs** |    **650.97 μs** |    **20,768.7 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    16,848.9 μs |    333.80 μs |    610.37 μs |    16,932.2 μs |  0.82 |    0.04 |
|                    Norm_NameValue_Array |   10000 |    17,008.1 μs |    331.85 μs |    454.24 μs |    16,879.6 μs |  0.83 |    0.03 |
|                Norm_PocoClass_Instances |   10000 |    22,868.3 μs |  1,807.31 μs |  5,300.54 μs |    19,634.4 μs |  1.10 |    0.30 |
|                             Norm_Tuples |   10000 |    17,104.5 μs |    336.52 μs |    589.39 μs |    17,211.5 μs |  0.83 |    0.04 |
|                       Norm_Named_Tuples |   10000 |    25,931.0 μs |    342.96 μs |    320.81 μs |    25,857.2 μs |  1.26 |    0.05 |
|                    Norm_Anonymous_Types |   10000 |    25,721.4 μs |    498.32 μs |    489.42 μs |    25,815.9 μs |  1.25 |    0.06 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    20,670.0 μs |    397.64 μs |    544.29 μs |    20,459.6 μs |  1.01 |    0.05 |
|              Norm_Tuples_ReaderCallback |   10000 |    17,414.4 μs |    336.72 μs |    400.85 μs |    17,409.4 μs |  0.85 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    24,763.8 μs |    479.83 μs |    718.19 μs |    24,594.2 μs |  1.20 |    0.05 |
|                          Command_Reader |   10000 |    15,407.0 μs |    304.85 μs |    649.66 μs |    15,355.7 μs |  0.75 |    0.04 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |  **100000** |   **216,734.3 μs** |  **4,189.05 μs** |  **5,144.54 μs** |   **216,117.6 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   174,843.4 μs |  3,483.15 μs |  7,347.14 μs |   176,287.9 μs |  0.79 |    0.04 |
|                    Norm_NameValue_Array |  100000 |   181,872.1 μs |  2,500.50 μs |  2,216.63 μs |   182,594.3 μs |  0.84 |    0.02 |
|                Norm_PocoClass_Instances |  100000 |   192,634.0 μs |  3,723.96 μs |  3,824.23 μs |   192,127.5 μs |  0.89 |    0.03 |
|                             Norm_Tuples |  100000 |   176,112.8 μs |  1,782.57 μs |  1,488.53 μs |   175,609.7 μs |  0.81 |    0.02 |
|                       Norm_Named_Tuples |  100000 |   225,552.2 μs |  4,473.16 μs |  8,066.03 μs |   223,499.1 μs |  1.04 |    0.05 |
|                    Norm_Anonymous_Types |  100000 |   248,709.6 μs |  4,832.23 μs |  8,335.36 μs |   247,189.9 μs |  1.15 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   194,542.3 μs |  3,869.20 μs |  6,877.49 μs |   195,261.4 μs |  0.90 |    0.03 |
|              Norm_Tuples_ReaderCallback |  100000 |   178,678.8 μs |  3,448.15 μs |  4,360.81 μs |   177,589.5 μs |  0.83 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   227,536.1 μs |  4,517.89 μs |  6,762.16 μs |   226,812.6 μs |  1.05 |    0.04 |
|                          Command_Reader |  100000 |   160,383.1 μs |  3,124.01 μs |  4,379.42 μs |   160,271.4 μs |  0.74 |    0.03 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** | **1000000** | **2,138,735.1 μs** | **17,736.13 μs** | **16,590.39 μs** | **2,138,604.3 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 1,815,362.4 μs | 25,789.35 μs | 24,123.38 μs | 1,822,391.3 μs |  0.85 |    0.01 |
|                    Norm_NameValue_Array | 1000000 | 1,793,846.5 μs | 23,550.50 μs | 20,876.90 μs | 1,791,900.9 μs |  0.84 |    0.01 |
|                Norm_PocoClass_Instances | 1000000 | 1,680,409.1 μs | 32,601.57 μs | 37,544.02 μs | 1,677,340.2 μs |  0.79 |    0.02 |
|                             Norm_Tuples | 1000000 | 1,778,247.5 μs | 22,219.87 μs | 20,784.48 μs | 1,778,100.8 μs |  0.83 |    0.01 |
|                       Norm_Named_Tuples | 1000000 | 2,098,856.9 μs | 15,821.63 μs | 14,799.56 μs | 2,104,367.2 μs |  0.98 |    0.01 |
|                    Norm_Anonymous_Types | 1000000 | 2,247,997.9 μs | 20,411.61 μs | 19,093.03 μs | 2,249,072.2 μs |  1.05 |    0.01 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 1,713,006.7 μs | 28,907.27 μs | 27,039.88 μs | 1,711,045.8 μs |  0.80 |    0.01 |
|              Norm_Tuples_ReaderCallback | 1000000 | 1,791,911.4 μs | 20,755.98 μs | 19,415.16 μs | 1,792,394.2 μs |  0.84 |    0.01 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 2,080,908.1 μs | 41,434.50 μs | 55,313.86 μs | 2,069,057.5 μs |  0.98 |    0.03 |
|                          Command_Reader | 1000000 | 1,600,191.3 μs | 21,650.89 μs | 20,252.26 μs | 1,599,971.6 μs |  0.75 |    0.01 |
