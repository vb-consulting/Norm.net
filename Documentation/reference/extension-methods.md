---
title: Extensions Methods
order: 1
nextUrl: 
nextTitle: 
prevUrl: 
prevTitle: 
---

## Extensions Methods

### As

```csharp
Norm As(System.Data.CommandType type);
```

- Sets the [`CommandType`](https://learn.microsoft.com/en-us/dotnet/api/system.data.commandtype) for the next database command.

- This specifies how a command string is interpreted:

```csharp
namespace System.Data
{
    //
    // Summary:
    //     Specifies how a command string is interpreted.
    public enum CommandType
    {
        //
        // Summary:
        //     An SQL text command. (Default.)
        Text = 1,
        //
        // Summary:
        //     The name of a stored procedure.
        StoredProcedure = 4,
        //
        // Summary:
        //     The name of a table.
        TableDirect = 512
    }
}
```

- Example:

```csharp
//
// Execute a stored procedure "delete_inactive_customers"
//
connection
    .As(System.Data.CommandType.StoredProcedure)
    .Execute("delete_inactive_customers");
```

### AsProcedure
 
```csharp
Norm AsProcedure();
```

 - Sets the [`CommandType`](https://learn.microsoft.com/en-us/dotnet/api/system.data.commandtype) to `System.Data.CommandType.StoredProcedure`.

- Example:

```csharp
//
// Execute a stored procedure "delete_inactive_customers"
//
connection
    .AsProcedure()
    .Execute("delete_inactive_customers");
```

- This is exactly the same as the previous example (`As(System.Data.CommandType.StoredProcedure)`).

### AsText

```csharp
Norm AsText();
```

 - Sets the [`CommandType`](https://learn.microsoft.com/en-us/dotnet/api/system.data.commandtype) to `System.Data.CommandType.Text`.

 - This is **the default** command interpretation.

- Example:

```csharp
//
// Execute a function "delete_inactive_customers" as text
//
connection
    .AsText() // this is unnecessary
    .Execute("select delete_inactive_customers()");
```

- Since `Execute` also returns the current instance, `AsText` may be used in a chain of commands. 
  
- So, in the example, `AsText` makes sense:

```csharp
//
// Execute a function "delete_inactive_customers1" as procedure and then delete_inactive_customers2 as text
connection
    .AsProcedure()
    .Execute("delete_inactive_customers1");
    .AsText() 
    .Execute("select delete_inactive_customers2()");
```

### Prepared

```csharp
Norm Prepared();
```

 - Sets the next command into a prepared (or compiled) mode. 
  
 - [`DbCommand.Prepare` Method Reference](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand.prepare)

- Example:

```csharp
//
// Execute prepared command
//
connection
    .Prepared() 
    .Execute("update my_table set value = 1 where id = 1");
```

- Note: to set all commands into a prepared (or compiled) mode - use `Configure` method in your program startup:

```csharp
//
// Set global options in your startup 
// to always execute in prepared mode (default is false).
//
NormOptions.Configure(options =>
{
    options.Prepared = true;
});

// later in your code ...

//
// Execute prepared command
//
connection
    .Prepared() 
    .Execute("update my_table set value = 1 where id = 1");
```

- Disclaimer: command preparation may vary on different provider implementations, and in some cases, it may be set as a connection string parameter. For example, see [prepare with PostgreSQL](https://www.npgsql.org/doc/prepare.html).

### WithCancellationToken

```csharp
Norm WithCancellationToken(System.Threading.CancellationToken cancellationToken);
```

- Sets the cancelation token for the next command that can propagate the notification that operations should be canceled.

- Example:

```csharp
// create a new cancelation source
var cancellationSource = new CancellationTokenSource();
cancellationSource.CancelAfter(5000);

//
// Execute command and cancel after 500 milliseconds as defined by the cancelation source
//
connection
    .WithCancellationToken(cancellationSource.Token)
    .Execute("update my_table set value = 1 where id = 1");
```

### WithCommandBehavior

```csharp
Norm WithCommandBehavior(System.Data.CommandBehavior behavior);
```

- Sets the [command behavior](https://learn.microsoft.com/en-us/dotnet/api/system.data.commandbehavior) on the next [reader execution](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand.executereader#system-data-sqlclient-sqlcommand-executereader(system-data-commandbehavior)) (by `Read` extension methods).

- This provides a description of the results of the query and its effect on the database:

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

- Example:

```csharp
//
// Read single row result from a single result set and close the connection when done
//
var result = connection
    .WithCommandBehavior(CommandBehavior.CloseConnection | CommandBehavior.SingleResult | CommandBehavior.SingleRow)
    .Read<int>("select count(*) from my_table");
```

### WithCommandCallback

```csharp
Norm WithCommandCallback(System.Action<System.Data.Common.DbCommand> dbCommandCallback);
```

- Adds a **callback function** that will be executed when:
  - After the [`DbCommand`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand) object has been created and initialized.
  - Before the command execution.

- Possible usage of this callback is to gain access to the `DbCommand` instance before execution to be able to change its properties.

- Common usage of this callback is usually to log command text.

- Example:

```csharp
//
// Read a single row and log the command text
//
var result = connection
    .WithCommandCallback(command => logger.LogInformation("SQL COMMAND: {0}", command.CommandText))
    .Read<int>("select count(*) from my_table");
```

- This example will produce an info log with the following content: `SQL COMMAND: select count(*) from my_table`.

- Note: this callback can be set for all commands - use `Configure` method in your program startup:

```csharp
//
// Set global options in your startup 
//
NormOptions.Configure(options =>
{
    options.DbCommandCallback = command => logger.LogInformation("SQL COMMAND: {0}", command.CommandText)
});

// later in your code ...

//
// Read a single row and log the command text
//
var result = connection.Read<int>("select count(*) from my_table");
```

- This will also produce an info log with the same content: `SQL COMMAND: select count(*) from my_table`.

### WithComment

```csharp
Norm WithComment(string comment);
```

 - Sets the custom comment header for the next command. The content of the `comment` string will be added to the top of the `CommandText` as an SQL comment.

- Example:

```csharp
var result = connection
    .WithComment("This is my comment")
    .WithCommandCallback(command => Console.WriteLine(command.CommandText))
    .Read<int>("select count(*) from my_table");
```

- This will print out the following console output:

```markdown
/*
This is my comment
*/
select count(*) from my_table
```

### WithCommentCallerInfo

```csharp
Norm WithCommentCallerInfo();
```

 - Sets the automatic custom comment header to the next command that will include caller information. 
 
 - Caller information is compiler-generated metadata that includes:
   - **Caller member name**: a method or property name of the caller to the method.
   - **Caller file path**: a full path of the source file that contains the caller at the compile time.
   - **Caller line number**: a line number in the source file at which the method is called at the compile time.

- Example:

```csharp
var result = connection
    .WithCommentCallerInfo()
    .WithCommandCallback(command => Console.WriteLine(command.CommandText))
    .Read<int>("select count(*) from my_table");
```

- If this example is executed in a method named `CallerInfoTest` that is located in a file `/home/MyProject/Examples.cs` at line 100 - this example will print out the following console output:

```markdown
/*
at CallerInfoTest in /home/MyProject/Examples.cs#100
*/
select count(*) from my_table
```

- Note: this option can be set for every command - use `Configure` method in your program startup:

```csharp
//
// Set global options in your startup 
//
NormOptions.Configure(options =>
{
    options.CommandCommentHeader.Enabled = true;
    options.CommandCommentHeader.IncludeCallerInfo = false;
});

// later in your code ...

//
// Read the single row 
//
var result = connection
    .Read<int>("select count(*) from my_table");
```

- This will include exact caller info (caller member, file path, and line number) in every command you create.

 ### WithCommentParameters

 ```csharp
Norm WithCommentParameters();
```

 - Sets the option for the next command to include command parameters in the command header comment.

- Example:

```csharp
var result = connection
    .WithCommentParameters()
    .WithCommandCallback(command => Console.WriteLine(command.CommandText))
    .WithParameters(1, "foo", false, new DateTime(2022, 5, 19))
    .Execute("select @p1, @p2, @p3, @p4");
```

- This will print out the following console output:

```markdown
/*
@p1 int = 2
@p2 nvarchar = "bar"
@p3 bit = false"
@p4 datetime = "1977-05-19T00:00:00.0000000"
*/
select @p1, @p2, @p3, @p4
```

### WithCommentHeader

 - Sets the options for the next command to include command header comment with options set in method parameters.

```csharp
Norm WithCommentHeader(string comment = null, bool includeCommandAttributes = true, bool includeParameters = true, bool includeCallerInfo = true, bool includeTimestamp = false);
```

- Parameters:
  
  - `string comment = null` - sets the custom text comment header for the next command.
  - `bool includeCommandAttributes = true` - includes command attributes, such as database provider, command type (text, procedure), and command timeout in the comment header for the next command.
  - `bool includeParameters = true` - includes parameter names and values in the comment header for the next command.
  - `bool includeCallerInfo = true` - includes caller info (member name, file path, and line number) in the comment header for the next command.
  - `bool includeTimestamp = false` - includes a current timestamp in the comment header for the next command.

 ### WithParameters

 ```csharp
Norm WithParameters(params object[] parameters);
```

 - Sets parameters for the next command. 
  
 - This method can receive one or more parameters of the `object` type.
  
 - Parameter value can be either:
  
   - Simple type (integers, strings, dates, etc.).
   - Object instances.
   - Two value tuples (value and database type).
   - [`DbParameter`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbparameter) instance.

- Depending on the parameter type, parameters can be set in different ways.

- Using simple values - we can set the positional parameters.

- Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- These parameters are set by position. 
  
- Name of parameters in query `select @s, @i, @b, @d, @null` is not actually important. 

- First value `"str"` is set to the first parameter `@s`, the second value to the second parameter `@i` and so on. Names of these parameters can be anything.

- Norm also supports PostgreSQL positional parameters where each parameter in the query is defined with a `$` character and position index (`$1`, `$2`, `$2`, etc.).

- Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
    .Single();

```

- Those two parameter styles can even be mixed in a query. Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select $s, @i, $3, $4, $5")
    .Single();
```

- Sometimes, we want to set a specific database type to a positional parameter. 

- In those cases, we can use a two values tuple, where the first value is the parameter value and the second value is the specific database type.

- Database type value of system enum [`System.Data.DbType`](https://learn.microsoft.com/en-us/dotnet/api/system.data.dbtype). Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(
        ("str", DbType.AnsiString),
        (999, DbType.Int32),
        (true, DbType.Boolean),
        (new DateTime(1977, 5, 19), DbType.Date),
        (null, DbType.AnsiString))
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- You can also mix simple values and tuple values with a specific database type. Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, (new DateTime(1977, 5, 19), DbType.Date), (null, DbType.AnsiString))
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- You can also use provider-specific database type enums. Example for PostgreSQL types:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(
        ("str", NpgsqlDbType.Text),
        (999, NpgsqlDbType.Bigint),
        (true, NpgsqlDbType.Boolean),
        (new DateTime(1977, 5, 19), NpgsqlDbType.Date),
        ((string)null, NpgsqlDbType.Text))
    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
    .Single();
```

- Parameter value can also be an object instance. 

- In that case, each public property or public field will be a named parameter with the same name and the same value. Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new
    {
        d = new DateTime(1977, 5, 19),
        b = true,
        i = 999,
        s = "str",
        @null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- This example uses an **anonymous object instance** to create a named parameters `d`, `b`, `i`, `s` and `˙null` with associated values.

- In this example paramaters appears in different order, because they are mapped by name, not by position.

- Note that `@null` starts with the `@` prefix because `null` is a C# keyword and the `@` prefix is ignored.

- Besides anonymous objects, normal instances can also be used as well:

```csharp
class TestClass
{
    public string S { get; set; }
    public int I { get; set; }
    public bool B { get; set; }
    public DateTime D { get; set; }
    public string Null { get; set; }
}

var (s, i, b, d, @null) = connection
    .WithParameters(new TestClass
    {
        D = new DateTime(1977, 5, 19),
        B = true,
        I = 999,
        S = "str",
        Null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- Note that parameter names are NOT case sensitive. 

- Also, you can set a specific database type, either generic `DbType` or provider-specific database type - by using tuples:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new
    {
        d = (new DateTime(1977, 5, 19), DbType.Date), // set parameter type to generic DbType.Date
        b = (true, NpgsqlDbType.Boolean), // set parameter type to PostgreSQL specific NpgsqlDbType.Boolean
        i = 999,
        s = "str",
        @null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- Positional and instance-named parameters can be mixed. Also, you can have multiple instance parameters:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", // "str" parameter is mapped by position in first place
    new // first named instance
    {
        d = new DateTime(1977, 5, 19),
        b = true,
    },
    new // second named instance
    {
        i = 999,
        @null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- For a greater parameter control, use a specific [`DbParameter`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbparameter) instance can also be used.

- SQL Server example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(
        new SqlParameter("s", "str"),
        new SqlParameter("i", 999),
        new SqlParameter("b", SqlDbType.Bit) { Value = true },
        new SqlParameter("d", new DateTime(1977, 5, 19)),
        new SqlParameter("null", SqlDbType.NText) { Value = null })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- PostgreSql example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(
        new NpgsqlParameter("s", "str"),
        new NpgsqlParameter("i", 999),
        new NpgsqlParameter("b", NpgsqlDbType.Boolean) { Value = true },
        new NpgsqlParameter("d", new DateTime(1977, 5, 19)),
        new NpgsqlParameter("null", NpgsqlDbType.Text) { Value = null })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- You can also set the `DbParameter` instance to an instance field or a property:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new
    {
        d = new NpgsqlParameter("d", new DateTime(1977, 5, 19)),
        b = true,
        i = 999,
        s = "str",
        @null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- In that case, the name of the instance property is discarded, and the actual parameter name from the `DbParameter` instance is valid. That means that the first date property could be named differently, for example: `_ = new NpgsqlParameter("d", new DateTime(1977, 5, 19)),`.

- Using `DbParameter` instances is helpful to have and use output parameters. PostgreSQL example:

```csharp
var p = new NpgsqlParameter("test_param", "I am output value") { Direction = ParameterDirection.InputOutput };
connection
    .Execute(@"
        create function test_inout_param_func_1(inout test_param text) returns text as
        $$
        begin
            test_param := test_param || ' returned from function';
        end
        $$
        language plpgsql")
    .AsProcedure()
    .WithParameters(p)
    .Execute("test_inout_param_func_1");

Assert.Equal("I am output value returned from function", p.Value);
```

- Note: you can combine any style of parameters (positional with simple values, value-type tuples, object instances, `DbParameter` instances) in any combination.

 ### WithReaderCallback

- Sets the custom database reader callback function that will be executed on each database reader iteration step for every field.

 ```csharp
 Norm WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback);
 ```

- Custom callback function will be called with one tuple value parameter with three values:
   - `string Name` - field name that is being read.
   - `int Ordinal` - ordinal, zero-based position of the field that is being read.
   - `DbDataReader Reader` - [`DbDataReader`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbdatareader) instance.

The custom callback function should return an object value that will be used as a value in further object mapping.

- If the custom callback function returns a `null` value - it will fall back to default behavior (reading and mapping that value).

- Return [`DBNull.Value`](https://learn.microsoft.com/en-us/dotnet/api/system.dbnull) value to map to `null` value.

- Examples:

- Query returns two fields: `i` and `j`, with values 1, 2 and 3. The custom function that adds 1 to the column at the first position would look like this:

```csharp
var result = connection
    .WithReaderCallback(r =>
    {
        if (r.Ordinal == 0)
        {
            return r.Reader.GetInt32(0) + 1;
        }
        return null; // default behavior
    })
    .Read<(int i, int j)>(@"
        select * from (values 
            (1, 1),
            (2, 2),
            (3, 3)
        ) t(i, j)")
    .ToArray();
```

- This can be simplified with expression function and switch expression, like this for example:

```csharp
var result = connection
    .WithReaderCallback(r => r.Ordinal switch
    {
        0 => r.Reader.GetInt32(0) + 1,
        _ => null // default behavior
    })
    .Read<(int i, int j)>(@"
        select * from (values 
            (1, 1),
            (2, 2),
            (3, 3)
        ) t(i, j)")
    .ToArray();
```

- If we wanted, for example, just to return `null` in the first column if the value is 1, otherwise keep the original value, we can do this:

```csharp
var result = connection
    .WithReaderCallback(r => r.Ordinal switch
    {
        0 => r.Reader.GetInt32(0) == 1 ? DBNull.Value : null, // DBNull.Value to set the value to null; otherwise, fallback to default behavior
        _ => null
    })
    .Read<(int i, int j)>(@"
        select * from (values 
            (1, 1),
            (2, 2),
            (3, 3)
        ) t(i, j)")
    .ToArray();
```

- This technique is used to handle complex types and to implement complex mappings, otherwise not supported by the library.

- Note: to set all database reads to execute a specific callback reader that will return custom values - use the `Configure` method in your program startup.

- For example, every field that is `json` database type can be converted to [`JsonObject`](https://learn.microsoft.com/en-us/dotnet/api/system.json.jsonobject) object with this configuration:

```csharp
// Reader callback can be in an expression method instead of a lambda function
private object? ReaderCallback((string Name, int Ordinal, DbDataReader Reader) arg) => 
    // switch over the column type
    arg.Reader.GetDataTypeName(arg.Ordinal) switch
{
    // if the column type is json, then convert it to JsonObject
    "json" => JsonNode.Parse(arg.Reader.GetString(arg.Ordinal))?.AsObject(),
    _ => null // default value
};

// Set the option in your startup code...
NormOptions.Configure(options => options.DbReaderCallback = ReaderCallback);
 ```
 
 ### WithTimeout

 - `WithTimeout(int timeout)` - add command timeout.

### WithTransaction

 - `WithTransaction(DbTransaction transaction)` - add transaction object to command.

### WithUnknownResultType

 - `WithUnknownResultType(params bool[])` - set the unknown type for all or some fields that will be returned as raw string (`Npgsql` only).

