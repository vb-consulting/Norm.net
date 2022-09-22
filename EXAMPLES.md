<!-- vscode-markdown-toc -->
* 1. [Prerequisites and Notes](#PrerequisitesandNotes)
* 2. [Execution](#Execution)
	* 2.1. [Simple execution](#Simpleexecution)
	* 2.2. [Chaining calls](#Chainingcalls)
* 3. [Single row mappings](#Singlerowmappings)
	* 3.1. [Single value](#Singlevalue)
	* 3.2. [Multiple value mappings](#Multiplevaluemappings)
	* 3.3. [ARRAY types (PostgreSQL only)](#ARRAYtypesPostgreSQLonly)
	* 3.4. [Named tuples](#Namedtuples)
	* 3.5. [Multiple named tuples](#Multiplenamedtuples)
	* 3.6. [Named Tuples with ARRAY types (PostgreSQL only)](#NamedTupleswithARRAYtypesPostgreSQLonly)
	* 3.7. [Class instance properties](#Classinstanceproperties)
	* 3.8. [Class instance properties mapping with different naming styles (different case, snake case)](#Classinstancepropertiesmappingwithdifferentnamingstylesdifferentcasesnakecase)
	* 3.9. [Ignored class instance properties](#Ignoredclassinstanceproperties)
	* 3.10. [Multiple class instances](#Multipleclassinstances)
	* 3.11. [Multiple class instances with the same name](#Multipleclassinstanceswiththesamename)
	* 3.12. [Class instances and map ARRAY types (PostgreSQL only)](#ClassinstancesandmapARRAYtypesPostgreSQLonly)
	* 3.13. [Record instances](#Recordinstances)
* 4. [Multiple values](#Multiplevalues)
* 5. [Anonymous type instances](#Anonymoustypeinstances)
	* 5.1. [Iterations](#Iterations)
	* 5.2. [Mapping to lists and array](#Mappingtolistsandarray)
	* 5.3. [Mapping to a dictionary](#Mappingtoadictionary)
	* 5.4. [Mapping to a dictionary using named tuples](#Mappingtoadictionaryusingnamedtuples)
	* 5.5. [Using GroupBy with the class instances](#UsingGroupBywiththeclassinstances)
* 6. [Batching multiple commands](#Batchingmultiplecommands)
* 7. [Database command parameters](#Databasecommandparameters)
	* 7.1. [Using formattable strings to supply parameters](#Usingformattablestringstosupplyparameters)
	* 7.2. [Using formattable strings to supply parameters and skipping parameters (raw)](#Usingformattablestringstosupplyparametersandskippingparametersraw)
	* 7.3. [Using formattable strings to supply native database parameters](#Usingformattablestringstosupplynativedatabaseparameters)
	* 7.4. [Simple parameters by position](#Simpleparametersbyposition)
	* 7.5. [Simple parameters by position and native database parameters](#Simpleparametersbypositionandnativedatabaseparameters)
	* 7.6. [Passing class instance and mapping values to parameters](#Passingclassinstanceandmappingvaluestoparameters)
	* 7.7. [Passing anonymous class instance and mapping values to parameters](#Passinganonymousclassinstanceandmappingvaluestoparameters)
	* 7.8. [Passing anonymous class instance and mapping values to parameters](#Passinganonymousclassinstanceandmappingvaluestoparameters-1)
	* 7.9. [Mixing class instance, database native, and simple parameters](#Mixingclassinstancedatabasenativeandsimpleparameters)
	* 7.10. [Using anonymous class instance to provide value, specific name and specific database type](#Usinganonymousclassinstancetoprovidevaluespecificnameandspecificdatabasetype)
	* 7.11. [Mapping parameters by specific name and specific custom database type](#Mappingparametersbyspecificnameandspecificcustomdatabasetype)
* 8. [Using reader callback](#Usingreadercallback)
* 9. [Command object options](#Commandobjectoptions)
	* 9.1. [Command type](#Commandtype)
	* 9.2. [Command timeout](#Commandtimeout)
	* 9.3. [Passing the cancellation token](#Passingthecancellationtoken)
	* 9.4. [Prepared statements](#Preparedstatements)


# Norm.net Data Access at a glance

##  1. <a name='PrerequisitesandNotes'></a>Prerequisites and Notes

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

##  2. <a name='Execution'></a>Execution

- Methods `Execute` and `ExecuteAsync` do not return values.

###  2.1. <a name='Simpleexecution'></a>Simple execution

```csharp
connection.Execute("sql statements to execute");
```

```csharp
await connection.ExecuteAsync("sql statements to execute");
```

###  2.2. <a name='Chainingcalls'></a>Chaining calls

```csharp
connection
    .Execute("begin tran")
    .Execute("create temp table test (t text)")
    .Execute("insert into test values ('foo')")
    .Execute("rollback");
```

##  3. <a name='Singlerowmappings'></a>Single row mappings

- Use any of the `Linq` extensions available to get a single value from the enumeration, such as `First`, `Single`, `FirstOrDefault` or `SingleOrDefault`.

- Single row mappings demonstrate different mapping techniques like values, names tuples, class instances, etc

###  3.1. <a name='Singlevalue'></a>Single value

```csharp
var value1 = connection.Read<int>("select 1").FirstOrDefault();
```

```csharp
var value1 = await connection.ReadAsync<int>("select 1").FirstOrDefaultAsync();
```

###  3.2. <a name='Multiplevaluemappings'></a>Multiple value mappings

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

###  3.3. <a name='ARRAYtypesPostgreSQLonly'></a>ARRAY types (PostgreSQL only)

```csharp
var arr = connection.Read<int[]>("select array[1, 2]").FirstOrDefault();
var (arr1, arr2) = connection.Read<int[], int[]>("select array[1, 2], array[1, 2, 3, 4]").FirstOrDefault();

Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", arr[0], arr[1], arr1[0], arr1[1], arr2[0], arr2[1], arr2[2], arr2[3]); 
```

###  3.4. <a name='Namedtuples'></a>Named tuples

- If you use `Read<T1, T2>`, the result will be an unnamed tuple that you can map to single values: `var (t1, t2) = conn.Read<T1, T2>(sql);`
- If you use `Read<(T1, T2)>`, the result will be named tuple which will have IntelliSense autocomplete enabled from your IDE.
- Named tuples are also mapped by position, not by name.

```csharp
var myTuple = connection.Read<(int Value1, int Value2, int Value3)>("select 1, 2, 3").FirstOrDefault();

// intellisense and autocomplete available at this point
Console.WriteLine("{0}, {1}, {2}", myTuple.Value1, myTuple.Value2, myTuple.Value3); 
```

###  3.5. <a name='Multiplenamedtuples'></a>Multiple named tuples

- You can map multiple named tuples at the same time, up to 12 tuples the most:

```csharp
var (myTuple1, myTuple2) = connection.Read<(int Value1, int Value2), (int Value1, int Value2, int Value3)>("select 1, 2, 3, 4, 5, 6").FirstOrDefault();

// intellisense and autocomplete available at this point
Console.WriteLine("{0}, {1}, {2}, {3}, {4}", myTuple1.Value1, myTuple1.Value2, myTuple2.Value1, myTuple2.Value2, myTuple2.Value3); 
```

###  3.6. <a name='NamedTupleswithARRAYtypesPostgreSQLonly'></a>Named Tuples with ARRAY types (PostgreSQL only)

```csharp
var tuple = connection.Read<(int[] Arr1, int[] Arr2)>("select array[1, 2], array[1, 2, 3, 4]").FirstOrDefault();

Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}", tuple.Arr1[0], tuple.Arr1[1], tuple.Arr2[0], tuple.Arr2[1], tuple.Arr2[2], tuple.Arr2[3]);
```

###  3.7. <a name='Classinstanceproperties'></a>Class instance properties

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

###  3.8. <a name='Classinstancepropertiesmappingwithdifferentnamingstylesdifferentcasesnakecase'></a>Class instance properties mapping with different naming styles (different case, snake case)

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

###  3.9. <a name='Ignoredclassinstanceproperties'></a>Ignored class instance properties

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

###  3.10. <a name='Multipleclassinstances'></a>Multiple class instances

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

###  3.11. <a name='Multipleclassinstanceswiththesamename'></a>Multiple class instances with the same name

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

###  3.12. <a name='ClassinstancesandmapARRAYtypesPostgreSQLonly'></a>Class instances and map ARRAY types (PostgreSQL only)

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

###  3.13. <a name='Recordinstances'></a>Record instances

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

##  4. <a name='Multiplevalues'></a>Multiple values

- Everything in regards to mapping is also true when returning multiple records from your query

##  5. <a name='Anonymoustypeinstances'></a>Anonymous type instances

var instance = connection.Read(new
{ 
    Value1 = default(int), 
    Value2 = default(int) 
}, "select 1 as Value1, 2 as Value2").FirstOrDefault();

var instance = await connection.ReadAsync<MyClass>(new
{ 
    Value1 = default(int), 
    Value2 = default(int)
}, "select 1 as Value1, 2 as Value2").FirstOrDefaultAsync();

Console.WriteLine("{0}, {1}", instance.Value1, instance.Value2); 

###  5.1. <a name='Iterations'></a>Iterations

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

###  5.2. <a name='Mappingtolistsandarray'></a>Mapping to lists and array

- Using `Linq` to map to any other data structure will do, such as `ToHashset` for example

```csharp
var list = connection.Read<T>(query).ToList();
var array = connection.Read<T>(query).ToArray();
```

```csharp
var list = await connection.ReadAsync<T>(query).ToListAsync();
var array = await connection.ReadAsync<T>(query).ToArrayAsync();
```

###  5.3. <a name='Mappingtoadictionary'></a>Mapping to a dictionary

- Reading generic values yields unnamed tuples, so, to map to dictionary generic names `Item1` and `Item2` must be used.

```csharp
var dict = connection.Read<int, string>("select id, value from my_table").ToDictionary(t => t.Item1, t => t.Item2);
```

```csharp
var dict = await connection.ReadAsync<int, string>("select id, value from my_table").ToDictionaryAsync(t => t.Item1, t => t.Item2);
```

###  5.4. <a name='Mappingtoadictionaryusingnamedtuples'></a>Mapping to a dictionary using named tuples

```csharp
var dict = connection.Read<(int Key, string Value)>("select id, value from my_table").ToDictionary(t => t.Key, t => t.Value);
```

```csharp
var dict = await connection.ReadAsync<(int Key, string Value)>("select id, value from my_table").ToDictionaryAsync(t => t.Key, t => t.Value);
```

###  5.5. <a name='UsingGroupBywiththeclassinstances'></a>Using GroupBy with the class instances

- Note: any other `Linq` extension that transforms enumeration will do.

```csharp
public class MyClass
{
    public int Key { get; set; };
    // other properties
}

var grouped = connection.Read<MyClass>(sql).GroupBy(g => g.Key);
```

##  6. <a name='Batchingmultiplecommands'></a>Batching multiple commands

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

##  7. <a name='Databasecommandparameters'></a>Database command parameters

- Parameters can be supplied to following extensions:
  - `Execute` and `ExecuteAsync`
  - `Read` and `ReadAsync`
  - `Multiple` and `MultipleAsync`
  - `ExecuteFormat` and `ExecuteFormatAsync`
  - `ReadFormat` and `ReadFormatAsync`
  - `MultipleFormat` and `MultipleFormatAsync`

###  7.1. <a name='Usingformattablestringstosupplyparameters'></a>Using formattable strings to supply parameters

- You can supply parameters using formattable string commands by using extension versions with `Format` suffix:

```csharp
connection.ReadFormat<T>($"select {1}, {2}, {3}");
connection.ReadFormat<T>($"select * from table where id = {1}");

var p = "xyz";
connection.ReadFormat<T>($"select * from table where id = {p}");
```

###  7.2. <a name='Usingformattablestringstosupplyparametersandskippingparametersraw'></a>Using formattable strings to supply parameters and skipping parameters (raw)

```csharp
var p = "xyz";
var table = "my_table";
var where = "where 1=1";
connection.ReadFormat<T>($"select * from {table:raw} {where:raw} and id = {p}");
```

###  7.3. <a name='Usingformattablestringstosupplynativedatabaseparameters'></a>Using formattable strings to supply native database parameters

- Native parameters allow you to set more precise parameter types.
- Note: Native parameters may require parameter names, which is in this case meaningless.
- Note2: you can mix simple and native parameters in formattable strings

```csharp
connection.ReadFormat<T>($"select {new SqlParameter("p1", 1)}, {new SqlParameter("p2", 1)}, {new SqlParameter("p3", 3)}");
connection.ReadFormat<T>($"select * from table where id = {1}");

var p = "xyz";
connection.ReadFormat<T>($"select * from table where id = {new SqlParameter("p1", 1)}");
```

###  7.4. <a name='Simpleparametersbyposition'></a>Simple parameters by position

- Note: parameter names are meaningless but required since they are mapped by position.

```csharp
connection.Read<T>("select @p1, @p2, @p3", 1, 2, 3);
connection.Read<T>("select * from table where id = @p", 1);

var p = "xyz";
connection.Read<T>("select * from table where id = @p", p);
```

###  7.5. <a name='Simpleparametersbypositionandnativedatabaseparameters'></a>Simple parameters by position and native database parameters

- If the supplied parameter is a native database parameter, it will be interpreted as such
- In this case, parameter name matters

```csharp
connection.WithParameters(new SqlParameter("p1", 1), new SqlParameter("p2", 1), new SqlParameter("p3", 3)).Read<T>("select @p1, @p2, @p3");
connection.Read<T>("select * from table where id = @p", new SqlParameter("p1", 1));

var p =  new SqlParameter("p", 1)
connection.Read<T>("select * from table where id = @p", p);
```

###  7.6. <a name='Passingclassinstanceandmappingvaluestoparameters'></a>Passing class instance and mapping values to parameters

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

###  7.7. <a name='Passinganonymousclassinstanceandmappingvaluestoparameters'></a>Passing anonymous class instance and mapping values to parameters

```csharp
connection.Read<T>("select @Param1, @Param2, @Param3", new { param1 = "value1", param2 = "value2", param3 = "value3" });
```

###  7.8. <a name='Passinganonymousclassinstanceandmappingvaluestoparameters-1'></a>Passing anonymous class instance and mapping values to parameters

```csharp
var param1 = "value1";
var param2 = "value2";
var param3 = "value3";
connection.Read<T>("select @Param1, @Param2, @Param3", new { param1, param2, param3 });
```


###  7.9. <a name='Mixingclassinstancedatabasenativeandsimpleparameters'></a>Mixing class instance, database native, and simple parameters

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


###  7.10. <a name='Usinganonymousclassinstancetoprovidevaluespecificnameandspecificdatabasetype'></a>Using anonymous class instance to provide value, specific name and specific database type

```csharp
connection.Read<T>("select @p1, @p2, @p3", new { p1 = (1, DbType.Int32), p2 = (2, DbType.Int32), p3 = (1, DbType.Int32)}
connection.Read<T>("select * from table where id = @p", new {p = (1, DbType.Int32)});
```

###  7.11. <a name='Mappingparametersbyspecificnameandspecificcustomdatabasetype'></a>Mapping parameters by specific name and specific custom database type

```csharp
connection.Read<T>("select @p1, @p2, @p3", new { p1 =, (1, NpgsqlDbType.Integer), p2 = (2, NpgsqlDbType.Integer), p3 = (1, NpgsqlDbType.Integer));
connection.Read<T>("select * from table where id = @p", ("p", 1, NpgsqlDbType.Integer));

connection.Read<T>("select @p1, @p2, @p3", new { p1 = (1, NpgsqlDbType.Integer), p2 = (2, NpgsqlDbType.Integer), p3 = (1, NpgsqlDbType.Integer)}
connection.Read<T>("select * from table where id = @p", new {p = (1, NpgsqlDbType.Integer)});
```

##  8. <a name='Usingreadercallback'></a>Using reader callback

You can pass reader callback lambda function to:
- Change the value or type
- Handle complex mappings, not available otherwise.
- Create complex objects.

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

##  9. <a name='Commandobjectoptions'></a>Command object options
- `AsProcedure` sets all command types to the procedure for this connection instance.

```csharp
connection.AsProcedure().Read<T>(name, parameters);
```

###  9.1. <a name='Commandtype'></a>Command type

- `As` sets command type to specific type for all commands in this connection instance.
- Command types can be `StoredProcedure`, `TableDirect` or `Text`.

```csharp
using System.Data;

connection.As(CommandType.TableDirect).Read<T>("my_table");
```

###  9.2. <a name='Commandtimeout'></a>Command timeout

- Sets all database commands for this connection to wait time in seconds given by timeout parameter.

```csharp
var timeoutSec = 100; // 100 seconds timeout for this connection
connection.WithCommandTimeout(timeoutSec).Read<T>(sql, parameters);
```

###  9.3. <a name='Passingthecancellationtoken'></a>Passing the cancellation token

- Sets the token to monitor for cancellation requests for all async database commands for this connection.

```csharp
var timeoutSec = 100; // 100 seconds timeout for this connection
connection.WithCancellationToken(cancellationToken).Execute(sql, parameters);
```

###  9.4. <a name='Preparedstatements'></a>Prepared statements

- Sets all database commands for this connection to prepared mode.

```csharp
connection.Prepared().Execute(sql, parameters);
```
