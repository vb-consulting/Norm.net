# **`Norm Micro-ORM`**

_The fastest database mapper for .NET Standard 2.1_

## Features at a Glance

- Modern: maps SQL results to `tuples`, `named tuples` `plain old classes` or `records`.
- Fast: mapping ` performances indistinguishable from the raw data reader`.
- Powerful: generates async enumerable to enable `asynchronous database streaming`.
- Simple: Implemented strictly as set of extensions - for `System.Data.Common.DbConnection` instances.
- Only four extensions  (plus parameters overloads and async versions): `Execute`, `Query`, `Read`, and `Single`. That's all it needs. There is no learning curve at all.
- Works with all databases based on `common DbConnection` class, and that is pretty much `all databases.`
- Works also with `array` types, where arrays are supported (PostgreSQL for example).
- Thoroughly tested, 250+ automated tests for `SqlServer`, `PostgreSQL`, `SQLite`, and `MySql`.
- No need for extra configuration or special attributes.
- Small, and absolutely `no dependencies whatsoever.`
- All public methods were thoroughly documented in documentation comments that are `available to IntelliSense` and shipped with the package 
- Package also includes `source links` for convenient debugging.

See [manual](https://github.com/vb-consulting/Norm.net/wiki) for more details

## Usage

### Get it on Nuget

```txt
> dotnet add package Norm.net
```

### Use in project

```csharp
using Norm;

// Start using database connection extensions. Some examples:

// Map results to record MyRecord
var records = connection.Query<MyRecord>("select id, foo, bar from my_table");

// Map results to class MyClass
var records = connection.Query<MyClass>("select id, foo, bar from my_table");

// Map single values to tuple and deconstruct to three variables
var (id, foo, bar) = connection.Single<int, string, string>("select id, foo, bar from my_table");

// Build enumerable of named tuples:
IEnumerable<(int id, string foo, string bar)> results = connection.Read<int, string, string>("select id, foo, bar from my_table");

// Asynchronously stream values directly from database
await foreach(var (id, foo, bar) in connection.ReadAsync<int, string, string>("select id, foo, bar from my_table"))
{
    //...
}

// etc...
```

## Performances

- See detailed perfomance benchmarks compared to **Dapper** at [performance tests page](https://github.com/vb-consulting/Norm.net/blob/master/PERFOMANCE-TESTS.md).

- To run [beckmark console](https://github.com/vb-consulting/Norm.net/blob/master/BenchmarksConsole/Program.cs) manually, configure local testing for [PostgreSQL unit tests project](https://github.com/vb-consulting/Norm.net/tree/master/Tests/PostgreSqlUnitTests) first. See instructions for local testing bellow.

- Techical deep-dive article with explanation: [What Makes Norm Micro ORM for .NET Fast As Raw DataReader](https://dev.to/vbilopav/what-makes-norm-micro-orm-for-net-fast-as-raw-datareader-5eoa)

## Testing

250 automated tests for SqlServer, PostgreSQL, SQLite and MySql

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
