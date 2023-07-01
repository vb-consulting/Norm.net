# Changelog

## [5.3.5](https://github.com/vb-consulting/Norm.net/tree/5.3.5) (2023-07-01)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.4...5.3.5)

When using the method for mapping anonymous instances based on instance prototype, e.g.:

```csharp
var result1 = connection
    .Read(
        new {id = default(int), name = default(string)}, // anonymous instance prototype
        "select 1 as id, 'foo' as name")
    .Single();

Assert.Equal(1, result1.Id);
Assert.Equal("foo", result1.Name);
```

From version `5.3.5` you can use a normal instance as a prototype, e.g.:

```sharp
class Class1
{
    public int Id { get; set; }
    public string Name { get; set; }
}

var instance1 = new Class1();
var result1 = connection
    .Read(instance1, "select 1 as id, 'foo' as name")
    .Single();

Assert.Equal(1, result1.Id);
Assert.Equal("foo", result1.Name);
```

When the mapper encounters an instance that is not anonymous, it will use the standard method of mapping instances.

## [5.3.4](https://github.com/vb-consulting/Norm.net/tree/5.3.4) (2023-05-26)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.3...5.3.4)

### New option - `KeepOriginalNames` 

When matching by name, the default behavior is to strip all database names of underscores and @ at characters.

This is done to make it easier to map underscore-separated names to C# properties (`field_name` to `FieldName` for example).

Set this option to true to skip this behavior and keep the original names.

Example:

```csharp
//
// in your startup
//
NormOptions.Configure(options =>
{
    options.KeepOriginalNames = true;
});

...

class FooBarClass
{
    public string? Foo_Bar { get; set; }
    public string? FooBar { get; set; }
}

...

result = connection
    .Read<FooBarClass>("select 'foobar' as foo_bar")
    .Single();

Assert.Equal("foobar", result.Foo_Bar);
Assert.Null(result.FooBar);
```

### New optimization and new performance tests

Some missed performance optimizations were added in this version. See the [changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.3...5.3.4) for more details.

Also, see [performance tests results](https://github.com/vb-consulting/Norm.net/blob/5.3.4/performance-tests.md).


## [5.3.3](https://github.com/vb-consulting/Norm.net/tree/5.3.3) (2023-05-16)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.2...5.3.3)

### Fix - `NullableInstances` option turned on but have non-nullable properties

- In previous version a new feature - when `NullableInstances` option is turned on, the mapper will return `null` instances if all mapped values are `null`.

- However, there was a edge-case bug in this implementation: Mapper tried first to assign a `null` value to instance, and if the property being mapped is not nullable - mapper would crash.

- This is now fixed in this version - if all mapped values are null, and `NullableInstances` option is on - return instance will be null, regardless of property types.

### New feature - `NormNullException`

- Attempted mapping of database null value to a non-nullable property will now throw `NormNullException` exception with nice message, for example: `Can't map null value for database field "foo" to non-nullable property "Foo".`.

## [5.3.2](https://github.com/vb-consulting/Norm.net/tree/5.3.2) (2023-05-03)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.1...5.3.2)

### New Feature - `NullableInstances`

- There is a new option `NullableInstances` that can be used to control whether to return `null` instances or not. 

- When this option is set to `true`, the mapper will return `null` instances for all classes that have all properties set to `null`.

- The default is `false` which is the same behavior as before (no changes).

- Example:

1) Set this option switch to true in your program startup:

```csharp
NormOptions.Configure(options =>
{
    options.NullableInstances = true;
});
```

2) When all mapped property values are `NULL`, instance will be null as well:

```csharp
    class TestClass
    {
        public int? Foo { get; set; }
        public string? Bar { get; set; }
    }

    var result = connection.Read<TestClass?>("select NULL as foo, NULL as bar").Single();
    
    // result is null since all properties are null
    Assert.Null(result);
```

This is can be useful when doing left joins and you want to know if join is matched. Example:

```csharp
    var (asset, vehicle) = connection.ReadAsync<Asset, Vehicle?>(
        """
        select a.*, v.*
        from assets a
        left join vehicles v on a.id = v.id
        """
    )
    .Single();
    
    if (vehicle is null)
    {
        // no matching vehicle
    }
```

### Improvements

- All tests with SqlServer are now using Microsoft.Data.SqlClient instead of System.Data.SqlClient.

## [5.3.1](https://github.com/vb-consulting/Norm.net/tree/5.3.1) (2022-11-18)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.0...5.3.1)

Minor optimizations and improvements.

