---
title: Read Mappings
order: 3
nextUrl: /docs/fundamentals/advanced-mappings/
nextTitle: Advanced Mappings
prevUrl: /docs/fundamentals/non-generic-read/
prevTitle: Non-Generic Read Method
---

## Read Mappings

Mapping to `.NET` types is achieved by using generic versions of the `Read` method. There are 12 versions of the generic versions, which means that you can map up to 12 types simultaneously. Those are:

```csharp
IEnumerable<T> Read<T>(string command);
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

There are four different types of mapping, depending on the target types:

1) Single-value types (`int`, `string`, `DateTime`, `Guid`, etc.)
2) Named tuples (`(int id, string name)` for example)
3) New instances of complex types (classes, records, etc.)
4) Existing instances of complex types (classes, records, etc.) and anonymous types mapping

### Single-Value Types

Single-value types are types returned by single-value from the database. 

In `.NET` they are all [value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types), such as:

- [Integral types (`int`, `short`, `long`, etc)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types)
- [Floating-point numeric types (`float`, `double`, `decimal`)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)
- `bool`
- `char`

Also, some of the basic reference types like `string`, `DateTime`, `Timespan`, `Guid`, etc.

In short - anything implemented by the database provider.

Provide a single-value type as a generic parameter:

```csharp
var count = connection
    .Read<int>("select count(*) from actor")
    .Single();
