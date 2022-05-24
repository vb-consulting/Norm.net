# Changelog

## [5.0.0](https://github.com/vb-consulting/Norm.net/tree/5.0.0) (2022-05-24)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.3.0...5.0.0)

### Breaking change - anonymous `Read` overload are renamed to `ReadAnonymous`

Anonyoums example from previous version looks like this now:

```csharp
var result = connection.ReadAnonyoums(new 
{ 
    id = default(int), 
    foo = default(string), 
    date = default(DateTime) 
}, @"select * from (values
    (1, 'foo1', cast('2022-01-01' as date)), 
    (2, 'foo2', cast('2022-01-10' as date)), 
    (3, 'foo3', cast('2022-01-20' as date))
) t(id, foo, date)").ToList();
```

The reason for this change is because method overload resolution couldn't distinguish anonymous reads from non-anonymous reads in some rare situations and the anonymous type blueprint instance parameter was accidentally swap for command which could break some code.

This is much more concise.

### Internal improvements 

- Removed some unused leftovers from before version 4.0.0 on methods `Execute` and `ExecuteAsync` which were still using tuple parameters, although they are not supported since 4.0.0.

- Changed internal instance methods scope modifiers from private to protected to improve future extendibility.

- Removed unnecessary type caching. They give no performance gains and are using memory. No need to have them.

## [4.3.0](https://github.com/vb-consulting/Norm.net/tree/4.3.0) (2022-05-07)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.2.0...4.3.0)

## New feature: mapping to anonymous types

From version 4.3.0., Norm allows **mapping to anonymous type instances** - by supplying anonymous type **blueprint instance** as the first parameter.

Since anonymous types cannot be declared and only created - that blueprint instance will be used to create new instances in a query result.

For example, you can declare new anonymous types with default values:


```csharp
var result = connection.Read(new 
{ 
    id = default(int), 
    foo = default(string), 
    date = default(DateTime) 
}, @"select * from (values
    (1, 'foo1', cast('2022-01-01' as date)), 
    (2, 'foo2', cast('2022-01-10' as date)), 
    (3, 'foo3', cast('2022-01-20' as date))
) t(id, foo, date)").ToList();
```

This will yield a three-element list of anonymous types:

```csharp
Assert.Equal(3, result.Count); // true

Assert.Equal(1, result[0].id); // true
Assert.Equal("foo1", result[0].foo); // true
Assert.Equal(new DateTime(2022, 1, 1), result[0].date); // true

Assert.Equal(2, result[1].id); // true
Assert.Equal("foo2", result[1].foo); // true
Assert.Equal(new DateTime(2022, 1, 10), result[1].date); // true

Assert.Equal(3, result[2].id); // true
Assert.Equal("foo3", result[2].foo); // true
Assert.Equal(new DateTime(2022, 1, 20), result[2].date); // true
```

Declaration of this anonymous type is provided by a blueprint instance with default values in the first parameter:

```csharp
new 
{ 
    id = default(int), 
    foo = default(string), 
    date = default(DateTime) 
};
```

That is one way to explicitly declare desired types in anonymous types. 

Of course, you can also explicitly declare values of certain types, which is a bit shorter and a bit less readable:

```csharp
new 
{ 
    id = 0, 
    foo = "", 
    date = DateTime.Now 
};
```

In this case, same as in the previous case, those values are discarded and instances populated by database values are returned.

### Remarks on using anonymous types

> - Fields are **matched by name**, not by position.

> - Matching is **case insensitive.**

> - **Snake case names are supported** and matched with non cnake case names.

> - `@` characters are ignored.

> - **All available types** returned by the `GetValue` reader method are supported including special types like `guid`, `timespan`, etc for example.

> - **PostgreSQL arrays** are supported.

> - Mapping **to enums from numbers** is supported.

> - Mapping **to enums from strings** is supported.

> - **Enum PostgreSQL arrays** are supported (numbers and strings).

> - Nullable arrays are not supported. Mapping nullable arrays can be achieved with reader callbacks.

