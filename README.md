# `NoOrm.Net`

Fast and extendible **`C# 8`** alternative to Dapper data access built for **.NET Core 3** era.

>**THIS IS NOT ORM**

And hence, the name - **`NoOrm`**.

## How It Works - Why **NoOrm** - And - Similarities With Dapper

Both libraries are implementation of set of various extensions over **`System.Data.Common.DbConnection`** system object to work efficiently with different databases.

They work quite differently. For example, query that returns **one million records** from database:

- Dapper

```csharp
var results = connection.Query<TestClass>(query);
```

Resulting in `IEnumerable<TestClass>` in **`00:00:02.0330651`** seconds.

- NoOrm

```csharp
var results = connection.Read(query).Select(t => new TestClass(t));
```

Resulting in `IEnumerable<TestClass>` in **`00:00:00.0003062`** seconds.

That is a fraction of time and reason is simple:

- *Dapper* triggers iteration and objects serialization immediately when called.

- *NoOrm* builds internal iterator over database results.

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

With NoOrm approach it would look something like this:

1. Build iterator over tuples that will be returned from database

2. Build expression tree over enumerable iterator (`System.Linq`, `System.Linq.Async`, custom, etc)

3. Execute everything to create results (view-models and service responses)

This allows to keep current design with separation of concerns and to have only one, single iteration.

Also, when working asynchronously in first scenario - typically we have to wait until all results are retrieved from database and then start building the response or view asynchronously.

NoOrm utilizes new `IAsyncEnumerable` interface which doesn't need to wait until all records are fetched and retrieved from database to start building the results.

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

### Working with parameters

#### Positional parameters

```csharp
connection.Execute("select @p1, p2", value1, value2);
```

#### Named parameters

```csharp
connection.Execute("select @p1, p2", ("p1", value1), ("p2", value2));
```

## Working with results and Object/Relational mapping

Results are always tuples by default.

- Non generic version will return enumerable iterator of tuples with `string name` and `object value`.

- Generic version will return tuples of type indicated by generic parameters, Up to 5 generic parameters are currently supported, there will be more in future.

> There is no automatic O/R mapping out-of-the-box, as name suggest, this is not ORM.

- Tuple enumerations (generic and non-generic) can be easily extended (with c# extension) to transform to any structure required or selected for something else **without triggering iteration.** There are already basic extensions to transform to dictionaries and lists [here](https://github.com/vbilopav/NoOrm.Net/blob/master/NoOrm/Extensions/NoOrmExtensions.cs). New ones are easily added.

- Generic tuples are not complicated to map to objects. For example following `PostgreSQL` query:

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

This requires two steps:

1. Add following constructor to this class:

```csharp
public TestClass((int id, string foo, string bar, DateTime dateTime) tuple)
{
    Id = tuple.id;
    Foo = tuple.foo;
    Bar = tuple.bar;
    Datetime = tuple.dateTime;
}
```

2. Serialize with following expression:

```csharp
var results = connection.Read<int, string, string, DateTime>(TestQuery).Select(tuple => new TestClass(tuple));
```

This will not trigger iteration and serialization until we start to actually iterate with `foreach` or `ToList`, thus allows us to continue building expression tree as we see fit.

Initial tests are showing that this approach will yield mapping and serialization around 15-20% faster then Dapper, and even without even using async streaming feature.

However, for some scenarios, this might be too much typing. For simple solutions it might be better to use JSON:

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

And after that it is enough to say:

```csharp
var results = connection.Json<TestClass>(TestQuery);
```

That is it. Note that this, again, will not yield iteration and mapping (unlike Dapper), until it is required.

Real O/R mapping extension might be possible in future.


## Licence

Copyright (c) Vedran Bilopavlović - VB Software 2019
This source code is licensed under the [MIT license](https://github.com/vbilopav/NoOrm.Net/blob/master/LICENSE).

