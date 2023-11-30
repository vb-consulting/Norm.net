# Version history and release notes

## [5.2.5](https://github.com/vb-consulting/Norm.net/tree/5.2.5) (2022-10-06)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.4...5.2.5)

Very small change that affects how caller info is formatted.

Instead of this `at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs 28`, hashtag was added now we have this `at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs#28`.

This is because, when caller info is displayed with a hashtag in Visual Studio Code, `Ctrl+Click` will navigate editor to that exact line.

## [5.2.4](https://github.com/vb-consulting/Norm.net/tree/5.2.4) (2022-10-04)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.3...5.2.4)

Fix anonymous parameter parser to allow for complex values.

## [5.2.3](https://github.com/vb-consulting/Norm.net/tree/5.2.3) (2022-07-08)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.2...5.2.3)

Comment headers are now always multi-line SQL comments.

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
/*
My foo bar query
Sql Text Command. Timeout: 30 seconds.
at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs 28
@foo nvarchar = "foo value"
@bar nvarchar = "bar value"
*/
select @foo, @bar
```

Single line comments, when showing parameter values that contain multiple lines could break comments and cause runtime errors.

This solves this problem.

Additionally, parameter values are parsed to replace comment start and end sequences (`/*` and `*/`) with `??`.

Comment header parameters now support PostgreSQL native positional parameters.

PostgreSQL array parameter output to comment header is also supported.

## [5.2.2](https://github.com/vb-consulting/Norm.net/tree/5.2.2) (2022-07-05)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.1...5.2.2)

- Smaller internal improvements and fixed async disposal for the Multiple Batch class implementation.

- New performance tests. They are showing that Norm is now at similar levels as Dapper. This might be due to the new feature implementations that have been added since version 4.3.0.

## [5.2.1](https://github.com/vb-consulting/Norm.net/tree/5.2.1) (2022-06-26)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.0...5.2.1)

### Support for specific parameter types when using PostgreSQL native positional parameters

In previous version with could set PostgreSQL positional paramters like this:

```csharp
var p1 = "_b_";
var p2 = "a__";

