---
title: Read Method
order: 2
nextUrl: /docs/fundamentals/non-generic-read/
nextTitle: Non-Generic Read Method
prevUrl: /docs/getting-started/basic-concepts/
prevTitle: Basic Concepts
---

# Read Method Extension

All mappings to the `.NET` types and structures are achieved by using the **`Read` method extension.**

This method has different versions based on number of generic parameters:

### Non-generic version

A version without generic parameters. 

It has the following basic signature: 

```csharp
IEnumerable<(string name, object value)[]> Read(string command);
```

The method creates and returns **the enumerator** over **the array** where each element is **the name and value tuple**: **`(string name, object value)[]`.**

- `string name` is the original **database column name**.
- `object value` is the **column value** as a root [object type](https://learn.microsoft.com/en-us/dotnet/api/system.object?view=net-7.0).

[See more info on non-generic read method.](/docs/fundamentals/non-generic-read/)

### Single generic parameter 

Single generic parameter `Read` method has the following basic signature: 

```csharp
IEnumerable<T> Read<T>(string command);
```

This method creates and returns **the enumerator** over a **single value** declared by the generic type parameter:

```csharp
var count = connection
    .Read<int>("select count(*) from actor")
    .Single();

// ...

foreach (var title in connection.Read<string>("select title from film"))
{
    WriteLine("Title: {0}", title, description, year);
}
```

[See more info on Read Mappings for Single Value Types.](/docs/fundamentals/read-mappings/#single-value-types)

### Multiple generic parameters: 
  
Multiple generic parameters `Read` method will create and return the enumerator of a **single tuple** with values declared by the generic type parameters.

Up to 12 generic parameters are supported. Here are the basic signatures:

```csharp
IEnumerable<(T1, T2)> Read<T1, T2>(string command);
IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command);
IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command);
IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command);
IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command);
IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command);
IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command);
IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string command);
IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string command);
IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string command);
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

[See more info on Read Mappings.](/docs/fundamentals/read-mappings/)