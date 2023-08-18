---
title: Read Method
order: 2
nextUrl: /docs/fundamentals/non-generic-read/
nextTitle: Non-Generic Read Method
prevUrl: /docs/getting-started/basic-concepts/
prevTitle: Basic Concepts
---

## Read Method Extension

All mappings to the `.NET` types and structures are achieved by using the **`Read` method extension.**

There are a couple of different versions of this method:

### Non-generic version

A version without generic parameters. It has the following basic signature: 

```csharp
IEnumerable<(string name, object value)[]> Read(string command);
```

The method creates and returns **the enumerator** over **the array** where each element is **the name and value tuple**: **`(string name, object value)[]`.**

- `string name` is the original **database column name**.
- `object value` is the **column value** as a root [object type](https://learn.microsoft.com/en-us/dotnet/api/system.object?view=net-7.0).

[See more info on this version on this page.](/docs/fundamentals/non-generic-read/)

### Single generic parameter: 

Single generic parameter `Read` method has the following basic signature: 

```csharp
IEnumerable<T> Read<T>(string command);
```

This method creates and returns **the enumerator** with a single type declared by the generic type parameter.

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

Since these methods return the **enumerator of tuples** defined by your generic parameters, we can use a **tuple deconstruction**. For Example:

```csharp
using Norm;

foreach(var (title, description, year) in 
    connection.Read<string, string, int>(@"
        select title, description, release_year 
        from film"))
{
    WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year)
}
```

[See more info on mapping to .NET types on this page.](/docs/fundamentals/read-method/)