# PERFOMANCE BENCHMARKS

[Benchmark code](https://github.com/vb-consulting/Norm.net/tree/master/BenchmarksConsole) is included in the project, you can run it manually to repeat these tests.

Query used for testing doesn't use any tables and it runs on the local PostgreSQL server. 
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

Results are serialized to Plain Old CLR Object (POCO) class and to Record stucture:

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

Norm also tests serialziation to enumaerable of named tuples:

```csharp
IEnumerable<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)> normTuples = 
    connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query).ToList();
```

All results are in seconds.

## Test 1

Serialization to `PocoClass` and `Record` by Dapper and Norm.

### Round 1 

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|Norm TUPLES|
|-|-----------|-------------|---------|-----------|-----------|
|1|05.1455272|03.9596666|03.4586582|03.5254997|03.4465898|
|2|03.9127349|04.0604935|03.6523312|03.7304884|03.4452352|
|3|03.9899153|03.9286251|03.5286826|03.6291740|03.5777153|
|4|04.0091647|03.9750749|03.5550664|03.6381423|03.7025510|
|5|03.9914741|03.9973090|03.6203395|03.6811963|03.6031815|
|6|04.0033919|04.0894141|03.6323265|03.8238115|03.6016093|
|7|04.1000903|04.0996135|03.6062704|03.5542624|03.7798762|
|8|04.1079846|04.0092054|03.6009532|03.7677789|03.6027011|
|9|04.0380553|04.1226816|03.6207815|03.6373508|03.6326630|
|10|04.0739952|04.2214659|03.6374645|03.7548517|03.6997661|
|**AVG**|**04.1372333**|**04.0463549**|**03.5912874**|**03.6742556**|**03.6091888**|

### Round 2

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|Norm TUPLES|
|-|-----------|-------------|---------|-----------|-----------|
|1|04.4183058|04.0400551|03.6035059|03.5342966|03.5998133|
|2|04.0776843|04.1613309|03.6617505|03.6062409|03.6761450|
|3|04.0100561|04.1227152|03.6369744|03.6431526|03.5902006|
|4|04.0562275|04.1633928|03.6676013|03.7270111|03.6136308|
|5|04.1058344|04.2600236|03.7402851|03.6845394|03.7013643|
|6|04.0811819|04.2080351|03.7400821|03.7061795|03.7041709|
|7|04.0657357|04.1257624|03.6972810|03.8183189|04.0077544|
|8|04.2049648|04.3156084|03.7284612|03.8512080|03.6735182|
|9|04.1641771|04.1812895|03.7368243|03.7629159|03.7421254|
|10|04.0668816|04.2097603|03.7384304|03.6819568|03.7096981|
|**AVG**|**04.1251049**|**04.1787973**|**03.6951196**|**03.7015819**|**03.7018421**|

### Round 3

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|Norm TUPLES|
|-|-----------|-------------|---------|-----------|-----------|
|1|04.4054878|04.0487712|03.6454937|03.6402207|03.7045746|
|2|04.0880753|04.1383219|03.7169228|03.7276631|03.6746465|
|3|04.1256318|04.1814120|03.8281930|03.8168590|04.0225428|
|4|04.2561417|04.1598631|03.6934548|03.8693414|03.7593348|
|5|04.1496276|04.2165020|03.7670548|03.7836193|03.7320108|
|6|04.1523712|04.1592908|03.7424341|03.6948336|03.7423826|
|7|04.1755777|04.1720562|03.6863557|03.7711186|03.8087274|
|8|04.2017810|04.2390583|03.6904897|03.6592988|03.7527307|
|9|04.2395556|04.2455513|03.6225669|03.7304140|03.7444495|
|10|04.1780416|04.1324062|03.7494919|03.6274571|03.7870099|
|AVG|**04.1972291**|**04.1693233**|**03.7142457**|**03.7320825**|**03.7728409**|


Serialization to named tuples enumerable doesn't use any mapping logic (no reflection and no caching) and
it is fast as raw data reader.

Results indicate that mapping to POCO or Record are indistinguishable from raw data reader. They are just as fast.

## Test 2

These tests will convert serialized results to `Dictionary<int, DateTime>`
where the key is populated by field `Id1` and value is `DateTime1` field from results.

- Dapper record query will seralize to record and use LINQ to convert to dictionary:

```cshap
connection.Query<Record>(query).ToDictionary(r => r.Id1, r => r.DateTime1);
```

- Norm tuples will not serialize at all, it will use tuple array results and reference fields by index:

```cshap
connection.Read(query).ToDictionary(t => t[0].value, t => (DateTime)t[3].value);
```

- Norm record query will serialize to record and use LINQ to convert to the dictionary, same as Dapper. 
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


Difference is slightly higher this time because Norm builds internal enumerator and skips unneccessary iteration.