[See more examples in unit tests.](https://github.com/vb-consulting/Norm.net/blob/master/Tests/PostgreSqlUnitTests/ReadAnonymousUnitTests.cs)

## Other imrpovements

Slight improvements and opštimizations, see [full changelog](https://github.com/vb-consulting/Norm.net/compare/4.2.0...4.3.0) for details.

## [4.2.0](https://github.com/vb-consulting/Norm.net/tree/4.2.0) (2022-04-07)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.1.0...4.2.0)

## New feature: Collection parameters support for non PostgreSQL connections

From version 4.2.0 Norm can parse collection-type parameters for non PostgreSQL connections.

PostgreSQL supports array types natively so you can pass an array or a list parameter and it will be mapped to the PostgreSQL array type parameter.

For non-PostgreSQL connections, any collection-type parameter (a list or an array for example) will be parsed to a sequence of new parameters.

For example, following statements on SQL Server:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@list)", new[] { 1, 2, 3 });
```

Normally, this wouldn't work because SQL Server does not support those parameter types. 

But, since this version, Norm will parse this expression to three parameters instead of one:

- `@__list0` with value `1`
- `@__list1` with value `2`
- `@__list2` with value `3`

And SQL command text will replace the original parameter placeholder `@list` with CSV list of ne parameters: `SELECT * FROM Table WHERE id IN (@__list0, @__list1, @__list2)`.


So basically, this:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@list)", new[] { 1, 2, 3 });
```

is equvivalent to this:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@__list0, @__list1, @__list2)", 1, 2, 3);
```

It also works normally with named parameters in an anonymous class:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@list)", new 
{
    list = new[] { 1, 2, 3 }
});
```

Which is equvivalent to this:

```csharp
connection.ReadAsync("SELECT * FROM Table WHERE id IN (@__list0, @__list1, @__list2)", new 
{
    __list0 = 1,
    __list1 = 2,
    __list2 = 3
});
```

Any type of parameter that implements `ICollection` will work this way on non-PostgreSQL connections.

## Internal improvements

See changelog for details.

## [4.1.0](https://github.com/vb-consulting/Norm.net/tree/4.1.0) (2022-02-23)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/4.0.0...4.1.0)

## New feature: raw modifier for string interpolation overloads

When using following extensions:

- `ReadFormat`
- `ReadFormatAsync`
- `ExecuteFormat`
- `ExecuteFormatAsync`
- `MultipleFormat`
- `MultipleFormatAsync`

To set query parameters via string interpolation formats, you can now add `raw` modifier to skip parameter creation and use value "as is".

This is useful when using string interpolation to build a query and you still want to pass the parameter creation.

For example:

```csharp
var p = "xyz";
var table = "my_table";
var where = "where 1=1";
connection.ReadFormat<T>($"select * from {table:raw} {where:raw} and id = {p}");
```

Variables `table` and `where` will not create query parameter and will be parsed "as is".

## [4.0.0](https://github.com/vb-consulting/Norm.net/tree/4.0.0) (2022-02-13)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/3.3.13...4.0.0)

## **New feature - reader callbacks**

All read extension methods on your connection object (`Read` and `ReadAsync`) now have one additional overload that allows you to pass in the **lambda function with direct low-level access to the database reader.**

Lambda function with direct low-level access to the database reader has the following signature:

```csharp
Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback
```

New methods overloads look like this:

```csharp
public static IEnumerable<T> Read<T>(this DbConnection connection, 
    string command, 
    Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback);

public static IEnumerable<T> ReadFormat<T>(this DbConnection connection, 
    FormattableString command,
    Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback);

public static IEnumerable<T> Read<T>(this DbConnection connection, string command,
    Func<(string Name, int Ordinal, DbDataReader Reader), object> readerCallback,
    params object[] parameters)
```

Same overloads also exist for all 12 generic versions of `Read` and `ReadAsync` extensions.

This lambda function receives a single parameter, a tuple with three values: `(string Name, int Ordinal, DbDataReader Reader)`, where:

- `string Name` is the name of the database field currently being mapped to a value.
- `int Ordinal` is the ordinal position number, starting from zero, of the database field currently being mapped to a value.
- `DbDataReader Reader` is the low-level access to the database reader being used to read the current row.

The expected result of this reader lambda function is an object of any type that represents a **new value that will be mapped** to the results with the following rules:

- Return any value that you wish to map to result for that field.
- Return `null` to use the default, normal mapping.
- Return `DBNull.Value` to set `null` value.

This allows for the elegant use case of the C# 8 switch expressions.

### Examples

Simple example, a query that returns three records of three integers fields (`a`, `b`, `c`) - from 1 to 3:

```csharp
var query = "select * from (values (1, 1, 1), (2, 2, 2), (3, 3, 3)) t(a, b, c)";
```

Will return following results:

| a | b | c |
| - | - | - |
| 1 | 1 | 1 |
| 2 | 2 | 2 |
| 3 | 3 | 3 |

- If we want to add 1 to the first field, for example, we can add an expression like this:

