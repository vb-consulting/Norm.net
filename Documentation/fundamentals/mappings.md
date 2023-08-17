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

- Basic (non-generic version)
- Single value types (`int`, `string`, `DateTime`, `Guid`, etc.)
- Named tuples (`(int id, string name)` for example)
- New instances of complex types (classes, records, etc.)
- Existing instances of complex types 
- Anonymous types mapping

### Basic Mapping to Name and Value Tuples With Non-Generic Version

A non-generic version of `Read` extension method returns the enumerator over the array where each element is the name and value tuple:

```csharp
IEnumerable<(string name, object value)[]> Read(string command);
```

This version can be helpful in different scenarios. For example, it is convenient to easily build a dictionary where the dictionary is the database `id` and the value is some other database value:

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

When using a non-generic version, there is no actual mapping yet. That means that the correct value must be identified and casted manually to appropirate types:

- First value to `int`: `(int)tuples.First().value`
- Second (last) value to `string`: `tuples.Last().value?.ToString()`

Most common use of non-generic version is in combination with [`Any`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any) method.

Since there is no actual mapping, the fastes way to determines does any element of a sequence exist is to use `Any` without mapping:

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
        connection.Read<string, string, int>("select title, description, release_year from film limit 3"))
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
        connection.Read<string, string, int>("select title, description, release_year from film limit 3"))
    {
        Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year);
    }
}
```

**Important:** 
**Single values are always mapped by position.**

### Named Tuples

Generic type parameter

### New Instances Complex Types

### Existing Instances 

### Anonymous Types