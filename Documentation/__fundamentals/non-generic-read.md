---
title: Non-Generic Read
order: 2
nextUrl: /docs/fundamentals/read-mappings/
nextTitle: Read Mappings
prevUrl: /docs/fundamentals/read-method/
prevTitle: Read Method
---

### Non-Generic Read Method

As already stated, a non-generic version of `Read` extension method returns the enumerator over the **array** where each element is **the name and value tuple**:

```csharp
IEnumerable<(string name, object value)[]> Read(string command);
```

Where:

- `string name` is the original **database column name**.
- `object value` is the **column value** as a root [object type](https://learn.microsoft.com/en-us/dotnet/api/system.object?view=net-7.0).

Obviously, the mapping is still not implemented in this method. For example, if we wanted to build a dictionary where key is database `id` and the value second database value, we would have to map it manually:

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

The most common use of a non-generic version is in combination with [`Any`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any) method. It can be used to see does the record exists.

```csharp
// does film_id=111 exists?
connection
    .Read("select 1 from film where film_id = 111")
    .Any();
```

This version of the `Read` method is suitable for the implementation of a custom mapping mechanism as an extension method for `IEnumerable<(string name, object value)[]>` type. Theoretically, it would look something like this:

```csharp
//
// pseudo
//
public static IEnumerable<T> Map<T>(this IEnumerable<(string name, object value)[]> tuples)
{
    foreach (var t in tuples)
    {
        yield return MapInstance(t);
    }
}
```

There is already a powerful and versatile mapping mechanism built into the library. Use the generic versions of the [`Read` method to map to .NET types.](/docs/fundamentals/read-mappings/)
