---
title: Basic Concepts
order: 4
nextUrl: /norm.net/docs/reference/methods/
nextTitle: Methods
prevUrl: /norm.net/docs/getting-started/first-use/
prevTitle: First Use
---

## Basic Concepts

### Extensions And Fluid Syntax

- `Norm` is implemented as a **set of extensions** over the [`System.Data.Common.DbConnection`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection) object.

- Some extensions may return the new `Norm` object instance.

- `Norm` object instances have methods with the same signature as the `System.Data.Common.DbConnection` extensions.

- This allows the use of the fluid syntax over the connection object. Example:

```csharp
//
// Execute a stored procedure with command timoout 60 seconds
//
connection
    .AsProcedure()
    .WithTimeout(60)
    .Execute("delete_inactive_customers");
```

- Since `WithTimeout` exists as an extension and as an instance method, the example from above could have been written like this:

```csharp
//
// Execute a stored procedure with command timoout 60 seconds
//
connection
    .WithTimeout(60)
    .AsProcedure()
    .Execute("delete_inactive_customers");
```

---

### Read Iterators

- The `Read` extension method and all the overload versions of that method - will always return **the [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators)** of the [`Enumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1) interface type.

- Values are **[yielded](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/yield) to the iterator** as they are returned from the database.

- For example, the following statements:

```csharp
using Norm;

// doesn't do anything with the database
var result1 = connection.Read<int>("select count(*) from actor");

// doesn't do anything with the database
var result2 = connection.Read<string>("select title from film");
```

- These two lines will NOT send or execute any commands to the database. 

- Instead, they will create an [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators) over the database connection that will be executed when the actual iteration starts:

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

- Or, simply combine `Linq` statements with the iterator returned from the `Read` method:

```csharp
using System.Linq;
using Norm;

// executes select count(*) from actor and retuns int
var count = connection.Read<int>("select count(*) from actor").Single();

// executes select title from film and builds a list of film titles
var list = connection.Read<string>("select title from film").Tolist();
```

- Or, for example, a standard `foreach` iteration without `Linq`:

```csharp
using Norm;

// executes select title from film and iterate titles
foreach(var title in connection.Read<string>("select title from film"))
{
    // ...
}
```

- As we can see, the actual execution is **delayed until the first iteration starts**. 

- In simple pseudo-code, this is what the `Read` method does:


```csharp
IEnumerable<TResult> Read()
{
    foreach(var record in reader)
    {
        yield return record;
    }
}
```

- This approach allows us to build `Linq` expressions over the iterator - which are then executed **only once per iteration**, and there is no need for additional buffering.

---

### Async Read Iterators

- The asynchronous version `ReadAsync` extension method and all the overload versions of that method - will always return **the [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators)** of the [`IAsyncEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1) interface type.

- The asynchronous version `ReadAsync` - behaves exactly the same as the synchronous `Read` method - execution is **delayed until the first iteration starts**. 
 
- [`IAsyncEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1) enumerator interface that provides **asynchronous iteration over values:**

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

- The example above uses the [`System.Linq.Async` package](https://www.nuget.org/packages/System.Linq.Async).

- `System.Linq.Async` is an official .NET Foundation implementation of asynchronous LINQ extension methods. It provides an asynchronous version of every LINQ extension method over the [`IAsyncEnumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1) interface.

---

### Working With Tuples

- Both `Read` and  `ReadAsync` methods implement a complex mapping system - but they both return a single generic value (`IEnumerable<T>` and `IAsyncEnumerable<T>`, respectively).

- When those iterators should return multiple values, that generic value is a [tuple type](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples). For example:
- `IEnumerable<(T1, T2)>` - two value tuple `(T1, T2)` for two values.
- `IEnumerable<(T1, T2, T3)>` - three value tuple `(T1, T2, T3)` for three values.
- etc.

- The same goes for the `IAsyncEnumerable<T>` type.

- This allows for a convenient [tuple deconstruction](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples#tuple-assignment-and-deconstruction), for example:

```csharp
using Norm;

foreach(var (title, description, year) in 
        connection.Read<string, string, int>("select title, description, release_year from film"))
{
    Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year)
}
```

- Tuples can also be named tuples:

```csharp
using Norm;

foreach(var tuple in 
        connection.Read<(string title, string description, int year)>("select title, description, release_year from film"))
{
    Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.title, tuple.description, tuple.year)
}
```

- We can also return multiple named tuples. In that case, each named tuple is a new tuple:

```csharp
using Norm;

// deconstruction of named tuples
foreach (var (actor, film) in connection.Read<(int id, string name), (int id, string name)>(@"
    select 
        actor_id, first_name || ' ' || last_name, 
        film_id, title
    from 
        actor
        join film_actor using (actor_id)
        join film using (film_id)
    limit 3"))
{
    WriteLine("Actor: {0}-{1}, Film: {1}-{2}", actor.id, actor.name, film.id, film.name);
}
```

- That same approach applies when mapping to multiple class instances. Multiple values always yield a tuple:

```csharp
using Norm;

// deconstruction of class instances
foreach (var (actor, film) in connection.Read<Actor, Film>(@"
    select 
        actor_id, first_name || ' ' || last_name, 
        film_id, title
    from 
        actor
        join film_actor using (actor_id)
        join film using (film_id)
    limit 3"))
{
    WriteLine("Actor: {0}-{1}, Film: {1}-{2}", actor.Id, actor.Name, film.Id, film.Name);
}
```

---

### Global Settings

- It is possible to modify `Norm`'s default behavior for the entire application by setting the global settings. For example:

```csharp
using Norm;

NormOptions.Configure(options =>
{
    options.CommandTimeout = 60;
});
```

- This example sets a global option for all commands executed by `Norm` to have a timeout of 60 seconds.

> Important note: `NormOptions.Configure` call is **not thread-safe.**

- `NormOptions.Configure` call is intended to be executed **only once from the application's startup code**, typically `Startup.cs` or `Program.cs`, before any database command.
