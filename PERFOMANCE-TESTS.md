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

#### Round 1, 2022-05-07, Dapper  2.0.123, Norm 4.3.0.0

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1645 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.202
  [Host]     : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT
  DefaultJob : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT


```
|                                  Method | Records |           Mean |        Error |       StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|-------------:|-------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **397.3 μs** |      **7.89 μs** |      **9.98 μs** |       **398.8 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       395.5 μs |      7.79 μs |     10.39 μs |       396.5 μs |  1.00 |    0.04 |
|                    Norm_NameValue_Array |      10 |       342.2 μs |      6.52 μs |      6.69 μs |       343.0 μs |  0.86 |    0.03 |
|                Norm_PocoClass_Instances |      10 |       369.7 μs |      7.37 μs |     13.66 μs |       366.2 μs |  0.92 |    0.04 |
|                             Norm_Tuples |      10 |       348.9 μs |      6.16 μs |      8.23 μs |       346.5 μs |  0.88 |    0.03 |
|                       Norm_Named_Tuples |      10 |       359.2 μs |      7.05 μs |     10.98 μs |       355.7 μs |  0.91 |    0.04 |
|                    Norm_Anonymous_Types |      10 |       376.8 μs |      7.37 μs |      9.05 μs |       374.8 μs |  0.95 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       654.7 μs |     66.13 μs |    195.00 μs |       631.7 μs |  1.73 |    0.58 |
|              Norm_Tuples_ReaderCallback |      10 |       368.0 μs |      7.19 μs |     14.53 μs |       367.5 μs |  0.94 |    0.05 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       364.1 μs |      7.18 μs |     13.13 μs |       365.1 μs |  0.92 |    0.04 |
|                          Command_Reader |      10 |       487.2 μs |     20.32 μs |     57.31 μs |       469.7 μs |  1.26 |    0.11 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |    **1000** |     **4,563.8 μs** |     **81.33 μs** |     **76.08 μs** |     **4,571.8 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     4,500.3 μs |     85.19 μs |     91.15 μs |     4,495.7 μs |  0.99 |    0.02 |
|                    Norm_NameValue_Array |    1000 |     4,277.4 μs |     84.90 μs |    101.07 μs |     4,304.3 μs |  0.94 |    0.03 |
|                Norm_PocoClass_Instances |    1000 |     4,425.8 μs |     87.68 μs |    155.85 μs |     4,366.2 μs |  0.99 |    0.04 |
|                             Norm_Tuples |    1000 |     4,377.0 μs |     85.76 μs |    102.09 μs |     4,388.4 μs |  0.96 |    0.03 |
|                       Norm_Named_Tuples |    1000 |     5,164.7 μs |    165.15 μs |    486.96 μs |     5,062.7 μs |  1.03 |    0.08 |
|                    Norm_Anonymous_Types |    1000 |     4,792.4 μs |     94.00 μs |    122.23 μs |     4,777.1 μs |  1.05 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     4,535.9 μs |     89.38 μs |     91.78 μs |     4,525.7 μs |  0.99 |    0.03 |
|              Norm_Tuples_ReaderCallback |    1000 |     4,503.3 μs |     70.32 μs |     54.90 μs |     4,518.0 μs |  0.99 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     4,697.5 μs |     78.03 μs |    109.39 μs |     4,701.0 μs |  1.04 |    0.04 |
|                          Command_Reader |    1000 |     4,429.7 μs |     77.23 μs |     72.24 μs |     4,408.8 μs |  0.97 |    0.02 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |   **10000** |    **74,768.1 μs** |  **2,261.39 μs** |  **6,524.62 μs** |    **75,784.2 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    61,545.1 μs |  1,858.81 μs |  5,480.73 μs |    60,878.5 μs |  0.83 |    0.10 |
|                    Norm_NameValue_Array |   10000 |    55,498.1 μs |  1,394.35 μs |  4,023.02 μs |    55,191.9 μs |  0.75 |    0.09 |
|                Norm_PocoClass_Instances |   10000 |    58,206.7 μs |  1,662.30 μs |  4,822.64 μs |    57,137.1 μs |  0.79 |    0.11 |
|                             Norm_Tuples |   10000 |    58,719.4 μs |  1,487.01 μs |  4,361.14 μs |    58,830.5 μs |  0.79 |    0.09 |
|                       Norm_Named_Tuples |   10000 |    51,204.3 μs |  1,123.63 μs |  3,277.69 μs |    50,287.3 μs |  0.69 |    0.09 |
|                    Norm_Anonymous_Types |   10000 |    51,821.3 μs |  1,031.21 μs |  2,348.59 μs |    51,328.9 μs |  0.71 |    0.08 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    49,620.1 μs |  1,254.60 μs |  3,619.82 μs |    48,763.9 μs |  0.67 |    0.09 |
|              Norm_Tuples_ReaderCallback |   10000 |    47,866.7 μs |  1,109.93 μs |  3,220.11 μs |    47,200.8 μs |  0.65 |    0.08 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    48,474.8 μs |  1,073.21 μs |  3,079.25 μs |    47,722.1 μs |  0.66 |    0.09 |
|                          Command_Reader |   10000 |    43,354.9 μs |    825.76 μs |    732.02 μs |    43,180.1 μs |  0.66 |    0.11 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** |  **100000** |   **517,592.3 μs** | **10,303.92 μs** | **26,963.57 μs** |   **515,878.6 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   453,626.0 μs |  8,968.04 μs | 10,675.82 μs |   456,302.6 μs |  0.86 |    0.05 |
|                    Norm_NameValue_Array |  100000 |   436,469.1 μs |  8,559.18 μs | 10,511.44 μs |   435,301.8 μs |  0.83 |    0.06 |
|                Norm_PocoClass_Instances |  100000 |   446,427.3 μs |  8,891.64 μs | 13,033.25 μs |   444,056.4 μs |  0.86 |    0.06 |
|                             Norm_Tuples |  100000 |   449,197.5 μs |  8,912.20 μs | 12,493.65 μs |   449,216.1 μs |  0.86 |    0.05 |
|                       Norm_Named_Tuples |  100000 |   452,545.4 μs |  8,999.10 μs | 17,338.21 μs |   447,411.5 μs |  0.87 |    0.05 |
|                    Norm_Anonymous_Types |  100000 |   452,893.6 μs |  8,982.65 μs | 13,444.81 μs |   448,454.7 μs |  0.87 |    0.05 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   448,064.5 μs |  8,664.05 μs |  9,270.44 μs |   446,931.0 μs |  0.85 |    0.05 |
|              Norm_Tuples_ReaderCallback |  100000 |   447,579.0 μs |  8,725.74 μs | 15,510.00 μs |   444,928.5 μs |  0.86 |    0.05 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   444,569.6 μs |  8,722.44 μs | 11,341.63 μs |   440,346.3 μs |  0.85 |    0.05 |
|                          Command_Reader |  100000 |   432,500.9 μs |  7,860.05 μs | 12,466.87 μs |   432,333.2 μs |  0.83 |    0.06 |
|                                         |         |                |              |              |                |       |         |
|                                  **Dapper** | **1000000** | **5,042,163.6 μs** | **69,311.71 μs** | **61,443.03 μs** | **5,035,737.7 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 4,562,323.8 μs | 57,230.83 μs | 50,733.64 μs | 4,543,589.0 μs |  0.90 |    0.02 |
|                    Norm_NameValue_Array | 1000000 | 4,386,575.3 μs | 41,167.58 μs | 34,376.80 μs | 4,395,786.4 μs |  0.87 |    0.01 |
|                Norm_PocoClass_Instances | 1000000 | 4,429,048.1 μs | 72,467.56 μs | 64,240.61 μs | 4,424,726.9 μs |  0.88 |    0.01 |
|                             Norm_Tuples | 1000000 | 4,499,020.0 μs | 52,774.08 μs | 46,782.84 μs | 4,497,631.3 μs |  0.89 |    0.02 |
|                       Norm_Named_Tuples | 1000000 | 4,563,444.2 μs | 65,371.60 μs | 54,588.25 μs | 4,563,869.2 μs |  0.91 |    0.02 |
|                    Norm_Anonymous_Types | 1000000 | 4,685,854.1 μs | 82,070.46 μs | 68,532.55 μs | 4,669,185.0 μs |  0.93 |    0.02 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 4,541,690.8 μs | 72,171.48 μs | 60,266.45 μs | 4,552,243.0 μs |  0.90 |    0.02 |
|              Norm_Tuples_ReaderCallback | 1000000 | 4,503,514.5 μs | 37,876.79 μs | 54,321.76 μs | 4,501,891.5 μs |  0.89 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 4,587,768.9 μs | 75,035.22 μs | 62,657.81 μs | 4,582,578.8 μs |  0.91 |    0.01 |
|                          Command_Reader | 1000000 | 4,396,129.7 μs | 49,696.60 μs | 38,799.84 μs | 4,385,285.8 μs |  0.87 |    0.02 |

#### Round 2, 2022-05-07, Dapper  2.0.123, Norm 4.3.0.0

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1645 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.202
  [Host]     : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT
  DefaultJob : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT


```
|                                  Method | Records |           Mean |         Error |        StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|--------------:|--------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **451.8 μs** |       **8.47 μs** |      **21.86 μs** |       **446.3 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       486.7 μs |      18.91 μs |      54.86 μs |       468.2 μs |  1.06 |    0.14 |
|                    Norm_NameValue_Array |      10 |       465.2 μs |      25.92 μs |      73.11 μs |       437.9 μs |  1.04 |    0.17 |
|                Norm_PocoClass_Instances |      10 |       441.9 μs |       8.62 μs |      14.40 μs |       439.0 μs |  0.95 |    0.05 |
|                             Norm_Tuples |      10 |       394.2 μs |       7.61 μs |      10.92 μs |       392.9 μs |  0.84 |    0.05 |
|                       Norm_Named_Tuples |      10 |       401.1 μs |       7.97 μs |      12.18 μs |       401.9 μs |  0.85 |    0.05 |
|                    Norm_Anonymous_Types |      10 |       427.4 μs |       8.37 μs |      14.88 μs |       427.7 μs |  0.92 |    0.06 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       426.4 μs |       8.36 μs |      13.25 μs |       424.9 μs |  0.91 |    0.05 |
|              Norm_Tuples_ReaderCallback |      10 |       407.8 μs |       7.87 μs |       9.96 μs |       405.0 μs |  0.86 |    0.04 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       415.6 μs |       6.74 μs |       6.30 μs |       414.6 μs |  0.89 |    0.04 |
|                          Command_Reader |      10 |       387.5 μs |       7.48 μs |       7.35 μs |       388.9 μs |  0.82 |    0.04 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |    **1000** |     **5,312.6 μs** |      **97.60 μs** |     **214.23 μs** |     **5,231.6 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     5,063.5 μs |      87.27 μs |      72.87 μs |     5,051.2 μs |  0.93 |    0.05 |
|                    Norm_NameValue_Array |    1000 |     4,894.2 μs |      97.14 μs |      90.86 μs |     4,887.5 μs |  0.91 |    0.05 |
|                Norm_PocoClass_Instances |    1000 |     4,960.7 μs |      90.77 μs |      80.47 μs |     4,932.5 μs |  0.92 |    0.05 |
|                             Norm_Tuples |    1000 |     4,949.3 μs |      71.35 μs |      59.58 μs |     4,937.5 μs |  0.91 |    0.04 |
|                       Norm_Named_Tuples |    1000 |     5,191.3 μs |      99.92 μs |     190.12 μs |     5,143.9 μs |  0.97 |    0.04 |
|                    Norm_Anonymous_Types |    1000 |     5,394.6 μs |     115.36 μs |     315.80 μs |     5,273.2 μs |  1.03 |    0.06 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     4,998.7 μs |      92.93 μs |     181.25 μs |     4,940.5 μs |  0.94 |    0.04 |
|              Norm_Tuples_ReaderCallback |    1000 |     4,841.8 μs |      82.97 μs |      73.56 μs |     4,843.9 μs |  0.89 |    0.05 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     5,014.7 μs |      81.87 μs |     109.30 μs |     4,990.6 μs |  0.93 |    0.04 |
|                          Command_Reader |    1000 |     4,734.2 μs |      91.15 μs |     101.31 μs |     4,747.1 μs |  0.88 |    0.04 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |   **10000** |    **56,704.8 μs** |   **1,112.60 μs** |     **986.29 μs** |    **56,482.0 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    48,527.3 μs |     946.29 μs |   2,574.45 μs |    47,923.9 μs |  0.91 |    0.06 |
|                    Norm_NameValue_Array |   10000 |    46,830.2 μs |     903.19 μs |   1,266.15 μs |    46,537.4 μs |  0.83 |    0.02 |
|                Norm_PocoClass_Instances |   10000 |    46,253.1 μs |     901.89 μs |     885.78 μs |    46,104.4 μs |  0.82 |    0.02 |
|                             Norm_Tuples |   10000 |    54,218.4 μs |   2,670.89 μs |   7,620.21 μs |    52,270.3 μs |  1.02 |    0.14 |
|                       Norm_Named_Tuples |   10000 |    47,836.1 μs |     935.69 μs |   1,001.18 μs |    47,645.1 μs |  0.84 |    0.03 |
|                    Norm_Anonymous_Types |   10000 |    48,151.2 μs |     648.05 μs |     574.48 μs |    48,121.7 μs |  0.85 |    0.02 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    53,395.8 μs |   2,394.29 μs |   6,946.28 μs |    53,984.5 μs |  0.87 |    0.06 |
|              Norm_Tuples_ReaderCallback |   10000 |    44,971.1 μs |     855.46 μs |     840.17 μs |    45,127.2 μs |  0.79 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    45,307.9 μs |     802.70 μs |   1,405.87 μs |    45,080.5 μs |  0.81 |    0.03 |
|                          Command_Reader |   10000 |    45,998.8 μs |   1,098.18 μs |   3,079.40 μs |    45,228.2 μs |  0.76 |    0.03 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |  **100000** |   **598,596.8 μs** |  **20,079.85 μs** |  **57,934.97 μs** |   **581,614.7 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   495,248.3 μs |   9,891.24 μs |  23,698.79 μs |   486,067.8 μs |  0.85 |    0.08 |
|                    Norm_NameValue_Array |  100000 |   469,076.8 μs |   6,605.95 μs |   5,856.00 μs |   468,033.7 μs |  0.78 |    0.08 |
|                Norm_PocoClass_Instances |  100000 |   482,242.2 μs |   9,640.35 μs |  20,123.01 μs |   475,747.6 μs |  0.82 |    0.07 |
|                             Norm_Tuples |  100000 |   483,532.4 μs |   9,630.66 μs |  13,812.00 μs |   480,128.2 μs |  0.82 |    0.08 |
|                       Norm_Named_Tuples |  100000 |   486,281.8 μs |   9,424.28 μs |  11,573.87 μs |   482,167.7 μs |  0.81 |    0.08 |
|                    Norm_Anonymous_Types |  100000 |   502,464.7 μs |   9,994.38 μs |  16,698.37 μs |   496,478.0 μs |  0.85 |    0.08 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   485,905.4 μs |   9,676.75 μs |  14,483.70 μs |   482,229.5 μs |  0.83 |    0.08 |
|              Norm_Tuples_ReaderCallback |  100000 |   483,879.8 μs |   9,040.58 μs |   8,014.24 μs |   484,563.3 μs |  0.80 |    0.09 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   493,133.9 μs |   9,773.22 μs |  16,328.86 μs |   487,854.8 μs |  0.84 |    0.08 |
|                          Command_Reader |  100000 |   469,919.8 μs |   8,591.70 μs |  12,859.66 μs |   468,112.0 μs |  0.80 |    0.07 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** | **1000000** | **5,681,930.9 μs** | **107,863.39 μs** | **100,895.48 μs** | **5,634,754.9 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 5,079,619.6 μs |  90,940.32 μs | 124,480.13 μs | 5,031,621.6 μs |  0.90 |    0.02 |
|                    Norm_NameValue_Array | 1000000 | 4,805,726.4 μs |  60,797.71 μs |  53,895.58 μs | 4,788,233.8 μs |  0.85 |    0.02 |
|                Norm_PocoClass_Instances | 1000000 | 4,649,393.8 μs | 120,749.23 μs | 328,507.17 μs | 4,629,184.3 μs |  0.87 |    0.05 |
|                             Norm_Tuples | 1000000 | 4,316,155.6 μs |  47,429.25 μs |  39,605.57 μs | 4,313,978.1 μs |  0.76 |    0.01 |
|                       Norm_Named_Tuples | 1000000 | 4,638,289.9 μs |  87,442.50 μs | 143,670.53 μs | 4,584,867.9 μs |  0.83 |    0.03 |
|                    Norm_Anonymous_Types | 1000000 | 4,716,751.6 μs |  84,715.73 μs | 104,038.52 μs | 4,682,252.2 μs |  0.84 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 4,513,001.9 μs |  42,393.32 μs |  37,580.57 μs | 4,507,323.7 μs |  0.79 |    0.02 |
|              Norm_Tuples_ReaderCallback | 1000000 | 4,463,506.5 μs |  87,311.74 μs |  81,671.46 μs | 4,433,857.2 μs |  0.79 |    0.02 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 4,564,329.9 μs |  52,705.35 μs |  46,721.92 μs | 4,566,484.8 μs |  0.80 |    0.01 |
|                          Command_Reader | 1000000 | 4,420,075.8 μs |  87,524.84 μs | 201,102.87 μs | 4,326,344.3 μs |  0.81 |    0.04 |