```csharp
var array = connection.Read<int, int, int>(query, r => r.Ordinal switch
{
    0 => r.Reader.GetInt32(0) + 1,  // add 1 to the first field with ordinal 0, mapped to the first value
    _ => null                       // for all other fields, use default mapping
}).ToArray();
```

This example will produce the following array of `int` tuples:

| Item1 | Item2 | Item3 |
| - | - | - |
| 2 | 1 | 1 |
| 3 | 2 | 2 |
| 4 | 3 | 3 |

Same could be achieved if we use switch by the field name:

```csharp
var array = connection.Read<int, int, int>(query, r => r.Name switch
{
    "a" => r.Reader.GetInt32(0) + 1,    // add 1 to the first field with name "a", mapped to the first value
    _ => null                           // for all other fields, use default mapping
}).ToArray();
```

Same logic applies to named tuples mapping:

```csharp
var array = connection.Read<(int a, int b, int c)>(query, r => r.Ordinal switch
{
    0 => r.Reader.GetInt32(0) + 1,  // add 1 to the first field with ordinal 0, mapped to the first tuple named "a"
    _ => null                       // for all other fields, use default mapping
}).ToArray();
```

or

```csharp
var array = connection.Read<(int a, int b, int c)>(query, r => r.Name switch
{
    "a" => r.Reader.GetInt32(0) + 1,  // add 1 to the first field with name "a", mapped to the first tuple named "a"
    _ => null                         // for all other fields, use default mapping
}).ToArray();
```

Produces the following array of named `int` tuples:

| a | b | c |
| - | - | - |
| 2 | 1 | 1 |
| 3 | 2 | 2 |
| 4 | 3 | 3 |

**Important: simple values, tuples, and named tuples are still mapped by the position.**

The same technique applies also to mapping to instances of classes and records. 
Only, in this case - mapping by field name, not by position will be used. Example:


```csharp
class TestClass
{
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
}
```

```csharp
var array = connection.Read<TestClass>(query, r => r.Ordinal switch
{
    0 => r.Reader.GetInt32(0) + 1,  // add 1 to the first field. First field has name "a" and it will be mapped to property "A".
    _ => null                       // for all other fields, use default mapping
}).ToArray();
```

or

```csharp
var array = connection.Read<TestClass>(query, r => r.Name switch
{
    "a" => r.Reader.GetInt32(0) + 1,  // add 1 to the field name "a", mapped to to property "A" by name
    _ => null                         // for all other fields, use default mapping
}).ToArray();
```

This will produce an array of the `TestClass` instances with the following properties

| A | B | C |
| - | - | - |
| 2 | 1 | 1 |
| 3 | 2 | 2 |
| 4 | 3 | 3 |

And now, of course, you can utilize a pattern matching mechanism in switch expressions for C# 9:

```csharp
var array = connection.Read<TestClass>(query, r => r switch
{
    {Ordinal: 0} => r.Reader.GetInt32(0) + 1,   // add 1 to the field at the first position adn with name "a", mapped to to property "A" by name
    _ => null                                   // for all other fields, use default mapping
}).ToArray();
```

Or, for both, name and oridnal number at the same time:

```csharp
var array = connection.Read<TestClass>(query, r => r switch
{
    {Ordinal: 0, Name: "a"} => r.Reader.GetInt32(0) + 1,    // add 1 to the field name "a", mapped to to property "A" by name
    _ => null                                               // for all other fields, use default mapping
}).ToArray();
```

### Perfomance impacts

There are no performance impacts when using this feature to speak of.

These new overloads with lambda callbacks are in the new [perfomance benchmarks](https://github.com/vb-consulting/Norm.net/blob/master/PERFOMANCE-TESTS.md)
and the difference is negligible is still slightly better than the `Dapper` without any callback.

However, it's worth noting that performances will always depend on what exactly you are doing within that function, and of course, the query itself.

### Justification and use cases

**1) Database types that are hard or impossible to map to the CLR types.**

For example, **PostgreSQL** can return a nullable array that should map to a nullable array of enums.
Enums require special care since the database usually returns an integer or text that should be parsed to enums.

This functionality of mapping enum arrays is available in Norm default mapping, however, nullable enum arrays had proved to be especially difficult if not outright impossible when mapping to non-instanced types for example.

In edge cases like this, a reader callback function that can handle special types can be very useful.

**2) Special non-standard database types.**

For example, PostgreSQL can define custom database types by using extensions. Most notably is PostGIS extension that defines geometric types such as point, line, polygon, geometry collections, etc.

For these complex types, a reader callback function can be used to map to the custom types.

**3) Complex mappings**

Reader callback can return any type of object which allows you to create mappings of any complexity, depending on your needs, not on the data access capabilities.

