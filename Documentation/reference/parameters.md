---
title: Parameters
order: 3
nextUrl: /norm.net/docs/reference/read/
nextTitle: Read MAppings
prevUrl: /norm.net/docs/reference/options/
prevTitle: Options
---

## Working With Parameters

- There are three main ways to set command parameters with Norm:

    1) > [By using the `WithParameters` extension method.](#1-withparameters-extension-method)
   
    2) > [By supplying an additional command parameter](#2-additional-command-parameter)

    3) > [By using string interpolation](#3-string-interpolation)

---

### 1) WithParameters Extension Method

- `WithParameters` extension sets parameters for the next command and it has the following signature:

```csharp
// Extension
public static Norm WithParameters(this DbConnection connection, params object[] parameters);
// Norm Instance Method
public Norm WithParameters(params object[] parameters);
```

- This method can receive one or more arguments of the `object` type.
  
 - The Parameter value can be either:
  
   - Simple type (integers, strings, dates, etc.).

   - Object instances.

   - Two value tuples (value and database type).

   - [`DbParameter`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbparameter) instance.

- Depending on the parameter type, parameters can be set in different ways: positional, named, or mixed.

---

#### Simple Values as Positional Parameters

- Using **simple values** - we can set the positional parameters.

- Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- These parameters are set **by position. **
  
- The name of parameters in the query `select @s, @i, @b, @d, @null` is not, actually, important at all. 

- The first value `"str"` is set to the first parameter `@s`, the second value to the second parameter `@i` and so on. Names of these parameters can be anything.

---

#### PostgreSQL Positional Parameters

- Norm also supports PostgreSQL positional parameters where each parameter in the query is defined with a `$` character and position index (`$1`, `$2`, `$2`, etc.).

- Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
    .Single();
```

---

#### Mixed PostgreSQL Positional Parameters and Simple Values

- Those two parameter styles can even be mixed in a query. Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, new DateTime(1977, 5, 19), null)
    .Read<string, int, bool, DateTime, string>("select $s, @i, $3, $4, $5")
    .Single();
```

---

#### Database Types with Positional Parameters

- Sometimes, we want to set a specific database type to a positional parameter. 

- In those cases, we can use the two values tuple, where the first value is the parameter value and the second value is the specific database type.

- Database type value of system enum [`System.Data.DbType`](https://learn.microsoft.com/en-us/dotnet/api/system.data.dbtype). Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(
        ("str", DbType.AnsiString),
        (999, DbType.Int32),
        (true, DbType.Boolean),
        (new DateTime(1977, 5, 19), DbType.Date),
        (null, DbType.AnsiString))
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```
---

#### Mixing Simple Values With Database Types

- You can also mix simple values and tuple values with a specific database type. Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", 999, true, (new DateTime(1977, 5, 19), DbType.Date), (null, DbType.AnsiString))
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

---

#### Provider-specific Database Types

- You can also use provider-specific database type enums. Example for PostgreSQL types:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(
        ("str", NpgsqlDbType.Text),
        (999, NpgsqlDbType.Bigint),
        (true, NpgsqlDbType.Boolean),
        (new DateTime(1977, 5, 19), NpgsqlDbType.Date),
        ((string)null, NpgsqlDbType.Text))
    .Read<string, int, bool, DateTime, string>("select $1, $2, $3, $4, $5")
    .Single();
```

---

#### Using Anonymous Object Instances

- The parameter value can also be an object instance. 

- In that case, each public property or public field will be a named parameter with the same name and the same value. Example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new
    {
        d = new DateTime(1977, 5, 19),
        b = true,
        i = 999,
        s = "str",
        @null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- This example uses an **anonymous object instance** to create the named parameters `d`, `b`, `i`, `s` and `˙null` with associated values.

- In this example, parameters appear in different order, because they are mapped by name, not by position.

- Note that `@null` starts with the `@` prefix because `null` is a C# keyword and the `@` prefix is ignored.

---

#### Using Object Instances

- Besides anonymous objects, normal instances can also be used as well:

```csharp
class TestClass
{
    public string S { get; set; }
    public int I { get; set; }
    public bool B { get; set; }
    public DateTime D { get; set; }
    public string Null { get; set; }
}

var (s, i, b, d, @null) = connection
    .WithParameters(new TestClass
    {
        D = new DateTime(1977, 5, 19),
        B = true,
        I = 999,
        S = "str",
        Null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- Note that parameter names are NOT case-sensitive. 

---

#### Specifying Database Type in Object Instance

- Also, you can set a specific database type, either generic `DbType` or provider-specific database type - by using tuples:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new
    {
        d = (new DateTime(1977, 5, 19), DbType.Date), // set parameter type to generic DbType.Date
        b = (true, NpgsqlDbType.Boolean), // set parameter type to PostgreSQL specific NpgsqlDbType.Boolean
        i = 999,
        s = "str",
        @null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

---

#### Mixing Positional Simple Values and Multiple Object Instances

- Positional and instance-named parameters can be mixed. Also, you can have multiple instance parameters:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters("str", // "str" parameter is mapped by position in first place
    new // first named instance
    {
        d = new DateTime(1977, 5, 19),
        b = true,
    },
    new // second named instance
    {
        i = 999,
        @null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```
---

#### Using DbParameter Instances

- For greater parameter control, a specific [`DbParameter`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbparameter) instance can also be used.

- SQL Server example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(
        new SqlParameter("s", "str"),
        new SqlParameter("i", 999),
        new SqlParameter("b", SqlDbType.Bit) { Value = true },
        new SqlParameter("d", new DateTime(1977, 5, 19)),
        new SqlParameter("null", SqlDbType.NText) { Value = null })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- PostgreSql example:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(
        new NpgsqlParameter("s", "str"),
        new NpgsqlParameter("i", 999),
        new NpgsqlParameter("b", NpgsqlDbType.Boolean) { Value = true },
        new NpgsqlParameter("d", new DateTime(1977, 5, 19)),
        new NpgsqlParameter("null", NpgsqlDbType.Text) { Value = null })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

---

#### Using DbParameter Instances as Object Properties

- You can also set the `DbParameter` instance to an instance field or a property:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters(new
    {
        d = new NpgsqlParameter("d", new DateTime(1977, 5, 19)),
        b = true,
        i = 999,
        s = "str",
        @null = (string)null
    })
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null")
    .Single();
```

- In that case, the name of the instance property is discarded, and the actual parameter name from the `DbParameter` instance is valid. That means that the first date property could be named differently, for example: `_ = new NpgsqlParameter("d", new DateTime(1977, 5, 19)),`.

---

#### DbParameter Instances as Output Parameters

- Using `DbParameter` instances is helpful to have and use output parameters. PostgreSQL example:

```csharp
var p = new NpgsqlParameter("test_param", "I am output value") { Direction = ParameterDirection.InputOutput };
connection
    .Execute(@"
        create function test_inout_param_func_1(inout test_param text) returns text as
        $$
        begin
            test_param := test_param || ' returned from function';
        end
        $$
        language plpgsql")
    .AsProcedure()
    .WithParameters(p)
    .Execute("test_inout_param_func_1");

Assert.Equal("I am output value returned from function", p.Value);
```

- Note: you can combine any style of parameters (positional with simple values, value-type tuples, object instances, `DbParameter` instances) in any combination.

---

### 2) Additional Command Parameter

- The parameter value as the object instance can also be set as an additional parameter in methods that will execute a database command:
  - `Execute`
  - `ExecuteAsync`
  - `Read`
  - `ReadAsync`
  - `Multiple`
  - `MultipleAsync`

- Examples are below.

#### Using Anonymous Object Instances

```csharp
var (s, i, b, d, @null) = connection
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null", new
    {
        d = new DateTime(1977, 5, 19),
        b = true,
        i = 999,
        s = "str",
        @null = (string)null
    })
    .Single();
```

- This example uses an **anonymous object instance** to create the named parameters `d`, `b`, `i`, `s` and `˙null` with associated values.

- In this example, parameters appear in different order, because they are mapped by name, not by position.

- Note that `@null` starts with the `@` prefix because `null` is a C# keyword and the `@` prefix is ignored.

---

#### Using Object Instances

```csharp
class TestClass
{
    public string S { get; set; }
    public int I { get; set; }
    public bool B { get; set; }
    public DateTime D { get; set; }
    public string Null { get; set; }
}

var (s, i, b, d, @null) = connection
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null", new TestClass
    {
        D = new DateTime(1977, 5, 19),
        B = true,
        I = 999,
        S = "str",
        Null = (string)null
    })
    .Single();
```

- Note that parameter names are NOT case-sensitive. 

---

#### Specifying Database Type in Object Instance

- Also, you can set a specific database type, either generic `DbType` or provider-specific database type - by using tuples:

```csharp
var (s, i, b, d, @null) = connection
    .WithParameters()
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null", new
    {
        d = (new DateTime(1977, 5, 19), DbType.Date), // set parameter type to generic DbType.Date
        b = (true, NpgsqlDbType.Boolean), // set parameter type to PostgreSQL specific NpgsqlDbType.Boolean
        i = 999,
        s = "str",
        @null = (string)null
    })
    .Single();