var result = connection
    .WithParameters(p1, p2)
    .Read<string>(@"select * from 
        (values ('abc'), ('bcd')) t (t1) 
        where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
    .Single();

Assert.Equal("abc", result);
```

However, sometimes when the parameter value is null, and the parameter type has not been set explicitly and PostgreSQL could not determine the data type of that parameter.

In fact, this would throw an exception:

```csharp
var p1 = "_b_";
var p2 = (string)null;

var result = connection
    .WithParameters(p1, p2)
    .Read<string>(@"select * from 
        (values ('abc'), ('bcd')) t (t1) 
        where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
    .Single();

Unhandled exception. Npgsql.PostgresException (0x80004005): 42P08: could not determine data type of parameter $1
```

From this version 5.2.1, positional parameter can be value tuple where first tuple is actual value and second value is database type:

```csharp
var p1 = "_b_";
var p2 = ((string)null, DbType.AnsiString);

var result = connection
    .WithParameters(p1, p2)
    .Read<string>(@"select * from 
        (values ('abc'), ('bcd')) t (t1) 
        where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
    .Single();
```

This will set the parameter type correctly and solve the error.

It is also possible to use native parameter type, for example:

```csharp
var p1 = "_b_";
var p2 = ((string)null, NpgsqlDbType.Text);

var result = connection
    .WithParameters(p1, p2)
    .Read<string>(@"select * from 
        (values ('abc'), ('bcd')) t (t1) 
        where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
    .Single();
```

Note that this approach will also save the underlying database driver of mapping correct types, and give another performance increase.

## [5.2.0](https://github.com/vb-consulting/Norm.net/tree/5.2.0) (2022-06-23)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.1.0...5.2.0)

### Support for PostgreSQL native positional parameters - Npgsql (PostgreSQL) 6+ only

You can now use PostgreSQL **native positional parameters** (or any other database provider that supports unnamed positional parameters).

PostgreSQL example by using [standard dollar notation](https://www.postgresql.org/docs/8.1/sql-expressions.html#AEN1626) to set parameters positions:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
    .Single();

Assert.Equal("str", s); // true
Assert.Equal(999, i); // true
Assert.True(b); // true
Assert.Equal(new DateTime(1977, 5, 19), d); // true
Assert.Null(@null); // true
```

Here the `$1` references the value of the first function argument, `$2` for the second, and so on.

There are two conditions to enable using PostgreSQL native positional parameters:

1) Connection must be an instance of `NpgsqlConnection` from `Npgsql` version 6 or higher.
2) All supplied parameters must be a simple type (`int`, `string`, `DateTime`, `bool`, etc). Using class or record instance values of complex types will force using named parameters again.

Mixing named and positional parameters in a query like this `SELECT * FROM employees WHERE first_name = $1 AND age = @age` will trow `System.NotSupportedException : Mixing named and positional parameters isn't supported` error.

Other then setting simple values, you can also use native `NpgsqlParameter` objects by setting only values, like this:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new NpgsqlParameter { Value = "str" },
                    new NpgsqlParameter { Value = 999 },
                    new NpgsqlParameter { Value = true },
                    new NpgsqlParameter { Value = new DateTime(1977, 5, 19) },
                    new NpgsqlParameter { Value = DBNull.Value })
    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
    .Single();

Assert.Equal("str", s); // true
Assert.Equal(999, i); // true
Assert.True(b); // true
Assert.Equal(new DateTime(1977, 5, 19), d); // true
Assert.Null(@null); // true
```

****Important:****

Using PostgreSQL native positional parameters will disable SQL rewriting by the `Npgsql` drivers for that command. 

That has a positive performance impact since the driver doesn't have to parse the command anymore. 

However, sending multiple commands in one command string separated by a semicolon will be disabled and will throw `Npgsql.PostgresException : 42601: cannot insert multiple commands into a prepared statement`.

See following [article](https://www.roji.org/parameters-batching-and-sql-rewriting).

### NpgsqlEnableSqlRewriting global setting - Npgsql (PostgreSQL) 6+ only

You can force all `Npgsql` commands to "raw" mode that disables `Npgsql` parser for all `Npgsql` commands globally.

Set global `NpgsqlEnableSqlRewriting` settings to false:

```csharp
NormOptions.Configure(options => options.NpgsqlEnableSqlRewriting = false);
```

Notes:

- This option only has an impact if it is set before any `Npgsql` command is executed. `NpgsqlCommand` caches internally this value.

- Default value is `null` which doesn't change anything, uses `Npgsql` default which is false.

- Only available for `Npgsql` version 6 or higher.

- When in "raw" mode (SQL rewriting disabled) following features are disabled:
   - Named parameters are disabled, and positional parameters with $ syntax must be used always (throws `System.NotSupportedException : Mixing named and positional parameters isn't supported`).
   - Multiple commands in one command string separated by a semicolon (throws `Npgsql.PostgresException : 42601: cannot insert multiple commands into a prepared statement`)

### WithUnknownResultType(params bool[] list) connection extension method - Npgsql (PostgreSQL) 6+ only

This new method extension will tell the underlying `Npgsql` command results parser not to map command results and return raw strings only.

For example, results of this PostgreSQL query are all strings:

```csharp
var (@int, @bool, @date, @num, @json) = connection
    .WithUnknownResultType()
    .Read<string, string, string, string, string>(
        "select 1::int, true::bool, '1977-05-19'::date, 3.14::numeric, '{\"x\": \"y\"}'::json")
    .Single();

Assert.Equal("1", @int);
Assert.Equal("t", @bool);
Assert.Equal("1977-05-19", @date);
Assert.Equal("3.14", @num);
Assert.Equal("{\"x\": \"y\"}", @json);
```

In this case, the results parser didn't even bother trying to map and just returned the raw string.

This, of course, has certain performance benefits. Sometimes, results don't have to be mapped and they can be passed as-is.

But sometimes, this can be useful when mapping rare and exotic PostgreSQL types which are not supported by the official `Npgsql` drivers.

PostgreSQL is unkown to have a variety of different and custom types and not all may be supported by the `Npgsql`. See this [FAQ](https://www.npgsql.org/doc/faq.html#a-nameunknowntypei-get-an-exception-the-field-field1-has-a-type-currently-unknown-to-npgsql-oid-xxxxx-you-can-retrieve-it-as-a-string-by-marking-it-as-unknowna)

If you supply array parameters for this method, you can tell the `Npgsql` that unknown types are only at certain position, by setting `true` at same paramater position. For example:

```csharp
var (@int, @bool, @date, @num, @json) = connection
    .WithUnknownResultType(true, false, true, false, true)
    .Read<string, bool, string, decimal, string>(sql
        "select 1::int, true::bool, '1977-05-19'::date, 3.14::numeric, '{\"x\": \"y\"}'::json")
    .Single();

Assert.Equal("1", @int);
Assert.Equal(true, @bool);
Assert.Equal("1977-05-19", @date);
Assert.Equal(3.14m, @num);
Assert.Equal("{\"x\": \"y\"}", @json);
```

So, in this case only the first, third, and fourth results are unknown and therefore returned as raw strings.

If simply called this method without any parameters like this `.WithUnknownResultType()` - it would set all results to unknown raw mode.

## [5.1.0](https://github.com/vb-consulting/Norm.net/tree/5.1.0) (2022-06-20)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.0.1...5.1.0)

### WithCommandBehavior(CommandBehavior) connection extension method

New extensions method that sets the command behavior of the data read for the next command.

You can combine `CommandBehavior` values as flags. For example:

```csharp
connection
    .WithCommandBehavior(CommandBehavior.CloseConnection | CommandBehavior.SingleRow)
    .Read(...);
```

Here are all command behaviors available:

```csharp
namespace System.Data
{
    //
    // Summary:
    //     Provides a description of the results of the query and its effect on the database.
    [Flags]
    public enum CommandBehavior
    {
        //
        // Summary:
        //     The query may return multiple result sets. Execution of the query may affect
        //     the database state. Default sets no System.Data.CommandBehavior flags, so calling
        //     ExecuteReader(CommandBehavior.Default) is functionally equivalent to calling
        //     ExecuteReader().
        Default = 0,
        //
        // Summary:
        //     The query returns a single result set.
        SingleResult = 1,
        //
        // Summary:
        //     The query returns column information only. When using System.Data.CommandBehavior.SchemaOnly,
        //     the .NET Framework Data Provider for SQL Server precedes the statement being
        //     executed with SET FMTONLY ON.
        SchemaOnly = 2,
        //
        // Summary:
        //     The query returns column and primary key information. The provider appends extra
        //     columns to the result set for existing primary key and timestamp columns.
        KeyInfo = 4,
        //
        // Summary:
        //     The query is expected to return a single row of the first result set. Execution
        //     of the query may affect the database state. Some .NET Framework data providers
        //     may, but are not required to, use this information to optimize the performance
        //     of the command. When you specify System.Data.CommandBehavior.SingleRow with the
        //     System.Data.OleDb.OleDbCommand.ExecuteReader method of the System.Data.OleDb.OleDbCommand
        //     object, the .NET Framework Data Provider for OLE DB performs binding using the
        //     OLE DB IRow interface if it is available. Otherwise, it uses the IRowset interface.
        //     If your SQL statement is expected to return only a single row, specifying System.Data.CommandBehavior.SingleRow
        //     can also improve application performance. It is possible to specify SingleRow
        //     when executing queries that are expected to return multiple result sets. In that
        //     case, where both a multi-result set SQL query and single row are specified, the
        //     result returned will contain only the first row of the first result set. The
        //     other result sets of the query will not be returned.
        SingleRow = 8,
        //
        // Summary:
        //     Provides a way for the DataReader to handle rows that contain columns with large
        //     binary values. Rather than loading the entire row, SequentialAccess enables the
        //     DataReader to load data as a stream. You can then use the GetBytes or GetChars
        //     method to specify a byte location to start the read operation, and a limited
        //     buffer size for the data being returned.
        SequentialAccess = 16,
        //
        // Summary:
        //     When the command is executed, the associated Connection object is closed when
        //     the associated DataReader object is closed.
        CloseConnection = 32
    }
}
```

### Parameters can be set again in Rea` and Execute extensions methods

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

### `WithTransaction(DbTransaction)` connection extension method

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

### WithTimeout connection extension method

This is equivalent to the `Timeout` connection extension, added only for the naming consistency ("with" prefix).

It just sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error.

### MapPrivateSetters global option

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

### Changed member scope from internal to allow greater extendibility

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

#### WithParameters(params object[] parameters)

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

#### WithCommandCallback(Action<DbCommand> dbCommandCallback)

Executes supplied action with constructed `DbCommand` as a single parameter just before the actual execution.

This exposes created `DbCommand` object and allows for eventual changes to be made to the `DbCommand` object such as changes in a command text, parameters collection, etc, depending on a provider.

There is also a global version of this method that is commonly used for logging tasks.

#### WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)

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

#### WithComment(string comment)

Adds a custom comment header to the next command.

#### WithCommentParameters()

Adds a comment header with command parameters described with names, database types, and values to the next command.

#### WithCommentCallerInfo()

Adds a comment header with caller info data (calling method names, source code file, and line number) to the next command.

#### WithCommentHeader(string comment = null, bool includeCommandAttributes = true, bool includeParameters = true, bool includeCallerInfo = true, bool includeTimestamp = false)

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

### New feature - Multiple and MultipleAsync now support reader callbacks

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

### New feature - anonymous Read and ReadAsync overloads for anonymous types added to Multiple and MultipleAsync

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



## [4.3.0](https://github.com/vb-consulting/Norm.net/tree/4.3.0) (2022-05-07)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.2.0...4.3.0)

## New feature: mapping to anonymous types

From version 4.3.0., Norm allows **mapping to anonymous type instances** - by supplying anonymous type **blueprint instance** as the first parameter.

Since anonymous types cannot be declared and only created - that blueprint instance will be used to create new instances in a query result.

For example, you can declare new anonymous types with default values:


```csharp
var result = connection.Read(new 
{ 
    id = default(int), 
    foo = default(string), 
    date = default(DateTime) 
}, @"select * from (values
    (1, 'foo1', cast('2022-01-01' as date)), 
    (2, 'foo2', cast('2022-01-10' as date)), 
    (3, 'foo3', cast('2022-01-20' as date))
) t(id, foo, date)").ToList();
```

This will yield a three-element list of anonymous types:

```csharp
Assert.Equal(3, result.Count); // true

Assert.Equal(1, result[0].id); // true
Assert.Equal("foo1", result[0].foo); // true
Assert.Equal(new DateTime(2022, 1, 1), result[0].date); // true

Assert.Equal(2, result[1].id); // true
Assert.Equal("foo2", result[1].foo); // true
Assert.Equal(new DateTime(2022, 1, 10), result[1].date); // true

Assert.Equal(3, result[2].id); // true
Assert.Equal("foo3", result[2].foo); // true
Assert.Equal(new DateTime(2022, 1, 20), result[2].date); // true
```

Declaration of this anonymous type is provided by a blueprint instance with default values in the first parameter:

```csharp
new 
{ 
    id = default(int), 
    foo = default(string), 
    date = default(DateTime) 
};
```

That is one way to explicitly declare desired types in anonymous types. 

Of course, you can also explicitly declare values of certain types, which is a bit shorter and a bit less readable:

```csharp
new 
{ 
    id = 0, 
    foo = "", 
    date = DateTime.Now 
};
```

In this case, same as in the previous case, those values are discarded and instances populated by database values are returned.

### Remarks on using anonymous types

> - Fields are **matched by name**, not by position.

> - Matching is **case insensitive.**

> - **Snake case names are supported** and matched with non cnake case names.

> - `@` characters are ignored.

> - **All available types** returned by the `GetValue` reader method are supported including special types like `guid`, `timespan`, etc for example.

> - **PostgreSQL arrays** are supported.

> - Mapping **to enums from numbers** is supported.

> - Mapping **to enums from strings** is supported.

> - **Enum PostgreSQL arrays** are supported (numbers and strings).

> - Nullable arrays are not supported. Mapping nullable arrays can be achieved with reader callbacks.

[See more examples in unit tests.](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/ReadAnonymousUnitTests.cs)

## Other imrpovements

Slight improvements and opštimizations, see [full changelog](https://github.com/vb-consulting/Norm.net/compare/4.2.0...4.3.0) for details.

## [4.2.0](https://github.com/vb-consulting/Norm.net/tree/4.2.0) (2022-04-07)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.1.0...4.2.0)

## New feature: Collection parameters support for non PostgreSQL connections

From version 4.2.0 Norm can parse collection-type parameters for non PostgreSQL connections.

PostgreSQL supports array types natively so you can pass an array or a list parameter and it will be mapped to the PostgreSQL array type parameter.

For non-PostgreSQL connections, any collection-type parameter (a list or an array for example) will be parsed to a sequence of new parameters.

For example, following statements on SQL Server:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@list)", new[] { 1, 2, 3 });
```

Normally, this wouldn't work because SQL Server does not support those parameter types. 

But, since this version, Norm will parse this expression to three parameters instead of one:

- `@__list0` with value `1`
- `@__list1` with value `2`
- `@__list2` with value `3`

And SQL command text will replace the original parameter placeholder `@list` with CSV list of ne parameters: `SELECT * FROM Table WHERE id IN (@__list0, @__list1, @__list2)`.


So basically, this:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@list)", new[] { 1, 2, 3 });
```

is equvivalent to this:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@__list0, @__list1, @__list2)", 1, 2, 3);
```

It also works normally with named parameters in an anonymous class:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@list)", new 
{
    list = new[] { 1, 2, 3 }
});
```

Which is equvivalent to this:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@__list0, @__list1, @__list2)", new 
{
    __list0 = 1,
    __list1 = 2,
    __list2 = 3
});
```

Any type of parameter that implements `ICollection` will work this way on non-PostgreSQL connections.

## Internal improvements

See changelog for details.

## [4.1.0](https://github.com/vb-consulting/Norm.net/tree/4.1.0) (2022-02-23)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.0.0...4.1.0)

## New feature: raw modifier for string interpolation overloads

When using following extensions:

- `ReadFormat`
- `ReadFormatAsync`
- `ExecuteFormat`
- `ExecuteFormatAsync`
- `MultipleFormat`
- `MultipleFormatAsync`

To set query parameters via string interpolation formats, you can now add `raw` modifier to skip parameter creation and use value "as is".

This is useful when using string interpolation to build a query and you still want to pass the parameter creation.

For example:

```csharp
var p = "xyz";
var table = "my_table";
var where = "where 1=1";
connection.ReadFormat<T>($"select * from {table:raw} {where:raw} and id = {p}");
```

Variables `table` and `where` will not create query parameter and will be parsed "as is".

## [4.0.0](https://github.com/vb-consulting/Norm.net/tree/4.0.0) (2022-02-13)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/3.3.13...4.0.0)

## **New feature - reader callbacks**

All read extension methods on your connection object (`Read` and `ReadAsync`) now have one additional overload that allows you to pass in the **lambda function with direct low-level access to the database reader.**

Lambda function with direct low-level access to the database reader has the following signature:

```csharp
Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback
```

New methods overloads look like this:

```csharp
public static IEnumerable<T> Read<T>(this DbConnection connection, 
    string command, 
    Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback);

