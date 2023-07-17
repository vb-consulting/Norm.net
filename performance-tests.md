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

#### Round 1, 2023-07-17, Dapper  2.0.123, Norm 5.3.7

``` ini

BenchmarkDotNet=v0.13.5, OS=debian 11 (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK=7.0.306
  [Host]     : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2


```
|                                  Method | Records |         Mean |        Error |        StdDev | Ratio | RatioSD |       Gen0 |      Gen1 |      Gen2 |   Allocated | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|--------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
|                                  **Dapper** |      **10** |     **641.0 μs** |     **16.40 μs** |      **47.85 μs** |  **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **6.16 KB** |        **1.00** |
|                   Dapper_Buffered_False |      10 |     636.3 μs |     18.38 μs |      53.89 μs |  1.00 |    0.11 |          - |         - |         - |      5.8 KB |        0.94 |
|                    Norm_NameValue_Array |      10 |     529.9 μs |     15.23 μs |      44.65 μs |  0.83 |    0.10 |     0.9766 |         - |         - |     9.25 KB |        1.50 |
|                Norm_PocoClass_Instances |      10 |     577.0 μs |     17.72 μs |      50.85 μs |  0.91 |    0.11 |     0.9766 |         - |         - |    10.54 KB |        1.71 |
|                             Norm_Tuples |      10 |     501.4 μs |     14.01 μs |      40.63 μs |  0.79 |    0.09 |          - |         - |         - |     3.86 KB |        0.63 |
|                       Norm_Named_Tuples |      10 |     544.3 μs |     16.96 μs |      49.47 μs |  0.85 |    0.10 |     0.9766 |         - |         - |    10.97 KB |        1.78 |
|                    Norm_Anonymous_Types |      10 |     555.6 μs |     14.58 μs |      42.07 μs |  0.87 |    0.09 |     0.9766 |         - |         - |    11.75 KB |        1.91 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |     586.3 μs |     16.39 μs |      46.49 μs |  0.92 |    0.09 |     0.9766 |         - |         - |    11.26 KB |        1.83 |
|              Norm_Tuples_ReaderCallback |      10 |     504.3 μs |     14.13 μs |      40.98 μs |  0.79 |    0.08 |          - |         - |         - |     4.09 KB |        0.67 |
|        Norm_Named_Tuples_ReaderCallback |      10 |     536.7 μs |     16.68 μs |      48.93 μs |  0.84 |    0.10 |     0.9766 |         - |         - |    10.97 KB |        1.78 |
|                          Command_Reader |      10 |     476.2 μs |     14.30 μs |      41.71 μs |  0.75 |    0.08 |          - |         - |         - |     4.85 KB |        0.79 |
|                                         |         |              |              |               |       |         |            |           |           |             |             |
|                                  **Dapper** |    **1000** |   **9,481.1 μs** |    **197.90 μs** |     **580.40 μs** |  **1.00** |    **0.00** |    **46.8750** |   **15.6250** |         **-** |   **429.59 KB** |        **1.00** |
|                   Dapper_Buffered_False |    1000 |   9,602.7 μs |    215.64 μs |     629.04 μs |  1.02 |    0.09 |    46.8750 |         - |         - |   413.37 KB |        0.96 |
|                    Norm_NameValue_Array |    1000 |   9,238.0 μs |    175.94 μs |     507.62 μs |  0.98 |    0.08 |    78.1250 |         - |         - |   687.38 KB |        1.60 |
|                Norm_PocoClass_Instances |    1000 |  10,015.8 μs |    210.24 μs |     613.29 μs |  1.06 |    0.08 |    62.5000 |         - |         - |   596.12 KB |        1.39 |
|                             Norm_Tuples |    1000 |   7,831.5 μs |    155.90 μs |     449.80 μs |  0.83 |    0.07 |    15.6250 |         - |         - |   210.66 KB |        0.49 |
|                       Norm_Named_Tuples |    1000 |   9,729.5 μs |    192.77 μs |     537.35 μs |  1.03 |    0.08 |    93.7500 |         - |         - |   851.85 KB |        1.98 |
|                    Norm_Anonymous_Types |    1000 |  10,962.6 μs |    217.49 μs |     569.14 μs |  1.16 |    0.09 |    93.7500 |         - |         - |   798.39 KB |        1.86 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |  10,323.8 μs |    236.88 μs |     694.72 μs |  1.09 |    0.10 |    78.1250 |         - |         - |   674.34 KB |        1.57 |
|              Norm_Tuples_ReaderCallback |    1000 |   7,897.8 μs |    157.85 μs |     460.45 μs |  0.84 |    0.08 |    15.6250 |         - |         - |   234.07 KB |        0.54 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |  10,088.7 μs |    201.05 μs |     540.11 μs |  1.07 |    0.09 |    93.7500 |         - |         - |   851.99 KB |        1.98 |
|                          Command_Reader |    1000 |   7,073.2 μs |    174.79 μs |     509.87 μs |  0.75 |    0.07 |    31.2500 |         - |         - |   297.17 KB |        0.69 |
|                                         |         |              |              |               |       |         |            |           |           |             |             |
|                                  **Dapper** |   **10000** |  **98,556.0 μs** |  **2,946.93 μs** |   **8,642.83 μs** |  **1.00** |    **0.00** |   **400.0000** |  **200.0000** |         **-** |  **4401.79 KB** |        **1.00** |
|                   Dapper_Buffered_False |   10000 |  82,555.2 μs |  3,041.75 μs |   8,920.93 μs |  0.84 |    0.12 |   500.0000 |         - |         - |  4145.52 KB |        0.94 |
|                    Norm_NameValue_Array |   10000 |  82,010.6 μs |  2,506.85 μs |   7,352.15 μs |  0.84 |    0.10 |   833.3333 |         - |         - |  6875.08 KB |        1.56 |
|                Norm_PocoClass_Instances |   10000 |  87,786.3 μs |  2,449.28 μs |   7,144.68 μs |  0.90 |    0.11 |   600.0000 |         - |         - |  5947.24 KB |        1.35 |
|                             Norm_Tuples |   10000 |  67,468.6 μs |  2,440.43 μs |   7,157.36 μs |  0.69 |    0.09 |   142.8571 |         - |         - |  2112.82 KB |        0.48 |
|                       Norm_Named_Tuples |   10000 |  93,381.0 μs |  2,816.77 μs |   8,305.30 μs |  0.96 |    0.12 |  1000.0000 |         - |         - |  8529.79 KB |        1.94 |
|                    Norm_Anonymous_Types |   10000 |  93,337.8 μs |  2,274.13 μs |   6,524.92 μs |  0.96 |    0.11 |   800.0000 |         - |         - |  7975.97 KB |        1.81 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |  88,716.5 μs |  2,577.07 μs |   7,517.43 μs |  0.91 |    0.11 |   666.6667 |         - |         - |  6728.89 KB |        1.53 |
|              Norm_Tuples_ReaderCallback |   10000 |  70,468.3 μs |  2,765.38 μs |   8,153.79 μs |  0.72 |    0.12 |   166.6667 |         - |         - |  2349.49 KB |        0.53 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |  86,241.2 μs |  3,217.83 μs |   9,437.34 μs |  0.88 |    0.11 |  1000.0000 |         - |         - |     8520 KB |        1.94 |
|                          Command_Reader |   10000 |  59,882.2 μs |  2,581.02 μs |   7,569.68 μs |  0.61 |    0.09 |   333.3333 |         - |         - |  2974.58 KB |        0.68 |
|                                         |         |              |              |               |       |         |            |           |           |             |             |
|                                  **Dapper** |  **100000** | **856,169.1 μs** | **25,935.07 μs** |  **76,063.06 μs** |  **1.00** |    **0.00** |  **6000.0000** | **3000.0000** | **1000.0000** | **44159.67 KB** |        **1.00** |
|                   Dapper_Buffered_False |  100000 | 769,094.9 μs | 33,753.48 μs |  99,522.91 μs |  0.91 |    0.15 |  5000.0000 |         - |         - | 42109.41 KB |        0.95 |
|                    Norm_NameValue_Array |  100000 | 769,056.2 μs | 36,609.30 μs | 107,943.36 μs |  0.90 |    0.16 |  8000.0000 |         - |         - |  69573.9 KB |        1.58 |
|                Norm_PocoClass_Instances |  100000 | 809,771.2 μs | 33,733.28 μs |  99,463.33 μs |  0.96 |    0.14 |  7000.0000 |         - |         - | 60081.74 KB |        1.36 |
|                             Norm_Tuples |  100000 | 613,410.2 μs | 22,969.99 μs |  67,367.00 μs |  0.72 |    0.11 |  2000.0000 |         - |         - | 21804.02 KB |        0.49 |
|                       Norm_Named_Tuples |  100000 | 786,101.7 μs | 36,773.33 μs | 108,427.00 μs |  0.93 |    0.14 | 10000.0000 |         - |         - |  85860.4 KB |        1.94 |
|                    Norm_Anonymous_Types |  100000 | 843,125.0 μs | 30,881.85 μs |  91,055.83 μs |  0.99 |    0.14 |  9000.0000 |         - |         - | 80392.98 KB |        1.82 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 | 855,525.9 μs | 35,508.93 μs | 104,698.89 μs |  1.01 |    0.17 |  8000.0000 |         - |         - | 67939.64 KB |        1.54 |
|              Norm_Tuples_ReaderCallback |  100000 | 650,132.6 μs | 37,800.97 μs | 111,457.02 μs |  0.76 |    0.15 |  2000.0000 |         - |         - | 24140.95 KB |        0.55 |
|        Norm_Named_Tuples_ReaderCallback |  100000 | 809,751.3 μs | 37,590.41 μs | 110,246.18 μs |  0.95 |    0.16 | 10000.0000 |         - |         - |  85860.4 KB |        1.94 |
|                          Command_Reader |  100000 | 549,415.1 μs | 26,458.60 μs |  77,180.97 μs |  0.65 |    0.11 |  3000.0000 |         - |         - | 30390.88 KB |        0.69 |

