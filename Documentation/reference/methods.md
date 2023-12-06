---
title: Methods
order: 1
nextUrl: /norm.net/docs/reference/options/
nextTitle: Options
prevUrl: /norm.net/docs/getting-started/basic-concepts/
prevTitle: Basic Concepts
---

## Extensions Methods

- To implement fluid syntax, Norm extensions are implemented in two versions:

1) As `DbConnection` object extension methods. 
2) As instance methods.

- Both extension methods and instance methods have the same basic signature (except for `this DbConnection connection` parameter in extension methods), and they return the `Norm` instance whenever they can to ensure fluid syntax.

---

### As

```csharp
// Extension
public static Norm As(this DbConnection connection, System.Data.CommandType type);
// Norm Instance Method
public Norm As(System.Data.CommandType type);
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

---

### AsProcedure
 
```csharp
// Extension
public static Norm AsProcedure(this DbConnection connection);
// Norm Instance Method
public Norm AsProcedure();
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

---

### AsText

```csharp
// Extension
public static Norm AsText(this DbConnection connection);
// Norm Instance Method
public Norm AsText();
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

### Execute

```csharp
public static Norm Execute(this DbConnection connection, string command, object parameters = null);
public Norm Execute(string command, object parameters = null);
```

- Executes the SQL command without returning a value from the database.

- Extension returns the new `Norm` instance and instance method existing `Norm` instance.

- Can have additional parameters where parameters are supplied as an anonymous object instance.

- The last three parameters with the `Caller` attribute (`memberName`, `sourceFilePath` and `sourceLineNumber`) should not be supplied, they are intended to be used in diagnostics and logging and are supplied automatically by the compiler. 

- Example:

```csharp
connection
    .Execute("insert into test values (@p1, @p2)", new {p1 = "first", p2 = "second"});
```

---

### ExecuteAsync

```csharp
public static async ValueTask<Norm> ExecuteAsync(this DbConnection connection, string command, object parameters = null);
public async ValueTask<Norm> ExecuteAsync(string command, object parameters = null);
```

- Execute the SQL command without returning a value from the database and return a value task representing the asynchronous operation.

- Value task returns the new `Norm` instance and instance method existing `Norm` instance.

- Can have additional parameters where parameters are supplied as an anonymous object instance.

- Example:

```csharp
await connection
    .ExecuteAsync("insert into test values (@p1, @p2)", new {p1 = "first", p2 = "second"});
```

---

### ExecuteFormat

```csharp
public static Norm ExecuteFormat(this DbConnection connection, FormattableString command, object parameters = null);
public Norm ExecuteFormat(FormattableString command, object parameters = null);
```

- Executes the SQL command without returning a value from the database in an interpolated (formattable) string and parses a formattable string for database parameters.

- Extension returns the new `Norm` instance and instance method existing `Norm` instance.

- Can have additional parameters where parameters are supplied as an anonymous object instance.

- Example:

```csharp
var p1 = "first"; 
var p2 = "second";
connection
    .ExecuteFormat($"insert into test values ({p1}, {p2})");
```

---

### ExecuteFormatAsync

```csharp
public static async ValueTask<Norm> ExecuteFormatAsync(this DbConnection connection, FormattableString command, object parameters = null);
public async ValueTask<Norm> ExecuteFormatAsync(FormattableString command, object parameters = null);
```

- Executes the SQL command without returning a value from the database in an interpolated (formattable) string and parses a formattable string for database parameters and returns a value task representing the asynchronous operation.

- Value task returns the new `Norm` instance and instance method existing `Norm` instance.

- Can have additional parameters where parameters are supplied as an anonymous object instance.

- Example:

```csharp
var p1 = "first"; 
var p2 = "second";
await connection
    .ExecuteFormatAsync($"insert into test values ({p1}, {p2})");
```

---

### GetRecordsAffected

```csharp
// Extension
public static int? GetRecordsAffected(this DbConnection connection);
// Norm Instance Method
public int? GetRecordsAffected();
```

- Returns the integer value returned from:
  - The last [`ExecuteNonQuery()`](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand.executenonquery) execution, initiated by `Execute` call (or `ExecuteNonQueryAsync()` initiated by `ExecuteAsync`).
  - The last value of the [`RecordsAffected`](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.recordsaffected) reader object property, that is initiated by `Read` or `ReadAsync` calls.

- The Result value is `NULL` if neither `Execute`, `ExecuteAsync`, `Read` or `ReadAsync` is called for the current call chain.

- Example:

```csharp
var rowsAffected = connection
    .Execute("insert into rows_affected_test values ('foo')")
    .GetRecordsAffected();
