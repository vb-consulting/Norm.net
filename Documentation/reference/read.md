---
title: Read Mappings
order: 4
nextUrl: /norm.net/docs/reference/multiple/
nextTitle: Multiple Mappings
prevUrl: /norm.net/docs/reference/parameters/
prevTitle: Parameters
---

## Read Mappings

- Read methods implement a complex mapping system that enables you to map to anything from simple types to tuples and object instances.

- There are four groups: `Read`, `ReadAsync`, `ReadFormat` and `ReadFormatAsync`.

- Each of these methods has:
  1) **Connections extension** and the **instance method** version. These are identical in functionality and exist only to implement fluid syntax.
  2) [A non-generic version](#non-generic-version) that yields a **name and value tuple** array for each row. 
  3) [Single generic type version](#single-generic-type-version) that yields **the same type as supplied** by the generic parameter for each row.
  4) [Multiple generic types](#multiple-generic-types) that yields tuples **based on generic parameters** for each row.
  5) [Instance blueprint version](#instance-blueprint-version) that yields new instances **based on the blueprint instance** for each row.

---

### Non-generic Version

```csharp
// Extension
public static IEnumerable<(string name, object value)[]> Read(this DbConnection connection, 
    string command, 
    object parameters = null);
// Method
public IEnumerable<(string name, object value)[]> Read(
    string command, 
    object parameters = null);
```

- Non generic version yields a name and value tuple array for each row: `(string name, object value)[]`.

- The tuple array represents a row returned from the database where:
  - `string name` is the column name.
  - `object value` is the column value.

- This is a very basic mapping approach. 

- Example of building a data dictionary:

```csharp
// dictionary where key is film_id and value is file title
var dict = connection
    .Read<int, string>("select film_id, title from film limit 3")
    .ToDictionary(
        tuple => tuple.Item1,
        tuple => tuple.Item2);
```

- The more common example of usage is checking if the query returns any records.

- Since this version doesn't implement any real mapping, this would be the most efficient version. 

- Example:

```csharp
var exists = connection.Read("select 1 from film where film_id = @id", 999).Any();
```

---

### Single Generic Type Version

```csharp
// Extension
public static IEnumerable<T> Read<T>(this DbConnection connection, 
    string command, 
    object parameters = null);
// Method
public IEnumerable<T> Read<T>(
    string command, 
    object parameters = null);
```

- Generic type can be either:

#### 1) Simple types

- Simple types such as `int`, `long`, `short`, `double`, `single`, `string`, `boolean`, `DateTime`, `TimeSpan`, `DateTimeOffset`, `Guid`, etc.

Example:

```csharp
var count = connection.Read<int>("select count(*) from actor").Single();
```

#### 2) Tuple types

- Tuple types can be named or unnamed.

- Unnamed tuple example:

```csharp
// dictionary where key is film_id and value is file title
var dict = connection
    .Read<(int, string)>("select film_id, title from film limit 3")
    .ToDictionary(
        tuple => tuple.Item1,
        tuple => tuple.Item2);
```

- Named tuple example:

```csharp
// dictionary where key is film_id and value is file title
var dict = connection
    .Read<(int id, string name)>("select film_id, title from film limit 3")
    .ToDictionary(
        tuple => tuple.id,
        tuple => tuple.name);
```

- > Note: all tuples are **mapped by position only**.

#### 3) Complex Instance types

-  `class`, `record`, or any other complex instance type that supports properties.

- Example: 

```csharp
public class Film
{
    public int FilmId { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public decimal RentalRate { get; set; }
}

var film = connection
    .Read<Film>(@"
        select film_id, title, release_year, rental_rate 
        from film
        limit 1")
    .Single();
```

- All existing fields are **mapped by name.**

- By default, the name mapper will ignore cases and will ignore these two characters: `@`, `_`. This enables **snake-case case insensitive mapping** by default. To override this behavior, use the [`KeepOriginalNames`](/norm.net/docs/reference/options/#keeporiginalnames) or the [`NameParserCallback`](/norm.net/docs/reference/options/#nameparsercallback) option.

- By default, only public properties are mapped. To change this behavior set the [`MapPrivateSetters` option](/norm.net/docs/reference/options/#mapprivatesetters) to true. 

---

### Multiple generic types

```csharp
// Extensions
public static IEnumerable<(T1, T2)> Read<T1, T2>(this DbConnection connection, 
    string command, 
    object parameters = null);
public static IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(this DbConnection connection, 
    string command, 
    object parameters = null);
// up to 12 generic parameters ...
public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this DbConnection connection, 
    string command, 
    object parameters = null);

// Methods
public IEnumerable<(T1, T2)> Read<T1, T2>(
    string command, 
    object parameters = null);
public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(
    string command, 
    object parameters = null);
// up to 12 generic parameters ...
public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
    string command, 
    object parameters = null);
```

---

### Instance Blueprint Version

```csharp
// Extension
public static IEnumerable<T> Read<T>(this DbConnection connection, 
    T bluePrintInstance,
    string command, 
    object parameters = null);
// Method
public IEnumerable<T> Read<T>(
    T bluePrintInstance,
    string command, 
    object parameters = null);
```