## [5.3.0](https://github.com/vb-consulting/Norm.net/tree/5.3.0) (2022-10-16)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.5...5.3.0)

### New feature - dynamic mapping support

For this version `5.3.0` it is possible to map objects to a dynamic class.

Simply, instead of class or record name - use `dynamic` and the mapper will return a dynamic instance of `ExpandoObjects` objects:

```csharp
var result = connection.Read<dynamic>(@"select *
                from (
                values 
                    (1, 'foo1', '1977-05-19'::date, true, null),
                    (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                    (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                ) t(id, foo, day, bool, foo_bar)")
    .ToList();

Assert.Equal(3, result.Count);
Assert.Equal(1, result[0].id);
Assert.Equal(2, result[1].id);
Assert.Equal(3, result[2].id);

Assert.Equal("foo1", result[0].foo);
Assert.Equal("foo2", result[1].foo);
Assert.Equal("foo3", result[2].foo);
```

Note that `ExpandoObjects` are a bit slow and you will lose on auto-complete features as well as static typing.

`dynamic` can also be mixed with mapping to normal classes, for example:

```csharp
var result = connection.Read<Class1, dynamic>(@"select *
                from (
                values 
                    (1, 'foo1', '1977-05-19'::date, true, null),
                    (2, 'foo2', '1978-05-19'::date, false, 'bar2'),
                    (3, 'foo3', '1979-05-19'::date, null, 'bar3')
                ) t(id, foo, day, bool, foo_bar)")
    .ToList();
```

In this example, whatever is not mapped to `Class1` will be mapped to a dynamic type.