```

---

### Multiple
### MultipleAsync
### MultipleFormat
### MultipleFormatAsync

- There are four connection extensions and four instance methods:
  - `Multiple` 
  - `MultipleAsync` 
  - `MultipleFormat` 
  - `MultipleFormatAsync` 


- Each of these methods and extensions has one non-generic version and 13 generic versions.

- They all create a disposable object that can be used to execute or read multiple commands in a batch.

> Note: For more information on mapping system [see multiple mappings section.](/norm.net/docs/reference/multiple/)

---

### Norm

```csharp
// Extension
public static Norm Norm(this DbConnection connection);
```

- Returns the new Norm instance.

- There is no instance method.

- Usage example:

```csharp
var instance = connection.Norm();
instance.Read("select * from rows_affected_test").ToList();
rowsAffected = instance.GetRecordsAffected();
```

---

### Prepared

```csharp
// Extension
public static Norm Prepared(this DbConnection connection);
// Norm Instance Method
public Norm Prepared();
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

- Note: to set all commands into a prepared (or compiled) mode - use the `Configure` method in your program startup:

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

- Disclaimer: command preparation may vary on different provider implementations, and in some cases, it may be set as a connection string parameter. For example, see the [prepared statements with PostgreSQL](https://www.npgsql.org/doc/prepare.html).

---

### Read

### ReadAsync

### ReadFormat

### ReadFormatAsync

- There are four connection extensions and four instance methods:
  - `Read` 
  - `ReadAsync` 
  - `ReadFormat` 
  - `ReadFormatAsync` 


- Each of these methods and extensions has one non-generic version and 13 generic versions.

- They all implement a complex mapping system that enables you to map to anything from simple types to tuples and object instances.

> Note: For more information on mapping system [see read mappings section.](/norm.net/docs/reference/read/)

---

### WithCancellationToken

```csharp
// Extension
public static Norm WithCancellationToken(this DbConnection connection, System.Threading.CancellationToken cancellationToken);
// Norm Instance Method
public Norm WithCancellationToken(System.Threading.CancellationToken cancellationToken);
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

---

### WithCommandBehavior

```csharp
// Extension
public static Norm WithCommandBehavior(this DbConnection connection, System.Data.CommandBehavior behavior);
// Norm Instance Method
public Norm WithCommandBehavior(System.Data.CommandBehavior behavior);
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

---

### WithCommandCallback

```csharp
// Extension
public static Norm WithCommandCallback(this DbConnection connection, System.Action<System.Data.Common.DbCommand> dbCommandCallback);
// Norm Instance Method
public Norm WithCommandCallback(System.Action<System.Data.Common.DbCommand> dbCommandCallback);
```

- Adds a **callback function** that will be executed when:
  - After the [`DbCommand`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand) object has been created and initialized.
  - Before the command execution.

- The possible usage of this callback is to gain access to the `DbCommand` instance before execution to be able to change its properties.

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

- The common use of this callback is when we want to run some commands in a prepared mode:

```csharp
var user = connection
    .WithCommandCallback(command => command.Prepare())
    .WithParameters(userId, date)
    .Read<User>(@"
        select u.* 
        from users u, logs l 
        where u.usrid = $1 and u.usrid = l.usrid and l.date = $2")
    .Single();
```

- Note: this callback can be set for all commands - use the `Configure` method in your program startup:

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

---

### WithComment

```csharp
// Extension
public static Norm WithComment(this DbConnection connection, string comment);
// Norm Instance Method
public Norm WithComment(string comment);
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

---

### WithCommentCallerInfo

```csharp
// Extension
public static Norm WithCommentCallerInfo(this DbConnection connection);
// Norm Instance Method
public Norm WithCommentCallerInfo();
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

- Note: this option can be set for every command - use the `Configure` method in your program startup:

```csharp
//
// Set global options in your startup 
//
NormOptions.Configure(options =>
{
    options.CommandCommentHeader.Enabled = true;
    options.CommandCommentHeader.IncludeCallerInfo = true;
});

// later in your code ...

//
// Read the single row 
//
var result = connection
    .Read<int>("select count(*) from my_table");
```

- This will include exact caller info (caller member, file path, and line number) in every command you create.

---

### WithCommentParameters

```csharp
// Extension
public static Norm WithCommentParameters(this DbConnection connection);
// Norm Instance Method
public Norm WithCommentParameters();
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

---

### WithCommentHeader

 - Sets the options for the next command to include command header comment with options set in method parameters.

```csharp
// Extension
public static Norm WithCommentHeader(this DbConnection connection, 
    string comment = null, 
    bool includeCommandAttributes = true, 
    bool includeParameters = true, 
    bool includeCallerInfo = true, 
    bool includeTimestamp = false);
// Norm Instance Method
public Norm WithCommentHeader(string comment = null, 
    bool includeCommandAttributes = true, 
    bool includeParameters = true, 
    bool includeCallerInfo = true, 
    bool includeTimestamp = false);
```

- Parameters:
  - `string comment = null` - sets the custom text comment header for the next command.
  - `bool includeCommandAttributes = true` - includes command attributes, such as database provider, command type (text, procedure), and command timeout in the comment header for the next command.
  - `bool includeParameters = true` - includes parameter names and values in the comment header for the next command.
  - `bool includeCallerInfo = true` - includes caller info (member name, file path, and line number) in the comment header for the next command.
  - `bool includeTimestamp = false` - includes a current timestamp in the comment header for the next command.

---

### WithParameters

 ```csharp
// Extension
public static Norm WithParameters(this DbConnection connection, params object[] parameters);
// Norm Instance Method
public Norm WithParameters(params object[] parameters);
```

- This method can receive one or more arguments of the `object` type.

- The parameter value can be either:
   - Simple type (integers, strings, dates, etc.).
   - Object instances.
   - Two value tuples (value and database type).
   - [`DbParameter`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbparameter) instance.

- Depending on the parameter type, parameters can be set in different ways: positional, named, or mixed.

> Note: For more information on working with parameters [see working with parameters section.](/norm.net/docs/reference/parameters/)

---

### WithReaderCallback

- Sets the custom database reader callback function that will be executed on each database reader iteration step for every field:

 ```csharp
 // Extension
public static Norm WithReaderCallback(this DbConnection connection, Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback);
// Norm Instance Method
public Norm WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback);
 ```

- The custom callback function will be called with one tuple value parameter with three values:
   - `string Name` - field name that is being read.
   - `int Ordinal` - ordinal, zero-based position of the field that is being read.
   - `DbDataReader Reader` - [`DbDataReader`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbdatareader) instance.

The custom callback function should return an object value that will be used as a value in further object mapping.

- If the custom callback function returns a `null` value - it will fall back to default behavior (reading and mapping that value).

- Return [`DBNull.Value`](https://learn.microsoft.com/en-us/dotnet/api/system.dbnull) value to map to `null` value.

- Examples:

- The query returns two fields: `i` and `j`, with values 1, 2 and 3. The custom function that adds 1 to the column at the first position would look like this:

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

- For example, every field that is the `JSON` database type can be converted to the [`JsonObject`](https://learn.microsoft.com/en-us/dotnet/api/system.json.jsonobject) object with this configuration:

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

---

### WithTimeout

- Sets the wait time in seconds for the connection commands before terminating the attempt to execute a command and generating an error:

```csharp
// Extension
public static Norm WithTimeout(this DbConnection connection, int timeout);
// Norm Instance Method
public Norm WithTimeout(int timeout);
```

- For example, execute stored procedure `update_data` with the command timeout 60 seconds.

```csharp
connection
    .AsProcedure()
    .WithTimeout(60)
    .Execute("update_data");
