---
title: Mappings
order: 1
nextUrl: /docs/fundamentals/advanced-mappings/
nextTitle: Advanced Mappings
prevUrl: /docs/getting-started/basic-concepts/
prevTitle: Basic Concepts
---

## Read Method Extension

Mappings into the `.NET` types and structures are achieved exclusively by using **`Read` method extension.**

This method extension has a couple of different versions for the different mapping scenarios:

### Non-generic version

A version without generic parameters. It has the following basic signature: 

```csharp
IEnumerable<(string name, object value)[]> Read(string command);
```

The method creates and returns the enumerator over the array where each element is the name and value tuple: **`(string name, object value)[]`.**

The name is an original **database column name**, and the value is **the column value** as a root [object type](https://learn.microsoft.com/en-us/dotnet/api/system.object?view=net-7.0).

### Single generic parameter: 

Single generic parameter `Read` method has the following basic signature: 

```csharp
IEnumerable<T> Read<T>(string command);
```

The method creates and returns the enumerator with a single type declared by the generic type parameter.

### Multiple generic parameters: 
  
Multiple generic parameters `Read` method will create and return the enumerator of a **single tuple** with values declared by the generic type parameters.

Up to 12 generic parameters are supported. Here are the basic signatures:

```csharp
IEnumerable<(T1, T2)> Read<T1, T2>(string command);
IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command);
IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command);
IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command);
IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command);
//
// ... up to 12 generic parameters max
//
IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string command);
```

Since these methods return the enumerator of tuples defined by your generic parameters, we can use a tuple deconstruction:

```csharp
using Norm;

foreach(var (title, description, year) in 
        connection.Read<string, string, int>("select title, description, release_year from film"))
{
    Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year)
}
```

## Mapping To .NET types

There are also a few different types of mappings To .NET types, such as:

- Name and value tuples (basic, non-generic version)
- Single value types (`int`, `string`, `DateTime`, `Guid`, etc.)
- Named tuples (`(int id, string name)` for example)
- New instances of complex types (classes, records, etc.)
- Existing instances of complex types 
- Anonymous types mapping

### Name and Value Tuples

A non-generic version of `Read` extension method returns the enumerator over the array where each element is the name and value tuple:

```csharp
IEnumerable<(string name, object value)[]> Read(string command);
```

This version can be helpful in different scenarios. 

For example, it is convenient to easily build a dictionary where the dictionary is the database `id` and the value is some other database value:

```csharp
var dict = connection
    .Read("select film_id, title from film")
    .ToDictionary(
        tuples => (int)tuples.First().value,
        tuples => tuples.Last().value?.ToString());
```

The example above creates a dictionary where the key is `film_id` from the table `film`, and the value is `title` for that `id`.

Since `Read` method returns the iterator, `ToDictionary` method iterates the data. 

That means that **multiple iterations are avoided.**

However, this version mapping is not yet a real mapping. First value is mapped to `int` with `(int)tuples.First().value` and second version convrted manually to `string` with `tuples.Last().value?.ToString()`.

Most common use of non-generic version is in combination with [`Any`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any) method - to quickly check does the record exists.

Since there is no actual mapping - this is the fastest way to determine does any element of a sequence exist and, consequently, does the provided query returns any data:

```csharp
// does film_id=111 exists?
connection.Read("select 1 from film where film_id=111").Any();
```

### Single Value Types

Single value types are types returned by single value from database. 

In `.NET` they are all [value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) such as:
- [Integral types (`int`, `short`, `long`, etc)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types)
- [Floating-point numeric types (`float`, `double`, `decimal`)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)
- `bool`
- `char`

Plus some basic reference types such as `string`, `DateTime`, `Timespan`, `Guid`, etc.

In short, anything implemnted by database provider that is mapped to a field value.

Provide a single value type as generic parameter:

```csharp
var count = connection.Read<int>("select count(*) from actor").Single();
```

Multiple single value types are mapped **by position** only and returned as tuple:

```csharp
public static void PrintTuples(NpgsqlConnection connection)
{
    // tuples mapping
    foreach (var tuple in 
        connection.Read<string, string, int>(@"
            select title, description, release_year 
            from film
        "))
    {
        Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.Item1, tuple.Item2, tuple.Item3);
    }
}
```

Tuples can be deconstructed:

```csharp
public static void PrintDeconstructedTuples(NpgsqlConnection connection)
{
    // tuples deconstruction
    foreach (var (title, description, year) in 
        connection.Read<string, string, int>(@"
            select title, description, release_year 
            from film
        "))
    {
        Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year);
    }
}
```

This makes a bit easier to build a dictionary from example above:

```csharp
var dict = connection
    .Read<int, string>("select film_id, title from film")
    .ToDictionary(
        tuple => tuple.Item1,
        tuple => tuple.Item2);
```

**Important:** 
**Single values are always mapped by position.**

### Named Tuples

Generic type parameters can also be a **named tuples of single value types**.

That means that instead of `Read<string, string, int>` we can used named tuple like this `Read<(string title, string description, int year)>`. Example:

```csharp
public static void PrintNamedTuples(NpgsqlConnection connection)
{
    // named tuples
    foreach (var tuple in
        connection
            .Read<(string title, string description, int year)>(@"
            select title, description, release_year 
            from film
        "))
    {
        Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.title, tuple.description, tuple.year);
    }
}
```

And now, building a dictionary from example above is even easier:

```csharp
var dict = connection
    .Read<(int id, string name)>("select film_id, title from film")
    .ToDictionary(
        tuple => tuple.id,
        tuple => tuple.name);
```

All modern IDE tools (Visual Studio, Visual Studio Code, Raider, etc) will provide auto-complete functionality for named tuples which this approach even easier.

And since generic `Read` method accepts up to 12 generic parameters, we can even map to multiple named tuples:

```csharp
foreach (var (actor, film) in
    connection.Read<(int id, string name), (int id, string name)>(@"
        select 
            actor_id, first_name || ' ' || last_name, film_id, title
        from 
            actor
            join film_actor using (actor_id)
            join film using (film_id)"))
{
    Console.WriteLine("Actor: {0}-{1}, Film: {1}-{2}", actor.id, actor.name, film.id, film.name);
}
```

### New Instances Complex Types

### Existing Instances 

### Anonymous Types