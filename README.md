# `Norm.Net`

Modern and extendible **`C# 8`** data access built for **.NET Core 3** era.

> Warning
>**THIS IS NOT ORM** `Norm` is `NoORM`, or not an `ORM` (although, there is O/R mapping extension).

`Norm` will postpone any reads from database until they are needed - allowing you to build expression trees and transformations - before it started fetching any data.

This allows avoiding unneccessary iterations and much greater flexibility.

By default it will return iterator over tuples and not serialized instances.

Because that's what databases do returns - **tuples.**

This allows for more extendibility - iterator results can be then further extended or mapped and transformed to something else (such as dictioanires or O/R mappings, see [O/R mapping](https://github.com/vbilopav/NoOrm.Net#working-with-results-and-objectrelational-mapping).

## [Change log](https://github.com/vbilopav/NoOrm.Net/blob/master/CHANGES.md)

## [How It Works - Why **Norm** - And - Similarities With Dapper](https://github.com/vbilopav/NoOrm.Net/blob/master/HOW-IT-WORKS.md)

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

### Working with database parameters

Each database operation can receive params array (a variable number of arguments) that will be mapped top appropriate `DbParameter` type instance to avoid SQL injection.

There are two overloads that receive parameters:

#### Positional parameters

Map parameter by position (name is not important, but it must start with `@` by convention)

```csharp
connection.Execute("select @p1, @p2", value1, value2);
connection.Execute("select @p1, @p2, @third", value1, value2, value3);
// etc...
```

#### Named parameters parameters

Map parameter by exact name, position is not important:

```csharp
connection.Execute("select @p1, @p2", ("p1", value1), ("p2", value2));
connection.Execute("select @p1, @p2, @third", ("p1", value1), ("p2", value2), ("third", value3));
// etc...
```

### Available extensions

By convention any extension that Start with `Select` will build up expression tree and not trigger any iteration. Available extensions are:

| Extension | Extends | Description |
| ------------- |-------------|-------------|
| `SelectDictionary` | name and value tuple pairs | adds expression to build a dictionary from name, value tuples |
| `SelectDictionaries`, `SelectDictionariesAsync` | enumerator over name and value tuple pairs | add expression to build a enumerator (sync or async) - of dictionaries from collection of name, value tuples |
| `SelectValues` |  name and value tuple pairs or enumerator over name and value tuple pairs | Select only values from name value tuples |
| `Select<T>`, `SelectAsync<T>` | name and value tuple pairs or enumerator over name and value tuple pairs | Map to an instance of provided generic type using `FastMember` (O/R mapping)

## Working with results and Object/Relational (O/R) mapping

Results are always tuples by default.

> There is no automatic O/R mapping out-of-the-box, - as name suggest - **this is not ORM**

- Non generic version will return enumerable iterator of tuples with `string name` and `object value`.

- Generic version return tuples of the type indicated by generic type parameters. For example:

```csharp
IEnumerable<(int, string, string, DateTime)> results = connection.Read<int, string, string, DateTime>(sql);
// use result to Select or map to required structures for your program.
```

Common usage scenario would be to use `Select` or `SelectMany` - `Linq` extensions to map those tuples to, for example:

- Dictionary where dictionary key is returned from database
- Multiple class instances by using `SelectMany` - `Linq` extensions
- etc

And later in program actual iteration will be executed when we call `ToList` or `foreach`.

However, if we would wanted to map to class instances - there are **couple of ways to do this.**

For example, let's take test query:

```sql
-- PostgreSQL syntax
select
    i as id,
    'foo' || i::text as foo,
    'bar' || i::text as bar,
    ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
from generate_series(1, 1000000) as i -- return a million
```

and test class which instances we wish serialize from this query:

```csharp
class TestClass
{
    public int Id { get; set; }
    public string Foo { get; set; }
    public string Bar { get; set; }
    public DateTime Datetime { get; set; }
}
```

### 1. Use `Select<T>` extension

Example:

```csharp
IEnumerable<TestClass> results = connection.Read(sql).Select<TestClass>();
```

Note:
This is implemented as extensions to value/name tuples result. Anyone can implemented custom extension for O/R mapping or anything else that is required. If you want greater speed for this functionality new extension that uses external library (such as extra fast `FastMember`) can be easily implemented.

This will not trigger iteration nor serialization until we call `ToList` or `foreach`.

### 2. Use `Json` methods to read the data

Query can return JSON values. Either entire JSON blob (use `SingleJson` method for this) or single row containing Json objects (use `Single` method for this)

We need to modify query to return JSON:

```sql
-- PostgreSQL syntax
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

```csharp
IEnumerable<TestClass> results = connection.Json(sql);
```

Again, note that this, again will not trigger iteration nor serialization until we call `ToList` or `foreach`.

### 3. Add mapping constructors to your class

This the method I would personally recommend - because flexibility and speed. Although it might require little bit of typing. We must add specialized constructor to our class first.

If we would map from dictionary, then we need this constructor:

```csharp
public TestClass(IDictionary<string, object> dictionary)
{
    Id = (int) dictionary["Id"];
    Foo = (string) dictionary["Foo"];
    Bar = (string) dictionary["Bar"];
    Datetime = (DateTime) dictionary["Datetime"];
}
```

Or, if we map from tuples we need this dictionary:

```csharp
public TestClass((int id, string foo, string bar, DateTime dateTime) tuple)
{
    Id = tuple.id;
    Foo = tuple.foo;
    Bar = tuple.bar;
    Datetime = tuple.dateTime;
}
```

And now, to serialize class instances - we can use these following expressions:

```csharp
IEnumerable<TestClass> results1 = connection.Read(sql).SelectDictionaries().Select(dict => new TestClass(dict));

IEnumerable<TestClass> results2 = connection.Read<int, string, string, DateTime>(TestQuery).Select(tuple => new TestClass(tuple));
```

## Performances

PostgreSQL SQL test query that returns a million tuples from server:

```sql
select
    i as id,
    'foo' || i::text as foo,
    'bar' || i::text as bar,
    ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
from generate_series(1, 1000000) as i
```

Following table shows execution times of Dapper read operation and different Norm read operations that yield enumerable results:

| | dapper read (1) | norm read (2) | norm read (3)  | norm read (4) | norm read (5) | norm read (6) | norm read (7) |
| - | --------- | --------  | --------  | --------  | --------  | --------  | --------  |
| 1 | 0:00:04.100 | 0:00:00.006 | 0:00:00.001 | 0:00:00.001 | 0:00:00.004 | 0:00:00.001 | 0:00:00.001 |
| 2 | 0:00:03.187 | 0:00:00.003 | 0:00:00.001 | 0:00:00.001 | 0:00:00.003 | 0:00:00.001 | 0:00:00.001 |
| 3 | 0:00:03.051 | 0:00:00.002 | 0:00:00.001 | 0:00:00.001 | 0:00:00.002 | 0:00:00.001 | 0:00:00.001 |
| 4 | 0:00:03.395 | 0:00:00.002 | 0:00:00.001 | 0:00:00.001 | 0:00:00.002 | 0:00:00.001 | 0:00:00.001 |
| 5 | 0:00:04.039 | 0:00:00.003 | 0:00:00.001 | 0:00:00.001 | 0:00:00.002 | 0:00:00.001 | 0:00:00.001 |
| 6 | 0:00:03.097 | 0:00:00.003 | 0:00:00.001 | 0:00:00.001 | 0:00:00.002 | 0:00:00.001 | 0:00:00.001 |
| 7 | 0:00:04.561 | 0:00:00.003 | 0:00:00.001 | 0:00:00.005 | 0:00:00.004 | 0:00:00.001 | 0:00:00.002 |
| 8 | 0:00:03.610 | 0:00:00.002 | 0:00:00.001 | 0:00:00.001 | 0:00:00.005 | 0:00:00.001 | 0:00:00.001 |
| 9 | 0:00:03.217 | 0:00:00.002 | 0:00:00.001 | 0:00:00.001 | 0:00:00.003 | 0:00:00.001 | 0:00:00.001 |
| 10 | 0:00:02.916 | 0:00:00.003 | 0:00:00.001 | 0:00:00.001 | 0:00:00.003 | 0:00:00.001 | 0:00:00.001 |
| AVG | **0:00:03.517** | **0:00:00.003** | **0:00:00.001** | **0:00:00.001** | **0:00:00.003** | **0:00:00.001** | **0:00:00.001** |

Following table shows execution times of count operations over enumeration results from same operations.

| | dapper count (1)  | norm count (2) | norm count (3)  | norm count (4) | norm count (5) | norm count (6) | norm count (7) |
| - | --------- | --------  | --------  | --------  | --------  | --------  | --------  |
| 1 | 0:00:00.002 | 0:00:03.679 | 0:00:03.989 | 0:00:03.813 | 0:00:02.639 | 0:00:04.987 | 0:00:04.008 |
| 2 | 0:00:00.002 | 0:00:02.771 | 0:00:04.264 | 0:00:03.168 | 0:00:02.571 | 0:00:04.117 | 0:00:03.463 |
| 3 | 0:00:00.002 | 0:00:03.198 | 0:00:04.940 | 0:00:04.129 | 0:00:03.277 | 0:00:05.513 | 0:00:03.346 |
| 4 | 0:00:00.002 | 0:00:02.429 | 0:00:03.647 | 0:00:03.128 | 0:00:02.769 | 0:00:05.267 | 0:00:04.993 |
| 5 | 0:00:00.002 | 0:00:02.761 | 0:00:03.950 | 0:00:03.471 | 0:00:02.641 | 0:00:05.018 | 0:00:04.270 |
| 6 | 0:00:00.002 | 0:00:03.043 | 0:00:03.698 | 0:00:02.920 | 0:00:02.195 | 0:00:04.470 | 0:00:03.975 |
| 7 | 0:00:00.003 | 0:00:03.870 | 0:00:05.282 | 0:00:03.586 | 0:00:02.852 | 0:00:05.348 | 0:00:04.066 |
| 8 | 0:00:00.002 | 0:00:02.549 | 0:00:03.635 | 0:00:03.659 | 0:00:02.860 | 0:00:04.495 | 0:00:03.824 |
| 9 | 0:00:00.003 | 0:00:02.389 | 0:00:03.507 | 0:00:02.974 | 0:00:02.269 | 0:00:04.917 | 0:00:03.723 |
| 10 | 0:00:00.002 | 0:00:02.654 | 0:00:03.679 | 0:00:02.778 | 0:00:02.926 | 0:00:04.203 | 0:00:03.563 |
| AVG | **0:00:00.002** | **0:00:02.934** | **0:00:04.059** | **0:00:03.363** | **0:00:02.700** | **0:00:04.834** | **0:00:03.923** |

### 1. Dapper query - read and serializes one million rows from SQL query

```csharp
// Average execution time: 0:00:03.517
IEnumerable<TestClass> results1 = connection.Query<TestClass>(sql);

// Average execution time: 00:00:00.002
results1.count();
```

### 2. Norm read operation - builds iterator over list of name and value tuples

```csharp
// Average execution time: 0:00:00.003
IEnumerable<IList<(string name, string value)>> results2 = connection.Read(sql);

// Average execution time: 0:00:02.934
results2.count();
```

### 3. Norm read operation, builds iterator over name/value dictionaries

```csharp
// Average execution time: 0:00:00.001
IEnumerable<IDictionary<string, object>> results3 = connection.Read(sql).SelectDictionaries();

// Average execution time: 0:00:04.059
results3.count();
```

### 4. Norm read operation - builds iterator over name/value dictionaries and use it to build iterator over `TestClass` instances

```csharp
// Average execution time: 0:00:00.001
IEnumerable<TestClass> results4 = connection.Read(sql).SelectDictionaries().Select(dict => new TestClass(dict));

// Average execution time: 0:00:03.363
results4.count();
```

### 5. Norm read operation - builds iterator over generic, typed tuples and use use it to build iterator over `TestClass` instances

```csharp
// Average execution time: 0:00:00.003
IEnumerable<TestClass> results5 = connection.Read<int, string, string, DateTime>(sql).Select(tuple => new TestClass(tuple));

// Average execution time: 0:00:02.700
results5.count();
```

### 6. Norm read operation - builds iterator over de-serialized JSON to class instances

```csharp
// Average execution time: 0:00:00.001
IEnumerable<TestClass> results6 = connection.Json<TestClass>(JsonTestQuery))

// Average execution time: 0:00:04.834
results5.count();
```

### 7. Norm read operation - builds iterator over class instances mapped with `Select<T>` O/R mapping extension

```csharp
// Average execution time: 0:00:00.001
IEnumerable<TestClass> results7 = connection.Read(TestQuery).Select<TestClass>());

// Average execution time: 0:00:03.923
results7.count();
```

## Get it on Nuget

```txt
> dotnet add package Norm.net
```

## Currently supported platforms

- .NET Standard 2.1
- .NET Core 3.0

## Licence

Copyright (c) Vedran Bilopavlović - VB Software 2019
This source code is licensed under the [MIT license](https://github.com/vbilopav/NoOrm.Net/blob/master/LICENSE).
