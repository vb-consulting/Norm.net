---
title: Read Mappings
order: 3
nextUrl: /docs/fundamentals/advanced-mappings/
nextTitle: Advanced Mappings
prevUrl: /docs/fundamentals/non-generic-read/
prevTitle: Non-Generic Read Method
---

### Read Mappings

There are also a few different types of mappings To .NET types, such as:

- Name and value tuples (basic, non-generic version)
- Single value types (`int`, `string`, `DateTime`, `Guid`, etc.)
- Named tuples (`(int id, string name)` for example)
- New instances of complex types (classes, records, etc.)
- Existing instances of complex types 
- Anonymous types mapping


### Single-Value Types

Single-value types are types returned by single-value from the database. 

In `.NET` they are all [value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) such as:
- [Integral types (`int`, `short`, `long`, etc)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types)
- [Floating-point numeric types (`float`, `double`, `decimal`)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)
- `bool`
- `char`

Plus, some basic reference types like `string`, `DateTime`, `Timespan`, `Guid`, etc.

In short - anything implemented by the database provider.

Provide a single-value type as a generic parameter:

```csharp
var count = connection.Read<int>("select count(*) from actor").Single();
```

Multiple single value types are mapped **by position only** and returned as a tuple:

```csharp
public static void PrintTuples(DbConnection connection)
{
    // tuples mapping
    foreach (var tuple in 
        connection.Read<string, string, int>(@"
            select title, description, release_year 
            from film
        "))
    {
        Console.WriteLine(
            "Title: {0}, Description: {1}, Year: {2}", 
            tuple.Item1, tuple.Item2, tuple.Item3);
    }
}
```

Tuples can be **deconstructed to values**:

```csharp
public static void PrintDeconstructedTuples(DbConnection connection)
{
    // tuples deconstruction
    foreach (var (title, description, year) in 
        connection.Read<string, string, int>(@"
            select title, description, release_year 
            from film
        "))
    {
        Console.WriteLine(
            "Title: {0}, Description: {1}, Year: {2}", 
            title, description, year);
    }
}
```

This makes it a bit easier to build a dictionary from the example above:

```csharp
var dict = connection
    .Read<int, string>("select film_id, title from film")
    .ToDictionary(
        tuple => tuple.Item1,
        tuple => tuple.Item2);
```

Again, **multiple iterations are avoided.**

**Important:** 
**Single values are always mapped by position.**

### Named Tuples

Generic type parameters can also be a **named tuple of single value types**.

That means that instead of `Read<string, string, int>(...)` - we can use named tuples like this: `Read<(string title, string description, int year)>(...)`. Example:

```csharp
public static void PrintNamedTuples(DbConnection connection)
{
    // named tuples
    foreach (var tuple in
        connection
            .Read<(string title, string description, int year)>(@"
            select title, description, release_year 
            from film
        "))
    {
        Console.WriteLine(
            "Title: {0}, Description: {1}, Year: {2}", 
            tuple.title, tuple.description, tuple.year);
    }
}
```

And now, building a dictionary from the example above is even easier:

```csharp
var dict = connection
    .Read<(int id, string name)>("select film_id, title from film")
    .ToDictionary(
        tuple => tuple.id,
        tuple => tuple.name);
```

Again, **multiple iterations are avoided.**

All modern IDE tools (Visual Studio, Visual Studio Code, Raider, etc.) will provide auto-complete functionality for named tuples, which makes this approach even easier.

And since the generic `Read` method accepts up to 12 generic parameters, we can even map to multiple named tuples:

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
    Console.WriteLine(
        "Actor: {0}-{1}, Film: {1}-{2}", 
        actor.id, actor.name, film.id, film.name);
}
```

### New Instances Complex Types

### Existing Instances 

### Anonymous Types