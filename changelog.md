# Changelog

## [5.1.0](https://github.com/vb-consulting/Norm.net/tree/5.1.0) (2022-06-20)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.0.1...5.1.0)

### Parameters can be set again in `Read` and `Execute` extensions methods.

This feature was removed after version 5.0.0.

The reason was that caller info default parameters were added and the `params` array doesn't work well with default parameters.

However, that doesn't mean that optional parameter object `object parameters = null` with null default value can't be used.

There is no convenience of the parameters array, but, it still can be used for an anonymous object which is the most common use case.

For example, now we can do this again:

```csharp
var (s, i, b, d, @null) = connection
    .Read<string, int, bool, DateTime, string>(
        "select @strValue, @intValue, @boolValue, @dateTimeValue, @nullValue",
        new
        {
            strValue = "str",
            intValue = 999,
            boolValue = true,
            dateTimeValue = new DateTime(1977, 5, 19),
            nullValue = (string)null,
        })
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
Assert.Null(@null);
```

However, the current extension methods `WithParameters` was not removed, and it's possible to combine both. The resulting parameters will be merged. Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new
    {
        strValue = "str",
        intValue = 999,
    })
    .Read<string, int, bool, DateTime, string>(
        "select @strValue, @intValue, @boolValue, @dateTimeValue, @nullValue",
        new
        {
            boolValue = true,
            dateTimeValue = new DateTime(1977, 5, 19),
            nullValue = (string)null,
        })
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
Assert.Null(@null);
```

This new optional paramemeter `paramemeters` on `Read` and `Execute` extensions is no longer an array. To be able to pass parameters array, actual array needs to be constructed. Example:

```csharp
var (s, i, b, d) = connection
    .Read<string, int, bool, DateTime>(
        "select @s, @i, @b, @d",
        new NpgsqlParameter[] 
        {
            new NpgsqlParameter("s", "str"),
            new NpgsqlParameter("i", 999),
            new NpgsqlParameter("b", true),
            new NpgsqlParameter("d", new DateTime(1977, 5, 19))
        })
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
```

### `WithTransaction(DbTransaction)` connection extension method.

Support for the transaction control by using `DbTransaction` object. 

`WithTransaction(DbTransaction)` will set the transaction object for the next command.

Example:

```csharp
using var connection = new NpgsqlConnection(fixture.ConnectionString)
    .Execute("create temp table transaction_test1 (i int);");

using var transaction = connection.BeginTransaction();

connection
    .WithTransaction(transaction)
    .Execute("insert into transaction_test1 values (1),(2),(3);");

var result1 = connection.Read("select * from transaction_test1").ToArray();
Assert.Equal(3, result1.Length);

transaction.Rollback();

var result2 = connection.Read("select * from transaction_test1").ToArray();
Assert.Empty(result2);
```

### `WithTimeout` connection extension method.

This is equivalent to the `Timeout` connection extension, added only for the naming consistency ("with" prefix).

It just sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error.

### `MapPrivateSetters` global option

By default, it is not possible to map instance properties that don't have a public setter method.

Now, it is possible to change that behavior with global options by using the `MapPrivateSetters` setting, like this:

```csharp
NormOptions.Configure(options => options.MapPrivateSetters = true); // default is false
```

This will enable mapping of the instance properties with private and protected setter methods:

```csharp
public class TestClass
{
    public int PrivateSetInt { get; private set; }
    public int ProtectedSetInt { get; protected set; }
}

```
```csharp

var result = connection
    .Read<TestClass>("select 1 as private_set_int, 2 as protected_set_int")
    .Single();

Assert.Equal(1, result.PrivateSetInt); // true
Assert.Equal(2, result.ProtectedSetInt); // true
```

Note that instance properties without any setter or a public getter are still not going to be mapped.

### Multiple readers now implement `IAsyncDisposable`

And now it's possible to do:

```csharp
await using var multiple = await connection.MultipleAsync(Queires);
```

## [5.0.1](https://github.com/vb-consulting/Norm.net/tree/5.0.1) (2022-06-16)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.0.0...5.0.1)

- Fixed weird edge case bug with mapping `datetimeoffset` on SQL Server, when actual type is `datetimeoffset`.
- Slightly improved parameters parsing when using positional parameters.

## [5.0.0](https://github.com/vb-consulting/Norm.net/tree/5.0.0) (2022-05-24)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.3.0...5.0.0)

### New feature - comment headers

From version 5, Norm can create SQL comment headers on all or some SQL commands. 

Those comment headers can include the following:

- Custom user comment.
- Parameters names, types, and values.
- Caller info such as calling method name, source code file name, and line number.
- Command attributes such as database provider, command type (text or stored procedure), and command timeout.
- Execution timestamp.

When enabled, this feature can help with diagnostics and monitoring through:

- Standard application loggers.
- Database monitoring tools such as Activity Monitor on SQL Server or monitoring tables on PostgreSQL such as `pg_stat_activity` or `pg_stat_statements`.

Quick example:

```csharp
public void ChangeLogExample1()
{
    var (foo, bar) = connection
        .WithCommentHeader(comment: "My foo bar query", includeCommandAttributes: true, includeParameters: true, includeCallerInfo: true)
        .WithParameters(new { foo = "foo value", bar = "bar value" })
        .Read<string, string>("select @foo, @bar")
        .Single();
}
```

This will produce the following command on SQL Server:

```
-- My foo bar query
-- Sql Text Command. Timeout: 30 seconds.
-- at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs 28
-- @foo nvarchar = "foo value"
-- @bar nvarchar = "bar value"
select @foo, @bar
```

Same code on PostgreSQL:

```
-- My foo bar query
-- Npgsql Text Command. Timeout: 30 seconds.
-- at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs 28
-- @foo text = "foo value"
-- @bar text = "bar value"
select @foo, @bar
```

These header values can be enabled for individual commands and globally for all commands.

A quick example of enabling logging of caller info and parameter values for all commands in application:

```csharp
/* On application startup */