#### Round 3, 2022-05-07, Dapper  2.0.123, Norm 4.3.0.0

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1645 (21H2)
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.202
  [Host]     : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT
  DefaultJob : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT


```
|                                  Method | Records |           Mean |         Error |        StdDev |         Median | Ratio | RatioSD |
|---------------------------------------- |-------- |---------------:|--------------:|--------------:|---------------:|------:|--------:|
|                                  **Dapper** |      **10** |       **393.7 μs** |       **7.81 μs** |      **12.62 μs** |       **395.8 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |      10 |       400.6 μs |       7.68 μs |       7.54 μs |       401.3 μs |  1.03 |    0.05 |
|                    Norm_NameValue_Array |      10 |       351.2 μs |       7.00 μs |       6.87 μs |       352.9 μs |  0.90 |    0.03 |
|                Norm_PocoClass_Instances |      10 |       379.2 μs |       7.48 μs |       8.00 μs |       382.4 μs |  0.98 |    0.03 |
|                             Norm_Tuples |      10 |       366.7 μs |       7.14 μs |      11.11 μs |       366.3 μs |  0.93 |    0.05 |
|                       Norm_Named_Tuples |      10 |       371.8 μs |       7.35 μs |       9.03 μs |       370.5 μs |  0.95 |    0.05 |
|                    Norm_Anonymous_Types |      10 |       395.1 μs |       7.75 μs |      11.60 μs |       396.9 μs |  1.01 |    0.04 |
| Norm_PocoClass_Instances_ReaderCallback |      10 |       444.3 μs |       8.88 μs |      23.86 μs |       451.7 μs |  1.09 |    0.07 |
|              Norm_Tuples_ReaderCallback |      10 |       437.1 μs |       8.60 μs |      18.69 μs |       441.5 μs |  1.09 |    0.06 |
|        Norm_Named_Tuples_ReaderCallback |      10 |       443.0 μs |       8.75 μs |      12.55 μs |       445.0 μs |  1.13 |    0.05 |
|                          Command_Reader |      10 |       416.3 μs |       8.18 μs |      11.73 μs |       417.3 μs |  1.06 |    0.05 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |    **1000** |     **5,174.8 μs** |      **98.14 μs** |     **137.57 μs** |     **5,167.8 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |    1000 |     5,025.2 μs |     100.36 μs |     167.68 μs |     5,000.7 μs |  0.97 |    0.04 |
|                    Norm_NameValue_Array |    1000 |     4,862.3 μs |      96.61 μs |     107.38 μs |     4,849.3 μs |  0.94 |    0.03 |
|                Norm_PocoClass_Instances |    1000 |     4,962.7 μs |      87.64 μs |      81.98 μs |     4,949.9 μs |  0.96 |    0.03 |
|                             Norm_Tuples |    1000 |     4,906.2 μs |      92.73 μs |      95.23 μs |     4,899.4 μs |  0.95 |    0.02 |
|                       Norm_Named_Tuples |    1000 |     5,158.9 μs |     101.43 μs |     180.29 μs |     5,142.5 μs |  1.00 |    0.04 |
|                    Norm_Anonymous_Types |    1000 |     5,188.8 μs |      97.76 μs |     100.39 μs |     5,201.1 μs |  1.00 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback |    1000 |     5,131.3 μs |     100.18 μs |     161.78 μs |     5,151.1 μs |  1.00 |    0.04 |
|              Norm_Tuples_ReaderCallback |    1000 |     4,935.9 μs |      96.63 μs |     141.63 μs |     4,908.5 μs |  0.95 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback |    1000 |     5,165.3 μs |     102.00 μs |     196.52 μs |     5,128.9 μs |  1.01 |    0.05 |
|                          Command_Reader |    1000 |     4,772.5 μs |      89.10 μs |     109.43 μs |     4,751.8 μs |  0.92 |    0.03 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |   **10000** |    **59,718.4 μs** |   **1,185.82 μs** |   **3,421.36 μs** |    **59,126.7 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |   10000 |    49,989.0 μs |     994.71 μs |   2,723.00 μs |    49,629.8 μs |  0.84 |    0.06 |
|                    Norm_NameValue_Array |   10000 |    51,850.6 μs |   2,002.45 μs |   5,809.47 μs |    49,918.7 μs |  0.87 |    0.09 |
|                Norm_PocoClass_Instances |   10000 |    47,484.9 μs |     937.36 μs |   1,760.60 μs |    47,549.9 μs |  0.78 |    0.05 |
|                             Norm_Tuples |   10000 |    46,661.8 μs |     932.20 μs |   1,680.96 μs |    46,640.9 μs |  0.77 |    0.05 |
|                       Norm_Named_Tuples |   10000 |    47,953.2 μs |     934.99 μs |   2,511.78 μs |    47,788.2 μs |  0.81 |    0.06 |
|                    Norm_Anonymous_Types |   10000 |    49,420.0 μs |     976.94 μs |   2,450.95 μs |    49,312.3 μs |  0.83 |    0.06 |
| Norm_PocoClass_Instances_ReaderCallback |   10000 |    47,562.2 μs |     926.13 μs |   1,357.51 μs |    47,450.7 μs |  0.77 |    0.04 |
|              Norm_Tuples_ReaderCallback |   10000 |    46,707.2 μs |     933.49 μs |   2,272.25 μs |    46,577.8 μs |  0.78 |    0.05 |
|        Norm_Named_Tuples_ReaderCallback |   10000 |    47,242.8 μs |     938.99 μs |   2,586.25 μs |    46,638.1 μs |  0.79 |    0.07 |
|                          Command_Reader |   10000 |    45,000.8 μs |     893.57 μs |   1,825.33 μs |    44,593.5 μs |  0.75 |    0.06 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** |  **100000** |   **571,345.6 μs** |  **15,123.39 μs** |  **43,875.71 μs** |   **571,688.7 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False |  100000 |   488,917.0 μs |   9,751.67 μs |  27,344.73 μs |   483,611.8 μs |  0.86 |    0.07 |
|                    Norm_NameValue_Array |  100000 |   479,602.2 μs |  10,789.03 μs |  30,606.67 μs |   474,336.8 μs |  0.84 |    0.08 |
|                Norm_PocoClass_Instances |  100000 |   484,329.0 μs |   9,501.31 μs |  26,169.36 μs |   480,128.7 μs |  0.85 |    0.08 |
|                             Norm_Tuples |  100000 |   489,670.3 μs |  11,516.27 μs |  33,775.22 μs |   483,278.4 μs |  0.86 |    0.10 |
|                       Norm_Named_Tuples |  100000 |   489,269.0 μs |   9,898.70 μs |  28,241.53 μs |   487,688.8 μs |  0.86 |    0.09 |
|                    Norm_Anonymous_Types |  100000 |   501,000.2 μs |   9,971.43 μs |  25,380.47 μs |   499,657.3 μs |  0.88 |    0.08 |
| Norm_PocoClass_Instances_ReaderCallback |  100000 |   483,803.2 μs |   9,656.16 μs |  21,397.38 μs |   483,790.8 μs |  0.85 |    0.07 |
|              Norm_Tuples_ReaderCallback |  100000 |   484,570.4 μs |   9,714.34 μs |  28,028.09 μs |   479,248.1 μs |  0.85 |    0.09 |
|        Norm_Named_Tuples_ReaderCallback |  100000 |   488,304.5 μs |   9,759.01 μs |  20,797.25 μs |   486,347.5 μs |  0.85 |    0.07 |
|                          Command_Reader |  100000 |   471,543.2 μs |   9,390.84 μs |  21,764.70 μs |   469,035.5 μs |  0.82 |    0.07 |
|                                         |         |                |               |               |                |       |         |
|                                  **Dapper** | **1000000** | **5,621,874.4 μs** | **110,637.20 μs** | **147,697.48 μs** | **5,590,648.3 μs** |  **1.00** |    **0.00** |
|                   Dapper_Buffered_False | 1000000 | 5,071,006.2 μs | 100,484.91 μs | 134,144.48 μs | 5,042,504.0 μs |  0.90 |    0.02 |
|                    Norm_NameValue_Array | 1000000 | 4,437,573.3 μs |  85,881.26 μs |  98,900.98 μs | 4,404,445.0 μs |  0.79 |    0.03 |
|                Norm_PocoClass_Instances | 1000000 | 4,457,574.7 μs |  83,552.66 μs |  78,155.21 μs | 4,451,731.2 μs |  0.80 |    0.03 |
|                             Norm_Tuples | 1000000 | 4,441,970.4 μs |  88,517.00 μs |  78,468.02 μs | 4,424,374.8 μs |  0.79 |    0.02 |
|                       Norm_Named_Tuples | 1000000 | 4,493,172.3 μs |  74,659.12 μs |  62,343.75 μs | 4,487,570.9 μs |  0.80 |    0.02 |
|                    Norm_Anonymous_Types | 1000000 | 4,575,186.6 μs |  44,548.09 μs |  37,199.67 μs | 4,585,481.3 μs |  0.81 |    0.03 |
| Norm_PocoClass_Instances_ReaderCallback | 1000000 | 4,429,504.4 μs |  39,336.97 μs |  34,871.20 μs | 4,429,399.2 μs |  0.79 |    0.02 |
|              Norm_Tuples_ReaderCallback | 1000000 | 4,413,235.0 μs |  46,866.98 μs |  39,136.05 μs | 4,409,699.9 μs |  0.79 |    0.03 |
|        Norm_Named_Tuples_ReaderCallback | 1000000 | 4,516,674.5 μs |  90,304.02 μs | 110,901.45 μs | 4,493,524.6 μs |  0.80 |    0.03 |
|                          Command_Reader | 1000000 | 4,337,004.8 μs |  74,011.76 μs |  65,609.50 μs | 4,336,996.0 μs |  0.77 |    0.03 |
