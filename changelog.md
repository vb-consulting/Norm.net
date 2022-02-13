# Changelog

## [3.4.0](https://github.com/vb-consulting/Norm.net/tree/3.4.0) (2022-02-13)

[Full Changelog](https://github.com/vb-consulting/Norm.net/compare/3.3.13...3.4.0)

## **New feature - reader callbacks**

All read extension methods on your connection object (`Read` and `ReadAsync`) now have one additional overload that allows you to pass in the **lambda function with direct low-level access to the database reader.**

Lambda function with direct low-level access to the database reader has following signature:

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

Same overloads also exists for all 12 generic versions of `Read` and `ReadAsync` extensions.

This lambda function receives a single parameter, a tuple with three values: `(string Name, int Ordinal, DbDataReader Reader)`, where:

- `string Name` is the name of the database field currently being mapped to a value.
- `int Ordinal` is the orinal position number, starting from zero, of the database field currently being mapped to a value.
- `DbDataReader Reader` is the low level access to the database reader being used to read the current row.

The expected result of this reader lambda function is an object of any type that represents a **new value that will be mapped** to the results with the following rules:

- Return any value to that you wish to map to result for that field.
- Return `null` to use the default, normal mapping.
- Return `DBNull.Value` to set `null` value.

This allows











# Version history and release notes

## 3.4.0

### Array mapping improvements

- **Fixed bug with arrays mapping to named tuple values.**

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

Important:

> Mapping nullable arrays to named tuples or to instance properties will not work.


### Mapping enums in a class, ints and strings

- Class instance: map enums from 
  - From ints 
  - From strings, 
  - From nullable ints 
  - From nullable strings
  - From string arrays
  - From int arrays
  - NOT from nullable arrays (of string or ints)

- Simple values: 
  - From ints 
  - From strings, 
  - From nullable ints 
  - From nullable strings
  - NOT from arrays (nullable or otherwise of any type)

- Named Tuples
  - None

### Removed Global SqlMapper for Custom Mappings

### Removed DbType overloads

### Removed NormMultipleMappingsException

### Internal improvements, new benchmark tests


