# **`Norm Micro-ORM`**
 
_The fastest database mapper for .NET Standard 2.1_
 
![build-test-publish](https://github.com/vb-consulting/Norm.net/workflows/build-test-publish/badge.svg)
 
## Features at a Glance
 
### `Modern and Fast`
 
- Maps to [basic, built-in types (int, string, etc)](https://github.com/vb-consulting/Norm.net/wiki/3.1.-Working-with-built-in-type-parameters), 
[`named tuples`](https://github.com/vb-consulting/Norm.net/wiki/3.3.-Working-with-tuple-type-parameters), 
[`records`](https://github.com/vb-consulting/Norm.net/wiki/3.2.-Working-with-class-and-record-type-parameters) or 
[`plain old classes`](https://github.com/vb-consulting/Norm.net/wiki/3.2.-Working-with-class-and-record-type-parameters) to map the results from your databases. In fact, it will map to pretty much anything.
 
- Uses async enumerables and powerful [`asynchronous database streaming`](https://github.com/vb-consulting/Norm.net/wiki/5.-Asynchronous-programming)
 
- Very fast mapping. [`performances indistinguishable from the raw data reader`](https://github.com/vb-consulting/Norm.net#performances).
 
### `Simple`
 
- You don't have to declare a new class every time:
 
```csharp
var result = connection.Read<(int i, string bar)>("select 1 as id, 'foo' as bar").First(); 
Console.WriteLine(result.i); // outputs 1 
Console.WriteLine(result.bar); // outputs "foo"
```
 
### `Trustworthy and Reliable`
 
- 350+ automated tests for `SqlServer`, `PostgreSQL`, `SQLite`, and `MySql`.
 
- [`Source links`](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink) are included in the package. That means that you can [Step Into] the source code when debugging to see exactly what it does.
 
### `All Databases`
 
- Implemented as set of extensions - for  [`System.Data.Common.DbConnection`](https://docs.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection?view=net-5.0) instances.
 
- Works with all databases based on `common DbConnection` class, and that is pretty much `all databases.`
 
- Native support for `ARRAY` database types for database providers that have `ARRAY` support (PostgreSQL).
 
### `Lightweight`
 
- Only three main extensions - [`Execute`](https://github.com/vb-consulting/Norm.net/wiki/2.1.-Execute(command,-params)-connection-extension), 
[`Read`](https://github.com/vb-consulting/Norm.net/wiki/2.2.-Read(command,-params)-connection-extensions) and [Multiple](https://github.com/vb-consulting/Norm.net/wiki/2.3.-Multiple(command,-params)-connection-extension). That's all it takes. There is no learning curve at all.
 
- No need for extra configuration or any special attributes.
 
- Small, and absolutely **`no dependencies whatsoever.`**
 
- All public methods were thoroughly documented in documentation comments that are `available to IntelliSense` and shipped with the package.
 
- User friendly [manual](https://github.com/vb-consulting/Norm.net/wiki) available.
 
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
var tuple = connection.Read(int id, string foo, string bar)>("select id, foo, bar from table").Single();
 
// Asynchronously stream values directly from database
await foreach(var (id, foo, bar) in connection.ReadAsync<int, string, string>("select id, foo, bar from table"))
{
    //...
}
 
// etc...
```
 
## Performances
 
- See detailed performance benchmarks compared to **Dapper** at [performance tests page](https://github.com/vb-consulting/Norm.net/blob/master/PERFOMANCE-TESTS.md).
 
- To run [beckmark console](https://github.com/vb-consulting/Norm.net/blob/master/BenchmarksConsole/Program.cs) manually, configure local testing for [PostgreSQL unit tests project](https://github.com/vb-consulting/Norm.net/tree/master/Tests/PostgreSqlUnitTests) first. See instructions for local testing bellow.
 
- Technical deep-dive article with explanation: [What Makes Norm Micro ORM for .NET Fast As Raw DataReader](https://dev.to/vbilopav/what-makes-norm-micro-orm-for-net-fast-as-raw-datareader-5eoa)
 
## Testing
 
350+ automated tests for `SqlServer`, `PostgreSQL`, `SQLite` and `MySql`.
 
![build-test-publish](https://github.com/vb-consulting/Norm.net/workflows/build-test-publish/badge.svg)
 
### Local testing
 
- Under each test project there is a `testsettings.json`. 
- Copy this file and rename it to `testsettings.local.json`. It will be ignored by git.
- Set the key `Default` to the value of your actual, local connection string.
- The key `TestDatabase` contains the name of the test database, which is created and dropped on each testing session, so be careful about that.
- Run `dotnet test`
 
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
