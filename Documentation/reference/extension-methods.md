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

- Since `Execute` also retruns the current instance `AsText` may be used in a chain of commands. 
  
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

- Disclamer: command preparation may vary on different provider implementation and in some cases it may be set as connection string parameter. For example, [prepare with PostgreSQL](https://www.npgsql.org/doc/prepare.html).

### WithCancellationToken

```csharp
Norm WithCancellationToken(System.Threading.CancellationToken cancellationToken)
```

- Sets the cancelation token for the next command that can propagates the notification that operations should be canceled.

- Example:

```csharp
// create a new cancelation source
var cancellationSource = new CancellationTokenSource();
cancellationSource.CancelAfter(5000);

//
// Execute command and cancel after 500 millseconds as defined by the cancelation source
//
connection
    .WithCancellationToken(cancellationSource.Token)
    .Execute("update my_table set value = 1 where id = 1");
```

### WithCommandBehavior

```csharp
Norm WithCommandBehavior(System.Data.CommandBehavior behavior)
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
Norm WithCommandCallback(System.Action<System.Data.Common.DbCommand> dbCommandCallback)
```

- Adds a **callback function** that will be executed when:
  - After the [`DbCommand`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand) object has been created and initialized.
  - Before the command execution.

- Possible usage of this callback is to gain access to the `DbCommand` instance before execution to be able to change its properties.

- Common usage of this callback is usually to log command text.

- Example:

```csharp
//
// Read single row and log the command text
//
var result = connection
    .WithCommandCallback(command => logger.LogInformation("SQL COMMAND: {0}", command.CommandText))
    .Read<int>("select count(*) from my_table");
```

- This example will produce info log with following content: `SQL COMMAND: select count(*) from my_table`.

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
// Read single row and log the command text
//
var result = connection.Read<int>("select count(*) from my_table");
```

- This will also produce info log with the same content: `SQL COMMAND: select count(*) from my_table`.

### WithComment

```csharp
Norm WithComment(string comment)
```

 - Sets the custom comment header for the next command. The content of the `comment` string will be added to the top of the `CommandText` as SQL comment.

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
Norm WithCommentCallerInfo()
```

 - Sets the automatic custom comment header to the next command that will include caller information. 
 
 - Caller information is compiler genereated metadata that include:
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

- If, this example is executed in method named `CallerInfoTest` that is located in a file `/home/MyProject/Examples.cs` at line 100 - this example will print out the following console output:

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

 ### WithCommentParameters

 ```csharp
Norm WithCommentParameters()
```

 - `WithCommentParameters()` - adds comment to SQL command what contains a parameters data.

### WithCommentHeader

```csharp
Norm WithCommentHeader()
```

 - `WithCommentHeader(string comment = null, bool includeCommandAttributes = true, bool includeParameters = true, bool includeCallerInfo = true, bool includeTimestamp = false)` - configures comment to SQL command to include either custom comment, command attributes, caller info, timestamp or parameters.
 

 ### WithParameters

 - `WithParameters(params object[] parameters)` - add command parameters list.

 ### WithReaderCallback

 - `WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)` - adds reader callback executed on each reader step. Gives you chance to return alternative values or types by returning a non-null value.
 
 ### WithTimeout

 - `WithTimeout(int timeout)` - add command timeout.

### WithTransaction

 - `WithTransaction(DbTransaction transaction)` - add transaction object to command.

### WithUnknownResultType

 - `WithUnknownResultType(params bool[])` - set the unknown type for all or some fields that will be returned as raw string (`Npgsql` only).

