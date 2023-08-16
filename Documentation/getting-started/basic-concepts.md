---
title: Basic Concepts
position: 4
---

## Basic Concepts

### Connection Extensions

There are two main extensions to the `System.Data.Common.DbConnection` type:

1) `Execute` - execute a command without returning any values.
2) `Read` - execute and return an iterator over return values.

Both extensions will attempt to **open the underlying connection (if not already open)** - and initiate command execution.

There are another two versions of these extensions for the `async` operations (`ExecuteAsync` and `ReadAsync`), plus another two versions for passing parameters with `FormattableString` (`ExecuteFormat` and `ReadFormat`) and their `async` versions (`ExecuteFormatAsync` and `ReadAFormatsync`).

`Execute` extensions are generally simple since they don't return any values, while `Read` extensions implement many generic overload versions to support many different type mappings.

### Fluid Syntax

There are also many other extensions to the `System.Data.Common.DbConnection` type that **doesn't do anything with the database**.

Instead, they will return **the new `Norm` instance** that implements the same methods as extensions on the `System.Data.Common.DbConnection` type (`Execute`, `Read`, etc).

This is useful for setting a different behavior or settings for the `Execute` and `Read` commands and to have more readable **fluid syntax.** For example:

```csharp
//
// Execute a stored procedure with command timoout 60 seconds
//
connection
    .AsProcedure()
    .WithTimeout(60)
    .Execute("delete_inactive_customers");
```

### Read Iterators

The `Read` extension method and all the overload versions of that method - will always return **the [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators)** of the [`Enumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1) interface type.

Values are **[yielded](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/yield) to the iterator** as they are returned from the database.

For example, the following statements:

```csharp
using Norm;

// doesn't do anything with the database
var result1 = connection.Read<int>("select count(*) from actor");

// doesn't do anything with the database
var result2 = connection.Read("select title from film");
```

These two lines will not send or execute any commands to the database. 

Instead, they will create an [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators) over the database connection that will be executed when the actual iteration starts:

```csharp
using System.Linq;
using Norm;

// doesn't do anything with the database, return iterator
var result1 = connection.Read<int>("select count(*) from actor");

// doesn't do anything with the database, return iterator
var result2 = connection.Read("select title from film");

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
var list = connection.Read("select title from film").Tolist();
```

Or, for example, a standard `foreach` iteration without `Linq`:

```csharp
using Norm;

// executes select title from film and iterate titles
foreach(var title in connection.Read("select title from film"))
{
    // ...
}
```

As we can see, the actual execution is **delayed until the first iteration starts**. 

This approach allows us to build `Linq` expressions over the iterator, which are then executed only once per iteration, and there is no need for additional buffering.

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
var result2 = connection.ReadAsync("select title from film");

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
    Console.WriteLine("Title: {0}, Descrioption: {1}, Year: {2}", title, description, year)
}
```

### Global Settings

...
