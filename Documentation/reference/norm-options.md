---
title: Norm Options
order: 1
nextUrl: 
nextTitle: 
prevUrl: 
prevTitle: 
---

## Norm Options

- Norm options are set at the program startup by calling `NormOptions.Configure` static method.

- This method has the following signature:

```csharp
public static void Configure(Action<NormOptions> options)
```

- Parameter is a callback action to the options object where we can set different options that affect the later behavior.

- Call to `NormOptions.Configure` is not thread-safe; it is intended to be called only once on a program startup.

### CommandCommentHeader

- Sets the command comment header options that will be applied for all commands.

- This is a complex object containing multiple different commands:

```csharp
public class CommandCommentHeader
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

#### CommandCommentHeader.Enabled

- Enables or disables (default) command comment headers.

- Default is false (disabled).

#### CommandCommentHeader.IncludeCommandAttributes

- Include command attribute descriptions to command comment headers.

- Command attributes are:
  
  - Database provider (SQL Server, PostgreSQL, etc).
  
  - Command type (text, stored procedure, etc).

  - Command timeout in seconds.

- Default is true (included).

#### CommandCommentHeader.IncludeCallerInfo

- Include caller info to command comment headers.

- Caller information is compiler-generated metadata that includes:

  - **Caller member name**: a method or property name of the caller to the method.

  - **Caller file path**: a full path of the source file that contains the caller at the compile time.

  - **Caller line number**: a line number in the source file at which the method is called at the compile time.

- Default is true (included).

#### CommandCommentHeader.IncludeParameters

- Include parameter names and values to command comment headers.

- Default is true (included).

#### CommandCommentHeader.ParametersFormat

- A format string that will be used to format parameters comment in the comment header when `CommandCommentHeader.IncludeParameters` is set to true.

- Default is `{0} {1} = {2}\n`, where:
  
  - Format placeholder `{0}` is parameter name.
  
  - Format placeholder `{1}` is parameter type.

  - Format placeholder `{2}` is parameter value.

#### CommandCommentHeader.IncludeTimestamp

- Include the current timestamp to command comment headers.

- Default is false (not included).

#### CommandCommentHeader.OmitStoredProcCommandCommentHeaderForDbTypes

- Skip comment headers when the command type is stored procedure for certain database types.

- Default values are `DatabaseType.Sql | DatabaseType.MySql`, which means that comment headers will not be created for SQL Server and MySQL database types when command types are stored procedures.

- Unlike PostgreSQL, SQL Server and MySQL will yield an error if we try to add a comment header to stored procedures.

### CommandTimeout

- Set the command timeout in seconds for all commands created by the Norm.

- For example, set the command timeout out for 60 seconds:

```csharp
NormOptions.Configure(options =>
{
    options.CommandTimeout = 60;
});
```

### DbCommandCallback

- Set the command callback function that will be executed before every command execution and pass the created `DbCommand`(https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand) object as parameter.

- This is typically used to enable command logging for the entire application. Example:

```csharp
NormOptions.Configure(options =>
{
    options.DbCommandCallback = command => 
        logger.LogInformation("SQL COMMAND: {0}", command.CommandText)
});
```

- This callback could also be used to manipulate already constructed and initialized `DbCommand`(https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand) object on a global level. 
  
- For example, another way to set command time out for 60 seconds for all commands:

```csharp
NormOptions.Configure(options =>
{
    options.DbCommandCallback = command =>
        command.CommandTimeout = 60;
});
```

### DbReaderCallback

- Sets the custom database reader callback function that will be executed on each database reader iteration step for every field - on every database reader operation executed by Norm.

- Custom callback function will be called with one tuple value parameter with three values:
  
   - `string Name` - field name that is being read.

   - `int Ordinal` - ordinal, zero-based position of the field that is being read.

   - `DbDataReader Reader` - [`DbDataReader`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbdatareader) instance.

The custom callback function should return an object value that will be used as a value in further object mapping.

- If the custom callback function returns a `null` value - it will fall back to default behavior (reading and mapping that value).

- Return [`DBNull.Value`](https://learn.microsoft.com/en-us/dotnet/api/system.dbnull) value to map to `null` value.

- This global function callback is used to implement a custom mapping mechanism.

A classic example is mapping from PostgreSQL JSON type to the [`JsonObject`](https://learn.microsoft.com/en-us/dotnet/api/system.json.jsonobject):

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

### KeepOriginalNames

- By default, mapping by name mechanism - supports the **snake-case naming** convention.

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

### MapPrivateSetters

- By default, mapping by name mechanism - will map only isntance members (fields or properties) that have public setters (can be only be set publicly).

- Set `MapPrivateSetters` to true to be able to map instance members with non-public (private or protected) setters.

- Example:

```csharp
public class TestMapPrivateProps
{
    public int PublicInt { get; set; }
    private int PrivateInt { get; set; }
    public int PrivateSetInt { get; private set; }
    protected int ProtectedInt { get; set; }
    public int ProtectedSetInt { get; protected set; }
    public int MissingSetInt { get; }
}
```

- Default is false (on members with public setter are mapped).

### NameParserCallback
### NpgsqlEnableSqlRewriting
### NullableInstances
### Prepared
### RawInterpolationParameterEscape