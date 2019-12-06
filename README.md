# `Norm.Net`

Modern and extendible **`C# 8`** data access built for **.NET Core 3** era.

> Warning
>**THIS IS NOT ORM** `Norm` is `NoORM`, or not an `ORM` (although, there is O/R mapping extension).

`Norm` will postpone any reads from database until they are needed - allowing you to build expression trees and transformations - before it started fetching any data.

This allows avoiding unneccessary iterations and much greater flexibility.

By default it will return iterator over tuples and not serialized instances.

Because that's what databases do returns - **tuples.**

This allows for more extendibility - iterator results can be then further extended or mapped and transformed to something else (such as dictioanires or O/R mappings, see [O/R mapping](https://github.com/vbilopav/NoOrm.Net#working-with-results-and-objectrelational-or-mapping).

## [Change log](https://github.com/vbilopav/NoOrm.Net/blob/master/CHANGES.md)

## [How It Works - Why **Norm** - And - Similarities With Dapper](https://github.com/vbilopav/NoOrm.Net/blob/master/HOW-IT-WORKS.md)

## Test coverage and usage examples

Usage examples can be found in [unit tests](https://github.com/vbilopav/NoOrm.Net/tree/master/Tests/)

Around 99% of API is covered with tests only for [`PostgreSQL`](https://github.com/vbilopav/NoOrm.Net/tree/master/Tests/PostgreSqlUnitTests) and [`Microsoft SQL Server`](https://github.com/vbilopav/NoOrm.Net/tree/master/Tests/SqlServerUnitTests).

Other types of databases should work theoretically but they are not currently tested.

## Quick examples

- Get the first value from single row:

```csharp
var value = connection.Single<string>("select v from t limit 1");
```

- Get the first and second values from single row:

```csharp
var (value1, value2) = connection.Single<string, string>("select v1, v2 from t limit 1");
```

- Get the first value from single row and pass parameter by position

```csharp
var value = connection.Single<string>("select v from t limit 1 where id = @id", 1);
```

- Get the first value from single row and pass parameter by name

```csharp
var value = connection.Single<string>("select v from t limit 1 where id = @id", ("id", 1));
```

- Iterate trough three values

```csharp
foreach(var (value, value2, value3) in connection.Read<string, int bool>("select v1, v2, v3 from t"))
{
    // do something with value, value2, value3
}
```

- Map to class instance

```csharp
var instance = connection.Single("select * from t limit 1").Select<MyClass>();
```

- Map to enumerable of instances (query si execute on enumeration):

```csharp
var instances = connection.Read("select * from t limit 1").Select<MyClass>();
```

- Get the async stream of values from database:

```csharp
await foreach(var (value, value2, value3) in connection.ReadAsync<string, int bool>("select v1, v2, v3 from t"))
{
    // do something with value, value2, value3
}
```

- Get the async stream of instances from database:

```csharp
await foreach(var instance in connection.ReadAsync("select v1, v2, v3 from t").Select<MyClass>())
{
    // do something with each instance
}
```

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
| `As`, `AsProcedure`, `AsText`, `Timeout`, `WithJsonOptions`, | Provide general functionality like changing command type from procedure to test, setting the timeout ...|
| Extensions | Set of `IEnumerable` and `IAsyncEnumerable` extensions to convert database tuples to lists and dictionaries. New extensions can be added on will (for object mapping for example). |

### Working with database parameters

Each database connection method can receive list of parameters that are mapped to appropriate `DbParameter` to avoid SQL injection.

There are two overloads:

#### Positional parameters by value

Map parameter by position (name is not important, but it must start with `@` by convention)

```csharp
connection.Execute("select @p1, @p2", value1, value2);
connection.Execute("select @p1, @p2, @third", value1, value2, value3);
// etc...
```

Values are parameter values which are mapped to appropriate type by underlying database connection provider.

#### Named parameters parameters by value

Map parameter by exact name, position is not important:

```csharp
connection.Execute("select @p1, @p2", ("p1", value1), ("p2", value2));
connection.Execute("select @p1, @p2, @third", ("p1", value1), ("p2", value2), ("third", value3));
// etc...
```

Values are parameter values which are mapped to appropriate type by underlying database connection provider.

#### Passing specific `DbParameter` value

First overload for positional parameters can receive concrete `DbParameter` instead of object value if we want to narrow parameter type more precisely.

For example, using PostgreSQL parameters would look like this:

```csharp
var (s, i, b, d) = connection.Single<string, int, bool, DateTime>(
    "select @s, @i, @b, @d",
    new NpgsqlParameter("s", "str"),
    new NpgsqlParameter("i", 999),
    new NpgsqlParameter("b", true),
    new NpgsqlParameter("d", new DateTime(1977, 5, 19)));
```

or same call for Microsoft SQL Server:

```csharp
var (s, i, b, d) = connection.Single<string, int, bool, DateTime>(
    "select @s, @i, @b, @d",
    new SqlParameter("s", "str"),
    new SqlParameter("i", 999),
    new SqlParameter("b", true),
    new SqlParameter("d", new DateTime(1977, 5, 19)));
```

Note that this approach is actually named parameters approach, since parameters are matched by name again. So, position is not important.

Positional value parameters and `DbParameter` can be mixed. In that case `DbParameter` are populated first and ignored by value parameters later. For more examples, see this [unit tests](https://github.com/vbilopav/NoOrm.Net/blob/master/Tests/PostgreSqlUnitTests/ParametersUnitTests.cs) for PostgreSQL or this [unit tests](https://github.com/vbilopav/NoOrm.Net/blob/master/Tests/SqlServerUnitTests/ParametersUnitTests.cs) for Microsoft SQL Server.

Using concrete `DbParameter` parameter types allows us to use specific parameter types such as arrays or inet addresses for PostgreSQL and also to alter parameter direction and use output parameters.

### Available extensions

By convention any extension that Start with `Select` will build up expression tree and not trigger any iteration. Available extensions are:

| Extension | Extends | Description |
| ------------- |-------------|-------------|
| `SelectDictionary` | name and value tuple pairs | adds expression to build a dictionary from name, value tuples |
| `SelectDictionaries`, `SelectDictionariesAsync` | enumerator over name and value tuple pairs | add expression to build a enumerator (sync or async) - of dictionaries from collection of name, value tuples |
| `SelectValues` |  name and value tuple pairs or enumerator over name and value tuple pairs | Select only values from name value tuples |
| `Select<T>` | name and value tuple pairs or enumerator over name and value tuple pairs | Map to an instance of provided generic type (O/R mapping)

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

| | dapper read ([1](https://github.com/vbilopav/NoOrm.Net#1-dapper-query---read-and-serializes-one-million-rows-from-sql-query)) | norm read ([2](https://github.com/vbilopav/NoOrm.Net#2-norm-read-operation---builds-iterator-over-list-of-name-and-value-tuples)) | norm read ([3](https://github.com/vbilopav/NoOrm.Net#3-norm-read-operation-builds-iterator-over-database-tuples))  | norm read ([4](https://github.com/vbilopav/NoOrm.Net#4-norm-read-operation---builds-iterator-over-namevalue-dictionaries-and-use-it-to-build-iterator-over-testclass-instances)) | norm read ([5](https://github.com/vbilopav/NoOrm.Net#5-norm-read-operation---builds-iterator-over-generic-typed-tuples-and-use-use-it-to-build-iterator-over-testclass-instances)) | norm read ([6](https://github.com/vbilopav/NoOrm.Net#6-norm-read-operation---builds-iterator-over-de-serialized-json-to-class-instances)) | norm read ([7](https://github.com/vbilopav/NoOrm.Net#7-norm-read-operation---builds-iterator-over-class-instances-mapped-with-selectt-or-mapping-extension)) |
| - | --------- | --------  | --------  | --------  | --------  | --------  | --------  |
| 1 | 0:02.907 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 2 | 0:02.778 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.002 | 0:00.001 | 0:00.001 |
| 3 | 0:02.992 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 4 | 0:02.765 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.002 | 0:00.001 | 0:00.001 |
| 5 | 0:02.813 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 6 | 0:02.540 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.002 | 0:00.001 | 0:00.001 |
| 7 | 0:02.813 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 8 | 0:02.933 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 9 | 0:02.762 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 10 | 0:03.282 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.002 | 0:00.001 | 0:00.001 |
| AVG | **0:02.859** | **0:00.002** | **0:00.001** | **0:00.001** | **0:00.002** | **0:00.001** | **0:00:00.001** |

Following table shows execution times of count operations over enumeration results from same operations.

| | dapper count ([1](https://github.com/vbilopav/NoOrm.Net#1-dapper-query---read-and-serializes-one-million-rows-from-sql-query)) | norm count ([2](https://github.com/vbilopav/NoOrm.Net#2-norm-read-operation---builds-iterator-over-list-of-name-and-value-tuples)) | norm count ([3](https://github.com/vbilopav/NoOrm.Net#3-norm-read-operation-builds-iterator-over-database-tuples)) | norm count ([4](https://github.com/vbilopav/NoOrm.Net#4-norm-read-operation---builds-iterator-over-namevalue-dictionaries-and-use-it-to-build-iterator-over-testclass-instances)) | norm count ([5](https://github.com/vbilopav/NoOrm.Net#5-norm-read-operation---builds-iterator-over-generic-typed-tuples-and-use-use-it-to-build-iterator-over-testclass-instances)) | norm count ([6](https://github.com/vbilopav/NoOrm.Net#6-norm-read-operation---builds-iterator-over-de-serialized-json-to-class-instances)) | norm count ([7](https://github.com/vbilopav/NoOrm.Net#7-norm-read-operation---builds-iterator-over-class-instances-mapped-with-selectt-or-mapping-extension)) |
| - | --------- | --------  | --------  | --------  | --------  | --------  | --------  |
| 1 | 0:00.002 | 0:02.537 | 0:02.391 | 0:02.914 | 0:02.304 | 0:04.219 | 0:03.042 |
| 2 | 0:00.001 | 0:02.319 | 0:02.088 | 0:02.999 | 0:01.999 | 0:03.631 | 0:02.957 |
| 3 | 0:00.001 | 0:02.232 | 0:01.862 | 0:02.488 | 0:02.157 | 0:03.949 | 0:02.719 |
| 4 | 0:00.002 | 0:02.439 | 0:03.111 | 0:03.208 | 0:02.878 | 0:04.799 | 0:03.081 |
| 5 | 0:00.001 | 0:02.216 | 0:02.069 | 0:02.578 | 0:02.179 | 0:03.966 | 0:02.532 |
| 6 | 0:00.001 | 0:02.113 | 0:01.910 | 0:02.541 | 0:02.007 | 0:03.577 | 0:02.506 |
| 7 | 0:00.001 | 0:02.118 | 0:02.201 | 0:03.068 | 0:03.107 | 0:05.965 | 0:03.705 |
| 8 | 0:00.002 | 0:02.596 | 0:02.338 | 0:02.781 | 0:02.483 | 0:04.602 | 0:03.756 |
| 9 | 0:00.002 | 0:02.249 | 0:02.190 | 0:02.780 | 0:02.700 | 0:04.213 | 0:02.866 |
| 10 | 0:00.002 | 0:02.372 | 0:02.229 | 0:02.654 | 0:02.190 | 0:03.911 | 0:03.056 |
| AVG | **0:00.001** | **0:02.319** | **0:02.239** | **0:02.801** | **0:02.400** | **0:04.283** | **0:03.022** |

### 1. Dapper query - read and serializes one million rows from SQL query

```csharp
// Average execution time: 0:02.859
IEnumerable<TestClass> results1 = connection.Query<TestClass>(sql);

// Average execution time: 0:00.001
results1.Count();
```

### 2. Norm read operation - builds iterator over list of name and value tuples

```csharp
// Average execution time: 0:00.002
IEnumerable<IList<(string name, string value)>> results2 = connection.Read(sql);

// Average execution time: 0:02.319
results2.Count();
```

### 3. Norm read operation, builds iterator over database tuples

```csharp
// Average execution time: 0:00.001
IEnumerable<(int, string, string, DateTime)> results3 = connection.Read<int, string, string, DateTime>(sql);

// Average execution time: 0:02.239
results3.Count();
```

### 4. Norm read operation - builds iterator over name/value dictionaries and use it to build iterator over `TestClass` instances

```csharp
// Average execution time: 0:00.001
IEnumerable<TestClass> results4 = connection.Read(sql).SelectDictionaries().Select(dict => new TestClass(dict));

// Average execution time: 0:02.801
results4.Count();
```

### 5. Norm read operation - builds iterator over generic, typed tuples and use use it to build iterator over `TestClass` instances

```csharp
// Average execution time: 0:00.002
IEnumerable<TestClass> results5 = connection.Read<int, string, string, DateTime>(sql).Select(tuple => new TestClass(tuple));

// Average execution time: 0:02.400
results5.Count();
```

### 6. Norm read operation - builds iterator over de-serialized JSON to class instances

```csharp
// Average execution time: 0:00.001
IEnumerable<TestClass> results6 = connection.Json<TestClass>(JsonTestQuery))

// Average execution time: 0:04.283
results5.Count();
```

### 7. Norm read operation - builds iterator over class instances mapped with `Select<T>` O/R mapping extension

```csharp
// Average execution time: 0:00.001
IEnumerable<TestClass> results7 = connection.Read(TestQuery).Select<TestClass>());

// Average execution time: 0:03.022
results7.Count();
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
