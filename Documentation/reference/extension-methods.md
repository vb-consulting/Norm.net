---
title: Extensions Methods
order: 1
nextUrl: 
nextTitle: 
prevUrl: 
prevTitle: 
---

# Extensions Methods

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

- Since `Execute` also retruns the current instance AsText may be used in a chain of commands:


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
  
 - [DbCommand.Prepare Method Reference](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand.prepare)

- Example:

```csharp
//
// Execute a function "delete_inactive_customers" as text
//
connection
    .Prepared() 
    .Execute("update table set value = 1");
```

Note: This is unnecessary on SQL Server. This feature was introducuded because of PostgreSQL. See this [article](https://www.npgsql.org/doc/prepare.html).

### WithCancellationToken

 - `WithCancellationToken(System.Threading.CancellationToken cancellationToken)` - sets the cancellation token for the execution.


### WithCommandBehavior

 - `WithCommandBehavior(System.Data.CommandBehavior behavior)` - sets the reader behavior like default, single result, schema only, key info, single row, sequential access or close connection (see more at [System.Data.CommandBehavior](https://learn.microsoft.com/en-us/dotnet/api/system.data.commandbehavior)).

### WithCommandCallback

 - `WithCommandCallback(Action<DbCommand> dbCommandCallback)` - adds a command callback to be executed before command execution which gives you a full access to the [DbCommand](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand) object.

### WithComment

 - `WithComment(string comment)` - adds custom comment to SQL command.

### WithCommentCallerInfo

 - `WithCommentCallerInfo()` - adds comment to SQL command what contains a caller info data (source method name from where this command was executed with source code file path and line number if available).

### WithCommentHeader

 - `WithCommentHeader(string comment = null, bool includeCommandAttributes = true, bool includeParameters = true, bool includeCallerInfo = true, bool includeTimestamp = false)` - configures comment to SQL command to include either custom comment, command attributes, caller info, timestamp or parameters.
 
 ### WithCommentParameters

 - `WithCommentParameters()` - adds comment to SQL command what contains a parameters data.

 ### WithParameters

 - `WithParameters(params object[] parameters)` - add command parameters list.

 ### WithReaderCallback

 - `WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)` - adds reader callback executed on each reader step. Gives you chance to return alternative values or types by returning a non-null value.
 
 ### WithReaderCallback

 - `WithTimeout(int timeout)` - add command timeout.

### WithTransaction

 - `WithTransaction(DbTransaction transaction)` - add transaction object to command.

### WithUnknownResultType

 - `WithUnknownResultType(params bool[])` - set the unknown type for all or some fields that will be returned as raw string (`Npgsql` only).

