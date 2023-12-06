---
title: Options
order: 2
nextUrl: /norm.net/docs/reference/parameters/
nextTitle: Parameters
prevUrl: /norm.net/docs/reference/methods/
prevTitle: Methods
---

## Norm Options

- Norm options are set at the program startup by calling `NormOptions.Configure` static method.

- This method has the following signature:

```csharp
public static void Configure(Action<NormOptions> options)
```

- The parameter is a callback action to the **options object** where we can set different options that affect the later behavior.

- Call to `NormOptions.Configure` is **not thread-safe** - it is intended to be called only once on a program startup.

- The list of available options is listed below.

---

### CommandCommentHeader

- Sets the **command comment header** options that will be applied for all commands.

- This is a complex object containing multiple different commands:

```csharp
class CommandCommentHeader
{
    public bool Enabled { get; set; } = false;
    public bool IncludeCommandAttributes { get; set; } = true;
    public bool IncludeCallerInfo { get; set; } = true;
    public bool IncludeParameters { get; set; } = true;
    public string ParametersFormat { get; set; } = "{0} {1} = {2}\n";
    public bool IncludeTimestamp { get; set; } = false;
    public DatabaseType OmitStoredProcCommandCommentHeaderForDbTypes { get; set; } = DatabaseType.Sql | DatabaseType.MySql;
}
```

- Example configuration:

```csharp
NormOptions.Configure(options =>
{
    options.CommandCommentHeader.Enabled = true;
});
```

---

#### CommandCommentHeader.Enabled

- Enables or disables (default) command comment headers.

- The default is false (disabled).

---

#### CommandCommentHeader.IncludeCommandAttributes

- Include command attribute descriptions to command comment headers.

- Command attributes are:
  
  - Database provider (SQL Server, PostgreSQL, etc).
  
  - Command type (text, stored procedure, etc).

  - Command timeout in seconds.

- The Default is true (included).

---

#### CommandCommentHeader.IncludeCallerInfo

- Include caller info to command comment headers.

- Caller information is compiler-generated metadata that includes:

  - **Caller member name**: a method or property name of the caller to the method.

  - **Caller file path**: a full path of the source file that contains the caller at the compile time.

  - **Caller line number**: a line number in the source file at which the method is called at the compile time.

- The Default is true (included).

---

#### CommandCommentHeader.IncludeParameters

- Include parameter names and values to command comment headers.

- The default is true (included).

---

#### CommandCommentHeader.ParametersFormat

- A format string that will be used to format parameters comment in the comment header when `CommandCommentHeader.IncludeParameters` is set to true.

- Default is `{0} {1} = {2}\n`, where:
  
  - Format placeholder `{0}` is the parameter name.
  
  - Format placeholder `{1}` is the parameter type.

  - Format placeholder `{2}` is the parameter value.

---

#### CommandCommentHeader.IncludeTimestamp

- Include the current timestamp to command comment headers.

- The default is false (not included).

---

#### CommandCommentHeader.OmitStoredProcCommandCommentHeaderForDbTypes

- Skip comment headers when the command type is stored procedure for certain database types.

- The default values are `DatabaseType.Sql | DatabaseType.MySql`, which means that comment headers will not be created for SQL Server and MySQL database types when command types are stored procedures.

- Unlike PostgreSQL for example, SQL Server and MySQL will yield an error if we try to add a comment header to stored procedures, so this behavior needs to be restricted to the database type.

---

### CommandTimeout

- Set the command timeout in seconds for all commands created by the Norm.

- For example, set the command timeout out for 60 seconds:

```csharp
NormOptions.Configure(options =>
{
    options.CommandTimeout = 60;
});
```

---

### DbCommandCallback

- Set the **command callback function** that will be executed before every command execution and pass the created [`DbCommand`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand) object as a parameter.

- This option has the following signature:

```csharp
public Action<DbCommand> DbCommandCallback { get; set; } = null;
```

- The default is null (no command callback is used by default).

- This is typically used to enable command logging for the entire application. Example:

```csharp
NormOptions.Configure(options =>
{
    options.DbCommandCallback = command => 
        logger.LogInformation("SQL COMMAND: {0}", command.CommandText)
});
```

- This callback could also be used to manipulate the already constructed and initialized `DbCommand`(https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand) object on a global level. 
  
- For example, another way to set command time out for 60 seconds for all commands:

```csharp
NormOptions.Configure(options =>
{
    options.DbCommandCallback = command =>
        command.CommandTimeout = 60;
});
```

--

### DbReaderCallback

- Sets the **custom database reader callback function** that will be executed on each database reader iteration step for every field - on every database reader operation executed by Norm.