See examples in [this unit tests](https://github.com/vb-consulting/Norm.net/blob/5.3.0/Tests/PostgreSqlUnitTests/DynamicMapUnitTests.cs).

### Fix - duplicate field names in multi mapping

Some queries can return duplicate field names. 

For example, if we have these multiple mappings:

```csharp
public class Shop
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<Account> Accounts { get; set; }
}

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public int ShopId { get; set; }
    public Shop Shop { get; set; }
}

//...
connection.Read<Shop, Account>(@"
        select s.*, a.*
        from shop s 
        inner join account a on s.id = a.shop_id")
//...
```

This query returns duplicate names (`id` and `name`):

| **id** | **name** | **id** | **name** | address | country | shop_id |
| -- | ---- | -- | ---- | ------- | ------- | ------- |
| 1 | shop1 | 3 | account3 | addr3 | country3 | 1 |
| 1 | shop1 | 2 | account2 | addr2 | country2 | 1 |
| 1 | shop1 | 1 | account1 | addr1 | country1 | 1 |
| 2 | shop2 | 5 | account5 | addr5 | country5 | 2 |
| 2 | shop2 | 4 | account4 | addr4 | country4 | 2 |

In previous versions, duplicate names were not handled correctly. 
In this example, only `Id` and `Name` from `Shop` class would be mapped correctly and the `Id` and `Name` from `Account` class would be skipped because the mapper would incorrectly conclude that those fields were already mapped. 

In this version, this is fixed and classes `Shop` and `Account` are mapped correctly.

This makes it easier to build nested object maps with the `Linq` expressions for example:

```csharp
var shops = connection.Read<Shop, Account>(@"
    select s.*, a.*
    from shop s 
    inner join account a on s.id = a.shop_id")
    .GroupBy(item => item.Item1.Id)
    .Select(group =>
    {
        var shop = group.First().Item1;
        shop.Accounts = group.Select(item =>
        {
            var account = item.Item2;
            account.Shop = shop;
            return account;
        }).ToList();
        return shop;
    })
    .ToList();
```

This will build a nested object tree where each shop contains a list of accounts and that every account has a reference to a shop instance, and so on.

See the example in [this unit tests](https://github.com/vb-consulting/Norm.net/blob/5.3.0/Tests/PostgreSqlUnitTests/NestedObjectMapUnitTests.cs).

### New benchmarks

Since the mapper code was changed, there are also new benchmarks for this version: [PERFOMANCE-TESTS](https://github.com/vb-consulting/Norm.net/blob/5.3.0/PERFOMANCE-TESTS.md)

## [5.2.5](https://github.com/vb-consulting/Norm.net/tree/5.2.5) (2022-10-06)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.4...5.2.5)

Very small change that affects how caller info is formatted.

Instead of this `at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs 28`, hashtag was added now we have this `at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs#28`.

This is because, when caller info is displayed with a hashtag in Visual Studio Code, `Ctrl+Click` will navigate editor to that exact line.

## [5.2.4](https://github.com/vb-consulting/Norm.net/tree/5.2.4) (2022-10-04)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.3...5.2.4)

Fix anonymous parameter parser to allow for complex values.

## [5.2.3](https://github.com/vb-consulting/Norm.net/tree/5.2.3) (2022-07-08)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.2...5.2.3)

Comment headers are now always multi-line SQL comments.

Quick example:

```csharp
public void ChangeLogExample1()
{
    var (foo, bar) = connection
        .WithCommentHeader(comment: "My foo bar query", includeCommandAttributes: true, includeParameters: true, includeCallerInfo: true)
        .WithParameters(new { foo = "foo value", bar = "bar value" })
        .Read<string, string>("select @foo, @bar")
        .Single();
}
```

This will produce the following command on SQL Server:

```
/*
My foo bar query
Sql Text Command. Timeout: 30 seconds.
at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs 28
@foo nvarchar = "foo value"
@bar nvarchar = "bar value"
*/
select @foo, @bar
```

Single line comments, when showing parameter values that contain multiple lines could break comments and cause runtime errors.

This solves this problem.

Additionally, parameter values are parsed to replace comment start and end sequences (`/*` and `*/`) with `??`.

Comment header parameters now support PostgreSQL native positional parameters.

PostgreSQL array parameter output to comment header is also supported.

## [5.2.2](https://github.com/vb-consulting/Norm.net/tree/5.2.2) (2022-07-05)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.1...5.2.2)

- Smaller internal improvements and fixed async disposal for the Multiple Batch class implementation.

- New performance tests. They are showing that Norm is now at similar levels as Dapper. This might be due to the new feature implementations that have been added since version 4.3.0.

## [5.2.1](https://github.com/vb-consulting/Norm.net/tree/5.2.1) (2022-06-26)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.2.0...5.2.1)

### Support for specific parameter types when using PostgreSQL native positional parameters

In previous version with could set PostgreSQL positional paramters like this:

```csharp
var p1 = "_b_";
var p2 = "a__";

var result = connection
    .WithParameters(p1, p2)
    .Read<string>(@"select * from 
        (values ('abc'), ('bcd')) t (t1) 
        where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
    .Single();

Assert.Equal("abc", result);
```

However, sometimes when the parameter value is null, and the parameter type has not been set explicitly and PostgreSQL could not determine the data type of that parameter.

In fact, this would throw an exception:

```csharp
var p1 = "_b_";
var p2 = (string)null;

var result = connection
    .WithParameters(p1, p2)
    .Read<string>(@"select * from 
        (values ('abc'), ('bcd')) t (t1) 
        where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
    .Single();

Unhandled exception. Npgsql.PostgresException (0x80004005): 42P08: could not determine data type of parameter $1
```

From this version 5.2.1, positional parameter can be value tuple where first tuple is actual value and second value is database type:

```csharp
var p1 = "_b_";
var p2 = ((string)null, DbType.AnsiString);

var result = connection
    .WithParameters(p1, p2)
    .Read<string>(@"select * from 
        (values ('abc'), ('bcd')) t (t1) 
        where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
    .Single();
```

This will set the parameter type correctly and solve the error.

It is also possible to use native parameter type, for example:

```csharp
var p1 = "_b_";
var p2 = ((string)null, NpgsqlDbType.Text);

var result = connection
    .WithParameters(p1, p2)
    .Read<string>(@"select * from 
        (values ('abc'), ('bcd')) t (t1) 
        where ($1 is null or t1 similar to $1) and ($2 is null or t1 similar to $2)")
    .Single();
```

Note that this approach will also save the underlying database driver of mapping correct types, and give another performance increase.

## [5.2.0](https://github.com/vb-consulting/Norm.net/tree/5.2.0) (2022-06-23)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.1.0...5.2.0)

### Support for PostgreSQL native positional parameters - Npgsql (PostgreSQL) 6+ only

You can now use PostgreSQL **native positional parameters** (or any other database provider that supports unnamed positional parameters).

PostgreSQL example by using [standard dollar notation](https://www.postgresql.org/docs/8.1/sql-expressions.html#AEN1626) to set parameters positions:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
    .Single();

Assert.Equal("str", s); // true
Assert.Equal(999, i); // true
Assert.True(b); // true
Assert.Equal(new DateTime(1977, 5, 19), d); // true
Assert.Null(@null); // true
```

Here the `$1` references the value of the first function argument, `$2` for the second, and so on.

There are two conditions to enable using PostgreSQL native positional parameters:

1) Connection must be an instance of `NpgsqlConnection` from `Npgsql` version 6 or higher.
2) All supplied parameters must be a simple type (`int`, `string`, `DateTime`, `bool`, etc). Using class or record instance values of complex types will force using named parameters again.

Mixing named and positional parameters in a query like this `SELECT * FROM employees WHERE first_name = $1 AND age = @age` will trow `System.NotSupportedException : Mixing named and positional parameters isn't supported` error.

Other then setting simple values, you can also use native `NpgsqlParameter` objects by setting only values, like this:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new NpgsqlParameter { Value = "str" },
                    new NpgsqlParameter { Value = 999 },
                    new NpgsqlParameter { Value = true },
                    new NpgsqlParameter { Value = new DateTime(1977, 5, 19) },
                    new NpgsqlParameter { Value = DBNull.Value })
    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
    .Single();

Assert.Equal("str", s); // true
Assert.Equal(999, i); // true
Assert.True(b); // true
Assert.Equal(new DateTime(1977, 5, 19), d); // true
Assert.Null(@null); // true
```

****Important:****

Using PostgreSQL native positional parameters will disable SQL rewriting by the `Npgsql` drivers for that command. 

That has a positive performance impact since the driver doesn't have to parse the command anymore. 

However, sending multiple commands in one command string separated by a semicolon will be disabled and will throw `Npgsql.PostgresException : 42601: cannot insert multiple commands into a prepared statement`.

See following [article](https://www.roji.org/parameters-batching-and-sql-rewriting).

## `NpgsqlEnableSqlRewriting` global setting - Npgsql (PostgreSQL) 6+ only

You can force all `Npgsql` commands to "raw" mode that disables `Npgsql` parser for all `Npgsql` commands globally.

Set global `NpgsqlEnableSqlRewriting` settings to false:

```csharp
NormOptions.Configure(options => options.NpgsqlEnableSqlRewriting = false);
```

Notes:

- This option only has an impact if it is set before any `Npgsql` command is executed. `NpgsqlCommand` caches internally this value.

- Default value is `null` which doesn't change anything, uses `Npgsql` default which is true.

- Only available for `Npgsql` version 6 or higher.

- When in "raw" mode (SQL rewriting disabled) following features are disabled:
   - Named parameters are disabled, and positional parameters with $ syntax must be used always (throws `System.NotSupportedException : Mixing named and positional parameters isn't supported`).
   - Multiple commands in one command string separated by a semicolon (throws `Npgsql.PostgresException : 42601: cannot insert multiple commands into a prepared statement`)

### `WithUnknownResultType(params bool[] list)` connection extension method - Npgsql (PostgreSQL) 6+ only

This new method extension will tell the underlying `Npgsql` command results parser not to map command results and return raw strings only.

For example, results of this PostgreSQL query are all strings:

```csharp
var (@int, @bool, @date, @num, @json) = connection
    .WithUnknownResultType()
    .Read<string, string, string, string, string>(
        "select 1::int, true::bool, '1977-05-19'::date, 3.14::numeric, '{\"x\": \"y\"}'::json")
    .Single();

Assert.Equal("1", @int);
Assert.Equal("t", @bool);
Assert.Equal("1977-05-19", @date);
Assert.Equal("3.14", @num);
Assert.Equal("{\"x\": \"y\"}", @json);
```

In this case, the results parser didn't even bother trying to map and just returned the raw string.

This, of course, has certain performance benefits. Sometimes, results don't have to be mapped and they can be passed as-is.

But sometimes, this can be useful when mapping rare and exotic PostgreSQL types which are not supported by the official `Npgsql` drivers.

PostgreSQL is unkown to have a variety of different and custom types and not all may be supported by the `Npgsql`. See this [FAQ](https://www.npgsql.org/doc/faq.html#a-nameunknowntypei-get-an-exception-the-field-field1-has-a-type-currently-unknown-to-npgsql-oid-xxxxx-you-can-retrieve-it-as-a-string-by-marking-it-as-unknowna)

If you supply array parameters for this method, you can tell the `Npgsql` that unknown types are only at certain position, by setting `true` at same paramater position. For example:

```csharp
var (@int, @bool, @date, @num, @json) = connection
    .WithUnknownResultType(true, false, true, false, true)
    .Read<string, bool, string, decimal, string>(sql
        "select 1::int, true::bool, '1977-05-19'::date, 3.14::numeric, '{\"x\": \"y\"}'::json")
    .Single();

Assert.Equal("1", @int);
Assert.Equal(true, @bool);
Assert.Equal("1977-05-19", @date);
Assert.Equal(3.14m, @num);
Assert.Equal("{\"x\": \"y\"}", @json);
```

So, in this case only the first, third, and fourth results are unknown and therefore returned as raw strings.

If simply called this method without any parameters like this `.WithUnknownResultType()` - it would set all results to unknown raw mode.

## [5.1.0](https://github.com/vb-consulting/Norm.net/tree/5.1.0) (2022-06-20)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.0.1...5.1.0)

### `WithCommandBehavior(CommandBehavior)` connection extension method

New extensions method that sets the command behavior of the data read for the next command.

You can combine `CommandBehavior` values as flags. For example:

```csharp
connection
    .WithCommandBehavior(CommandBehavior.CloseConnection | CommandBehavior.SingleRow)
    .Read(...);
```

Here are all command behaviors available:

```csharp
namespace System.Data
{
    //
    // Summary:
    //     Provides a description of the results of the query and its effect on the database.
    [Flags]
    public enum CommandBehavior
    {
        //
        // Summary:
        //     The query may return multiple result sets. Execution of the query may affect
        //     the database state. Default sets no System.Data.CommandBehavior flags, so calling
        //     ExecuteReader(CommandBehavior.Default) is functionally equivalent to calling
        //     ExecuteReader().
        Default = 0,
        //
        // Summary:
        //     The query returns a single result set.
        SingleResult = 1,
        //
        // Summary:
        //     The query returns column information only. When using System.Data.CommandBehavior.SchemaOnly,
        //     the .NET Framework Data Provider for SQL Server precedes the statement being
        //     executed with SET FMTONLY ON.
        SchemaOnly = 2,
        //
        // Summary:
        //     The query returns column and primary key information. The provider appends extra
        //     columns to the result set for existing primary key and timestamp columns.
        KeyInfo = 4,
        //
        // Summary:
        //     The query is expected to return a single row of the first result set. Execution
        //     of the query may affect the database state. Some .NET Framework data providers
        //     may, but are not required to, use this information to optimize the performance
        //     of the command. When you specify System.Data.CommandBehavior.SingleRow with the
        //     System.Data.OleDb.OleDbCommand.ExecuteReader method of the System.Data.OleDb.OleDbCommand
        //     object, the .NET Framework Data Provider for OLE DB performs binding using the
        //     OLE DB IRow interface if it is available. Otherwise, it uses the IRowset interface.
        //     If your SQL statement is expected to return only a single row, specifying System.Data.CommandBehavior.SingleRow
        //     can also improve application performance. It is possible to specify SingleRow
        //     when executing queries that are expected to return multiple result sets. In that
        //     case, where both a multi-result set SQL query and single row are specified, the
        //     result returned will contain only the first row of the first result set. The
        //     other result sets of the query will not be returned.
        SingleRow = 8,
        //
        // Summary:
        //     Provides a way for the DataReader to handle rows that contain columns with large
        //     binary values. Rather than loading the entire row, SequentialAccess enables the
        //     DataReader to load data as a stream. You can then use the GetBytes or GetChars
        //     method to specify a byte location to start the read operation, and a limited
        //     buffer size for the data being returned.
        SequentialAccess = 16,
        //
        // Summary:
        //     When the command is executed, the associated Connection object is closed when
        //     the associated DataReader object is closed.
        CloseConnection = 32
    }
}
```

### Parameters can be set again in `Read` and `Execute` extensions methods

This feature was removed after version 5.0.0.

The reason was that caller info default parameters were added and the `params` array doesn't work well with default parameters.

However, that doesn't mean that optional parameter object `object parameters = null` with null default value can't be used.

There is no convenience of the parameters array, but, it still can be used for an anonymous object which is the most common use case.

For example, now we can do this again:

```csharp
var (s, i, b, d, @null) = connection
    .Read<string, int, bool, DateTime, string>(
        "select @strValue, @intValue, @boolValue, @dateTimeValue, @nullValue",
        new
        {
            strValue = "str",
            intValue = 999,
            boolValue = true,
            dateTimeValue = new DateTime(1977, 5, 19),
            nullValue = (string)null,
        })
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
Assert.Null(@null);
```

However, the current extension methods `WithParameters` was not removed, and it's possible to combine both. The resulting parameters will be merged. Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new
    {
        strValue = "str",
        intValue = 999,
    })
    .Read<string, int, bool, DateTime, string>(
        "select @strValue, @intValue, @boolValue, @dateTimeValue, @nullValue",
        new
        {
            boolValue = true,
            dateTimeValue = new DateTime(1977, 5, 19),
            nullValue = (string)null,
        })
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
Assert.Null(@null);
```

This new optional paramemeter `paramemeters` on `Read` and `Execute` extensions is no longer an array. To be able to pass parameters array, actual array needs to be constructed. Example:

```csharp
var (s, i, b, d) = connection
    .Read<string, int, bool, DateTime>(
        "select @s, @i, @b, @d",
        new NpgsqlParameter[] 
        {
            new NpgsqlParameter("s", "str"),
            new NpgsqlParameter("i", 999),
            new NpgsqlParameter("b", true),
            new NpgsqlParameter("d", new DateTime(1977, 5, 19))
        })
    .Single();

Assert.Equal("str", s);
Assert.Equal(999, i);
Assert.True(b);
Assert.Equal(new DateTime(1977, 5, 19), d);
```

### `WithTransaction(DbTransaction)` connection extension method

Support for the transaction control by using `DbTransaction` object. 

`WithTransaction(DbTransaction)` will set the transaction object for the next command.

Example:

```csharp
using var connection = new NpgsqlConnection(fixture.ConnectionString)
    .Execute("create temp table transaction_test1 (i int);");

using var transaction = connection.BeginTransaction();

connection
    .WithTransaction(transaction)
    .Execute("insert into transaction_test1 values (1),(2),(3);");

var result1 = connection.Read("select * from transaction_test1").ToArray();
Assert.Equal(3, result1.Length);

transaction.Rollback();

var result2 = connection.Read("select * from transaction_test1").ToArray();
Assert.Empty(result2);
```

### `WithTimeout` connection extension method

This is equivalent to the `Timeout` connection extension, added only for the naming consistency ("with" prefix).

It just sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error.

### `MapPrivateSetters` global option

By default, it is not possible to map instance properties that don't have a public setter method.

Now, it is possible to change that behavior with global options by using the `MapPrivateSetters` setting, like this:

```csharp
NormOptions.Configure(options => options.MapPrivateSetters = true); // default is false
```

This will enable mapping of the instance properties with private and protected setter methods:

```csharp
public class TestClass
{
    public int PrivateSetInt { get; private set; }
    public int ProtectedSetInt { get; protected set; }
}

```
```csharp

var result = connection
    .Read<TestClass>("select 1 as private_set_int, 2 as protected_set_int")
    .Single();

Assert.Equal(1, result.PrivateSetInt); // true
Assert.Equal(2, result.ProtectedSetInt); // true
```

Note that instance properties without any setter or a public getter are still not going to be mapped.

### Multiple readers now implement `IAsyncDisposable`

And now it's possible to do:

```csharp
await using var multiple = await connection.MultipleAsync(Queires);
```

### Changed member scope from internal to allow greater extendibility

## [5.0.1](https://github.com/vb-consulting/Norm.net/tree/5.0.1) (2022-06-16)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.0.0...5.0.1)

- Fixed weird edge case bug with mapping `datetimeoffset` on SQL Server, when actual type is `datetimeoffset`.
- Slightly improved parameters parsing when using positional parameters.

## [5.0.0](https://github.com/vb-consulting/Norm.net/tree/5.0.0) (2022-05-24)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.3.0...5.0.0)

### New feature - comment headers

From version 5, Norm can create SQL comment headers on all or some SQL commands. 

Those comment headers can include the following:

- Custom user comment.
- Parameters names, types, and values.
- Caller info such as calling method name, source code file name, and line number.
- Command attributes such as database provider, command type (text or stored procedure), and command timeout.
- Execution timestamp.

When enabled, this feature can help with diagnostics and monitoring through:

- Standard application loggers.
- Database monitoring tools such as Activity Monitor on SQL Server or monitoring tables on PostgreSQL such as `pg_stat_activity` or `pg_stat_statements`.

Quick example:

```csharp
public void ChangeLogExample1()
{
    var (foo, bar) = connection
        .WithCommentHeader(comment: "My foo bar query", includeCommandAttributes: true, includeParameters: true, includeCallerInfo: true)
        .WithParameters(new { foo = "foo value", bar = "bar value" })
        .Read<string, string>("select @foo, @bar")
        .Single();
}
```

This will produce the following command on SQL Server:

```
-- My foo bar query
-- Sql Text Command. Timeout: 30 seconds.
-- at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs 28
-- @foo nvarchar = "foo value"
-- @bar nvarchar = "bar value"
select @foo, @bar
```

Same code on PostgreSQL:

```
-- My foo bar query
-- Npgsql Text Command. Timeout: 30 seconds.
-- at ChangeLogExample1 in /SourcePath/ChangeLogExample.cs 28
-- @foo text = "foo value"
-- @bar text = "bar value"
select @foo, @bar
```

These header values can be enabled for individual commands and globally for all commands.

A quick example of enabling logging of caller info and parameter values for all commands in application:

```csharp
/* On application startup */

NormOptions.Configure(options =>
{
    options.CommandCommentHeader.Enabled = true;
    options.CommandCommentHeader.IncludeCallerInfo = true;
    options.CommandCommentHeader.IncludeParameters = true;
    options.DbCommandCallback = command => logger.LogInformation(command.CommandText);
});
```

This allows you to log all your commands with all relevant information needed for debugging, such as parameter values, and also with source code information that allows you to locate commands in your source code.

And since it is included in the command comment header, they are visible on a database server as well through monitoring tools.

> Note: Caller info is generated at the compile time by the compiler, which makes them no different than constant values, so, there is no performance penalty for including such information.

To support this feature, several new extensions are added and support global configuration.

More details bellow:

### New method extensions

Important note:

Same `Norm` method extensions are available as `DbConnection` extension methods and Norm instance methods.

Those methods who don't return any results are returning `Norm` instance, which allows us to chain multiple calls like in example above or simplified:

```csharp
connection
    .WithCommentHeader(/*comment header options*/)
    .WithParameters(/*parameters*/)
    .Read(/*command*/);
}
```

#### `WithParameters(params object[] parameters)`

Sets parameters for the next command.

**This is a breaking change!**

In versions 4 and lower, parameters were supplied using this pattern:

```csharp
connection.Execute("command", param1, param2, param3);
```

From version 5, parameters are supplied using `WithParameters` extension method:

```csharp
connection
    .WithParameters(param1, param2, param3)
    .Execute("command");
```

#### `WithCommandCallback(Action<DbCommand> dbCommandCallback)`

Executes supplied action with constructed `DbCommand` as a single parameter just before the actual execution.

This exposes created `DbCommand` object and allows for eventual changes to be made to the `DbCommand` object such as changes in a command text, parameters collection, etc, depending on a provider.

There is also a global version of this method that is commonly used for logging tasks.

#### `WithReaderCallback(Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback)`

Executes reader callback function for each value just before assigning mapper results.

The input parameter is a named tuple of three values:

1) `Name` - field name from your command results.
2) `Ordinal` - ordinal position of the field from your command results.
3) `Reader` - `DbDataReader` object used for reading results.

This allows supplying custom results to mapper by returning values:

- If this function callback returns a non-null value, that value is mapped.
- If this function callback returns a null value, the default value is mapped.
- For null values, return `DBNull.Value`.

Example:

```csharp
var array = connection
    .WithReaderCallback(r => r.Ordinal switch
    {
        0 => r.Reader.GetInt32(0) + 1,  // add 1 to the first field with ordinal 0, mapped to the first tuple named "a"
        _ => null                       // for all other fields, use the default mapping
    })
    .Read<(int a, int b, int c)>("select * from (values (1, 1, 1), (2, 2, 2), (3, 3, 3)) t(a, b, c)")
    .ToArray();
```

Note: 

**This is a breaking change!**

In version 4 and lower, this callback function was supplied as an optional parameter to the `Read` method overloads extensions.

```csharp
connection.Read("command", r => { /*...*/});
```
From version 5, the `WithReaderCallback` extension method is used to supply this callback to the next read operation.

#### `WithComment(string comment)`

Adds a custom comment header to the next command.

#### `WithCommentParameters()`

Adds a comment header with command parameters described with names, database types, and values to the next command.

#### `WithCommentCallerInfo()`

Adds a comment header with caller info data (calling method names, source code file, and line number) to the next command.

#### `WithCommentHeader(string comment = null, bool includeCommandAttributes = true, bool includeParameters = true, bool includeCallerInfo = true, bool includeTimestamp = false)`

Adds a comment header  to the next command and configures options:

- `comment` - custom comment text.

- `includeCommandAttributes` - command attributes such as database provider, command type, and command timeout.

- `includeParameters` -  command parameters described with names, database types, and values.

- `includeCallerInfo` - caller info data (calling method names, source code file, and line number).

- `includeTimestamp` - timestamp of command execution.

### Global Configuration

Use the global static method `NormOptions.Configure(options => { /* options */ });` to configure global options.

This is usually called once at program startup. 
Do not call this from multithreaded sections of your application and do not call it more than once.

For example:

```csharp
NormOptions.Configure(options =>
{
    options.CommandTimeout = 10 * 60;
    options.CommandCommentHeader.Enabled = true;
    options.CommandCommentHeader.IncludeCallerInfo = true;
    options.CommandCommentHeader.IncludeParameters = true;
    options.DbCommandCallback = command => logger.LogInformation(command.CommandText);
});
```

This will configure all commands to have a timeout of 10 minutes and enables comment headers with parameters description.

Available options are:

| Option name | Description | Default |
| ----------- | ----------- | ------- |
| `CommandTimeout` | Sets the wait time in seconds for the connection commands, before terminating the attempt to execute a command and generating an error | `NULL` (system default) |
| `DbCommandCallback` | Command callback that will be called before every execution. | `NULL` (don't run this callback) |
| `RawInterpolationParameterEscape` | Escape sequence, when using parameters via string interpolation formats, use this escape to skip parameter parsing and use values as is. | "raw" | 
| `Prepared` | Set to true to run all commands in prepared mode every time by calling `Prepare()` method before execution. | `false` | 
| `CommandCommentHeader` | Autmoatic command comment header options. | see table bellow |

`CommandCommentHeader` options are:

| Option name | Description | Default |
| ----------- | ----------- | ------- |
| `Enabled` | Enable automatic comment headers that are added to each query command. | `false` |
| `IncludeCommandAttributes` | Include current command attributes like database provider, command type, and command timeout. | `true` |
| `IncludeCallerInfo` | Include command caller info like calling method member name, source code file path, and source code line number. | `true` |
| `IncludeParameters` | Include command parameters in comment headers. | `true` |
| `ParametersFormat` | Format string for parameters in comment headers. Placeholder 0 is the parameter name, 1 is the parameter type and 2 is the parameter value. Used only when `IncludeParameters` is `true`. | `$"-- @{{0}} {{1}} = {{2}}{Environment.NewLine}"` |
| `IncludeTimestamp` | Include command execution timestamp in comment headers. | `false` |
| `OmmitStoredProcCommandCommentHeaderForDbTypes` | Omits comment headers if enabled from a command text when command type is Stored Procedure for the database providers that do not support comments in a Stored Procedure calls (SQL Server and MySQL). | ` DatabaseType.Sql | DatabaseType.MySql` |

### New feature - `Multiple` and `MultipleAsync` now support reader callbacks

This was previously missing. Now, it is possible to use reader callbacks with multiple queries:

```csharp
using var multiple = await connection
    .WithReaderCallback(r => r.Ordinal switch
    {
        0 => r.Reader.GetInt32(0) + 1,
        _ => null
    })
    .MultipleAsync(@"
        select 1 as id1, 'foo1' as foo1, 'bar1' as bar1; 
        select 2 as id2, 'foo2' as foo2, 'bar2' as bar2");

var result1 = await multiple.ReadAsync<Record1>().SingleAsync();

var next1 = multiple.Next(); // true

var result2 = await multiple.ReadAsync<Record2>().SingleAsync();

var next2 = multiple.Next();  // false

// ...
```

### New feature - anonymous `Read` and `ReadAsync` overloads for anonymous types added to `Multiple` and `MultipleAsync`

This was previously missing. Now, it is possible to map anonymous types with multiple queries:

```csharp
using var multiple = connection.Multiple(@"
    select * from (
        values 
        (1, 'foo1'),
        (2, 'foo2'),
        (3, 'foo3')
    ) t(first, bar);

    select * from (
        values 
        ('1977-05-19'::date, true, null),
        ('1978-05-19'::date, false, 'bar2'),
        (null::date, null, 'bar3')
    ) t(day, bool, s)
");

var first = multiple.Read(new
{
    first = default(int),
    bar = default(string)
}).ToList();

multiple.Next();

var second = multiple.Read(new
{
    day = default(DateTime?),
    @bool = default(bool?),
    s = default(string),
}).ToList();
```

### Internal improvements 

- Removed some unused leftovers from before version 4.0.0 on methods `Execute` and `ExecuteAsync` which were still using tuple parameters, although they are not supported since 4.0.0.
- Changed internal instance methods scope modifiers from private to protected to improve future extendibility.
- Removed unnecessary type caching. They give no performance gains and are using memory. No need to have them.
- Remove unnecessary and obsolete instance managament mechanism: improves memory and perfomances a bit.
- Many other improvements for internal structure that allow for future extendibility.


