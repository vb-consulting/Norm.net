# Norm.net Data Access at a glance

- [Prerequisites and Notes](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#prerequisites-and-notes)
- [Execution](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#execution)
  - [Simple execution](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#simple-execution)
  - [Chaining calls](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#chaining-calls)
- [Single row mappings](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#single-row-mappings)
  - [Single value](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#single-value)
  - [Multiple value mappings](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#multiple-value-mappings)
  - [ARRAY types (PostgreSQL only)](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#array-types-postgresql-only)
  - [Named tuples](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#named-tuples)
  - [Multiple named tuples](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#multiple-named-tuples)
  - [Named Tuples with ARRAY types (PostgreSQL only)](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#named-tuples-with-array-types-postgresql-only)
  - [Class instance properties](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#class-instance-properties)
  - [Class instance properties mapping with different naming styles (different case, snake case)](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#class-instance-properties-mapping-with-different-naming-styles-different-case-snake-case)
  - [Ignored class instance properties](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#ignored-class-instance-properties)
  - [Multiple class instances](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#multiple-class-instances)
  - [Multiple class instances with the same name](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#multiple-class-instances-with-the-same-name)
  - [Class instances and map ARRAY types (PostgreSQL only)](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#class-instances-and-map-array-types-postgresql-only)
  - [Record instances](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#record-instances)
- [Multiple values](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#multiple-values)
  - [Iterations](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#iterations)
  - [Mapping to lists and array](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#mapping-to-lists-and-array)
  - [Mapping to a dictionary](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#mapping-to-a-dictionary)
  - [Mapping to a dictionary using named tuples](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#mapping-to-a-dictionary-using-named-tuples)
  - [Using GroupBy with the class instances](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#using-groupby-with-the-class-instances)
- [Batching multiple commands](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#batching-multiple-commands)
- [Database command Parameters](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#database-command-parameters)
  - [Using formattable strings to supply parameters](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#using-formattable-strings-to-supply-parameters)
  - [Using formattable strings to supply native database parameters](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#using-formattable-strings-to-supply-native-database-parameters)
  - [Simple parameters by position](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#simple-parameters-by-position)
  - [Simple parameters by position and native database parameters](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#simple-parameters-by-position-and-native-database-parameters)
  - [Passing class instance and mapping values to parameters](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#passing-class-instance-and-mapping-values-to-parameters)
  - [Mixing class instance, database native, and simple parameters](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#mixing-class-instance-database-native-and-simple-parameters)
  - [Mapping parameters by specific name](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#mapping-parameters-by-specific-name)
  - [Mapping parameters by specific name and specific database type](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#mapping-parameters-by-specific-name-and-specific-database-type)
  - [Mapping parameters by specific name and specific custom database type](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#mapping-parameters-by-specific-name-and-specific-custom-database-type)
- [Command object parameters](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#command-object-parameters)
  - [Executing stored procedure (or function) command type](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#executing-stored-procedure-or-function-command-type)
  - [Command type](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#command-type)
  - [Command timeout](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#command-timeout)
  - [Passing the cancellation token](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#passing-the-cancellation-token)
  - [Prepared statements](https://github.com/vb-consulting/Norm.net/blob/master/EXAMPLES.md#prepared-statements)

## Prerequisites and Notes

- All Norm read operations are returning `IEnumerable<?>` or `IAsyncEnumerable<?>` for asynchronous operations.

- Values are yielded to the resulting enumerator as they appear on your database connection.

- To transform values from those resulting enumeration (without manual `foreach`-es), use the `Linq` extensions:

```csharp
using System.Linq;
using Norm;
```

- Using `Linq` extensions to transforms your values does not add extra iteration over your values, since they are yielded.

- To be able to use `Linq` extensions for `IAsyncEnumerable<?>` types and transform values from asynchronous operations you will need to add `System.Linq.Async` package to your project.

```cs
dotnet add package System.Linq.Async
```

- This applies only to frameworks before .NET 6.0, which apparently, has this package embedded with the framework itself.

- System.Linq.Async shares the same namespace with the `System.Linq`, so no new `using` is needed.

## Execution

- Methods `Execute` and `ExecuteAsync` do not return values.

### Simple execution

```csharp
connection.Execute("sql statements to execute");
```

```csharp
await connection.ExecuteAsync("sql statements to execute");
```

### Chaining calls

```csharp
connection
    .Execute("begin tran")
    .Execute("create temp table test (t text)")
    .Execute("insert into test values ('foo')")
    .Execute("rollback");
```

## Single row mappings

- Use any of the `Linq` extensions available to get a single value from the enumeration, such as `First`, `Single`, `FirstOrDefault` or `SingleOrDefault`.

- Single row mappings demonstrate different mapping techniques like values, names tuples, class instances, etc

### Single value

```csharp
var value1 = connection.Read<int>("select 1").FirstOrDefault();
```

```csharp
var value1 = await connection.ReadAsync<int>("select 1").FirstOrDefaultAsync();
```

### Multiple value mappings

- Up to 12 values the most.
- Mapped by position (name is not present).

```csharp
var (number1, str1, date1) = connection.Read<int, string, DateTime>("select 1, 'str', '2021-18-10'").FirstOrDefault();
var (v1, v2, v3, v4) = connection.Read<int, int, int, int>("select 1, 2, 3, 4").FirstOrDefault();
```

```csharp
var (number1, str1, date1) = await connection.ReadAsync<int, string, DateTime>("select 1, 'str', '2021-18-10'").FirstOrDefaultAsync();
var (v1, v2, v3, v4) = await connection.ReadAsync<int, int, int, int>("select 1, 2, 3, 4").FirstOrDefaultAsync();
```

### ARRAY types (PostgreSQL only)

```csharp
var arr = connection.Read<int[]>("select array[1, 2]").FirstOrDefault();
var (arr1, arr2) = connection.Read<int[], int[]>("select array[1, 2], array[1, 2, 3, 4]").FirstOrDefault();

Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", arr[0], arr[1], arr1[0], arr1[1], arr2[0], arr2[1], arr2[2], arr2[3]); 
```

### Named tuples

- If you use `Read<T1, T2>`, the result will be an unnamed tuple that you can map to single values: `var (t1, t2) = conn.Read<T1, T2>(sql);`
- If you use `Read<(T1, T2)>`, the result will be named tuple which will have IntelliSense autocomplete enabled from your IDE.
- Named tuples are also mapped by position, not by name.

```csharp
var myTuple = connection.Read<(int Value1, int Value2, int Value3)>("select 1, 2, 3").FirstOrDefault();

// intellisense and autocomplete available at this point
Console.WriteLine("{0}, {1}, {2}", myTuple.Value1, myTuple.Value2, myTuple.Value3); 
```

### Multiple named tuples

- You can map multiple named tuples at the same time, up to 12 tuples the most:

```csharp
var (myTuple1, myTuple2) = connection.Read<(int Value1, int Value2), (int Value1, int Value2, int Value3)>("select 1, 2, 3, 4, 5, 6").FirstOrDefault();

// intellisense and autocomplete available at this point
Console.WriteLine("{0}, {1}, {2}, {3}, {4}", myTuple1.Value1, myTuple1.Value2, myTuple2.Value1, myTuple2.Value2, myTuple2.Value3); 
```

### Named Tuples with ARRAY types (PostgreSQL only)

```csharp
var tuple = connection.Read<(int[] Arr1, int[] Arr2)>("select array[1, 2], array[1, 2, 3, 4]").FirstOrDefault();

Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}", tuple.Arr1[0], tuple.Arr1[1], tuple.Arr2[0], tuple.Arr2[1], tuple.Arr2[2], tuple.Arr2[3]);
```

### Class instance properties

- Unlimited number of class instance properties (limited only by .NET internal number of class properties)of any type.

- **Mapped by name** (instead of position):

```csharp
public class MyClass
{
    public int Value1 { get; set; }
    public int Value2 { get; set; }
}

var instance = connection.Read<MyClass>("select 1 as Value1, 2 as Value2").FirstOrDefault();
var instance = await connection.ReadAsync<MyClass>("select 1 as Value1, 2 as Value2").FirstOrDefaultAsync();

Console.WriteLine("{0}, {1}", instance.Value1, instance.Value2); 
```

### Class instance properties mapping with different naming styles (different case, snake case)

- Mapping by name is case insensitive.
- Mapping by name works with snake case naming out of the box.

```csharp
using System.Linq;

public class MyClass
{
    public int MyValue1 { get; set; }
    public int MyValue2 { get; set; }
}

var instance = connection.Read<MyClass>("@select
    1 as myvalue1,
    2 as my_value_2,
    3 as my_value_3").FirstOrDefault();

//
// myvalue1 is mapped MyValue1 with different name casing
// my_value_2 (snake cased) is mapped to MyValue2 
// my_value_3 is ignored since it is not found
//

Console.WriteLine("{0}, {1}", instance.MyValue1, instance.MyValue2); 
```

### Ignored class instance properties

- Only properties with the public setter are mapped

```csharp
public class MyClass
{
    public int Value1 { get; set; }             // mapped
    public int Value2 { get; init; }            // mapped
    public int Value3;                          // not mapped
    public int Value4 { get; }                  // not mapped
    public int Value5 { get; private set; }     // not mapped
    public int Value6 { get; protected set; }   // not mapped
    public int Value6 { get; internal set; }    // not mapped
}

var instance = connection.Read<MyClass>(@"select 
                                            1 as value1, 
                                            2 as value2, 
                                            3 as value3, 
                                            4 as value4, 
                                            5 as value5, 
                                            6 as value6, 
                                            7 as value7").FirstOrDefault();
```

### Multiple class instances

- Map up to 12 class instances at the same time.
- Class instance properties are always **mapped by name.**

```csharp
public class MyClass1
{
    public int Value1 { get; set; }
    public int Value2 { get; set; }
}

public class MyClass2
{
    public int Value3 { get; set; }
    public int Value4 { get; set; }
}

var (instance1, instance2) = connection.Read<MyClass1, MyClass2>(@"select 1 as value1, 2 as value2, 3 as value3, 4 as value4").FirstOrDefault();

Console.WriteLine("{0}, {1}, {2}, {3}", instance1.Value1, instance1.Value2, instance2.Value3, instance2.Value4); 
// outputs 1, 2, 3, 4
```

### Multiple class instances with the same name

- When mapping multiple class instances, properties with the same name are mapped by the position they appear.
- From left to right: `MyClass1` maps the first appearances of `value1` and `value2` as they appear in the query from left to right.


```csharp
public class MyClass1
{
    public int Value1 { get; set; }
    public int Value2 { get; set; }
}

public class MyClass2
{
    public int Value1 { get; set; }
    public int Value2 { get; set; }
}

var (instance1, instance2) = connection.Read<MyClass1, MyClass2>(@"select 1 as value1, 2 as value2, 3 as value1, 4 as value2").FirstOrDefault();

Console.WriteLine("{0}, {1}, {2}, {3}", instance1.Value1, instance1.Value2, instance2.Value1, instance2.Value2); 
// outputs 1, 2, 3, 4
```

### Class instances and map ARRAY types (PostgreSQL only)

- Array types are mapped normally

```csharp
public class MyClass
{
    public int[] Array1 { get; set; }
    public int[] Array2 { get; set; }
}

var instance = connection.Read<MyClass>(@"select array[1,2] as array1, array[3,4] as array2").FirstOrDefault();

Console.WriteLine("{0}, {1}, {2}, {3}", instance.Array1[0], instance.Array1[1], instance.Array2[0], instance.Array2[1]); 
```

### Record instances

- Everything true for class instance mapping is also true for Record instance mapping:
  - Mapping by name (case insensitive, snake case, etc)
  - Multiple mappings
  - Array mappings
  - etc

```csharp
public record MyRecord(int Value1, int Value2);

var instance = connection.Read<MyRecord>("select 1 as value1, 2 as value2").FirstOrDefault();
var instance = await connection.ReadAsync<MyRecord>("select 1 as value1, 2 as value2").FirstOrDefaultAsync();

Console.WriteLine("{0}, {1}", instance.Value1, instance.Value2);
```

## Multiple values

- Everything in regards to mapping is also true when returning multiple records from your query

### Iterations

- Mapped by position, no field name is required in a query:

```csharp
var query = "select * from (values (1, 'a', 'x'), (2, 'b', 'y'), (3, 'c', 'z')) t";

foreach (var (id, str1, str2) in connection.Read<int, string, string>(query))
{
    Console.WriteLine("{0}, {1}, {2}", id, str1, str2);
}

await foreach(var (id, str1, str2) in connection.ReadAsync<int, string, string>(query))
{
    Console.WriteLine("{0}, {1}, {2}", id, str1, str2);
}

foreach(var tuple in connection.Read<(int Id, string Str1, string Str2)>(query))
{
    Console.WriteLine("{0}, {1}, {2}", tuple.Id, tuple.Str1, tuple.Str2);
}

await foreach(var tuple in connection.ReadAsync<(int Id, string Str1, string Str2)>(query))
{
    Console.WriteLine("{0}, {1}, {2}", tuple.Id, tuple.Str1, tuple.Str2);
}
```

- Mapped by name, the field name is required in a query:

```csharp
var query = "select * from (values (1, 'a', 'x'), (2, 'b', 'y'), (3, 'c', 'z')) t (id, str1, str2)";

public class MyClass
{
    public int Id { get; set; }
    public string Str1 { get; set; }
    public string Str2 { get; set; }
}

foreach (var instance in connection.Read<MyClass>(query))
{
    Console.WriteLine("{0}, {1}, {2}", instance.Id, instance.Str1, instance.Str2);
}

await foreach (var instance in connection.ReadAsync<MyClass>(query))
{
    Console.WriteLine("{0}, {1}, {2}", instance.Id, instance.Str1, instance.Str2);
}

public record MyRecord(int Id, string Str1, string Str2);

foreach (var instance in connection.Read<MyRecord>(query))
{
    Console.WriteLine("{0}, {1}, {2}", instance.Id, instance.Str1, instance.Str2);
}

await foreach (var instance in connection.ReadAsync<MyRecord>(query))
{
    Console.WriteLine("{0}, {1}, {2}", instance.Id, instance.Str1, instance.Str2);
}
```

### Mapping to lists and array

- Using `Linq` to map to any other data structure will do, such as `ToHashset` for example

```csharp
var list = connection.Read<T>(query).ToList();
var array = connection.Read<T>(query).ToArray();
```

```csharp
var list = await connection.ReadAsync<T>(query).ToListAsync();
var array = await connection.ReadAsync<T>(query).ToArrayAsync();
```

### Mapping to a dictionary

- Reading generic values yields unnamed tuples, so, to map to dictionary generic names `Item1` and `Item2` must be used.

```csharp
var dict = connection.Read<int, string>("select id, value from my_table").ToDictionary(t => t.Item1, t => t.Item2);
```

```csharp
var dict = await connection.ReadAsync<int, string>("select id, value from my_table").ToDictionaryAsync(t => t.Item1, t => t.Item2);
```

### Mapping to a dictionary using named tuples

```csharp
var dict = connection.Read<(int Key, string Value)>("select id, value from my_table").ToDictionary(t => t.Key, t => t.Value);
```

```csharp
var dict = await connection.ReadAsync<(int Key, string Value)>("select id, value from my_table").ToDictionaryAsync(t => t.Key, t => t.Value);
```

### Using GroupBy with the class instances

- Note: any other `Linq` extension that transforms enumeration will do.

```csharp
public class MyClass
{
    public int Key { get; set; };
    // other properties
}

var grouped = connection.Read<MyClass>(sql).GroupBy(g => g.Key);
```

## Batching multiple commands

- Batches are executed by using `Multiple(Async)` extension that returns a disposable object that can do `Execute(Async)` and/or `Read(Async)`.
- SQL Commands (and potential parameters) are supplied to `Multiple(Async)` extension, subsequent `Execute(Async)` and/or `Read(Async)` calls are parameterless. 

```csharp
using var multiple = connection.Multiple(@"
    select 1, 2, 3;
    begin tran;
    create temp table test (t text);
    insert into test values ('foo');
    select * from values;
    rollback;");

var (one, two, three) = multiple.Read<int, int, int>();
multiple.Execute();
multiple.Execute();
multiple.Execute();
var valuefromTran = multiple.Read<string>().Single();
multiple.Execute();
```

## Database command parameters

- Parameters can be supplied to following extensions:
  - `Execute` and `ExecuteAsync`
  - `Read` and `ReadAsync`
  - `Multiple` and `MultipleAsync`
  - `ExecuteFormat` and `ExecuteFormatAsync`
  - `ReadFormat` and `ReadFormatAsync`
  - `MultipleFormat` and `MultipleFormatAsync`

### Using formattable strings to supply parameters

- You can supply parameters using formattable string commands by using extension versions with `Format` suffix:

```csharp
connection.ReadFormat<T>($"select {1}, {2}, {3}");
connection.ReadFormat<T>($"select * from table where id = {1}");

var p = "xyz";
connection.ReadFormat<T>($"select * from table where id = {p}");
```

### Using formattable strings to supply native database parameters

- Native parameters allow you to set more precise parameter types.
- Note: Native parameters may require parameter names, which is in this case meaningless.
- Note2: you can mix simple and native parameters in formattable strings

```csharp
connection.ReadFormat<T>($"select {new SqlParameter("p1", 1)}, {new SqlParameter("p2", 1)}, {new SqlParameter("p3", 3)}");
connection.ReadFormat<T>($"select * from table where id = {1}");

var p = "xyz";
connection.ReadFormat<T>($"select * from table where id = {new SqlParameter("p1", 1)}");
```

### Simple parameters by position

- Note: parameter names are meaningless but required since they are mapped by position.

```csharp
connection.Read<T>("select @p1, @p2, @p3", 1, 2, 3);
connection.Read<T>("select * from table where id = @p", 1);

var p = "xyz";
connection.Read<T>("select * from table where id = @p", p);
```

### Simple parameters by position and native database parameters

- If the supplied parameter is a native database parameter, it will be interpreted as such
- In this case, parameter name matters

```csharp
connection.Read<T>("select @p1, @p2, @p3", new SqlParameter("p1", 1), new SqlParameter("p2", 1), {new SqlParameter("p3", 3));
connection.Read<T>("select * from table where id = @p", new SqlParameter("p1", 1));

var p =  new SqlParameter("p", 1)
connection.Read<T>("select * from table where id = @p", p);
```

### Passing class instance and mapping values to parameters

- In this case, parameter name matters

```csharp
public class MyClass
{
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string Param3 { get; set; }
}

var instance = new MyClass{ Param1 = "value1", Param2 = "value2", Param3 = "value3" };

connection.Read<T>("select @Param1, @Param2, @Param3", instance);
```

### Mixing class instance, database native, and simple parameters

- Instance parameters and native database parameters are mapped by name first and what remains is mapped by position.

```csharp
public class MyClass
{
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string Param3 { get; set; }
}

var instance = new MyClass{ Param1 = "value1", Param2 = "value2", Param3 = "value3" };

connection.Read<T>("select @X, @Param1, @Param2, @Param3, @Y", instance, new SqlParameter("X", 1), "Y");
// outputs 1, "value1", "value2", "value3", "Y"
```

### Mapping parameters by specific name

```csharp
connection.Read<T>("select @p1, @p2, @p3", ("p1", 1), ("p2", 2), ("p3", 1));
connection.Read<T>("select * from table where id = @p", ("p", 1));

var p =  new SqlParameter("p", 1)
connection.Read<T>("select * from table where id = @p", ("p", p));
```

### Mapping parameters by specific name and specific database type

```csharp
connection.Read<T>("select @p1, @p2, @p3", ("p1", 1, DbType.Int32), ("p2", 2, DbType.Int32), ("p3", 1, DbType.Int32));
connection.Read<T>("select * from table where id = @p", ("p", 1, DbType.Int32));

var p =  new SqlParameter("p", 1)
connection.Read<T>("select * from table where id = @p", ("p", p, DbType.Int32));
```

### Mapping parameters by specific name and specific custom database type

```csharp
connection.Read<T>("select @p1, @p2, @p3", ("p1", 1, NpgsqlDbType.Integer), ("p2", 2, NpgsqlDbType.Integer), ("p3", 1, NpgsqlDbType.Integer));
connection.Read<T>("select * from table where id = @p", ("p", 1, NpgsqlDbType.Integer));

var p =  new SqlParameter("p", 1)
connection.Read<T>("select * from table where id = @p", ("p", p, NpgsqlDbType.Integer));
```

## Command object parameters

### Executing stored procedure (or function) command type

- `AsProcedure` sets all command types to the procedure for this connection instance.

```csharp
connection.AsProcedure().Read<T>(name, parameters);
```

### Command type

- `As` sets command type to specific type for all commands in this connection instance.
- Command types can be `StoredProcedure`, `TableDirect` or `Text`.

```csharp
using System.Data;

connection.As(CommandType.TableDirect).Read<T>("my_table");
```

### Command timeout

- Sets all database commands for this connection to wait time in seconds given by timeout parameter.

```csharp
var timeoutSec = 100; // 100 seconds timeout for this connection
connection.Timeout(timeoutSec).Read<T>(sql, parameters);
```

### Passing the cancellation token

- Sets the token to monitor for cancellation requests for all async database commands for this connection.

```csharp
var timeoutSec = 100; // 100 seconds timeout for this connection
connection.WithCancellationToken(cancellationToken).Execute(sql, parameters);
```

### Prepared statements

- Sets all database commands for this connection to prepared mode.

```csharp
connection.Prepared().Execute(sql, parameters);
```
