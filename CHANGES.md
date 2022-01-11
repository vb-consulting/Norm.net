# Version history and release notes

## 3.3.9

- Everything from 3.3.8 is only true where property is class type of object and it is not string:

```cs

public class BaseRef
{
}

public class MyRefClass : BaseRef
{
}


public class MyClassWithVirtualRef
{
    public int Value1 { get; set; } // maps, normal property
    public virtual BaseRef Ref1 { get; set; } // does not map, virtual class
    public virtual MyRefClass Ref2 { get; set; } // does not map, virtual class
    public virtual int VirtualValue { get; set; } // maps, not class type
    public virtual DateTime Date { get; set; } // maps, not class type
    public virtual string Str { get; set; } // maps, class type but string
    public virtual Guid Guid { get; set; } // maps, not class type
}
```

## 3.3.8

- Skip mappings for virtaul properties when mapping class or records.

This is mainly for EF models compatibility. Example:

```cs
public class MyRefClass
{
    public int Value2 { get; set; }
}

public class MyClassWithVirtualRef
{
    public int Value1 { get; set; }
    public virtual MyRefClass Ref { get; set; }
}

var result = connection.Read<MyClassWithVirtualRef>(@"select 1 as value1").First();
Assert.Equal(1, result.Value1);
Assert.Null(result.Ref);
```


## 3.3.7

- Added support anonymous class instance parameters:

```csharp
connection.Execute("update table set name = @name where id = @id", new {name = name, id = id});
```

or shorter:

```csharp
connection.Execute("update table set name = @name where id = @id", new {name, id});
```

- Situations where unintentionally by mistake - query has multiple parameters with the same name when using parameters by position, like this:

```csharp
connection.Execute("update table set name = @name where id = @id and parentId = @id", name, id);
```

New exception is now raised: `NormParametersException` with message `Parameter name {name} appears more than once. Parameter names must be unique.`

## 3.3.6

- Internal optimizations and improvements to mapper code.