NormOptions.Configure(options =>
{
    options.CommandCommentHeader.Enabled = true;
    options.CommandCommentHeader.IncludeCallerInfo = true;
    options.CommandCommentHeader.IncludeParameters = true;
    options.DbCommandCallback = command => logger.LogInformation(command.CommandText);
});
```

This allows you to log all your commands with all relevant information needed for debugging, such as parameter values, and also with source code information that allows you to locate commands in your source code.

And since it is included in the command comment header, they are visible on a database server as well through monitoring tools.

> Note: Caller info is generated at the compile time by the compiler, which makes them no different than constant values, so, there is no performance penalty for including such information.

To support this feature, several new extensions are added and support global configuration.

More details bellow:

### New method extensions

Important note:

Same `Norm` method extensions are available as `DbConnection` extension methods and Norm instance methods.

Those methods who don't return any results are returning `Norm` instance, which allows us to chain multiple calls like in example above or simplified:

```csharp
connection
    .WithCommentHeader(/*comment header options*/)
    .WithParameters(/*parameters*/)
    .Read(/*command*/);
}
```

#### `WithParameters(params object[] parameters)`

Sets parameters for the next command.

**This is a breaking change!**

In versions 4 and lower, parameters were supplied using this pattern:

```csharp
connection.Execute("command", param1, param2, param3);
```

From version 5, parameters are supplied using `WithParameters` extension method:

```csharp
connection
    .WithParameters(param1, param2, param3)
    .Execute("command");
```

#### `WithCommandCallback(Action<DbCommand> dbCommandCallback)`

Executes supplied action with constructed `DbCommand` as a single parameter just before the actual execution.

This exposes created `DbCommand` object and allows for eventual changes to be made to the `DbCommand` object such as changes in a command text, parameters collection, etc, depending on a provider.

There is also a global version of this method that is commonly used for logging tasks.

#### `WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)`

Executes reader callback function for each value just before assigning mapper results.

The input parameter is a named tuple of three values:

1) `Name` - field name from your command results.
2) `Ordinal` - ordinal position of the field from your command results.
3) `Reader` - `DbDataReader` object used for reading results.

This allows supplying custom results to mapper by returning values:

- If this function callback returns a non-null value, that value is mapped.
- If this function callback returns a null value, the default value is mapped.
- For null values, return `DBNull.Value`.

Example:

```csharp
var array = connection
    .WithReaderCallback(r => r.Ordinal switch
    {
        0 => r.Reader.GetInt32(0) + 1,  // add 1 to the first field with ordinal 0, mapped to the first tuple named "a"
        _ => null                       // for all other fields, use the default mapping
    })
    .Read<(int a, int b, int c)>("select * from (values (1, 1, 1), (2, 2, 2), (3, 3, 3)) t(a, b, c)")
    .ToArray();
