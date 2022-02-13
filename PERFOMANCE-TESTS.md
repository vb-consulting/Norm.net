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

#### Results, Round 1

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1526 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|                                  Method | Records |           Mean |        Error |       StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|-------------:|-------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **317.4 μs** |     **10.27 μs** |     **27.76 μs** |       **309.8 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       305.9 μs |      6.04 μs |      9.23 μs |       306.4 μs |  0.95 |    0.10 |
|                    Norm_NameValue_Array |      10 |       273.6 μs |      5.44 μs |     11.24 μs |       274.9 μs |  0.86 |    0.09 |
|                Norm_PocoClass_Instances |      10 |       300.7 μs |      5.95 μs |     12.41 μs |       299.8 μs |  0.94 |    0.10 |
|                             Norm_Tuples |      10 |       285.0 μs |      5.21 μs |      8.99 μs |       284.0 μs |  0.88 |    0.09 |
|                       Norm_Named_Tuples |      10 |       289.5 μs |      5.72 μs |      7.83 μs |       290.0 μs |  0.89 |    0.10 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       305.9 μs |      5.94 μs |      9.07 μs |       304.5 μs |  0.95 |    0.11 |
|              Norm_Tuples_ReaderCallback |      10 |       285.9 μs |      5.61 μs |      7.68 μs |       286.4 μs |  0.88 |    0.09 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       290.5 μs |      5.81 μs |      5.43 μs |       288.5 μs |  0.85 |    0.11 |
|                          Command_Reader |      10 |       275.3 μs |      5.42 μs |      9.20 μs |       273.4 μs |  0.85 |    0.10 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |    **1000** |     **3,545.6 μs** |     **70.71 μs** |     **69.45 μs** |     **3,522.6 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     3,523.0 μs |     65.95 μs |     67.73 μs |     3,530.9 μs |  0.99 |    0.03 |
|                    Norm_NameValue_Array |    1000 |     3,399.1 μs |     51.94 μs |     46.05 μs |     3,396.9 μs |  0.96 |    0.02 |
|                Norm_PocoClass_Instances |    1000 |     3,466.3 μs |     62.04 μs |     58.03 μs |     3,476.7 μs |  0.98 |    0.02 |
|                             Norm_Tuples |    1000 |     3,498.8 μs |     34.93 μs |     29.17 μs |     3,501.4 μs |  0.99 |    0.02 |
|                       Norm_Named_Tuples |    1000 |     3,600.6 μs |     68.47 μs |     73.26 μs |     3,630.8 μs |  1.01 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     3,548.2 μs |     54.83 μs |     51.28 μs |     3,526.0 μs |  1.00 |    0.02 |
|              Norm_Tuples_ReaderCallback |    1000 |     4,199.5 μs |    121.71 μs |    355.04 μs |     4,123.9 μs |  1.15 |    0.10 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     4,187.5 μs |     91.68 μs |    265.99 μs |     4,117.2 μs |  1.16 |    0.07 |
|                          Command_Reader |    1000 |     3,432.5 μs |     67.92 μs |    169.14 μs |     3,378.1 μs |  1.03 |    0.07 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |   **10000** |    **40,513.3 μs** |    **560.71 μs** |    **497.06 μs** |    **40,618.9 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    32,798.6 μs |    631.35 μs |    590.56 μs |    32,999.8 μs |  0.81 |    0.02 |
|                    Norm_NameValue_Array |   10000 |    31,965.6 μs |    545.32 μs |    455.37 μs |    31,882.7 μs |  0.79 |    0.02 |
|                Norm_PocoClass_Instances |   10000 |    32,307.2 μs |    625.24 μs |    720.03 μs |    32,169.2 μs |  0.80 |    0.02 |
|                             Norm_Tuples |   10000 |    32,340.5 μs |    627.89 μs |    859.46 μs |    32,242.7 μs |  0.80 |    0.02 |
|                       Norm_Named_Tuples |   10000 |    32,479.7 μs |    511.32 μs |    453.27 μs |    32,401.6 μs |  0.80 |    0.01 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    32,953.2 μs |    651.87 μs |    870.22 μs |    32,748.1 μs |  0.82 |    0.02 |
|              Norm_Tuples_ReaderCallback |   10000 |    32,710.3 μs |    633.80 μs |    729.89 μs |    32,550.1 μs |  0.81 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    32,528.2 μs |    520.38 μs |    461.30 μs |    32,418.6 μs |  0.80 |    0.02 |
|                          Command_Reader |   10000 |    31,541.4 μs |    589.19 μs |    492.00 μs |    31,369.2 μs |  0.78 |    0.02 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |  **100000** |   **398,221.6 μs** |  **7,937.11 μs** | **23,278.17 μs** |   **396,273.0 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   346,840.3 μs |  6,769.82 μs |  7,243.64 μs |   348,650.0 μs |  0.87 |    0.05 |
|                    Norm_NameValue_Array |  100000 |   337,878.7 μs |  6,716.14 μs |  8,732.87 μs |   336,242.2 μs |  0.85 |    0.05 |
|                Norm_PocoClass_Instances |  100000 |   338,108.6 μs |  6,515.82 μs |  8,002.01 μs |   336,582.0 μs |  0.85 |    0.06 |
|                             Norm_Tuples |  100000 |   337,788.6 μs |  6,537.47 μs |  8,028.60 μs |   339,190.3 μs |  0.85 |    0.06 |
|                       Norm_Named_Tuples |  100000 |   340,085.2 μs |  6,751.88 μs |  8,037.64 μs |   339,804.1 μs |  0.85 |    0.06 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   343,354.7 μs |  6,700.83 μs | 10,432.38 μs |   342,150.1 μs |  0.86 |    0.06 |
|              Norm_Tuples_ReaderCallback |  100000 |   339,970.7 μs |  6,666.41 μs |  8,186.95 μs |   339,122.2 μs |  0.86 |    0.06 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   351,267.7 μs |  6,959.53 μs | 11,038.56 μs |   348,119.6 μs |  0.89 |    0.07 |
|                          Command_Reader |  100000 |   346,930.8 μs |  6,801.86 μs | 10,387.16 μs |   349,981.1 μs |  0.88 |    0.05 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** | **1000000** | **3,970,934.6 μs** | **42,631.17 μs** | **39,877.22 μs** | **3,967,372.8 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 3,518,555.5 μs | 30,994.97 μs | 28,992.72 μs | 3,525,319.7 μs |  0.89 |    0.01 |
|                    Norm_NameValue_Array | 1000000 | 3,424,080.4 μs | 32,700.94 μs | 30,588.48 μs | 3,420,295.3 μs |  0.86 |    0.01 |
|                Norm_PocoClass_Instances | 1000000 | 3,500,699.0 μs | 35,663.80 μs | 33,359.94 μs | 3,494,653.2 μs |  0.88 |    0.02 |
|                             Norm_Tuples | 1000000 | 3,520,724.2 μs | 69,869.03 μs | 95,637.52 μs | 3,485,324.9 μs |  0.89 |    0.03 |
|                       Norm_Named_Tuples | 1000000 | 3,547,525.9 μs | 50,133.25 μs | 46,894.67 μs | 3,549,032.8 μs |  0.89 |    0.01 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 3,563,635.1 μs | 70,172.10 μs | 93,677.73 μs | 3,548,479.5 μs |  0.90 |    0.03 |
|              Norm_Tuples_ReaderCallback | 1000000 | 3,525,188.5 μs | 29,676.27 μs | 24,781.03 μs | 3,527,487.5 μs |  0.89 |    0.01 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 3,546,173.6 μs | 50,574.72 μs | 42,232.18 μs | 3,536,121.6 μs |  0.89 |    0.01 |
|                          Command_Reader | 1000000 | 3,445,900.4 μs | 43,206.62 μs | 38,301.54 μs | 3,442,127.0 μs |  0.87 |    0.01 |