- New perfomance benchmarks, now using standard BenchmarkDotNet library. See [benchmarks page](https://github.com/vb-consulting/Norm.net/blob/master/PERFOMANCE-TESTS.md)

## 3.3.5

Small inconsistency fix: **class instance properties with protected access modifiers will not be mapped any more.**

Only instance properties with public setters are mapped for now on:

```csharp
public class MyClass
{
    public int Value1 { get; set; }             // mapped
    public int Value2 { get; init; }            // mapped
    public int Value3;                          // not mapped, no public setter found
    public int Value4 { get; }                  // not mapped, no public setter found
    public int Value5 { get; private set; }     // not mapped, setter is private
    public int Value6 { get; protected set; }   // not mapped, setter is protected
    public int Value6 { get; internal set; }    // not mapped, setter is internal
}
```

There is no reason to map protected properties since, by design and intent, they are expected to be mapped by derived classes not by library.

Additionally, full list of example files in one place is provided. See here -> [EXAMPLES](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md)

## 3.3.4

- Breaking change:

Mapping to properties with private setters is not permitted any more. 

Because it does not make any sense to do so. You want to have ability to have property that is ignored by the mapper and set the value yourself.

All private fields will be ignored.

- Properties without getter are also ignored, mapper will not throw an error.

## 3.3.3

BUGFIX

MS SQL Server uses `@@` syntax for global values. For example `@@rowcount` or `@@error`.

When using positional parameters in a query that uses these `@@` global variables, parser `@@` is recognized as a parameter, which is wrong.

Now, all `@@` values are ignored.

## 3.3.2

Added an ability to pass a standard POCO object as a parameter and parse each property as a named parameter by the property name.

For example:

```csharp
 class PocoClassParams
 {
     public string StrValue { get; set; } = "str";
     public int IntValue { get; set; } = 999;
     public bool BoolValue { get; set; } = true;
     public DateTime DateTimeValue { get; set; } = new DateTime(1977, 5, 19);
     public string NullValue { get; set; } = null;
 }

//...

using var connection = new NpgsqlConnection(fixture.ConnectionString);
var (s, i, b, d, @null) = connection.Read<string, int, bool, DateTime, string>(
        "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue", new PocoClassParams())
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
Assert.Null(@null);
```

In this example, there is only one parameter passed to a `Read` method command.

Each property is parsed as an actual database parameter by name correctly to the following parameters:`@StrValue`, `@IntValue`, `@BoolValue`, `@DateTimeValue`, `@NullValue` respectively.

This is a requested feature because there are many situations when parameters that we need to send to a command are already in some type of object instance, like web requests, and similar. In that situation, previously we would pass each parameter individually. So, now, we can simply pass an object instance.

We can also mix this approach with positional parameters. For example:

```csharp
 class PocoClassParams
 {
     public string StrValue { get; set; } = "str";
     public int IntValue { get; set; } = 999;
     public bool BoolValue { get; set; } = true;
     public DateTime DateTimeValue { get; set; } = new DateTime(1977, 5, 19);
     public string NullValue { get; set; } = null;
 }

//...

var (s, i, b, d, @null, p1) = connection.Read<string, int, bool, DateTime, string, string>(
        "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1", 
        new PocoClassParams(), "pos1")
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
Assert.Null(@null);
Assert.Equal("pos1", p1);
```

The first parameter is a class instance that has 5 properties and each property is mapped by name, and the second parameter is a string literal which is mapped by the position.

In the case of positional the parameters, name of the parameter (in this case `@pos1`) is irrelevant since they are mapped by the position, not by name.

The parser will first add parameters from the class instance, and what is left is mapped by the position.
That means that if we place positional parameter first, it will work just as same:

```csharp
var (s, i, b, d, @null, p1) = connection.Read<string, int, bool, DateTime, string, string>(
        "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1", 
        "pos1", new PocoClassParams())
    .Single();
```

Also, we can mix this approach with the actual database parameters, for example:

```csharp
var (s, i, b, d, @null, p1) = connection.Read<string, int, bool, DateTime, string, string>(
        "select @StrValue, @IntValue, @BoolValue, @DateTimeValue, @NullValue, @pos1", 
        new NpgsqlParameter("pos1", "pos1"), new PocoClassParams())
    .Single();
```

In this case, all parameters were mapped by name, not by position.

Parameters from the object instance by property name, and the parameter from database parameter by parameter name.

## 3.3.1

### Native `Guid` type mapping.

Native mapping for all types that generate `Guid` type on data reader is now properly mapped.

This applies to classes, records, value types, and named types.

Mapping of PostgreSQL UUID Arrays to `Guid[]` type is also supported, only for classes and records. 

### Global mapping handlers

It's now possible to define global mappings for certain class/record types.

New static class `SqlMapper` in `Norm` namespace defines four methods for global mapping:

```csharp
namespace Norm
{
    public static class SqlMapper
    {
        /// <summary>
        /// Define global custom mapping for class type by defining a row handler with name and value tuple array.
        /// </summary>
        /// <typeparam name="T">Class type that will register for custom mapping.</typeparam>
        /// <param name="handler">Function handler that sends a name and value tuple array as aparameter for each row.</param>
        public static void AddTypeByTuples<T>(Func<(string name, object value)[], T> handler) where T : class { ... }

        /// <summary>
        /// Define global custom mapping for class type by defining a row handler with value array.
        /// </summary>
        /// <typeparam name="T">Class type that will register for custom mapping.</typeparam>
        /// <param name="handler">Function handler that sends a value array as aparameter for each row.</param>
        public static void AddTypeByValues<T>(Func<object[], T> handler) where T : class { ... }

        /// <summary>
        /// Define global custom mapping for class type by defining a row handler with dictionary of names and values.
        /// </summary>
        /// <typeparam name="T">Class type that will register for custom mapping.</typeparam>
        /// <param name="handler">Function handler that sends a dictionary of names and values as aparameter for each row.</param>
        public static void AddTypeByDict<T>(Func<IDictionary<string, object>, T> handler) where T : class { ... }
    }
}

```

For example, if we have a query that returns two GUIDs (id1, and id2) and we want to map to the class that has four fields, one string and one Guid for each value:

```csharp
class CustomMapTestClass
{
    public string Str1 { get; set; }
    public Guid Guid1 { get; set; }
    public string Str2 { get; set; }
    public Guid Guid2 { get; set; }
}
```

To register this class for global custom mapping:

```csharp
// add global mapper by using name and value tuples array:
SqlMapper.AddTypeByTuples(row => new CustomMapTestClass
{
    Str1 = row.First(r => r.name == "id1").value.ToString(),
    Guid1 = (Guid)row.First(r => r.name == "id1").value,
    Str2 = row.First(r => r.name == "id2").value.ToString(),
    Guid2 = (Guid)row.First(r => r.name == "id2").value
});

// add global mapper by using values array:
SqlMapper.AddTypeByValues(row => new CustomMapTestClass
{
    Str1 = row[0].ToString(),
    Guid1 = (Guid)row[0],
    Str2 = row[1].ToString(),
    Guid2 = (Guid)row[1]
});

// add global mapper by using dictionary of names and values:
SqlMapper.AddTypeByDict(row => new CustomMapTestClass
{
    Str1 = row["id1"].ToString(),
    Guid1 = (Guid)row["id1"],
    Str2 = row["id2"].ToString(),
    Guid2 = (Guid)row["id2"]
});
```

Each time we try to map to this registered type, this global mapper will be used: `connection.Read<CustomMapTestClass>("select uuid_generate_v4() id1, uuid_generate_v4() id2").ToList();`.

**Important notes on this implementation:**

- `SqlMapper` static methods are not thread-safe. They are supposed to be called once on program startup.
- Multiple mappings on the same type will not throw any error or exception. Last registered mapping will be used. But, don't do that, it's stupid.
- Only classes and record mappings are supported. There is a generic constraint preventing you to do this on named tuples.
- Mappings on multiple classes are not supported because of performances impact reasons. For example mapping `connection.Read<Class1, Class2>(query).ToList();` will not use global mapping handler and will fall back to default mapping.
- There is also a custom mapping technique that doesn't require global registration and relays on static extensions. See this example here: [https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/CustomMappingsUnitTests.cs#L96](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/CustomMappingsUnitTests.cs#L96)

## 3.3.0

### Internal improvement

Separated overload methods into separate files by using partials for easier maintenance.

### Fixed very rare race condition bug that raised an index out of bounds exception when mapping in parallel same class to different queries.

This bug fix required a redesign of the entire mapping mechanism which is now even more optimized and uses even less memory cache. 

Benchmarks are available on the [benchmarks page](https://github.com/vb-consulting/Norm.net/blob/master/PERFOMANCE-TESTS.md)

Following [unit test](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/MapUnitTests.cs#L312) demonstrates the bug condition. 

## Added support fof the `DateTimeOffset` type. 

You can map your database datetime types to `DateTimeOffset` type properly. See the following unit tests:

- [Test_DateTimeOffsetType_Sync](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/QueryUnitTests.cs#L351) - map a class with a `DateTimeOffset` type
- [Test_DateTimeOffsetNullableType_Sync](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/QueryUnitTests.cs#L378) - map a class with a nullable `DateTimeOffset` type
- [Test_DateTimeOffset_Array_Sync](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/QueryUnitTests.cs#L404) - map an array field of the `DateTimeOffset` type

Note:
 `DateTimeOffset` is not supported for when mapping tupleas and named tuples. See [here](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/ReadTuplesUnitTests.cs#L484) and [here](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/ReadTuplesUnitTests.cs#L498). Reason is that it would have big impact on mapping mechanism in terms of perfomances. If you still want to use `DateTimeOffset` tuples with Norm, please map to the DateTime first and use `Select` to re-map into something else, like  `DateTimeOffset`. See example [here](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/MapUnitTests.cs#L293). `Select` method provides an additional mapping logic without any perfomance impact.


## 3.2.0

**Support for the `FormattableString` commands.**

Now each method that receives a command also have a version that can receive `FormattableString` instead of string, where format arguments are parsed as valid database parameters to avoid injection.

For example, before:

```csharp
connection.Read("select @p1, @p2", 1, 2);
```

Now can be written as 

```csharp
connection.ReadFormat($"select {1}, {2}");
```

Where arguments can be any object value that will be parsed to database parameter, or valid `DbParameter` instance. 

For example:

```csharp
connection.ReadFormat($"select {new SqlParameter("", 1)}, {new SqlParameter("", 1)}");
```

You can even mix parameter values and `DbParameter` instances:

```csharp
connection.ReadFormat($"select {1}, {new SqlParameter("", 2)}");
```

Notes:
- You can use `DbParameter` instances to set parameter types more precisely.
- When using `DbParameter` instances name of the parameter is irelevant, argument parser will asign a new name. In examples above, name is left blank.

This applies to all extensions that receive a command string. New method extensions are:

```
ReadFormat
ReadFormatAsync
ExecuteFormat
ExecuteFormatAsync
MultipleFormat
MultipleFormatAsync
```

See unit tests [here](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/FormattableUnitTests.cs).

## 3.1.2

Fix rare cache issue not updating cache when `TimeSpan` is resolved.

## 3.1.0

#### 1) Support for multiple mappings from the same command

Norm can now map to multiple instances from the same command. Example:

```csharp
record Record1(int Id1, int Id2);
record Record2(int Id3, int Id4);

//...
var sql = "select 1 as id1, 2 as id2, 3 as id3, 4 as id4";
var (record1, record2) = connection.Read<Record1, Record2>(sql).Single();

Console.WriteLine(record1.Id1); // outputs 1
Console.WriteLine(record1.Id2); // outputs 2
Console.WriteLine(record2.Id3); // outputs 3
Console.WriteLine(record2.Id4); // outputs 4
```

This also works with named tuples. Example:

```csharp
var sql = "select 1 as id1, 2 as id2, 3 as id3, 4 as id4";
var (record1, record2) = connection.Read<(int Id1, int Id2), (int Id3, int Id4)>(sql).Single();

Console.WriteLine(record1.Id1); // outputs 1
Console.WriteLine(record1.Id2); // outputs 2
Console.WriteLine(record2.Id3); // outputs 3
Console.WriteLine(record2.Id4); // outputs 4
```

Multiple mapping works for all `Read` and `ReadAsync` overloads (up to 12 multiple mappings).

#### 2) Other improvements

- Named tuples mapping massivly improved with much better perfomances (on pair with Dapper).
- Named tuples now allow mapping with less named tuple members then result in query.
- Better error handling: Added NormException as base Exception for all known Exceptions.

## 3.0.0
 
### Breaking changes
 
**Consolidation and simplification of the interface.**
 
#### 1) Removed extensnions `Query` and `QueryAsync` with all overloads
 
Functionalities of these extensions are now implemented in `Read` and `ReadAsync` extensions.
 
For example, before:
 
```csharp
public class MyClass { ... }
public record MyRecord(...);
 
// mapping to class instances enumeration generator
var r1 = connection.Query<MyClass>(sql);
 
// mapping to record instances enumeration generator
var r2 = connection.Query<MyRecord>(sql);
 
// mapping to named value tupleas enumeration generator
var r3 = connection.Query<(...)>(sql);
```
 
Now, all of this is done by existing `Read` extension. Example:
 
```csharp
public class MyClass { ... }
public record MyRecord(...);
 
// mapping to class instances enumeration generator
var r1 = connection.Read<MyClass>(sql);
 
// mapping to record instances enumeration generator
var r2 = connection.Read<MyRecord>(sql);
 
// mapping to named value tupleas enumeration generator
var r3 = connection.Read<(...)>(sql);
```
 
Note that existing functionality of `Read` extension remeains the same. You can still map single value or tuples. Example:
 
```csharp
// map single int values to int enumeration generator
var r1 = connection.Read<int>(sql);
 
// map unnamed int and string tuples to int and string enumeration generator
var r2 = connection.Read<int, int>(sql); 
 
// map unnamed int, int and string tuples to int, int and string enumeration generator
var r3 = connection.Read<int, int, string>(sql);
```
 
#### 2) Removed extensnions `Single` and `SingleAsync` with all overloads
 
These extensions are completely unnecessary. 
 
Since `Read` extensions are returning enumeration generators, **identical functionalities** can be achieved with `LINQ` extensnions.
 
 
For example, before:
 
```csharp
// map to single int value
var i = connection.Single<int>(sql);
 
// map to two int values
var (i1, i2) = connection.Single<int, int>(sql); 
 
// map to two int and single string values
var (i1, i2, s) = connection.Single<int, int, string>(sql); 
```
 
Now, identical functionality can be achieved with `Single` or `First` LINQ extension. Example:
 
```csharp
using System.Linq;
 
// map to single int value
var i = connection.Read<int>(sql).Single();
 
// map to two int values
var (i1, i2) = connection.Read<int, int>(sql).Single(); 
 
// map to two int and single string values
var (i1, i2, s) = connection.Read<int, int, string>(sql).Single(); 
```
 
### New functionality - multiple results
 
Norm supports queries returning multiple results.
 
For example, following query returns two result sets with different names:
 
```csharp
public Queires = @"
    select 1 as id1, 'foo1' as foo1, 'bar1' as bar1; 
    select 2 as id2, 'foo2' as foo2, 'bar2' as bar2";
```
 
Mapping to a record type example:
 
```csharp
using var connection = new SQLiteConnection(fixture.ConnectionString);
using var multiple = connection.Multiple(Queires);

var result1 = multiple.Read<Record1>();
multiple.Next();
var result2 = multiple.Read<Record2>();
```
 
- Extension `Multiple` executes a command with multiple select statements and returns a disposable object.
 
- Use that object to read the results and avance to the next result set:
 
```csharp
using var connection = new SQLiteConnection(fixture.ConnectionString);
using var multiple = connection.Multiple(Queires);
while (multiple.Next())
{
    var result = multiple.Read<MyStructure>();
}
```
 
- Extension `Multiple` receives command with parameters same way as any other method that executes sql:
 
```csharp
using var connection = new SQLiteConnection(fixture.ConnectionString);
using var multiple = connection.Multiple(QueiresWithParams, 1, "bar2");
// - or -
using var multiple = connection.Multiple(QueiresWithParams, ("bar2", "bar2"), ("id1", 1));
// - or -
using var multiple = connection.Multiple(QueiresWithParams, ("bar2", "bar2", DbType.String), ("id1", 1, DbType.Int32));
// - or -
using var multiple = connection.Multiple(QueiresWithParams, ("bar2", "bar2", SqlDbType.VarChar), ("id1", 1, SqlDbType.Int));
 
```
 
- Or, asynchronous version:
 
```csharp
using var connection = new SQLiteConnection(fixture.ConnectionString);
using var multiple = await connection.MultipleAsync(Queires);
 
var result1 = await multiple.ReadAsync<Record1>().SingleAsync();
await multiple.NextAsync();
var result2 = await multiple.ReadAsync<Record2>().SingleAsync();
```
 
### Improvements and bugfixes
 
- Reading named tuples can now have up to 14 members. Example:
 
```
connection.Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query)
```
 
There was a bug that caused crashing when reading more than 8 named tuples. That is fixed and the limit is 14. 
More than 14 will raise `ArgumentException` with the message: `Too many named tuple members. Maximum is 14.`.
 
- Added missing cancelation tokens on asynchronous reads. Before cancelation wouldn't stop the asynchronous iteration, just the command. That is fixed.

## 2.0.8

Add type checking for mapping methods `Query` and `QueryAsync`.

Those methods will throw AgrumentException with appropriate message if generic type parameter is not either class, record or value tuple.

## 2.0.7

- `Query` and `QueryAsync` methods can now also work with value tuples. 

Example:

```csharp
var result = connection.Query<(int id, string name)>("select 1 as id, 'myname' as name").First();
Console.WriteLine(result.id); // outputs 1
Console.WriteLine(result.name); // outputs myname
```

Limitation: **Value tuples are mapped by matching position in the query.**

Meaning, value tuple members `(int id, string name)` must match positions of query results `'myid' as id, 'myname' as name"`. Names are completely irelevant, this works identically with the query `"select 1, 'myname'`.

## 2.0.6

- fix comments documentation for package

## 2.0.5

- include comments documentation in package

## 2.0.4

- include source link package

## 2.0.3

- Added comment documentation for all public methods
- Command extensnion changed to internal access
- Type cache for GetProperties made public for future extensions
- Added missing Query methods to INorm interface

## 2.0.2
## 2.0.1

Version 2.0.1, 2.0.2 are identical to 2.0.0 and used only for building packeges with source links to github repo and automated publish integration.

## 2.0.0

### Breaking changes

#### 1) Return types for `Read()`, `ReadAsync()`, `Single()` and `SingleAsync()` changed to return array instead of lists:

- `Read()` now returns `IEnumerable<(string name, object value)[]>`
- `ReadAsync()` now returns `IAsyncEnumerable<(string name, object value)[]>`
- `Single()` now returns `(string name, object value)[]`
- `SingleAsync()` now returns `ValueTask<(string name, object value)[]>`

The list of tuples returned from the database should never be resized or changed. Also, small optimization for object mapper.

#### 2) Tuple array extensions `SelectDictionary`, `SelectDictionaries` and `SelectValues` have been removed also. They can be replaced with simple LINQ expressions.

#### 3) Extension on tuple array (previously list) `(string name, object value)[]` is renamed from `Select` to `Map`

#### 4) `Norm.Extensions` namespace is removed. Now, only `Norm` namespace should be used.

#### Migration from 1.X version of library

1) Replace  `using Norm.Extensions;` with `using Norm;`
2) Replace all calls `connection.Read(...).Select<MyClass>` with `connection.Read(...).Map<MyClass>` or `connection.Query<MyClass>(...)`
3) Replace all calls `connection.ReadAsync(...).Select<MyClass>` with `connection.Read(...).MapAsync<MyClass>` or `connection.QueryAsync<MyClass>(...)`
4) Replace `SelectDictionary`, `SelectDictionaries` and `SelectValues` with following LINQ expressions:

LINQ expression:

- `SelectDictionary` with `tuples.ToDictionary(t => t.name, t => t.value);`
- `SelectDictionaries` with `tuples.Select(t => t.ToDictionary(t => t.name, t => t.value));`
- `SelectValues` with `tuples.Select(t => t.value);`

### New API

#### 1) `Query` and `QueryAsync` with overloads:

- `IEnumerable<T> Query<T>(this DbConnection connection, string command)`
- `IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command)`
- `IEnumerable<T> Query<T>(this DbConnection connection, string command, params object[] parameters)`
- `IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params object[] parameters)`
- `IEnumerable<T> Query<T>(this DbConnection connection, string command, params (string name, object value)[] parameters)`
- `IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params (string name, object value)[] parameters)`
- `IEnumerable<T> Query<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)`
- `IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params (string name, object value, DbType type)[] parameters)`
- `IEnumerable<T> Query<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)`
- `IAsyncEnumerable<T> QueryAsync<T>(this DbConnection connection, string command, params (string name, object value, object type)[] parameters)`

This new API is just shorthand for standard object-mapping calls.

Instead of `Read(command, parameters).Map<T>()` (previosly `Select`) you can use `Query<T>(command, parameters)`.


### Improvements


Object mapper is rewritten from scratch and it has many performance improvements.

See [performance tests here](https://github.com/vb-consulting/Norm.net/blob/master/PERFOMANCE-TESTS.md).

For list of changes in older versions, go [here](https://github.com/vbilopav/NoOrm.Net/blob/master/CHANGES.md).
