# Version history

## 1.3.0

#### Added support for **prepared statemnts.** 

- To execute prepared statement execute `Prepared()` method before any given execution:

```csharp
connection.Prepared().Execute("command 1").Execute("command 2");
```

In example above only `command 1` is executed as prepared. 

- To execute both commands as prepared `Prepared()` call must preceed command execution:

```csharp
connection.Prepared().Execute("command 1").Prepared().Execute("command 2");
```

- In SQL Server, prepared statements don't have [has no significant performance advantage over direct execution](https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/ms175528(v=sql.105)?redirectedfrom=MSDN)

- In PostgreSQL, prepared statements have [17% performance increase](http://www.roji.org/prepared-statements-in-npgsql-3-2)

## 1.2.0

- Extensions that will change state of connection object like `As`, `AsProcedure`, `AsText`, `Timeout`, `WithJsonOptions` are now thread safe. That means that connection can safely be singleton or static object.
- New (thread safe) extension `WithCancellationToken` that will throw exception if cancellation is requested and propogate cancellation token to all async calls.
- Extensions `As`, `AsProcedure`, `AsText`, `Timeout`, `WithJsonOptions` and `WithCancellationToken` will only apply to current call chain.

## 1.1.9

- New overload for all functions with named parameters. Named parametzers now accept DBType along name and value.

## 1.1.8

- All parameters with `null` value will be interpreted as database null value (`DBNull.Value`)

## 1.1.7

- Added tests and support for SQLite database
- Add 11 and 12 tuples overloads

## 1.1.6

- Removes unneccessary dependency to `System.Linq.Async`
- Improves Async mapping `Select` and `JsonAsync`
- Remove obsolete extensions

## 1.1.4

- O/R mapping extension method on `IAsyncEnumerable` called `SelectAsync<TModel>` is deprecated in favor of `Select<TModel>` which is more consistent with `AsyncLinq` approach.

## 1.1.3

- Positional parameters can now receive parameters of native type derived from `DbParameter`. 
This allows custom types of parameters to be passed to query (PostgreSQL array types for example) and
eliminates need for `WithOutParameter` and `GetOutParameter` which are removed. See this [tests](https://github.com/vbilopav/NoOrm.Net/blob/master/Tests/PostgreSqlUnitTests/ParametersUnitTests.cs) for examples.

## 1.1.2

- Fixed extensions to use `IList` of tuples instead of `IEnumerable`

## 1.1.1

- Replaced `FastMember` O/R Mapping `Select<T>` extensions with custom implementation

## 1.1.0

- All non-generic result types `IEnumerable<(string name, object value)>` - are replaced with materialized lists, type: `IList<(string name, object value)>`.

- Consequently name/value tuple results are generating lists structure and do not deffer serialization and this allowed simplification of extensions. Current list of extensions:
[see here](https://github.com/vbilopav/NoOrm.Net/blob/master/Norm/Extensions/NormExtensions.cs)

- Added extension for O/R Mapping  by using [`FastMember`](https://github.com/mgravell/fast-member) library

Note:
`FastMember` yields slightly better results then Dapper but it doesn't support Ahead of Time compilation scenarios and members are case sensitive.

- Expended generic tuple parameters up to 10 members max. Will be more in future.