public static IEnumerable<T> ReadFormat<T>(this DbConnection connection, 
    FormattableString command,
    Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback);

public static IEnumerable<T> Read<T>(this DbConnection connection, string command,
    Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
    params object[] parameters)
```

Same overloads also exist for all 12 generic versions of `Read` and `ReadAsync` extensions.

This lambda function receives a single parameter, a tuple with three values: `(string Name, int Ordinal, DbDataReader Reader)`, where:

- `string Name` is the name of the database field currently being mapped to a value.
- `int Ordinal` is the ordinal position number, starting from zero, of the database field currently being mapped to a value.
- `DbDataReader Reader` is the low-level access to the database reader being used to read the current row.

The expected result of this reader lambda function is an object of any type that represents a **new value that will be mapped** to the results with the following rules:

- Return any value that you wish to map to result for that field.
- Return `null` to use the default, normal mapping.
- Return `DBNull.Value` to set `null` value.

This allows for the elegant use case of the C# 8 switch expressions.

### Examples

Simple example, a query that returns three records of three integers fields (`a`, `b`, `c`) - from 1 to 3:

```csharp
var query = "select * from (values (1, 1, 1), (2, 2, 2), (3, 3, 3)) t(a, b, c)";
```

Will return following results:

| a | b | c |
| - | - | - |
| 1 | 1 | 1 |
| 2 | 2 | 2 |
| 3 | 3 | 3 |

- If we want to add 1 to the first field, for example, we can add an expression like this:

```csharp
var array = connection.Read<int, int, int>(query, r => r.Ordinal switch
{
    0 => r.Reader.GetInt32(0) + 1,  // add 1 to the first field with ordinal 0, mapped to the first value
    _ => null                       // for all other fields, use default mapping
}).ToArray();
```

This example will produce the following array of `int` tuples:

| Item1 | Item2 | Item3 |
| - | - | - |
| 2 | 1 | 1 |
| 3 | 2 | 2 |
| 4 | 3 | 3 |

Same could be achieved if we use switch by the field name:

```csharp
var array = connection.Read<int, int, int>(query, r => r.Name switch
{
    "a" => r.Reader.GetInt32(0) + 1,    // add 1 to the first field with name "a", mapped to the first value
    _ => null                           // for all other fields, use default mapping
}).ToArray();
```

Same logic applies to named tuples mapping:

```csharp
var array = connection.Read<(int a, int b, int c)>(query, r => r.Ordinal switch
{
    0 => r.Reader.GetInt32(0) + 1,  // add 1 to the first field with ordinal 0, mapped to the first tuple named "a"
    _ => null                       // for all other fields, use default mapping
}).ToArray();
```

or

```csharp
var array = connection.Read<(int a, int b, int c)>(query, r => r.Name switch
{
    "a" => r.Reader.GetInt32(0) + 1,  // add 1 to the first field with name "a", mapped to the first tuple named "a"
    _ => null                         // for all other fields, use default mapping
}).ToArray();
```

Produces the following array of named `int` tuples:

| a | b | c |
| - | - | - |
| 2 | 1 | 1 |
| 3 | 2 | 2 |
| 4 | 3 | 3 |

**Important: simple values, tuples, and named tuples are still mapped by the position.**

The same technique applies also to mapping to instances of classes and records. 
Only, in this case - mapping by field name, not by position will be used. Example:


```csharp
class TestClass
{
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
}
```

```csharp
var array = connection.Read<TestClass>(query, r => r.Ordinal switch
{
    0 => r.Reader.GetInt32(0) + 1,  // add 1 to the first field. First field has name "a" and it will be mapped to property "A".
    _ => null                       // for all other fields, use default mapping
}).ToArray();
```

or

```csharp
var array = connection.Read<TestClass>(query, r => r.Name switch
{
    "a" => r.Reader.GetInt32(0) + 1,  // add 1 to the field name "a", mapped to to property "A" by name
    _ => null                         // for all other fields, use default mapping
}).ToArray();
```

This will produce an array of the `TestClass` instances with the following properties

| A | B | C |
| - | - | - |
| 2 | 1 | 1 |
| 3 | 2 | 2 |
| 4 | 3 | 3 |

And now, of course, you can utilize a pattern matching mechanism in switch expressions for C# 9:

```csharp
var array = connection.Read<TestClass>(query, r => r switch
{
    {Ordinal: 0} => r.Reader.GetInt32(0) + 1,   // add 1 to the field at the first position adn with name "a", mapped to to property "A" by name
    _ => null                                   // for all other fields, use default mapping
}).ToArray();
```

Or, for both, name and oridnal number at the same time:

```csharp
var array = connection.Read<TestClass>(query, r => r switch
{
    {Ordinal: 0, Name: "a"} => r.Reader.GetInt32(0) + 1,    // add 1 to the field name "a", mapped to to property "A" by name
    _ => null                                               // for all other fields, use default mapping
}).ToArray();
```

### Perfomance impacts

There are no performance impacts when using this feature to speak of.

These new overloads with lambda callbacks are in the new [perfomance benchmarks](https://github.com/vb-consulting/Norm.net/blob/master/PERFOMANCE-TESTS.md)
and the difference is negligible is still slightly better than the `Dapper` without any callback.

However, it's worth noting that performances will always depend on what exactly you are doing within that function, and of course, the query itself.

### Justification and use cases

**1) Database types that are hard or impossible to map to the CLR types.**

For example, **PostgreSQL** can return a nullable array that should map to a nullable array of enums.
Enums require special care since the database usually returns an integer or text that should be parsed to enums.

This functionality of mapping enum arrays is available in Norm default mapping, however, nullable enum arrays had proved to be especially difficult if not outright impossible when mapping to non-instanced types for example.

In edge cases like this, a reader callback function that can handle special types can be very useful.

**2) Special non-standard database types.**

For example, PostgreSQL can define custom database types by using extensions. Most notably is PostGIS extension that defines geometric types such as point, line, polygon, geometry collections, etc.

For these complex types, a reader callback function can be used to map to the custom types.

**3) Complex mappings**

Reader callback can return any type of object which allows you to create mappings of any complexity, depending on your needs, not on the data access capabilities.

## **Breaking Change: parameter handlings**

Parameter handling has been simplified.

**Breaking change:**

**All method overloads that supplied query parameters via named and value tuple or name, value, and type tuple are now removed.**

Instead, instance (anonymous or otherwise) parameters are now used to supply named parameters with or without a specific type.

For example, **this call will no longer work:**

```csharp
connection.Read<T>("select * from table where id = @myParam", ("myParam", myParamValue));
```

To define named paramater, instances are used:

```csharp
connection.Read<T>("select * from table where id = @myParam", new {myParam = myParamValue});
```

Note: non-anonymous instances will also work.

The first version was removed because before there were two different ways to define named parameters and it was confusing and required a lot of unnecessary method overloads.

Removing those overloads has made the library much lighter.

Also, it is easier now to define named parameters this way, especially when your variable is named same as parameter, for example:

```csharp
var myParam = 123;
connection.Read<T>("select * from table where id = @myParam", new {myParam});
```

Now, it is also possible to supply specific database type to named paramter, without having to create specific `DbParamater` type by using tuple (value and type) as parameter value:

```csharp
connection.Read<T>("select * from table where id = @myParam", new {myParam = (myParamValue, DbType.Int)});
```

Setting specific parameter type will also work with specific provider defined types:

```csharp
connection.Read<T>("select * from table where id = @myParam", new {myParam = (myParamValue, NpgsqlDbType.Integer)}); // PostgreSQL Integer type
```

This, of course, allows easy access to more "exotic" PostgreSQL parameter types, such as JSON and arrays for example:

```csharp
var test = connection.Read<string>("select @p->>'test'", 
                new
                {
                    p = ("{\"test\": \"value\"}", NpgsqlDbType.Json)
                })
                .Single();