### Results, Round 2

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1526 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|                                  Method | Records |           Mean |        Error |       StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|-------------:|-------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **308.8 μs** |      **8.09 μs** |     **23.08 μs** |       **310.2 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       306.3 μs |      7.41 μs |     21.73 μs |       304.6 μs |  1.00 |    0.10 |
|                    Norm_NameValue_Array |      10 |       272.5 μs |      5.19 μs |     10.24 μs |       274.7 μs |  0.90 |    0.08 |
|                Norm_PocoClass_Instances |      10 |       324.7 μs |     12.59 μs |     37.11 μs |       311.5 μs |  1.04 |    0.13 |
|                             Norm_Tuples |      10 |       331.5 μs |      6.93 μs |     19.87 μs |       336.0 μs |  1.08 |    0.09 |
|                       Norm_Named_Tuples |      10 |       314.2 μs |     12.17 μs |     35.89 μs |       303.2 μs |  1.03 |    0.15 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       297.3 μs |      5.80 μs |      9.03 μs |       295.6 μs |  0.98 |    0.09 |
|              Norm_Tuples_ReaderCallback |      10 |       283.8 μs |      5.40 μs |      6.00 μs |       284.5 μs |  0.96 |    0.08 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       291.1 μs |      5.79 μs |     11.29 μs |       293.4 μs |  0.96 |    0.08 |
|                          Command_Reader |      10 |       276.8 μs |      4.78 μs |      6.54 μs |       276.1 μs |  0.93 |    0.08 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |    **1000** |     **3,598.7 μs** |     **70.57 μs** |     **78.44 μs** |     **3,599.7 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     3,529.2 μs |     37.07 μs |     32.86 μs |     3,539.5 μs |  0.98 |    0.02 |
|                    Norm_NameValue_Array |    1000 |     3,398.3 μs |     56.49 μs |     52.84 μs |     3,397.9 μs |  0.94 |    0.03 |
|                Norm_PocoClass_Instances |    1000 |     3,407.9 μs |     48.50 μs |     42.99 μs |     3,412.1 μs |  0.95 |    0.03 |
|                             Norm_Tuples |    1000 |     3,466.8 μs |     62.79 μs |     52.43 μs |     3,459.1 μs |  0.96 |    0.03 |
|                       Norm_Named_Tuples |    1000 |     3,584.6 μs |     71.42 μs |     82.25 μs |     3,566.6 μs |  1.00 |    0.04 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     3,468.6 μs |     49.16 μs |     45.99 μs |     3,452.6 μs |  0.96 |    0.02 |
|              Norm_Tuples_ReaderCallback |    1000 |     3,478.6 μs |     55.20 μs |     51.63 μs |     3,478.3 μs |  0.96 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     3,602.4 μs |     72.05 μs |     85.77 μs |     3,588.7 μs |  1.00 |    0.03 |
|                          Command_Reader |    1000 |     3,390.3 μs |     52.46 μs |     49.07 μs |     3,386.8 μs |  0.94 |    0.03 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |   **10000** |    **41,308.7 μs** |    **579.29 μs** |    **513.53 μs** |    **41,294.1 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    33,150.6 μs |    581.45 μs |    646.29 μs |    33,000.3 μs |  0.80 |    0.02 |
|                    Norm_NameValue_Array |   10000 |    32,816.4 μs |    649.38 μs |    773.04 μs |    32,780.3 μs |  0.80 |    0.02 |
|                Norm_PocoClass_Instances |   10000 |    32,857.3 μs |    654.82 μs |    828.13 μs |    32,637.9 μs |  0.80 |    0.02 |
|                             Norm_Tuples |   10000 |    32,818.8 μs |    620.43 μs |    689.60 μs |    32,614.0 μs |  0.80 |    0.02 |
|                       Norm_Named_Tuples |   10000 |    33,370.2 μs |    649.13 μs |    747.53 μs |    33,257.7 μs |  0.81 |    0.02 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    32,949.3 μs |    635.03 μs |    623.69 μs |    32,828.3 μs |  0.80 |    0.02 |
|              Norm_Tuples_ReaderCallback |   10000 |    33,093.4 μs |    603.37 μs |    564.39 μs |    32,920.6 μs |  0.80 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    32,908.7 μs |    454.12 μs |    402.57 μs |    32,997.2 μs |  0.80 |    0.01 |
|                          Command_Reader |   10000 |    32,922.5 μs |    566.66 μs |    530.05 μs |    32,703.9 μs |  0.80 |    0.02 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |  **100000** |   **405,241.4 μs** |  **8,544.10 μs** | **25,192.46 μs** |   **397,900.2 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   342,896.6 μs |  6,648.71 μs |  7,114.05 μs |   342,071.2 μs |  0.86 |    0.06 |
|                    Norm_NameValue_Array |  100000 |   340,816.2 μs |  6,490.92 μs |  6,374.95 μs |   342,838.3 μs |  0.85 |    0.05 |
|                Norm_PocoClass_Instances |  100000 |   340,820.4 μs |  6,789.01 μs |  8,827.63 μs |   343,649.7 μs |  0.85 |    0.06 |
|                             Norm_Tuples |  100000 |   339,489.0 μs |  5,977.23 μs |  5,591.11 μs |   340,394.4 μs |  0.85 |    0.05 |
|                       Norm_Named_Tuples |  100000 |   343,491.1 μs |  6,764.31 μs | 10,124.51 μs |   341,811.3 μs |  0.86 |    0.06 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   346,423.2 μs |  6,925.17 μs |  9,479.25 μs |   345,479.2 μs |  0.87 |    0.06 |
|              Norm_Tuples_ReaderCallback |  100000 |   341,989.2 μs |  6,591.52 μs |  7,846.73 μs |   341,157.3 μs |  0.86 |    0.05 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   345,924.9 μs |  6,790.03 μs |  8,083.05 μs |   347,478.5 μs |  0.87 |    0.05 |
|                          Command_Reader |  100000 |   337,865.6 μs |  6,408.84 μs |  6,581.41 μs |   336,590.8 μs |  0.84 |    0.06 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** | **1000000** | **3,947,159.4 μs** | **28,093.97 μs** | **26,279.11 μs** | **3,943,309.9 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 3,573,211.9 μs | 47,468.25 μs | 44,401.83 μs | 3,569,440.5 μs |  0.91 |    0.01 |
|                    Norm_NameValue_Array | 1000000 | 3,457,844.6 μs | 43,674.68 μs | 40,853.33 μs | 3,451,163.3 μs |  0.88 |    0.01 |
|                Norm_PocoClass_Instances | 1000000 | 3,508,613.1 μs | 31,643.33 μs | 29,599.19 μs | 3,515,847.5 μs |  0.89 |    0.01 |
|                             Norm_Tuples | 1000000 | 3,547,304.5 μs | 42,687.81 μs | 39,930.21 μs | 3,545,085.1 μs |  0.90 |    0.01 |
|                       Norm_Named_Tuples | 1000000 | 3,559,934.1 μs | 48,922.44 μs | 45,762.08 μs | 3,545,923.1 μs |  0.90 |    0.01 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 3,564,099.2 μs | 30,771.91 μs | 28,784.06 μs | 3,565,478.8 μs |  0.90 |    0.01 |
|              Norm_Tuples_ReaderCallback | 1000000 | 3,508,870.4 μs | 34,183.34 μs | 30,302.64 μs | 3,505,219.0 μs |  0.89 |    0.01 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 3,598,256.6 μs | 47,634.69 μs | 42,226.91 μs | 3,590,845.8 μs |  0.91 |    0.01 |
|                          Command_Reader | 1000000 | 3,461,428.5 μs | 48,327.64 μs | 45,205.70 μs | 3,456,567.2 μs |  0.88 |    0.01 |

