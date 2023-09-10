---
title: Basic Concepts
order: 4
nextUrl: /docs/fundamentals/read-method/
nextTitle: Read Method
prevUrl: /docs/getting-started/first-use/
prevTitle: First Use
---

## Basic Concepts

### Connection Extensions

**Three main extensions methods (and their derivatives):**

1) **`Execute`** - execute a command **without returning any values**.
2) **`Read`** - execute and return an **iterator over values**.
3) **`Multiple`** - return disposable object for multiple `Read` operations.

- Both extensions will attempt to **open the underlying connection** - and then initiate command execution.

- `Execute` extensions are generally simple since they don't return any values. Just execute a command without returning a value.

- `Read` extensions implement many overload versions and support many different type mappings (tuples, named tuples, instances, dynaimc, anonymous, etc).

[See more on the `Read` method.](/docs/fundamentals/read-method/)

### Fluid Syntax

- In addition to these three main extensions - there are 14 additional helper extensions on the connection object.

- These extensions methods will return `Norm` instance object that has state set according to the extensions.

- `Norm` instance object implements the same instance methods as extension methods on the connection object.

- This is useful to be able to have a **fluid syntax.** 

- Foe example, execute a stored procedure with command timoout 60 seconds would look like this by using **fluid syntax**:

```csharp
//
// Execute a stored procedure with command timoout 60 seconds
//
connection
    .AsProcedure()
    .WithTimeout(60)
    .Execute("delete_inactive_customers");
```

- Note that `AsProcedure` and `WithTimeout` will return `Norm` instance with altered state and all of these three methods exist as instance method and as extension method on connection object.