#### Round 2, 2023-07-17, Dapper  2.0.123, Norm 5.3.7

``` ini

BenchmarkDotNet=v0.13.5, OS=debian 11 (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK=7.0.306
  [Host]     : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2


```
|                                  Method | Records |         Mean |        Error |        StdDev | Ratio | RatioSD |       Gen0 |      Gen1 |      Gen2 |   Allocated | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|--------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
|                                  **Dapper** |      **10** |     **652.2 μs** |     **15.04 μs** |      **44.12 μs** |  **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **6.16 KB** |        **1.00** |
|                   Dapper_Buffered_False |      10 |     659.0 μs |     17.72 μs |      51.40 μs |  1.02 |    0.11 |          - |         - |         - |      5.8 KB |        0.94 |
|                    Norm_NameValue_Array |      10 |     562.0 μs |     19.05 μs |      55.88 μs |  0.86 |    0.09 |     0.9766 |         - |         - |     9.25 KB |        1.50 |
|                Norm_PocoClass_Instances |      10 |     596.5 μs |     15.37 μs |      43.86 μs |  0.92 |    0.08 |     0.9766 |         - |         - |    10.54 KB |        1.71 |
|                             Norm_Tuples |      10 |     509.1 μs |     14.96 μs |      43.63 μs |  0.79 |    0.09 |          - |         - |         - |     3.86 KB |        0.63 |
|                       Norm_Named_Tuples |      10 |     574.3 μs |     21.18 μs |      62.12 μs |  0.89 |    0.12 |     0.9766 |         - |         - |    10.97 KB |        1.78 |
|                    Norm_Anonymous_Types |      10 |     578.6 μs |     15.31 μs |      44.43 μs |  0.89 |    0.09 |     0.9766 |         - |         - |    11.75 KB |        1.91 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |     592.1 μs |     17.01 μs |      48.82 μs |  0.91 |    0.10 |     0.9766 |         - |         - |    11.25 KB |        1.83 |
|              Norm_Tuples_ReaderCallback |      10 |     527.7 μs |     15.50 μs |      45.21 μs |  0.81 |    0.08 |          - |         - |         - |     4.09 KB |        0.67 |
|        Norm_Named_Tuples_ReaderCallback |      10 |     554.7 μs |     14.44 μs |      41.20 μs |  0.86 |    0.08 |     0.9766 |         - |         - |    10.97 KB |        1.78 |
|                          Command_Reader |      10 |     503.6 μs |     13.02 μs |      37.58 μs |  0.78 |    0.08 |          - |         - |         - |     4.85 KB |        0.79 |
|                                         |         |              |              |               |       |         |            |           |           |             |             |
|                                  **Dapper** |    **1000** |   **9,487.9 μs** |    **236.47 μs** |     **693.53 μs** |  **1.00** |    **0.00** |    **46.8750** |   **15.6250** |         **-** |   **429.48 KB** |        **1.00** |
|                   Dapper_Buffered_False |    1000 |   9,367.6 μs |    198.68 μs |     582.69 μs |  0.99 |    0.09 |    46.8750 |         - |         - |   413.49 KB |        0.96 |
|                    Norm_NameValue_Array |    1000 |   9,049.2 μs |    247.19 μs |     728.83 μs |  0.96 |    0.10 |    78.1250 |         - |         - |   687.82 KB |        1.60 |
|                Norm_PocoClass_Instances |    1000 |   9,572.3 μs |    229.65 μs |     669.90 μs |  1.01 |    0.11 |    62.5000 |         - |         - |   596.48 KB |        1.39 |
|                             Norm_Tuples |    1000 |   7,694.4 μs |    191.25 μs |     560.91 μs |  0.82 |    0.09 |    15.6250 |         - |         - |   210.91 KB |        0.49 |
|                       Norm_Named_Tuples |    1000 |   9,770.4 μs |    234.10 μs |     690.25 μs |  1.04 |    0.10 |    93.7500 |         - |         - |   851.91 KB |        1.98 |
|                    Norm_Anonymous_Types |    1000 |  10,322.4 μs |    198.14 μs |     539.07 μs |  1.09 |    0.10 |    93.7500 |         - |         - |   798.45 KB |        1.86 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |  10,054.8 μs |    232.56 μs |     685.70 μs |  1.06 |    0.11 |    78.1250 |         - |         - |   674.31 KB |        1.57 |
|              Norm_Tuples_ReaderCallback |    1000 |   7,988.9 μs |    200.32 μs |     584.35 μs |  0.85 |    0.09 |    15.6250 |         - |         - |   233.78 KB |        0.54 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |   9,885.3 μs |    195.75 μs |     574.10 μs |  1.05 |    0.10 |    93.7500 |         - |         - |   851.84 KB |        1.98 |
|                          Command_Reader |    1000 |   7,069.8 μs |    172.00 μs |     499.00 μs |  0.75 |    0.08 |    31.2500 |         - |         - |    297.2 KB |        0.69 |
|                                         |         |              |              |               |       |         |            |           |           |             |             |
|                                  **Dapper** |   **10000** |  **96,005.9 μs** |  **2,751.56 μs** |   **8,113.03 μs** |  **1.00** |    **0.00** |   **500.0000** |  **166.6667** |         **-** |  **4396.14 KB** |        **1.00** |
|                   Dapper_Buffered_False |   10000 |  80,546.9 μs |  2,755.53 μs |   8,124.74 μs |  0.84 |    0.11 |   428.5714 |         - |         - |  4144.82 KB |        0.94 |
|                    Norm_NameValue_Array |   10000 |  82,703.0 μs |  2,517.31 μs |   7,343.11 μs |  0.87 |    0.11 |   714.2857 |         - |         - |  6878.21 KB |        1.56 |
|                Norm_PocoClass_Instances |   10000 |  90,050.4 μs |  2,103.53 μs |   6,136.11 μs |  0.95 |    0.11 |   714.2857 |         - |         - |  5946.28 KB |        1.35 |
|                             Norm_Tuples |   10000 |  66,944.3 μs |  2,100.18 μs |   6,059.49 μs |  0.70 |    0.09 |   166.6667 |         - |         - |  2111.01 KB |        0.48 |
|                       Norm_Named_Tuples |   10000 |  90,411.7 μs |  2,118.35 μs |   6,179.32 μs |  0.95 |    0.12 |  1000.0000 |         - |         - |  8521.24 KB |        1.94 |
|                    Norm_Anonymous_Types |   10000 |  93,663.2 μs |  2,378.80 μs |   6,976.61 μs |  0.98 |    0.11 |   833.3333 |         - |         - |  7973.71 KB |        1.81 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |  92,490.7 μs |  2,884.33 μs |   8,459.23 μs |  0.97 |    0.11 |   800.0000 |         - |         - |  6727.55 KB |        1.53 |
|              Norm_Tuples_ReaderCallback |   10000 |  69,772.0 μs |  2,286.94 μs |   6,634.84 μs |  0.73 |    0.10 |   250.0000 |         - |         - |  2343.92 KB |        0.53 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |  91,352.5 μs |  2,452.80 μs |   7,232.15 μs |  0.96 |    0.12 |  1000.0000 |         - |         - |  8526.09 KB |        1.94 |
|                          Command_Reader |   10000 |  58,618.7 μs |  2,105.78 μs |   6,075.64 μs |  0.62 |    0.09 |   333.3333 |         - |         - |  2978.54 KB |        0.68 |
|                                         |         |              |              |               |       |         |            |           |           |             |             |
|                                  **Dapper** |  **100000** | **855,875.6 μs** | **25,041.95 μs** |  **73,443.70 μs** |  **1.00** |    **0.00** |  **6000.0000** | **3000.0000** | **1000.0000** | **44161.48 KB** |        **1.00** |
|                   Dapper_Buffered_False |  100000 | 759,733.8 μs | 34,881.88 μs | 102,850.01 μs |  0.89 |    0.14 |  5000.0000 |         - |         - | 42111.02 KB |        0.95 |
|                    Norm_NameValue_Array |  100000 | 772,461.5 μs | 36,415.55 μs | 107,372.06 μs |  0.91 |    0.15 |  8000.0000 |         - |         - | 69486.05 KB |        1.57 |
|                Norm_PocoClass_Instances |  100000 | 849,417.4 μs | 31,040.41 μs |  90,546.32 μs |  1.00 |    0.14 |  7000.0000 |         - |         - | 60140.14 KB |        1.36 |
|                             Norm_Tuples |  100000 | 671,862.2 μs | 36,283.02 μs | 106,411.83 μs |  0.79 |    0.15 |  2000.0000 |         - |         - | 21892.61 KB |        0.50 |
|                       Norm_Named_Tuples |  100000 | 839,390.2 μs | 29,003.31 μs |  85,516.90 μs |  0.99 |    0.13 | 10000.0000 |         - |         - | 85860.58 KB |        1.94 |
|                    Norm_Anonymous_Types |  100000 | 890,519.7 μs | 28,916.43 μs |  83,891.82 μs |  1.05 |    0.15 |  9000.0000 |         - |         - |  80393.7 KB |        1.82 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 | 887,689.5 μs | 29,901.94 μs |  88,166.55 μs |  1.05 |    0.13 |  8000.0000 |         - |         - | 67894.18 KB |        1.54 |
|              Norm_Tuples_ReaderCallback |  100000 | 668,084.5 μs | 35,305.25 μs | 102,987.07 μs |  0.79 |    0.14 |  2000.0000 |         - |         - | 24140.01 KB |        0.55 |
|        Norm_Named_Tuples_ReaderCallback |  100000 | 805,646.6 μs | 38,855.47 μs | 114,566.22 μs |  0.95 |    0.16 | 10000.0000 |         - |         - |  85860.4 KB |        1.94 |
|                          Command_Reader |  100000 | 542,140.1 μs | 26,318.79 μs |  75,513.54 μs |  0.64 |    0.11 |  3000.0000 |         - |         - | 30450.87 KB |        0.69 |