## 2. Enum mapping tests

Enums are handled a bit differently: 

Both Norm and Dapper can map integers and text database fields to .NET `enum` types.

Norm can map enums to instances or to simple values and unnamed tuples.

**Mapping enums to named tuples is not supported at this point**, but, reader callback can be used to handle such cases.

### Test Query and Test Class

3 fields wide query on PostgreSQL 13 with parametrized number of records.
The real number of records is multipled by four, one for each enum.

Query returns id number and two enum values, one in text one in integer.

- Query:

```sql
select 
    (id * 10) + Value2 as id,
    value1, 
    value2
from
    generate_series(1, {records}) id
    cross join (
        select * from (values
            ('EnumValue1', 1),
            ('EnumValue2', 2),
            ('EnumValue3', 3),
            ('EnumValue4', 4)
        ) t(value1, value2)
    ) e
```

- Test Class and Enume

```csharp
public enum TestEnum { EnumValue1, EnumValue2, EnumValue3, EnumValue4 }

public class TestEnumClass 
{
    public int Id { get; set; }
    public TestEnum Value1 { get; set; }
    public TestEnum Value2 { get; set; }
}
```

### Test Methods

#### `Dapper`

Normal Dapper serialization, used as baseline for ratio.

```csharp
foreach (var i in connection.Query<EnumBenchmarks>(query))
{
}
```