```

Multiple single-value types are mapped **by position only** and returned as a tuple:

```csharp
// tuples mapping
foreach (var tuple in connection.Read<string, string, int>(@"
    select title, description, release_year 
    from film"))
{
    WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.Item1, tuple.Item2, tuple.Item3);
}
```

Tuples can be **[deconstructed](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/deconstruct)**. Example:

```csharp
// tuples deconstruction
foreach (var (title, description, year) in connection.Read<string, string, int>(@"
    select title, description, release_year 
    from film"))
{
    WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year);
}
```

This makes an example of building a dictionary object where the dictionary key is the database id and value is the second field from a query a bit easier:

```csharp
var dict = connection
    .Read<int, string>("select film_id, title from film")
    .ToDictionary(
        tuple => tuple.Item1,   // key
        tuple => tuple.Item2);  // value
```

Since the result of the enumeration is an unnamed tuple, the first field of type `int` is named `Item1`, and the second field of type `string` is named `Item2`.

Again, since the `Read` method returns the iterator - **multiple iterations are avoided.**

> **Important:** 
> **Single-value types are always mapped by THE POSITION.**

That means in following expression `.Read<int, string>("select film_id, title from film")`:

-The  `film_id` is mapped to the `int` of `Item1` - because they are all at position 1.
- The `title` is mapped to the `string` of `Item2` - because they are all at position 2.

### Named Tuples

Generic type parameters can also be a **named tuple of single value types**.

That means that
- instead of: `Read<string, string, int>(...)`
- we can use: `Read<(string title, string description, int year)>(...)` 

Example:

```csharp
// named tuples
foreach (var tuple in connection.Read<(string title, string description, int year)>(@"
    select title, description, release_year 
    from film"))
{
    WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.title, tuple.description, tuple.year);
}
```

With this approach, we have a complete complex-type declaration with multiple fields with type and name - declared in place: `(string title, string description, int year)`.

And now, building a dictionary from the same example above (key is database `id`, and value is the second field) - is even easier:

```csharp
var dict = connection
    .Read<(int id, string name)>("select film_id, title from film")
    .ToDictionary(
        tuple => tuple.id,      // key
        tuple => tuple.name);   // value
```

Again, since the `Read` method returns the iterator - **multiple iterations are avoided.**

But this time, we have real, actual names like `id` and `name`, not just `Item1` and `Item2`. All modern IDE tools (Visual Studio, Visual Studio Code, Raider, etc.) will provide auto-complete functionality for named tuples.

And now, since the generic `Read` method accepts up to 12 generic parameters, we can even map to **multiple named tuples**:

```csharp
foreach (var (actor, film) in connection.Read<
    (int id, string name), 
    (int id, string name)>(@"
    select 
        actor_id, first_name || ' ' || last_name, 
        film_id, title
    from 
        actor
        join film_actor using (actor_id)
        join film using (film_id)"))
{
    WriteLine("Actor: {0}-{1}, Film: {1}-{2}", actor.id, actor.name, film.id, film.name);
}
```

Those two named tuples have identical structures with the same names: 
- `(int id, string name)`. 

And the actual query columns returned from the database are: 
- `actor_id`, `?column?`, `film_id` and `title`

That means that - again:

> **Important:** 
> **Named tuple types are always mapped by THE POSITION.**

### New Instances Complex Types

This is a classical mapping approach where the resulting query is mapped to **new instances of a `class` or a `record`. **

Example class:

```csharp
public class Film
{
    public int FilmId { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public decimal RentalRate { get; set; }
}
```

And we want to print the first film from the query:

```csharp
var film = connection
    .Read<Film>(@"
        select film_id, title, release_year, rental_rate
        from film 
        limit 1")
    .Single();

WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}", film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
```

Or, iterate through films:

```csharp
foreach (var film in connection.Read<Film>(@"
    select film_id, title, release_year, rental_rate 
    from film"))
{
    WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}", 
        film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
}
```

Important things to notice:

#### Mapping is by name

> All class or record instance mapping is by name.

That means that position is not important, and we can reverse the order of columns in the query, and we will end up with the same result.

```csharp
var film = connection
    .Read<Film>(@"
        select rental_rate, release_year, title, film_id
        from film 
        limit 1")
    .Single();

WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}", film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
```

We can also select all fields, and those that are not matched (by name) will be ignored:

```csharp
var film = connection
    .Read<Film>("select * from film limit 1")
    .Single();

WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}", film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
```

Also, if we had an extra property in a class - that is not matched by a query - that property is ignored:

```csharp
public class ExtraFilm : Film
{
    public string Extra { get; set; } = "not-mapped";
}

var film = connection
    .Read<ExtraFilm>("select * from film limit 1")
    .Single();

WriteLine("{0}", film.Extra); // Prints: not-mapped
```

#### The snake-case naming convention

> Mapping by name supports the **snake-case** naming convention.

In this example, we have a query column `film_id` and a property on a class `FilmId`. This property is mapped because:

- Mapping will lowercase all names to make the name-matching case ignorant.
- Mapping will remove all underscore `_` characters to support the [snake-case naming convention](https://en.wikipedia.org/wiki/Snake_case).

We can bypass this behavior by setting `KeepOriginalNames` to `true` in global options:

```csharp
// keep original names
NormOptions.Configure(options =>
{
    options.KeepOriginalNames = true;
});

var film = connection
    .Read<Film>("select * from film limit 1")
    .Single();

WriteLine("Film id: {0}", film.FilmId); // film id defaults to 0
```

So, in this case - `film_id` can no longer be matched by name. We would have to name our class property `film_id` or return `FilmId` from the query.

By default, all name mappings support snake-case naming - unless set otherwise in global options.

#### Public members only by default

> Only fields and properties that are public (have public setters) are mapped by default.

A property or field in a class or a record - must have a public setter (must have the ability to be set publicly) - to be mapped. Otherwise, it is ignored.

```csharp
public class NonPublicFilm
{
    public int FilmId { get; private set; } // not mapped and ignored
    public string Title { get; protected set; } // not mapped and ignored
    public int ReleaseYear { get; set; } // ignored
    public decimal RentalRate { get; set; } // mapped
}

var film = connection
    .Read<NonPublicFilm>("select * from film limit 1")
    .Single();

// film.FilmId has default value (not mapped)
// film.Title has default value (not mapped)
WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
    film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
```

However, this behavior can be changed with the global option `MapPrivateSetters`. Set this option to `true` to map all fields and properties, even if they are public or protected.

```csharp
// map private and protected members too
NormOptions.Configure(options =>
{
    options.MapPrivateSetters = true;
});

film = connection
    .Read<NonPublicFilm>("select * from film limit 1")
    .Single();

// film.FilmId is mapped properly
// film.Title is mapped properly
WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
    film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
```

### Existing Instances And Anonymous Types

