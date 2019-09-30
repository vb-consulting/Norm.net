# `NoOrm.Net`

Fast and extendible **`C# 8`** alternative to Dapper data access built for **.NET Core 3** era.

>**THIS IS NOT ORM**

And hence, the name - **`NoOrm`**.

## How It Works - Why **`NoOrm`** - And - Similarities With `Dapper`

Both libraries are implementation of set of various extensions over `System.Data.Common.DbConnection` system object to work efficiently with different databases.

They work quite differently. For example:

- Dapper

```csharp
string query = "< one million records database query >";
var results = connection.Query<TestClass>(query);
// results is IEnumerable<TestClass> in 00:00:02.0330651
```

- NoOrm

```csharp
string query = "< one million records database query >";
var results = connection.Read(query).Select(t => new TestClass(t));
// results is IEnumerable<TestClass> in 00:00:00.0003062
```

`NoOrm` creates results of same type `IEnumerable<TestClass>` in fraction of time (`00.0003062` seconds to Dapper `02.0330651` seconds in example).

Reason is simple:

- *Dapper* triggers iteration and objects serialization immediately when called.

- *NoOrm* builds internal iterator over database results. This allows delaying any results iteration (and potential serialization) - so that expression tree can be built (using `Linq` and `AsyncLinq` libraries or custom `IEnumerable` extensions) for view models or service responses - before any actual results iteration.

This approach can save unnecessary database result iterations to improve system performances.


