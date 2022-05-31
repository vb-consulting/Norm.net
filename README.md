# **`Norm Micro-ORM`**
 
_High performance micro-ORM database mapper and modernized Dapper replacement for .NET Standard 2.1 and higher_
 
![build-test-publish](https://github.com/vb-consulting/Norm.net/workflows/build-test-publish/badge.svg)
 
## Features at a Glance
 
## **High-performance mapping**

- Outstanding perfomances. [See benchmarsks here](https://github.com/vb-consulting/Norm.net/blob/master/PERFOMANCE-TESTS.md#results-round-1)
- Build an iterator over database reader to avoid unnecessary iterations.
- Use asynchronous streaming directly from the database.

## **Powerful and extendible mapping system**

- Map up to 12 instances from the same command.
- Map to **simple values**, **tuples**, **named tuples**, **anonymous types**, whatever you need. Not every query deserves a class.
- Map to arrays and other exotic types available on databases such as PostgreSQL.
- Implement your custom mapping logic to handle custom types such as geometry types from PostGIS.

 
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
 
If you find it useful please consider rewarding me on my effort by [buying me a beer](https://www.paypal.me/vbsoftware/5)🍻 or [buying me a pizza](https://www.paypal.me/vbsoftware/10)🍕
 
Or if you prefer bitcoin:
bitcoincash:qp93skpzyxtvw3l3lqqy7egwv8zrszn3wcfygeg0mv
 
## Licence
 
Copyright (c) Vedran Bilopavlović - VB Consulting and VB Software 2020
This source code is licensed under the [MIT license](https://github.com/vbilopav/NoOrm.Net/blob/master/LICENSE).
