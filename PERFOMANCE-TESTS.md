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
    'long_foo_bar_' || (i+2)::text as longfoobar, 
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
|1|05.7436215|04.3740157|03.8784907|03.8203426|
|2|04.1491573|04.2200430|03.8530082|03.8314177|
|3|04.5683285|04.2840642|03.7475639|03.7947453|
|4|04.0689478|04.2871359|03.9170383|03.6602834|
|5|04.0194517|04.1079186|03.6375660|03.6319482|
|6|03.9900124|04.1452463|03.5890262|03.7750911|
|7|04.0965557|04.1881737|03.7617025|03.6733226|
|8|03.9991026|04.0590633|03.9744150|04.2425081|
|9|04.1220426|04.1309612|03.8321324|03.6486829|
|10|04.3221879|04.1217288|03.6021495|03.7312605|
|**AVG**|**04.3079408**|**04.1918350**|**03.7793092**|**03.7809602**|

### Round 2

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|
|-|-----------|-------------|---------|-----------|
|1|04.6444758|04.3038165|03.7633814|03.6224407|
|2|03.9652000|04.1286032|03.7065907|03.6975498|
|3|04.0111194|04.1018603|03.5940484|03.5874507|
|4|04.0000450|04.1563585|03.6060406|03.5628298|
|5|04.0312880|04.1674726|03.9520174|03.6972884|
|6|03.9835398|04.3296336|03.7013902|03.5588114|
|7|04.0988099|04.1301041|03.5901883|03.6324565|
|8|03.9888021|04.0988069|03.6581588|03.5804932|
|9|04.0251520|04.1003781|03.8006083|03.7657931|
|10|04.0677861|04.1675663|03.6541157|03.7996297|
|**AVG**|**04.0816218**|**04.1684600**|**03.7026539**|**03.6504743**|

### Round 3

|#|Dapper POCO|Dapper RECORD|Norm POCO|Norm RECORD|
|-|-----------|-------------|---------|-----------|
|1|04.3546780|04.0439048|03.9072074|04.0780570|
|2|04.0149766|04.0741690|03.6257235|03.7035539|
|3|04.0167809|04.0476142|03.7396306|03.5619949|
|4|03.9384857|04.0788737|03.6381754|03.5958990|
|5|03.9463583|04.0952644|03.7020939|03.9324867|
|6|04.1309226|04.0667880|03.6341982|03.8705895|
|7|04.3993218|04.0977595|03.6963693|03.6102759|
|8|03.9928200|04.0933621|03.6415327|03.6449551|
|9|03.9847281|04.1026113|03.6572780|03.8957852|
|10|04.2878746|04.0931707|03.5820210|03.6571660|
|**AVG**|**04.1066946**|**04.0793517**|**03.6824230**|**03.7550763**|

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

|#|Dapper RECORD to Dict|Norm TUPLES to Dict|Norm RECORD to Dict|
|-|---------------------|-------------------|-------------------|
|1|04.9611222|03.5793548|03.7829188|
|2|04.2776633|03.6313018|03.8526667|
|3|04.2826017|03.5731747|03.5728392|
|4|04.0493683|03.6734413|03.6763409|
|5|03.9983924|03.6098277|03.5688257|
|6|04.0381132|03.5507968|03.6146418|
|7|04.0114675|03.5194242|03.5572489|
|8|04.0532484|03.7172804|03.7005553|
|9|03.9979233|03.5440084|03.5902588|
|10|04.0863978|03.5932316|03.5553588|
|**AVG**|**04.1756298**|**03.5991841**|**03.6471654**|

### Round 2

|#|Dapper RECORD to Dict|Norm TUPLES to Dict|Norm RECORD to Dict|
|-|---------------------|-------------------|-------------------|
|1|04.2674022|03.4135225|03.4531498|
|2|04.2665891|03.8019945|03.7883671|
|3|04.1407496|03.5674730|03.5370632|
|4|04.0190651|03.5549003|03.6065343|
|5|04.0641311|03.6277302|03.5746980|
|6|03.9979360|03.5638271|03.4893770|
|7|04.0896766|03.5672561|03.6144918|
|8|04.5820484|03.7027318|03.5736443|
|9|04.1093835|03.5552922|03.4871451|
|10|04.0639286|03.5927925|03.5993064|
|**AVG**|**04.1600910**|**03.5947520**|**03.5723777**|

### Round 3

|#|Dapper RECORD to Dict|Norm TUPLES to Dict|Norm RECORD to Dict|
|-|---------------------|-------------------|-------------------|
|1|04.3944028|03.5087631|03.5379839|
|2|04.3562263|03.8040958|03.6170409|
|3|04.2045850|03.6738324|03.6044319|
|4|04.3177220|03.7091615|03.6331545|
|5|04.1090268|03.5574325|03.5779432|
|6|04.0899715|03.5925017|03.6373472|
|7|04.0991569|03.5213084|03.7296755|
|8|04.2949127|03.5889943|03.5440725|
|9|04.0705439|03.5643371|03.5391212|
|10|04.1206734|03.5872851|03.5377796|
|**AVG**|**04.2057221**|**03.6107711**|**03.5958550**|
