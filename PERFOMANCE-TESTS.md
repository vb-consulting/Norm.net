# PERFOMANCE BENCHMARKS

[Benchmark code](https://github.com/vb-consulting/Norm.net/tree/master/BenchmarksConsole) is included in the project, you can run it manually to repeat these tests.

Query used for testing doesn't use any tables and it runs on local PostgreSQL server. 
It generates series of 1 million records an returns 10 fields of various types for each record:

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
    'long_foo_bar_' || (i+2)::text as longfoobar, -- dapper doesn't understands the snake case
    (i % 2)::boolean as isfoobar
from generate_series(1, 1000000) as i
```

Results are serialized to one class amnd one record used for testing:

```csharp
class PocoClass
{
    public int Id1 { get; set; }
    public string Foo1 { get; set; }
    public string Bar1 { get; set; }
    public DateTime DateTime1 { get; set; }
    public int Id2 { get; set; }
    public string Foo2 { get; set; }
    public string Bar2 { get; set; }
    public DateTime DateTime2 { get; set; }
    public string LongFooBar { get; set; }
    public bool IsFooBar { get; set; }
}

record Record(int Id1, string Foo1, string Bar1, DateTime DateTime1, int Id2, string Foo2, string Bar2, DateTime DateTime2, string LongFooBar, bool IsFooBar);
```

All results are in seconds.

## Test 1

Serialization to `PocoClass` and `Record` by Dapper and Norm.

### Round 1 

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|
|-|-----------|-------------|---------|-----------|
|1|05.2315907|04.1006906|03.5621702|03.5857530|
|2|03.9253157|03.9943038|03.6192553|03.5963830|
|3|03.9219931|04.0114606|03.6580044|03.8893121|
|4|04.0293532|04.0431813|03.6315226|03.5396274|
|5|03.8667698|04.0996654|03.6381734|03.6189072|
|6|03.9684965|04.0429242|03.5706624|03.6161385|
|7|03.9828915|04.1039182|03.6976398|03.6453438|
|8|04.3634040|04.1289492|03.6189972|03.6192946|
|9|04.0023115|04.0544611|03.6016949|03.5856133|
|10|03.9533691|04.0051033|03.5925382|03.5705393|
|**AVG**|**04.1245495**|**04.0584657**|**03.6190658**|**03.6266912**|

### Round 2

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|
|-|-----------|-------------|---------|-----------|
|1|04.1481317|03.8669305|03.3785824|03.4899212|
|2|03.8112111|04.1876449|03.5246086|03.4747038|
|3|03.8389118|03.8985847|03.5585125|03.5868675|
|4|03.8996247|03.9650030|03.5141023|03.4983895|
|5|03.9239912|03.9981311|03.5309641|03.5696853|
|6|03.8802493|03.9806347|03.8671620|03.8111448|
|7|03.9897228|04.0363655|03.5369584|03.5440338|
|8|03.9545117|03.9032082|03.5447936|03.6488062|
|9|03.9352990|03.9764827|03.5409397|03.6164543|
|10|03.9471273|04.0079026|03.6181917|03.8860504|
|**AVG**|**03.9328780**|**03.9820887**|**03.5614815**|**03.6126056**|

### Round 3

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|
|-|-----------|-------------|---------|-----------|
|1|04.2220060|03.8852373|03.5303164|03.5138871|
|2|03.8768922|03.9507294|03.5352218|03.5247298|
|3|03.8675217|04.0007740|03.7019054|03.8921423|
|4|04.1089727|04.0632187|03.6902531|03.5846946|
|5|03.9475909|04.0052973|03.6349816|03.5916114|
|6|03.8972927|04.0414167|03.5844300|03.5291884|
|7|04.0240892|04.0805240|03.6331303|03.5558175|
|8|04.2335362|04.0651840|03.6304646|03.5715216|
|9|03.9011557|04.0391390|03.6045675|03.5232020|
|10|03.9098892|04.0428101|03.7490736|03.6192526|
|**AVG**|**03.9988946**|**04.0174330**|**03.6294344**|**03.5906047**|

## Test 2

These tests will convert serliazed results to `Dictionary<int, DateTime>`
where key is populated by field `Id1` and value is `DateTime1` field from results.

- Dapper record query will seralize to record and use LINQ to convert to dictionary:

```cshap
connection.Query<Record>(query).ToDictionary(r => r.Id1, r => r.DateTime1);
```

- Norm tuples will not serialize at all, it will use tuple array results and reference fields by index:

```cshap
connection.Read(query).ToDictionary(t => t[0].value, t => (DateTime)t[3].value);
```

- Norm record query will seralize to record and use LINQ to convert to dictionary, same as Dapper. 
However, Norm generates iterators internally instead of list, so iteration should occur only once and difference should be slightly higher:

```cshap
connection.Query<Record>(query).ToDictionary(r => r.Id1, r => r.DateTime1);
```

### Round 1 

|#|Dapper POCO to Dict|Norm TUPLES to Dict|Norm POCO to Dict|
|-|-------------------|-------------------|-----------------|
|1|04.8561901|03.5874758|03.3690820|
|2|03.9389593|03.6437820|03.6143264|
|3|03.9447737|03.4281739|03.4413731|
|4|04.1336856|03.6695639|03.8774125|
|5|03.9277426|03.7429747|03.4478531|
|6|03.9239840|03.4986394|03.4742715|
|7|03.9278891|03.5207011|03.4864901|
|8|03.9129620|03.6693034|03.7005085|
|9|03.9398544|03.6987456|03.6902129|
|10|03.9375266|03.7351398|03.6683761|
|**AVG**|**04.0443567**|**03.6194499**|**03.5769906**|

### Round 2

|#|Dapper POCO to Dict|Norm TUPLES to Dict|Norm POCO to Dict|
|-|-------------------|-------------------|-----------------|
|1|04.6227946|06.1475540|03.5079053|
|2|04.1975670|03.6359537|03.7032145|
|3|04.6909435|03.5011946|03.6465795|
|4|04.0098895|03.7077762|03.7844054|
|5|05.5719534|03.7498396|03.9638440|
|6|04.2072972|03.6434484|03.6408465|
|7|04.1836771|03.9141761|03.7007655|
|8|05.2606700|04.0035003|03.6510496|
|9|03.9552342|03.5369438|03.4982962|
|10|03.9153144|03.5190601|03.5410003|
|**AVG**|**04.4615340**|**03.9359446**|**03.6637906**|

### Round 3

|#|Dapper POCO to Dict|Norm TUPLES to Dict|Norm POCO to Dict|
|-|-------------------|-------------------|-----------------|
|1|04.4954343|03.4703449|03.4129177|
|2|04.1256940|03.6523199|03.5715662|
|3|03.9646072|03.4536014|03.5365319|
|4|03.8964883|03.4867826|03.4791681|
|5|03.9772121|03.5229636|03.5298906|
|6|03.9594684|03.5044716|03.5415191|
|7|03.9338210|03.5131341|03.3827119|
|8|04.4390730|03.6169347|03.5215499|
|9|03.9517431|03.4543346|03.6411662|
|10|04.0103395|03.5382665|03.5217174|
|**AVG**|**04.0753880**|**03.5213153**|**03.5138739**|
