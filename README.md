# **`Norm Micro-ORM`**
 
_High performance micro-ORM database mapper and modernized Dapper replacement for .NET Standard 2.1 and higher_
 
![build-test-publish](https://github.com/vb-consulting/Norm.net/workflows/build-test-publish/badge.svg)
![License](https://img.shields.io/badge/license-MIT-green)
![GitHub Stars](https://img.shields.io/github/stars/vb-consulting/Norm.net?style=social)
![GitHub Forks](https://img.shields.io/github/forks/vb-consulting/Norm.net?style=social)

[Website under construction](https://vb-consulting.github.io/norm.net/)
 
## Features at a Glance
 
## **High-performance mapping**

- Outstanding perfomances. [See benchmarsks here](https://github.com/vb-consulting/Norm.net/blob/master/performance-tests.md)
- Build an iterator over database reader to avoid unnecessary iterations.
- Use asynchronous streaming directly from the database.

## **Powerful and extendible mapping system**

- Map up to 12 instances from the same command.
- Map to **simple values**, **tuples**, **named tuples**, **anonymous types**, whatever you need. Not every query deserves a class.
- Map to arrays and other exotic types available on databases such as PostgreSQL.
- Implement your custom mapping logic to handle custom types such as geometry types from PostGIS.

## Some examples:

###  At least three things that Norm data access for .NET can do - but Dapper can't: 
![](https://github.com/vb-consulting/Norm.net/blob/master/norm-mapping.jpg)

### Advanced logging and analytics:

You can configure Norm to automatically add a comment header to all your commands that will contain stuff like:

1) Caller info containing calling method name and source code file name with exact line number.
2) Full parameter list containing parameter name, type, and value.

Why we would ever want to do this?

This is very useful debugging info that:

1) Will be visible in your console if we add a logging callback (see example). You can actually use Ctrl+Click in your Visual Studio Code to navigate to that source line instantly.
2) Will be visible in your database monitoring tools like Activity Monitor for SQL Server or pg_stat_activity on PostgreSQL, since it is just a command comment header.

![](https://github.com/vb-consulting/Norm.net/blob/master/norm-logging.jpg)

### How to build a nested objects tree from a multiple tables join query - Norm vs Dapper comparison:

The goal is to map query results to an object tree, where each Shop object from the list has a collection of Account objects and each Account object has a reference to a Shop object to whom it belongs, and so on.

Norm works differently from Dapper.

Norm builds an iterator over the data reader which means that the read operation will not start until the iteration is triggered (ToList in this example).

That means that we can build a Linq expression tree before we start with the iteration and it will still iterate and read data only once.

On another hand, Dapper accepts the lambda function as a parameter that allows you to build the object tree using lookups.

I think that the Norm version is cleaner, more elegant, more flexible, and faster.


![](https://github.com/vb-consulting/Norm.net/blob/master/nested.png)

## Usage
 
### Get it on Nuget
 
```txt
> dotnet add package Norm.net
```
 
### Use in project
 
```csharp
using Norm;
using System.Linq;
 
// Start using database connection extensions:
 
// Map results to record
var records = connection.Read<MyRecord>("select id, foo, bar from table");
 
// Map results to class
var classes = connection.Read<MyClass>("select id, foo, bar from table");
 
// Map single values to a tuple and deconstruct to three variables
var (id, foo, bar) = connection.Read<int, string, string>("select id, foo, bar from table").Single();
 
// Map to a named tuple (id, foo, bar):
var tuple = connection.Read<(int id, string foo, string bar)>("select id, foo, bar from table").Single();
 
// Asynchronously stream values directly from database
await foreach(var (id, foo, bar) in connection.ReadAsync<int, string, string>("select id, foo, bar from table"))
{
    //...
}

//anonymous type instance
var instance = connection.Read(new { id = default(int), foo = default(string), bar = default(string) }, "select id, foo, bar from table").Single();

// Asynchronously stream to anonymous types directly from database
await foreach(var i in connection.ReadAsync(new 
{ 
    id = default(int), 
    foo = default(string), 
    bar = default(string) 
}, "select id, foo, bar from table"))
{
    //...
}
 
// etc...
```
 
## Currently supported platforms
 
- .NET Standard 2.1
 
## Support
 
This is open-source software developed and maintained freely without any compensation whatsoever.
 
## Licence
 
Copyright (c) Vedran Bilopavlović - VB Consulting and VB Software
This source code is licensed under the [MIT license](https://github.com/vbilopav/NoOrm.Net/blob/master/LICENSE).
