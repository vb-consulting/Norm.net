# Changelog

Note: changelogs prior to version 5.3.0 can be found in archive markup: [changelog-archive.md](https://github.com/vb-consulting/Norm.net/blob/master/changelog-archive.md)

## [5.4.0](https://github.com/vb-consulting/Norm.net/tree/5.3.9) (2023-11-27)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.9...5.4.0)

### New feature: GetRecordsAffected method

Signature: 

- Extension: `public static int? GetRecordsAffected(this DbConnection connection)`
- Instance: `public int? GetRecordsAffected()`

Returns a number of records affected by the last query.

This is the value that [`ExecuteNonQuery()`](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand.executenonquery) method returns if one of the `Execute` versions is executed.

Example:

```csharp
var rowsAffected = connection
    .Execute("insert into rows_affected_test values ('foo')")
    .GetRecordsAffected();
```

If one of the `Read` methods is executed, this method will contain a value of [`RecordsAffected` reader property](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.recordsaffected)

However, `Read` methods will always return a value of the read operation (enumerator), so access to the instance is hidden.

That is why the `Norm` extension method is also introduced.

### New feature: Norm extension method

Signature: 

- Extension: `public static Norm Norm(this DbConnection connection)`

Creates and returns a new `Norm` instance from the connection.

Example:

```csharp
var instance = connection.Norm();
instance.Read("select * from rows_affected_test").ToList();
rowsAffected = instance.GetRecordsAffected();
```

### Breaking changes:

- Extensions `Execute` and `ExecuteFormat` were returning the connection instance in previous versions.
- This is changed to return the current `Norm` instance instead.

This may break certain codebases. 

For example, before this version, it was possible to chain `Execute` method with connection creation like this:

```csharp
using var connection = new NpgsqlConnection(connectionString).Execute("create temp table test (i int);");
```

Now, this code won't work because `Execute` method returns `Norm` instance. Instead, it should look like this:

```csharp
using var connection = new NpgsqlConnection(connectionString);
connection.Execute("create temp table test (i int);");
```

## [5.3.9](https://github.com/vb-consulting/Norm.net/tree/5.3.9) (2023-10-07)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.8...5.3.9)

Fix the issue with mapping protected properties.

Setting the global option should also include mapping to protected properties:

```csharp
NormOptions.Configure(options =>
{
    options.MapPrivateSetters = true;
});
```

By setting this global option, it was possible to map public properties with protected setter `public int ProtectedSetInt { get; protected set; }`, but not protected properties `protected int ProtectedInt { get; set; }`.

This inconsistency is fixed with this release.

## [5.3.8](https://github.com/vb-consulting/Norm.net/tree/5.3.8) (2023-09-07)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.7...5.3.8)

Support for HSTORE PostgreSQL data type.

HSTORE is a key/value data type that is returned as `Dictionary<string, string>` by the Npgsql reader.

There was a [mapping issue with HSTORE data type](https://github.com/vb-consulting/Norm.net/issues/20) - when mapping to class instances by name that was fixed in this release.

This works properly now:

```csharp
public class HstoreTest
{
    public string I { get; set; }
    public Dictionary<string, string> J { get; set; }
}
        
[Fact]
public void Hstore_Read_Class_Instance_Sync()
{
    using var connection = new NpgsqlConnection(fixture.ConnectionString);
    connection.Execute("create extension if not exists hstore");
    connection.ReloadTypes();

    var result = connection
        .Read<HstoreTest>(query)
        .Single();

    Assert.IsType<string>(result.I);
    Assert.IsType<Dictionary<string, string>>(result.J);
    Assert.Equal("123", result.J["foo"]);
}
```

Also, this release includes some minor performance optimizations too (objects not passed by reference). There will be new performance tests soon.

## [5.3.7](https://github.com/vb-consulting/Norm.net/tree/5.3.7) (2023-07-17)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.6...5.3.7)

### New feature NameParserCallback option

This is a new global option that can be set for all `Read` operations globally via options:

```csharp
public class NormOptions
{
    // ...

    /// <summary>
    /// Set the global name parser to return custom names for columns.
    /// </summary>
    public Func<(string Name, int Ordinal), string> NameParserCallback { get; set; } = null;

    // ...
}
```

This callback is invoked once per query for every column when column names are retrieved before any mapping.

It is very useful in situations where you need to map column names to a different format, or in general to be able to map to different names.

Input parameter is a tuple with the column name (as it is retrieved from database connection) and ordinal position. Use return value to set the name for the column.

For example, if you want to remove a prefix (`prefix_` for example) string from all column names where it appears, you can do it like this:

```csharp

//
// Program startup
//

NormOptions.Configure(o =>
{
    o.NameParserCallback = arg =>
        arg.Name.StartsWith("prefix_") ? arg.Name[7..] : arg.Name;
});

//
// In your code, when mapping by name, target class doesn't require prefix "prefix_" in property name
//
private class NameParserTest
{
    public string? Foo { get; set; }
    public string? Bar { get; set; }
}

// Read to single result:
var result = connection
    .Read<NameParserTest>("select * from name_parser_test")
    .Single();
```

Since this callback, if defined, is executed once per query it doesn't have any impact on mapping performances.

### More internal optimizations

There were two missed optimization opportunities in the code that were fixed in this release:

- Passing some structures by references instead of values.
- Retrieving column names from the reader once per query instead of once per row.

These optimizations aren't significant and are hardly noticeable, but they required a completely new set of performance tests.
See [performance test results here.](https://github.com/vb-consulting/Norm.net/blob/5.3.4/performance-tests.md)

To facilitate easier performance testing, there is a [docker file](https://github.com/vb-consulting/Norm.net/blob/master/Benchmarks/Dockerfile) that can be used to run performance tests in a docker environment.

### Changed access modifiers for internal mappers

Previously public parts of internal mappers that were marked as `public` are now `internal`.

## [5.3.6](https://github.com/vb-consulting/Norm.net/tree/5.3.6) (2023-07-02)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.5...5.3.6)

### Global handler for reader callback

Added global handler that can be set for all Read operations globally via options:

```csharp
NormOptions.Configure(options =>
{
    options.DbReaderCallback = r => r.Ordinal switch
    {
        0 => r.Reader.GetInt32(0) + 1,  // add 1 to the first field with ordinal 0, mapped to the first tuple named "a"
        _ => null                       // for all other fields, use the default mapping
    });
});
```

This would be equivalent to executing every Read operation with the `WithReaderCallback` method like this:

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

> Important: reader callback can be set only once per read operation and that includes global reader callbacks.

If you set global reader callback and another reader callback with `WithReaderCallback` - a `NormReaderAlreadyAssignedException` exception will be thrown.

- You can assign reader callbacks from private methods, for example:

```csharp
private object? ReaderCallback((string Name, int Ordinal, DbDataReader Reader) arg) => 
    arg.Reader.GetDataTypeName(arg.Ordinal) switch
{
    "json" => JsonNode.Parse(arg.Reader.GetString(arg.Ordinal))?.AsObject(),
    _ => null
};

//
// in your startup
NormOptions.Configure(o => o.DbReaderCallback = ReaderCallback);
```

The example above of the global reader handler shows how to implement a custom type mapping in your application: 
If the record type is `json` it will be automatically parsed to `JsonObject` object.

Example:
```csharp

private object? ReaderCallback((string Name, int Ordinal, DbDataReader Reader) arg) => 
    arg.Reader.GetDataTypeName(arg.Ordinal) switch
{
    "json" => JsonNode.Parse(arg.Reader.GetString(arg.Ordinal))?.AsObject(),
    _ => null
};

//
// in your startup
NormOptions.Configure(o => o.DbReaderCallback = ReaderCallback);

private class JsonTest
{
    public string I { get; set; }
    public JsonObject J { get; set; }
}

// all `json` types are automatically converted to `JsonObject` object
var instance = connection
    .Read<JsonTest>("select '{\"a\": 1}'::text as i, '{\"a\": 1}'::json as j")
    .Single();
```

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

```csharp
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

### New option - KeepOriginalNames

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

### Fix - NullableInstances option turned on but have non-nullable properties

- In previous version a new feature - when `NullableInstances` option is turned on, the mapper will return `null` instances if all mapped values are `null`.

- However, there was a edge-case bug in this implementation: Mapper tried first to assign a `null` value to instance, and if the property being mapped is not nullable - mapper would crash.

- This is now fixed in this version - if all mapped values are null, and `NullableInstances` option is on - return instance will be null, regardless of property types.

### New feature - NormNullException

- Attempted mapping of database null value to a non-nullable property will now throw `NormNullException` exception with nice message, for example: `Can't map null value for database field "foo" to non-nullable property "Foo".`.

## [5.3.2](https://github.com/vb-consulting/Norm.net/tree/5.3.2) (2023-05-03)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/5.3.1...5.3.2)

### New Feature - NullableInstances

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

