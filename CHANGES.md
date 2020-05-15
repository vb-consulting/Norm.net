# Version history

## 1.5.1

#### Changes to object mapper read extension. Extensions call `connection.Read(Query).Select<MyClass>()` now supports:

##### Snake case naming. 

Query results containing case name fields will be mapped properly to C# names.

For example query:

```sql
select * from (
values 
    (1, 'foo1', '1977-05-19'::date, true, null),
    (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
    (3, 'foo3', '1979-05-19'::date, null, 'bar3')
) t(my_id, my_foo, my_day, my_bool, my_bar)
```

will return following fields: `my_id`, `my_foo`, `my_day`, `my_bool`, `my_bar`.

Those fields will map properly to you C# class by using standard call 
`var result = connection.Read(sql).Select<SnakeCaseMapTestClass>();` to class instance:

```csharp
class SnakeCaseMapTestClass
{
    public int MyId { get; private set; }
    public string MyFoo { get; private set; }
    public DateTime MyDay { get; private set; }
    public bool? MyBool { get; private set; }
    public string MyBar { get; private set; }
}
```

##### Array field support. 

Some servers like `PostgreSQL` can return array as result which simplifies programming model and improves perfomances.

Now you can map directly to your instances declared as arrays. 

For example this PostgreSQL query will return 5 arrays:

```sql
select 
    array_agg(id) as id,
    array_agg(foo) as foo,
    array_agg(day) as day,
    array_agg(bool) as bool,
    array_agg(bar) as bar
from (
values 
    (1, 'foo1', '1977-05-19'::date, true, null),
    (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
    (3, 'foo3', '1979-05-19'::date, null, 'bar3')
) t(id, foo, day, bool, bar)
```

You can map these results with standard call 
`var result = connection.Read(sql).Select<ArraysTestClass>()` to class instance:

```csharp
class ArraysTestClass
{
    public int[] Id { get; private set; }
    public string[] Foo { get; private set; }
    public DateTime[] Day { get; private set; }
    public bool[] Bool { get; private set; }
    public string[] Bar { get; private set; }
}
```

**Important notes:**

> **Nullable types when using standard value types are not supported.
> `NULL` values willl just fall back to its default value.**

For example, if `int` array contains some `NULL` values, value after mapping will be `0` (default `int` value).

> **However, if type is reference type, like for example `string` type, `NULL` values will be used and mapped correctly.**

This is issue with underlying database provider, `Npgsql` in this case and cannot be resolved at this moment. Nevertheless array types in results are immensly useful feature that everyone should use..

Also:

> **This feature depeneds heavily on underlying database provider implementation and it is tested only with PostgreSQL at this point.**


## 1.5.0

- Removed unneccessary JSON support. There are so many different JSON libraries, there is no need to have reference to `System.Json` package. You can always get `string` result and serialize/deserialize as you want.
- Fixed issue with weak table connection reference in highly concurrent code.

## 1.4.0

#### Added support `PostgreSQL` format parameters

- Calling

```csharp
connection.UsingPostgresFormatParamsMode().Execute("command 1", p1, p2)
```

Will parse parameters using `PostgreSQL format` function to parse user input and prevent SQL injection.

This can send parameter to `PostgreSQL` anonymous script block;

```csharp
connection
    .Execute("create table plpgsql_test (t text)")
    .UsingPostgresFormatParamsMode()
    .Execute(@"

        do $$
        begin

            insert into plpgsql_test values (@foo1);
            insert into plpgsql_test values (@foo2);
            insert into plpgsql_test values (@foo1);
            insert into plpgsql_test values (@foo2);

        end
        $$;", ("foo1", "foo1"), ("foo2", "foo2"));
```

## 1.3.0

#### Added support for **prepared statemnts.** 

- To execute prepared statement execute `Prepared()` method before any given execution:

```csharp
connection.Prepared().Execute("command 1").Execute("command 2");
```

In example above only `command 1` is executed as prepared. 

- To execute both commands as prepared `Prepared()` call must preceed command execution:

```csharp
connection.Prepared().Execute("command 1").Prepared().Execute("command 2");
```

- In SQL Server, prepared statements don't have [has no significant performance advantage over direct execution](https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/ms175528(v=sql.105)?redirectedfrom=MSDN)

- In PostgreSQL, prepared statements have [17% performance increase](http://www.roji.org/prepared-statements-in-npgsql-3-2)

## 1.2.0

- Extensions that will change state of connection object like `As`, `AsProcedure`, `AsText`, `Timeout`, `WithJsonOptions` are now thread safe. That means that connection can safely be singleton or static object.
- New (thread safe) extension `WithCancellationToken` that will throw exception if cancellation is requested and propogate cancellation token to all async calls.
- Extensions `As`, `AsProcedure`, `AsText`, `Timeout`, `WithJsonOptions` and `WithCancellationToken` will only apply to current call chain.

## 1.1.9

- New overload for all functions with named parameters. Named parametzers now accept DBType along name and value.

## 1.1.8

- All parameters with `null` value will be interpreted as database null value (`DBNull.Value`)

## 1.1.7

- Added tests and support for SQLite database
- Add 11 and 12 tuples overloads

## 1.1.6

- Removes unneccessary dependency to `System.Linq.Async`
- Improves Async mapping `Select` and `JsonAsync`
- Remove obsolete extensions

## 1.1.4

- O/R mapping extension method on `IAsyncEnumerable` called `SelectAsync<TModel>` is deprecated in favor of `Select<TModel>` which is more consistent with `AsyncLinq` approach.

## 1.1.3

- Positional parameters can now receive parameters of native type derived from `DbParameter`. 
This allows custom types of parameters to be passed to query (PostgreSQL array types for example) and
eliminates need for `WithOutParameter` and `GetOutParameter` which are removed. See this [tests](https://github.com/vbilopav/NoOrm.Net/blob/master/Tests/PostgreSqlUnitTests/ParametersUnitTests.cs) for examples.

## 1.1.2

- Fixed extensions to use `IList` of tuples instead of `IEnumerable`

## 1.1.1

- Replaced `FastMember` O/R Mapping `Select<T>` extensions with custom implementation

## 1.1.0

- All non-generic result types `IEnumerable<(string name, object value)>` - are replaced with materialized lists, type: `IList<(string name, object value)>`.

- Consequently name/value tuple results are generating lists structure and do not deffer serialization and this allowed simplification of extensions. Current list of extensions:
[see here](https://github.com/vbilopav/NoOrm.Net/blob/master/Norm/Extensions/NormExtensions.cs)

- Added extension for O/R Mapping  by using [`FastMember`](https://github.com/mgravell/fast-member) library

Note:
`FastMember` yields slightly better results then Dapper but it doesn't support Ahead of Time compilation scenarios and members are case sensitive.

- Expended generic tuple parameters up to 10 members max. Will be more in future.
