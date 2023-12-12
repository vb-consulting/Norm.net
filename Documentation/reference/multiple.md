---
title: Multiple Mappings
order: 5
nextUrl: 
nextTitle: 
prevUrl: /norm.net/docs/reference/read/
prevTitle: Read Mappings
---

# Multiple Commands

- There's built-in support for working with multiple mappings in a single batch.

- Extensions and methods `Multiple` (in all versions) accept multiple commands separated by the **semicolon character (`;`)** - and will return a disposable instance used for reading and mapping each command:

```csharp
// Extensions
public static NormMultipleBatch Multiple(this DbConnection connection, string command, object parameters = null);
public static NormMultipleBatch MultipleFormat(this DbConnection connection, FormattableString command, object parameters = null);
// Sync Extensions
public static ValueTask<NormMultipleBatch> MultipleAsync(this DbConnection connection, string command, object parameters = null);
public static ValueTask<NormMultipleBatch> MultipleFormatAsync(this DbConnection connection, FormattableString command, object parameters = null);

// Methods
public NormMultipleBatch Multiple(string command, object parameters = null);
public NormMultipleBatch MultipleFormat(FormattableString command, object parameters = null);
// Async Extensions
public async ValueTask<NormMultipleBatch> MultipleAsync(string command, object parameters = null);
public async ValueTask<NormMultipleBatch> MultipleFormatAsync(FormattableString command, object parameters = null);
```

## NormMultipleBatch

- `NormMultipleBatch` is a disposable instance used for reading multiple batch commands.

- It can't be created directly, the new instance is created and returned by extensions and methods `Multiple` (in all versions).

- Methods:

### Next and NextAsync

```csharp
// Sync
public bool Next();
// Async
public async ValueTask<bool> NextAsync();
```

- Advances the reader to the next result when reading the results of a batch of statements.

- Returns true if there are more result sets; otherwise, false.

### Read and ReadAsync

```csharp
// Sync
public IEnumerable<(string name, object value)[]> Read();
public IEnumerable<T> Read<T>();
public IEnumerable<(T1, T2)> Read<T1, T2>();
//.. up to 12 generic parameters
public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
// Async
public IAsyncEnumerable<(string name, object value)[]> ReadAsync();
public IAsyncEnumerable<T> ReadAsync<T>();
public IAsyncEnumerable<(T1, T2)> ReadAsync<T1, T2>();
//.. up to 12 generic parameters
public IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> ReadAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
```

- These methods will read and map results for the current step.

- For more information on read mappings, see the [read mappings section](/norm.net/docs/reference/read/).

## Examples

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

using var multiple = connection.Multiple(@"
    select actor_id, first_name || ' ' || last_name as name from actor limit 3;
    select film_id, title from film limit 3;");

bool next;

var actors = multiple.Read<ActorDto>();
foreach (var actor in actors) // writes first three actors
{
    WriteLine("Actor: {0}-{1}", actor.ActorId, actor.Name);
}

next = multiple.Next(); // next is true

var films = multiple.Read<FilmDto>(); // writes first three films
foreach (var film in films)
{
    WriteLine("Film: {0}-{1}", film.FilmId, film.Title);
}

next = multiple.Next(); // next is now false
```