## **Breaking Change: parameter handlings**

Parameter handling has been simplified.

**Breaking change:**

**All method overloads that supplied query parameters via named and value tuple or name, value, and type tuple are now removed.**

Instead, instance (anonymous or otherwise) parameters are now used to supply named parameters with or without a specific type.

For example, **this call will no longer work:**

```csharp
connection.Read<T>("select * from table where id = @myParam", ("myParam", myParamValue));
```

To define named paramater, instances are used:

```csharp
connection.Read<T>("select * from table where id = @myParam", new {myParam = myParamValue});
```

Note: non-anonymous instances will also work.

The first version was removed because before there were two different ways to define named parameters and it was confusing and required a lot of unnecessary method overloads.

Removing those overloads has made the library much lighter.

Also, it is easier now to define named parameters this way, especially when your variable is named same as parameter, for example:

```csharp
var myParam = 123;
connection.Read<T>("select * from table where id = @myParam", new {myParam});
```

Now, it is also possible to supply specific database type to named paramter, without having to create specific `DbParamater` type by using tuple (value and type) as parameter value:

```csharp
connection.Read<T>("select * from table where id = @myParam", new {myParam = (myParamValue, DbType.Int)});
```

Setting specific parameter type will also work with specific provider defined types:

```csharp
connection.Read<T>("select * from table where id = @myParam", new {myParam = (myParamValue, NpgsqlDbType.Integer)}); // PostgreSQL Integer type
```

This, of course, allows easy access to more "exotic" PostgreSQL parameter types, such as JSON and arrays for example:

```csharp
var test = connection.Read<string>("select @p->>'test'", 
                new
                {
                    p = ("{\"test\": \"value\"}", NpgsqlDbType.Json)
                })
                .Single();
// test == "value"

var array = connection.Read<int>("select unnest(@p)", 
                new
                {
                    p = (new List<int> { 1, 2, 3 }, NpgsqlDbType.Array | NpgsqlDbType.Integer)
                }).ToArray();

// test == int[]{1,2,3}
```

But, of course, `DbParameter` instances can be used as well:

```csharp
var test = connection.Read<string>("select @p->>'test'", 
                new
                {
                    p = new NpgsqlParameter("p", "{\"test\": \"value\"}"){ NpgsqlDbType = NpgsqlDbType.Json }
                })
                .Single();
// test == "value"

var array = connection.Read<int>("select unnest(@p)", 
                new
                {
                    p = new NpgsqlParameter("p", new List<int> { 1, 2, 3 }){ NpgsqlDbType =  NpgsqlDbType.Array | NpgsqlDbType.Integer}
                }).ToArray();

// test == int[]{1,2,3}
```

Supplying parameters in that way is a bit confusing because in this example above, a parameter is named twice, first as instance property name and the second time as actual parameter name, and only the second will be used.

So, it might be better to use the old way of declare parameters from `DbParameter` instances:

```csharp
var p1 = new NpgsqlParameter("p", "{\"test\": \"value\"}"){ NpgsqlDbType = NpgsqlDbType.Json };
var test = connection.Read<string>("select @p->>'test'", p1).Single();
// test == "value"

var p2 = new NpgsqlParameter("p", new List<int> { 1, 2, 3 }){ NpgsqlDbType =  NpgsqlDbType.Array | NpgsqlDbType.Integer};
var array = connection.Read<int>("select unnest(@p)", p2).ToArray();

// test == int[]{1,2,3}
```

Using `DbParameter` instances like this allows also for changing parameter direction to `Out` and `InOut` parameter directions.

##  **Fixed bug/missing feature: arrays mapping for named tuples**

Now it's possible to map arrays to named tuples for database provedires that support arrays. Example:


```csharp
var result = connection.Read<(int[] i, string[] s)>(@"
                select array_agg(i), array_agg(s)
                from (
                values 
                    (1, 'foo1'),
                    (2, 'foo2'),
                    (3, 'foo3')
                ) t(i, s)").ToArray();

Assert.Single(result);

Assert.Equal(1, result[0].i[0]);
Assert.Equal(2, result[0].i[1]);
Assert.Equal(3, result[0].i[2]);

Assert.Equal("foo1", result[0].s[0]);
Assert.Equal("foo2", result[0].s[1]);
Assert.Equal("foo3", result[0].s[2]);
```

This was missing from previous versions.

Maping arrays for simple values was always possible:

```csharp
var result = connection.Read<int[]>(@"
    select array_agg(e) 
    from ( values (1), (2), (3) ) t(e)
").ToArray();

Assert.Equal(1, result[0][0]);
Assert.Equal(2, result[0][1]);
Assert.Equal(3, result[0][2]);
```

