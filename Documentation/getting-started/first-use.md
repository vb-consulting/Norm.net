---
title: First Use
order: 3
nextUrl: /docs/getting-started/basic-concepts/
nextTitle: Basic Concepts
prevUrl: /docs/getting-started/installation/
prevTitle: Installation
---

# First Use

### Add Using

Add `using Norm` directive to the `using` section of your source file:


```csharp
using Norm;

//... the rest of the source code ...

```

Or, add `global using Norm` to use `Norm` in all of your source files (C# 10 or higher only):

```csharp
global using Norm;

//... the rest of the source code ...

```

### Connection Reference

Obtain a database connection reference, either by creating a new instance:

```csharp
using Norm;

using var connection = new NpgsqlConnection("Server=localhost;Database=dvdrental;Port=5432;User Id=postgres;Password=postgres;");

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

> Note: Connection must be descendant of [`System.Data.Common.DbConnection`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection) type.

### First Commands

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

> *Note: this documentation will use a sample database from a PostgreSQL public tutorial ([postgresqltutorial.com](https://www.postgresqltutorial.com/)). You can find detailed instructions on installing this sample database on [this page](https://www.postgresqltutorial.com/postgresql-getting-started/postgresql-sample-database/).*