```

---

### WithTransaction

- Sets the transaction object for the current database command:

```csharp
// Extension
public static Norm WithTransaction(this DbConnection connection, DbTransaction transaction);
// Norm Instance Method
public Norm WithTransaction(DbTransaction transaction);
```

Example:

```csharp
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

---

### WithUnknownResultType

> **PostgreSQL only feature**

- Sets PostgreSQL results behavior by marking some or all results as unknown. The unknown result type is serialized as a raw string:

```csharp
// Extension
public static Norm WithUnknownResultType(this DbConnection connection, params bool[] list);
// Norm Instance Method
public Norm WithUnknownResultType(params bool[] list);
```

- To set all results as unknown, call `WithUnknownResultType` without parameters.

- For example, PostgreSQL query that returns text, boolean, date, numeric, and JSON fields.

```csharp
var (@int, @bool, @date, @num, @json) = connection
    .WithUnknownResultType()
    .Read<string, string, string, string, string>("select 1::int, true::bool, '1977-05-19'::date, 3.14::numeric, '{\"x\": \"y\"}'::json")
    .Single();
```

- To set some results as unknown, call `WithUnknownResultType` and set the true value to the field position that needs to be marked as unknown:

```csharp
var (@int, @bool, @date, @num, @json) = connection
    .WithUnknownResultType(true, false, true, false, true)
    .Read<string, bool, string, decimal, string>("select 1::int, true::bool, '1977-05-19'::date, 3.14::numeric, '{\"x\": \"y\"}'::json")
    .Single();
```

- Marking results as unknown is useful for handling exotic PostgreSQL types and custom domains, which the underlying data access provider does not know how to handle yet.