Or, mapping arrays to nullable simple value arrays:


```csharp
var result = connection.Read<int?[]>(@"
    select array_agg(e) 
    from ( values (1), (null), (3) ) t(e)
").ToArray();

Assert.Equal(1, result[0][0]);
Assert.Null(result[1]);
Assert.Equal(3, result[0][2]);
```

**Important:**

**Mapping nullable arrays (arrays that contain null values) to named tuples or instance properties will not work.**

Following mappings are still unavailable out os the box:

```csharp
var query = @"
    select array_agg(i), array_agg(s) from (values 
        (1, 'foo1'),
        (null, 'foo2'),
        (2, null)
    ) t(i, s)
";

class SomeClass
{ 
    public int?[] Ints { get; set; }
    public string[] Strings { get; set; }
}

//
// These calls won't work because of int?[] mappings on named tuples and instance property
//
var result1 = connection.Read<(int?[] Ints, string[] Strings)>(query);
var result2 = connection.Read<SomeClass>(query);
```

However, new feature with reader callback can be used to solve this mapping problem:

```csharp
var result1 = connection.Read<(int?[] Ints, string[] Strings)>(query r => r.Ordinal switch
{
    0 => r.Reader.GetFieldValue<int?[]>(0),
    _ => null
});
```

##  **Mapping to enum types improvements**

Mapping to enums is a crucial task for any database mapper, but it requires special treatment.

On the database side, enums can be either text or an integer and to map them to enum types requires special parsing.

In previous versions mapping to enums on instance properties from the database, text was added. 

This version expanded enum mapping greatly. 

Now, it is possible to:

- Map from int value to enum for instance properties.
- Map from nullable int value to nullable enum for instance properties.
- Map from string array value to enum array or list for instance properties.
- Map from int array value to enum array or list for instance properties.

- Map from string value to enum for simple values.
- Map from int value to enum for simple values.
- Map from nullable int value to nullable enum for simple values.
- Map from string array value to enum array for simple values.
- Map from int array value to enum array for simple values.

However, the following mappings are still not available.

- Any array containing nulls to nullable enum for instance properties.
- Any enum mapping for named tuples.

Implementation was too difficult and costly. 
To successfully overcome these limitations, a new feature with a reader callback can be used.

Here is the full feature grid for enum mappings:

| | instance mapping `Read<ClassOrRecordType1, ClassOrRecordType2, ClassOrRecordType3>` | simple value mapping `Read<int, string, DateTime>` | named tuple mapping `Read<(int Field1, string Field3, DateTime Field3)>` |
| - | - | - | - |
| text → enum | YES | YES | NO |
| int → enum | YES | YES | NO |
| text containing nulls → nullable enum | YES | YES | NO |
| int containing nulls → nullable enum | YES | YES | NO |
| text array → enum array | YES | YES | NO |
| int array → enum array | YES | YES | NO |
| text array containing nulls → nullable enum array | NO | NO | NO |
| int array containing nulls → nullable enum array | NO | NO | NO |


To overcome these limitations, use new feature with a reader callback. Here are some examples:

```csharp
var result = connection.Read<(TestEnum Enum1, TestEnum Enum2)>(@"
select *
from (
values 
    ('Value1', 'Value3'),
    ('Value2', 'Value2'),
    ('Value3', 'Value1')
) t(Enum1, Enum2)", r => Enum.Parse<TestEnum>(r.Reader.GetFieldValue<string>(r.Ordinal))).ToArray();
```

```csharp
var result = connection.Read<TestEnum?[]>(@"
select array_agg(e) as MyEnums
from (
values 
    (0),
    (null),
    (2)
) t(e)", r =>
{
    var result = new List<TestEnum?>();
    foreach (var value in r.Reader.GetFieldValue<int?[]>(0))
    {
        if (value is null)
        {
            result.Add(null);
        }
        else
        {
            result.Add((TestEnum?)Enum.ToObject(typeof(TestEnum), value));
        }
    }
    return result.ToArray();
}).ToArray();
```

## **Breaking change: `SqlMapper` global class removed**

`SqlMapper` global class used to be previously used to inject custom mapping to the mapper.

This is no longer necessary since there is a reader callback overload for each `Read` extension. 

See examples above.

## **Timeout extension marked as obsolete**

`Timeout` is extension marked as obsolete, altough it will still work.

Instead, `WithCommandTimeout` which has much more precise and consistent naming should be used.

## **Many other internal improvements**


