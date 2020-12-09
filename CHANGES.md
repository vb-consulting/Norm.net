# Version history

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