```

---

#### Using DbParameter Instances as Object Properties

- You can also set the `DbParameter` instance to an instance field or a property:

```csharp
var (s, i, b, d, @null) = connection
    .Read<string, int, bool, DateTime, string>("select @s, @i, @b, @d, @null", new
    {
        d = new NpgsqlParameter("d", new DateTime(1977, 5, 19)),
        b = true,
        i = 999,
        s = "str",
        @null = (string)null
    })
    .Single();
```

- In that case, the name of the instance property is discarded, and the actual parameter name from the `DbParameter` instance is valid. That means that the first date property could be named differently, for example: `_ = new NpgsqlParameter("d", new DateTime(1977, 5, 19)),`.

---

#### DbParameter Instances as Output Parameters

- Using `DbParameter` instances is helpful to have and use output parameters. PostgreSQL example:

```csharp
var p = new NpgsqlParameter("test_param", "I am output value") { Direction = ParameterDirection.InputOutput };
connection
    .Execute(@"
        create function test_inout_param_func_1(inout test_param text) returns text as
        $$
        begin
            test_param := test_param || ' returned from function';
        end
        $$
        language plpgsql")
    .AsProcedure()
    .Execute("test_inout_param_func_1", p);

Assert.Equal("I am output value returned from function", p.Value);
```

- Note: you can combine any style of parameters (positional with simple values, value-type tuples, object instances, `DbParameter` instances) in any combination.

#### Using Single Value Parameter

- Instead of supplying the instance, if the command has only one parameter, that also can be used:

```csharp
var (id, name) = connection
    .Read<int, string>("select film_id, title from where film_id = @id", 999)
    .Single();
```
```csharp
var exists = connection.Read("select 1 from film where film_id = @id", 999).Any();
```

- In these examples, the single parameter named `@id` is irrelevant, it can be any valid name.

---

### 3) String Interpolation

- Besides those two approaches, command parameters can supplied through the **string interpolation** mechanism by using special `Format` versions the command methods:
  
  - `ExecuteFormat`
  - `ExecuteFormatAsync`
  - `ReadFormat`
  - `ReadFormatAsync`
  - `MultipleFormat`
  - `MultipleFormatAsync`

- Example:

```csharp
var user = connection
    .ReadFormat<User>(@$"
        select u.* 
        from users u, logs l 
        where u.usrid = {userId} and u.usrid = l.usrid and l.date = {date}")
    .Single();
```

- In this example, variables `userId` and `date` are used as normal **database command parameters**.

- There is also an option that allows for escaping format parameters and using normal string interpolation for certain parameters in the format string:

```csharp
var table = "logs";
var user = connection
    .ReadFormat<User>(@$"
        select u.* 
        from users u, {table:raw} l 
        where u.usrid = {userId} and u.usrid = l.usrid and l.date = {date}")
    .Single();
```

- In this example, the variable `table` is used in normal string interpolation (because we used the `raw` modifier), and variables `userId` and `date` are still used as database command parameters.

- The value of the `raw` modifier can be set in the [options](/norm.net/docs/reference/options/#rawinterpolationparameterescape).

```csharp
NormOptions.Configure(options =>
{
    options.RawInterpolationParameterEscape = "raw";
});
```
