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

Norm also tests serialziation to enumerable of named tuples:

```csharp
IEnumerable<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)> normTuples = 
    connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query).ToList();
```

All results are in seconds.

## Test 1

Serialization to `PocoClass` and `Record` by Dapper and Norm.

### Round 1 

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|Norm TUPLES|Raw DataReader|
|-|-----------|-------------|---------|-----------|-----------|--------------|
|1|05.8025930|03.9257842|03.4476872|03.4005168|03.4962855|03.4492357|
|2|06.0066571|04.6297992|03.9375367|03.9628754|03.9636546|03.4815188|
|3|04.1079661|04.4479405|03.4918979|03.4874563|03.4739197|03.4373477|
|4|03.8241052|03.9440888|03.5481203|03.4962378|03.5970963|03.4345607|
|5|03.9227889|03.8672537|03.5270855|03.5670758|03.4530075|03.4396712|
|6|03.9365109|03.9009880|03.4277436|03.5154730|03.4948439|03.4314255|
|7|03.8137786|03.9517794|03.6711828|03.4599004|03.5232537|03.4174293|
|8|03.9162212|03.8381552|03.5453183|03.5335635|03.4950666|03.4791826|
|9|03.8746262|03.9102678|03.5650245|03.4378088|03.4469888|03.4470586|
|10|03.9304067|03.9217274|03.4046821|03.5152450|03.4661721|03.5199039|
|**AVG**|**04.3135653**|**04.0337784**|**03.5566278**|**03.5376152**|**03.5410288|**03.4537334**|


### Round 2

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|Norm TUPLES|Raw DataReader|
|-|-----------|-------------|---------|-----------|-----------|--------------|
|1|05.1507459|04.0248490|03.4553928|03.4898228|03.4454819|03.6652173|
|2|03.8679590|04.0432841|03.3844195|03.4338807|03.3805373|03.4049319|
|3|03.9244829|04.0776852|03.5146225|03.5266683|03.9178342|03.5860509|
|4|04.0532428|04.0481091|03.4690038|03.5427280|03.4087373|03.4170472|
|5|04.0205996|04.0320517|03.7435097|03.6744232|03.5223342|03.6735942|
|6|03.9704174|04.0875421|03.6742348|03.6363717|03.4841327|03.4487219|
|7|03.9078427|04.0042793|03.7572632|03.8503936|03.4338911|03.5015400|
|8|03.9751321|03.9914152|03.6897327|03.6256718|03.6632366|03.7008167|
|9|03.9691990|03.9920399|03.6869331|03.7483528|03.5136311|03.5418013|
|10|03.8893251|04.1737140|03.7143296|03.6263200|03.6535762|03.5511610|
|**AVG**|**04.0728946**|**04.0474969**|**03.6089441**|**03.6154632**|**03.5423392**|**03.5490882**|

### Round 3

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|Norm TUPLES|Raw DataReader|
|-|-----------|-------------|---------|-----------|-----------|--------------|
|1|04.3101856|03.9383326|03.4970278|03.2741134|03.3867993|03.3934763|
|2|03.8855439|03.9815244|03.3784879|03.4931214|03.4527211|03.3811276|
|3|03.9293713|04.0181483|03.4456998|03.4370836|03.3915271|03.5341020|
|4|04.0463935|04.0129422|03.4920009|03.4510810|03.5107734|03.4544545|
|5|03.9404838|04.0350873|03.4238162|03.4790497|03.6978646|03.6568042|
|6|04.0305185|04.0876946|03.5131145|03.4918386|03.5281288|03.5323353|
|7|04.0004984|04.0684690|03.5778807|03.4949941|03.5268483|03.5704475|
|8|04.0807761|04.0637439|03.4821100|03.4778834|03.4859002|03.5175608|
|9|03.9361910|04.0815588|03.7447562|03.4814912|03.5394205|03.4657830|
|10|03.9584394|04.1086788|03.5203725|03.5346357|03.4802855|03.5114095|
|**AVG**|**04.0118401**|**04.0396179**|**03.5075266**|**03.4615292**|**03.5000268**|**03.5017500**|


Serialization to named tuples enumerable doesn't use any mapping logic (no reflection and no caching) and
it is fast as raw data reader.

Results indicate that mapping to POCO or Record are indistinguishable from raw data reader. They seem to be just as fast.

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