- The custom callback function will be called with one tuple value parameter with three values:
  
   - `string Name` - field name that is being read.

   - `int Ordinal` - ordinal, zero-based position of the field that is being read.

   - `DbDataReader Reader` - [`DbDataReader`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbdatareader) instance.

The custom callback function should return an object value that will be used as a value in further object mapping.

- If the custom callback function returns a `null` value - it will fall back to default behavior (reading and mapping that value).

- Return [`DBNull.Value`](https://learn.microsoft.com/en-us/dotnet/api/system.dbnull) value to map to `null` value.

- This global function callback is used to implement a custom mapping mechanism.

- This option has the following signature:

```csharp
public Func<(string Name, int Ordinal, DbDataReader Reader), object> DbReaderCallback { get; set; } = null;
```

- The default is null (no reader callback is used by default).

- A classic example is mapping from PostgreSQL JSON type to the [`JsonObject`](https://learn.microsoft.com/en-us/dotnet/api/system.json.jsonobject):

```csharp
NormOptions.Configure(options =>
{
    options.DbReaderCallback = arg =>
    {
        // check if the column is of type json
        if (string.Equals(arg.Reader.GetDataTypeName(arg.Ordinal), "json", StringComparison.InvariantCultureIgnoreCase))
        {
            if (arg.Reader.IsDBNull(arg.Ordinal))
            {
                // resolve as null
                return DBNull.Value; 
            }
            // Create and return a JsonObject from the string value
            return JsonNode.Parse(arg.Reader.GetString(arg.Ordinal))?.AsObject();
        }
        // fall-back to default behavior
        return null;
    };
});
```

- We can also bind these callback functions to proper method functions, for example:

```csharp
private static object? MapJsonCallback((string Name, int Ordinal, DbDataReader Reader) arg)
{
    // check if the column is of type json
    if (string.Equals(arg.Reader.GetDataTypeName(arg.Ordinal), "json", StringComparison.InvariantCultureIgnoreCase))
    {
        if (arg.Reader.IsDBNull(arg.Ordinal))
        {
            return DBNull.Value; // resolve as null
        }
        // Create and return a JsonObject from the string value
        return JsonNode.Parse(arg.Reader.GetString(arg.Ordinal))?.AsObject();
    }
    // fall-back to default behavior
    return null;
}
```
```csharp
NormOptions.Configure(options =>
{
    options.DbReaderCallback = MapJsonCallback;
});
```

---

### KeepOriginalNames

- By default, the mapping by name mechanism implemented in this library supports the **snake-case naming** convention.

- This is achieved by automatically removing certain characters from database field names like `_` and `@`.

- By doing this, we can map the database name `my_column` to the C# name `MyColumn`.

- However, if you wanted to map `my_column` to a C# name containing an underscore, like `My_Column`, that would not work.

- Set `KeepOriginalNames` to true to keep the database names unchanged (don't strip any characters). 
  
- Example:

```csharp
NormOptions.Configure(options =>
{
    options.KeepOriginalNames = true;
});
```

- Default is false (`_` and `@` are stripped always).

---

### MapPrivateSetters

- By default, the mapping by name mechanism implemented in this library will map only instance members (fields or properties) that have **public setters** (can only be set publicly).

- Set `MapPrivateSetters` to true to be able to map instance members with non-public (private or protected) setters.

- Example:

```csharp
NormOptions.Configure(options =>
{
    options.MapPrivateSetters = true;
});
```
```csharp
class TestMapPrivateProps
{
    public int PublicInt { get; set; }
    private int PrivateInt { get; set; }
    public int PrivateSetInt { get; private set; }
    protected int ProtectedInt { get; set; }
    public int ProtectedSetInt { get; protected set; }
    public int MissingSetInt { get; }
}
```
```csharp
var result = connection
    .Read<TestMapPrivateProps>(@"select 
        1 as public_int, 
        2 as private_int, 
        3 as private_set_int,
        4 as protected_int,
        5 as protected_set_int,
        6 as missing_set_int")
    .Single();
```

- This example will map value to `PublicInt`, `PrivateInt`, `PrivateSetInt`, `ProtectedInt`, and `ProtectedSetInt` properties. 
  
- Property `MissingSetInt` doesn't have any setter method, and it will not be mapped at all.

- When `MapPrivateSetters` is false (default), only `PublicInt` would have been mapped.

- The default is false (only members with public setters are mapped).

---

### NameParserCallback

- Define a **callback function** that is called in a **database field names parsing phase** and can **replace names** with return values from this callback function.

- This option has the following signature:

```csharp
public Func<(string Name, int Ordinal), string> NameParserCallback { get; set; } = null;
```

- Callback input parameters that contain an original field name and ordinal position, zero-based position in results, and expect a new name string as a result.

- The default is null (no name parser callback is used by default).

- Example:

```csharp
NormOptions.Configure(o =>
{
    //
    // If name starts with "prefix_", remove the first seven characters from the name
    //
    o.NameParserCallback = arg =>
        arg.Name.StartsWith("prefix_") ? arg.Name[7..] : arg.Name;
});
```
```csharp
class NameParserTest
{
    public string? Foo { get; set; }
    public string? Bar { get; set; }
    public string? FooBar { get; set; }
}
```
```csharp
var result = connection
    .Read<NameParserTest>(@"
        select 
            'foo' as prefix_foo, 
            'bar' as prefix_bar, 
            'foobar' as foobar")
    .Single();

Assert.Equal("foo", result.Foo);
Assert.Equal("bar", result.Bar);
Assert.Equal("foobar", result.FooBar);
```

---

### NpgsqlEnableSqlRewriting

> **PostgreSQL only feature**

- This feature requires the `Npgsql` data access provider with version 6 or higher.

- This option only has an impact if it is set before any `Npgsql` command is executed. 

- `NpgsqlCommand` caches this value internally.

- When set to true, it enforces PostgreSQL "raw" mode, and all SQL rewriting is disabled.

- That means that:

    1) Named parameters are disabled, and positional parameters with $ syntax must always be used (throws `System.NotSupportedException: Mixing named and positional parameters isn't supported`).

    2) Multiple commands in one command string separated by a semicolon are disabled (throws `Npgsql.PostgresException : 42601: cannot insert multiple commands into a prepared statement`).

- Setting this option to true and disabling internal rewriting by the data-access provider, we gain performance benefits.

- Example:

```csharp
NormOptions.Configure(options => options.NpgsqlEnableSqlRewriting = false);
```

- The default value is null, which doesn't change anything, uses `Npgsql` default which is false (rewriting not disabled).

---

### NullableInstances

- When this option is true, the instance mapper will **return null** for all instances that have **all properties set to null.**

- Example:

```csharp
NormOptions.Configure(options =>
{
    options.NullableInstances = true;
});
```
```csharp
class TestClass1
{
    public int? Foo1 { get; set; }
    public string? Bar1 { get; set; }
}
```
```csharp
var instance = connection
    .Read<TestClass1?>("select null as foo1, null as bar1")
    .Single();

Assert.Null(instance);
```

- This is particularly useful when mapping to multiple class instances from a query, for example:

```csharp
var (instance1, instance2, instance3) = connection
    .Read<TestClass1?, TestClass2?, TestClass3?>(@"
        select * 
        from table1 
        left outer join table2 using (table1_id)
        left outer join table3 using (table2_id)
        where table1_id = 1")
    .Single();
```

- In this example, if the match doesn't exist for `table2` and `table3`, `instance2` and `instance3` will be null if `NullableInstances` is set to true.

- The default is false (nullable instances are disabled).

---

### Prepared

- Set to true to run all commands in **prepared mode every time** - by calling the [`Prepare()`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand.prepare) method before every execution.

```csharp
NormOptions.Configure(options =>
{
    options.Prepared = true;
});
```

- The default is false (commands are not prepared automatically).

- Note: this feature is useless for SQL Server because SQL Server does this automatically. PostgreSQL does not, but it can be enabled through connection string or calling prepare manually for each command. See [this article](https://www.npgsql.org/doc/prepare.html).

---

### RawInterpolationParameterEscape

- Methods `ExecuteFormat`, `ExecuteFormatAsync`, `ReadFormat` and `ReadAFormatsync` can accept database command parameters as **interpolated strings.**

- For example:

```csharp
var user = connection
    .ReadFormat<User>(@$"
        select u.* 
        from users u, logs l 
        where u.usrid = {userId} and u.usrid = l.usrid and l.date = {date}")
    .Single();
```

- In this example, variables `userId` and `date` are used as normal **database command parameters**.

- If we want, to use `ReadFormat` and, in certain cases, skip command parameters and just use normal string interpolation, we could use a `raw` modifier. Example:

```csharp
var table = "logs";
var user = connection
    .ReadFormat<User>(@$"
        select u.* 
        from users u, {table:raw} l 
        where u.usrid = {userId} and u.usrid = l.usrid and l.date = {date}")
    .Single();
```

- In this example, the variable `table` is used in normal string interpolation (because we used the `raw` modifier), and variables `userId` and `date` are still used as database command parameters.

- Value `RawInterpolationParameterEscape` determines the value of this modifier.

- The default is `raw`.