```

Note: 

**This is a breaking change!**

In version 4 and lower, this callback function was supplied as an optional parameter to the `Read` method overloads extensions.

```csharp
connection.Read("command", r => { /*...*/});
```
From version 5, the `WithReaderCallback` extension method is used to supply this callback to the next read operation.

#### `WithComment(string comment)`

Adds a custom comment header to the next command.

#### `WithCommentParameters()`

Adds a comment header with command parameters described with names, database types, and values to the next command.

#### `WithCommentCallerInfo()`

Adds a comment header with caller info data (calling method names, source code file, and line number) to the next command.

#### `WithCommentHeader(string comment = null, bool includeCommandAttributes = true, bool includeParameters = true, bool includeCallerInfo = true, bool includeTimestamp = false)`

Adds a comment header  to the next command and configures options:

- `comment` - custom comment text.

- `includeCommandAttributes` - command attributes such as database provider, command type, and command timeout.

- `includeParameters` -  command parameters described with names, database types, and values.

- `includeCallerInfo` - caller info data (calling method names, source code file, and line number).

- `includeTimestamp` - timestamp of command execution.

### Global Configuration

Use the global static method `NormOptions.Configure(options => { /* options */ });` to configure global options.

This is usually called once at program startup. 
Do not call this from multithreaded sections of your application and do not call it more than once.

For example:

```csharp
NormOptions.Configure(options =>
{
    options.CommandTimeout = 10 * 60;
    options.CommandCommentHeader.Enabled = true;
    options.CommandCommentHeader.IncludeCallerInfo = true;
    options.CommandCommentHeader.IncludeParameters = true;
    options.DbCommandCallback = command => logger.LogInformation(command.CommandText);
});
```

This will configure all commands to have a timeout of 10 minutes and enables comment headers with parameters description.

Available options are:

| Option name | Description | Default |
| ----------- | ----------- | ------- |
| `CommandTimeout` | Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error | `NULL` (system default) |
| `DbCommandCallback` | Command callback that will be called before every execution. | `NULL` (don't run this callback) |
| `RawInterpolationParameterEscape` | Escape sequence, when using parameters via string interpolation formats, use this escape to skip parameter parsing and use values as is. | "raw" | 
| `Prepared` | Set to true to run all commands in prepared mode every time by calling `Prepare()` method before execution. | `false` | 
| `CommandCommentHeader` | Autmoatic command comment header options. | see table bellow |

`CommandCommentHeader` options are:

| Option name | Description | Default |
| ----------- | ----------- | ------- |
| `Enabled` | Enable automatic comment headers that are added to each query command. | `false` |
| `IncludeCommandAttributes` | Include current command attributes like database provider, command type, and command timeout. | `true` |
| `IncludeCallerInfo` | Include command caller info like calling method member name, source code file path, and source code line number. | `true` |
| `IncludeParameters` | Include command parameters in comment headers. | `true` |
| `ParametersFormat` | Format string for parameters in comment headers. Placeholder 0 is the parameter name, 1 is the parameter type and 2 is the parameter value. Used only when `IncludeParameters` is `true`. | `$"-- @{{0}} {{1}} = {{2}}{Environment.NewLine}"` |
| `IncludeTimestamp` | Include command execution timestamp in comment headers. | `false` |
| `OmmitStoredProcCommandCommentHeaderForDbTypes` | Omits comment headers if enabled from a command text when command type is Stored Procedure for the database providers that do not support comments in a Stored Procedure calls (SQL Server and MySQL). | ` DatabaseType.Sql | DatabaseType.MySql` |

### New feature - `Multiple` and `MultipleAsync` now support reader callbacks

This was previously missing. Now, it is possible to use reader callbacks with multiple queries:

```csharp
using var multiple = await connection
    .WithReaderCallback(r => r.Ordinal switch
    {
        0 => r.Reader.GetInt32(0) + 1,
        _ => null
    })
    .MultipleAsync(@"
        select 1 as id1, 'foo1' as foo1, 'bar1' as bar1; 
        select 2 as id2, 'foo2' as foo2, 'bar2' as bar2");

var result1 = await multiple.ReadAsync<Record1>().SingleAsync();

var next1 = multiple.Next(); // true

var result2 = await multiple.ReadAsync<Record2>().SingleAsync();

var next2 = multiple.Next();  // false

// ...
```

### New feature - anonymous `Read` and `ReadAsync` overloads for anonymous types added to `Multiple` and `MultipleAsync`

This was previously missing. Now, it is possible to map anonymous types with multiple queries:

```csharp
using var multiple = connection.Multiple(@"
    select * from (
        values 
        (1, 'foo1'),
        (2, 'foo2'),
        (3, 'foo3')
    ) t(first, bar);

    select * from (
        values 
        ('1977-05-19'::date, true, null),
        ('1978-05-19'::date, false, 'bar2'),
        (null::date, null, 'bar3')
    ) t(day, bool, s)
");

var first = multiple.Read(new
{
    first = default(int),
    bar = default(string)
}).ToList();

multiple.Next();

var second = multiple.Read(new
{
    day = default(DateTime?),
    @bool = default(bool?),
    s = default(string),
}).ToList();
```

### Internal improvements 

- Removed some unused leftovers from before version 4.0.0 on methods `Execute` and `ExecuteAsync` which were still using tuple parameters, although they are not supported since 4.0.0.
- Changed internal instance methods scope modifiers from private to protected to improve future extendibility.
- Removed unnecessary type caching. They give no performance gains and are using memory. No need to have them.
- Remove unnecessary and obsolete instance managament mechanism: improves memory and perfomances a bit.
- Many other improvements for internal structure that allow for future extendibility.