// test == "value"

var array = connection.Read<int>("select unnest(@p)", 
                new
                {
                    p = (new List<int> { 1, 2, 3 }, NpgsqlDbType.Array | NpgsqlDbType.Integer)
                }).ToArray();

// test == int[]{1,2,3}
```

But, of course, `DbParameter` instances can be used as well:

```csharp
var test = connection.Read<string>("select @p->>'test'", 
                new
                {
                    p = new NpgsqlParameter("p", "{\"test\": \"value\"}"){ NpgsqlDbType = NpgsqlDbType.Json }
                })
                .Single();
// test == "value"

var array = connection.Read<int>("select unnest(@p)", 
                new
                {
                    p = new NpgsqlParameter("p", new List<int> { 1, 2, 3 }){ NpgsqlDbType =  NpgsqlDbType.Array | NpgsqlDbType.Integer}
                }).ToArray();

// test == int[]{1,2,3}
```

Supplying parameters in that way is a bit confusing because in this example above, a parameter is named twice, first as instance property name and the second time as actual parameter name, and only the second will be used.

So, it might be better to use the old way of declare parameters from `DbParameter` instances:

```csharp
var p1 = new NpgsqlParameter("p", "{\"test\": \"value\"}"){ NpgsqlDbType = NpgsqlDbType.Json };
var test = connection.Read<string>("select @p->>'test'", p1).Single();
// test == "value"

