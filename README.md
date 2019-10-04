# `Norm.Net`

Fast, modern and extendible **`C# 8`** data access built for **.NET Core 3** era.

> **THIS IS NOT ORM** `Norm` is `NoORM`, or not an `ORM` (but there is O/R mapping extension).

`Norm` will postpone any reads from database until they are needed - allowing you to build expression trees and transformations - before it started fetching any data.

This allows avoiding unneccessary iterations and as well greater flexibility.

By default - it will return iterator over tuples and not serialized instances. Because that's what databases do return - tuples.

Those iterator over tuples can be then further extended with expressions (such as dictioanires or O/R mappings, see [O/R mapping](https://github.com/vbilopav/NoOrm.Net#working-with-results-and-objectrelational-mapping) section bellow). 

See [How it works](https://github.com/vbilopav/NoOrm.Net#how-it-works---why-norm---and---similarities-with-dapper) section bellow.

## Changes in version 1.1.0

- All result types `IEnumerable<(string name, object value)>` replaced with`IList<(string name, object value)>`.
- Consequently name/value tuple results are generating lists structure and do not deffer serialization.
- This allowed simplification of extensions and to remoev some of them.
- Added proper extension for O/R Mapping  by using `FastMember` library

## How It Works - Why **Norm** - And - Similarities With Dapper

Both libraries are implementation of set of various extensions over **`System.Data.Common.DbConnection`** system object to work efficiently with different databases.

They work quite differently. For example, query that returns **one million records** from database:

- Dapper:

```csharp
var results = connection.Query<TestClass>(query);
```

Resulting in `IEnumerable<TestClass>` in **`00:00:02.0330651`** seconds.

- Norm:

```csharp
var results = connection.Read(query).Select(t => new TestClass(t));
```

Resulting in `IEnumerable<TestClass>` in **`00:00:00.0003062`** seconds.

That is a fraction of time and reason is simple:

- *Dapper* triggers iteration and objects serialization immediately when called.

- *Norm* builds internal iterator over database results.

- This allows delaying any results iteration (and potential serialization) - so that expression tree can be built (using `System.Linq` and/or `System.Linq.Async` libraries or custom `IEnumerable` extensions) for our view models or service responses - **before any actual results iteration.**

This approach can save unnecessary database result iterations to improve system performances.

If we execute `ToList()` extension on results above - we will see similar execution times but this time - vice versa. Meaning this time results from Dapper will execute in fraction of time and results from NoOrm will execute approximately as Dapper the first time.

That is because `ToList()` triggers iteration automatically - if `List` structure hasn't been built yet. And since Dapper builds `List` internally each call by default, it will not be executed again. Contrary NoRom haven't run any iterations yet, and it will have to do it for the first time.

So, why it matters then?

This is typical application scenario with Dapper:

1. Runs iteration over database results to build internal list.

2. Application defines expressions and transformations that transforms the data in required output (such as typically view-models and service responses).

3. Application iterates again over transformed data to build required view or to serialize to response.

So this scenario requires at least two iteration over data.

With `Norm` approach it would look something like this:

1. Build iterator over tuples that will be returned from database

2. Build expression tree over enumerable iterator - sync or async - (`System.Linq`, `System.Linq.Async`, custom, etc)

3. Execute everything to create results (view-models and service responses)

This allows to keep current design with separation of concerns and to have only one, single iteration.

Also, when working asynchronously in first scenario - typically we have to wait until all results are retrieved from database and then start building the response or view asynchronously.

Norm utilizes new `IAsyncEnumerable` interface which doesn't need to wait until all records are fetched and retrieved from database to start building the results.

Instead, result item is processed as it appears, effectively doing the asynchronous streaming directly from database.

## Test coverage and usage examples

Usage examples can be found in [unit tests](https://github.com/vbilopav/NoOrm.Net/tree/master/Tests/)

Around 99% of API is covered with tests only for [`PostgreSQL`](https://github.com/vbilopav/NoOrm.Net/tree/master/Tests/PostgreSqlUnitTest) and [`Microsoft SQL Server`](https://github.com/vbilopav/NoOrm.Net/tree/master/Tests/SqlServerUnitTests).

Other types of databases should work theoretically but they are not currently tested.

## API

Entire API that is implemented as **`System.Data.Common.DbConnection`** extension can be found [on this interface](https://github.com/vbilopav/NoOrm.Net/blob/master/NoOrm/INoOrm.cs)

Recap:

| API Group | Description |
| ------------- |-------------|
| `Execute`, `ExecuteAsync` | Execute command on database without returning results. These command can be chained in multiple executions. See some [examples here.](https://github.com/vbilopav/NoOrm.Net/blob/master/Tests/PostgreSqlUnitTests/ExecuteUnitTests.cs#L25) |
| `Single`, `SingleAsync` | Execute command and return single tuple from database. See some [examples here.](https://github.com/vbilopav/NoOrm.Net/blob/master/Tests/PostgreSqlUnitTests/SingleTuplesUnitTests.cs) |
| `Read`, `ReadAsync`  | Execute command and builds iterator over tuples. See some [examples here.](https://github.com/vbilopav/NoOrm.Net/blob/master/Tests/PostgreSqlUnitTests/ReadTuplesUnitTests.cs) |
| `SingleJson`, `SingleJsonAsync` |  single database JSON result (query that returns single value from database - JSON blob) -  into an instance of the type specified by a generic type parameter. See some [examples here.](https://github.com/vbilopav/NoOrm.Net/blob/master/Tests/PostgreSqlUnitTests/SingleJsonUnitTests.cs) |
| `Json`, `JsonAsync` |  database JSON results (single row of JSON objects) - into an enumerator (or async enumerator) of instance of the type specified by a generic type parameter. See some [examples here.](https://github.com/vbilopav/NoOrm.Net/blob/master/Tests/PostgreSqlUnitTests/JsonUnitTests.cs)|
| `As`, `AsProcedure`, `AsText`, `Timeout`, `WithJsonOptions`, `WithOutParameter`, `GetOutParameterValue` | Provide general functionality like changing command type from procedure to test, setting the timeout, and output parameters...|
| Extensions | Set of `IEnumerable` and `IAsyncEnumerable` extensions to convert database tuples to lists and dictionaries. New extensions can be added on will (for object mapping for example). |

### Available extensions

- By convention any extension that Start with `Select` will build up expression tree and not trigger any iteration. Available extensions are:

#### SelectDictionary

Add expression to build a dictionary from name, value tuples

#### SelectDictionaries and SelectDictionariesAsync

Add expression to build a enumerator (sync or async) - of dictionaries from collection of name, value tuples

#### SelectValues

Select only valeus fron name value tuples

#### Select`T` and SelectAsync`T`

Select results mapped to a class instance (O/R mapping, case sensitive).

#### Named parameters

```csharp
connection.Execute("select @p1, p2", ("p1", value1), ("p2", value2));
```

## Working with results and Object/Relational mapping

Results are always tuples by default.

- Non generic version will return enumerable iterator of tuples with `string name` and `object value`.

- Generic version will return tuples of type indicated by generic parameters, up to 5 generic parameters are currently supported, there will be more in future.

> There is no automatic O/R mapping out-of-the-box, as name suggest, this is not ORM.

There are couple ways to achive this:

1. Simplest way: use default extension that maps (selects) to generic parameter object instance. For example:

```csharp
connection.Read(sql).Select<TestClass>();
```

2.  Map generic tuples to objects. This is fastest and most flexible method. For example following `PostgreSQL` query:

```sql
select 
    i as id, 
    'foo' || i::text as foo, 
    'bar' || i::text as bar, 
    ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
from generate_series(1, 1000000) as i -- return a million
```

We want to map to instances of following corresponding class:

```csharp
class TestClass
{
    public int Id { get; set; }
    public string Foo { get; set; }
    public string Bar { get; set; }
    public DateTime Datetime { get; set; }
}
```


First add constructor that have tuple parameter and map data manually:

```csharp
public TestClass((int id, string foo, string bar, DateTime dateTime) tuple)
{
    Id = tuple.id;
    Foo = tuple.foo;
    Bar = tuple.bar;
    Datetime = tuple.dateTime;
}
```

Second, Serialize with following expression:

```csharp
var results = connection.Read<int, string, string, DateTime>(TestQuery).Select(tuple => new TestClass(tuple));
```

3. Map JSON results directly. This requires query to return either JSON blob or single row with JSON object

`PostgreSQL` should look like this:

```sql
select to_json(t) -- return json rows:
from (
    select 
        i as id, 
        'foo' || i::text as foo, 
        'bar' || i::text as bar, 
        ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
    from generate_series(1, 1000000) as i
) t -- return a million
```

Code:

```csharp
var results = connection.Json<TestClass>(TestQuery);
```

## Performance tests

Following table shows some performance metrics. 

All tests are executed over one million tuples returned from database and all values are in seconds.

| | dapper read (1) | norm read (2) | norm read (3)  | norm read (4) | norm read (5) | norm read (6) | norm read (7) |
| - | --------- | --------  | --------  | --------  | --------  | --------  | --------  |
| 1 | 3,0415078 | 0,0024166 | 0,0007101 | 0,0005985 | 0,0031619 | 0,0007754 | 0,0012049 |
| 2 | 3,245256 | 0,0032193 | 0,0006805 | 0,000583 | 0,002197 | 0,000882 | 0,0010584 |
| 3 | 2,9803222 | 0,0026597 | 0,0006636 | 0,0005223 | 0,0021911 | 0,0007316 | 0,0008133 |
| 4 | 4,2485572 | 0,0026039 | 0,0007918 | 0,0007257 | 0,0035843 | 0,0008177 | 0,0008636 |
| 5 | 3,4473896 | 0,0024689 | 0,0009009 | 0,0005666 | 0,0034545 | 0,0007081 | 0,0008028 |
| 6 | 3,9070679 | 0,002534 | 0,0007605 | 0,0008123 | 0,0023278 | 0,0007847 | 0,0009933 |
| 7 | 3,3050129 | 0,0025335 | 0,0008778 | 0,0005671 | 0,0034135 | 0,0008028 | 0,0008808 |
| 8 | 3,058142 | 0,0022742 | 0,0006804 | 0,0005627 | 0,0022258 | 0,001048 | 0,0008435 |
| 9 | 3,0750567 | 0,0028665 | 0,0009022 | 0,0008439 | 0,0044506 | 0,0008151 | 0,0009868 |
| AVG | **3,367590256** | **0,002619622** | **0,0007742** | **0,000642456** | **0,003000722** | **0,000818378** | **0,0009386** |

| | dapper count (1)  | norm count (2) | norm count (3)  | norm count (4) | norm count (5) | norm count (6) | norm count (7) |
| - | --------- | --------  | --------  | --------  | --------  | --------  | --------  |
| 1 | 0,0026828 | 2,8800147 | 3,9677773 | 4,044749 | 3,5522813 | 4,29726 | 2,9316158 |
| 2 | 0,0017753 | 2,5111109 | 3,5520368 | 3,0283845 | 2,5383521 | 4,4486203 | 3,4632417 |
| 3 | 0,0019028 | 2,4975842 | 3,6029344 | 2,9978021 | 2,6323984 | 4,1697686 | 3,108879 |
| 4 | 0,0014821 | 2,7933007 | 3,7870573 | 3,8797356 | 3,126549 | 5,0243688 | 3,8503477 |
| 5 | 0,0018032 | 2,609527 | 4,2870604 | 3,2902466 | 2,4776056 | 4,9150692 | 3,296226 |
| 6 | 0,0018624 | 3,1660909 | 3,7884559 | 3,3306854 | 3,1666028 | 5,5989298 | 3,2163334 |
| 7 | 0,0019943 | 3,0209209 | 5,7025867 | 4,995924 | 2,4584816 | 4,424112 | 2,6890962 |
| 8 | 0,001791 | 2,3538676 | 4,1760797 | 3,173455 | 2,4060259 | 4,0451822 | 3,1164555 |
| 9 | 0,0032063 | 3,3263054 | 4,8390286 | 3,2985729 | 2,9437614 | 4,7721182 | 3,1037891 |
| AVG | **0,002055578** | **2,795413589** | **4,189224122** | **3,559950567** | **2,811339789** | **4,632825456** | **3,1973316** |

### 1. Dapper query - read and serializes one million rows from SQL query. Averages in **3,367590256** seconds

```csharp
IEnumerable<TestClass> results1 = connection.Query<TestClass>(sql);
```

### 2. Norm read operation - builds iterator over list of name and value tuples. Averages in **0,002619622** seconds

```csharp
IEnumerable<IList<(string name, string value)>> results2 = connection.Read(sql);
```

### 3. Norm read operation, builds iterator over name/value dictionaries. Averages in **0,0007742** seconds

```csharp
IEnumerable<IDictionary<string, object>> results3 = connection.Read(sql).SelectDictionaries();
```

### 4. Norm read operation - builds iterator over name/value dictionaries and use it to build iterator over `TestClass` instances. Averages in **0,0007742** seconds

```csharp
IEnumerable<TestClass> results4 = connection.Read(sql).SelectDictionaries().Select(dict => new TestClass(dict));
```

This approach requires class constructor that receives dictionary as parameter:

```csharp
public TestClass(IDictionary<string, object> dictionary)
{
    Id = (int) dictionary["Id"];
    Foo = (string) dictionary["Foo"];
    Bar = (string) dictionary["Bar"];
    Datetime = (DateTime) dictionary["Datetime"];
}
```

### 5. Norm read operation - builds iterator over generic, typed tuples and use nd use it to build iterator over `TestClass` instances. Averages in **0,003000722**

```csharp
IEnumerable<TestClass> results5 = connection.Read<int, string, string, DateTime>(sql).Select(tuple => new TestClass(tuple));
```

This approach requires class constructor that receives row tuple as parameter:

```csharp
public TestClass((int id, string foo, string bar, DateTime dateTime) tuple)
{
    Id = tuple.id;
    Foo = tuple.foo;
    Bar = tuple.bar;
    Datetime = tuple.dateTime;
}
```

### 6. Norm read operation - 

```csharp
IEnumerable<TestClass> results6 = connection.Json<TestClass>(JsonTestQuery))
```

### 7. Norm read operation - 

```csharp
IEnumerable<TestClass> results7 = connection.Read(TestQuery).Select<TestClass>());
```

(8) Dapper

```csharp
```

(9) Dapper
(10) Dapper
(11) Dapper
(12) Dapper
(13) Dapper



## Licence

Copyright (c) Vedran Bilopavlović - VB Software 2019
This source code is licensed under the [MIT license](https://github.com/vbilopav/NoOrm.Net/blob/master/LICENSE).