#### `Norm_PocoClass_Instances`

Standard Norm serialization to class instances. Equivalent to unbuffered dapper query.

```csharp
foreach (var i in connection.Read<EnumBenchmarks>(query))
{
}
```

#### `Norm_Tuples`

Norm serialization to unnamed tuples.

```csharp
foreach (var i in connection.Read<int, TestEnum, TestEnum>(query))
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
    var instance = new TestEnumClass
    {
        Id = reader.GetInt32(0),
        Value1 = Enum.Parse<TestEnum>(reader.GetString(1)),
        Value2 = (TestEnum)Enum.ToObject(typeof(TestEnum), reader.GetInt32(1)),
    };
}
```

#### Results, Round 1

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1526 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|                   Method | Records |         Mean |       Error |      StdDev |       Median | Ratio | RatioSD |
|------------------------- |-------- |-------------:|------------:|------------:|-------------:|------:|--------:|
|                   **Dapper** |      **10** |     **282.4 μs** |     **9.27 μs** |    **27.20 μs** |     **274.3 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |      10 |     239.1 μs |     4.73 μs |    13.26 μs |     237.4 μs |  0.85 |    0.08 |
|              Norm_Tuples |      10 |     244.9 μs |     4.90 μs |    11.05 μs |     242.5 μs |  0.85 |    0.09 |
|           Command_Reader |      10 |     225.8 μs |     4.47 μs |     4.97 μs |     224.9 μs |  0.81 |    0.06 |
|                          |         |              |             |             |              |       |         |
|                   **Dapper** |     **100** |     **490.5 μs** |     **9.31 μs** |    **10.35 μs** |     **489.1 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |     100 |     479.7 μs |     5.28 μs |     4.68 μs |     480.8 μs |  0.98 |    0.02 |
|              Norm_Tuples |     100 |     587.3 μs |    11.50 μs |    10.76 μs |     583.7 μs |  1.20 |    0.03 |
|           Command_Reader |     100 |     484.5 μs |     8.57 μs |    10.84 μs |     484.0 μs |  0.99 |    0.03 |
|                          |         |              |             |             |              |       |         |
|                   **Dapper** |    **1000** |   **2,398.4 μs** |    **47.34 μs** |    **46.50 μs** |   **2,402.4 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |    1000 |   2,283.9 μs |    36.06 μs |    30.11 μs |   2,285.7 μs |  0.95 |    0.02 |
|              Norm_Tuples |    1000 |   3,680.0 μs |    63.61 μs |    91.23 μs |   3,679.8 μs |  1.54 |    0.06 |
|           Command_Reader |    1000 |   2,385.8 μs |    38.03 μs |    33.72 μs |   2,380.3 μs |  0.99 |    0.02 |
|                          |         |              |             |             |              |       |         |
|                   **Dapper** |   **10000** |  **23,873.7 μs** |   **401.59 μs** |   **356.00 μs** |  **23,764.8 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |   10000 |  20,642.9 μs |   392.09 μs |   385.08 μs |  20,559.3 μs |  0.86 |    0.02 |
|              Norm_Tuples |   10000 |  34,191.2 μs |   653.58 μs |   872.51 μs |  34,129.5 μs |  1.44 |    0.04 |
|           Command_Reader |   10000 |  21,351.4 μs |   420.26 μs |   412.75 μs |  21,192.8 μs |  0.90 |    0.03 |
|                          |         |              |             |             |              |       |         |
|                   **Dapper** |  **100000** | **264,642.5 μs** | **5,029.85 μs** | **4,458.83 μs** | **264,643.9 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |  100000 | 236,759.0 μs | 3,781.27 μs | 4,354.52 μs | 235,940.0 μs |  0.90 |    0.03 |
|              Norm_Tuples |  100000 | 343,346.3 μs | 6,685.26 μs | 9,371.80 μs | 342,040.1 μs |  1.30 |    0.04 |
|           Command_Reader |  100000 | 215,953.7 μs | 4,077.69 μs | 3,614.77 μs | 214,502.5 μs |  0.82 |    0.02 |