#### Round 3, 2023-07-17, Dapper  2.0.123, Norm 5.3.7

``` ini

BenchmarkDotNet=v0.13.5, OS=debian 11 (container)
Intel Xeon Processor (Skylake, IBRS), 1 CPU, 2 logical and 2 physical cores
.NET SDK=7.0.306
  [Host]     : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2


```
|                                  Method | Records |         Mean |        Error |        StdDev | Ratio | RatioSD |       Gen0 |      Gen1 |      Gen2 |   Allocated | Alloc Ratio |
|---------------------------------------- |-------- |-------------:|-------------:|--------------:|------:|--------:|-----------:|----------:|----------:|------------:|------------:|
|                                  **Dapper** |      **10** |     **657.6 μs** |     **16.77 μs** |      **49.44 μs** |  **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **6.16 KB** |        **1.00** |
|                   Dapper_Buffered_False |      10 |     666.7 μs |     14.13 μs |      41.21 μs |  1.02 |    0.10 |          - |         - |         - |      5.8 KB |        0.94 |
|                    Norm_NameValue_Array |      10 |     532.6 μs |     13.01 μs |      37.94 μs |  0.81 |    0.09 |     0.9766 |         - |         - |     9.25 KB |        1.50 |
|                Norm_PocoClass_Instances |      10 |     592.9 μs |     19.42 μs |      56.65 μs |  0.91 |    0.11 |     0.9766 |         - |         - |    10.54 KB |        1.71 |
|                             Norm_Tuples |      10 |     534.1 μs |     14.01 μs |      40.44 μs |  0.82 |    0.08 |          - |         - |         - |     3.86 KB |        0.63 |
|                       Norm_Named_Tuples |      10 |     557.8 μs |     12.49 μs |      36.64 μs |  0.85 |    0.08 |     0.9766 |         - |         - |    10.97 KB |        1.78 |
|                    Norm_Anonymous_Types |      10 |     567.5 μs |     14.94 μs |      43.35 μs |  0.87 |    0.09 |     0.9766 |         - |         - |    11.75 KB |        1.91 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |     572.3 μs |     15.37 μs |      44.59 μs |  0.87 |    0.08 |     0.9766 |         - |         - |    11.26 KB |        1.83 |
|              Norm_Tuples_ReaderCallback |      10 |     511.5 μs |     16.64 μs |      48.55 μs |  0.78 |    0.09 |          - |         - |         - |     4.09 KB |        0.66 |
|        Norm_Named_Tuples_ReaderCallback |      10 |     551.0 μs |     16.50 μs |      48.39 μs |  0.84 |    0.10 |     0.9766 |         - |         - |    10.97 KB |        1.78 |
|                          Command_Reader |      10 |     487.4 μs |     13.38 μs |      39.02 μs |  0.74 |    0.08 |          - |         - |         - |     4.85 KB |        0.79 |
|                                         |         |              |              |               |       |         |            |           |           |             |             |
|                                  **Dapper** |    **1000** |   **9,182.0 μs** |    **215.06 μs** |     **630.73 μs** |  **1.00** |    **0.00** |    **46.8750** |   **15.6250** |         **-** |   **429.93 KB** |        **1.00** |
|                   Dapper_Buffered_False |    1000 |   9,238.8 μs |    195.28 μs |     572.71 μs |  1.01 |    0.10 |    46.8750 |         - |         - |    413.6 KB |        0.96 |
|                    Norm_NameValue_Array |    1000 |   9,075.5 μs |    188.85 μs |     544.88 μs |  0.99 |    0.09 |    78.1250 |         - |         - |   687.77 KB |        1.60 |
|                Norm_PocoClass_Instances |    1000 |  10,281.4 μs |    207.82 μs |     606.23 μs |  1.12 |    0.10 |    62.5000 |         - |         - |   596.13 KB |        1.39 |
|                             Norm_Tuples |    1000 |   7,949.1 μs |    163.03 μs |     472.97 μs |  0.87 |    0.09 |    15.6250 |         - |         - |   210.62 KB |        0.49 |
|                       Norm_Named_Tuples |    1000 |  10,065.2 μs |    201.16 μs |     547.27 μs |  1.10 |    0.09 |    93.7500 |         - |         - |   851.78 KB |        1.98 |
|                    Norm_Anonymous_Types |    1000 |  10,835.0 μs |    237.23 μs |     680.65 μs |  1.18 |    0.10 |    93.7500 |         - |         - |   798.45 KB |        1.86 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |  10,782.1 μs |    213.11 μs |     440.11 μs |  1.18 |    0.09 |    78.1250 |         - |         - |   674.22 KB |        1.57 |
|              Norm_Tuples_ReaderCallback |    1000 |   8,083.7 μs |    184.72 μs |     530.00 μs |  0.88 |    0.09 |    15.6250 |         - |         - |    234.3 KB |        0.54 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |   9,974.9 μs |    199.32 μs |     521.58 μs |  1.10 |    0.10 |    93.7500 |         - |         - |   851.74 KB |        1.98 |
|                          Command_Reader |    1000 |   7,160.0 μs |    156.50 μs |     449.02 μs |  0.78 |    0.07 |    31.2500 |         - |         - |   296.91 KB |        0.69 |
|                                         |         |              |              |               |       |         |            |           |           |             |             |
|                                  **Dapper** |   **10000** | **100,807.0 μs** |  **2,216.14 μs** |   **6,429.43 μs** |  **1.00** |    **0.00** |   **400.0000** |  **200.0000** |         **-** |  **4409.99 KB** |        **1.00** |
|                   Dapper_Buffered_False |   10000 |  84,970.5 μs |  2,626.84 μs |   7,620.94 μs |  0.85 |    0.10 |   500.0000 |         - |         - |  4139.77 KB |        0.94 |
|                    Norm_NameValue_Array |   10000 |  81,967.2 μs |  2,815.48 μs |   8,257.31 μs |  0.82 |    0.10 |   833.3333 |         - |         - |  6889.58 KB |        1.56 |
|                Norm_PocoClass_Instances |   10000 |  89,898.2 μs |  2,541.28 μs |   7,453.12 μs |  0.90 |    0.10 |   666.6667 |         - |         - |  5940.22 KB |        1.35 |
|                             Norm_Tuples |   10000 |  67,782.6 μs |  2,546.88 μs |   7,509.53 μs |  0.67 |    0.08 |   250.0000 |         - |         - |  2110.33 KB |        0.48 |
|                       Norm_Named_Tuples |   10000 |  90,459.5 μs |  2,583.82 μs |   7,496.14 μs |  0.90 |    0.09 |  1000.0000 |         - |         - |  8525.76 KB |        1.93 |
|                    Norm_Anonymous_Types |   10000 |  94,617.6 μs |  2,372.36 μs |   6,920.29 μs |  0.94 |    0.10 |   833.3333 |         - |         - |   7978.3 KB |        1.81 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |  90,825.9 μs |  2,860.03 μs |   8,387.96 μs |  0.91 |    0.11 |   800.0000 |         - |         - |  6729.92 KB |        1.53 |
|              Norm_Tuples_ReaderCallback |   10000 |  68,385.6 μs |  2,967.64 μs |   8,703.57 μs |  0.68 |    0.10 |   166.6667 |         - |         - |  2347.72 KB |        0.53 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |  89,067.7 μs |  2,621.68 μs |   7,688.93 μs |  0.89 |    0.10 |  1000.0000 |         - |         - |  8518.93 KB |        1.93 |
|                          Command_Reader |   10000 |  59,975.5 μs |  2,576.96 μs |   7,557.78 μs |  0.60 |    0.08 |   250.0000 |         - |         - |  2974.03 KB |        0.67 |
|                                         |         |              |              |               |       |         |            |           |           |             |             |
|                                  **Dapper** |  **100000** | **888,816.8 μs** | **23,056.88 μs** |  **67,621.85 μs** |  **1.00** |    **0.00** |  **6000.0000** | **3000.0000** | **1000.0000** | **44228.84 KB** |        **1.00** |
|                   Dapper_Buffered_False |  100000 | 776,828.3 μs | 35,493.46 μs | 104,096.18 μs |  0.88 |    0.13 |  5000.0000 |         - |         - | 42161.52 KB |        0.95 |
|                    Norm_NameValue_Array |  100000 | 784,239.5 μs | 31,260.13 μs |  92,171.21 μs |  0.89 |    0.13 |  8000.0000 |         - |         - | 69458.74 KB |        1.57 |
|                Norm_PocoClass_Instances |  100000 | 846,063.6 μs | 31,032.10 μs |  91,498.86 μs |  0.96 |    0.13 |  7000.0000 |         - |         - |  60114.8 KB |        1.36 |
|                             Norm_Tuples |  100000 | 640,229.8 μs | 38,592.40 μs | 113,184.83 μs |  0.73 |    0.15 |  2000.0000 |         - |         - |  21797.2 KB |        0.49 |
|                       Norm_Named_Tuples |  100000 | 837,526.2 μs | 30,962.55 μs |  91,293.78 μs |  0.95 |    0.12 | 10000.0000 |         - |         - | 85914.66 KB |        1.94 |
|                    Norm_Anonymous_Types |  100000 | 904,078.1 μs | 28,422.46 μs |  83,358.17 μs |  1.02 |    0.13 |  9000.0000 |         - |         - | 80470.78 KB |        1.82 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 | 893,867.2 μs | 28,245.64 μs |  83,282.91 μs |  1.01 |    0.12 |  8000.0000 |         - |         - | 67894.18 KB |        1.54 |
|              Norm_Tuples_ReaderCallback |  100000 | 690,811.8 μs | 30,956.54 μs |  91,276.05 μs |  0.78 |    0.11 |  2000.0000 |         - |         - | 24173.83 KB |        0.55 |
|        Norm_Named_Tuples_ReaderCallback |  100000 | 845,814.9 μs | 34,146.43 μs |  99,606.74 μs |  0.96 |    0.13 | 10000.0000 |         - |         - |  85860.4 KB |        1.94 |
|                          Command_Reader |  100000 | 612,265.7 μs | 23,841.80 μs |  66,854.96 μs |  0.68 |    0.09 |  3000.0000 |         - |         - | 30473.35 KB |        0.69 |

