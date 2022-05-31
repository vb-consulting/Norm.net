# Changelog

## [5.0.0](https://github.com/vb-consulting/Norm.net/tree/5.0.0) (2022-05-24)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.3.0...5.0.0)

### New feature - comment headers

From version 5, Norm can create SQL comment headers on all or some SQL commands. 

Those comment headers than can include following:

- Custom user comment.
- Parameters names, types and values.
- Caller info such as calling method name, source code file name and line number.
- Command attributes such as database provider, command type (text or stored procedure) and command timeout.
- Execution timestamp.

When enabled, this feature can help with diagnostics and monitoring trough:

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

These header values can be enabled for individual command and globally for all commands.

Quick example of enabling logging of caller info and parameter values for all command in application:

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

This allows you to log all your commands with all relevant information needed for dubugging, such as parameter values and also with source code information that allows you to locate commands in your source code.

And since it is included in command comment header, they are visible on a database server as well trough monitoring tools.

> Note: Caller info is generated at the compile time by the compiler, so there is no perfomany penalty for including such information.

To support this feauture, number of new extensions are added and support for global configuration.

More details bellow:

### New method extensions

Important note:

Same `Norm` method extensions are available as `DbConnection` extnesions methods and and Norm instance methods.

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

Executes supplied action with constructed `DbCommand` as single parameter just before the actual execution.

This exposes created `DbCommand` object and allows for eventual changes to be made to the `DbCommand` object such as changes in a command text, parameters collection, etc, depending on a provider.

There is also a global version of this method that is commonly used for logging tasks.

#### `WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)`

Executes reader callback function for each value just before assigning a mapper results.

Input parameter is a named tuple of three values:

1) `Name` - field name from your command results.
2) `Ordinal` - ordinal position of the field from your command results.
3) `Reader` - `DbDataReader` object used for reading results.

This allows suppliying custom results to mapperby retruning values:

- If this function callback returns non-null value, that value is mapped.
- If this function callback returns null value, default value is mapped.
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

In versions 4 and lower, this callback function was supplied as an optional parameter to `Read` method extensions.

```csharp
connection.Read("command", r => { /*...*/});
```

From version 5, `WithReaderCallback` extension method is used to supply this callback to the next `Read` command.

#### `WithComment(string comment)`
#### `WithCommentParameters()`
#### `WithCommentCallerInfo()`
#### `WithCommentHeader(string comment = null, bool includeCommandAttributes = true, bool includeParameters = true, bool includeCallerInfo = true, bool includeTimestamp = false)`

### New feature - anonymous `Read` and `ReadAsync` overloads for anonymous types added to Multiple and MultipleAsync

This was previously missing. Now, it is possible to map anonymous types from multiple queries:

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



