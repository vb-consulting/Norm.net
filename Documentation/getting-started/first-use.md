---
title: First Use
position: 3
---

# First Use

## Add Using

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

## Obtain Connection reference

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

## Execute First Commands

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
