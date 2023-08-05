---
title: Introduction
next: read-mapping.md
nextTitle: Mapping The Results
---

# Introduction

`Norm` is a Micro-ORM library for efficient data access in the `.NET` ecosystem (see the [compatibility table](/#compatibility).)

Micro-ORM library implements one-way database mapping only - from your database commands and queries to  `.NET` types and structures.

`Norm` is a **set of extensions** over the [`System.Data.Common.DbConnection`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection) object.

That means that if the connection implements `System.Data.Common.DbConnection` type - the database that implements that connection is supported. And that includes, for  example:

- [`SqlConnection`](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection) for SQL Server databases.
- [`NpgsqlConnection`](https://www.npgsql.org/doc/api/Npgsql.NpgsqlConnection.html) for PostgreSQL databases.
- [`SqliteConnection`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite.sqliteconnection) for SQLite databases.
- [`MySqlConnection`](https://mysqlconnector.net/api/mysqlconnector/mysqlconnectiontype/) for MySQL databases.
- etc

## Installation

### .NET Command-Line-Interface

Run this command from the command line to install the latest version:

```bash
dotnet add package Norm.net --version {version}
```

### Visual Studio Package Manager Console (PowerShell)

Run this command from the Visual Studio Package Manager Console (PowerShell) to install the latest version:

```powershell
NuGet\Install-Package Norm.net -Version {version}
```

### Package Reference

Add this line to your `csproj`` project file inside `<ItemGroup>` section to install the latest version:

```xml
<PackageReference Include="Norm.net" Version="{version}" />
```

### Script & Interactive

In your C# Interactive console or another scripting engine (such as `csrepl`, for example) - execute the following command to install the latest version:

```yaml
#r "nuget: Norm.net, {version}
```

## First Usage

### Add Using

Add `using Norm` directive to the `using` section of your source file or add `global using Norm` to use `Norm` in all of your source files:


```csharp
using Norm;

//... the rest of the source code ...

```

Or, for all source files in a project:

```csharp
global using Norm;

//... the rest of the source code ...

```

### Obtain Connection reference

Obtain a database connection reference, either by creating a new instance:


```csharp
using Norm;

using var connection = new NpgsqlConnection("Server=localhost;Database=dvdrental;Port=5433;User Id=postgres;Password=postgres;");

// ...

```

Or, pass it down as a method parameter:

```csharp
using Norm;

public int CountActors(NpgsqlConnection connection)
{
    //...
}
```

Or, get it from the database EF context:

```csharp
using Norm;

public int CountActors(MyDatabaseContext context)
{
    var connection = context.Database.GetDbConnection()
    //...
}
```

### Execute First Commands

Execute count on table `actor` and return the result:

```csharp
using Norm;
using System.Linq;

public int CountActors(NpgsqlConnection connection)
{
    return connection.Read<int>("select count(*) from actor").Single();
}
```

Or, perhaps, execute a command without returning a value:

```csharp
// delete inactive customers
connection.Execute("delete from customer where active = 0");
```

*Note: this documentation will use a sample database from a PostgreSQL public tutorial ([postgresqltutorial.com](https://www.postgresqltutorial.com/)). 
You can find detailed instructions on installing this sample database on [this page](https://www.postgresqltutorial.com/postgresql-getting-started/postgresql-sample-database/).*

## Basic Concepts

### Connection Extensions

There are two main extensions to the `System.Data.Common.DbConnection` type (plus another two for the `async` versions):

1) `Execute` - execute a command without returning any values.
2) `Read` - execute a command and return some values.

Both extensions will attempt to **open the underlying connection (if not already open)** - and initiate command execution.

### Fluid Syntax

There are also many other extensions to the `System.Data.Common.DbConnection` type that will do anything with the database but instead return the new `Norm` instance that implements the same methods as extensions on the `System.Data.Common.DbConnection` type.

That allows for a fluid syntax, for example:

```csharp
//
// Execute a stored procedure with command timoout 60 seconds
//
connection
    .AsProcedure()
    .WithTimeout(60)
    .Execute("delete_inactive_customers");
```

### Read Iterators

The `Read` extension method and all the overload versions of that method - will always return an instance of the [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators) of either [`IEnumerable`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerable) or [`Enumerable<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1) types.

This means that database values are **[yielded](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/yield)** as they are returned from the database.

For example, the following statements:

```csharp
using Norm;

var result1 = connection.Read<int>("select count(*) from actor");

var result2 = connection.Read("select title from film");
```

These two lines will not send or execute any commands to database. Instead, they will create an [iterator](https://learn.microsoft.com/en-us/dotnet/csharp/iterators) over the database connection, that will be executed when the actual iteration starts:

```csharp
using System.Linq;
using Norm;

// executes select count(*) from actor and retuns int
var count = result1.Single();

// executes select title from film and builds a list of film titles
var list = result2.Tolist();
```

Or combine `Linq` statements with the iterator returned from the Read method:

```csharp
using System.Linq;
using Norm;

// executes select count(*) from actor and retuns int
var count = connection.Read<int>("select count(*) from actor").Single();

// executes select title from film and builds a list of film titles
var list = connection.Read("select title from film").Tolist();
```

Or, for example, a standard `foreach` iteration without `Linq`:

```csharp
using System.Linq;
using Norm;

// executes select title from film and iterate titles
foreach(var title in connection.Read("select title from film"))
{
    // ...
}
```

As we can see, the actual execution is delayed until the first iteration. This approach allows us to build `Linq` expressions over the iterator, which are then executed only once per iteration, and there is no need for any additional buffering.

## Command Execution

For the command execution that doesn't return any values - there are two simple extension methods: `Execute` and `ExecuteAsync`. Since there are no results, mapping isn't involved, and therefore, these extension methods are simple.

Both of these extension methods will execute the command immediately (no delayed execution). And they have a couple of more default parameters (like parameters object) that we will discuss later.

### Execute

Executes synchronous command.

```csharp
using Norm;

public void DeleteInactiveCustomers(NpgsqlConnection connection)
{
    connection.Execute("delete from customer where active = 0");
}
```

### ExecuteAsync

Executes command and returns asynchronous task. 

```csharp
using System.Threading.Tasks;
using Norm;

public async Task DeleteInactiveCustomersAsync(NpgsqlConnection connection)
{
    await connection.ExecuteAsync("delete from customer where active = 0");
}
```