#### Results, Round 2

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1526 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|                   Method | Records |         Mean |       Error |      StdDev |       Median | Ratio | RatioSD |
|------------------------- |-------- |-------------:|------------:|------------:|-------------:|------:|--------:|
|                   **Dapper** |      **10** |     **254.3 μs** |     **4.99 μs** |     **6.31 μs** |     **251.8 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |      10 |     226.6 μs |     4.17 μs |     4.96 μs |     225.6 μs |  0.89 |    0.03 |
|              Norm_Tuples |      10 |     244.1 μs |     4.87 μs |     5.01 μs |     243.3 μs |  0.97 |    0.03 |
|           Command_Reader |      10 |     227.6 μs |     4.50 μs |     4.81 μs |     227.5 μs |  0.90 |    0.03 |
|                          |         |              |             |             |              |       |         |
|                   **Dapper** |     **100** |     **484.4 μs** |     **8.83 μs** |     **8.26 μs** |     **482.0 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |     100 |     481.1 μs |     6.19 μs |     5.79 μs |     481.6 μs |  0.99 |    0.02 |
|              Norm_Tuples |     100 |     595.6 μs |    10.08 μs |     8.93 μs |     593.9 μs |  1.23 |    0.02 |
|           Command_Reader |     100 |     481.0 μs |     5.62 μs |     4.98 μs |     481.3 μs |  0.99 |    0.02 |
|                          |         |              |             |             |              |       |         |
|                   **Dapper** |    **1000** |   **2,399.6 μs** |    **35.56 μs** |    **33.26 μs** |   **2,401.3 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |    1000 |   2,273.1 μs |    32.98 μs |    29.23 μs |   2,269.6 μs |  0.95 |    0.02 |
|              Norm_Tuples |    1000 |   3,730.5 μs |    73.70 μs |   180.79 μs |   3,658.1 μs |  1.68 |    0.07 |
|           Command_Reader |    1000 |   2,361.7 μs |    35.79 μs |    50.17 μs |   2,353.4 μs |  0.99 |    0.03 |
|                          |         |              |             |             |              |       |         |
|                   **Dapper** |   **10000** |  **23,734.6 μs** |   **381.32 μs** |   **338.03 μs** |  **23,690.3 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |   10000 |  20,495.4 μs |   285.43 μs |   293.12 μs |  20,517.6 μs |  0.87 |    0.01 |
|              Norm_Tuples |   10000 |  34,118.2 μs |   674.74 μs | 1,331.88 μs |  33,716.5 μs |  1.45 |    0.06 |
|           Command_Reader |   10000 |  22,394.9 μs |   457.88 μs | 1,350.08 μs |  21,775.5 μs |  1.01 |    0.03 |
|                          |         |              |             |             |              |       |         |
|                   **Dapper** |  **100000** | **268,649.4 μs** | **5,158.58 μs** | **5,519.62 μs** | **268,369.0 μs** |  **1.00** |    **0.00** |
| Norm_PocoClass_Instances |  100000 | 217,145.8 μs | 4,320.94 μs | 6,196.96 μs | 214,969.7 μs |  0.81 |    0.04 |
|              Norm_Tuples |  100000 | 356,541.7 μs | 6,956.04 μs | 9,044.81 μs | 354,679.0 μs |  1.33 |    0.05 |
|           Command_Reader |  100000 | 220,602.8 μs | 4,176.35 μs | 3,906.56 μs | 219,604.6 μs |  0.82 |    0.02 |
