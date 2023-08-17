---
title: Read Method
order: 1
nextUrl: /docs/fundamentals/mappings/
nextTitle: Mappings
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
    Console.WriteLine(
        "Title: {0}, Description: {1}, Year: {2}", 
        title, description, year)
}
```