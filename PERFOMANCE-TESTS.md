# PERFOMANCE BENCHMARKS

## Query

10 fields wide query on PostgreSQL 13 with parametrized number of records.

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
    from generate_series(1, {records}) as i
```

## Methods

### `Dapper`

Normal Dapper serialization, used as baseline for ratio.

```csharp
foreach (var i in connection.Query<PocoClass>(query))
{
}
```

### `Dapper_Buffered_False`

Unbuffered Dapper serialization.

```csharp
foreach (var i in connection.Query<PocoClass>(query, buffered: false))
{
}
```

### `Norm_NameValue_Array`

Non-generic Read method that yields name and value tuple array.

```csharp
foreach (var i in connection.Read(query))
{
}
```

### `Norm_PocoClass_Instances`

Standard Norm serialization to class instances. Equivalent to unbuffered dapper query.

```csharp
foreach (var i in connection.Read<PocoClass>(query))
{
}
```

### `Norm_Tuples`

Norm serialization to unnamed tuples.

```csharp
foreach (var i in connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query))
{
}
```

### `Norm_Named_Tuples`

Norm serialization to named tuples.

```csharp
foreach (var i in connection.Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query))
{
}
```

### `Command_Reader`

Raw data reader approach.

```csharp
using var command = new NpgsqlCommand(query, connection);
using var reader = command.ExecuteReader();
while (reader.Read())
{
    var id1 = reader.GetInt32(0);
    var foo1 = reader.GetString(1);
    var bar1 = reader.GetString(2);
    var dateTime1 = reader.GetDateTime(3);
    var id2 = reader.GetInt32(4);
    var foo2 = reader.GetString(5);
    var bar2 = reader.GetString(6);
    var dateTime2 = reader.GetDateTime(7);
    var longFooBar = reader.GetString(8);
    var isFooBar = reader.GetBoolean(9);
}
```

## Results
 
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1348 (20H2/October2020Update)

Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores

.NET SDK=6.0.100
- [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
- DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT


|                   Method | Records |           Mean |        Error |       StdDev | Ratio | RatioSD |
------------------------- |-------- |---------------|-------------|-------------|------|--------|
|                   **Dapper** |      **10** |       **307.0 μs** |      **5.00 μs** |      **4.68 μs** |  **1.00** |    **0.00** |
|    Dapper_Buffered_False |      10 |       310.1 μs |      4.58 μs |      4.06 μs |  1.01 |    0.02 |
|     Norm_NameValue_Array |      10 |       283.6 μs |      4.77 μs |      4.46 μs |  0.92 |    0.02 |
| Norm_PocoClass_Instances |      10 |       306.8 μs |      5.98 μs |      6.65 μs |  1.00 |    0.03 |
|              Norm_Tuples |      10 |       286.4 μs |      4.93 μs |      4.37 μs |  0.93 |    0.02 |
|        Norm_Named_Tuples |      10 |       299.8 μs |      5.95 μs |      7.08 μs |  0.98 |    0.03 |
|           Command_Reader |      10 |       280.8 μs |      5.58 μs |      7.45 μs |  0.91 |    0.03 |
|                          |         |                |              |              |       |         |
|                   **Dapper** |    **1000** |     **3,717.5 μs** |     **51.61 μs** |     **48.28 μs** |  **1.00** |    **0.00** |
|    Dapper_Buffered_False |    1000 |     3,631.5 μs |     44.82 μs |     41.93 μs |  0.98 |    0.02 |
|     Norm_NameValue_Array |    1000 |     3,547.7 μs |     42.13 μs |     35.18 μs |  0.95 |    0.02 |
| Norm_PocoClass_Instances |    1000 |     3,530.2 μs |     41.32 μs |     38.65 μs |  0.95 |    0.02 |
|              Norm_Tuples |    1000 |     3,505.7 μs |     37.67 μs |     35.24 μs |  0.94 |    0.02 |
|        Norm_Named_Tuples |    1000 |     3,714.7 μs |     47.36 μs |     44.30 μs |  1.00 |    0.02 |
|           Command_Reader |    1000 |     3,495.9 μs |     49.07 μs |     45.90 μs |  0.94 |    0.02 |
|                          |         |                |              |              |       |         |
|                   **Dapper** |   **10000** |    **41,878.4 μs** |    **707.70 μs** |    **627.36 μs** |  **1.00** |    **0.00** |
|    Dapper_Buffered_False |   10000 |    35,113.6 μs |    649.50 μs |  1,203.88 μs |  0.85 |    0.02 |
|     Norm_NameValue_Array |   10000 |    33,810.6 μs |    494.29 μs |    462.36 μs |  0.81 |    0.02 |
| Norm_PocoClass_Instances |   10000 |    33,968.5 μs |    662.44 μs |    837.78 μs |  0.81 |    0.03 |
|              Norm_Tuples |   10000 |    33,718.4 μs |    515.07 μs |    770.93 μs |  0.81 |    0.02 |
|        Norm_Named_Tuples |   10000 |    34,640.4 μs |    581.49 μs |    543.92 μs |  0.83 |    0.02 |
|           Command_Reader |   10000 |    33,904.4 μs |    663.69 μs |    862.98 μs |  0.81 |    0.02 |
|                          |         |                |              |              |       |         |
|                   **Dapper** |  **100000** |   **412,609.5 μs** |  **9,057.33 μs** | **26,705.74 μs** |  **1.00** |    **0.00** |
|    Dapper_Buffered_False |  100000 |   360,375.9 μs |  7,092.47 μs |  8,167.70 μs |  0.84 |    0.06 |
|     Norm_NameValue_Array |  100000 |   352,967.8 μs |  5,897.84 μs |  5,516.84 μs |  0.81 |    0.06 |
| Norm_PocoClass_Instances |  100000 |   351,799.5 μs |  5,544.45 μs |  4,915.01 μs |  0.81 |    0.06 |
|              Norm_Tuples |  100000 |   350,703.7 μs |  4,810.60 μs |  4,499.84 μs |  0.80 |    0.05 |
|        Norm_Named_Tuples |  100000 |   355,798.9 μs |  6,324.89 μs |  5,916.31 μs |  0.82 |    0.06 |
|           Command_Reader |  100000 |   351,432.3 μs |  6,768.20 μs |  8,311.96 μs |  0.82 |    0.06 |
|                          |         |                |              |              |       |         |
|                   **Dapper** | **1000000** | **4,041,054.1 μs** | **25,880.00 μs** | **21,610.97 μs** |  **1.00** |    **0.00** |
|    Dapper_Buffered_False | 1000000 | 3,732,459.3 μs | 20,529.78 μs | 17,143.30 μs |  0.92 |    0.00 |
|     Norm_NameValue_Array | 1000000 | 3,663,935.6 μs | 42,641.43 μs | 37,800.52 μs |  0.91 |    0.01 |
| Norm_PocoClass_Instances | 1000000 | 3,629,446.6 μs | 34,053.10 μs | 30,187.18 μs |  0.90 |    0.01 |
|              Norm_Tuples | 1000000 | 3,580,865.1 μs | 29,649.73 μs | 27,734.38 μs |  0.89 |    0.01 |
|        Norm_Named_Tuples | 1000000 | 3,636,840.2 μs | 34,555.43 μs | 28,855.35 μs |  0.90 |    0.01 |
|           Command_Reader | 1000000 | 3,587,729.1 μs | 34,892.99 μs | 30,931.72 μs |  0.89 |    0.01 |


