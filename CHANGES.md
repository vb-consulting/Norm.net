# Version history

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
