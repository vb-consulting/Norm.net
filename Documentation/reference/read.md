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
  
  1) > **Connections extension** and the **instance method** version. These are identical in functionality and exist only to implement fluid syntax.
   
  2) > [A non-generic version](#non-generic-version) that yields a **name and value tuple** array for each row. 

  3) > [Single generic type version](#single-generic-type-version) that yields **the same type as supplied** by the generic parameter for each row.
   
  4) > [Multiple generic types](#multiple-generic-types) that yields tuples **based on generic parameters** for each row.

  5) > [Instance blueprint version](#instance-blueprint-version) that yields new instances **based on the blueprint instance** for each row.

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
  - `string name` is the column name
  - `object value` is the column value

- This is a very basic mapping approach. 

- Example of building a data dictionary:

```csharp
// dictionary where key is film_id and value is file title
var dict = connection
    .Read("select film_id, title from film limit 3")
    .ToDictionary(
        tuples => (int)tuples.First().value,
        tuples => tuples.Last().value?.ToString());
```

- The more common example of usage is checking if the query returns any records.

- Since this version doesn't implement any real mapping, this would be the most efficient version. 

- Example:

```csharp
var exists = connection
    .Read("select 1 from film where film_id = @id", 999)
    .Any();
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
var count = connection
    .Read<int>("select count(*) from actor")
    .Single();
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

### Multiple Generic Types

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

- Multiple generic types support multiple mappings and will return results in a value tuple.

- Up to **12 multiple mappings maximum** are supported.

- Examples:

#### 1) Simple types

```csharp
// tuples mapping
foreach (var tuple in connection.Read<string, string, int>(
    "select title, description, release_year from film limit 3"))
{
    Console.WriteLine("Title: {0}, Description: {1}, Year: {2}", tuple.Item1, tuple.Item2, tuple.Item3);
}
```
```csharp
// tuples deconstruction
foreach (var (title, description, year) in connection.Read<string, string, int>(
    "select title, description, release_year from film limit 3"))
{
    WriteLine("Title: {0}, Description: {1}, Year: {2}", title, description, year);
}
```
```csharp
// dictionary where key is film_id and value is file title
var dict = connection
    .Read<int, string>("select film_id, title from film limit 3")
    .ToDictionary(
        tuple => tuple.Item1,
        tuple => tuple.Item2);
```

- In these examples, simple values are mapped **by position** into value tuples.

#### 2) Tuple types

```csharp
// deconstruction of named tuples
foreach (var (actor, film) in connection.Read<
    (int id, string name),
    (int id, string name)>(@"
    select 
        actor_id, 
        first_name || ' ' || last_name, 
        film_id, 
        title
    from 
        actor
        join film_actor using (actor_id)
        join film using (film_id)
    limit 3"))
{
    WriteLine("Actor: {0}-{1}, Film: {2}-{3}", actor.id, actor.name, film.id, film.name);
}
```

- In these examples, named tuples are mapped **by position** into value tuples. Names are irrelevant.

#### 3) Complex Instance types

```csharp
public class FilmDto
{
    public int FilmId { get; set; }
    public string Title { get; set; }
}

public class ActorDto
{
    public int ActorId { get; set; }
    public string Name { get; set; }
}

// deconstruction of class instances
foreach (var (actor, film) in connection.Read<ActorDto, FilmDto>(@"
    select 
        actor_id, 
        first_name || ' ' || last_name as name, 
        film_id, 
        title
    from 
        actor
        join film_actor using (actor_id)
        join film using (film_id)
    limit 3"))
{
    WriteLine("Actor: {0}-{1}, Film: {2}-{3}", actor.ActorId, actor.Name, film.FilmId, film.Title);
}
```

- In these examples, class instances are **mapped by name**. 

- By default, the name mapper will ignore cases and will ignore these two characters: `@`, `_`. This enables **snake-case case insensitive mapping** by default. To override this behavior, use the [`KeepOriginalNames`](/norm.net/docs/reference/options/#keeporiginalnames) or the [`NameParserCallback`](/norm.net/docs/reference/options/#nameparsercallback) option.

- By default, only public properties are mapped. To change this behavior set the [`MapPrivateSetters` option](/norm.net/docs/reference/options/#mapprivatesetters) to true. 

- For multiple instance mappings by name, if properties with the same name exist - the first instance is server first.

#### Mixing Different Mapping Types

- Mixing different mapping types is not allowed and will raise the `System.InvalidCastException` exception.

- For example:
  - Simple types with class instances: `connection.Read<int, string, FilmDto>`.
  - Named tuples with class instances: `connection.Read<(int id, string name), FilmDto>`
  - Simple types with named tuples: `connection.Read<int, string, (int id, string name)`

- Will raise the `System.InvalidCastException` exception.

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

- It is possible to supply existing instances that will be used as a blueprint for creating new instances.

- This overload version receives the blueprint instance as the first parameter.

- Generic type can be omitted on usage.

- This approach is commonly used to map into [anonymous type instances](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types).

- It can also be used when we have some existing instances and we can supply that instance as the first parameter.

- This approach will follow the same rules when mapping class instances described above (mapping by name).

- Examples:

#### Anonymous Type Instances Mapping

```csharp
var film = connection
    .Read(new
    {
        filmId = default(int),
        title = default(string),
        releaseYear = default(int),
        rentalRate = default(decimal)
    }, @"
        select film_id, title, release_year, rental_rate 
        from film
        limit 1")
    .Single();

WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
    film.filmId, film.title, film.releaseYear, film.rentalRate);
```

- Note that since this anonymous instance is supplied as the first parameter and is later used to create new ones of the same type - property values aren't important, only type is.

- Therefore, the easiest way to declare type is by using the `default` keyword:

```csharp
new
{
    filmId = default(int),
    title = default(string),
    releaseYear = default(int),
    rentalRate = default(decimal)
}
```

- In practice this can also be this:

```csharp
new
{
    filmId = 1,
    title = "",
    releaseYear = 1,
    rentalRate = 1m
}
```

#### Existing Class Instances Mapping

- This approach can be used with normal class instances as well:

```csharp
public class Film
{
    public int FilmId { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public decimal RentalRate { get; set; }
}

var instance = new Film();

var film = connection
    .Read(instance, @"
        select film_id, title, release_year, rental_rate 
        from film
        limit 1")
    .Single();

WriteLine("Film: {0}-{1} Year: {2}, Rate: {3}",
    film.FilmId, film.Title, film.ReleaseYear, film.RentalRate);
```

---

### Arrays And Enums

- Mapping to special types like arrays and enums (and the combination of two) - is possible as well, with some limitations.

- Note that, not all database providers support array types, this feature is commonly used with the PostgreSQL database systems.

- The following table describes those limitations:

| Mapping Type | Complex Instances | Simple Values | Named Tuples |
| :----------- | :---------------: | :-----------: | :----------: |
| **array** (not null) | YES | YES | YES |
| **array** (nullable) | NO | YES | NO |
| **text → enum** (not null) | YES | YES | NO |
| **int → enum** (not null) | YES | YES | NO |
| **text → enum** (nullable) | YES | YES | NO |
| **int → enum** (nullable) | YES | YES | NO |
| **text array → enum array** (not null) | YES | NO | NO |
| **int array → enum array** (not null) | YES | NO | NO |
| **text array → enum array** (nullable)| NO | NO | NO |
| **int array → enum array** (nullable) | NO | NO | NO |


- Simple values arrays and enums example:

```csharp
public enum MyEnum { Value1, Value2, Value3 }

var tuple = connection
    .Read<int[], int?[], MyEnum, MyEnum, MyEnum?, MyEnum?> (@"
        select
            array[1,2,3] as array_not_null,
            array[1,null,3] as array_null,
            'Value1' as text_enum_not_null,
            0 as int_enum_not_null,
            null::text as text_enum_null,
            null::int as int_enum_null")
    .Single();
```

- Named tuple array example:

```csharp
var tuple = connection
    .Read<(int[] intArray, string[] strArray)> ("select array[1,2,3], array['a','b','c']")
    .Single();
```

- Complex instance type arrays and enums example:

```csharp
public enum MyEnum { Value1, Value2, Value3 }

class MyComplexType
{
    public int[] ArrayNotNull { get; set; }
    public MyEnum TextEnumNotNull { get; set; }
    public MyEnum IntEnumNotNull { get; set; }
    public MyEnum? TextEnumNull { get; set; }
    public MyEnum? IntEnumNull { get; set; }
    public MyEnum[] TextEnumArray { get; set; }
    public MyEnum[] IntEnumArray { get; set; }
}

var instance = connection
    .Read<MyComplexType>(@"
    select 
        array[1,2,3] as array_not_null,
        'Value1' as text_enum_not_null,
        0 as int_enum_not_null,
        null::text as text_enum_null,
        null::int as int_enum_null,
        array['Value1', 'Value2', 'Value2'] as text_enum_array,
        array[0,1,2] as int_enum_array")
    .Single();
```

---

### DbReader Callback

- To be able to map efficiently any type in any combination, there is a special extension method called [`WithReaderCallback`](/norm.net/docs/reference/methods/#withreadercallback) that allows that.

- Example callback that allows mapping of enums in the named tuples:

```csharp
public enum TestEnum { Value1, Value2, Value3 }

var result = connection
    .WithReaderCallback(r => Enum.Parse<TestEnum>(r.Reader.GetFieldValue<string>(r.Ordinal)))
    .Read<(TestEnum Enum1, TestEnum Enum2)>(@"
        select *
        from (
        values 
            ('Value1', 'Value3'),
            ('Value2', 'Value2'),
            ('Value3', 'Value1')
        ) t(Enum1, Enum2)")
    .ToArray();
```

- There is also [`DbReaderCallback` a global option](/norm.net/docs/reference/options/#dbreadercallback) that can be used to set global callback.

- Example of configuration that maps PostgreSQL JSON type to the `JsonObject`:


```csharp
// configuration
NormOptions.Configure(options =>
{
    options.DbReaderCallback = arg =>
    {
        // check if the column is of type json
        if (string.Equals(arg.Reader.GetDataTypeName(arg.Ordinal), "json", StringComparison.InvariantCultureIgnoreCase))
        {
            if (arg.Reader.IsDBNull(arg.Ordinal))
            {
                // resolve as null
                return DBNull.Value; 
            }
            // Create and return a JsonObject from the string value
            return JsonNode.Parse(arg.Reader.GetString(arg.Ordinal))?.AsObject();
        }
        // fall-back to default behavior
        return null;
    };
});

// class with JsonObject object
private class JsonTest
{
    public string I { get; set; }
    public JsonObject J { get; set; }
}

// mapping of the JsonObject object
var instance = connection
    .WithReaderCallback(ReaderCallback)
    .Read<JsonTest>("select '{\"a\": 1}'::text as i, '{\"a\": 1}'::json as j")
    .Single();
```