var p2 = new NpgsqlParameter("p", new List<int> { 1, 2, 3 }){ NpgsqlDbType =  NpgsqlDbType.Array | NpgsqlDbType.Integer};
var array = connection.Read<int>("select unnest(@p)", p2).ToArray();

// test == int[]{1,2,3}
```

Using `DbParameter` instances like this allows also for changing parameter direction to `Out` and `InOut` parameter directions.

##  **Fixed bug/missing feature: arrays mapping for named tuples**

Now it's possible to map arrays to named tuples for database provedires that support arrays. Example:


```csharp
var result = connection.Read<(int[] i, string[] s)>(@"
                select array_agg(i), array_agg(s)
                from (
                values 
                    (1, 'foo1'),
                    (2, 'foo2'),
                    (3, 'foo3')
                ) t(i, s)").ToArray();

Assert.Single(result);

Assert.Equal(1, result[0].i[0]);
Assert.Equal(2, result[0].i[1]);
Assert.Equal(3, result[0].i[2]);

Assert.Equal("foo1", result[0].s[0]);
Assert.Equal("foo2", result[0].s[1]);
Assert.Equal("foo3", result[0].s[2]);
```

This was missing from previous versions.

Maping arrays for simple values was always possible:

```csharp
var result = connection.Read<int[]>(@"
    select array_agg(e) 
    from ( values (1), (2), (3) ) t(e)
").ToArray();

Assert.Equal(1, result[0][0]);
Assert.Equal(2, result[0][1]);
Assert.Equal(3, result[0][2]);
```

Or, mapping arrays to nullable simple value arrays:


```csharp
var result = connection.Read<int?[]>(@"
    select array_agg(e) 
    from ( values (1), (null), (3) ) t(e)
").ToArray();

Assert.Equal(1, result[0][0]);
Assert.Null(result[1]);
Assert.Equal(3, result[0][2]);
```

**Important:**

**Mapping nullable arrays (arrays that contain null values) to named tuples or instance properties will not work.**

Following mappings are still unavailable out os the box:

```csharp
var query = @"
    select array_agg(i), array_agg(s) from (values 
        (1, 'foo1'),
        (null, 'foo2'),
        (2, null)
    ) t(i, s)
";

class SomeClass
{ 
    public int?[] Ints { get; set; }
    public string[] Strings { get; set; }
}

//
// These calls won't work because of int?[] mappings on named tuples and instance property
//
var result1 = connection.Read<(int?[] Ints, string[] Strings)>(query);
var result2 = connection.Read<SomeClass>(query);
```

However, new feature with reader callback can be used to solve this mapping problem:

```csharp
var result1 = connection.Read<(int?[] Ints, string[] Strings)>(query r => r.Ordinal switch
{
    0 => r.Reader.GetFieldValue<int?[]>(0),
    _ => null
});
```

##  **Mapping to enum types improvements**

Mapping to enums is a crucial task for any database mapper, but it requires special treatment.

On the database side, enums can be either text or an integer and to map them to enum types requires special parsing.

In previous versions mapping to enums on instance properties from the database, text was added. 

This version expanded enum mapping greatly. 

Now, it is possible to:

- Map from int value to enum for instance properties.
- Map from nullable int value to nullable enum for instance properties.
- Map from string array value to enum array or list for instance properties.
- Map from int array value to enum array or list for instance properties.

- Map from string value to enum for simple values.
- Map from int value to enum for simple values.
- Map from nullable int value to nullable enum for simple values.
- Map from string array value to enum array for simple values.
- Map from int array value to enum array for simple values.

However, the following mappings are still not available.

- Any array containing nulls to nullable enum for instance properties.
- Any enum mapping for named tuples.

Implementation was too difficult and costly. 
To successfully overcome these limitations, a new feature with a reader callback can be used.

Here is the full feature grid for enum mappings:

| | instance mapping `Read<ClassOrRecordType1, ClassOrRecordType2, ClassOrRecordType3>` | simple value mapping `Read<int, string, DateTime>` | named tuple mapping `Read<(int Field1, string Field3, DateTime Field3)>` |
| - | - | - | - |
| text → enum | YES | YES | NO |
| int → enum | YES | YES | NO |
| text containing nulls → nullable enum | YES | YES | NO |
| int containing nulls → nullable enum | YES | YES | NO |
| text array → enum array | YES | YES | NO |
| int array → enum array | YES | YES | NO |
| text array containing nulls → nullable enum array | NO | NO | NO |
| int array containing nulls → nullable enum array | NO | NO | NO |


To overcome these limitations, use new feature with a reader callback. Here are some examples:

```csharp
var result = connection.Read<(TestEnum Enum1, TestEnum Enum2)>(@"
select *
from (
values 
    ('Value1', 'Value3'),
    ('Value2', 'Value2'),
    ('Value3', 'Value1')
) t(Enum1, Enum2)", r => Enum.Parse<TestEnum>(r.Reader.GetFieldValue<string>(r.Ordinal))).ToArray();
```

```csharp
var result = connection.Read<TestEnum?[]>(@"
select array_agg(e) as MyEnums
from (
values 
    (0),
    (null),
    (2)
) t(e)", r =>
{
    var result = new List<TestEnum?>();
    foreach (var value in r.Reader.GetFieldValue<int?[]>(0))
    {
        if (value is null)
        {
            result.Add(null);
        }
        else
        {
            result.Add((TestEnum?)Enum.ToObject(typeof(TestEnum), value));
        }
    }
    return result.ToArray();
}).ToArray();
```

## **Breaking change: `SqlMapper` global class removed**

`SqlMapper` global class used to be previously used to inject custom mapping to the mapper.

This is no longer necessary since there is a reader callback overload for each `Read` extension. 

See examples above.

## **Timeout extension marked as obsolete**

`Timeout` is extension marked as obsolete, altough it will still work.

Instead, `WithCommandTimeout` which has much more precise and consistent naming should be used.

## **Many other internal improvements**


## 3.3.13

- Skip mappings for virtaul properties when mapping class or records now also applies to any generic type.

## 3.3.12

- Mapping parameters from class instance or anonymous instance will only map paramerts for simple types.

## 3.3.11

- Fix rare SQL Server parsing bug that caused "Norm.NormParametersException : Parameter name "ErrorMessage" appears more than once. Parameter names must be unique.", when mixing parameters and local script variables.

## 3.3.10

- Class instance mapper now supports mapping to enums. Example:

```csharp
public class TestEnumClass 
{ 
    public TestEnum Item1 { get; set; }
    public TestEnum? Item2 { get; set; }
}

using var connection = new NpgsqlConnection(fixture.ConnectionString);

var result = connection.Read<TestEnumClass>(@"
            select *
            from (
            values 
                ('Value1', 'Value1'),
                ('Value2', null),
                ('Value3', 'Value3')
            ) t(Item1, Item2)").ToArray();

Assert.Equal(3, result.Length);
Assert.Equal(TestEnum.Value1, result[0].Item1);
Assert.Equal(TestEnum.Value1, result[0].Item2);
Assert.Equal(TestEnum.Value2, result[1].Item1);
Assert.Null(result[1].Item2);
Assert.Equal(TestEnum.Value3, result[2].Item1);
Assert.Equal(TestEnum.Value3, result[2].Item2);
```
Remarks:

- Mapping to enums from tuples and named tuples is still not supported in this version.
- Database value must be string type (text, char, varchar, etc) to be able to be mapped to enum type.

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
