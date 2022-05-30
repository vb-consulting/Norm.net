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

```
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
-- at ChangeLogExample1 in /path/to/source/ChangeLogExample.cs 28
-- @foo nvarchar = "foo value"
-- @bar nvarchar = "bar value"
select @foo, @bar
```

Or, on PostgreSQL:

```
-- My foo bar query
-- Npgsql Text Command. Timeout: 30 seconds.
-- at ChangeLogExample1 in /path/to/source/ChangeLogExample.cs 28
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

### New extensions:

#### `WithParameters(params object[] parameters)`

Sets parameters for the next command.

This is a breaking change:

In versions 4 and lower, parameters  

#### `WithCommandCallback(Action<DbCommand> dbCommandCallback)`
#### `WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)`
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



