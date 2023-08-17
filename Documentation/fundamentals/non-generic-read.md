---
title: Non-Generic Read Method
order: 2
nextUrl: /docs/fundamentals/read-mappings/
nextTitle: Read Mappings
prevUrl: /docs/fundamentals/read-method/
prevTitle: Read Method
---

### Non-Generic Read Method

A non-generic version of `Read` extension method returns the enumerator over the **array** where each element is **the name and value tuple**:

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

However, this version mapping is not yet a real mapping. First value is mapped to `int` with `(int)tuples.First().value` and second version converted manually to `string` with `tuples.Last().value?.ToString()`.

The most common use of a non-generic version is in combination with [`Any`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any) method. It can be used to see does the record exists.

This is the fastest way to determine does any element of a sequence exist. And, consequently, does the provided query returns any data:

```csharp
// does film_id=111 exists?
connection
    .Read("select 1 from film where film_id = 111")
    .Any();
```