- A full list of helper extension methods:

 - `As(System.Data.Common.CommandType type)` - sets the command type - text, table direct or stored procedure (see more at [System.Data.CommandType](https://learn.microsoft.com/en-us/dotnet/api/system.data.commandtype))
 - `AsProcedure()` - sets the command type to stored procedure.
 - `AsText()` - sets command type to text.
 - `Prepared()` - sets the command into a prepared mode.
 - `WithCancellationToken(System.Threading.CancellationToken cancellationToken)` - sets the cancellation token for the execution.
 - `WithCommandBehavior(System.Data.CommandBehavior behavior)` - sets the reader behavior like default, single result, schema only, key info, single row, sequential access or close connection (see more at [System.Data.CommandBehavior](https://learn.microsoft.com/en-us/dotnet/api/system.data.commandbehavior)).
 - `WithCommandCallback(Action<DbCommand> dbCommandCallback)` - adds a command callback to be executed before command execution which gives you a full access to the [DbCommand](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbcommand) object.
 - `WithComment(string comment)` - adds custom comment to SQL command.
 - `WithCommentCallerInfo()` - adds comment to SQL command what contains a caller info data (source method name from where this command was executed with source code file path and line number if available).
 - `WithCommentHeader(string comment = null, bool includeCommandAttributes = true, bool includeParameters = true, bool includeCallerInfo = true, bool includeTimestamp = false)` - configures comment to SQL command to include either custom comment, command attributes, caller info, timestamp or parameters.
 - `WithCommentParameters()` - adds comment to SQL command what contains a parameters data.
 - `WithParameters(params object[] parameters)` - add command parameters list.
 - `WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)` - adds reader callback executed on each reader step. Gives you chance to return alternative values or types by returning a non-null value.
 - `WithTimeout(int timeout)` - add command timeout.
 - `WithTransaction(DbTransaction transaction)` - add transaction object to command.
 - `WithUnknownResultType(params bool[])` - set the unknown type for all or some fields that will be returned as raw string (`Npgsql` only).

### Read Iterators

The `Read` extension method and all the overload versions of that method - will always return **the [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators)** of the [`Enumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1) interface type.

Values are **[yielded](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/yield) to the iterator** as they are returned from the database.

For example, the following statements:

```csharp
using Norm;

// doesn't do anything with the database
var result1 = connection.Read<int>("select count(*) from actor");

// doesn't do anything with the database
var result2 = connection.Read<string>("select title from film");
```

These two lines will not send or execute any commands to the database. 

Instead, they will create an [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators) over the database connection that will be executed when the actual iteration starts:

```csharp
using System.Linq;
using Norm;

// doesn't do anything with the database, return iterator
var result1 = connection.Read<int>("select count(*) from actor");

// doesn't do anything with the database, return iterator
var result2 = connection.Read<string>("select title from film");

// executes select count(*) from actor and retuns int
var count = result1.Single();

// executes select title from film and builds a list of film titles
var list = result2.Tolist();
```

Or, simply combine `Linq` statements with the iterator returned from the `Read` method:

```csharp
using System.Linq;
using Norm;

// executes select count(*) from actor and retuns int
var count = connection.Read<int>("select count(*) from actor").Single();

// executes select title from film and builds a list of film titles
var list = connection.Read<string>("select title from film").Tolist();
```

Or, for example, a standard `foreach` iteration without `Linq`:

```csharp
using Norm;

// executes select title from film and iterate titles
foreach(var title in connection.Read<string>("select title from film"))
{
    // ...
}
```

As we can see, the actual execution is **delayed until the first iteration starts**. 

In simple pseudo-code, this is what the `Read` method does:


```csharp
IEnumerable<TResult> Read()
{
    foreach(var record in reader)
    {
        yield return record;
    }
}
```

This approach allows us to build `Linq` expressions over the iterator - which are then executed **only once per iteration**, and there is no need for additional buffering.

### Async Read Iterators

The asynchronous version `ReadAsync` extension method and all the overload versions of that method - will always return **the [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators)** of the [`IAsyncEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1) interface type.

The asynchronous version `ReadAsync` - behaves exactly the same as the synchronous `Read` method - execution is **delayed until the first iteration starts**. 
 
[`IAsyncEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1) enumerator interface that provides **asynchronous iteration over values:**

```csharp
using System.Linq;
using Norm;

// doesn't do anything with the database, return iterator
var result1 = connection.ReadAsync<int>("select count(*) from actor");

// doesn't do anything with the database, return iterator
var result2 = connection.ReadAsync<string>("select title from film");

// executes select count(*) from actor and retuns int
var count = await result1.SingleAsync();

// executes select title from film and builds a list of film titles
var list = await result2.ToListAsync();
```

The example above uses [`System.Linq.Async` package](https://www.nuget.org/packages/System.Linq.Async).

`System.Linq.Async` is an official .NET Foundation implementation of asynchronous LINQ extension methods. It provides an asynchronous version of every LINQ extension method over the [`IAsyncEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1) interface.

### Working With Tuples

Both `Read` and  `ReadAsync` methods implement a complex mapping system, but they both return a single generic value (`IEnumerable<T>` and `IAsyncEnumerable<T>`, respectively).

When those iterators should return multiple values, that generic value is a [tuple type](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples). For example:

- `IEnumerable<(T1, T2)>` - two value tuple `(T1, T2)` for two values.
- `IEnumerable<(T1, T2, T3)>` - three value tuple `(T1, T2, T3)` for three values.
- etc.

The same goes for the `IAsyncEnumerable<T>` type.

This allows for a convenient [tuple deconstruction](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples#tuple-assignment-and-deconstruction), for example:

```csharp
using Norm;

foreach(var (title, description, year) in 
        connection.Read<string, string, int>("select title, description, release_year from film"))
{
    Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year)
}
```

### Global Settings

It is possible to modify `Norm`'s default behavior for the entire application by setting the global settings. For example:

```csharp
using Norm;

NormOptions.Configure(options =>
{
    options.CommandTimeout = 60;
});
```

This example sets a global option for all commands executed by `Norm` to have a timeout of 60 seconds.

> Important note: `NormOptions.Configure` call is **not thread-safe.**

`NormOptions.Configure` call is intended to be executed **only once from the application's startup code**, typically `Startup.cs` or `Program.cs`, before any database command.
